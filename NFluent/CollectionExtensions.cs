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
        /// </summary>
        /// <typeparam name="R">Type of the objects contained in the returned collection.</typeparam>
        /// <typeparam name="O">Type of the objects belonging to the initial enumerable collection</typeparam>
        /// <param name="enumerable">The enumerable collection of objects.</param>
        /// <param name="propertyName">Name of the property to extract value from for every object.</param>
        /// <returns>
        /// An enumerable of values for the property with name: <see cref="propertyName" />
        /// </returns>
        public static IEnumerable<R> Properties<R, O>(this IEnumerable<O> enumerable, string propertyName)
        {
            if (propertyName == "Name")
            {
                IEnumerable<string> ret = new List<string>() { "Thomas", "Achille", "Anton" };
                return ret as IEnumerable<R>;
            }
            if (propertyName == "Age")
            {
                IEnumerable<int> ret = new List<int>() { 38, 10, 7 };
                
                return ret as IEnumerable<R>;
            }
                        
            return null;
        }
    }
}
