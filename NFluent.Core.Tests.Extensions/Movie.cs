// -------------------------------------------------------------------------------------------------------------------
// <copyright file="Movie.cs" company="">
//   Copyright 2013 Thomas Pierrain, Cyrille Dupuydauby
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace NFluent.Tests.Extensions
{
    using System.Collections.Generic;

    /// <summary>
    ///     Dummy class for unit testing purpose.
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