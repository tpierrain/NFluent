namespace NFluent
{
    public static class EqualityHelper
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
        public static void IsEqualTo(object sut, object expected)
        {
            if (!object.Equals(sut, expected))
            {
                throw new FluentAssertionException(string.Format("[{0}] not equals to the expected [{1}]", sut.ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
            }
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
        public static void IsNotEqualTo(object sut, object expected)
        {
            if (object.Equals(sut, expected))
            {
                throw new FluentAssertionException(string.Format("[{0}] equals to the value [{1}] which is not expected.", sut.ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
            }
        }
    }
}