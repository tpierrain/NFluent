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
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using Extensions;
    using Kernel;

    internal static class DifferenceFinders
    {
        [Flags]
        internal enum Option
        {
            Detailed=1,
            Fast = 2,
            Equivalence = 4
        }

        private class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y) => ReferenceEquals(x, y);

            public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
        }

        private static readonly IEqualityComparer<object> ReferenceComparer = new ReferenceEqualityComparer();

        internal static DifferenceDetails ValueDifference<TA, TE>(TA firstItem, 
            string firstName, 
            TE otherItem,
            Option options = Option.Detailed)
        {
            return ValueDifference(firstItem, 
                firstName, 
                otherItem, 
                EnumerableExtensions.NullIndex, 
                EnumerableExtensions.NullIndex, 
                new Dictionary<object, object>(1, ReferenceComparer),
                options);
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
        /// <param name="options">scan options <see cref="Option"/></param>
        /// <returns></returns>
        private static DifferenceDetails ValueDifference<TA, TE>(TA actual, 
            string firstName, 
            TE expected,
            long refIndex, 
            long expectedIndex, 
            IDictionary<object, object> firstSeen,
            Option options)
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

            firstSeen = new Dictionary<object, object>(firstSeen, ReferenceComparer) {[actual] = expected};

            // deals with numerical values
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
                return ValueDifferenceEnumerable(actual as IEnumerable, 
                    firstName, 
                    expected as IEnumerable, 
                    refIndex, 
                    expectedIndex, 
                    firstSeen,
                    options);
            }

            return DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex , expectedIndex);
        }

        // compare to an expected anonymous type
        private static DifferenceDetails AnonymousTypeDifference<TA, TE>(TA actual, string firstname, TE expected, Type type)
        {
            // anonymous types only have public properties
            var criteria = new ClassMemberCriteria(BindingFlags.Instance|BindingFlags.Public);
            var wrapper = ReflectionWrapper.BuildFromInstance(type, expected, criteria);
            var actualWrapped = ReflectionWrapper.BuildFromInstance(actual.GetType(), actual, criteria);
            var differences = actualWrapped.MemberMatches(wrapper).Where(match => !match.DoValuesMatches).Select(DifferenceDetails.FromMatch).ToList();

            return DifferenceDetails.DoesNotHaveExpectedDetails(firstname, actual, expected, 0, 0, differences);
        }

        // compare to a numerical difference
        private static DifferenceDetails NumericalValueDifference<TA, TE>(TA actual,         
            string firstName, 
            TE expected,
            long refIndex, 
            long expectedIndex, 
            Type commonType)
        {
            var convertedActual = Convert.ChangeType(actual, commonType, CultureInfo.InvariantCulture);
            var convertedExpected = Convert.ChangeType(expected, commonType, CultureInfo.InvariantCulture);
            return convertedExpected.Equals(convertedActual) ? null : 
                DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex, expectedIndex);
        }

        private static DifferenceDetails ValueDifferenceDictionary(IReadOnlyDictionary<object, object> sutDictionary,
            string sutName,
            IReadOnlyDictionary<object, object> expectedDictionary,
            long refIndex, 
            long expectedIndex, 
            IDictionary<object, object> firstItemsSeen,
            Option options)
        {
            var index = 0L;
            var valueDifferences = new List<DifferenceDetails>();
            var equivalent = true;
            var unexpectedKeys = sutDictionary.Keys.Where(k => !expectedDictionary.ContainsKey(k)).ToList();
            // Stryker disable once Equality,Block : Mutation does not alter behaviour
            if (options.HasFlag(Option.Fast) && unexpectedKeys.Count > 0)
            {
                return DifferenceDetails.DoesNotHaveExpectedValue(sutName, sutDictionary, expectedDictionary, refIndex, expectedIndex);
            }

            var sutIndexes = new Dictionary<object, long>(sutDictionary.Count);
            var sutIndex= 0;
            foreach (var sutDictionaryKey in sutDictionary.Keys)
            {
                sutIndexes[sutDictionaryKey] = sutIndex++;
            }

            foreach (var keyValuePair in expectedDictionary)
            {
                if (!sutDictionary.ContainsKey(keyValuePair.Key))
                {
                    if (unexpectedKeys.Count > 0)
                    {
                        var unexpectedKey = unexpectedKeys[0];
                        valueDifferences.Add(DifferenceDetails.WasFoundInsteadOf(
                            sutName,
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
                        sutIndexes[keyValuePair.Key], 
                        index,
                        firstItemsSeen,
                        options);
                    if (itemDiffs != null)
                    {
                        // Stryker disable once Block : Mutation does not alter behaviour
                        if (options.HasFlag(Option.Fast))
                        {
                            return DifferenceDetails.DoesNotHaveExpectedValue(sutName, sutDictionary, expectedDictionary, refIndex, expectedIndex);
                        }
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
            IDictionary<object, object> firstSeen,
            Option options)
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

                return $"{firstName}[{string.Join(",", indices.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray())}]";
            }, 
                sutIndex, 
                expectedIndex,
                firstSeen,
                options);
        }

        private static DifferenceDetails ValueDifferenceEnumerable(IEnumerable firstItem, 
            string firstName,
            IEnumerable otherItem,
            long sutIndex,
            long expectedIndex,
            IDictionary<object, object> firstSeen,
            Option options)
        {
            if (firstItem.GetType().IsArray && otherItem.GetType().IsArray)
            {
                return ValueDifferenceArray(firstItem as Array,
                    firstName, 
                    otherItem as Array,
                    sutIndex,
                    expectedIndex,
                    firstSeen, 
                    options);
            }

            var dictionary = DictionaryExtensions.WrapDictionary<object, object>(otherItem);
            if (dictionary != null)
            {
                var wrapDictionary = DictionaryExtensions.WrapDictionary<object, object>(firstItem);
                if (wrapDictionary != null)
                {
                    return ValueDifferenceDictionary(wrapDictionary, firstName, dictionary, sutIndex, expectedIndex, firstSeen, options);
                }
            }

            return ScanEnumeration(firstItem, otherItem, firstName, x => $"{firstName}[{x}]", sutIndex, expectedIndex, firstSeen, options);
        }

        private static DifferenceDetails ScanEnumeration(
            IEnumerable actualEnumerable, 
            IEnumerable expectedEnumerable, 
            string firstName,
            Func<long, string> namingCallback,
            long sutIndex,
            long expectedIndex,
            IDictionary<object, object> firstSeen,
            Option options)
        {
 
            var index = 0L;
            var expected = new Dictionary<long, object>();
            var unexpected = new Dictionary<long, object>();
            var aggregatedDifferences = new Dictionary<long, DifferenceDetails>();
            var aggregatedEquivalenceErrors = new Dictionary<long, DifferenceDetails>();
            var valueDifferences = new List<DifferenceDetails>();
            var scanner = expectedEnumerable.GetEnumerator();
            var isEquivalent = true;
            var ordered = !options.HasFlag(Option.Equivalence) && expectedEnumerable.GetType().IsAList();

            foreach (var actualItem in actualEnumerable)
            {
                var firstItemName = namingCallback(index);
                if (!scanner.MoveNext())
                {
                    valueDifferences.Add(DifferenceDetails.WasNotExpected(firstItemName, actualItem, index));
                    unexpected.Add(index, actualItem);
                    if (options.HasFlag(Option.Fast))
                    {
                        return DifferenceDetails.DoesNotHaveExpectedValue(firstName, actualEnumerable, expectedEnumerable, sutIndex, expectedIndex);
                    }
                    continue;
                }

                var aggregatedDifference = ValueDifference(actualItem, firstItemName, scanner.Current, index, index, firstSeen, options);

                if (aggregatedDifference != null)
                {
                    if (ordered)
                    {
                        aggregatedDifferences.Add(index, aggregatedDifference);
                    }
                    if (!aggregatedDifference.IsEquivalent())
                    {
                        // try to see it was at a different position
                        var entryInCache = expected.Where(pair => EqualityHelper.FluentEquivalent(pair.Value, actualItem));
                        if (entryInCache.Any())
                        {
                            var expectedEntryIndex = entryInCache.First().Key;
                            if (ordered)
                            {
                                // we found the value at another index
                                aggregatedEquivalenceErrors.Add(index,
                                    DifferenceDetails.WasFoundElseWhere(firstItemName, actualItem, expectedEntryIndex, index));
                                aggregatedDifferences.Remove(index);
                            }

                            expected.Remove(expectedEntryIndex);
                        }
                        else
                        {
                            // store it in case this is a needed entry
                            unexpected.Add(index, actualItem);
                        }

                        // what about the expected value
                        var indexInCache = unexpected.Where(pair => EqualityHelper.FluentEquivalent(pair.Value, scanner.Current));
                        if (indexInCache.Any())
                        {
                            var actualIndex = indexInCache.First().Key;
                            if (ordered)
                            {
                                aggregatedEquivalenceErrors.Add(actualIndex, DifferenceDetails.WasFoundElseWhere(
                                    namingCallback(actualIndex),
                                    scanner.Current, index, actualIndex));
                                aggregatedDifferences.Remove(actualIndex);
                            }

                            unexpected.Remove(actualIndex);
                        }
                        else
                        {
                            expected.Add(index, scanner.Current);
                        }
                    }
                }

                index++;
            }

            // several entries appear to be missing
            while (scanner.MoveNext())
            {
                if (options.HasFlag(Option.Fast))
                {
                    return DifferenceDetails.DoesNotHaveExpectedValue(firstName, actualEnumerable, expectedEnumerable, sutIndex, expectedIndex);
                }
                valueDifferences.Add(DifferenceDetails.WasNotFound(namingCallback(index), scanner.Current, index));
                isEquivalent = false;
            }

            if (isEquivalent)
            {
                isEquivalent = expected.Count == 0 && unexpected.Count == 0;
            }

            // for equivalency, we have a bunch of different entries
            if (!ordered)
            {
                while (expected.Count > 0 && unexpected.Count > 0)
                {
                    var unexpectedEntry = unexpected.First();
                    var expectedEntryIndex = unexpectedEntry.Key;
                    if (!expected.TryGetValue(unexpectedEntry.Key, out var expectedEntryObject))
                    {
                        expectedEntryIndex = expected.Keys.First();
                        expectedEntryObject = expected[expectedEntryIndex];
                    }


                    var differenceDetails = ValueDifference(unexpectedEntry.Value, namingCallback(unexpectedEntry.Key), expectedEntryObject, unexpectedEntry.Key, expectedIndex, firstSeen, options);
                    if (differenceDetails.Count == 0)
                    {
                        // if there is no detailed difference, just describe a general one
                        differenceDetails = DifferenceDetails.WasFoundInsteadOf(namingCallback(unexpectedEntry.Key),
                            unexpectedEntry.Value, expectedEntryObject, unexpectedEntry.Key, expectedEntryIndex);
                    }
                    valueDifferences.Add(differenceDetails);
                    expected.Remove(expectedEntryIndex);
                    unexpected.Remove(unexpectedEntry.Key);
                }
            }
            
            valueDifferences.AddRange(aggregatedDifferences.Values);
            valueDifferences.AddRange(aggregatedEquivalenceErrors.Values);
            
            if (valueDifferences.Count == 0)
            {
                return null;
            }

            valueDifferences = valueDifferences.OrderBy(d => d.ActualIndex).ToList();
            return isEquivalent ? 
                DifferenceDetails.DoesNotHaveExpectedDetailsButIsEquivalent(firstName, actualEnumerable, expectedEnumerable, sutIndex, expectedIndex, valueDifferences)
                : DifferenceDetails.DoesNotHaveExpectedDetails(firstName, actualEnumerable, expectedEnumerable, sutIndex, expectedIndex, valueDifferences);
        }
    }
}