// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EnumerableFluentAssertion.cs" company="">
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
    using System.Collections.Generic;
    using System.Linq;

    using NFluent.Helpers;

    /// <summary>
    /// Provides assertion methods to be executed on the enumerable System Under Test (SUT) instance.
    /// </summary>
    internal class EnumerableFluentAssertion : IEnumerableFluentAssertion
    {
        private readonly IEnumerable sutEnumerable;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerableFluentAssertion" /> class.
        /// </summary>
        /// <param name="sutEnumerable">The System Under Test enumerable.</param>
        public EnumerableFluentAssertion(IEnumerable sutEnumerable)
        {
            this.sutEnumerable = sutEnumerable;
        }

        #region IEqualityFluentAssertion members

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public IChainableFluentAssertion<IEnumerableFluentAssertion> IsEqualTo(object expected)
        {
            EqualityHelper.IsEqualTo(this.sutEnumerable, expected);
            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public IChainableFluentAssertion<IEnumerableFluentAssertion> IsNotEqualTo(object expected)
        {
            EqualityHelper.IsNotEqualTo(this.sutEnumerable, expected);
            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        #endregion

        #region IInstanceTypeFluentAssertion members

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        IChainableFluentAssertion<IEnumerableFluentAssertion> IInstanceTypeFluentAssertion<IEnumerableFluentAssertion>.IsInstanceOf<T>()
        {
            IsInstanceHelper.IsInstanceOf(this.sutEnumerable, typeof(T));
            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        IChainableFluentAssertion<IEnumerableFluentAssertion> IInstanceTypeFluentAssertion<IEnumerableFluentAssertion>.IsNotInstanceOf<T>()
        {
            IsInstanceHelper.IsNotInstanceOf(this.sutEnumerable, typeof(T));
            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        #endregion

        #region IEnumerableFluentAssertion members

        /// <summary>
        /// Checks that the enumerable contains all the given expected values, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the enumerable.</typeparam>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contain all the expected values.</exception>
        public IChainableFluentAssertion<IEnumerableFluentAssertion> Contains<T>(params T[] expectedValues)
        {
            IEnumerable properExpectedValues;
            if (IsAOneValueArrayWithOneCollectionInside(expectedValues))
            {
                properExpectedValues = expectedValues[0] as IEnumerable;
            }
            else
            {
                properExpectedValues = expectedValues as IEnumerable;
            }

            this.Contains(properExpectedValues);
            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        /// <summary>
        /// Checks that the enumerable contains all the values present in another enumerable, in any order.
        /// </summary>
        /// <param name="otherEnumerable">The enumerable containing the expected values to be found.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contain all the expected values present in the other enumerable.</exception>
        public IChainableFluentAssertion<IEnumerableFluentAssertion> Contains(IEnumerable otherEnumerable)
        {
            var notFoundValues = ExtractNotFoundValues(this.sutEnumerable, otherEnumerable);

            if (notFoundValues.Count == 0)
            {
                return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
            }

            throw new FluentAssertionException(string.Format("The enumerable [{0}] does not contain the expected value(s): [{1}].", this.sutEnumerable.ToEnumeratedString(), notFoundValues.ToEnumeratedString()));
        }

        /// <summary>
        /// Checks that the enumerable contains only the given values and nothing else, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the expected values to be found.</typeparam>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contain only the expected values provided.</exception>
        public IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsOnly<T>(params T[] expectedValues)
        {
            IEnumerable properExpectedValues;
            if (IsAOneValueArrayWithOneCollectionInside(expectedValues))
            {
                properExpectedValues = expectedValues[0] as IEnumerable;
            }
            else
            {
                properExpectedValues = expectedValues as IEnumerable;
            }

            this.ContainsOnly(properExpectedValues);

            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        /// <summary>
        /// Checks that the enumerable contains only the values present in another enumerable, and nothing else, in any order.
        /// </summary>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contain only the expected values present in the other enumerable.</exception>
        public IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsOnly(IEnumerable expectedValues)
        {   
            var unexpectedValuesFound = ExtractUnexpectedValues(this.sutEnumerable, expectedValues);

            if (unexpectedValuesFound.Count > 0)
            {
                throw new FluentAssertionException(string.Format("The enumerable [{0}] does not contain only the expected value(s). It contains also other values: [{1}].", this.sutEnumerable.ToEnumeratedString(), unexpectedValuesFound.ToEnumeratedString()));
            }

            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        /// <summary>
        /// Checks that the enumerable contains only the given expected values and nothing else, in order.
        /// This assertion should only be used with IEnumerable that have a consistent iteration order
        /// (i.e. don't use it with <see cref="Hashtable" />, prefer <see cref="ContainsOnly{T}" /> in that case).
        /// </summary>
        /// <typeparam name="T">Type of the elements to be found.</typeparam>
        /// <param name="expectedValues">The expected values to be found.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contains only the exact given values and nothing else, in order.</exception>
        public IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsExactly<T>(params T[] expectedValues)
        {
            IEnumerable properExpectedValues;
            if (IsAOneValueArrayWithOneCollectionInside(expectedValues))
            {
                properExpectedValues = expectedValues[0] as IEnumerable;
            }
            else
            {
                properExpectedValues = expectedValues as IEnumerable;
            }

            this.ContainsExactly(properExpectedValues);

            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        /// <summary>
        /// Checks that the enumerable contains only the values of another enumerable and nothing else, in order.
        /// This assertion should only be used with IEnumerable that have a consistent iteration order
        /// (i.e. don't use it with <see cref="Hashtable" />, prefer <see cref="ContainsOnly{T}" /> in that case).
        /// </summary>
        /// <param name="otherEnumerable">The other enumerable containing the exact expected values to be found.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The enumerable does not contains only the exact given values and nothing else, in order.</exception>
        public IChainableFluentAssertion<IEnumerableFluentAssertion> ContainsExactly(IEnumerable otherEnumerable)
        {
            // TODO: Refactor this implementation
            if (otherEnumerable == null)
            {
                long foundCount;
                var foundItems = this.sutEnumerable.ToEnumeratedString(out foundCount);
                var foundItemsCount = FormatItemCount(foundCount);
                throw new FluentAssertionException(string.Format("Found: [{0}] ({1}) instead of the expected [null] (0 item).", foundItems, foundItemsCount));
            }

            var first = this.sutEnumerable.GetEnumerator();
            var enumerable = otherEnumerable as IList<object> ?? otherEnumerable.Cast<object>().ToList();
            var second = enumerable.GetEnumerator();

            while (first.MoveNext())
            {
                if (!second.MoveNext() || !object.Equals(first.Current, second.Current))
                {
                    long foundCount;
                    var foundItems = this.sutEnumerable.ToEnumeratedString(out foundCount);
                    var formatedFoundCount = FormatItemCount(foundCount);

                    long expectedCount;
                    object expectedItems = enumerable.ToEnumeratedString(out expectedCount);
                    var formatedExpectedCount = FormatItemCount(expectedCount);

                    throw new FluentAssertionException(string.Format("Found: [{0}] ({1}) instead of the expected [{2}] ({3}).", foundItems, formatedFoundCount, expectedItems, formatedExpectedCount));
                }
            }

            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        /// <summary>
        /// Checks that the enumerable has the proper number of elements.
        /// </summary>
        /// <param name="expectedSize">The expected size to be found.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The enumerable has not the expected number of elements.</exception>
        public IChainableFluentAssertion<IEnumerableFluentAssertion> HasSize(long expectedSize)
        {
            long itemsCount = this.sutEnumerable.Cast<object>().LongCount();

            if (expectedSize != itemsCount)
            {
                throw new FluentAssertionException(string.Format("Has [{0}] items instead of the expected value [{1}].", itemsCount, expectedSize));
            }

            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        /// <summary>
        /// Checks that the enumerable is empty.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The enumerable is not empty.</exception>
        public IChainableFluentAssertion<IEnumerableFluentAssertion> IsEmpty()
        {
            if (this.sutEnumerable.Cast<object>().Any())
            {
                throw new FluentAssertionException(string.Format("Enumerable not empty. Contains the element(s): [{0}].", this.sutEnumerable.ToEnumeratedString()));
            }

            return new ChainableFluentAssertion<IEnumerableFluentAssertion>(this);
        }

        #endregion

        /// <summary>
        /// Returns all expected values that aren't present in the enumerable.
        /// </summary>
        /// <param name="enumerable">The enumerable to inspect.</param>
        /// <param name="expectedValues">The expected values to search within the enumerable.</param>
        /// <returns>
        /// A list containing all the expected values that aren't present in the enumerable.
        /// </returns>
        internal static IList ExtractNotFoundValues(IEnumerable enumerable, IEnumerable expectedValues)
        {
            // Prepares the list to return
            var notFoundValues = new List<object>();
            foreach (var expectedValue in expectedValues)
            {
                notFoundValues.Add(expectedValue);
            }

            foreach (var element in enumerable)
            {
                foreach (var expectedValue in expectedValues)
                {
                    if (object.Equals(element, expectedValue))
                    {
                        notFoundValues.RemoveAll((one) => one.Equals(expectedValue));
                        break;
                    }
                }
            }

            return notFoundValues;
        }

        /// <summary>
        /// Returns all the values of the enumerable that don't belong to the expected ones.
        /// </summary>
        /// <param name="enumerable">The enumerable to inspect.</param>
        /// <param name="expectedValues">The allowed values to be part of the enumerable.</param>
        /// <returns>
        /// A list with all the values found in the enumerable that don't belong to the expected ones.
        /// </returns>
        internal static IList ExtractUnexpectedValues(IEnumerable enumerable, IEnumerable expectedValues)
        {
            var unexpectedValuesFound = new List<object>();
            foreach (var element in enumerable)
            {
                var isExpectedValue = false;
                foreach (var expectedValue in expectedValues)
                {
                    if (object.Equals(element, expectedValue))
                    {
                        isExpectedValue = true;
                        break;
                    }
                }

                if (!isExpectedValue)
                {
                    unexpectedValuesFound.Add(element);
                }
            }

            return unexpectedValuesFound;
        }

        /// <summary>
        /// Generates the proper description for the items count, based on their numbers.
        /// </summary>
        /// <param name="itemsCount">The number of items.</param>
        /// <returns>
        /// The proper description for the items count.
        /// </returns>
        internal static string FormatItemCount(long itemsCount)
        {
            return string.Format(itemsCount <= 1 ? "{0} item" : "{0} items", itemsCount);
        }

        private static bool IsAOneValueArrayWithOneCollectionInside<T>(T[] expectedValues)
        {
            // For every collections like ArrayList, List<T>, IEnumerable<T>, StringCollection, etc.
            return expectedValues != null && (expectedValues.LongLength == 1) && IsAnEnumerableButNotAnEnumerableOfChars(expectedValues[0]);
        }

        private static bool IsAnEnumerableButNotAnEnumerableOfChars<T>(T element)
        {
            return (element is IEnumerable) && !(element is IEnumerable<char>);
        }
    }
}