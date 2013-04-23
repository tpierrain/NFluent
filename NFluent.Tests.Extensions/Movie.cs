namespace NFluent.Tests.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    /// Dummy class for unit testing purpose.
    /// </summary>
    public class Movie
    {
        public Movie(string name, Person director, List<Nationality> producersNationalities)
        {
            this.Name = name;
            this.Director = director;
            this.ProducersNationalities = producersNationalities;
        }

        public string Name { get; private set; }

        public Person Director { get; private set; }

        public List<Nationality> ProducersNationalities { get; private set; }
    }
}
