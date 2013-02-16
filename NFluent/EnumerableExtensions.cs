namespace NFluent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    // TODO: check performances

    /// <summary>
    /// Extension methods for exploiting enumerable content in a fluent manner (i.e. with auto completion support and in an english readable way).
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Extract all the values of a given property given its name, from an enumerable collection of objects holding that property.
        /// </summary>
        /// <typeparam name="T">Type of the objects belonging to the initial enumerable collection.</typeparam>
        /// <param name="enumerable">The enumerable collection of objects.</param>
        /// <param name="propertyName">Name of the property to extract value from for every object of the collection.</param>
        /// <returns>
        /// An enumerable of all the property values for every <see cref="T"/> objects in the <see cref="enumerable"/>.
        /// </returns>
        public static IEnumerable Properties<T>(this IEnumerable<T> enumerable, string propertyName)
        {
            Type type = typeof(T);
            var getter = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (getter == null)
            {
                throw new InvalidOperationException(string.Format("Objects of expectedType {0} don't have property with name '{1}'", type, propertyName));
            }

            foreach (var o in enumerable)
            {
                var value = getter.GetValue(o, null);
                yield return value;
            }
        }

        /// <summary>
        /// Checks that an object is equal to another instance, and throws a <see cref="FluentAssertionException"/> if they are not equal.
        /// This method is provided as a replacement to the classic Equals() method because we need it to throw exception with proper message (which is of course not the case of the .NET Equals methods).
        /// </summary>
        /// <remarks>This method is not named EqualsOrThrowException to ensure english readability when used within an Assert.That() statement.</remarks>
        /// <param name="obj">The current object instance.</param>
        /// <param name="expected">The object that we expect to be equal.</param>
        /// <returns><c>true</c> if the two objects are equal, or throw a <see cref="FluentAssertionException"/> otherwise.</returns>
        /// <exception cref="NFluent.FluentAssertionException">The two objects are not equal.</exception>
        public static bool IsEqualTo(this object obj, object expected)
        {
            if (!object.Equals(obj, expected))
            {
                throw new FluentAssertionException(string.Format("[{0}] not equals to the expected [{1}]", obj.ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
            }

            return true;
        }

        /// <summary>
        /// Checks that an object is NOT equal to another instance, and throws a <see cref="FluentAssertionException"/> if they are equal.
        /// This method is provided as a replacement to the classic Equals() method because we need it to throw exception with proper message (which is of course not the case of the .NET Equals methods).
        /// </summary>
        /// <remarks>This method is not named NotEqualsOrThrowException to ensure english readability when used within an Assert.That() statement.</remarks>
        /// <param name="obj">The current object instance.</param>
        /// <param name="expected">The object that we expect to be NOT equal.</param>
        /// <returns><c>true</c> if the two objects are not equal, or throw a <see cref="FluentAssertionException"/> otherwise.</returns>
        /// <exception cref="NFluent.FluentAssertionException">The two objects are equal.</exception>
        public static bool IsNotEqualTo(this object obj, object expected)
        {
            if (object.Equals(obj, expected))
            {
                throw new FluentAssertionException(string.Format("[{0}] equals to the expected value [{1}]", obj.ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
            }

            return true;
        }

        /// <summary>
        /// Determines whether an object is an instance of a given type, and throw a <see cref="FluentAssertionException"/> with proper details if not the case.
        /// </summary>
        /// <param name="obj">The current object instance to check.</param>
        /// <param name="expectedType">The type we expect the object to be.</param>
        /// <returns>
        ///   <c>true</c> if this object is an instance of this type; otherwise, throw a <see cref="FluentAssertionException"/> with proper details.
        /// </returns>
        /// <exception cref="FluentAssertionException">The object is not an instance of this type.</exception>
        public static bool IsInstanceOf(this object obj, Type expectedType)
        {
            if (obj.GetType() != expectedType)
            {
                throw new FluentAssertionException(string.Format("[{0}] is not an instance of the expectedType [{1}] but of [{2}] instead.", obj.ToStringProperlyFormated(), expectedType, obj.GetType()));
            }

            return true;
        }

        /// <summary>
        /// Verifies that an object is not an instance of a given type, and throw a <see cref="FluentAssertionException"/> with proper details if not the case.
        /// </summary>
        /// <param name="obj">The current object instance to check.</param>
        /// <param name="expectedType">The type we expect the object to be.</param>
        /// <returns>
        ///   <c>true</c> if this object is not an instance of this type; otherwise, throw a <see cref="FluentAssertionException"/> with proper details.
        /// </returns>
        /// <exception cref="FluentAssertionException">The object is an instance of this type (which is not expected).</exception>
        public static bool IsNotInstanceOf(this object obj, Type expectedType)
        {
            if (obj.GetType() == expectedType)
            {
                throw new FluentAssertionException(string.Format("[{0}] is an instance of the type [{1}] which is not expected.", obj.ToStringProperlyFormated(), obj.GetType()));
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified enumerable contains at least all the expected values provided.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the arrays.</typeparam>
        /// <param name="array">The array that should hold the expected values.</param>
        /// <param name="expectedValues">The expected values.</param>
        /// <returns>
        ///   <c>true</c> if the array contains all the specified expected values]; throw a <see cref="FluentAssertionException"/> otherwise.
        /// </returns>
        /// <exception cref="NFluent.FluentAssertionException">The specified enumerable does not contains exactly the specified expected values.</exception>
        public static bool Contains<T>(this T[] array, params T[] expectedValues)
        {
            var notFoundValues = new List<T>(expectedValues);
            foreach (var element in array)
            {
                foreach (var expectedValue in expectedValues)
                {
                    if (object.Equals(element, expectedValue))
                    {
                        notFoundValues.Remove(expectedValue);
                        break;
                    }
                }
            }    

            if (notFoundValues.Count == 0)
            {
                return true;
            }
            
            throw new FluentAssertionException(string.Format("The array does not contain the expected value(s): [{0}].", notFoundValues.ToEnumeratedString()));
        }

        /// <summary>
        /// Determines whether the specified enumerable contains exactly some expected values.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the <see cref="expectedValues"/> array.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="expectedValues">The expected values.</param>
        /// <returns>
        ///   <c>true</c> if the enumerable contains exactly the specified expected values; throw a <see cref="FluentAssertionException"/> otherwise.
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
        ///   <c>true</c> if the specified enumerable contains exactly the specified expected values; throw a <see cref="FluentAssertionException" /> otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="NFluent.FluentAssertionException">The specified enumerable does not contains exactly the specified expected values.</exception>
        public static bool ContainsExactly(this IEnumerable enumerable, IEnumerable otherEnumerable)
        {
            // TODO: Refactor this implementation
            if (otherEnumerable == null)
            {
                long foundCount;
                var foundItems = ToEnumeratedString(enumerable, out foundCount);
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
                    var foundItems = ToEnumeratedString(enumerable, out foundCount);
                    var formatedFoundCount = FormatItemCount(foundCount);

                    long expectedCount;
                    object expectedItems = ToEnumeratedString(otherEnumerable, out expectedCount);
                    var formatedExpectedCount = FormatItemCount(expectedCount);

                    throw new FluentAssertionException(string.Format("Found: [{0}] ({1}) instead of the expected [{2}] ({3}).", foundItems, formatedFoundCount, expectedItems, formatedExpectedCount));
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified enumerable has the proper size (i.e. number of elements).
        /// </summary>
        /// <param name="enumerable">The enumerable to inspect.</param>
        /// <param name="expectedSize">The expected size.</param>
        /// <returns>
        ///   <c>true</c> if the specified enumerable has the expected size; otherwise throws a <see cref="FluentAssertionException"/>.
        /// </returns>
        /// <exception cref="FluentAssertionException">The enumerable has not the expected size.</exception>
        public static bool HasSize(this IEnumerable enumerable, long expectedSize)
        {
            long itemsCount = 0;

            foreach (var item in enumerable)
            {
                itemsCount++;
            }

            if (expectedSize != itemsCount)
            {
                throw new FluentAssertionException(string.Format("Has [{0}] items instead of the expected value [{1}].", itemsCount, expectedSize));
            }
            
            return true;
        }

        /// <summary>
        /// Return a string containing all the <see cref="IEnumerable" /> elements, separated by a comma.
        /// </summary>
        /// <param name="enumerable">The enumerable to transform into a string.</param>
        /// <returns>
        /// A string containing all the <see cref="IEnumerable" /> elements, separated by a comma.
        /// </returns>
        public static string ToEnumeratedString(this IEnumerable enumerable)
        {
            long itemsCount = 0;
            return ToEnumeratedString(enumerable, out itemsCount);
        }

        /// <summary>
        /// Return a string containing all the elements of an <see cref="IEnumerable" />, separated by a comma.
        /// </summary>
        /// <param name="enumerable">The enumerable to transform into a string.</param>
        /// <param name="itemsCount">The number of items within the <see cref="IEnumerable"/>.</param>
        /// <returns>
        /// A string containing all the <see cref="IEnumerable" /> elements, separated by a comma.
        /// </returns>
        public static string ToEnumeratedString(this IEnumerable enumerable, out long itemsCount)
        {
            // TODO: extract method to introduce Separator as a parameter (and using comma as Default value)
            var firstTime = true;
            var sb = new StringBuilder();
            const string Separator = ", ";
            itemsCount = 0;

            foreach (var obj in enumerable)
            {
                if (!firstTime)
                {
                    sb.Append(Separator);
                }

                sb.Append(obj.ToStringProperlyFormated());
                firstTime = false;

                itemsCount++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string that represents the current object. If the object is already a string, this method will surround it with brackets.
        /// </summary>
        /// <param name="theObject">The theObject.</param>
        /// <returns>A string that represents the current object. If the object is already a string, this method will surround it with brackets.</returns>
        private static string ToStringProperlyFormated(this object theObject)
        {
            if (theObject is string)
            {
                return string.Format(@"""{0}""", theObject);
            }
            else
            {
                return theObject.ToString();
            }
        }

        /// <summary>
        /// Generates the proper description for the items count, based on their numbers.
        /// </summary>
        /// <param name="itemsCount">The number of items.</param>
        /// <returns>The proper description for the items count.</returns>
        private static string FormatItemCount(long itemsCount)
        {
            return string.Format(itemsCount <= 1 ? "{0} item" : "{0} items", itemsCount);
        }
    }
}
