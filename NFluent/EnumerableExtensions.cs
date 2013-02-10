namespace NFluent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

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
    }
}
