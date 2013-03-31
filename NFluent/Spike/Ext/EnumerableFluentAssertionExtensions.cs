// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EnumerableFluentAssertionExtensions.cs" company="">
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
namespace Spike.Ext
{
    using System.Collections;
    using NFluent;

    /// <summary>
    /// Provides assertion methods to be executed on an <see cref="IEnumerable"/> value.
    /// </summary>
    public static class EnumerableFluentAssertionExtensions
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
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> IsEqualTo(this IFluentAssertion<IEnumerable> fluentAssertion, object expected)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value);
            return assertionStrategy.IsEqualTo(expected);
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
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> IsNotEqualTo(this IFluentAssertion<IEnumerable> fluentAssertion, object expected)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value);
            return assertionStrategy.IsNotEqualTo(expected);
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
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> IsInstanceOf<T>(this IFluentAssertion<IEnumerable> fluentAssertion)
        {
            // TODO review whether it is necessary or not to have such explicit interface implementation on the EnumerableFluentAssertion type. 
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value) as IInstanceTypeFluentAssertionTrait<IEnumerableFluentAssertion>;
            return assertionStrategy.IsInstanceOf<T>();
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
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> IsNotInstanceOf<T>(this IFluentAssertion<IEnumerable> fluentAssertion)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value) as IInstanceTypeFluentAssertionTrait<IEnumerableFluentAssertion>;
            return assertionStrategy.IsNotInstanceOf<T>();
        }

        /// <summary>
        /// Checks that the enumerable contains all the given expected values, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the enumerable.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contain all the expected values.</exception>
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> Contains<T>(this IFluentAssertion<IEnumerable> fluentAssertion, params T[] expectedValues)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value);
            return assertionStrategy.Contains(expectedValues);
        }

        /// <summary>
        /// Checks that the enumerable contains all the values present in another enumerable, in any order.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="otherEnumerable">The enumerable containing the expected values to be found.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contain all the expected values present in the other enumerable.</exception>
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> Contains(this IFluentAssertion<IEnumerable> fluentAssertion, IEnumerable otherEnumerable)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value);
            return assertionStrategy.Contains(otherEnumerable);
        }

        /// <summary>
        /// Checks that the enumerable contains only the given values and nothing else, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the expected values to be found.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contain only the expected values provided.</exception>
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsOnly<T>(this IFluentAssertion<IEnumerable> fluentAssertion, params T[] expectedValues)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value);
            return assertionStrategy.ContainsOnly(expectedValues);
        }

        /// <summary>
        /// Checks that the enumerable contains only the values present in another enumerable, and nothing else, in any order.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contain only the expected values present in the other enumerable.</exception>
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsOnly(this IFluentAssertion<IEnumerable> fluentAssertion, IEnumerable expectedValues)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value);
            return assertionStrategy.ContainsOnly(expectedValues);
        }

        /// <summary>
        /// Checks that the enumerable contains only the given expected values and nothing else, in order.
        /// This assertion should only be used with IEnumerable that have a consistent iteration order
        /// (i.e. don't use it with <see cref="Hashtable" />, prefer <see cref="ContainsOnly{T}" /> in that case).
        /// </summary>
        /// <typeparam name="T">Type of the elements to be found.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contains only the exact given values and nothing else, in order.</exception>
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsExactly<T>(this IFluentAssertion<IEnumerable> fluentAssertion, params T[] expectedValues)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value);
            return assertionStrategy.ContainsExactly(expectedValues);
        }

        /// <summary>
        /// Checks that the enumerable contains only the values of another enumerable and nothing else, in order.
        /// This assertion should only be used with IEnumerable that have a consistent iteration order
        /// (i.e. don't use it with <see cref="Hashtable" />, prefer <see cref="ContainsOnly{T}" /> in that case).
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="otherEnumerable">The other enumerable containing the exact expected values to be found.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contains only the exact given values and nothing else, in order.</exception>
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsExactly(this IFluentAssertion<IEnumerable> fluentAssertion, IEnumerable otherEnumerable)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value);
            return assertionStrategy.ContainsExactly(otherEnumerable);
        }

        /// <summary>
        /// Checks that the enumerable has the proper number of elements.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expectedSize">The expected size to be found.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The enumerable has not the expected number of elements.</exception>
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> HasSize(this IFluentAssertion<IEnumerable> fluentAssertion, long expectedSize)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value);
            return assertionStrategy.HasSize(expectedSize);
        }

        /// <summary>
        /// Checks that the enumerable is empty.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The enumerable is not empty.</exception>
        public static IChainableFluentAssertion<IEnumerableFluentAssertion> IsEmpty(this IFluentAssertion<IEnumerable> fluentAssertion)
        {
            var assertionStrategy = new EnumerableFluentAssertion(fluentAssertion.Value);
            return assertionStrategy.IsEmpty();
        }
    }
}
