namespace NFluent.ApiChecks
{
    using Kernel;

    /// <summary>
    /// This class hosts extensions methods related to exception related checks for APIs/lib.
    /// </summary>
    internal static class ExceptionChecks
    {
        /// <summary>
        /// Provides access to the error message of the exception.
        /// </summary>
        /// <param name="check"></param>
        /// <typeparam name="TParent"></typeparam>
        /// <returns></returns>
        public static ICheck<string> AndWhichMessage<TParent>(this ILambdaExceptionCheck<TParent> check)
        {
            var checker = check as LambdaExceptionCheck<TParent>;
            return new FluentCheck<string>(checker.Value.Message);
        }
    }
}
