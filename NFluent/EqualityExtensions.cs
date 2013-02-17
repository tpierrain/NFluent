namespace NFluent
{
    /// <summary>
    /// Extension methods for exploiting enumerable content in a fluent manner (i.e. with auto completion support and in an english readable way).
    /// </summary>
    public static class EqualityExtensions
    {
        /// <summary>
        /// Checks that an object is equal to another instance, and throws a <see cref="FluentAssertionException"/> if they are not equal.
        /// This method is provided as a replacement to the classic Equals() method because we need it to throw exception with proper message (which is of course not the case of the .NET Equals methods).
        /// </summary>
        /// <remarks>This method is not named EqualsOrThrowException to ensure english readability when used within an Assert.That() statement.</remarks>
        /// <param name="obj">The current object instance.</param>
        /// <param name="expected">The object that we expect to be equal.</param>
        /// <returns><c>true</c> if the two objects are equal, or throws a <see cref="FluentAssertionException"/> otherwise.</returns>
        /// <exception cref="NFluent.FluentAssertionException">The two objects are not equal.</exception>
        public static bool IsEqualTo(this object obj, object expected)
        {
            if (!object.Equals(obj, expected))
            {
                throw new FluentAssertionException(string.Format("[{0}] not equals to the expected [{1}]", obj.ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
            }

            return true;
        }

        /// <summary>
        /// Checks that an object is NOT equal to another instance, and throws a <see cref="FluentAssertionException"/> if they are equal.
        /// This method is provided as a replacement to the classic Equals() method because we need it to throw exception with proper message (which is of course not the case of the .NET Equals methods).
        /// </summary>
        /// <remarks>This method is not named NotEqualsOrThrowException to ensure english readability when used within an Assert.That() statement.</remarks>
        /// <param name="obj">The current object instance.</param>
        /// <param name="expected">The object that we expect to be NOT equal.</param>
        /// <returns><c>true</c> if the two objects are not equal, or throws a <see cref="FluentAssertionException"/> otherwise.</returns>
        /// <exception cref="NFluent.FluentAssertionException">The two objects are equal.</exception>
        public static bool IsNotEqualTo(this object obj, object expected)
        {
            if (object.Equals(obj, expected))
            {
                throw new FluentAssertionException(string.Format("[{0}] equals to the value [{1}] which is not expected.", obj.ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
            }

            return true;
        }
    }
}