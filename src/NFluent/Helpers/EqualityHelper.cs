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
        /// <summary>
        ///     Builds the error message related to the Equality verification. This should be called only if the test failed (no
        ///     matter it is negated or not).
        /// </summary>
        /// <typeparam name="T">
        ///     Checked type.
        /// </typeparam>
        /// <typeparam name="TU">Checker type.</typeparam>
        /// <param name="checker">
        ///     The checker.
        /// </param>
        /// <param name="expected">
        ///     The other operand.
        /// </param>
        /// <param name="isEqual">
        ///     A value indicating whether the two values are equal or not. <c>true</c> if they are equal; <c>false</c> otherwise.
        /// </param>
        /// <param name="usingOperator">true if comparison is done using operator</param>
        /// <returns>
        ///     The error message related to the Equality verification.
        /// </returns>
        public static string BuildErrorMessage<T, TU>(IChecker<T, TU> checker, object expected, bool isEqual,
            bool usingOperator)
            where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            var msg = isEqual
                ? checker.BuildShortMessage("The {0} is equal to the {1} whereas it must not.")
                : checker.BuildShortMessage("The {0} is different from the {1}.");

            FillEqualityErrorMessage(msg, checker.Value, expected, isEqual, usingOperator);

            return msg.ToString();
        }

        public static void FillEqualityErrorMessage(FluentMessage msg, object instance, object expected, bool negated,
            bool usingOperator)
        {
            string operatorText;
            if (usingOperator)
            {
                operatorText = negated ? "different from (using operator!=)" : "equals to (using operator==)";
            }
            else
            {
                operatorText = negated ? "different from" : string.Empty;
            }

            if (negated)
            {
                msg.Expected(expected).Comparison(operatorText).WithType();
                return;
            }

            // shall we display the type as well?
            var withType = instance != null && expected != null && instance.GetType() != expected.GetType()
                           || instance == null;

            // shall we display the hash too
            var withHash = instance != null && expected != null && instance.GetType() == expected.GetType()
                           && instance.ToStringProperlyFormatted() == expected.ToStringProperlyFormatted();

            msg.On(instance)
                .WithType(withType)
                .WithHashCode(withHash)
                .And.Expected(expected)
                .WithType(withType)
                .Comparison(operatorText)
                .WithHashCode(withHash);
        }

        /// <summary>
        ///     Checks that a given instance is considered to be equal to another expected instance. Throws
        ///     <see cref="FluentCheckException" /> otherwise.
        /// </summary>
        /// <typeparam name="T">
        ///     Checked type.
        /// </typeparam>
        /// <typeparam name="TU">
        ///     Checker type.
        /// </typeparam>
        /// <param name="checker">
        ///     The checker.
        /// </param>
        /// <param name="expected">
        ///     The expected instance.
        /// </param>
        /// <exception cref="FluentCheckException">
        ///     The actual value is not equal to the expected value.
        /// </exception>
        public static void IsEqualTo<T, TU>(IChecker<T, TU> checker, object expected)
            where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            var instance = checker.Value;
            if (FluentEquals(instance, expected))
            {
                return;
            }

            // Should throw
            throw new FluentCheckException(BuildErrorMessage(checker, expected, false, false));
        }

        /// <summary>
        ///     Checks that a given instance is not considered to be equal to another expected instance. Throws
        ///     <see cref="FluentCheckException" /> otherwise.
        /// </summary>
        /// <typeparam name="T">
        ///     Checked type.
        /// </typeparam>
        /// <typeparam name="TU">
        ///     Checker type.
        /// </typeparam>
        /// N
        /// <param name="checker">The checker.</param>
        /// <param name="expected">The expected instance.</param>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public static void IsNotEqualTo<T, TU>(IChecker<T, TU> checker, object expected)
            where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            if (FluentEquals(checker.Value, expected))
            {
                throw new FluentCheckException(BuildErrorMessage(checker, expected, true, false));
            }
        }

        internal static ICheckLink<ICheck<double>> PerformEqualCheck(IChecker<double, ICheck<double>> checker,
            double expected)
        {
            var diff = Math.Abs(checker.Value - expected);
            return checker.ExecuteCheck(() =>
                {
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

                    var message = checker.BuildMessage(mainLine).Expected(expected);
                    throw new FluentCheckException(message.ToString());
                },
                BuildErrorMessage(checker, expected, true, false));
        }

        internal static ICheckLink<ICheck<float>> PerformEqualCheck(IChecker<float, ICheck<float>> checker,
            float expected)
        {
            var diff = Math.Abs(checker.Value - expected);
            return checker.ExecuteCheck(() =>
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (diff == 0.0)
                    {
                        return;
                    }

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    var ratio = expected == 0.0 ? 1.0 : Math.Abs(diff / expected);
                    var mainLine = $"The {{0}} is different from the {{1}}";
                    if (ratio < 0.001)
                    {
                        mainLine += $", with a difference of {diff:G2}";
                    }

                    mainLine += ".";

                    if (ratio < 0.00001)
                    {
                        mainLine += " You may consider using IsCloseTo() for comparison.";
                    }

                    var message = checker.BuildMessage(mainLine).Expected(checker.Value);
                    throw new FluentCheckException(message.ToString());
                },
                BuildErrorMessage(checker, expected, true, false));
        }

        internal static ICheckLink<TU> PerformEqualCheck<T, TU, TE>(
            IChecker<T, TU> checker,
            TE expected,
            bool useOperator = false,
            bool negated = false)
            where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            var mode = Check.EqualMode;

            var shouldFail = negated;
            if (useOperator)
            {
                mode = negated ? EqualityMode.OperatorNeq : EqualityMode.OperatorEq;
                shouldFail = false;
            }

            return checker.ExecuteCheck(() =>
                {
                    if (shouldFail == FluentEquals(checker.Value, expected, mode))
                    {
                        throw new FluentCheckException(BuildErrorMessage(checker, expected, negated, useOperator));
                    }
                },
                BuildErrorMessage(checker, expected, !negated, useOperator));
        }

        internal static bool FluentEquals(object instance, object expected)
        {
            return FluentEquals(instance, expected, Check.EqualMode);
        }

        private static bool FluentEquals(object instance, object expected, EqualityMode mode)
        {
            // ReSharper disable once RedundantNameQualifier
            var ret = Equals(instance, expected);
            if (mode == EqualityMode.FluentEquals)
            {
                return ValueDifference(instance, "actual", expected, "expected").Count == 0;
            }

            if (mode == EqualityMode.OperatorEq || mode == EqualityMode.OperatorNeq)
            {
                if (mode == EqualityMode.OperatorNeq)
                {
                    ret = !ret;
                }

                var actualType = instance.GetTypeWithoutThrowingException();
                var expectedType = expected.GetTypeWithoutThrowingException();
                var operatorName = mode == EqualityMode.OperatorEq ? "op_Equality" : "op_Inequality";
                var ope = actualType
                              .GetMethod(operatorName, new[] {actualType, expectedType}) ?? expectedType
                              .GetMethod(operatorName, new[] {actualType, expectedType});
                if (ope == null)
                {
                    return ret;
                }

                ret = (bool) ope.Invoke(null, new[] {instance, expected});
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
                if (firstItem is IEnumerable first && otherItem is IEnumerable second)
                {
                    return ValueDifferenceEnumerable(first, firstName, second, secondName, firstSeen, secondSeen);
                }

                if (ExtensionsCommonHelpers.IsNumerical(firstItem.GetType()) &&
                    ExtensionsCommonHelpers.IsNumerical(otherItem.GetType()))
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