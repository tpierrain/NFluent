// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StringFluentAssertionExtensions.cs" company="">
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

    using NFluent.Helpers;

    /// <summary>
    /// Provides assertion methods to be executed on a string instance.
    /// </summary>
    public static class StringFluentAssertionExtensions
    {
        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<string>> IsEqualTo(this IFluentAssertion<string> fluentAssertion, object expected)
        {
            var runnableAssertion = fluentAssertion as IRunnableAssertion<string>;
            var actual = runnableAssertion.Value;
 
            var messageText = AssessEquals(expected, runnableAssertion.Negated, actual);
            if (!string.IsNullOrEmpty(messageText))
            {
                throw new FluentAssertionException(messageText);
            }

            return new ChainableFluentAssertion<IFluentAssertion<string>>(fluentAssertion);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<string>> IsNotEqualTo(this IFluentAssertion<string> fluentAssertion, object expected)
        {
            var runnableAssertion = fluentAssertion as IRunnableAssertion<string>;
            var actual = runnableAssertion.Value;

            var messageText = AssessEquals(expected, !runnableAssertion.Negated, actual);
            if (!string.IsNullOrEmpty(messageText))
            {
                throw new FluentAssertionException(messageText);
            }

            return new ChainableFluentAssertion<IFluentAssertion<string>>(fluentAssertion);
        }

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<string>> IsInstanceOf<T>(this IFluentAssertion<string> fluentAssertion)
        {
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<string>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<string>;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        IsInstanceHelper.IsInstanceOf(runnableAssertion.Value, typeof(T));
                    },
                IsInstanceHelper.BuildErrorMessage(runnableAssertion, typeof(T), true));
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<string>> IsNotInstanceOf<T>(this IFluentAssertion<string> fluentAssertion)
        {
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<string>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<string>;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        IsInstanceHelper.IsNotInstanceOf(runnableAssertion.Value, typeof(T));
                    },
                IsInstanceHelper.BuildErrorMessage(runnableAssertion, typeof(T), false));
        }

        /// <summary>
        /// Checks that the string contains the given expected values, in any order.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="values">The expected values to be found.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The string does not contains all the given strings in any order.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<string>> Contains(this IFluentAssertion<string> fluentAssertion, params string[] values)
        {
            var runnableAssertion = fluentAssertion as IRunnableAssertion<string>;

            var result = ContainsImpl(runnableAssertion.Value, values, runnableAssertion.Negated);

            if (string.IsNullOrEmpty(result))
            {
                return new ChainableFluentAssertion<IFluentAssertion<string>>(fluentAssertion);
            }

            throw new FluentAssertionException(result);
        }

        private static string AssessEquals(object expected, bool negated, string actual)
        {
            if (EqualityHelper.FluentEquals(actual, expected) != negated)
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
                            if (expectedString.StartsWith(actual))
                            {
                                mainMessage = FluentMessage.BuildMessage(
                                    "The {0} is different from {1}, it is missing the end.");
                            }
                        }
                        else
                        {
                            if (actual.StartsWith(expectedString))
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

                messageText = mainMessage.For("string").On(actual).Expected(expected).ToString();
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
                                 .Expected(values)
                                 .Label("The unauthorized substring(s):")
                                 .ToString();
            }

            return 
                FluentMessage.BuildMessage(
                    "The {0} does not contains the expected value(s): " + items.ToEnumeratedString())
                             .For("string")
                             .On(checkedValue)
                             .Expected(values)
                             .Label("The {0} substring(s):")
                             .ToString();
        }

        /// <summary>
        /// Checks that the string starts with the given expected prefix.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expectedPrefix">The expected prefix.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The string does not start with the expected prefix.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<string>> StartsWith(this IFluentAssertion<string> fluentAssertion, string expectedPrefix)
        {
            var runnableAssertion = fluentAssertion as IRunnableAssertion<string>;

            var result = StartsWithImpl(runnableAssertion.Value, expectedPrefix, runnableAssertion.Negated);
            if (string.IsNullOrEmpty(result))
            {
                return new ChainableFluentAssertion<IFluentAssertion<string>>(fluentAssertion);
            }

            throw new FluentAssertionException(result);
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
                                 .Expected(starts)
                                 .Comparison("does not start with")
                                 .ToString();
            }
  
            return
                FluentMessage.BuildMessage("The {0}'s start is different from the {1}.")
                .For("string")
                             .On(checkedValue)
                             .Expected(starts)
                             .Comparison("starts with")
                             .ToString();
        }
    }
}