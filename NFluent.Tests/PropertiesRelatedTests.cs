namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Assert = NFluent.Assert;

    [TestFixture]
    public class PropertiesRelatedTests
    {
        #region Properties extension method with IEnumerable

        [Test]
        public void PropertiesWorksWithEnumerable()
        {
            var enumerable = new List<Person>
                                 {
                                     new Person { Name = "Thomas", Age = 38 },
                                     new Person { Name = "Achille", Age = 10, Nationality = Nationality.French },
                                     new Person { Name = "Anton", Age = 7, Nationality = Nationality.French },
                                     new Person { Name = "Arjun", Age = 7, Nationality = Nationality.Indian }
                                 };
            // TODO: find a way to get rid of the lack of type inference here (<Person, string> is not really fluent...)
            Assert.That(enumerable.Properties("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
            Assert.That(enumerable.Properties("Age")).ContainsExactly(38, 10, 7, 7);
            Assert.That(enumerable.Properties("Nationality")).ContainsExactly(Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian);

            // more fluent than the following classical NUnit way, isn't it? 
            //CollectionAssert.AreEquivalent(enumerable.Properties("Age"), new[] { 38, 10, 7, 7 });

            // NFluent relies intensively on intellisense to make you more productive in your day to day TDD

            // java version (FEST fluent assert)
            // assertThat(inn.getItems()).onProperty("name").containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PropertiesThrowInvalidOperationExceptionIfPropertyDoesNotExist()
        {
            var enumerable = new List<Person>
                                 {
                                     new Person { Name = "MethodMan", Age = 38 },
                                     new Person { Name = "GZA", Nationality = Nationality.American }
                                 };

            // Forced to enumerate the result so that the Properties extension method is executed (IEnumerable's lazy evaluation)
            foreach (var propertyValue in enumerable.Properties("Portnaouaq"))
            {
            }
        }

        [Test]
        public void PropertiesWorksEvenWithPrivateProperty()
        {
            var enumerable = new List<Person>
                                 {
                                     new Person { Name = "Ali G" },
                                     new Person { Name = "Borat" }
                                 };

            Assert.That(enumerable.Properties("PrivateHesitation")).ContainsExactly("Kamoulox !", "Kamoulox !");
        }

        #endregion

        #region Properties extension method with Array

        [Test]
        public void PropertiesWorksWithArray()
        {
            var array = new Person[]
                                 {
                                     new Person { Name = "Thomas", Age = 38 },
                                     new Person { Name = "Achille", Age = 10, Nationality = Nationality.French },
                                     new Person { Name = "Anton", Age = 7, Nationality = Nationality.French },
                                     new Person { Name = "Arjun", Age = 7, Nationality = Nationality.Indian }
                                 };

            Assert.That(array.Properties("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
            Assert.That(array.Properties("Age")).ContainsExactly(38, 10, 7, 7);
            Assert.That(array.Properties("Nationality")).ContainsExactly(Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian);
        }

        #endregion
    }
}
