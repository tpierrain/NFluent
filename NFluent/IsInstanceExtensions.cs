namespace NFluent
{
    using System;

    /// <summary>
    /// Extension methods for exploiting enumerable content in a fluent manner (i.e. with auto completion support and in an english readable way).
    /// </summary>
    public static class IsInstanceExtensions
    {
        /// <summary>
        /// Determines whether an object is an instance of a given type, and throws a <see cref="FluentAssertionException"/> with proper details if not the case.
        /// </summary>
        /// <param name="obj">The current object instance to check.</param>
        /// <param name="expectedType">The type we expect the object to be.</param>
        /// <returns>
        ///   <c>true</c> if this object is an instance of this type; otherwise, throws a <see cref="FluentAssertionException"/> with proper details.
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
        /// Verifies that an object is not an instance of a given type, and throws a <see cref="FluentAssertionException"/> with proper details if not the case.
        /// </summary>
        /// <param name="obj">The current object instance to check.</param>
        /// <param name="expectedType">The type we expect the object to be.</param>
        /// <returns>
        ///   <c>true</c> if this object is not an instance of this type; otherwise, throws a <see cref="FluentAssertionException"/> with proper details.
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
    }
}