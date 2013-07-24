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
    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on a char value.
    /// </summary>
    public static class CharCheckExtensions
    {
        /// <summary>
        /// Checks that the checked <see cref="char"/> is the same as the other, but with different case only.
        /// </summary>
        /// <param name="check">The chained fluent check.</param>
        /// <param name="otherChar">The other char that.</param>
        /// <exception cref="FluentCheckException">The checked <see cref="char"/> is not the same as the expected one, or is the same but with the same case.</exception>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<char>> IsTheSameButWithDifferentCaseAs(this ICheck<char> check, char otherChar)
        {
            var checkRunner = check as ICheckRunner<char>;
            var runnableCheck = check as IRunnableCheck<char>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        char checkedChar = runnableCheck.Value;
                        if (!IsSameCharCaseInsensitive(checkedChar, otherChar) || HaveSameCase(checkedChar, otherChar))
                        {
                            var errorMessage = FluentMessage.BuildMessage("The {0} is not the same but with different case as the given one.").For("char").On(checkedChar).And.WithGivenValue(otherChar).ToString();
                            throw new FluentCheckException(errorMessage);
                        }
                    },
                    FluentMessage.BuildMessage("The {0} is the same as the given one but with different case, whereas it must not.").For("char").On(runnableCheck.Value).And.WithGivenValue(otherChar).ToString());
        }

        private static bool IsSameCharCaseInsensitive(char checkedChar, char otherChar)
        {
            return (char.ToLower(checkedChar).Equals(char.ToLower(otherChar)));
        }

        private static bool HaveSameCase(char checkedChar, char otherChar)
        {
            return (!char.IsLower(checkedChar) || !char.IsUpper(otherChar)) && (!char.IsUpper(checkedChar) || !char.IsLower(otherChar));
        }
    }
}
