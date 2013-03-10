// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IEnumerableFluentAssertion.cs" company="">
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
    using System.Collections;

    // TODO: add the IFluentAssertion interface to all I...FluentAssert?

    /// <summary>
    /// Provides assertion methods to be executed on an enumerable instance.
    /// </summary>
    public interface IEnumerableFluentAssertion : IFluentAssertion, IEqualityFluentAssertion, IInstanceTypeFluentAssertion<IEnumerableFluentAssertion>
    {
        /// <summary>
        /// Checks that the enumerable contains all the given expected values, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the enumerable.</typeparam>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <exception cref="FluentAssertionException">The enumerable does not contain all the expected values.</exception>
        IChainableFluentAssertion<IEnumerableFluentAssertion> Contains<T>(params T[] expectedValues);

        /// <summary>
        /// Checks that the enumerable contains all the values present in another enumerable, in any order.
        /// </summary>
        /// <param name="otherEnumerable">The enumerable containing the expected values to be found.</param>
        /// <exception cref="FluentAssertionException">The enumerable does not contain all the expected values present in the other enumerable.</exception>
        IChainableFluentAssertion<IEnumerableFluentAssertion> Contains(IEnumerable otherEnumerable);

        /// <summary>
        /// Checks that the enumerable contains only the given values and nothing else, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the expected values to be found.</typeparam>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <exception cref="FluentAssertionException">The enumerable does not contain only the expected values provided.</exception>
        IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsOnly<T>(params T[] expectedValues);

        /// <summary>
        /// Checks that the enumerable contains only the values present in another enumerable, and nothing else, in any order.
        /// </summary>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <exception cref="FluentAssertionException">The enumerable does not contain only the expected values present in the other enumerable.</exception>
        IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsOnly(IEnumerable expectedValues);

        /// <summary>
        /// Checks that the enumerable contains only the given expected values and nothing else, in order.
        /// This assertion should only be used with IEnumerable that have a consistent iteration order
        /// (i.e. don't use it with <see cref="Hashtable" />, prefer <see cref="ContainsOnly{T}" /> in that case).
        /// </summary>
        /// <typeparam name="T">Type of the elements to be found.</typeparam>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <exception cref="FluentAssertionException">The enumerable does not contains only the exact given values and nothing else, in order.</exception>
        IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsExactly<T>(params T[] expectedValues);

        /// <summary>
        /// Checks that the enumerable contains only the values of another enumerable and nothing else, in order.
        /// This assertion should only be used with IEnumerable that have a consistent iteration order
        /// (i.e. don't use it with <see cref="Hashtable" />, prefer <see cref="ContainsOnly{T}" /> in that case).
        /// </summary>
        /// <param name="otherEnumerable">The other enumerable containing the exact expected values to be found.</param>
        /// <exception cref="FluentAssertionException">The enumerable does not contains only the exact given values and nothing else, in order.</exception>
        IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsExactly(IEnumerable otherEnumerable);

        /// <summary>
        /// Checks that the enumerable has the proper number of elements.
        /// </summary>
        /// <param name="expectedSize">The expected size to be found.</param>
        /// <exception cref="FluentAssertionException">The enumerable has not the expected number of elements.</exception>
        IChainableFluentAssertion<IEnumerableFluentAssertion> HasSize(long expectedSize);

        /// <summary>
        /// Checks that the enumerable is empty.
        /// </summary>
        /// <exception cref="FluentAssertionException">The enumerable is not empty.</exception>
        IChainableFluentAssertion<IEnumerableFluentAssertion> IsEmpty();
    }
}