// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DifferenceFinders.cs" company="NFluent">
//   Copyright 2021 Cyrille DUPUYDAUBY
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Extensions;
    using Kernel;

    internal static class DifferenceFinders
    {

        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y) => ReferenceEquals(x, y);

            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }

        private static readonly IEqualityComparer<object> ReferenceComparer = new ReferenceEqualityComparer();

        internal static DifferenceDetails ValueDifference<TA, TE>(TA firstItem, string firstName, TE otherItem)
        {
            return ValueDifference(firstItem, firstName, otherItem, 0, 0, new Dictionary<object, object>(1, ReferenceComparer));
        }

        /// <summary>
        ///     Check Equality between actual and expected and provides details regarding differences, if any.
        /// </summary>
        /// <remarks>
        ///     Is recursive.
        ///     Algorithm focuses on value comparison, to better match expectations. Here is a summary of the logic:
        ///     1. deals with expected = null case
        ///     2. tries Equals, if success, values are considered equals
        ///     3. if there is recursion (self referencing object), values are assumed as different
        ///     4. if both values are numerical, compare them after conversion if needed.
        ///     5. if expected is an anonymous type, use a property based comparison
        ///     6. if both are enumerations, perform enumeration comparison
        ///     7. report values as different.
        /// </remarks>
        /// <typeparam name="TA">type of the actual value</typeparam>
        /// <typeparam name="TE">type of the expected value</typeparam>
        /// <param name="actual">actual value</param>
        /// <param name="firstName">name/label to use for messages</param>
        /// <param name="expected">expected value</param>
        /// <param name="refIndex">reference index (for collections)</param>
        /// <param name="expectedIndex">index in the expected collections</param>
        /// <param name="firstSeen">track recursion</param>
        /// <returns></returns>
        private static DifferenceDetails ValueDifference<TA, TE>(TA actual, 
            string firstName, 
            TE expected,
            long refIndex, 
            long expectedIndex, 
            IDictionary<object, object> firstSeen)
        {
            // handle expected null case first
            if (expected == null)
            {
                return actual == null
                    ? null
                    : DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, null, refIndex, expectedIndex);
            }

            // if both equals from a BCL perspective, we are done.
            if (EqualityHelper.CustomEquals(actual, expected))
            {
                return null;
            }

            // handle actual is null
            if (actual == null)
            {
                return DifferenceDetails.DoesNotHaveExpectedValue(firstName, null, expected, refIndex, expectedIndex);
            }

            // do not recurse
            if (firstSeen.ContainsKey(actual))
            {
                return ReferenceEquals(firstSeen[actual], expected) ? null : DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex, expectedIndex);
            }

            firstSeen = new Dictionary<object, object>(firstSeen, ReferenceComparer) {[actual] = expected };


            // deals with numerical
            var type = expected.GetType();
            var commonType = actual.GetType().FindCommonNumericalType(type);
            // we silently convert numerical values
            if (commonType != null)
            {
                return NumericalValueDifference(actual, firstName, expected, refIndex, expectedIndex, commonType);
            }

            if (type.TypeIsAnonymous())
            {
                return AnonymousTypeDifference(actual, firstName, expected, type);
            }

            // handle enumeration
            if (actual.IsAnEnumeration(false) && expected.IsAnEnumeration(false))
            {
                return ValueDifferenceEnumerable(actual as IEnumerable, firstName, expected as IEnumerable, refIndex, expectedIndex, firstSeen);
            }

            return DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex , expectedIndex);
        }

        private static DifferenceDetails AnonymousTypeDifference<TA, TE>(TA actual, string firstname, TE expected, Type type)
        {
            var criteria = new ClassMemberCriteria(BindingFlags.Instance);
            criteria.CaptureProperties();
            criteria.CaptureFields();
            // use field based comparison
            var wrapper = ReflectionWrapper.BuildFromInstance(type, expected, criteria);
            var actualWrapped = ReflectionWrapper.BuildFromInstance(actual.GetType(), actual, criteria);
            var differences = actualWrapped.MemberMatches(wrapper).Where(match => !match.DoValuesMatches).Select(DifferenceDetails.FromMatch).ToList();

            return DifferenceDetails.DoesNotHaveExpectedDetails(firstname, actual, expected, 0, 0, differences);
        }

        private static DifferenceDetails NumericalValueDifference<TA, TE>(TA actual,         
            string firstName, 
            TE expected,
            long refIndex, 
            long expectedIndex, 
            Type commonType)
        {
            var convertedActual = Convert.ChangeType(actual, commonType);
            var convertedExpected = Convert.ChangeType(expected, commonType);
            return convertedExpected.Equals(convertedActual) ? null : DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex, expectedIndex);
        }

        private static DifferenceDetails ValueDifferenceDictionary(IReadOnlyDictionary<object, object> sutDictionary,
            string sutName,
            IReadOnlyDictionary<object, object> expectedDictionary,
            long refIndex, 
            long expectedIndex, 
            IDictionary<object, object> firstItemsSeen)
        {
            var index = 0L;
            var valueDifferences = new List<DifferenceDetails>();
            var equivalent = true;
            var unexpectedKeys = sutDictionary.Keys.Where(k => !expectedDictionary.ContainsKey(k)).ToList();
            foreach (var keyValuePair in expectedDictionary)
            {
                if (!sutDictionary.ContainsKey(keyValuePair.Key))
                {
                    if (unexpectedKeys.Count > 0)
                    {
                        var unexpectedKey = unexpectedKeys[0];
                        valueDifferences.Add(DifferenceDetails.WasFoundInsteadOf(
                            $"{sutName}",
                            new DictionaryEntry(unexpectedKey, sutDictionary[unexpectedKey]),
                            new DictionaryEntry(keyValuePair.Key, keyValuePair.Value),
                            index));
                        unexpectedKeys.RemoveAt(0);
                    }
                    else
                    {
                        valueDifferences.Add(DifferenceDetails.WasNotFound(
                            $"{sutName}[{keyValuePair.Key.ToStringProperlyFormatted()}]",
                            new DictionaryEntry(keyValuePair.Key, keyValuePair.Value),
                            index));
                    }
                    equivalent = false;
                }
                else
                {
                    var itemDiffs = ValueDifference(sutDictionary[keyValuePair.Key],
                        $"{sutName}[{keyValuePair.Key.ToStringProperlyFormatted()}]",
                        keyValuePair.Value,
                        index, 
                        index,
                        firstItemsSeen);
                    if (itemDiffs != null)
                    {
                        if (!itemDiffs.IsEquivalent())
                        {
                            equivalent = false;
                        }
                        valueDifferences.Add(itemDiffs);
                    }
                }

                index++;
            }

            foreach (var unexpectedKey in unexpectedKeys)
            {
                equivalent = false;
                valueDifferences.Add(DifferenceDetails.WasNotExpected(
                    $"{sutName}[{unexpectedKey.ToStringProperlyFormatted()}]",
                    sutDictionary[unexpectedKey], index));
                
            }
            /*
            var stillExpectedKeys = true;
            var stillActualKeys = true;
            using var actualKeyIterator = sutDictionary.Keys.GetEnumerator();
            using var expectedKeyIterator = expectedDictionary.Keys.GetEnumerator();
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

                    equivalent = false;
                    // the sut has extra key(s)
                    valueDifferences.Add(DifferenceDetails.WasNotExpected(
                        $"{sutName}[{actualKeyIterator.Current.ToStringProperlyFormatted()}]",
                        sutDictionary[actualKeyIterator.Current], index));
                }
                else if (!stillActualKeys)
                {
                    equivalent = false;
                    // key not found
                    valueDifferences.Add(DifferenceDetails.WasNotFound(
                        $"{sutName}[{expectedKeyIterator.Current.ToStringProperlyFormatted()}]",
                        // ReSharper disable once AssignNullToNotNullAttribute
                        new DictionaryEntry(expectedKeyIterator.Current,
                            expectedDictionary[expectedKeyIterator.Current]),
                        index));
                }
                else
                {
                    var actualKey = actualKeyIterator.Current;
                    var actualKeyName = $"{sutName} key[{index}]";
                    var itemDiffs = ValueDifference(actualKey,
                        actualKeyName,
                        expectedKeyIterator.Current,
                        index, 
                        index,
                        firstItemsSeen);

                    if (expectedDictionary.TryGetValue(actualKey!, out var keyEntry))
                    {
                        var altIndex = expectedDictionary.Keys.TakeWhile(key => key != actualKey).Count();
                        itemDiffs = ValueDifference(sutDictionary[actualKey],
                            $"{sutName}[{actualKey.ToStringProperlyFormatted()}]",
                            keyEntry,
                            index, 
                            altIndex,
                            firstItemsSeen);
                    }
                    else
                    {
                        equivalent = false;
                        valueDifferences.Add(DifferenceDetails.WasNotExpected(
                            $"{sutName}'s key {actualKey.ToStringProperlyFormatted()}", 
                            sutDictionary[actualKey],
                            index));
                    }

                    if (itemDiffs != null)
                    {
                        if (!itemDiffs.IsEquivalent())
                        {
                            equivalent = false;
                        }
                        valueDifferences.Add(itemDiffs);
                    }
                }

                index++;
            }
            */
            if (valueDifferences.Count == 0)
            {
                return null;
            }

            return equivalent ? 
                DifferenceDetails.DoesNotHaveExpectedDetailsButIsEquivalent(sutName, sutDictionary, expectedDictionary, refIndex, expectedIndex, valueDifferences) 
                : DifferenceDetails.DoesNotHaveExpectedDetails(sutName, sutDictionary, expectedDictionary, refIndex, expectedIndex, valueDifferences);
        }

        private static DifferenceDetails ValueDifferenceArray(Array firstArray, 
            string firstName, 
            Array secondArray,
            long sutIndex,
            long expectedIndex,
            IDictionary<object, object> firstSeen)
        {
            if (firstArray.Rank != secondArray.Rank)
            {
                return DifferenceDetails.DoesNotHaveExpectedAttribute($"{firstName}.Rank",
                    firstArray.Rank, secondArray.Rank);
            }

            for (var i = 0; i < firstArray.Rank; i++)
            {
                if (firstArray.SizeOfDimension(i) == secondArray.SizeOfDimension(i))
                {
                    continue;
                }

                return DifferenceDetails.DoesNotHaveExpectedAttribute($"{firstName}.Dimension({i})",
                    firstArray.SizeOfDimension(i),
                    secondArray.SizeOfDimension(i));
            }

            return ScanEnumeration(firstArray, secondArray, firstName, index =>
            {
                var temp = index;
                var indices = new long[firstArray.Rank];
                for (var j = 0; j < firstArray.Rank; j++)
                {
                    var currentIndex = temp % firstArray.SizeOfDimension(j);
                    indices[firstArray.Rank - j - 1] = currentIndex;
                    temp /= firstArray.SizeOfDimension(j);
                }

                return $"{firstName}[{string.Join(",", indices.Select(x => x.ToString()).ToArray())}]";
            }, 
                sutIndex, 
                expectedIndex,
                firstSeen);
        }

        private static DifferenceDetails ValueDifferenceEnumerable(IEnumerable firstItem, 
            string firstName,
            IEnumerable otherItem,
            long sutIndex,
            long expectedIndex,
           IDictionary<object, object> firstSeen)
        {
            if (firstItem.GetType().IsArray && otherItem.GetType().IsArray)
            {
                return ValueDifferenceArray(firstItem as Array,                 
                    firstName, 
                    otherItem as Array,
                    sutIndex,
                    expectedIndex,
                    firstSeen);
            }

            var dictionary = DictionaryExtensions.WrapDictionary<object, object>(otherItem);
            if (dictionary != null)
            {
                var wrapDictionary = DictionaryExtensions.WrapDictionary<object, object>(firstItem);
                if (wrapDictionary != null)
                {
                    return ValueDifferenceDictionary(wrapDictionary, firstName, dictionary, sutIndex, expectedIndex, firstSeen);
                }
            }

            return ScanEnumeration(firstItem, otherItem, firstName, x => $"{firstName}[{x}]", sutIndex, expectedIndex, firstSeen);
        }

        private static DifferenceDetails ScanEnumeration(IEnumerable actualEnumerable, 
            IEnumerable expectedEnumerable, 
            string firstName,
            Func<long, string> namingCallback,
            long sutIndex,
            long expectedIndex,
            IDictionary<object, object> firstSeen)
        {
 
            var index = 0L;
            var expected = new List<KeyValuePair<object, long>>();
            var unexpected = new List<KeyValuePair<object, long>>();
            var aggregatedDifferences = new Dictionary<long, DifferenceDetails>();
            var valueDifferences = new List<DifferenceDetails>();
            var scanner = expectedEnumerable.GetEnumerator();
            var isEquivalent = true;

            foreach (var actualItem in actualEnumerable)
            {
                var firstItemName = namingCallback(index);
                if (!scanner.MoveNext())
                {
                    valueDifferences.Add(DifferenceDetails.WasNotExpected(firstItemName, actualItem, index));
                    unexpected.Add(new KeyValuePair<object, long>(actualItem, index));
                    continue;
                }

                var aggregatedDifference = ValueDifference(actualItem, firstItemName, scanner.Current, index, index, firstSeen);

                if (aggregatedDifference != null)
                {
                    aggregatedDifferences.Add(index, aggregatedDifference);
                    if (!aggregatedDifference.IsEquivalent())
                    {
                        // try to see it was at a different position
                        var indexInCache = expected.FindIndex(pair => FluentEquivalent(pair.Key, actualItem));
                        if (indexInCache >= 0)
                        {
                            var expectedEntryIndex = expected[indexInCache].Value;
                            // we found the value at another index
                            valueDifferences.Add(DifferenceDetails.WasFoundElseWhere(firstItemName, actualItem, expectedEntryIndex, index));
                            CleanUpEntry(aggregatedDifferences, expectedEntryIndex);
                            expected.RemoveAt(indexInCache);
                            aggregatedDifferences.Remove(index);
                        }
                        else
                        {
                            // store it in case this is a needed entry
                            unexpected.Add(new KeyValuePair<object, long>(actualItem, index));
                        }

                        // what about the expected value
                        indexInCache = unexpected.FindIndex(pair => FluentEquivalent(pair.Key, scanner.Current));
                        if (indexInCache >= 0)
                        {
                            var actualIndex = unexpected[indexInCache].Value;
                            valueDifferences.Add(DifferenceDetails.WasFoundElseWhere(namingCallback(actualIndex),
                               scanner.Current, index, actualIndex));
                            CleanUpEntry(aggregatedDifferences, index);

                            unexpected.RemoveAt(indexInCache);
                            aggregatedDifferences.Remove(actualIndex);
                            //aggregatedDifferences.Remove(index);
                        }
                        else
                        {
                            expected.Add(new KeyValuePair<object, long>(scanner.Current, index));
                        }
                    }
                }

                index++;
            }

            // several entries appear to be missing
            while (scanner.MoveNext())
            {
                valueDifferences.Add(DifferenceDetails.WasNotFound(namingCallback(index), scanner.Current, index));
                isEquivalent = false;

            }

            valueDifferences.AddRange(aggregatedDifferences.Values);
            
            for (var i = 0; i < Math.Min(unexpected.Count, expected.Count); i++)
            {
                valueDifferences.Add(DifferenceDetails.WasFoundInsteadOf(namingCallback(unexpected[i].Value),
                    unexpected[i].Key,
                    expected[i].Key,                     
                    unexpected[i].Value));
            }
            
            if (isEquivalent)
            {
                isEquivalent = expected.Count == 0 && unexpected.Count == 0;
            }

            if (valueDifferences.Count == 0)
            {
                return null;
            }

            valueDifferences = valueDifferences.OrderBy(d => d.ActualIndex).ToList();
            return isEquivalent ? 
                DifferenceDetails.DoesNotHaveExpectedDetailsButIsEquivalent(firstName, actualEnumerable, expectedEnumerable, sutIndex, expectedIndex, valueDifferences)
                : DifferenceDetails.DoesNotHaveExpectedDetails(firstName, actualEnumerable, expectedEnumerable, sutIndex, expectedIndex, valueDifferences);

            // removes any equivalence related error. Needed when we find an equivalent entry at a different index.
            static void CleanUpEntry(IDictionary<long, DifferenceDetails> differenceDetailsMap, long l)
            {
                if (!differenceDetailsMap.ContainsKey(l))
                {
                    return;
                }

                differenceDetailsMap[l] = differenceDetailsMap[l].WithoutEquivalenceErrors();
            }
        }

        private static bool FluentEquivalent<TS, TE>(TS instance, TE expected)
        {
            var scan = EqualityHelper.FluentEquals(instance, expected, Check.EqualMode);
            return scan == null || scan.IsEquivalent();
        }
    }
}