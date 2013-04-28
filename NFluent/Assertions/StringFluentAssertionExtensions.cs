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
    using System.Collections.Generic;

    using NFluent.Extensions;
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
            EqualityHelper.IsEqualTo(fluentAssertion.Value, expected);
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
            EqualityHelper.IsNotEqualTo(fluentAssertion.Value, expected);
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
            IsInstanceHelper.IsInstanceOf(fluentAssertion.Value, typeof(T));
            return new ChainableFluentAssertion<IFluentAssertion<string>>(fluentAssertion);
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
            IsInstanceHelper.IsNotInstanceOf(fluentAssertion.Value, typeof(T));
            return new ChainableFluentAssertion<IFluentAssertion<string>>(fluentAssertion);
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
            if (!fluentAssertion.Negated)
            {
                var notFound = new List<string>();
                foreach (string value in values)
                {
                    if (!fluentAssertion.Value.Contains(value))
                    {
                        notFound.Add(value);
                    }
                }

                if (notFound.Count > 0)
                {
                    throw new FluentAssertionException(string.Format("\nThe actual string:\n\t[{0}]\ndoes not contain the expected value(s):\n\t[{1}].", fluentAssertion.Value.ToStringProperlyFormated(), notFound.ToEnumeratedString()));
                }
            }
            else
            {
                var foundItems = new List<string>();
                foreach (string value in values)
                {
                    if (fluentAssertion.Value.Contains(value))
                    {
                        foundItems.Add(value);
                    }
                }

                if (foundItems.Count > 0)
                {
                    throw new FluentAssertionException(string.Format("\nThe actual string:\n\t[{0}]\n contains the value(s):\n\t[{1}]\nwhich was not expected.", fluentAssertion.Value.ToStringProperlyFormated(), foundItems.ToEnumeratedString()));
                }
            }
            
            return new ChainableFluentAssertion<IFluentAssertion<string>>(fluentAssertion);
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
            if (!fluentAssertion.Value.StartsWith(expectedPrefix))
            {
                throw new FluentAssertionException(string.Format("\nThe actual string:\n\t[{0}]\ndoes not start with:\n\t[{1}].", fluentAssertion.Value.ToStringProperlyFormated(), expectedPrefix.ToStringProperlyFormated()));
            }

            return new ChainableFluentAssertion<IFluentAssertion<string>>(fluentAssertion);
        }
    }
}