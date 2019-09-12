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
    using System.Text;
    using Extensibility;
    using Extensions;

#if NETSTANDARD1_3
    using System.Reflection;
#endif
    /// <summary>
    ///     Helper class related to Equality methods (used like a traits).
    /// </summary>
    internal static class EqualityHelper
    {
        private const string SutLabel = "actual";
        private const float FloatCloseToThreshold = 1f/100000;
        private const double DoubleCloseToThreshold = 1d/100000000;

        internal static ICheckLink<ICheck<T>> PerformEqualCheck<T, TE>(
            ICheck<T> check,
            TE expected,
            bool useOperator = false)
        {
            var mode = Check.EqualMode;

            if (useOperator)
            {
                mode =  EqualityMode.OperatorEq;
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

                    if (expected is IEnumerable && differenceDetails.Count>0)
                    {
                        test.SetValuesIndex(differenceDetails[0].Index);
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

        internal static bool FluentEquivalent(object instance, object expected)
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
                    if ( analysis.IsDifferent)
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
                    test.Fail("The {0} is equal to the {1} whereas it must not.", options|MessageOption.NoCheckedBlock);
                })
                .OnNegate( "The {0} is different from the {1}.")
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
                                  .GetMethod(operatorName, new[] {actualType, expectedType}) ?? expectedType
                                  .GetMethod(operatorName, new[] {actualType, expectedType});
                    if (ope != null)
                    {
                        var ret = (bool) ope.Invoke(null, new object[] {sut, expected});
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
            } 
            
            return result;
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
                    result.Add( DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex));
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

                firstSeen = new List<object>(firstSeen) {actual};

                if ( actual.IsAnEnumeration(false) && expected.IsAnEnumeration(false))
                {
                    return ValueDifferenceEnumerable(actual as IEnumerable, firstName, expected as IEnumerable, firstSeen);
                }
            }

            result.Add( DifferenceDetails.DoesNotHaveExpectedValue(firstName, actual, expected, refIndex));
            return result;
        }

        private static AggregatedDifference ValueDifferenceDictionary(IDictionary sutDico, 
            string sutName, 
            IDictionary expectedDico,
            ICollection<object> firstItemsSeen)
        {
            var valueDifferences = new AggregatedDifference {IsEquivalent = true};

            var actualKeyIterator = sutDico.Keys.GetEnumerator();
            var expectedKeyIterator = expectedDico.Keys.GetEnumerator();
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
                    valueDifferences.Add(DifferenceDetails.WasNotExpected($"{sutName}[{actualKeyIterator.Current.ToStringProperlyFormatted()}]", sutDico[actualKeyIterator.Current], index));
                    valueDifferences.IsEquivalent = false;
                } else if (!stillActualKeys)
                {
                    // key not found
                    valueDifferences.IsEquivalent = false;
                    valueDifferences.Add( DifferenceDetails.WasNotFound($"{sutName}[{expectedKeyIterator.Current.ToStringProperlyFormatted()}]", 
                        expectedDico[expectedKeyIterator.Current],
                        0));
                }
                else
                {
                    var actualKey = actualKeyIterator.Current;
                    var itemDiffs = ValueDifference(actualKey, 
                        $"{sutName} key[{index}]", 
                        expectedKeyIterator.Current, 
                        index, 
                        firstItemsSeen);
                    if (!itemDiffs.IsDifferent)
                    {
                        // same key, check the values
                        var keyAsString = actualKey.ToStringProperlyFormatted();
                        itemDiffs = ValueDifference(sutDico[actualKey],
                            $"{sutName}[{keyAsString}]",
                            expectedDico[actualKey], 
                            index, 
                            firstItemsSeen);
                        valueDifferences.IsEquivalent &= !itemDiffs.IsDifferent;
                    }
                    else if (valueDifferences.IsEquivalent)
                    {
                        // check if the dictionaries are equivalent anyway
                        valueDifferences.IsEquivalent = expectedDico.Contains(actualKey) &&
                                                        FluentEquivalent(sutDico[actualKey], expectedDico[actualKey]);
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
                valueDifferences.Add( DifferenceDetails.DoesNotHaveExpectedValue(firstName+".Rank", firstArray.Rank, secondArray.Rank, 0));
                return valueDifferences;
            }

            for (var i = 0; i < firstArray.Rank; i++)
            {
                if (firstArray.SizeOfDimension(i) == secondArray.SizeOfDimension(i))
                {
                    continue;
                }

                valueDifferences.Add(DifferenceDetails.DoesNotHaveExpectedValue($"{firstName}.Dimension({i})", 
                    firstArray.SizeOfDimension(i), 
                    secondArray.SizeOfDimension(i), 
                    i));
                return valueDifferences;
            }

            var indices = new int[firstArray.Rank];
            var secondIndices = new int[secondArray.Rank];
            for (var i = 0; i < firstArray.Length; i++)
            {
                var temp = i;       
                var label = new StringBuilder("[");
                for (var j = 0; j < firstArray.Rank; j++)
                {
                    var currentIndex = temp % firstArray.SizeOfDimension(j);
                    label.Append(currentIndex.ToString());
                    label.Append(j < firstArray.Rank - 1 ? "," : "]");
                    indices[j] = currentIndex + firstArray.GetLowerBound(j);
                    secondIndices[j] = currentIndex + secondArray.GetLowerBound(j);
                    temp /= firstArray.SizeOfDimension(j);
                }

                var firstEntry = firstArray.GetValue(indices);
                var secondEntry = secondArray.GetValue(secondIndices);
                valueDifferences.Merge(ValueDifference(firstEntry, firstName+label, secondEntry, i, firstSeen));
            }

            return valueDifferences;
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
            if (firstItem is IDictionary firstDico && otherItem is IDictionary secondDico)
            {
                return ValueDifferenceDictionary(firstDico, firstName, secondDico, firstSeen);
            }
            var valueDifferences = new AggregatedDifference();

            var scanner = otherItem.GetEnumerator();
            var index = 0;
            foreach (var item in firstItem)
            {
                var firstItemName = $"{firstName}[{index}]";
                if (!scanner.MoveNext())
                {
                    valueDifferences.Add(DifferenceDetails.WasNotExpected(firstItemName, item, index));
                    break;
                }

                valueDifferences.Merge(ValueDifference(item, firstItemName, scanner.Current,
                     index, firstSeen));
                index++;
            }

            if (scanner.MoveNext())
            {
                valueDifferences.Add(DifferenceDetails.WasNotFound($"{firstName}[{index}]", scanner.Current, index));
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

        internal class DifferenceDetails
        {
            private readonly DifferenceMode mode;

            private DifferenceDetails(string firstName, object firstValue, object secondValue, int index, DifferenceMode mode)
            {
                this.mode = mode;
                this.FirstName = firstName;
                this.FirstValue = firstValue;
                this.SecondValue = secondValue;
                this.Index = index;
            }

            public static DifferenceDetails WasNotExpected(string checkedName, object value, int index)
            {
                return new DifferenceDetails(checkedName, value, null, index, DifferenceMode.Extra);
            }

            public static DifferenceDetails DoesNotHaveExpectedValue(string checkedName, object value, object expected, int index)
            {
                return new DifferenceDetails(checkedName, value, expected, index, DifferenceMode.Value);
            }

            public static DifferenceDetails WasNotFound(string checkedName, object expected, int index)
            {
                return new DifferenceDetails(checkedName, null, expected, index, DifferenceMode.Missing);
            }

            public string FirstName { get; internal set; }
            public object FirstValue { get; internal set; }
            public object SecondValue { get; internal set; }
            public int Index { get; }

            public override string ToString()
            {
                return this.mode == DifferenceMode.Extra ? $"{this.FirstName} should not exist (value {this.SecondValue.ToStringProperlyFormatted()})"
                    : this.mode == DifferenceMode.Missing ?
                        $"{this.FirstName} does not exist. Expected {this.SecondValue.ToStringProperlyFormatted()}."
                    : $"{this.FirstName} = {this.FirstValue.ToStringProperlyFormatted()} instead of {this.SecondValue.ToStringProperlyFormatted()}.";
            }

            private enum DifferenceMode
            {
                Value,
                Missing,
                Extra
            };
        }

        internal class AggregatedDifference
        {
            private readonly List<DifferenceDetails> details = new List<DifferenceDetails>();
            private bool different;
            public bool IsEquivalent { get; set; }

            public int Count => this.details.Count;

            public bool IsDifferent => this.different|| this.details.Count > 0;

            public DifferenceDetails this[int id] => this.details[id];

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

            public bool DoesProvideDetails(object actual, object expected)
            {
                if (this.details.Count == 1)
                {
                    return !object.Equals(this.details[0].FirstValue, actual) || !object.Equals(this.details[0].SecondValue, expected);
                }

                return true;
            }

            public string GetErrorMessage(object sut, object expected)
            {
                var messageText = new StringBuilder("The {0} is different from the {1}."); 
                if (details.Count > 1)
                {
                    messageText.Append($" {details.Count} differences found!");
                }

                if (this.IsEquivalent)
                {
                    messageText.Append(" But they are equivalent.");
                }

                if (this.DoesProvideDetails(sut, expected))
                {
                    var differenceDetailsCount = Math.Min(ExtensionsCommonHelpers.CountOfLineOfDetails, this.details.Count);

                    for(var i = 0; i < differenceDetailsCount; i++)
                    {
                        messageText.AppendLine();
                        messageText.Append(this.details[i].ToString().DoubleCurlyBraces());
                    }

                    if (differenceDetailsCount != this.details.Count)
                    {
                        messageText.AppendLine();
                        messageText.Append($"... ({this.details.Count - differenceDetailsCount} differences omitted)");
                    }
                }

                return messageText.ToString();
            }
        }
    }
}