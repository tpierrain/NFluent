// -------------------------------------------------------------------------------------------------------------------
// <copyright file="CharCheckExtensions.cs" company="">
//   Copyright 2017 Cyrille Dupuydauby
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent
{
    using System;

    using Extensibility;
    using Kernel;

    /// <summary>
    ///     Provides check methods to be executed on a char value.
    /// </summary>
    public static class CharCheckExtensions
    {
        /// <summary>
        /// Checks that the checked <see cref="char" /> is a letter.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <exception cref="FluentCheckException">The checked <see cref="char" /> is not a letter.</exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsALetter(this ICheck<char> check)
        {
            ExtensibilityHelper.BeginCheck(check).
                FailWhen(sut => !IsALetter(sut), "The {0} is not a letter.").
                OnNegate("The {0} is a letter whereas it must not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Checks that the checked <see cref="char" /> is a decimal digit.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <exception cref="FluentCheckException">The checked <see cref="char" /> is not a decimal digit.</exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsADigit(this ICheck<char> check)
        {
            ExtensibilityHelper.BeginCheck(check).
                FailWhen(sut => !char.IsDigit(sut), "The {0} is not a decimal digit.").
                OnNegate("The {0} is a decimal digit whereas it must not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that the checked <see cref="char" /> is a punctuation mark.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <exception cref="FluentCheckException">The checked <see cref="char" /> is not a punctuation mark.</exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsAPunctuationMark(this ICheck<char> check)
        {
            ExtensibilityHelper.BeginCheck(check).
                FailWhen(sut => !char.IsPunctuation(sut), "The {0} is not a punctuation mark.").
                OnNegate("The {0} is a punctuation mark whereas it must not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Checks that the checked <see cref="char" /> and the given one are the same letter, whatever the case.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <param name="otherChar">The other char that.</param>
        /// <exception cref="FluentCheckException">
        ///     The checked <see cref="char" /> is not the same letter as the expected one,
        ///     whatever the case.
        /// </exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsSameLetterAs(this ICheck<char> check, char otherChar)
        {
            ExtensibilityHelper.BeginCheck(check).
                DefineExpectedValue(otherChar).
                FailWhen(sut => !IsALetter(sut), "The {0} is not the same letter as the {1} (whatever the case)."+ Environment.NewLine + "The {0} is not even a letter!").
                FailWhen(sut => !IsSameCharCaseInsensitive(sut, otherChar), "The {0} is not the same letter as the {1} (whatever the case).").
                OnNegate("The {0} is the same letter as the {1} (whatever the case), whereas it must not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        ///     Checks that the checked <see cref="char" /> is the same letter as the other, but with different case only.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <param name="otherChar">The other char that.</param>
        /// <exception cref="FluentCheckException">
        ///     The checked <see cref="char" /> is not the same as the expected one, or is the
        ///     same but with the same case.
        /// </exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsSameLetterButWithDifferentCaseAs(
            this ICheck<char> check,
            char otherChar)
        {
            ExtensibilityHelper.BeginCheck(check).
                DefineExpectedValue(otherChar).
                FailWhen(sut => !IsALetter(sut), "The {0} is not a letter, where as it must.").
                FailWhen(sut => !IsSameCharCaseInsensitive(sut, otherChar), "The {0} is different from the expected letter.").
                FailWhen(sut => HaveSameCase(sut, otherChar), "The {0} is the same letter, but must have different case than the {1}.").
                OnNegate("The {0} is the same letter as the {1} with different case, whereas it must not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }


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
            return (!char.IsLower(checkedChar) || !char.IsUpper(otherChar))
                   && (!char.IsUpper(checkedChar) || !char.IsLower(otherChar));
        }
    }
}