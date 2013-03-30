namespace NFluent.Tests.Extensions
{
    using NFluent.Extensions;

    using Spike.Core;

    public static class MovieFluentAssertionExtensions
    {
        public static IFluentAssertion<Movie> IsDirectedBy(this IFluentAssertion<Movie> fluentAssertion, string directorFullName)
        {
            if (!fluentAssertion.Sut.Director.ToString().ToLower().Contains(directorFullName.ToLower()))
            {
                throw new FluentAssertionException(string.Format("Not!"));    
            }

            return fluentAssertion;
        }

        public static IFluentAssertion<Movie> IsAFGreatMovie(this IFluentAssertion<Movie> fluentAssertion)
        {
            if (!fluentAssertion.Sut.Name.Contains("Eternal sunshine of the spotless mind"))
            {
                throw new FluentAssertionException(string.Format("[{0}]Is not a great movie", fluentAssertion.Sut.Name.ToStringProperlyFormated()));
            }

            return fluentAssertion;
        }
    }
}
