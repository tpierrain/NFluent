namespace NFluent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Extension methods for easily exploiting enumerable content.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Extract all the values of a given property given its name, from an enumerable collection of objects.
        /// </summary>
        /// <typeparam name="T">Type of the objects belonging to the initial enumerable collection</typeparam>
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
                throw new InvalidOperationException(string.Format("Objects of type {0} don't have property with name '{1}'", type, propertyName));
            }

            foreach (var o in enumerable)
            {
                var value = getter.GetValue(o, null);
                yield return value;
            }
        }

        /// <summary>
        /// Checks that an object is equal to the current instance, and throws a <see cref="FluentAssertionException"/> if it is not the case.
        /// </summary>
        /// <param name="obj">The current object instance.</param>
        /// <param name="expected">The object that we expect to be equal.</param>
        /// <returns><c>true</c> if the two objects are equal, or throw a <see cref="FluentAssertionException"/> otherwise.</returns>
        /// <exception cref="NFluent.FluentAssertionException">the two objects are not equal.</exception>
        public static bool EqualsExactly(this object obj, object expected)
        {
            if (!object.Equals(obj, expected))
            {
                throw new FluentAssertionException(string.Format("'{0}' not equals to the expected '{1}'", obj, expected));
            }

            return true;
        }

        /// <summary>
        /// Determines whether the specified enumerable contains exactly some expected values.
        /// </summary>
        /// <typeparam name="T">Type of the elements contained in the <see cref="expectedValues"/> array.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="expectedValues">The expected values.</param>
        /// <returns>
        ///   <c>true</c> if the specified enumerable contains exactly the specified expected values; throw a <see cref="FluentAssertionException"/> otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="NFluent.FluentAssertionException">the specified enumerable does not contains exactly the specified expected values.</exception>
        public static bool ContainsExactly<T>(this IEnumerable enumerable, params T[] expectedValues)
        {
            long i = 0;
            foreach (var obj in enumerable)
            {
                if (!object.Equals(obj, expectedValues[i]))
                {
                    var expectedNumberOfItemsDescription = string.Format(expectedValues.LongLength <= 1 ? "{0} item" : "{0} items", expectedValues.LongLength);

                    var enumerableCount = 0;
                    foreach (var item in enumerable)
                    {
                        enumerableCount++;
                    }

                    var foundNumberOfItemsDescription = string.Format(enumerableCount <= 1 ? "{0} item" : "{0} items", enumerableCount);
                    
                    throw new FluentAssertionException(string.Format("Found: '{0}' ({1}) instead of the expected '{2}' ({3}).", enumerable.ToAString(), foundNumberOfItemsDescription, expectedValues.ToAString(), expectedNumberOfItemsDescription));
                }

                i++;
            }
         
            return true;
        }

        /// <summary>
        /// Return a string containing all the <see cref="IEnumerable"/> elements, separated by a comma.
        /// </summary>
        /// <param name="enumerable">The enumerable to transform into a string.</param>
        /// <returns>A string containing all the <see cref="IEnumerable"/> elements, separated by a comma.</returns>
        public static string ToAString(this IEnumerable enumerable)
        {
            var firstTime = true;
            var sb = new StringBuilder();
            const string Separator = ", ";
            
            foreach (var obj in enumerable)
            {
                if (!firstTime)
                {
                    sb.Append(Separator);
                }

                sb.Append(obj.ToString());
                firstTime = false;
            }

            return sb.ToString();
        }
    }
}
