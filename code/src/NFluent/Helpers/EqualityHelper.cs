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
#if NETSTANDARD1_3
    using System.Reflection;
#endif
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

        internal static ICheckLink<ICheck<T>> PerformEqualCheck<T, TE>(ICheck<T> check,
            TE expected,
            IEqualityComparer comparer = null)
        {
            return PerformEqualCheck(check, expected, Check.EqualMode, comparer);
        }

        internal static ICheckLink<ICheck<T>> PerformEqualCheck<T, TE>(ICheck<T> check,
            TE expected,
            EqualityMode mode,
            IEqualityComparer comparer = null)
        {

            ExtensibilityHelper.BeginCheck(check)
                .Analyze((sut, test) =>
                {
                    var modeLabel = string.Empty;
                    var negatedMode = string.Empty;
                    switch (mode)
                    {
                        case EqualityMode.OperatorEq:
                            modeLabel = "equals to (using operator==)";
                            negatedMode = " (using !operator==)";
                            break;
                        case EqualityMode.OperatorNeq:
                            modeLabel = "equals to (using !operator!=)";
                            negatedMode = " (using operator!=)";
                            break;
                    }
                    test.DefineExpectedValue(expected, modeLabel,
                            $"different from{negatedMode}");

                    var differenceDetails = FluentEquals(sut, expected, mode, comparer);
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

                    // shall we display the hash too?
                    if (sut != null && expected != null && sut.GetType() == expected.GetType()
                        && sut.ToStringProperlyFormatted() == expected.ToStringProperlyFormatted())
                    {
                        options |= MessageOption.WithHash;
                    }

                    test.SetValuesIndex(differenceDetails.GetFirstIndex(false));

                    test.Fail(differenceDetails.GetErrorMessage(sut, expected), options);
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

        internal static AggregatedDifference FluentEquals<TS, TE>(TS sut, TE expected, EqualityMode mode, IEqualityComparer comparer = null)
        {
            var result = new AggregatedDifference();
            if (comparer != null)
            {
                result.SetAsDifferent(!comparer.Equals(expected, sut));
                return result;
            }

            switch (mode)
            {
                case EqualityMode.FluentEquals:
                    return DifferenceFinders.ValueDifference(sut, SutLabel, expected);
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

        internal class EqualityComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return FluentEquals(x, y);
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
}