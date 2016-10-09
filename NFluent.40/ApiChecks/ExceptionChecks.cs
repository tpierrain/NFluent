namespace NFluent.ApiChecks
{
    using Kernel;

    /// <summary>
    /// This class hosts extensions methods related to exception related checks for APIs/lib.
    /// </summary>
    public static class ExceptionChecks
    {
        /// <summary>
        /// Provides access to the error message of the exception.
        /// </summary>
        /// <param name="check">Original checker for the exception.</param>
        /// <typeparam name="TParent">Exception type</typeparam>
        /// <returns>A checker for the exception message.</returns>
        public static ICheck<string> AndWhichMessage<TParent>(this ILambdaExceptionCheck<TParent> check)
        {
            var checker = check as LambdaExceptionCheck<TParent>;
            return new FluentCheck<string>(checker.Value.Message);
        }
    }
}
