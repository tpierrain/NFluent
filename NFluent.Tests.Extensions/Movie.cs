namespace NFluent.Tests.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    /// Dummy class for unit testing purpose.
    /// </summary>
    public class Movie
    {
        public string Name { get; private set; }
        public Person Director { get; private set; }
        public List<Nationality> ProducersNationalities { get; private set; }

        /// <summary>
        /// Initializes a new instance of a  <see cref="Movie" />.
        /// </summary>
        /// <param name="name">The name of the movie.</param>
        /// <param name="director">The movie's director.</param>
        /// <param name="producersNationalities">The nationalities of the producers of the movie.</param>
        public Movie(string name, Person director, List<Nationality> producersNationalities)
        {
            this.Name = name;
            this.Director = director;
            this.ProducersNationalities = producersNationalities;
        }
    }
}
