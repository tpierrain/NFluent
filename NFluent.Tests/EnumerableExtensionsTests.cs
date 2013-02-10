namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class EnumerableExtensionsTests
    {
        #region Properties extension method

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

            Assert.That(enumerable.Properties("Name").ContainsExactly("Thomas", "Achille", "Anton", "Arjun"));
            Assert.That(enumerable.Properties("Age").ContainsExactly(38, 10, 7, 7));
            Assert.That(enumerable.Properties("Nationality").ContainsExactly(Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian));

            // java version
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

            Assert.That(enumerable.Properties("PrivateHesitation").ContainsExactly("Kamoulox !", "Kamoulox !"));
        }

        #endregion

        #region ContainsExactly extension method

        [Test]
        public void ContainsExactlyWorks()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers.ContainsExactly(1, 2, 3, 4, 5, 666));

            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Assert.That(guitarHeroes.ContainsExactly("Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell"));
        }
        
        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"Found: [""Hendrix"", ""Paco de Lucia"", ""Django Reinhardt"", ""Baden Powell""] (4 items) instead of the expected [""Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell""] (1 item).")]
        public void ContainsExactlyThrowExplicitExceptionMessage()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Assert.That(guitarHeroes.ContainsExactly("Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell"));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Found: [1, 2, 3, 4, 5, 666] (6 items) instead of the expected [42, 42, 42] (3 items).")]
        public void ContainsExactlyThrowExceptionWhenFalse()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers.ContainsExactly(42, 42, 42));
        }

        #endregion

        #region EqualsExactly extension method

        [Test]
        public void EqualsExactlyWorks()
        {
            var first = "Son of a test";
            Assert.That(first.EqualsExactly("Son of a test"));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""Son of a test""] not equals to the expected [""no way""]")]
        public void EqualsExactlyThrowExceptionWhenFalse()
        {
            var first = "Son of a test";
            Assert.That(first.EqualsExactly("no way"));
        }

        #endregion

        #region ToAString extension method

        [Test]
        public void ToAStringParticularBehaviourWithStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Assert.That(guitarHeroes.ToAString().EqualsExactly(@"""Hendrix"", ""Paco de Lucia"", ""Django Reinhardt"", ""Baden Powell"""));
        }

        [Test]
        public void ToAStringWorksFineWithStrings()
        {
            var guitarHeroes = new[] { 93, 56, 35, 75 };
            Assert.That(guitarHeroes.ToAString().EqualsExactly("93, 56, 35, 75"));
        }

        #endregion
    }
}
