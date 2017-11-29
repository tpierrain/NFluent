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

namespace NFluent
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Extensions;
#if !DOTNET_30 && !DOTNET_20
    using System.Linq;
#endif

    /// <summary>
    ///     Extension methods for adding new fluent methods to enumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Returns the number of items present within the specified enumerable (returns 0 if the enumerable is null).
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>The number of items present within the specified enumerable (returns 0 if the enumerable is null).</returns>
        public static long Count(this IEnumerable enumerable)
        {
            return enumerable?.Cast<object>().LongCount() ?? 0;
        }

        /// <summary>
        ///     Return a string containing all the <see cref="IEnumerable" /> elements, separated by a comma.
        /// </summary>
        /// <param name="enumerable">The enumerable to transform into a string.</param>
        /// <returns>
        ///     A string containing all the <see cref="IEnumerable" /> elements, separated by a comma.
        /// </returns>
        public static string ToEnumeratedString(this IEnumerable enumerable)
        {
            return ToEnumeratedString(enumerable, out _);
        }

        /// <summary>
        ///     Return a string containing all the elements of an <see cref="IEnumerable" />, separated by a given separator.
        /// </summary>
        /// <param name="enumerable">The enumerable to transform into a string.</param>
        /// <param name="itemsCount">The number of items within the <see cref="IEnumerable" />.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>
        ///     A string containing all the <see cref="IEnumerable" /> elements, separated by a separator.
        /// </returns>
        public static string ToEnumeratedString(this IEnumerable enumerable, out long itemsCount, string separator)
        {
            return SafeRecursedEnumeratedString(enumerable, out itemsCount, separator, new List<object>());
        }

        /// <summary>
        ///     Return a string containing all the elements of an <see cref="IEnumerable" />, separated by a comma.
        /// </summary>
        /// <param name="enumerable">The enumerable to transform into a string.</param>
        /// <param name="itemsCount">The number of items within the <see cref="IEnumerable" />.</param>
        /// <returns>
        ///     A string containing all the <see cref="IEnumerable" /> elements, separated by a comma.
        /// </returns>
        public static string ToEnumeratedString(this IEnumerable enumerable, out long itemsCount)
        {
            const string separator = ", ";
            return ToEnumeratedString(enumerable, out itemsCount, separator);
        }

        /// <summary>
        ///     Return a string containing all the elements of an <see cref="IEnumerable" />, separated by a given separator.
        /// </summary>
        /// <param name="enumerable">The enumerable to transform into a string.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>A string containing all the <see cref="IEnumerable" /> elements, separated by the given separator.</returns>
        /// A string containing all the
        /// <see cref="IEnumerable" />
        /// elements, separated by the given separator.
        public static string ToEnumeratedString(this IEnumerable enumerable, string separator)
        {
            return ToEnumeratedString(enumerable, out _, separator);
        }

        private static string SafeRecursedEnumeratedString(this IEnumerable enumerable, out long itemsCount,
            string separator, List<object> stack)
        {
            var firstTime = true;
            var sb = new StringBuilder();
            itemsCount = 0;
            if (enumerable == null)
                return "null";
            var copy = new List<object>(stack) {enumerable};
            foreach (var obj in enumerable)
            {
                if (!firstTime)
                    sb.Append(separator);
                switch (obj)
                {
                    case string s:
                        sb.Append(s.ToStringProperlyFormatted());
                        break;
                    case IEnumerable sub:
                        sb.Append(copy.Contains(sub)
                            ? "(...)"
                            : sub.SafeRecursedEnumeratedString(out _, separator, copy));
                        break;
                    default:
                        sb.Append(obj.ToStringProperlyFormatted());
                        break;
                }

                firstTime = false;
                itemsCount++;
            }

            return sb.ToString();
        }
    }
}