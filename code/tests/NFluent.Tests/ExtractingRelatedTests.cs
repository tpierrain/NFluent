// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ExtractingRelatedTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class ExtractingRelatedTests
    {
        #region Extracting extension method with IEnumerable

        [Test]
        public void ExtractingWorksWithEnumerable()
        {
            var persons = new List<Person>
            {
                new Person {Name = "Thomas", Age = 38},
                new Person {Name = "Achille", Age = 10, Nationality = Nationality.French},
                new Person {Name = "Anton", Age = 7, Nationality = Nationality.French},
                new Person {Name = "Arjun", Age = 7, Nationality = Nationality.Indian}
            };

            Check.That(persons.Extracting("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
            Check.That(persons.Extracting("Age")).ContainsExactly(38, 10, 7, 7);
            Check.That(persons.Extracting("Nationality")).ContainsExactly(Nationality.Unknown, Nationality.French,
                Nationality.French, Nationality.Indian);

            // more fluent than the following classical NUnit way, isn't it? 
            // CollectionAssert.AreEquivalent(enumerable.Extracting("Age"), new[] { 38, 10, 7, 7 });

            // it's maybe even more fluent than the java versions
            // FEST fluent assert v 2.x:
            // assertThat(extractProperty("name" , String.class).from(inn.getItems())).containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
            // FEST fluent assert v 1.x:
            // assertThat(inn.getItems()).onProperty("name").containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
        }

        [Test]
        public void LambdaExtractingWorksWithEnumerable()
        {
            var persons = new List<Person>
            {
                new Person {Name = "Thomas", Age = 38},
                new Person {Name = "Achille", Age = 10, Nationality = Nationality.French},
                new Person {Name = "Anton", Age = 7, Nationality = Nationality.French},
                new Person {Name = "Arjun", Age = 7, Nationality = Nationality.Indian}
            };

            Check.That(persons.Extracting(p => p.Name)).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
            Check.That(persons.Extracting(p => p.Age)).ContainsExactly(38, 10, 7, 7);
            Check.That(persons.Extracting(p => p.Nationality)).ContainsExactly(Nationality.Unknown, Nationality.French,
                Nationality.French, Nationality.Indian);
        }

        [Test]
        public void ExtractingThrowInvalidOperationExceptionIfPropertyDoesNotExist()
        {
            var musicians = new List<Person>
            {
                new Person {Name = "MethodMan", Age = 38},
                new Person {Name = "GZA", Nationality = Nationality.American}
            };

            Check.ThatCode(() =>
                {
                    // Forced to enumerate the result so that the Extracting extension method is executed (IEnumerable's lazy evaluation)
                    foreach (var unused in musicians.Extracting("Portnaouaq"))
                    {
                    }
                })
                .Throws<InvalidOperationException>()
                .WithMessage("Objects of expectedType NFluent.Tests.Person don't have property with name 'Portnaouaq'");
        }

        [Test]
        public void ExtractingWorksEvenWithPrivateProperty()
        {
            var persons = new List<Person>
            {
                new Person {Name = "Ali G"},
                new Person {Name = "Borat"}
            };

            Check.That(persons.Extracting("PrivateHesitation")).ContainsExactly("Kamoulox !", "Kamoulox !");
        }

        #endregion

        #region Extracting extension method with Array

        [Test]
        public void ExtractingWorksWithArray()
        {
            Person[] persons = new[]
            {
                new Person {Name = "Thomas", Age = 38},
                new Person {Name = "Achille", Age = 10, Nationality = Nationality.French},
                new Person {Name = "Anton", Age = 7, Nationality = Nationality.French},
                new Person {Name = "Arjun", Age = 7, Nationality = Nationality.Indian}
            };

            Check.That(persons.Extracting("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
            Check.That(persons.Extracting("Age")).ContainsExactly(38, 10, 7, 7);
            Check.That(persons.Extracting("Nationality")).ContainsExactly(Nationality.Unknown, Nationality.French,
                Nationality.French, Nationality.Indian);

            Check.That(persons.Extracting("Age")).IsEquivalentTo(7, 7, 10, 38);
        }

        [Test]
        public void LambdaExtractingWorksWithArray()
        {
            Person[] persons = new[]
            {
                new Person {Name = "Thomas", Age = 38},
                new Person {Name = "Achille", Age = 10, Nationality = Nationality.French},
                new Person {Name = "Anton", Age = 7, Nationality = Nationality.French},
                new Person {Name = "Arjun", Age = 7, Nationality = Nationality.Indian}
            };

            Check.That(persons.Extracting(p => p.Name)).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
            Check.That(persons.Extracting(p => p.Age)).ContainsExactly(38, 10, 7, 7);
            Check.That(persons.Extracting(p => p.Nationality)).ContainsExactly(Nationality.Unknown, Nationality.French,
                Nationality.French, Nationality.Indian);
        }

#pragma warning disable 618
        [Test]
        public void PropertiesWorksWithArray()
        {
            Person[] persons = new[]
            {
                new Person {Name = "Thomas", Age = 38},
                new Person {Name = "Achille", Age = 10, Nationality = Nationality.French},
                new Person {Name = "Anton", Age = 7, Nationality = Nationality.French},
                new Person {Name = "Arjun", Age = 7, Nationality = Nationality.Indian}
            };

            Check.That(persons.Properties("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
        }

        [Test]
        public void PropertiesWorksWithEnumerable()
        {
            var persons = new List<Person>
            {
                new Person {Name = "Thomas", Age = 38},
                new Person {Name = "Achille", Age = 10, Nationality = Nationality.French},
                new Person {Name = "Anton", Age = 7, Nationality = Nationality.French},
                new Person {Name = "Arjun", Age = 7, Nationality = Nationality.Indian}
            };

            Check.That(persons.Properties("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
        }
#pragma warning restore 618

        #endregion
    }
}