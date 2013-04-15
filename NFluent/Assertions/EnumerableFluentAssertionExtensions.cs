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
namespace NFluent
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using NFluent.Helpers;

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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> IsEqualTo(this IFluentAssertion<IEnumerable> fluentAssertion, object expected)
        {
            EqualityHelper.IsEqualTo(fluentAssertion.Value, expected);
            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> IsNotEqualTo(this IFluentAssertion<IEnumerable> fluentAssertion, object expected)
        {
            EqualityHelper.IsNotEqualTo(fluentAssertion.Value, expected);
            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> IsInstanceOf<T>(this IFluentAssertion<IEnumerable> fluentAssertion)
        {
            IsInstanceHelper.IsInstanceOf(fluentAssertion.Value, typeof(T));
            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> IsNotInstanceOf<T>(this IFluentAssertion<IEnumerable> fluentAssertion)
        {
            IsInstanceHelper.IsNotInstanceOf(fluentAssertion.Value, typeof(T));
            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> Contains<T>(this IFluentAssertion<IEnumerable> fluentAssertion, params T[] expectedValues)
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

            fluentAssertion.Contains(properExpectedValues);

            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> Contains(this IFluentAssertion<IEnumerable> fluentAssertion, IEnumerable otherEnumerable)
        {
            var notFoundValues = ExtractNotFoundValues(fluentAssertion.Value, otherEnumerable);

            if (notFoundValues.Count > 0)
            {
                throw new FluentAssertionException(string.Format("\nThe enumerable:\n\t[{0}]\ndoes not contain the expected value(s):\n\t[{1}]", fluentAssertion.Value.ToEnumeratedString(), notFoundValues.ToEnumeratedString()));
            }

            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> ContainsOnly<T>(this IFluentAssertion<IEnumerable> fluentAssertion, params T[] expectedValues)
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

            fluentAssertion.ContainsOnly(properExpectedValues);

            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> ContainsOnly(this IFluentAssertion<IEnumerable> fluentAssertion, IEnumerable expectedValues)
        {
            var unexpectedValuesFound = ExtractUnexpectedValues(fluentAssertion.Value, expectedValues);

            if (unexpectedValuesFound.Count > 0)
            {
                throw new FluentAssertionException(string.Format("\nThe enumerable:\n\t[{0}]\ndoes not contain only the expected value(s):\n\t[{1}].\nIt contains also other values:\n\t[{2}]", fluentAssertion.Value.ToEnumeratedString(), expectedValues.ToEnumeratedString(), unexpectedValuesFound.ToEnumeratedString()));
            }

            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> ContainsExactly<T>(this IFluentAssertion<IEnumerable> fluentAssertion, params T[] expectedValues)
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

            fluentAssertion.ContainsExactly(properExpectedValues);

            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> ContainsExactly(this IFluentAssertion<IEnumerable> fluentAssertion, IEnumerable otherEnumerable)
        {
            if (otherEnumerable == null)
            {
                ThrowsNotExactlyException(fluentAssertion, null);
            }

            var first = fluentAssertion.Value.GetEnumerator();
            var enumerable = otherEnumerable as IList<object> ?? otherEnumerable.Cast<object>().ToList();
            var second = enumerable.GetEnumerator();

            while (first.MoveNext())
            {
                if (!second.MoveNext() || !object.Equals(first.Current, second.Current))
                {
                    ThrowsNotExactlyException(fluentAssertion, enumerable);
                }
            }
            
            if (second.MoveNext())
            {
                ThrowsNotExactlyException(fluentAssertion, enumerable);
            }

            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> HasSize(this IFluentAssertion<IEnumerable> fluentAssertion, long expectedSize)
        {
            long itemsCount = fluentAssertion.Value.Cast<object>().LongCount();

            if (expectedSize != itemsCount)
            {
                string foundElementsNumberDescription = itemsCount.ToString();
                if (itemsCount > 1)
                {
                    foundElementsNumberDescription += " elements";
                }
                else
                {
                    foundElementsNumberDescription += " element";
                }
                
                var elements = fluentAssertion.Value.ToEnumeratedString();

                throw new FluentAssertionException(string.Format("\nFound {0} instead of {1}.\nFound:\n\t[{2}]", foundElementsNumberDescription, expectedSize, elements));
            }

            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
        }

        /// <summary>
        /// Checks that the enumerable is empty.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The enumerable is not empty.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<IEnumerable>> IsEmpty(this IFluentAssertion<IEnumerable> fluentAssertion)
        {
            if (fluentAssertion.Value.Cast<object>().Any())
            {
                throw new FluentAssertionException(string.Format("\nThe enumerable is not empty. Contains:\n\t[{0}]", fluentAssertion.Value.ToEnumeratedString()));
            }

            return new ChainableFluentAssertion<IFluentAssertion<IEnumerable>>(fluentAssertion);
        }

        #region private or internal methods

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

        private static void ThrowsNotExactlyException(IFluentAssertion<IEnumerable> fluentAssertion, IList<object> enumerable)
        {
            long foundCount;
            var foundItems = fluentAssertion.Value.ToEnumeratedString(out foundCount);
            var formatedFoundCount = FormatItemCount(foundCount);

            long expectedCount;
            object expectedItems = enumerable.ToEnumeratedString(out expectedCount);
            var formatedExpectedCount = FormatItemCount(expectedCount);

            throw new FluentAssertionException(string.Format("\nThe enumerable:\n\t[{0}] ({1})\ndoes not contain exactly the expected value(s):\n\t[{2}] ({3}).", foundItems, formatedFoundCount, expectedItems, formatedExpectedCount));
            
            // throw new FluentAssertionException(string.Format("\nThe enumerable:\n\t[{0}] ({1})\ndoes not contain exactly the expected value(s):\n\t[{2}] ({3}).\nIt contains also:\n\t[\"Vador\"]", foundItems, formatedFoundCount, expectedItems, formatedExpectedCount));
        }

        #endregion
    }
}
