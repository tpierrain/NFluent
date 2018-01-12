// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ExtractingExtensions.cs" company="">
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Extension methods for exploiting enumerable content in a fluent manner (i.e. with auto completion support and in an english readable way).
    /// </summary>
    public static class ExtractingExtensions
    {
        /// <summary>
        /// Extract all the values of a given property given its name, from an enumerable collection of objects holding that property.
        /// </summary>
        /// <typeparam name="T">Type of the objects belonging to the initial enumerable collection.</typeparam>
        /// <param name="enumerable">The enumerable collection of objects.</param>
        /// <param name="propertyName">Name of the property to extract value from for every object of the collection.</param>
        /// <returns>
        /// An enumerable of all the property values for every <typeparamref name="T"/> objects in the <paramref name="enumerable"/>.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">The object of type <typeparamref name="T"/> don't have a property with the given property name.</exception>
        public static IEnumerable Extracting<T>(this IEnumerable<T> enumerable, string propertyName)
        {
            var type = typeof(T);
            var getter = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
 
            if (getter == null)
            {
                throw new InvalidOperationException(
                    $"Objects of expectedType {type} don't have property with name '{propertyName}'");
            }

            foreach (var o in enumerable)
            {
                var value = getter.GetValue(o, null);
                yield return value;
            }
        }
        
        /// <summary>
        /// Extract all the values of a given property given its name, from an enumerable collection of objects holding that property.
        /// </summary>
        /// <typeparam name="T">Type of the objects belonging to the initial enumerable collection.</typeparam>
        /// <param name="enumerable">The enumerable collection of objects.</param>
        /// <param name="propertyName">Name of the property to extract value from for every object of the collection.</param>
        /// <returns>
        /// An enumerable of all the property values for every <typeparamref name="T"/> objects in the <paramref name="enumerable"/>.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">The object of type <typeparamref name="T"/> don't have a property with the given property name.</exception>
        [Obsolete("Use Extracting instead.")]
        public static IEnumerable Properties<T>(this IEnumerable<T> enumerable, string propertyName)
        {
            return Extracting(enumerable, propertyName);
        }

        /// <summary>
        /// Extract all the values of a given property given its name, from an array of objects holding that property.
        /// </summary>
        /// <typeparam name="T">Type of the objects belonging to the array.</typeparam>
        /// <param name="array">The array of <typeparamref name="T"/>.</param>
        /// <param name="propertyName">Name of the property to extract value from for every object of the array.</param>
        /// <returns>
        /// An enumerable of all the property values for every <typeparamref name="T"/> objects in the <see cref="Array"/>.
        /// </returns>
        public static IEnumerable Extracting<T>(this T[] array, string propertyName)
        {
            var enumerableArray = array as IEnumerable<T>;
            return enumerableArray.Extracting(propertyName);
        }

        /// <summary>
        /// Extract all the values of a given property given its name, from an array of objects holding that property.
        /// </summary>
        /// <typeparam name="T">Type of the objects belonging to the array.</typeparam>
        /// <param name="array">The array of <typeparamref name="T"/>.</param>
        /// <param name="propertyName">Name of the property to extract value from for every object of the array.</param>
        /// <returns>
        /// An enumerable of all the property values for every <typeparamref name="T"/> objects in the <see cref="Array"/>.
        /// </returns>
        [Obsolete("Use Extracting instead.")]
        public static IEnumerable Properties<T>(this T[] array, string propertyName)
        {
            return Extracting(array, propertyName);
        }
    }
}