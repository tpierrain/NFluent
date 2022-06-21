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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Extensions;
    using System.Linq;

    /// <summary>
    ///     Extension methods for adding new fluent methods to enumerable.
    /// </summary>
    internal static class EnumerableExtensions
    {
        private const string Ellipsis = "...";
        private const string Separator = ",";
        public const long NullIndex = -1;

        /// <summary>
        ///     Returns the number of items present within the specified enumerable (returns 0 if the enumerable is null).
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>The number of items present within the specified enumerable (returns 0 if the enumerable is null).</returns>
        public static long Count(this IEnumerable enumerable)
        {
            // Stryker disable once Block: Mutation does not alter behaviour<
            if (enumerable is ICollection collection)
            {
                return collection.Count;
            }
            return enumerable?.Cast<object>().LongCount() ?? 0;
        }

        /// <summary>
        /// Returns the dimension of the array along one of its axes
        /// </summary>
        /// <param name="array">array</param>
        /// <param name="dimension">axes number</param>
        /// <returns>the size of the requested dimension</returns>
        public static int SizeOfDimension(this Array array, int dimension)
        {
            return array.GetUpperBound(dimension) - array.GetLowerBound(dimension) + 1;
        }

        /// <summary>
        ///     Return a string containing all the elements of an <see cref="IEnumerable" />, separated by a given separator.
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="fromIndex"></param>
        /// <param name="len"></param>
        /// <returns>A string containing all the <see cref="IEnumerable" /> elements, separated by the given separator.</returns>
        public static string ToEnumeratedString(this IEnumerable enumerable, long fromIndex = NullIndex, long len = 0)
        {
            if (enumerable is Array array)
            {
                return array.ArrayToStringProperlyFormatted(fromIndex, len);
            }
            return enumerable.ToEnumeratedStringAdvanced(fromIndex, len, new List<object>());
        }

        /// <summary>
        /// Cast all items within an enumeration
        /// </summary>
        /// <typeparam name="T">Target type for item</typeparam>
        /// <param name="enumerable">enumerable to cast</param>
        /// <returns></returns>
        public static IEnumerable<T> AmbitiousCast<T>(this IEnumerable enumerable)
        {
            return new MyEnumerable<T>(enumerable);
        }

        private static string ToEnumeratedStringAdvanced(this IEnumerable enumerable,
            long referenceIndex, long numberOfItemsToOutput, ICollection<object> seen)
        {
            if (seen.Contains(enumerable))
            {
                return "{{"+Ellipsis+"}}";
            }
            var sb = new StringBuilder();
            sb.Append('{');
            var iterator = enumerable.GetEnumerator();
            var copy = new List<object>(seen) {enumerable};
            var itemCount = enumerable is ICollection collection ? collection.Count : long.MaxValue;
            var firstIndex = Math.Max(0, referenceIndex - (numberOfItemsToOutput / 2));
            var lastItem = numberOfItemsToOutput == 0 ? int.MaxValue : firstIndex + numberOfItemsToOutput;

            // we skip the first items
            if (firstIndex > 0)
            {
                for (var i = 0; i < firstIndex; i++)
                {
                    iterator.MoveNext();
                }
                sb.Append(Ellipsis);
            }

            if (itemCount == 1 || lastItem == 1)
            {
                // we do not highlight a specific item if there is only one item
                referenceIndex = NullIndex;
            }
            // items to display
            for (var i = firstIndex; i < lastItem; i++)
            {
                if (!iterator.MoveNext())
                {
                    // end of enumeration
                    break;
                }

                if (i != 0)
                {
                    // add comma
                    sb.Append(Separator);
                }

                var item = iterator.Current;
                if (i == referenceIndex)
                {
                    sb.Append('*');
                }
                switch (item)
                {
                    case string s:
                        sb.Append(s.ToStringProperlyFormatted());
                        break;
                    case IEnumerable sub:
                        sb.Append(sub.ToEnumeratedStringAdvanced(NullIndex, numberOfItemsToOutput, copy));
                        break;
                    default:
                        sb.Append(item.ToStringProperlyFormatted());
                        break;
                }
                if (i == referenceIndex)
                {
                    sb.Append('*');
                }

                if (i == lastItem-1 && lastItem < itemCount)
                {
                    sb.Append(Ellipsis);
                }
            }

            sb.Append('}');

            return sb.ToString();
        }

        private static string ArrayToStringProperlyFormatted(this Array array, long referenceIndex, long numberOfItems)
        {
            var result = new StringBuilder();
            var indices = new long[array.Rank];
            result.Append('{');
            
            var firstIndex = Math.Max(0, referenceIndex - (numberOfItems / 2));
            var lastItem = numberOfItems == 0 ? array.Length : Math.Min(firstIndex + numberOfItems, array.LongLength());
            if (firstIndex > 0)
            {
                result.Append(Ellipsis);
                result.Append(Separator);
            }

            if (lastItem == 1)
            {
                // we do not highlight a specific item if there is only one item
                referenceIndex = NullIndex;
            }
            for (var i = firstIndex; i < lastItem; i++)
            {
                var closing = HandleDimensions(array, i, result, indices);
                if (i == referenceIndex)
                {
                    result.Append('*');
                }
                result.Append(array.GetValue(indices).ToStringProperlyFormatted());
                if (i == referenceIndex)
                {
                    result.Append('*');
                }
                result.Append(closing);
                if (i != array.Length - 1)
                {
                    result.Append(Separator);
                }
            }

            if (lastItem < array.Length)
            {
                result.Append(Ellipsis);
            }
            result.Append('}');
            return result.ToString();
        }

        private static string HandleDimensions(Array array, long i, StringBuilder result, IList<long> indices)
        {
            var temp = i;
            var closing = string.Empty;
            var canOpen = true;
            var canClose = true;
            for (var j = array.Rank - 1; j >= 0; j--)
            {
                var dimension = array.GetLongLength(j);
                var currentIndex = temp % dimension;
                if (canOpen && currentIndex == 0)
                {
                    if (j > 0)
                    {
                        result.Append('{');
                    }
                }
                else
                {
                    canOpen = false;
                }
                if (canClose &&  currentIndex == dimension - 1)
                {
                    if (j > 0)
                    {
                        closing += '}';
                    }
                }
                else
                {
                    canClose = false;
                }
                indices[j] = currentIndex + array.GetLowerBound(j);
                temp /= dimension;
            }

            return closing;
        }

        private class MyEnumerable<T>: IEnumerable<T>
        {
            private readonly IEnumerable wrapped;

            public MyEnumerable(IEnumerable wrapped)
            {
                this.wrapped = wrapped;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new MyEnumerator(((IEnumerable) this).GetEnumerator());
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.wrapped.GetEnumerator();
            }

            private class MyEnumerator: IEnumerator<T>
            {
                private readonly IEnumerator wrapped;

                public MyEnumerator(IEnumerator wrapped)
                {
                    this.wrapped = wrapped;
                }

                public void Dispose()
                {
                    if (this.wrapped is IDisposable disposable)
                    {
                        // Stryker disable once Statement: Can't test this line
                        disposable.Dispose();
                    }
                }

                public bool MoveNext()
                {
                    return this.wrapped.MoveNext();
                }

                [ExcludeFromCodeCoverage]
                public void Reset()
                {
                    this.wrapped.Reset();
                }

                public T Current => (T) this.wrapped.Current;

                [ExcludeFromCodeCoverage]
                object IEnumerator.Current => this.Current;
            }
        }
    }
}