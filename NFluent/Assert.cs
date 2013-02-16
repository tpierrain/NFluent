namespace NFluent
{
    /// <summary>
    /// Contains a collection of static methods that implement the only assertions you need to be fluent (if your favorite .NET unit test framework doesn't provide it).
    /// If you are using NUnit, this <see cref="T:NFluent.Assert"/> class is useless (since NUnit provides now few Assert.That() overloads).
    /// On the other hand, xUnit users may benefit to this class.
    /// </summary>
    public static class Assert
    {
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throw a <see cref="T:FluentAssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition.</param>
        public static void That(bool condition)
        {
            // TODO: improve the default message of failing Assert.That
            That(condition, string.Empty);
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throw a <see cref="T:FluentAssertionException" />.
        /// </summary>
        /// <param name="condition">The evaluated condition.</param>
        /// <param name="message">The message for the <see cref="FluentAssertionException"/> if assertion fails.</param>
        /// <exception cref="FluentAssertionException">The assertion fails.</exception>
        public static void That(bool condition, string message)
        {
            if (!condition)
            {
                throw new FluentAssertionException(message);
            }
        }
    }
}
