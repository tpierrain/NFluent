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
    using NFluent.Extensibility;
    using NFluent.Helpers;

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
            var runnableStructCheck = ExtensibilityHelper.ExtractStructChecker(check);

            return runnableStructCheck.ExecuteCheck(
                () =>
                {
                    EqualityHelper.IsEqualTo(runnableStructCheck.Value, expected);
                },
                EqualityHelper.BuildErrorMessage(runnableStructCheck.Value, expected, true));
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
            var runnableStructCheck = ExtensibilityHelper.ExtractStructChecker(check);

            return runnableStructCheck.ExecuteCheck(
                () =>
                    {
                        EqualityHelper.IsNotEqualTo(runnableStructCheck.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableStructCheck.Value, expected, false));
        }
    }
}
