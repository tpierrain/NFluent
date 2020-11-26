namespace NFluent.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using Kernel;

    internal static class DifferenceFinders
    {
        internal static AggregatedDifference ValueDifference<TA, TE>(TA firstItem, string firstName, TE otherItem)
        {
            return ValueDifference(firstItem, firstName, otherItem, 0, new List<object>());
        }

        private static AggregatedDifference ValueDifference<TA, TE>(TA actual, string firstName, TE expected, int refIndex, ICollection<object> firstSeen)
        {
            var result = new AggregatedDifference();
            // handle null case first
            if (expected == null)
            {
                if (actual != null)
                {
                    result.Add(DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, null, refIndex));
                }

                return result;
            }

            // if both equals from a BCL perspective, we are done.
            if (EqualityHelper.CustomEquals(expected, actual))
            {
                return result;
            }

            // handle actual is null
            if (actual == null)
            {
                result.Add(DifferenceDetails.DoesNotHaveExpectedValue(firstName, null, expected, refIndex));
                return result;
            }

            // do not recurse
            if (firstSeen.Contains(actual))
            {
                result.Add(DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, 0));
                return result;
            }
            firstSeen = new List<object>(firstSeen) { actual };

            // deals with numerical
            var type = expected.GetType();
            var commonType = actual.GetType().FindCommonNumericalType(type);
            // we silently convert numerical value
            if (commonType != null)
            {
                return NumericalValueDifference(actual, firstName, expected, refIndex, commonType, result);
            }

            if (type.TypeIsAnonymous())
            {
                return AnonymousTypeDifference(actual, expected, type, result);
            }

            // handle enumeration
            if (actual.IsAnEnumeration(false) && expected.IsAnEnumeration(false))
            {
                return ValueDifferenceEnumerable(actual as IEnumerable, firstName, expected as IEnumerable, firstSeen);
            }

            result.Add(DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex));
            return result;
        }

        private static AggregatedDifference AnonymousTypeDifference<TA, TE>(TA actual, TE expected, Type type,
            AggregatedDifference result)
        {
            var criteria = new ClassMemberCriteria(BindingFlags.Instance);
            criteria.SetPublic();
            criteria.CaptureProperties();
            criteria.CaptureFields();
            // use field based comparison
            var wrapper = ReflectionWrapper.BuildFromInstance(type, expected, criteria);
            var actualWrapped = ReflectionWrapper.BuildFromInstance(actual.GetType(), actual, criteria);
            foreach (var match in actualWrapped.MemberMatches(wrapper).Where(match => !match.DoValuesMatches))
            {
                result.Add(DifferenceDetails.FromMatch(match));
            }

            return result;
        }

        private static AggregatedDifference NumericalValueDifference<TA, TE>(TA actual, string firstName, TE expected,
            int refIndex, Type commonType, AggregatedDifference result)
        {
            var convertedActual = Convert.ChangeType(actual, commonType);
            var convertedExpected = Convert.ChangeType(expected, commonType);
            if (convertedExpected.Equals(convertedActual))
            {
                return result;
            }

            result.Add(DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex));
            return result;
        }

        private static AggregatedDifference ValueDifferenceDictionary(IReadOnlyDictionary<object, object> sutDictionary,
            string sutName,
            IReadOnlyDictionary<object, object> expectedDictionary,
            ICollection<object> firstItemsSeen)
        {
            var valueDifferences = new AggregatedDifference { IsEquivalent = true };

            using var actualKeyIterator = sutDictionary.Keys.GetEnumerator();
            using var expectedKeyIterator = expectedDictionary.Keys.GetEnumerator();
            var stillExpectedKeys = true;
            var stillActualKeys = true;
            var index = 0;
            for (;;)
            {
                stillExpectedKeys = stillExpectedKeys && expectedKeyIterator.MoveNext();
                stillActualKeys = stillActualKeys && actualKeyIterator.MoveNext();
                if (!stillExpectedKeys)
                {
                    // no more expected keys
                    if (!stillActualKeys)
                    {
                        // we're done
                        break;
                    }
                    // the sut has extra key(s)
                    valueDifferences.Add(DifferenceDetails.WasNotExpected($"{sutName}[{actualKeyIterator.Current.ToStringProperlyFormatted()}]", sutDictionary[actualKeyIterator.Current], index));
                    valueDifferences.IsEquivalent = false;
                }
                else if (!stillActualKeys)
                {
                    // key not found
                    valueDifferences.IsEquivalent = false;
                    valueDifferences.Add(DifferenceDetails.WasNotFound($"{sutName}[{expectedKeyIterator.Current.ToStringProperlyFormatted()}]",
                        // ReSharper disable once AssignNullToNotNullAttribute
                        new DictionaryEntry(expectedKeyIterator.Current, expectedDictionary[expectedKeyIterator.Current]), 
                        0));
                }
                else
                {
                    var actualKey = actualKeyIterator.Current;
                    var actualKeyName = $"{sutName} key[{index}]";
                    var itemDiffs = ValueDifference(actualKey,
                        actualKeyName,
                        expectedKeyIterator.Current,
                        index,
                        firstItemsSeen);
                    if (!expectedDictionary.TryGetValue(actualKey, out _))
                    {
                        valueDifferences.Add(DifferenceDetails.WasNotExpected(
                            $"{sutName}'s key {actualKey.ToStringProperlyFormatted()}", sutDictionary[actualKey],
                            index));
                        valueDifferences.IsEquivalent = false;
                    }
                    else
                    {
                        itemDiffs = ValueDifference(sutDictionary[actualKey],
                        $"{sutName}[{actualKey.ToStringProperlyFormatted()}]",
                        expectedDictionary[actualKey],
                        index,
                        firstItemsSeen);
                        valueDifferences.IsEquivalent &= (!itemDiffs.IsDifferent || itemDiffs.IsEquivalent);
                    }
                    valueDifferences.Merge(itemDiffs);
                }

                index++;
            }
            return valueDifferences;
        }

        private static AggregatedDifference ValueDifferenceArray(Array firstArray, string firstName, Array secondArray, ICollection<object> firstSeen)
        {
            var valueDifferences = new AggregatedDifference();
            // TODO: consider providing more details when dimension(s) differs
            if (firstArray.Rank != secondArray.Rank)
            {
                valueDifferences.Add(DifferenceDetails.DoesNotHaveExpectedAttribute(firstName + ".Rank", firstArray.Rank, secondArray.Rank, 0));
                return valueDifferences;
            }

            for (var i = 0; i < firstArray.Rank; i++)
            {
                if (firstArray.SizeOfDimension(i) == secondArray.SizeOfDimension(i))
                {
                    continue;
                }

                valueDifferences.Add(DifferenceDetails.DoesNotHaveExpectedAttribute($"{firstName}.Dimension({i})",
                    firstArray.SizeOfDimension(i),
                    secondArray.SizeOfDimension(i),
                    i));
                return valueDifferences;
            }

            return ScanEnumeration(firstArray, secondArray, (index)=>
            {
                var temp = index;
                var indices = new int[firstArray.Rank];
                for (var j = 0; j < firstArray.Rank; j++)
                {
                    var currentIndex = temp % firstArray.SizeOfDimension(j);
                    indices[firstArray.Rank - j - 1] = currentIndex;
                    temp /= firstArray.SizeOfDimension(j);
                }

                return $"actual[{string.Join(",", indices.Select(x=> x.ToString()).ToArray())}]";
            }, firstSeen); 
        }

        private static AggregatedDifference ValueDifferenceEnumerable(IEnumerable firstItem, string firstName,
            IEnumerable otherItem,
            ICollection<object> firstSeen)
        {
            if (firstItem.GetType().IsArray && otherItem.GetType().IsArray)
            {
                return ValueDifferenceArray(firstItem as Array, firstName, otherItem as Array,
                    firstSeen);
            }

            var dictionary = DictionaryExtensions.WrapDictionary<object, object>(otherItem);
            if (dictionary != null)
            {
                var wrapDictionary = DictionaryExtensions.WrapDictionary<object, object>(firstItem);
                if (wrapDictionary != null)
                {
                    return ValueDifferenceDictionary(wrapDictionary, firstName, dictionary, firstSeen);
                }
            }

            return ScanEnumeration(firstItem, otherItem, (x) => $"{firstName}[{x}]", firstSeen);
        }

        private static AggregatedDifference ScanEnumeration(IEnumerable firstItem, IEnumerable otherItem, Func<int, string> namingCallback, ICollection<object> firstSeen)
        { 
            var index = 0;
            var mayBeEquivalent = true;
            var expected = new List<KeyValuePair<object, int>>();
            var unexpected = new List<KeyValuePair<object, int>>();
            var aggregatedDifferences = new Dictionary<int, AggregatedDifference>();
            var valueDifferences = new AggregatedDifference();
            var scanner = otherItem.GetEnumerator();

            foreach (var item in firstItem)
            {
                var firstItemName = namingCallback(index);
                if (!scanner.MoveNext())
                {
                    valueDifferences.Add(DifferenceDetails.WasNotExpected(firstItemName, item, index));
                    unexpected.Add(new KeyValuePair<object, int>(item, index));
                    break;
                }

                var aggregatedDifference = DifferenceFinders.ValueDifference(item, firstItemName, scanner.Current, index, firstSeen);
 
                if (aggregatedDifference.IsDifferent)
                {
                    aggregatedDifferences.Add(index, aggregatedDifference);
                    if (!aggregatedDifference.IsEquivalent)
                    {
                        // try to see it was at a different position
                        var indexOrigin = expected.FindIndex(pair => FluentEquivalent(pair.Key, item));
                        if (indexOrigin >= 0)
                        {
                            // we found the value at another index
                            valueDifferences.Add(DifferenceDetails.WasFoundElseWhere(firstItemName, item, index, expected[indexOrigin].Value));
                            expected.RemoveAt(indexOrigin);
                            aggregatedDifferences.Remove(indexOrigin);
                            aggregatedDifferences.Remove(index);
                        }
                        else
                        {
                            unexpected.Add(new KeyValuePair<object, int>(item, index));
                        }

                        // what about the expected value
                        var indexOther = unexpected.FindIndex(pair => FluentEquivalent(pair.Key, scanner.Current));
                        if (indexOther >= 0)
                        {
                            valueDifferences.Add(DifferenceDetails.WasFoundElseWhere(firstItemName, unexpected[indexOther].Key, unexpected[indexOther].Value, index));
                            aggregatedDifferences.Remove(unexpected[indexOther].Value);
                            unexpected.RemoveAt(indexOther);
                        }
                        else
                        {
                            expected.Add(new KeyValuePair<object, int>(scanner.Current, index));
                        }
                    }
                }

                index++;
            }

            if (scanner.MoveNext())
            {
                valueDifferences.Add(DifferenceDetails.WasNotFound(namingCallback(index), scanner.Current, index));
                mayBeEquivalent = false;
            }

            foreach (var differencesValue in aggregatedDifferences.Values)
            {
                valueDifferences.Merge(differencesValue);
            }

            for(var i = 0; i < Math.Min(unexpected.Count, expected.Count); i++)
            {
                //aggregatedDifferences.Remove(unexpected[i].Value);
                valueDifferences.Add(DifferenceDetails.WasFoundInsteadOf(namingCallback(unexpected[i].Value), unexpected[i].Key,
                    expected[i].Key));
            }

            if (mayBeEquivalent && valueDifferences.IsDifferent)
            {
                valueDifferences.IsEquivalent = expected.Count == 0 && unexpected.Count == 0;
            }

            return valueDifferences;
        }

        private static bool FluentEquivalent<TS, TE>(TS instance, TE expected)
        {
            var scan = EqualityHelper.FluentEquals(instance, expected, Check.EqualMode);
            return !scan.IsDifferent || scan.IsEquivalent;
        }
    }
}