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

                    if (FluentEquals(sut, expected, mode))
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

                    test.Fail("The {0} is different from the {1}.", options);
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
                    if (FluentEquals(sut, expected, mode))
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

        internal static ICheckLink<ICheck<T>> PerformInequalCheck<T, TE>(
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

                    if ( !FluentEquals(sut, expected, mode))
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
                    var mainLine = $"The {{0}} is different from the {{1}}";
                    if (ratio < 0.0001)
                    {
                        mainLine += $", with a difference of {diff:G2}";
                    }

                    mainLine += ".";

                    if (ratio < 0.000000001)
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
                    var mainLine = $"The {{0}} is different from the {{1}}";
                    if (ratio < 0.0001)
                    {
                        mainLine += $", with a difference of {diff:G2}";
                    }

                    mainLine += ".";

                    if (ratio < 0.00001)
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
            return FluentEquals(instance, expected, Check.EqualMode);
        }

        private static bool FluentEquals(object instance, object expected, EqualityMode mode)
        {
            var ret = Equals(instance, expected);
            switch (mode)
            {
                case EqualityMode.FluentEquals:
                    return ValueDifference(instance, "actual", expected, "expected").Count == 0;
                case EqualityMode.OperatorEq:
                case EqualityMode.OperatorNeq:

                    var actualType = instance.GetTypeWithoutThrowingException();
                    var expectedType = expected.GetTypeWithoutThrowingException();
                    var operatorName = mode == EqualityMode.OperatorEq ? "op_Equality" : "op_Inequality";
                    var ope = actualType
                                  .GetMethod(operatorName, new[] {actualType, expectedType}) ?? expectedType
                                  .GetMethod(operatorName, new[] {actualType, expectedType});
                    if (ope != null)
                    {
                        ret = (bool) ope.Invoke(null, new[] {instance, expected});
                        if (mode == EqualityMode.OperatorNeq)
                        {
                            ret = !ret;
                        }
                    }

                    break;
            }

            return ret;
        }

        internal static IList<DifferenceDetails> ValueDifference(object firstItem, string firstName, object otherItem,
            string secondName)
        {
            return ValueDifference(firstItem, firstName, otherItem, secondName, new List<object>(),
                new List<object>());
        }

        private static IList<DifferenceDetails> ValueDifference(object firstItem, string firstName, object otherItem,
            string secondName, List<object> firstSeen, List<object> secondSeen)
        {
            var result = new List<DifferenceDetails>();
            if (firstItem == null)
            {
                if (otherItem != null)
                {
                    result.Add(new DifferenceDetails(firstName, null, secondName, otherItem));
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
                        firstSeen, secondSeen);
                }
                if (firstItem is IEnumerable first && otherItem is IEnumerable second)
                {
                    return ValueDifferenceEnumerable(first, firstName, second, secondName, firstSeen, secondSeen);
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

            result.Add(new DifferenceDetails(firstName, firstItem, secondName, otherItem));
            return result;
        }

        private static IList<DifferenceDetails> ValueDifferenceArray(Array firstArray, string firstName, Array secondArray, string secondName, List<object> firstSeen, List<object> secondSeen)
        {
            var valueDifferences = new List<DifferenceDetails>();

            if (firstArray.Rank != secondArray.Rank)
            {
                valueDifferences.Add(new DifferenceDetails(firstName+".Rank", firstArray, secondName+".Rank", secondArray));
                return valueDifferences;
            }
            for (var i = 0; i < firstArray.Rank; i++)
            {
                if (firstArray.SizeOfDimension(i) != secondArray.SizeOfDimension(i))
                {
                    valueDifferences.Add(new DifferenceDetails($"{firstName}.Dimension({i})", firstArray, $"{secondName}.Dimension({i})", secondArray));
                    return valueDifferences;
                }
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
                valueDifferences.AddRange(ValueDifference(firstEntry, firstName+label, secondEntry, secondName+label, firstSeen, secondSeen));
            }

            return valueDifferences;
        }

        private static IList<DifferenceDetails> ValueDifferenceEnumerable(IEnumerable firstItem, string firstName,
            IEnumerable otherItem,
            string secondName, List<object> firstSeen, List<object> secondSeen)
        {
            var valueDifferences = new List<DifferenceDetails>();
            if (firstSeen.Contains(firstItem) || secondSeen.Contains(otherItem))
            {
                valueDifferences.Add(new DifferenceDetails(firstName, null, secondName, null));
                return valueDifferences;
            }

            firstSeen.Add(firstItem);
            secondSeen.Add(otherItem);

            var scanner = otherItem.GetEnumerator();
            var index = 0;
            foreach (var item in firstItem)
            {
                var firstItemName = $"{firstName}[{index}]";
                if (!scanner.MoveNext())
                {
                    valueDifferences.Add(new DifferenceDetails(firstItemName, item, null, null));
                    break;
                }

                var secondItemName = $"{secondName}[{index}]";
                valueDifferences.AddRange(ValueDifference(item, firstItemName, scanner.Current,
                    secondItemName, new List<object>(firstSeen), new List<object>(secondSeen)));
                index++;
            }

            if (scanner.MoveNext())
            {
                valueDifferences.Add(new DifferenceDetails(null, null, $"{secondName}[{index}]", scanner.Current));
            }

            return valueDifferences;
        }

        internal class EqualityComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return FluentEquals(x, y, Check.EqualMode);
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
            public DifferenceDetails(string firstName, object firstValue, string secondName, object secondValue)
            {
                this.FirstName = firstName;
                this.FirstValue = firstValue;
                this.SecondName = secondName;
                this.SecondValue = secondValue;
            }

            public string FirstName { get; internal set; }
            public string SecondName { get; internal set; }
            public object FirstValue { get; internal set; }
            public object SecondValue { get; internal set; }
        }
    }
}