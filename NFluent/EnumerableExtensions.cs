namespace NFluent
{
    using System.Collections;
    using System.Linq;
    using System.Text;

    // TODO: check performances

    /// <summary>
    /// Extension methods for exploiting enumerable content in a fluent manner (i.e. with auto completion support and in an english readable way).
    /// </summary>
    public static class EnumerableExtensions
    {
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
        /// Verifies whether the enumerable is empty, and throws a <see cref="FluentAssertionException" /> if not empty.
        /// </summary>
        /// <param name="enumerable">The enumerable to check.</param>
        /// <returns>
        ///   <c>true</c> if the enumerable is empty; throws a <see cref="FluentAssertionException"/> otherwise.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual enumeration is not empty.</exception>
        public static bool IsEmpty(this IEnumerable enumerable)
        {
            if (enumerable.Cast<object>().Any())
            {
                throw new FluentAssertionException(string.Format("Enumerable not empty. Contains the element(s): [{0}].", enumerable.ToEnumeratedString()));
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

                if (obj == null)
                {
                    sb.Append("null");
                }
                else
                {
                    sb.Append(obj.ToStringProperlyFormated());
                }

                firstTime = false;
                itemsCount++;
            }

            return sb.ToString();
        }
    }
}
