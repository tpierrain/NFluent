// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EqualityHelper.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using NFluent.Extensions;

namespace NFluent.Helpers
{
    using Extensibility;
    using System.Reflection;

    /// <summary>
    /// Helper class related to Equality methods (used like a traits).
    /// </summary>
    internal static class EqualityHelper
    {
        /// <summary>
        /// Checks that a given instance is considered to be equal to another expected instance. Throws <see cref="FluentCheckException"/> otherwise.
        /// </summary>
        /// <typeparam name="T">Checked type.
        /// </typeparam>
        /// <typeparam name="TU">Checker type.
        /// </typeparam>
        /// <param name="checker">
        /// The checker.
        /// </param>
        /// <param name="expected">
        /// The expected instance.
        /// </param>
        /// <param name="useOperator">Set to true to use operator== instead of Equals</param>
        /// <exception cref="FluentCheckException">
        /// The actual value is not equal to the expected value.
        /// </exception>
        public static void IsEqualTo<T, TU>(IChecker<T, TU> checker, object expected, bool useOperator) where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            var instance = checker.Value;
            if (FluentEquals(instance, expected, useOperator ? EqualityMode.OperatorEq : EqualityMode.Equals))
            {
                return;
            }
            // Should throw

            throw new FluentCheckException(BuildErrorMessage(checker, expected, false, useOperator));
        }

        internal static ICheckLink<TU> PerformEqualCheck<T, TU, TE>(IChecker<T, TU> checker, TE expected,
            bool userOperator, bool negated = false) where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            var mode = EqualityMode.Equals;
            if (userOperator)
            {
                mode = EqualityMode.OperatorEq;
            }
            if (checker.Negated)
                negated = !negated;
            if (negated == FluentEquals(checker.Value, expected, mode))
            {
                throw new FluentCheckException(BuildErrorMessage(checker, expected, negated, userOperator));
            }
            return checker.BuildChainingObject();
        }

        /// <summary>
        /// Checks that a given instance is not considered to be equal to another expected instance. Throws <see cref="FluentCheckException"/> otherwise.
        /// </summary>
        /// <typeparam name="T">Checked type.
        /// </typeparam>
        /// <typeparam name="TU">Checker type.
        /// </typeparam>
        /// <param name="checker">The checker.</param>
        /// <param name="expected">The expected instance.</param>
        /// <param name="useOperator">Set to true to use operator== instead of Equals</param>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public static void IsNotEqualTo<T, TU>(IChecker<T, TU> checker, object expected, bool useOperator) where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            if (FluentEquals(checker.Value, expected, useOperator ? EqualityMode.OperatorEq : EqualityMode.Equals))
            {
                throw new FluentCheckException(BuildErrorMessage(checker, expected, true, false));
            }
        }

        private static bool FluentEquals(object instance, object expected, EqualityMode mode)
        {
            // ReSharper disable once RedundantNameQualifier
            var ret=  object.Equals(instance, expected);
            if (mode == EqualityMode.OperatorEq || mode == EqualityMode.OperatorNeq)
            {
                var actualType = instance.GetTypeWithoutThrowingException();
                var expectedType = expected.GetTypeWithoutThrowingException();
                var operatorName = mode == EqualityMode.OperatorEq ? "op_Equality" : "op_Inequality";
                var ope = actualType
                    .GetMethod(operatorName, new[] { actualType, expectedType }) ?? expectedType
                        .GetMethod(operatorName, new[] { actualType, expectedType });
                if (ope == null) return ret;
                ret =(bool) ope.Invoke(null, new[] { instance, expected});
            }
            else if (expected != null && instance != null)
            {
                var expectedType = expected.GetType();
                // if both types are numerical, check if the values are the same to generate a precise message
                if (ExtensionsCommonHelpers.IsNumerical(expectedType) && ExtensionsCommonHelpers.IsNumerical(instance.GetType()))
                {

                    var changeType = Convert.ChangeType(instance, expectedType, null);
                    if (expected.Equals(changeType))
                    {
                        return true;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Builds the error message related to the Equality verification. This should be called only if the test failed (no matter it is negated or not).
        /// </summary>
        /// <typeparam name="T">Checked type.
        /// </typeparam>
        /// <typeparam name="TU">Checker type.</typeparam>
        /// <param name="checker">
        /// The checker.
        /// </param>
        /// <param name="expected">
        /// The other operand.
        /// </param>
        /// <param name="isEqual">
        /// A value indicating whether the two values are equal or not. <c>true</c> if they are equal; <c>false</c> otherwise.
        /// </param>
        /// <param name="usingOperator">true if comparison is done using operator</param>
        /// <returns>
        /// The error message related to the Equality verification.
        /// </returns>
        public static string BuildErrorMessage<T, TU>(IChecker<T, TU> checker, object expected, bool isEqual, bool usingOperator) where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            var msg = isEqual ? checker.BuildShortMessage("The {0} is equal to the {1} whereas it must not.") : checker.BuildShortMessage("The {0} is different from the {1}.");

            FillEqualityErrorMessage(msg, checker.Value, expected, isEqual, usingOperator);

            return msg.ToString();
        }

        public static void FillEqualityErrorMessage(FluentMessage msg, object instance, object expected, bool negated, bool usingOperator)
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
            var withType = (instance != null && expected != null && instance.GetType() != expected.GetType())
                           || (instance == null);

            // shall we display the hash too
            var withHash = instance != null && expected != null && instance.GetType() == expected.GetType()
                           && instance.ToString() == expected.ToString();

            msg.On(instance)
                .WithType(withType)
                .WithHashCode(withHash)
                .And.Expected(expected)
                .WithType(withType)
                .Comparison(operatorText)
                .WithHashCode(withHash);
        }

        private enum EqualityMode
        {
            Equals, OperatorEq, OperatorNeq
        }
    }
}