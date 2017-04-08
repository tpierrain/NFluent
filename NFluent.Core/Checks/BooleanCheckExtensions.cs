// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="BooleanCheckExtensions.cs" company="">
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

    /// <summary>
    /// Provides check methods to be executed on a boolean value.
    /// </summary>
    public static class BooleanCheckExtensions
    {
        #region fields

        // message when the value must be false
        private const string MustBeFalseMessage = "The {0} is true whereas it must be false.";

        // message when the value must be true
        private const string MustBeTrueMessage = "The {0} is false whereas it must be true.";

        #endregion

        #region methods

        /// <summary>
        /// Checks that the actual value is true.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not true.</exception>
        public static ICheckLink<ICheck<bool>> IsTrue(this ICheck<bool> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (!checker.Value)
                        {
                            var message =
                                checker.BuildMessage(MustBeTrueMessage).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage(MustBeFalseMessage).ToString());
        }

        /// <summary>
        /// Checks that the actual value is false.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not false.</exception>
        public static ICheckLink<ICheck<bool>> IsFalse(this ICheck<bool> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value)
                        {
                            throw new FluentCheckException(
                                checker.BuildMessage(MustBeFalseMessage).ToString());
                        }
                    },
                checker.BuildMessage(MustBeTrueMessage).ToString());
        }
    }

    #endregion
}