// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ReadmeRelatedTests.cs" company="">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    using NUnit.Framework;

    /// <summary>
    /// Source code which is referenced within the Readme.md file.
    /// </summary>
    [TestFixture]
    public class ReadmeRelatedTests
    {
        private const int EnoughMillisecondsForMutualizedSoftwareFactorySlaveToSucceed = 6000 * 2;

        #region dummy fields for the NFluentMotto method to compile

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
        private static readonly TddForMarketing TDD = new TddForMarketing();
        private static readonly NFluentForMarketing NFluent = new NFluentForMarketing();

        #endregion

        /// <summary>
        /// Methods to screenshot and copy and paste the NFluent motto.
        /// </summary>
        public void NFluentMotto()
        {
            // Assert is dead!                                         
            Check.That(TDD).With(NFluent).IsAnInstanceOf<Awesomeness>();
        }
        
        [Test]
        public void CodeSnippetForReadmeMarkdownFile()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).Contains(3, 5, 666);

            integers = new int[] { 1, 2, 3 };
            Check.That(integers).IsOnlyMadeOf(3, 2, 1);

            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Check.That(guitarHeroes).ContainsExactly("Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell");

            var camus = new Person() { Name = "Camus" };
            var sartre = new Person() { Name = "Sartre" };
            Check.That(camus).IsNotEqualTo(sartre).And.IsInstanceOf<Person>();

            var heroes = "Batman and Robin";
            Check.That(heroes).Not.Contains("Joker").And.StartsWith("Bat").And.Contains("Robin");

            int? one = 1;
            Check.That(one).HasAValue().Which.IsPositive().And.IsEqualTo(1);

            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).IsNotEqualTo(Nationality.Korean);

            string motivationalSaying = "Failure is mother of success.";
            Check.That(motivationalSaying).IsNotInstanceOf<int>();

            Check.That('A').IsSameLetterAs('a');
        }

        [Test]
        public void PlayingWithProperties()
        {
            var persons = new List<Person>
                            {
                                new Person { Name = "Thomas", Age = 38 },
                                new Person { Name = "Achille", Age = 10, Nationality = Nationality.French },
                                new Person { Name = "Anton", Age = 7, Nationality = Nationality.French },
                                new Person { Name = "Arjun", Age = 7, Nationality = Nationality.Indian }
                            };

            Check.That(persons.Extracting("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
            Check.That(persons.Extracting("Age")).ContainsExactly(38, 10, 7, 7);
            Check.That(persons.Extracting("Nationality")).ContainsExactly(Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian);

            // more fluent than the following classical NUnit way, isn't it? 
            // CollectionAssert.AreEquivalent(persons.Properties("Age"), new[] { 38, 10, 7, 7 });

            // it's maybe even more fluent than the java versions

            // FEST fluent assert v 2.x:
            // assertThat(extractProperty("name" , String.class).from(inn.getItems())).containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
            
            // FEST fluent assert v 1.x:
            // assertThat(inn.getItems()).onProperty("name").containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
        }

        [Test]
        public void PlayingWithLambda()
        {
            // Works also with lambda for exception checking
            Check.ThatCode(() => { throw new InvalidOperationException(); }).Throws<InvalidOperationException>();

            // or execution duration checking
            Check.ThatCode(() => Thread.Sleep(30)).LastsLessThan(EnoughMillisecondsForMutualizedSoftwareFactorySlaveToSucceed, TimeUnit.Milliseconds);
        }
    }
}
