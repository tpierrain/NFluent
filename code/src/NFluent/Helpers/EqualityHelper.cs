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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
#if DOTNET_45
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

        private static readonly IEqualityComparer DefaultComparer = new EqualityComparer();
        private static readonly IDictionary<Type, IEqualityComparer> ComparerMap = new Dictionary<Type, IEqualityComparer>();

        internal static ICheckLink<ICheck<T>> PerformEqualCheck<T, TE>(ICheck<T> check,
            TE expected,
            IEqualityComparer comparer = null)
        {
            return PerformEqualCheck(check, expected, Check.EqualMode, comparer);
        }

        public static IEqualityComparer RegisterComparer<T>(IEqualityComparer comparer)
        {
            lock (ComparerMap)
            {
                ComparerMap.TryGetValue(typeof(T), out var result);
                if (comparer == null)
                {
                    ComparerMap.Remove(typeof(T));
                }
                else
                {
                    ComparerMap[typeof(T)] = comparer;
                }
                return result;
            }
        }

        private static IEqualityComparer FindComparer(Type searchType)
        {
            if (ComparerMap.Count == 0 || searchType == null)
            {
                return null;
            }

            lock (ComparerMap)
            {
                if (ComparerMap.TryGetValue(searchType, out var comparer))
                {
                    return comparer;
                }

                // Stryker disable once Linq: Mutation does not alter behaviour
                return searchType.GetInterfaces().Any(@interface => ComparerMap.TryGetValue(@interface, out comparer))
                    ? comparer
                    : FindComparer(searchType.GetTypeInfo().BaseType);
            }
        }

        private static IEqualityComparer FindComparer<T>()
        {
            return FindComparer(typeof(T));
        }

        public static bool CustomEquals<T, TU>(T sut, TU expected)
        {
            var comparer = FindComparer<T>() ?? FindComparer(expected.GetType()) ?? DefaultComparer;
            return comparer.Equals(expected, sut);
        }

        internal static ICheckLink<ICheck<T>> PerformEqualCheck<T, TE>(ICheck<T> check,
            TE expected,
            EqualityMode mode,
            IEqualityComparer comparer = null)
        {

            if (typeof(T).IsNumerical() && typeof(T) == typeof(TE))
            {
                if (typeof(T) == typeof(double))
                {
                    PerformEqualCheckDouble((ICheck<double>) check, expected);
                    return ExtensibilityHelper.BuildCheckLink(check);
                }

                if (typeof(T) == typeof(float))
                {
                    PerformEqualCheckFloat((ICheck<float>) check, expected);
                    return ExtensibilityHelper.BuildCheckLink(check);
                }
            }
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
                    var differenceDetails = FluentEquals(sut, expected, mode, DifferenceFinders.Option.Detailed, comparer);
                    if (differenceDetails == null)
                    {
                        return;
                    }

                    // shall we display the type as well?
                    var options = MessageOption.None;
                    if (sut == null || 
                        (expected != null 
                         && sut.GetType() != expected.GetType() 
                         && !(sut.GetType().IsNumerical() && expected.GetType().IsNumerical())))
                    {
                        // Stryker disable once Assignment: Mutation does not alter behaviour
                        options |= MessageOption.WithType;
                    }

                    // shall we display the hash too?
                    if (sut != null && expected != null && sut.GetType() == expected.GetType()
                        && sut.ToStringProperlyFormatted() == expected.ToStringProperlyFormatted())
                    {
                        // Stryker disable once Assignment: Mutation does not alter behaviour
                        options |= MessageOption.WithHash;
                    }

                    differenceDetails.GetFirstDifferenceIndexes(out var actualIndex, out var expectedIndex);
                    test.SetValuesIndex(actualIndex, expectedIndex);

                    test.Fail(differenceDetails.GetMessage(false), options);
                })
                .OnNegate("The {0} is equal to the {1} whereas it must not.",
                    MessageOption.NoCheckedBlock | MessageOption.WithType)
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        internal static bool FluentEquals(object instance, object expected)
        {
            return FluentEquals(instance, expected, Check.EqualMode, DifferenceFinders.Option.Fast) == null;
        }

        internal static bool FluentEquivalent<TS, TE>(TS actual, TE expected)
        {
            var result = FluentEquals(actual, expected, Check.EqualMode, DifferenceFinders.Option.Equivalence);
            return result == null;
        }

        private static void PerformEqualCheckDouble(ICheck<double> check, object val)
        {
            var expected = (double) val;
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
                    if (ratio < 1.0/10240)
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
        }

        private static void PerformEqualCheckFloat(ICheck<float> check, object value)
        {
            var expected = (float) value;
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
                    if (ratio < 1.0f/10240)
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
        }

        private static DifferenceDetails FluentEquals<TS, TE>(TS sut, TE expected, EqualityMode mode, DifferenceFinders.Option option, IEqualityComparer comparer = null)
        {
            var result = DifferenceDetails.IsDifferent(sut, expected);
            if (comparer != null)
            {
                return !comparer.Equals(expected, sut) ? result : null;
            }

            switch (mode)
            {
                case EqualityMode.FluentEquals:
                    return DifferenceFinders.ValueDifference(sut, SutLabel, expected, option);
                case EqualityMode.OperatorEq:
                case EqualityMode.OperatorNeq:
                    var actualType = sut.GetTypeWithoutThrowingException();
                    var expectedType = expected.GetTypeWithoutThrowingException();
                    var operatorName = mode == EqualityMode.OperatorEq ? "op_Equality" : "op_Inequality";
                    var ope = actualType
                                  .GetMethod(operatorName, new[] { actualType, expectedType }) ?? expectedType
                                  .GetMethod(operatorName, new[] { actualType, expectedType });
                    bool ret;
                    if (ope != null)
                    {
                        ret = (bool)ope.Invoke(null, new object[] { sut, expected });
                        if (mode == EqualityMode.OperatorNeq)
                        {
                            ret = !ret;
                        }
                    }
                    else
                    {
                        ret = Equals(sut, expected);
                    }

                    if (ret)
                    {
                        result = null;
                    }
                    break;
                case EqualityMode.Equals:
                    if (Equals(expected, sut))
                    {
                        result = null;
                    }
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

                    var scan = FluentEquals(sut, content, EqualityMode.FluentEquals, DifferenceFinders.Option.Detailed | DifferenceFinders.Option.Equivalence);

                    if (scan == null)
                    {
                        return;
                    }

                    test.Fail(scan.GetMessage(true));
                }).DefineExpectedValue(content)
                .OnNegate("The {checked} is equivalent to the {expected} whereas it should not.").EndCheck();
        }

        private class EqualityComparer : IEqualityComparer
        {
            public new bool Equals(object x, object y)
            {
                return x.Equals(y);
            }

            //ncrunch: no coverage start
            [ExcludeFromCodeCoverage]
            public int GetHashCode(object obj)
            {
                throw new NotSupportedException();
            }
            //ncrunch: no coverage end
        }

        internal class EqualityComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return FluentEquals(x, y);
            }

            //ncrunch: no coverage start
            [ExcludeFromCodeCoverage]
            public int GetHashCode(T obj)
            {
                throw new NotSupportedException();
            }
            //ncrunch: no coverage end
        }
    }
}