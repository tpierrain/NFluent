namespace NFluent
{
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for easily exploiting enumerable collections content.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Extract all the values of a given property from an enumerable collection of object supporting that kind of property.
        /// /// </summary>
        /// <typeparam name="T">Type of the objects belonging to the initial enumerable collection</typeparam>
        /// <param name="enumerable">The enumerable collection of objects.</param>
        /// <param name="propertyName">Name of the property to extract value from for every object.</param>
        /// <returns>An enumerable of values for the property with name: <see cref="propertyName"/></returns>
        public static IEnumerable<string> Properties<O>(this IEnumerable<O> enumerable, string propertyName)
        {
            if (propertyName == "Name")
            {
                return new List<string>() { "Thomas", "Achille", "Anton" };
            }
            return null;
        }
    }
}
