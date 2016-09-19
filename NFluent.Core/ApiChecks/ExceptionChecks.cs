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
        /// <param name="check"></param>typeparam>
        /// <returns></returns>
        public static ICheck<string> AndWhichMessage(this ILambdaExceptionCheck<RunTrace> check)
        {
            var checker = check as LambdaExceptionCheck<RunTrace>;
            return new FluentCheck<string>(checker.Value.Message);
        }
    }
}
