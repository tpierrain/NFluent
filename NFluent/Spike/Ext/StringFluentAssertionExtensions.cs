// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StringFluentAssertionExtensions.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN, Rui CARVALHO
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
namespace Spike.Ext
{
    using NFluent;

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
        public static IChainableFluentAssertion<IStringFluentAssertion> IsEqualTo(this IFluentAssertion<string> fluentAssertion, object expected)
        {
            var assertStrategy = new StringFluentAssertion(fluentAssertion.Value);
            return assertStrategy.IsEqualTo(expected);
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
        public static IChainableFluentAssertion<IStringFluentAssertion> IsNotEqualTo(this IFluentAssertion<string> fluentAssertion, object expected)
        {
            var assertStrategy = new StringFluentAssertion(fluentAssertion.Value);
            return assertStrategy.IsNotEqualTo(expected);
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
        public static IChainableFluentAssertion<IStringFluentAssertion> IsInstanceOf<T>(this IFluentAssertion<string> fluentAssertion)
        {
            var assertStrategy = new StringFluentAssertion(fluentAssertion.Value);
            return assertStrategy.IsInstanceOf<T>();
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
        public static IChainableFluentAssertion<IStringFluentAssertion> IsNotInstanceOf<T>(this IFluentAssertion<string> fluentAssertion)
        {
            var assertStrategy = new StringFluentAssertion(fluentAssertion.Value);
            return assertStrategy.IsNotInstanceOf<T>();
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
        public static IChainableFluentAssertion<IStringFluentAssertion> Contains(this IFluentAssertion<string> fluentAssertion, params string[] values)
        {
            var assertStrategy = new StringFluentAssertion(fluentAssertion.Value);
            return assertStrategy.Contains(values);
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
        public static IChainableFluentAssertion<IStringFluentAssertion> StartsWith(this IFluentAssertion<string> fluentAssertion, string expectedPrefix)
        {
            var assertStrategy = new StringFluentAssertion(fluentAssertion.Value);
            return assertStrategy.StartsWith(expectedPrefix);
        }
    }
}