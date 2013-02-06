namespace NFest.Tests
{
    using System.Collections.Generic;

    using NFluent;
    using NFluent.Tests;

    using NUnit.Framework;

    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void HowPropertiesWorks()
        {
            var collection = new List<Student>()
                                 {
                                     new Student() { Name = "Thomas", Age = 38 },
                                     new Student() { Name = "Achille", Age = 10 },
                                     new Student() { Name = "Anton", Age = 7 }
                                 };

            CollectionAssert.AreEqual(new[] {"Thomas", "Achille", "Anton"}, collection.Properties<string, Student>("Name"));
            CollectionAssert.AreEqual(new[] {38, 10, 7}, collection.Properties<int, Student>("Age"));

            // assertThat(inn.getItems()).onProperty("name").containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
        }
    }
}
