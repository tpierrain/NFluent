// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// ReSharper disable CheckNamespace
namespace NFluent
// ReSharper restore CheckNamespace
{
    using System.Collections;
    using System.Text;

    using NFluent.Extensions;

    /// <summary>
    /// Extension methods for adding new fluent methods to enumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Return a string containing all the <see cref="IEnumerable" /> elements, separated by a comma.
        /// </summary>
        /// <param name="enumerable">The enumerable to transform into a string.</param>
        /// <returns>
        /// A string containing all the <see cref="IEnumerable" /> elements, separated by a comma.
        /// </returns>
        public static string ToEnumeratedString(this IEnumerable enumerable)
        {
            long itemsCount;
            return ToEnumeratedString(enumerable, out itemsCount);
        }

        /// <summary>
        /// Return a string containing all the elements of an <see cref="IEnumerable" />, separated by a given separator.
        /// </summary>
        /// <param name="enumerable">The enumerable to transform into a string.</param>
        /// <param name="itemsCount">The number of items within the <see cref="IEnumerable" />.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>
        /// A string containing all the <see cref="IEnumerable" /> elements, separated by a separator.
        /// </returns>
        public static string ToEnumeratedString(this IEnumerable enumerable, out long itemsCount, string separator)
        {
            var firstTime = true;
            var sb = new StringBuilder();
            itemsCount = 0;
            if (enumerable == null)
            {
                return "null";
            }

            foreach (var obj in enumerable)
            {
                if (!firstTime)
                {
                    sb.Append(separator);
                }

                sb.Append(obj == null ? "null" : obj.ToStringProperlyFormated());

                firstTime = false;
                itemsCount++;
            }

            return sb.ToString();
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
            const string Separator = ", ";
            return ToEnumeratedString(enumerable, out itemsCount, Separator);
        }

        /// <summary>
        /// Return a string containing all the elements of an <see cref="IEnumerable" />, separated by a given separator.
        /// </summary>
        /// <param name="enumerable">The enumerable to transform into a string.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>A string containing all the <see cref="IEnumerable" /> elements, separated by the given separator.</returns>
        /// A string containing all the <see cref="IEnumerable" /> elements, separated by the given separator.
        public static string ToEnumeratedString(this IEnumerable enumerable, string separator)
        {
            long dontCareItemsCount;
            return ToEnumeratedString(enumerable, out dontCareItemsCount, separator);
        }
    }
}
