namespace NFluent.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void HowPropertiesWorks()
        {
            var collection = new List<Student> {
                                     new Student { Name = "Thomas", Age = 38 }, 
                                     new Student { Name = "Achille", Age = 10, Nationality = Nationality.French}, 
                                     new Student { Name = "Anton", Age = 7, Nationality = Nationality.French}, 
                                     new Student { Name = "Arjun", Age = 7, Nationality = Nationality.Indian}
                                 };

            CollectionAssert.AreEqual(new[] {"Thomas", "Achille", "Anton", "Arjun"}, collection.Properties("Name"));
            CollectionAssert.AreEqual(new[] {38, 10, 7, 7}, collection.Properties("Age"));
            CollectionAssert.AreEqual(new[] {Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian}, collection.Properties("Nationality"));

            // assertThat(inn.getItems()).onProperty("name").containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
        }
    }
}
