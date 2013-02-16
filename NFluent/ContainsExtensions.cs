namespace NFluent
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for exploiting enumerable content in a fluent manner (i.e. with auto completion support and in an english readable way).
    /// </summary>
    public static class ContainsExtensions
    {
        /// <summary>
        /// Verifies that the specified array contains the given expected values, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the arrays.</typeparam>
        /// <param name="array">The array that should hold the expected values.</param>
        /// <param name="expectedValues">The expected values.</param>
        /// <returns>
        ///   <c>true</c> if the array contains all the specified expected values, in any order; throws a <see cref="FluentAssertionException"/> otherwise.
        /// </returns>
        /// <exception cref="NFluent.FluentAssertionException">The array does not contains all the expected values.</exception>
        public static bool Contains<T>(this T[] array, params T[] expectedValues)
        {
            var notFoundValues = ExtractNotFoundValues(array, expectedValues);

            if (notFoundValues.Count == 0)
            {
                return true;
            }
            
            throw new FluentAssertionException(string.Format("The array does not contain the expected value(s): [{0}].", notFoundValues.ToEnumeratedString()));
        }

        /// <summary>
        /// Verifies that the specified enumerable contains the given expected values, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable that should hold the expected values.</param>
        /// <param name="expectedValues">The expected values.</param>
        /// <returns>
        ///   <c>true</c> if the enumerable contains all the specified expected values, in any order; throws a <see cref="FluentAssertionException"/> otherwise.
        /// </returns>
        /// <exception cref="NFluent.FluentAssertionException">The enumerable does not contains all the expected values.</exception>
        public static bool Contains<T>(this IEnumerable<T> enumerable, params T[] expectedValues)
        {
            var notFoundValues = ExtractNotFoundValues(enumerable, expectedValues);

            if (notFoundValues.Count == 0)
            {
                return true;
            }

            throw new FluentAssertionException(string.Format("The enumerable does not contain the expected value(s): [{0}].", notFoundValues.ToEnumeratedString()));
        }

        /// <summary>
        /// Verifies that the actual array contains only the given values and nothing else, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the expected elements to search within.</typeparam>
        /// <param name="array">The array to verify.</param>
        /// <param name="expectedValues">The expected values to be searched.</param>
        /// <returns>
        ///   <c>true</c> if the specified array contains only the given values and nothing else, in any order; otherwise, throws a <see cref="FluentAssertionException"/>.
        /// </returns>
        public static bool ContainsOnly<T>(this T[] array, params T[] expectedValues)
        {
            var unexpectedValuesFound = ExtractUnexpectedValues(array, expectedValues);

            if (unexpectedValuesFound.Count > 0)
            {
                throw new FluentAssertionException(string.Format("The array does not contain only the expected value(s). It contains also other values: [{0}].", unexpectedValuesFound.ToEnumeratedString()));
            }

            return true;
        }

        /// <summary>
        /// Verifies that the actual enumerable contains only the given values and nothing else, in any order.
        /// </summary>
        /// <typeparam name="T">Type of the expected elements to search within.</typeparam>
        /// <param name="enumerable">The array to verify.</param>
        /// <param name="expectedValues">The expected values to be searched.</param>
        /// <returns>
        ///   <c>true</c> if the specified enumerable contains only the given values and nothing else, in any order; otherwise, throws a <see cref="FluentAssertionException"/>.
        /// </returns>
        public static bool ContainsOnly<T>(this IEnumerable<T> enumerable, params T[] expectedValues)
        {
            var unexpectedValuesFound = ExtractUnexpectedValues(enumerable, expectedValues);

            if (unexpectedValuesFound.Count > 0)
            {
                throw new FluentAssertionException(string.Format("The enumerable does not contain only the expected value(s). It contains also other values: [{0}].", unexpectedValuesFound.ToEnumeratedString()));
            }

            return true;
        }

        /// <summary>
        /// Verifies that the actual enumerable contains only the given expected values and nothing else, in order.
        /// This assertion should only be used with IEnumerable that have a consistent iteration order 
        /// (i.e. don't use it with <see cref="Hashtable"/>, prefer <see cref="ContainsOnly"/> in that case).
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the <see cref="expectedValues"/> array.</typeparam>
        /// <param name="enumerable">The enumerable to verify.</param>
        /// <param name="expectedValues">The expected values to be searched.</param>
        /// <returns>
        ///   <c>true</c> if the enumerable contains exactly the specified expected values; throws a <see cref="FluentAssertionException"/> otherwise.
        /// </returns>
        /// <exception cref="NFluent.FluentAssertionException">The specified enumerable does not contains exactly the specified expected values.</exception>
        public static bool ContainsExactly<T>(this IEnumerable enumerable, params T[] expectedValues)
        {
            long i = 0;
            foreach (var obj in enumerable)
            {
                if (!object.Equals(obj, expectedValues[i]))
                {
                    var expectedNumberOfItemsDescription = FormatItemCount(expectedValues.LongLength);

                    var enumerableCount = 0;
                    foreach (var item in enumerable)
                    {
                        enumerableCount++;
                    }

                    var foundNumberOfItemsDescription = string.Format(enumerableCount <= 1 ? "{0} item" : "{0} items", enumerableCount);

                    throw new FluentAssertionException(string.Format("Found: [{0}] ({1}) instead of the expected [{2}] ({3}).", enumerable.ToEnumeratedString(), foundNumberOfItemsDescription, expectedValues.ToEnumeratedString(), expectedNumberOfItemsDescription));
                }

                i++;
            }
         
            return true;
        }

        /// <summary>
        /// Determines whether the specified enumerable contains exactly some expected values.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="otherEnumerable">The other enumerable.</param>
        /// <returns>
        ///   <c>true</c> if the specified enumerable contains exactly the specified expected values; throws a <see cref="FluentAssertionException" /> otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="NFluent.FluentAssertionException">The specified enumerable does not contains exactly the specified expected values.</exception>
        public static bool ContainsExactly(this IEnumerable enumerable, IEnumerable otherEnumerable)
        {
            // TODO: Refactor this implementation
            if (otherEnumerable == null)
            {
                long foundCount;
                var foundItems = enumerable.ToEnumeratedString(out foundCount);
                var foundItemsCount = FormatItemCount(foundCount);
                throw new FluentAssertionException(string.Format("Found: [{0}] ({1}) instead of the expected [] (0 item).", foundItems, foundItemsCount));
            }

            var first = enumerable.GetEnumerator();
            var second = otherEnumerable.GetEnumerator();

            while (first.MoveNext())
            {
                if (!second.MoveNext() || !object.Equals(first.Current, second.Current))
                {
                    long foundCount;
                    var foundItems = enumerable.ToEnumeratedString(out foundCount);
                    var formatedFoundCount = FormatItemCount(foundCount);

                    long expectedCount;
                    object expectedItems = otherEnumerable.ToEnumeratedString(out expectedCount);
                    var formatedExpectedCount = FormatItemCount(expectedCount);

                    throw new FluentAssertionException(string.Format("Found: [{0}] ({1}) instead of the expected [{2}] ({3}).", foundItems, formatedFoundCount, expectedItems, formatedExpectedCount));
                }
            }

            return true;
        }

        /// <summary>
        /// Returns all expected values that aren't present in the enumerable.
        /// </summary>
        /// <typeparam name="T">Type of data to enumerate and find.</typeparam>
        /// <param name="enumerable">The enumerable to inspect.</param>
        /// <param name="expectedValues">The expected values to search within the enumerable.</param>
        /// <returns>A list containing all the expected values that aren't present in the enumerable.</returns>
        internal static IList ExtractNotFoundValues<T>(IEnumerable<T> enumerable, T[] expectedValues)
        {
            var notFoundValues = new List<T>(expectedValues);
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
        /// <typeparam name="T">Type of enumerable and expected values.</typeparam>
        /// <param name="enumerable">The enumerable to inspect.</param>
        /// <param name="expectedValues">The allowed values to be part of the enumerable.</param>
        /// <returns>A list with all the values found in the enumerable that don't belong to the expected ones.</returns>
        internal static IList ExtractUnexpectedValues<T>(IEnumerable<T> enumerable, T[] expectedValues)
        {
            var unexpectedValuesFound = new List<T>();
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
        /// <returns>The proper description for the items count.</returns>
        internal static string FormatItemCount(long itemsCount)
        {
            return string.Format(itemsCount <= 1 ? "{0} item" : "{0} items", itemsCount);
        }
    }
}