namespace NFluent.Tests.Extensions
{
    using NFluent.Extensions;

    using Spike;

    using global::Spike;

    public static class MovieFluentAssertionExtensions
    {
        public static IFluentAssertion<Movie> IsDirectedBy(this IFluentAssertion<Movie> fluentAssertion, string directorFullName)
        {
            if (!fluentAssertion.Value.Director.ToString().ToLower().Contains(directorFullName.ToLower()))
            {
                throw new FluentAssertionException(string.Format("Not!"));    
            }

            return fluentAssertion;
        }

        public static IFluentAssertion<Movie> IsAFGreatMovie(this IFluentAssertion<Movie> fluentAssertion)
        {
            if (!fluentAssertion.Value.Name.Contains("Eternal sunshine of the spotless mind"))
            {
                throw new FluentAssertionException(string.Format("[{0}]Is not a great movie", fluentAssertion.Value.Name.ToStringProperlyFormated()));
            }

            return fluentAssertion;
        }
    }
}
