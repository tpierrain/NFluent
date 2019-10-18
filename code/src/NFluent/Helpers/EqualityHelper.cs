// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EqualityHelper.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
#if !DOTNET_20 && !DOTNET_30
    using System.Linq;
#endif
#if NETSTANDARD1_3
    using System.Reflection;
#endif
    using System.Text;
    using Extensibility;
    using Extensions;

    /// <summary>
    ///     Helper class related to Equality methods (used like a traits).
    /// </summary>
    internal static class EqualityHelper
    {
        private const string SutLabel = "actual";
        private const float FloatCloseToThreshold = 1f / 100000;
        private const double DoubleCloseToThreshold = 1d / 100000000;

        internal static ICheckLink<ICheck<T>> PerformEqualCheck<T, TE>(
            ICheck<T> check,
            TE expected,
            bool useOperator = false)
        {
            var mode = Check.EqualMode;

            if (useOperator)
            {
                mode = EqualityMode.OperatorEq;
            }
            ExtensibilityHelper.BeginCheck(check)
                .Analyze((sut, test) =>
                {
                    test.DefineExpectedValue(expected, useOperator ? "equals to (using operator==)" : "",
                            "different from" + (useOperator ? " (using !operator==)" : ""));

                    var differenceDetails = FluentEquals(sut, expected, mode);
                    if (!differenceDetails.IsDifferent)
                    {
                        return;
                    }

                    // shall we display the type as well?
                    var options = MessageOption.None;
                    if (sut == null || (expected != null && sut.GetType() != expected.GetType()))
                    {
                        options |= MessageOption.WithType;
                    }

                    // shall we display the hash too
                    if (sut != null && expected != null && sut.GetType() == expected.GetType()
                        && sut.ToStringProperlyFormatted() == expected.ToStringProperlyFormatted())
                    {
                        options |= MessageOption.WithHash;
                    }

                    if (expected is IEnumerable && differenceDetails.GetCount(false) > 0)
                    {
                        test.SetValuesIndex(differenceDetails.GetFirstIndex(false));
                    }

                    test.Fail(differenceDetails.GetErrorMessage(sut, expected), options);
                })
                .OnNegate("The {0} is equal to the {1} whereas it must not.",
                    MessageOption.NoCheckedBlock | MessageOption.WithType)
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        internal static ICheckLink<IStructCheck<T>> PerformEqualCheck<T, TE>(
            IStructCheck<T> check,
            TE expected) where T : struct
        {
            var mode = Check.EqualMode;

            ExtensibilityHelper.BeginCheck(check)
                .DefineExpectedValue(expected)
                .Analyze((sut, test) =>
                {
                    var analysis = FluentEquals(sut, expected, mode);
                    if (!analysis.IsDifferent)
                    {
                        return;
                    }

                    // shall we display the type as well?
                    var options = MessageOption.None;

                    // shall we display the hash too
                    if (expected != null && sut.GetType() == expected.GetType()
                        && sut.ToStringProperlyFormatted() == expected.ToStringProperlyFormatted())
                    {
                        options |= MessageOption.WithHash;
                    }

                    test.Fail(analysis.GetErrorMessage(sut, expected), options);
                })
                .OnNegate("The {0} is equal to the {1} whereas it must not.",
                    MessageOption.NoCheckedBlock | MessageOption.WithType)
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        internal static bool FluentEquals(object instance, object expected)
        {
            return !FluentEquals(instance, expected, Check.EqualMode).IsDifferent;
        }

        private static bool FluentEquivalent(object instance, object expected)
        {
            var scan = FluentEquals(instance, expected, Check.EqualMode);
            return !scan.IsDifferent || scan.IsEquivalent;
        }

        internal static ICheckLink<ICheck<T>> PerformUnequalCheck<T, TE>(
            ICheck<T> check,
            TE expected,
            bool useOperator = false)
        {
            var mode = Check.EqualMode;

            if (useOperator)
            {
                mode = EqualityMode.OperatorNeq;
            }
            ExtensibilityHelper.BeginCheck(check)
                .DefineExpectedValue(expected, "different from" + (useOperator ? " (using operator!=)" : ""),
                    useOperator ? "equals to (using operator==)" : "")
                .Analyze((sut, test) =>
                {
                    var analysis = FluentEquals(sut, expected, mode);
                    if (analysis.IsDifferent)
                    {
                        return;
                    }

                    // shall we display the type as well?
                    var options = MessageOption.None;
                    if (expected != null)
                    {
                        options |= MessageOption.WithType;
                    }

                    // shall we display the hash too
                    test.Fail("The {0} is equal to the {1} whereas it must not.", options | MessageOption.NoCheckedBlock);
                })
                .OnNegate("The {0} is different from the {1}.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        internal static ICheckLink<ICheck<double>> PerformEqualCheck(ICheck<double> check, double expected)
        {
            ExtensibilityHelper.BeginCheck(check)
                .DefineExpectedValue(expected)
                .Analyze((sut, test) =>
                {
                    var diff = Math.Abs(sut - expected);
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (diff == 0.0)
                    {
                        return;
                    }
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    var ratio = expected == 0.0 ? 1.0 : Math.Abs(diff / expected);
                    var mainLine = "The {0} is different from the {1}";
                    if (ratio < 0.0001)
                    {
                        mainLine += $", with a difference of {diff:G2}";
                    }

                    mainLine += ".";

                    if (ratio < DoubleCloseToThreshold)
                    {
                        mainLine += " You may consider using IsCloseTo() for comparison.";
                    }

                    test.Fail(mainLine);
                })
                .OnNegate("The {0} is equal to the {1} whereas it must not.", MessageOption.NoCheckedBlock | MessageOption.WithType)
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        internal static ICheckLink<ICheck<float>> PerformEqualCheck(ICheck<float> check, float expected)
        {
            ExtensibilityHelper.BeginCheck(check)
                .DefineExpectedValue(expected)
                .Analyze((sut, test) =>
                {
                    var diff = Math.Abs(sut - expected);
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (diff == 0.0)
                    {
                        return;
                    }
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    var ratio = expected == 0.0 ? 1.0 : Math.Abs(diff / expected);
                    var mainLine = "The {0} is different from the {1}";
                    if (ratio < 0.0001f)
                    {
                        mainLine += $", with a difference of {diff:G2}";
                    }

                    mainLine += ".";

                    if (ratio < FloatCloseToThreshold)
                    {
                        mainLine += " You may consider using IsCloseTo() for comparison.";
                    }

                    test.Fail(mainLine);
                })
                .OnNegate("The {0} is equal to the {1} whereas it must not.", MessageOption.NoCheckedBlock | MessageOption.WithType)
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        private static AggregatedDifference FluentEquals<TS, TE>(TS sut, TE expected, EqualityMode mode)
        {
            var result = new AggregatedDifference();
            switch (mode)
            {
                case EqualityMode.FluentEquals:
                    return ValueDifference(sut, SutLabel, expected);
                case EqualityMode.OperatorEq:
                case EqualityMode.OperatorNeq:
                    var actualType = sut.GetTypeWithoutThrowingException();
                    var expectedType = expected.GetTypeWithoutThrowingException();
                    var operatorName = mode == EqualityMode.OperatorEq ? "op_Equality" : "op_Inequality";
                    var ope = actualType
                                  .GetMethod(operatorName, new[] { actualType, expectedType }) ?? expectedType
                                  .GetMethod(operatorName, new[] { actualType, expectedType });
                    if (ope != null)
                    {
                        var ret = (bool)ope.Invoke(null, new object[] { sut, expected });
                        if (mode == EqualityMode.OperatorNeq)
                        {
                            ret = !ret;
                        }
                        result.SetAsDifferent(!ret);
                    }
                    else
                    {
                        result.SetAsDifferent(!Equals(sut, expected));
                    }
                    break;
                case EqualityMode.Equals:
                    result.SetAsDifferent(!Equals(expected, sut));
                    break;
                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        public static void ImplementEquivalentTo<T>(ICheckLogic<object> checker, IEnumerable<T> content)
        {
            var length = content?.Count() ?? 0;
            checker.Analyze((sut, test) =>
                {
                    if (sut == null)
                    {
                        if (content != null)
                        {
                            test.Fail("The {checked} is null whereas it should not.");
                        }

                        return;
                    }

                    if (content == null)
                    {
                        test.Fail("The {checked} must be null.");
                        return;
                    }

                    var scan = FluentEquals(sut, content, EqualityMode.FluentEquals);

                    if (scan.IsEquivalent || !scan.IsDifferent)
                    {
                        return;
                    }

                    test.Fail(scan.GetErrorMessage(sut, content, true));
                }).DefineExpectedValue(content)
                .OnNegate("The {checked} is equivalent to the {expected} whereas it should not.").EndCheck();
        }


        internal static AggregatedDifference ValueDifference<TA, TE>(TA firstItem, string firstName, TE otherItem)
        {
            return ValueDifference(firstItem, firstName, otherItem, 0, new List<object>());
        }

        private static AggregatedDifference ValueDifference<TA, TE>(TA actual, string firstName, TE expected, int refIndex, ICollection<object> firstSeen)
        {
            var result = new AggregatedDifference();
            if (expected == null)
            {
                if (actual != null)
                {
                    result.Add(DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex));
                }

                return result;
            }

            if (expected.Equals(actual))
            {
                return result;
            }

            if (actual != null)
            {
                // we silently convert numerical value
                if (actual.GetType().IsNumerical() &&
                    expected.GetType().IsNumerical())
                {
                    var changeType = Convert.ChangeType(actual, expected.GetType(), null);
                    if (expected.Equals(changeType))
                    {
                        return result;
                    }
                }

                if (firstSeen.Contains(actual))
                {
                    result.Add(DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, 0));
                    return result;
                }

                firstSeen = new List<object>(firstSeen) { actual };

                if (actual.IsAnEnumeration(false) && expected.IsAnEnumeration(false))
                {
                    return ValueDifferenceEnumerable(actual as IEnumerable, firstName, expected as IEnumerable, firstSeen);
                }
            }

            result.Add(DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex));
            return result;
        }

        private static AggregatedDifference ValueDifferenceDictionary(IReadOnlyDictionary<object, object> sutDico,
            string sutName,
            IReadOnlyDictionary<object, object> expectedDico,
            ICollection<object> firstItemsSeen)
        {
            // TODO: improve error messages
            var valueDifferences = new AggregatedDifference { IsEquivalent = true };

            var actualKeyIterator = sutDico.Keys.GetEnumerator();
            var expectedKeyIterator = expectedDico.Keys.GetEnumerator();
            var stillExpectedKeys = true;
            var stillActualKeys = true;
            var index = 0;
            for (; ; )
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
                    valueDifferences.Add(DifferenceDetails.WasNotExpected($"{sutName}[{actualKeyIterator.Current.ToStringProperlyFormatted()}]", sutDico[actualKeyIterator.Current], index));
                    valueDifferences.IsEquivalent = false;
                }
                else if (!stillActualKeys)
                {
                    // key not found
                    valueDifferences.IsEquivalent = false;
                    valueDifferences.Add(DifferenceDetails.WasNotFound($"{sutName}[{expectedKeyIterator.Current.ToStringProperlyFormatted()}]",
                        expectedDico[expectedKeyIterator.Current],
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
                    if (!itemDiffs.IsDifferent)
                    {
                        // same key, check the values
                        itemDiffs = ValueDifference(sutDico[actualKey],
                            $"{sutName}[{actualKey.ToStringProperlyFormatted()}]",
                            expectedDico[actualKey],
                            index,
                            firstItemsSeen);
                        valueDifferences.IsEquivalent &= (!itemDiffs.IsDifferent || itemDiffs.IsEquivalent);
                    }
                    else //if (valueDifferences.IsEquivalent)
                    {
                        // check if the dictionaries are equivalent anyway
                        var expectedIndex = expectedDico.ContainsKey(actualKey) ? expectedDico.Keys.ToList().FindIndex(x => x == actualKey) : -1;
                        if (expectedIndex >= 0)
                        {

                            itemDiffs = ValueDifference(sutDico[actualKey],
                                $"{sutName}[{actualKey.ToStringProperlyFormatted()}]",
                                expectedDico[actualKey],
                                index,
                                firstItemsSeen);
                            valueDifferences.IsEquivalent &= itemDiffs.IsEquivalent || !itemDiffs.IsDifferent;
                            valueDifferences.Add(
                                DifferenceDetails.WasFoundElseWhere($"{sutName} entry {actualKey.ToStringProperlyFormatted()}", expectedDico[actualKey], index, expectedIndex));
                        }
                        else
                        {
                            valueDifferences.Add(DifferenceDetails.WasNotExpected($"{sutName}'s key {actualKey.ToStringProperlyFormatted()}", sutDico[actualKey], index));
                            valueDifferences.IsEquivalent = false;
                        }
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

            var notSeen = new List<object>(firstArray.Length);
            var unexpected = new List<object>(firstArray.Length);

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

            var otherDico = DictionaryExtensions.WrapDictionary<object, object>(otherItem);
            if (otherDico != null)
            {
                var firstDico = DictionaryExtensions.WrapDictionary<object, object>(firstItem);
                if (firstDico != null)
                {
                    return ValueDifferenceDictionary(firstDico, firstName, otherDico, firstSeen);
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
                    mayBeEquivalent = false;
                    break;
                }

                var aggregatedDifference = ValueDifference(item, firstItemName, scanner.Current, index, firstSeen);
 
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

        internal class EqualityComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return !FluentEquals(x, y, Check.EqualMode).IsDifferent;
            }

            //ncrunch: no coverage start
            [Obsolete("Not implemented")]
            public int GetHashCode(T obj)
            {
                throw new NotSupportedException();
            }
            //ncrunch: no coverage end
        }

    }

    internal class AggregatedDifference
    {
        private readonly List<DifferenceDetails> details = new List<DifferenceDetails>();
        private bool different;
        public bool IsEquivalent { get; set; }

        public bool IsDifferent => this.different || this.details.Count > 0;

        public DifferenceDetails this[int id] => this.details[id];

        public int GetCount(bool forEquivalence = false)
        {
            return this.details.Count(x=>forEquivalence ? x.StillNeededForEquivalence : x.StillNeededForEquality);
        }

        public void Add(DifferenceDetails detail)
        {
            this.details.Add(detail);
        }

        public void SetAsDifferent(bool state)
        {
            this.different = state;
        }

        public void Merge(AggregatedDifference other)
        {
            this.details.AddRange(other.details);
        }

        public int GetFirstIndex(bool forEquivalence)
        {
            return this.details.FirstOrDefault(x => forEquivalence ? x.StillNeededForEquivalence : x.StillNeededForEquality)?.ActualIndex??0;
        }
        
        public bool DoesProvideDetails(object actual, object expected)
        {
            if (this.details.Count == 1)
            {
                return !Equals(this.details[0].FirstValue, actual) || !Equals(this.details[0].SecondValue, expected);
            }

            return true;
        }

        public string GetErrorMessage(object sut, object expected, bool forEquivalence = false)
        {
            var messageText = new StringBuilder(forEquivalence ? "The {0} is not equivalent to the {1}." : "The {0} is different from the {1}.");
            var errorCount = this.GetCount(forEquivalence);
            if (errorCount > 1)
            {
                messageText.Append($" {errorCount} differences found!");
            }

            if (this.IsEquivalent)
            {
                messageText.Append(" But they are equivalent.");
            }

            if (this.DoesProvideDetails(sut, expected))
            {
                var differenceDetailsCount = Math.Min(ExtensionsCommonHelpers.CountOfLineOfDetails, errorCount);

                if (errorCount - differenceDetailsCount == 1)
                {
                    // we don't want to truncate for one difference
                    differenceDetailsCount++;
                }

                var messages = 0;
                for (var i = 0; i < this.details.Count && messages<differenceDetailsCount; i++)
                {
                    if ((forEquivalence && !this.details[i].StillNeededForEquivalence)||(!forEquivalence && !this.details[i].StillNeededForEquality))
                    {
                        continue;
                    }

                    messages++;
                    messageText.AppendLine();
                    messageText.Append(this.details[i].GetMessage(forEquivalence).DoubleCurlyBraces());
                }
                    
                if (differenceDetailsCount != errorCount)
                {
                    messageText.AppendLine();
                    messageText.Append($"... ({errorCount - differenceDetailsCount} differences omitted)");
                }
            }

            return messageText.ToString();
        }
    }
}