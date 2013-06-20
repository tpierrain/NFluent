// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StringCheckExtensions.cs" company="">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on a string instance.
    /// </summary>
    public static class StringCheckExtensions
    {
        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public static ICheckLink<ICheck<string>> IsEqualTo(this ICheck<string> check, object expected)
        {
            var runnableCheck = check as IRunnableCheck<string>;
            var actual = runnableCheck.Value;
 
            var messageText = AssessEquals(actual, expected, runnableCheck.Negated);
            if (!string.IsNullOrEmpty(messageText))
            {
                throw new FluentCheckException(messageText);
            }

            return new CheckLink<ICheck<string>>(check);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is equal to the expected value.</exception>
        public static ICheckLink<ICheck<string>> IsNotEqualTo(this ICheck<string> check, object expected)
        {
            var runnableCheck = check as IRunnableCheck<string>;
            var actual = runnableCheck.Value;

            var messageText = AssessEquals(actual, expected, !runnableCheck.Negated);
            if (!string.IsNullOrEmpty(messageText))
            {
                throw new FluentCheckException(messageText);
            }

            return new CheckLink<ICheck<string>>(check);
        }

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual instance is not of the provided type.</exception>
        public static ICheckLink<ICheck<string>> IsInstanceOf<T>(this ICheck<string> check)
        {
            var checkRunner = check as ICheckRunner<string>;
            var runnableCheck = check as IRunnableCheck<string>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        IsInstanceHelper.IsInstanceOf(runnableCheck.Value, typeof(T));
                    },
                IsInstanceHelper.BuildErrorMessage(runnableCheck, typeof(T), true));
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual instance is of the provided type.</exception>
        public static ICheckLink<ICheck<string>> IsNotInstanceOf<T>(this ICheck<string> check)
        {
            var checkRunner = check as ICheckRunner<string>;
            var runnableCheck = check as IRunnableCheck<string>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        IsInstanceHelper.IsNotInstanceOf(runnableCheck.Value, typeof(T));
                    },
                IsInstanceHelper.BuildErrorMessage(runnableCheck, typeof(T), false));
        }

        /// <summary>
        /// Checks that the string contains the given expected values, in any order.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="values">The expected values to be found.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string  contains all the given strings in any order.</exception>
        public static IExtendableCheckLink<string, string[]> Contains(this ICheck<string> check, params string[] values)
        {
            var runnableCheck = check as IRunnableCheck<string>;

            var result = ContainsImpl(runnableCheck.Value, values, runnableCheck.Negated);

            if (string.IsNullOrEmpty(result))
            {
                return new ExtendableCheckLink<string, string[]>(check, values);
            }

            throw new FluentCheckException(result);
        }

            /// <summary>
            /// Checks that the string does not contain any of the given expected values.
            /// </summary>
            /// <param name="check">The fluent check to be extended.</param>
            /// <param name="values">The values not to be present.</param>
            /// <returns>
            /// A check link.
            /// </returns>
            /// <exception cref="FluentCheckException">The string contains at least one of the given strings.</exception>
        public static ICheckLink<ICheck<string>> DoesNotContain(
                this ICheck<string> check, params string[] values)
        {    
            var runnableCheck = check as IRunnableCheck<string>;

            var result = ContainsImpl(runnableCheck.Value, values, !runnableCheck.Negated);

            if (string.IsNullOrEmpty(result))
            {
                return new CheckLink<ICheck<string>>(check);
            }

            throw new FluentCheckException(result);
        }

        private static string AssessEquals(string actual, object expected, bool negated, bool ignoreCase = false)
        {
            if (string.Equals(actual, (string)expected, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture) != negated)
            {
                return null;
            }

            string messageText;
            if (negated)
            {
                messageText =
                    FluentMessage.BuildMessage("The {0} is equal to the {1} whereas it must not.")
                                 .Expected(expected)
                                 .Comparison("different from")
                                 .ToString();
            }
            else
            {
                // we try to refine the difference
                // should have been different
                var expectedString = expected as string;
                FluentMessage mainMessage = null;
                if (expectedString != null && actual != null)
                {
                    if (expectedString.Length == actual.Length)
                    {
                        // same length
                        mainMessage =
                            FluentMessage.BuildMessage(
                                string.Compare(actual, expectedString, StringComparison.CurrentCultureIgnoreCase) == 0
                                    ? "The {0} is different from the {1} but only in case."
                                    : "The {0} is different from the {1} but has same length.");
                    }
                    else
                    {
                        if (expectedString.Length > actual.Length)
                        {   
                            if (expectedString.StartsWith(actual, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture))
                            {
                                mainMessage = FluentMessage.BuildMessage(
                                    "The {0} is different from {1}, it is missing the end.");
                            }
                        }
                        else
                        {
                            if (actual.StartsWith(expectedString, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture))
                            {
                                mainMessage =
                                    FluentMessage.BuildMessage(
                                        "The {0} is different from {1}, it contains extra text at the end.");
                            }
                        }
                    }
                }

                if (mainMessage == null)
                {
                    mainMessage = FluentMessage.BuildMessage("The {0} is different from {1}.");
                }

                messageText = mainMessage.For("string").On(actual).And.Expected(expected).ToString();
            }

            return messageText;
        }

        private static string ContainsImpl(string checkedValue, IEnumerable<string> values, bool negated)
        {
            // special case if checkedvalue is null
            if (checkedValue == null)
            {
                return negated ? null : FluentMessage.BuildMessage("The {0} is null.").Expected(values).Label("The {0} substrin(s):").ToString();
            }

            var items = values.Where(item => checkedValue.Contains(item) == negated).ToList();

            if (items.Count == 0)
            {
                return null;
            }

            if (negated)
            {
                return 
                    FluentMessage.BuildMessage(
                        "The {0} contains unauthorized value(s): " + items.ToEnumeratedString())
                                 .For("string")
                                 .On(checkedValue)
                                 .And.Expected(values)
                                 .Label("The unauthorized substring(s):")
                                 .ToString();
            }

            return 
                FluentMessage.BuildMessage(
                    "The {0} does not contains the expected value(s): " + items.ToEnumeratedString())
                             .For("string")
                             .On(checkedValue)
                             .And.Expected(values)
                             .Label("The {0} substring(s):")
                             .ToString();
        }

        /// <summary>
        /// Checks that the string starts with the given expected prefix.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedPrefix">The expected prefix.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string does not start with the expected prefix.</exception>
        public static ICheckLink<ICheck<string>> StartsWith(this ICheck<string> check, string expectedPrefix)
        {
            var runnableCheck = check as IRunnableCheck<string>;

            var result = StartsWithImpl(runnableCheck.Value, expectedPrefix, runnableCheck.Negated);
            if (string.IsNullOrEmpty(result))
            {
                return new CheckLink<ICheck<string>>(check);
            }

            throw new FluentCheckException(result);
        }

        private static string StartsWithImpl(string checkedValue, string starts, bool negated)
        {
            // special case if checkedvalue is null
            if (checkedValue == null)
            {
                return negated ? null : FluentMessage.BuildMessage("The {0} is null.").Expected(starts).Comparison("starts with").ToString();
            }
            
            if (checkedValue.StartsWith(starts) != negated)
            {
                // success
                return null;
            }

            if (negated)
            {
                return
                    FluentMessage.BuildMessage("The {0} starts with {1}, whereas it must not.")
                    .For("string")
                                 .On(checkedValue)
                                 .And.Expected(starts)
                                 .Comparison("does not start with")
                                 .ToString();
            }
  
            return
                FluentMessage.BuildMessage("The {0}'s start is different from the {1}.")
                .For("string")
                             .On(checkedValue)
                             .And.Expected(starts)
                             .Comparison("starts with")
                             .ToString();
        }

        /// <summary>
        /// Checks that the string ends with the given expected suffix.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedEnd">The expected suffix.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string does not end with the expected prefix.</exception>
        public static ICheckLink<ICheck<string>> EndsWith(
            this ICheck<string> check, string expectedEnd)
        {
            var runnableCheck = check as IRunnableCheck<string>;

            var result = EndsWithImpl(runnableCheck.Value, expectedEnd, runnableCheck.Negated);
            if (string.IsNullOrEmpty(result))
            {
                return new CheckLink<ICheck<string>>(check);
            }

            throw new FluentCheckException(result);
        }

        private static string EndsWithImpl(string checkedValue, string ends, bool negated)
        {
            // special case if checkedvalue is null
            if (checkedValue == null)
            {
                return negated ? null : FluentMessage.BuildMessage("The {0} is null.").Expected(ends).Comparison("ends with").ToString();
            }

            if (checkedValue.EndsWith(ends) != negated)
            {
                // success
                return null;
            }

            if (negated)
            {
                return
                    FluentMessage.BuildMessage("The {0} ends with {1}, whereas it must not.")
                    .For("string")
                                 .On(checkedValue)
                                 .And.Expected(ends)
                                 .Comparison("does not end with")
                                 .ToString();
            }

            return
                FluentMessage.BuildMessage("The {0}'s end is different from the {1}.")
                .For("string")
                             .On(checkedValue)
                             .And.Expected(ends)
                             .Comparison("ends with")
                             .ToString();
        }

        /// <summary>
        /// Checks that the string matches a given regular expression.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="regExp">The regular expression.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string does not end with the expected prefix.</exception>
        public static ICheckLink<ICheck<string>> Matches(
            this ICheck<string> check, string regExp)
        {
            var runnableCheck = check as IRunnableCheck<string>;

            var result = MatchesImpl(runnableCheck.Value, regExp, runnableCheck.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return new CheckLink<ICheck<string>>(check);
        }

        /// <summary>
        /// Checks that the string does not match a given regular expression.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="regExp">The regular expression prefix.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string does not end with the expected prefix.</exception>
        public static ICheckLink<ICheck<string>> DoesNotMatch(
            this ICheck<string> check, string regExp)
        {
            var runnableCheck = check as IRunnableCheck<string>;

            var result = MatchesImpl(runnableCheck.Value, regExp, !runnableCheck.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return new CheckLink<ICheck<string>>(check);
        }
        
        private static string MatchesImpl(string checkedValue, string regExp, bool negated)
        {
            // special case if checkedvalue is null
            if (checkedValue == null)
            {
                return negated ? null : FluentMessage.BuildMessage("The {0} is null.").Expected(regExp).Comparison("matches").ToString();
            }

            Regex exp = new Regex(regExp);
            if (exp.IsMatch(checkedValue) != negated)
            {
                // success
                return null;
            }

            if (negated)
            {
                return
                    FluentMessage.BuildMessage("The {0} matches {1}, whereas it must not.")
                    .For("string")
                                 .On(checkedValue)
                                 .And.Expected(regExp)
                                 .Comparison("does not match")
                                 .ToString();
            }

            return
                FluentMessage.BuildMessage("The {0} does not match the {1}.")
                .For("string")
                             .On(checkedValue)
                             .And.Expected(regExp)
                             .Comparison("matches")
                             .ToString();
        }

        /// <summary>
        /// Checks that the string is empty.
        /// </summary>
        /// <param name="check">The fluent check.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string is not empty.</exception>
        public static ICheckLink<ICheck<string>> IsEmpty(
            this ICheck<string> check)
        {
            var runnableCheck = check as IRunnableCheck<string>;

            var result = IsEmptyImpl(runnableCheck.Value, false, runnableCheck.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return new CheckLink<ICheck<string>>(check);
        }

        /// <summary>
        /// Checks that the string is empty or null.
        /// </summary>
        /// <param name="check">The fluent check.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string is neither empty or null.</exception>
        public static ICheckLink<ICheck<string>> IsNullOrEmpty(this ICheck<string> check)
        {
            var runnableCheck = check as IRunnableCheck<string>;

            var result = IsEmptyImpl(runnableCheck.Value, true, runnableCheck.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return new CheckLink<ICheck<string>>(check);            
        }

        /// <summary>
        /// Checks that the string is not empty.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string is empty.</exception>
        public static ICheckLink<ICheck<string>> IsNotEmpty(
            this ICheck<string> check)
        {
            var runnableCheck = check as IRunnableCheck<string>;

            var result = IsEmptyImpl(runnableCheck.Value, false, !runnableCheck.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return new CheckLink<ICheck<string>>(check);
        }

        /// <summary>
        /// Checks that the string has content.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string is empty or null.</exception>
        public static ICheckLink<ICheck<string>> HasContent(this ICheck<string> check)
        {
            var runnableCheck = check as IRunnableCheck<string>;

            var result = IsEmptyImpl(runnableCheck.Value, true, !runnableCheck.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return new CheckLink<ICheck<string>>(check);
        }

        private static string IsEmptyImpl(string checkedValue, bool canBeNull, bool negated)
        {
            // special case if checkedvalue is null
            if (checkedValue == null)
            {
                if (canBeNull != negated)
                {
                    return null;
                }

                return negated ? FluentMessage.BuildMessage("The {0} is null whereas it must have content.").For("string").ToString()
                    : FluentMessage.BuildMessage("The {0} is null instead of being empty.").For("string").ToString();
            }

            if (string.IsNullOrEmpty(checkedValue) != negated)
            {
                // success
                return null;
            }

            if (negated)
            {
                return
                    FluentMessage.BuildMessage("The {0} is empty, whereas it must not.")
                    .For("string")
                                 .ToString();
            }

            return
                FluentMessage.BuildMessage("The {0} is not empty or null.")
                .For("string")
                             .On(checkedValue)
                             .ToString();
        }

        /// <summary>
        /// Checks that the string is equals to another one, disregarding case.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="comparand">The string to compare to.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string is not equal to the comparand.</exception>
        public static ICheckLink<ICheck<string>> IsEqualIgnoringCase(
            this ICheck<string> check, string comparand)
        {
            var runnableCheck = check as IRunnableCheck<string>;

            var result = AssessEquals(runnableCheck.Value, comparand, runnableCheck.Negated, true);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return new CheckLink<ICheck<string>>(check);
        }
    }
}