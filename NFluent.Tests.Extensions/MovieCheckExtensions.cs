namespace NFluent.Tests.Extensions
{
    using NFluent.Extensions;

    public static class MovieCheckExtensions
    {
        public static ICheckLink<ICheck<Movie>> IsDirectedBy(this ICheck<Movie> check, string directorFullName)
        {
            var checkRunner = check as ICheckRunner<Movie>;
            var runnableCheck = check as IRunnableCheck<Movie>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        if (!runnableCheck.Value.Director.ToString().ToLower().Contains(directorFullName.ToLower()))
                        {
                            throw new FluentCheckException(string.Format("Not!"));
                        }
                    },
                "Effectively...");
        }

        public static ICheckLink<ICheck<Movie>> IsAFGreatMovie(this ICheck<Movie> check)
        {
            var checkRunner = check as ICheckRunner<Movie>;
            var runnableCheck = check as IRunnableCheck<Movie>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        if (!runnableCheck.Value.Name.Contains("Eternal sunshine of the spotless mind"))
                        {
                            throw new FluentCheckException(string.Format("[{0}]Is not a great movie", runnableCheck.Value.Name.ToStringProperlyFormated()));
                        }
                    },
                string.Format("[{0}]Is a great movie!", runnableCheck.Value.Name.ToStringProperlyFormated()));
        }
    }
}
