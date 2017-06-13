// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StringCheckExtensions.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN, Cyrille DUPUYDAUBY
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
#if !DOTNET_30 && !DOTNET_20
    using System.Linq;
#endif
    using System.Text.RegularExpressions;

    using Extensibility;

    using Helpers;

    using Kernel;

    /// <summary>
    /// Provides check methods to be executed on a string instance.
    /// </summary>
    public static class StringCheckExtensions
    {
        /// <summary>
        /// Checks that the checker value is equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checker value is not equal to the expected value.</exception>
        public static ICheckLink<ICheck<string>> IsEqualTo(this ICheck<string> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var messageText = AssessEquals(checker, expected, checker.Negated);
            if (!string.IsNullOrEmpty(messageText))
            {
                throw new FluentCheckException(messageText);
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        /// Checks that the checker value is equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checker value is not equal to the expected value.</exception>
        public static ICheckLink<ICheck<string>> IsEqualTo(this ICheck<string> check, string expected)
        {
            return IsEqualTo(check, (object)expected);
        }

        /// <summary>
        /// Checks that the checker value is not equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checker value is equal to the expected value.</exception>
        public static ICheckLink<ICheck<string>> IsNotEqualTo(this ICheck<string> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var messageText = AssessEquals(checker, expected, !checker.Negated);
            if (!string.IsNullOrEmpty(messageText))
            {
                throw new FluentCheckException(messageText);
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        /// Checks that the checker value is not equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checker value is equal to the expected value.</exception>
        public static ICheckLink<ICheck<string>> IsNotEqualTo(this ICheck<string> check, string expected)
        {
            return IsNotEqualTo(check, (object)expected);
        }

        /// <summary>
        /// Checks that the checker value is one of these possible elements.
        /// </summary>
        /// <param name="check">The check.</param>
        /// <param name="possibleElements">The possible elements.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checker value is NOT one of the elements.</exception>
        public static ICheckLink<ICheck<string>> IsOneOfThese(this ICheck<string> check, params string[] possibleElements)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        string errorMessage;
                        if (possibleElements == null)
                        {
                            // the rare case where possible elements is null
                            if (checker.Value == null)
                            {
                                return;
                            }
                            else
                            {
                                errorMessage =
                                    checker.BuildMessage("The {0} is not one of the possible elements.")
                                        .On(checker.Value)
                                        .And.Expected(null/*possibleElements*/)
                                        .Label("The possible elements:")
                                        .ToString();
                                throw new FluentCheckException(errorMessage);
                            }
                        }

                        if (possibleElements.Any(possibleElement => string.Equals(possibleElement, checker.Value)))
                        {
                            return;
                        }

                        errorMessage =
                            checker.BuildMessage("The {0} is not one of the possible elements.")
                                .Expected(possibleElements)
                                .Label("The possible elements:")
                                .ToString();
                        throw new FluentCheckException(errorMessage);
                    },
                checker.BuildMessage("The {0} is one of the possible elements whereas it must not.").Expected(possibleElements).Label("The possible elements:").ToString());
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
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = ContainsImpl(checker, values, checker.Negated, false);

            if (string.IsNullOrEmpty(result))
            {
                return ExtensibilityHelper.BuildExtendableCheckLink(check, values);
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
        public static ICheckLink<ICheck<string>> DoesNotContain(this ICheck<string> check, params string[] values)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = ContainsImpl(checker, values, checker.Negated, true);

            if (string.IsNullOrEmpty(result))
            {
                return checker.BuildChainingObject();
            }

            throw new FluentCheckException(result);
        }

        private static string AssessEquals(IChecker<string, ICheck<string>> checker, object expected, bool negated, bool ignoreCase = false)
        {
            var value = checker.Value;

            var analysis = StringDifference.Analyze(value, (string)expected, ignoreCase);

            if (negated == (analysis != null && analysis.Count > 0))
            {
                return null;
            }

            if (negated)
            {
                return 
                    checker.BuildShortMessage("The {0} is equal to the {1} whereas it must not.").Expected(expected).Comparison("different from").ToString();
            }

            if (value == null)
            {
                return checker.BuildShortMessage("The {0} is null whereas it must not.").For(typeof(string)).On(null/*value*/).And.Expected(expected).ToString();
            }

            if (expected == null)
            {
                return checker.BuildShortMessage("The {0} is not null whereas it must.").For(typeof(string)).On(value).And.Expected(null).ToString();
            }

            var summary = StringDifference.Summarize(analysis);
            var message = checker.BuildShortMessage(string.Empty);
            analysis[0].FillMessage(message, summary);

            // we try to refine the difference
            return message.ToString();
        }

        private static string ContainsImpl(IChecker<string, ICheck<string>> checker, IEnumerable<string> values, bool negated, bool notContains)
        {
            var checkedValue = checker.Value;

            if (checkedValue == null)
            {
                return negated || notContains
                           ? null
                           : checker.BuildShortMessage("The {0} is null.").For(typeof(string)).ReferenceValues(values).Label("The {0} substring(s):").ToString();
            }

            var items = values.ToList().Where(item => checkedValue.Contains(item) == notContains).ToList();

            if (negated == items.Count > 0)
            {
                return null;
            }

            if (!notContains && negated)
            {
                items = values.ToList();
            }

            if (negated != notContains)
            {
                return
                    checker.BuildMessage("The {0} contains unauthorized value(s): " + items.ToEnumeratedString())
                        .ReferenceValues(values)
                        .Label("The unauthorized substring(s):")
                        .ToString();
            }

            return
                checker.BuildMessage("The {0} does not contains the expected value(s): " + items.ToEnumeratedString())
                    .ReferenceValues(values)
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
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = StartsWithImpl(checker, expectedPrefix, checker.Negated);
            if (string.IsNullOrEmpty(result))
            {
                return checker.BuildChainingObject();
            }

            throw new FluentCheckException(result);
        }

        private static string StartsWithImpl(IChecker<string, ICheck<string>> checker, string starts, bool negated)
        {
            var checkedValue = checker.Value;

            // special case if checkedvalue is null
            if (checkedValue == null)
            {
                return negated ? null : checker.BuildShortMessage("The {0} is null.").Expected(starts).Comparison("starts with").ToString();
            }

            if (checkedValue.StartsWith(starts) != negated)
            {
                // success
                return null;
            }

            if (negated)
            {
                return checker.BuildMessage("The {0} starts with {1}, whereas it must not.").Expected(starts).Comparison("does not start with").ToString();
            }

            return checker.BuildMessage("The {0}'s start is different from the {1}.").Expected(starts).Comparison("starts with").ToString();
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
        public static ICheckLink<ICheck<string>> EndsWith(this ICheck<string> check, string expectedEnd)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = EndsWithImpl(checker, expectedEnd, checker.Negated);
            if (string.IsNullOrEmpty(result))
            {
                return checker.BuildChainingObject();
            }

            throw new FluentCheckException(result);
        }

        private static string EndsWithImpl(IChecker<string, ICheck<string>> checker, string ends, bool negated)
        {
            var checkedValue = checker.Value;

            // special case if checkedvalue is null
            if (checkedValue == null)
            {
                return negated ? null : checker.BuildShortMessage("The {0} is null.").Expected(ends).Comparison("ends with").ToString();
            }

            if (checkedValue.EndsWith(ends) != negated)
            {
                // success
                return null;
            }

            if (negated)
            {
                return checker.BuildMessage("The {0} ends with {1}, whereas it must not.").Expected(ends).Comparison("does not end with").ToString();
            }

            return checker.BuildMessage("The {0}'s end is different from the {1}.").Expected(ends).Comparison("ends with").ToString();
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
        public static ICheckLink<ICheck<string>> Matches(this ICheck<string> check, string regExp)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = MatchesImpl(checker, regExp, checker.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return checker.BuildChainingObject();
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
        public static ICheckLink<ICheck<string>> DoesNotMatch(this ICheck<string> check, string regExp)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = MatchesImpl(checker, regExp, !checker.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return checker.BuildChainingObject();
        }

        private static string MatchesImpl(IChecker<string, ICheck<string>> checker, string regExp, bool negated)
        {
            var checkedValue = checker.Value;

            // special case if checkedvalue is null
            if (checkedValue == null)
            {
                return negated ? null : checker.BuildShortMessage("The {0} is null.").Expected(regExp).Comparison("matches").ToString();
            }

            var exp = new Regex(regExp);
            if (exp.IsMatch(checkedValue) != negated)
            {
                // success
                return null;
            }

            if (negated)
            {
                return checker.BuildMessage("The {0} matches {1}, whereas it must not.").Expected(regExp).Comparison("does not match").ToString();
            }

            return checker.BuildMessage("The {0} does not match the {1}.").Expected(regExp).Comparison("matches").ToString();
        }

        /// <summary>
        /// Checks that the string is empty.
        /// </summary>
        /// <param name="check">The fluent check.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string is not empty.</exception>
        public static ICheckLink<ICheck<string>> IsEmpty(this ICheck<string> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = IsEmptyImpl(checker, false, checker.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return checker.BuildChainingObject();
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
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = IsEmptyImpl(checker, true, checker.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        /// Checks that the string is not empty.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The string is empty.</exception>
        public static ICheckLink<ICheck<string>> IsNotEmpty(this ICheck<string> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = IsEmptyImpl(checker, false, !checker.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return checker.BuildChainingObject();
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
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = IsEmptyImpl(checker, true, !checker.Negated);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return checker.BuildChainingObject();
        }

        private static string IsEmptyImpl(IChecker<string, ICheck<string>> checker, bool canBeNull, bool negated)
        {
            var checkedValue = checker.Value;

            // special case if checkedValue is null
            if (checkedValue == null)
            {
                if (canBeNull != negated)
                {
                    return null;
                }

                return negated
                           ? checker.BuildShortMessage("The {0} is null whereas it must have content.").For(typeof(string)).ToString()
                           : checker.BuildShortMessage("The {0} is null instead of being empty.").For(typeof(string)).ToString();
            }

            if (string.IsNullOrEmpty(checkedValue) != negated)
            {
                // success
                return null;
            }

            if (negated)
            {
                return checker.BuildShortMessage("The {0} is empty, whereas it must not.").For(typeof(string)).ToString();
            }

            return checker.BuildMessage("The {0} is not empty or null.").On(checkedValue).ToString();
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
        public static ICheckLink<ICheck<string>> IsEqualIgnoringCase(this ICheck<string> check, string comparand)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            var result = AssessEquals(checker, comparand, checker.Negated, true);
            if (!string.IsNullOrEmpty(result))
            {
                throw new FluentCheckException(result);
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        /// Convert a string to an array of lines.
        /// </summary>
        /// <param name="check">The fluent check to be processed.</param>
        /// <returns>A checker.</returns>
        public static ICheck<IEnumerable<string>> AsLines(this ICheck<string> check)
        {
            IEnumerable<string> next;
            var checker = ExtensibilityHelper.ExtractChecker(check);
            if (checker.Value != null)
            {
                var lines = checker.Value.Split(Environment.NewLine[0]);
                if (Environment.NewLine.Length > 1)
                {
                    for (var i = 0; i < lines.Length; i++)
                    {
                        lines[i] = lines[i].Trim(Environment.NewLine[1]);
                    }
                }

                next = lines;
            }
            else
            {
                next = new List<string>();
            }

            return new FluentCheck<IEnumerable<string>>(next);
        }
    }
}