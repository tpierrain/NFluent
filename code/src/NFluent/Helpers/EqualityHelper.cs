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
        private const string ExpectedLabel = "expected";
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
                    if (differenceDetails.Count==0)
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

                    var messageText = new StringBuilder("The {0} is different from the {1}."); 
                    if (expected is IEnumerable)
                    {
                        test.SetValuesIndex(differenceDetails[0].Index);
                        if (differenceDetails.Count > 1)
                        {
                            messageText.Append($" {differenceDetails.Count} differences found!");
                        }

                        if (differenceDetails.IsEquivalent)
                        {
                            messageText.Append(" But the {checked} is equivalent to the {1}.");
                        }

                        if (differenceDetails.DoesProvideDetails(sut, expected))
                        {
                            var differenceDetailsCount = Math.Min(5, differenceDetails.Count);

                            for(var i = 0; i < differenceDetailsCount; i++)
                            {
                                messageText.AppendLine();
                                messageText.Append(differenceDetails[i].ToString().DoubleCurlyBraces());
                            }

                            if (differenceDetailsCount != differenceDetails.Count)
                            {
                                messageText.AppendLine();
                                messageText.Append("...");
                            }
                        }
                    }

                    test.Fail(messageText.ToString(), options);
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
                    if (FluentEquals(sut, expected, mode).Count == 0)
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

                    test.Fail("The {0} is different from the {1}.", options);
                })
                .OnNegate("The {0} is equal to the {1} whereas it must not.", 
                    MessageOption.NoCheckedBlock | MessageOption.WithType)
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
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

                    if ( FluentEquals(sut, expected, mode).Count != 0)
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

        internal static ICheckLink<ICheck<float>> PerformEqualCheck(ICheck<float> check,
            float expected)
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

        internal static bool FluentEquals(object instance, object expected)
        {
            return FluentEquals(instance, expected, Check.EqualMode).Count==0;
        }

        private static AggregatedDifference FluentEquals<TS, TE>(TS instance, TE expected, EqualityMode mode)
        {
            var result = new AggregatedDifference();
            var ret = false;
            switch (mode)
            {
                case EqualityMode.FluentEquals:
                    return ValueDifference(instance, SutLabel, expected, ExpectedLabel);
                case EqualityMode.OperatorEq:
                case EqualityMode.OperatorNeq:
                    ret = Equals(instance, expected);

                    var actualType = instance.GetTypeWithoutThrowingException();
                    var expectedType = expected.GetTypeWithoutThrowingException();
                    var operatorName = mode == EqualityMode.OperatorEq ? "op_Equality" : "op_Inequality";
                    var ope = actualType
                                  .GetMethod(operatorName, new[] {actualType, expectedType}) ?? expectedType
                                  .GetMethod(operatorName, new[] {actualType, expectedType});
                    if (ope != null)
                    {
                        ret = (bool) ope.Invoke(null, new object[] {instance, expected});
                        if (mode == EqualityMode.OperatorNeq)
                        {
                            ret = !ret;
                        }
                    }
                    break;
            }

            if (!ret)
            {
                result.Add(new DifferenceDetails(SutLabel, false, instance, expected, 0));
            }
            return result;
        }

        internal static AggregatedDifference ValueDifference<TA, TE>(TA firstItem, string firstName, TE otherItem,
            string secondName)
        {
            return ValueDifference(firstItem, firstName, otherItem, secondName, 0, new List<object>());
        }

        private static AggregatedDifference ValueDifference<TA, TE>(TA firstItem, string firstName, TE otherItem,
            string secondName, int refIndex, List<object> firstSeen)
        {
            var result = new AggregatedDifference();
            if (firstItem == null)
            {
                if (otherItem != null)
                {
                    result.Add( DifferenceDetails.WasNotFound(firstName, otherItem, refIndex));
                }

                return result;
            }

            if (firstItem.Equals(otherItem))
            {
                return result;
            }

            if (otherItem != null)
            {
                if (firstItem.GetType().IsArray && otherItem.GetType().IsArray)
                {
                    return ValueDifferenceArray(firstItem as Array, firstName, otherItem as Array, secondName,
                        firstSeen);
                }
                if (firstItem is IDictionary firstDico && otherItem is IDictionary secondDico)
                {
                    return ValueDifferenceDictionary(firstDico, firstName, secondDico, secondName, firstSeen);
                }
                else if (!(firstItem is string) && !(otherItem is string) && firstItem is IEnumerable first && otherItem is IEnumerable second)
                {
                    return ValueDifferenceEnumerable(first, firstName, second, secondName, firstSeen);
                }

                if (firstItem.GetType().IsNumerical() &&
                    otherItem.GetType().IsNumerical())
                {
                    var changeType = Convert.ChangeType(firstItem, otherItem.GetType(), null);
                    if (otherItem.Equals(changeType))
                    {
                        return result;
                    }
                }
            }

            result.Add( DifferenceDetails.DoesNotHaveExpectedValue(firstName, firstItem, otherItem, refIndex));
            return result;
        }

        private static AggregatedDifference ValueDifferenceDictionary(IDictionary sutDico, string sutName, 
            IDictionary expectedDico, string expectedName,
            List<object> firstItemsSeen)
        {
            var valueDifferences = new AggregatedDifference {IsEquivalent = true};
            if (firstItemsSeen.Contains(sutDico))
            {
                if (sutDico != expectedDico)
                {
                    valueDifferences.Add(DifferenceDetails.DoesNotHaveExpectedValue(sutName, sutDico, expectedDico, 0));
                }
                return valueDifferences;
            }

            firstItemsSeen.Add(sutDico);

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
                    valueDifferences.Add(new DifferenceDetails($"{sutName}[{expectedKeyIterator.Current.ToStringProperlyFormatted()}]", 
                        true,
                        null,  
                        expectedDico[expectedKeyIterator.Current], 
                        0));
                }
                else
                {
                    var actualKey = actualKeyIterator.Current;
                    var itemDiffs = ValueDifference(actualKey, 
                        $"{sutName} key[{index}]", 
                        expectedKeyIterator.Current, 
                        $"{expectedName} key[${index}]", 
                        index, 
                        firstItemsSeen);
                    if (itemDiffs.Count == 0)
                    {
                        // same key, check the values
                        var keyAsString = actualKey.ToStringProperlyFormatted();
                        itemDiffs = ValueDifference(sutDico[actualKey],
                            $"{sutName}[{keyAsString}]",
                            expectedDico[actualKey], 
                            $"{expectedName}[{keyAsString}]", 
                            index, 
                            firstItemsSeen);
                        valueDifferences.IsEquivalent &= itemDiffs.Count == 0;
                    }
                    else if (valueDifferences.IsEquivalent)
                    {
                        // check if the dictionaries are equivalent anyway
                        valueDifferences.IsEquivalent = expectedDico.Contains(actualKey) &&
                                                        FluentEquals(sutDico[actualKey], expectedDico[actualKey]);
                    }
                    valueDifferences.Merge(itemDiffs);
                }

                index++;
            }
            return valueDifferences;
        }

        private static AggregatedDifference ValueDifferenceArray(Array firstArray, string firstName, Array secondArray, string secondName, List<object> firstSeen)
        {
            var valueDifferences = new AggregatedDifference();

            if (firstArray.Rank != secondArray.Rank)
            {
                valueDifferences.Add(new DifferenceDetails(firstName+".Rank", false, firstArray.Rank, secondArray.Rank, 0));
                return valueDifferences;
            }
            for (var i = 0; i < firstArray.Rank; i++)
            {
                if (firstArray.SizeOfDimension(i) == secondArray.SizeOfDimension(i))
                {
                    continue;
                }

                valueDifferences.Add(new DifferenceDetails($"{firstName}.Dimension({i})", false, firstArray.SizeOfDimension(i), secondArray.SizeOfDimension(i), i));
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
                valueDifferences.Merge(ValueDifference(firstEntry, firstName+label, secondEntry, secondName+label, i, firstSeen));
            }

            return valueDifferences;
        }

        private static AggregatedDifference ValueDifferenceEnumerable(IEnumerable firstItem, string firstName,
            IEnumerable otherItem, string secondName,
            ICollection<object> firstSeen)
        {
            var valueDifferences = new AggregatedDifference();
            if (firstSeen.Contains(firstItem))
            {
                if (firstSeen != otherItem)
                {
                    valueDifferences.Add(DifferenceDetails.DoesNotHaveExpectedValue(firstName, firstItem, otherItem, 0));
                }
                return valueDifferences;
            }

            firstSeen.Add(firstItem);
            firstSeen.Add(otherItem);

            var scanner = otherItem.GetEnumerator();
            var index = 0;
            foreach (var item in firstItem)
            {
                var firstItemName = $"{firstName}[{index}]";
                if (!scanner.MoveNext())
                {
                    valueDifferences.Add(new DifferenceDetails(firstItemName, false, item, null, index));
                    break;
                }

                var secondItemName = $"{secondName}[{index}]";
                valueDifferences.Merge(ValueDifference(item, firstItemName, scanner.Current,
                    secondItemName, index, new List<object>(firstSeen)));
                index++;
            }

            if (scanner.MoveNext())
            {
                valueDifferences.Add(new DifferenceDetails($"{firstName}[{index}]", true, null, scanner.Current, index));
            }

            return valueDifferences;
        }

        internal class EqualityComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return FluentEquals(x, y, Check.EqualMode).Count == 0;
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
            private readonly bool notFound;
            private readonly bool notExpected;

            public DifferenceDetails(string firstName, bool notFound, object firstValue, object secondValue, int index)
            {
                this.notFound = notFound;
                this.FirstName = firstName;
                this.FirstValue = firstValue;
                this.SecondValue = secondValue;
                this.Index = index;
            }

            private DifferenceDetails(string firstName, object firstValue, object secondValue, int index, bool notFound, bool notExpected)
            {
                this.notFound = notFound;
                this.notExpected = notExpected;
                this.FirstName = firstName;
                this.FirstValue = firstValue;
                this.SecondValue = secondValue;
                this.Index = index;
            }

            public static DifferenceDetails WasNotExpected(string checkedName, object value, int index)
            {
                return new DifferenceDetails(checkedName, value, null, index, false, true);
            }

            public static DifferenceDetails DoesNotHaveExpectedValue(string checkedName, object value, object expected, int index)
            {
                return new DifferenceDetails(checkedName, value, expected, index, false, false);
            }

            public static DifferenceDetails WasNotFound(string checkedName, object expected, int index)
            {
                return new DifferenceDetails(checkedName, null, expected, index, true, false);
            }

            public string FirstName { get; internal set; }
            public object FirstValue { get; internal set; }
            public object SecondValue { get; internal set; }
            public int Index { get; }

            public override string ToString()
            {
                return this.notExpected ? $"{this.FirstName} should not exist (value {this.SecondValue.ToStringProperlyFormatted()})"
                    : this.notFound ?
                        $"{this.FirstName} does not exist. Expected {this.SecondValue.ToStringProperlyFormatted()}."
                    : $"{this.FirstName} = {this.FirstValue.ToStringProperlyFormatted()} instead of {this.SecondValue.ToStringProperlyFormatted()}.";
            }
        }

        internal class AggregatedDifference
        {
            private readonly List<DifferenceDetails> details = new List<DifferenceDetails>();
            public bool IsEquivalent { get; set; }

            public int Count => this.details.Count;

            public DifferenceDetails this[int id] => this.details[id];

            public void Add(DifferenceDetails detail)
            {
                this.details.Add(detail);
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
        }
    }
}