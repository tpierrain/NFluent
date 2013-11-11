namespace NFluent
{

    /// <summary>
    /// A contract to force the type of the expected parent
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface IHasParentCheck<TParent> { }

    /// <summary>
    /// Provides exception check for lambda/action specific check.
    /// </summary>
    public interface ILambdaExceptionCheck<TParent> : IHasParentCheck<TParent>, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
    {
        /// <summary>
        /// Checks that message in an exception is correctly written
        /// </summary>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The code did not raised an exception of any type.</exception>
        ICheckLink<ILambdaExceptionCheck<TParent>> WithMessage(string exceptionMessage);

        /// <summary>
        /// Checks that a specific property of an exception has an expected value
        /// </summary>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The code did not raised an exception of any type.</exception>
        ICheckLink<ILambdaExceptionCheck<TParent>> WithProperty(string propertyName, object propertyValue);
    }
}