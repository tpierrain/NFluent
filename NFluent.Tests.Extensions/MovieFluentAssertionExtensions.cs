namespace NFluent.Tests.Extensions
{
    using NFluent.Extensions;

    public static class MovieFluentAssertionExtensions
    {
        public static IChainableFluentAssertion<ICheck<Movie>> IsDirectedBy(this ICheck<Movie> check, string directorFullName)
        {
            var assertionRunner = check as IFluentAssertionRunner<Movie>;
            var runnableAssertion = check as IRunnableAssertion<Movie>;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        if (!runnableAssertion.Value.Director.ToString().ToLower().Contains(directorFullName.ToLower()))
                        {
                            throw new FluentAssertionException(string.Format("Not!"));
                        }
                    },
                "Effectively...");
        }

        public static IChainableFluentAssertion<ICheck<Movie>> IsAFGreatMovie(this ICheck<Movie> check)
        {
            var assertionRunner = check as IFluentAssertionRunner<Movie>;
            var runnableAssertion = check as IRunnableAssertion<Movie>;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        if (!runnableAssertion.Value.Name.Contains("Eternal sunshine of the spotless mind"))
                        {
                            throw new FluentAssertionException(string.Format("[{0}]Is not a great movie", runnableAssertion.Value.Name.ToStringProperlyFormated()));
                        }
                    },
                string.Format("[{0}]Is a great movie!", runnableAssertion.Value.Name.ToStringProperlyFormated()));
        }
    }
}
