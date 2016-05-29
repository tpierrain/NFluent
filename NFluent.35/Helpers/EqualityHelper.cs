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
        /// <exception cref="FluentCheckException">
        /// The actual value is not equal to the expected value.
        /// </exception>
        public static void IsEqualTo<T, TU>(IChecker<T, TU> checker, object expected) where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            var instance = checker.Value;
            if (FluentEquals(instance, expected))
            {
                return;
            }
#if! PORTABLE
            if (expected != null && instance != null)
            {
                var expectedType = expected.GetType();
                // if both types are numerical, check if the values are the same to generate a precise message
                if (ExtensionsCommonHelpers.IsNumerical(expectedType) && ExtensionsCommonHelpers.IsNumerical(instance.GetType()))
                {
                    var changeType = Convert.ChangeType(instance, expectedType);
                    if (expected.Equals(changeType))
                    {
                        var msg = checker.BuildShortMessage("The {0} is not of the expected type, but has the same value than the {1}.");

                        FillEqualityErrorMessage(msg, checker.Value, expected, false);
                        throw new FluentCheckException(msg.ToString());
                    }
                }
            }
#endif
            // Should throw
            var errorMessage = BuildErrorMessage(checker, expected, false);

            throw new FluentCheckException(errorMessage);
        }

        private static bool FluentEquals(object instance, object expected)
        {
            // ReSharper disable once RedundantNameQualifier
            return object.Equals(instance, expected);
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
        /// <returns>
        /// The error message related to the Equality verification.
        /// </returns>
        public static string BuildErrorMessage<T, TU>(IChecker<T, TU> checker, object expected, bool isEqual) where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            var msg = isEqual ? checker.BuildShortMessage("The {0} is equal to the {1} whereas it must not.") : checker.BuildShortMessage("The {0} is different from the {1}.");

            FillEqualityErrorMessage(msg, checker.Value, expected, isEqual);

            return msg.ToString();
        }

        public static void FillEqualityErrorMessage(FluentMessage msg, object instance, object expected, bool negated)
        {
            if (negated)
            {
                msg.Expected(expected).Comparison("different from").WithType();
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
                .WithHashCode(withHash);
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
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public static void IsNotEqualTo<T, TU>(IChecker<T, TU> checker, object expected) where TU : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        {
            if (FluentEquals(checker.Value, expected))
            {
                throw new FluentCheckException(BuildErrorMessage(checker, expected, true));
            }
        }
    }
}