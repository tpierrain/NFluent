// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StructCheckExtensions.cs" company="">
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
namespace NFluent
{
    using Extensibility;
    using Helpers;
    using Kernel;

    /// <summary>
    /// Provides check methods to be executed on an struct instance.
    /// </summary>
    public static class StructCheckExtensions
    {
        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <typeparam name="T">Type of the struct or the enum to assert on.</typeparam>
        /// <param name="check">The fluent fluent check.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public static ICheckLink<IStructCheck<T>> IsEqualTo<T>(this IStructCheck<T> check, T expected) where T : struct
        {
            return EqualityHelper.PerformEqualCheck(check, expected);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <typeparam name="T">Type of the struct or the enum to assert on.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is equal to the expected value.</exception>
        public static ICheckLink<IStructCheck<T>> IsNotEqualTo<T>(this IStructCheck<T> check, object expected) where T : struct
        {
            return EqualityHelper.PerformEqualCheck(check.Not, expected);
        }

        /// <summary>
        /// Checks that the actual nullable value has a value and thus, is not null.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is null.</exception>
        public static INullableOrNumberCheckLink<T> HasAValue<T>(this ICheck<T?> check) where T : struct
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("nullable")
                .FailIfNull("The {0} has no value, which is unexpected.")
                .OnNegate("The {0} has a value, whereas it must not.")
                .EndCheck();

            return new NullableOrNumberCheckLink<T>(check);
        }

        /// <summary>
        /// Checks that the actual nullable value has no value and thus, is null. 
        /// Note: this method does not return A check link since the nullable is null.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <exception cref="FluentCheckException">The value is not null.</exception>
        public static void HasNoValue<T>(this ICheck<T?> check) where T : struct
        {
            check.Not.HasAValue();
        }
    }
}
