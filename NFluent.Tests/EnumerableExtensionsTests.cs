namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void HowPropertiesWorks()
        {
            var enumerable = new List<Person> 
                                {
                                     new Person { Name = "Thomas", Age = 38 }, 
                                     new Person { Name = "Achille", Age = 10, Nationality = Nationality.French }, 
                                     new Person { Name = "Anton", Age = 7, Nationality = Nationality.French }, 
                                     new Person { Name = "Arjun", Age = 7, Nationality = Nationality.Indian }
                                };

            CollectionAssert.AreEqual(new[] { "Thomas", "Achille", "Anton", "Arjun" }, enumerable.Properties("Name"));
            CollectionAssert.AreEqual(new[] { 38, 10, 7, 7 }, enumerable.Properties("Age"));
            CollectionAssert.AreEqual(new[] { Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian }, enumerable.Properties("Nationality"));

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
        public void PropertiesWorksWithPrivateProperty()
        {
            var enumerable = new List<Person> {
                                     new Person { Name = "Ali G", Nationality = Nationality.English }, 
                                     new Person { Name = "Borat", Nationality = Nationality.Kazakhstan }
                                 };

          CollectionAssert.AreEqual(new[] { "Kamoulox !", "Kamoulox !" }, enumerable.Properties("PrivatePassword"));
        }
    }
}
