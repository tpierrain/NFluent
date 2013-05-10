namespace NFluent.Tests.Extensions
{
    using NFluent.Extensions;

    public static class MovieFluentAssertionExtensions
    {
        public static IChainableFluentAssertion<IFluentAssertion<Movie>> IsDirectedBy(this IFluentAssertion<Movie> fluentAssertion, string directorFullName)
        {
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<Movie>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<Movie>;

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

        public static IChainableFluentAssertion<IFluentAssertion<Movie>> IsAFGreatMovie(this IFluentAssertion<Movie> fluentAssertion)
        {
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<Movie>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<Movie>;

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
