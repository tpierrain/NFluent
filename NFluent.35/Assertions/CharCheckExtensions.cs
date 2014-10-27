// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="CharCheckExtensions.cs" company="">
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

    /// <summary>
    /// Provides check methods to be executed on a char value.
    /// </summary>
    public static class CharCheckExtensions
    {
        private const string EntityDescription = "char";

        /// <summary>
        /// Checks that the checked <see cref="char"/> is a letter.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <exception cref="FluentCheckException">The checked <see cref="char"/> is not a letter.</exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsALetter(this ICheck<char> check)
        {
            // Every check method starts by extracting a checker instance from the check thanks to
            // the ExtensibilityHelper static class.
            var checker = ExtensibilityHelper.ExtractChecker(check);

            // Then, we let the checker's ExecuteCheck() method return the ICheckLink<ICheck<T>> result (with T as string here).
            // This method needs 2 arguments:
            //   1- a lambda that checks what's necessary, and throws a FluentAssertionException in case of failure
            //      The exception message is usually fluently build with the FluentMessage.BuildMessage() static method.
            //
            //   2- a string containing the message for the exception to be thrown by the checker when 
            //      the check fails, in the case we were running the negated version.
            //
            // e.g.:
            return checker.ExecuteCheck(
                () =>
                    {
                        if (!IsALetter(checker.Value))
                        {
                            throw new FluentCheckException(
                                checker.BuildMessage("The {0} is not a letter.").For(EntityDescription).ToString());
                        }
                    },
                checker.BuildMessage("The {0} is a letter whereas it must not.").For(EntityDescription).ToString());
        }

        /// <summary>
        /// Checks that the checked <see cref="char"/> is a decimal digit.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <exception cref="FluentCheckException">The checked <see cref="char"/> is not a decimal digit.</exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsADigit(this ICheck<char> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (!char.IsDigit(checker.Value))
                    {
                        throw new FluentCheckException(checker.BuildMessage("The {0} is not a decimal digit.").For(EntityDescription).ToString());
                    }
                },
                checker.BuildMessage("The {0} is a decimal digit whereas it must not.").For(EntityDescription).ToString());
        }

        /// <summary>
        /// Checks that the checked <see cref="char"/> is a punctuation mark.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <exception cref="FluentCheckException">The checked <see cref="char"/> is not a punctuation mark.</exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsAPunctuationMark(this ICheck<char> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (!char.IsPunctuation(checker.Value))
                    {
                        throw new FluentCheckException(checker.BuildMessage("The {0} is not a punctuation mark.").For(EntityDescription).ToString());
                    }
                },
                checker.BuildMessage("The {0} is a punctuation mark whereas it must not.").For(EntityDescription).ToString());
        }

        /// <summary>
        /// Checks that the checked <see cref="char"/> and the given one are the same letter, whatever the case.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <param name="otherChar">The other char that.</param>
        /// <exception cref="FluentCheckException">The checked <see cref="char"/> is not the same letter as the expected one, whatever the case.</exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsSameLetterAs(this ICheck<char> check, char otherChar)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (!IsALetter(checker.Value))
                    {
                        var errorMessage = checker.BuildMessage("The {0} is not the same letter as the given one (whatever the case).\nThe checked char is not even a letter!").For(EntityDescription).WithGivenValue(otherChar).ToString();
                        throw new FluentCheckException(errorMessage);
                    }

                    if (!IsSameCharCaseInsensitive(checker.Value, otherChar))
                    {
                        var errorMessage = checker.BuildMessage("The {0} is not the same letter as the given one (whatever the case).").For(EntityDescription).WithGivenValue(otherChar).ToString();
                        throw new FluentCheckException(errorMessage);
                    }
                },
                checker.BuildMessage("The {0} is the same letter as the given one (whatever the case), whereas it must not.").For(EntityDescription).WithGivenValue(otherChar).ToString());
        }

        /// <summary>
        /// Checks that the checked <see cref="char"/> is the same letter as the other, but with different case only.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <param name="otherChar">The other char that.</param>
        /// <exception cref="FluentCheckException">The checked <see cref="char"/> is not the same as the expected one, or is the same but with the same case.</exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsSameLetterButWithDifferentCaseAs(this ICheck<char> check, char otherChar)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            
            return checker.ExecuteCheck(
                () =>
                    {
                        if (!IsALetter(checker.Value) || !IsSameCharCaseInsensitive(checker.Value, otherChar) || HaveSameCase(checker.Value, otherChar))
                        {
                            var errorMessage = checker.BuildMessage("The {0} is not the same letter but with different case as the given one.").For(EntityDescription).WithGivenValue(otherChar).ToString();
                            throw new FluentCheckException(errorMessage);
                        }
                    },
                    checker.BuildMessage("The {0} is the same letter as the given one but with different case, whereas it must not.").For(EntityDescription).WithGivenValue(otherChar).ToString());
        }

        #region helper methods

        private static bool IsALetter(char checkedChar)
        {
            return char.IsLetter(checkedChar);
        }

        private static bool IsSameCharCaseInsensitive(char checkedChar, char otherChar)
        {
            return char.ToLower(checkedChar).Equals(char.ToLower(otherChar));
        }

        private static bool HaveSameCase(char checkedChar, char otherChar)
        {
            return (!char.IsLower(checkedChar) || !char.IsUpper(otherChar)) && (!char.IsUpper(checkedChar) || !char.IsLower(otherChar));
        }

        #endregion
    }
}
