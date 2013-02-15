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

            // more fluent than the following classical NUnit way, isn't it? 
            CollectionAssert.AreEquivalent(enumerable.Properties("Age"), new[] { 38, 10, 7, 7 });

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

            Assert.That(enumerable.Properties("PrivateHesitation").ContainsExactly("Kamoulox !", "Kamoulox !"));
        }

        #endregion

        #region Contains extension method

        [Test]
        public void ContainsWork()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers.Contains(3, 5, 666));

            var enumerable = new List<string>() { "un", "dos", "tres" };
            Assert.That(enumerable.Contains("dos"));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The array does not contain the expected value(s): [666, 1974].")]
        public void ContainsThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new int[] { 1, 2, 3 };
            Assert.That(integers.Contains(3, 2, 666, 1974));
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Found: [1, 2, 3, 4, 5, 666] (6 items) instead of the expected [42, 42, 42] (3 items).")]
        public void ContainsExactlyThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers.ContainsExactly(42, 42, 42));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"Found: [""Hendrix"", ""Paco de Lucia"", ""Django Reinhardt"", ""Baden Powell""] (4 items) instead of the expected [""Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell""] (1 item).")]
        public void ContainsExactlyThrowsExceptionWithClearStatusWhenFailsWithStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Assert.That(guitarHeroes.ContainsExactly("Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell"));
        }

        [Test]
        public void ContainsExactlyAlsoWorksWithEnumerableParameter()
        {
            Assert.That(InstantiateDirectors().Properties("Name").ContainsExactly(InstantiateDirectors().Properties("Name")));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"Found: [""Michel Gondry"", ""Joon-ho Bong"", ""Darren Aronofsky""] (3 items) instead of the expected [""Steve Tesich"", ""Albert Camus"", ""Eiji Yoshikawa"", ""Friedrich Nietzsche""] (4 items).")]
        public void ContainsExactlyThrowsExceptionWithClearStatusWhenFailsWithEnumerable()
        {
            Assert.That(InstantiateDirectors().Properties("Name").ContainsExactly(InstantiateWriters().Properties("Name")));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"Found: [""Michel Gondry"", ""Joon-ho Bong"", ""Darren Aronofsky""] (3 items) instead of the expected [] (0 item).")]
        public void ContainsExactlyThrowsExceptionWithClearStatusWhenFailsWithNullEnumerable()
        {
            Assert.That(InstantiateDirectors().Properties("Name").ContainsExactly(null));
        }

        #endregion

        #region EqualsExactly extension method

        [Test]
        public void EqualsExactlyWorks()
        {
            var first = "Son of a test";
            Assert.That(first.EqualsExactly("Son of a test"));

            var array = new int[] { 45, 43, 54, 666 };
            var otherReference = array;

            Assert.That(array.EqualsExactly(array));
            Assert.That(array.EqualsExactly(otherReference));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException))]
        public void EqualsExactlyThrowsExceptionWhenFails()
        {
            var array = new int[] { 45, 43, 54, 666 };
            var otherEquivalentArray = new int[] { 45, 43, 54, 666 };

            Assert.That(array.EqualsExactly(otherEquivalentArray));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""Son of a test""] not equals to the expected [""no way""]")]
        public void EqualsExactlyThrowsExceptionWithClearStatusWhenFails()
        {
            var first = "Son of a test";
            Assert.That(first.EqualsExactly("no way"));
        }

        #endregion

        #region HasSize extension method

        [Test]
        public void HasSizeWorksWithArray()
        {
            var array = new int[] { 45, 43, 54, 666 };

            Assert.That(array.HasSize(4));
        }

        [Test]
        public void HasSizeWorksWithEnumerable()
        {
            var enumerable = new List<int>() { 45, 43, 54, 666 };

            Assert.That(enumerable.HasSize(4));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Has [4] items instead of the expected value [2].")]
        public void HasSizeThrowsExceptionWithClearStatusWhenFails()
        {
            var enumerable = new List<int>() { 45, 43, 54, 666 };

            Assert.That(enumerable.HasSize(2));
        }

        #endregion

        #region ToEnumeratedString extension method

        [Test]
        public void ToEnumeratedStringParticularBehaviourWithStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Assert.That(guitarHeroes.ToEnumeratedString().EqualsExactly(@"""Hendrix"", ""Paco de Lucia"", ""Django Reinhardt"", ""Baden Powell"""));
        }

        [Test]
        public void ToEnumeratedStringWorksFineWithStrings()
        {
            var guitarHeroes = new[] { 93, 56, 35, 75 };
            Assert.That(guitarHeroes.ToEnumeratedString().EqualsExactly("93, 56, 35, 75"));
        }

        #endregion

        private static IEnumerable<Person> InstantiateDirectors()
        {
            return new List<Person>() { 
                                        new Person() { Name = "Michel Gondry", Nationality = Nationality.French }, 
                                        new Person() { Name = "Joon-ho Bong", Nationality = Nationality.Korean }, 
                                        new Person() { Name = "Darren Aronofsky", Nationality = Nationality.American } 
            };
        }

        private static IEnumerable<Person> InstantiateWriters()
        {
            return new List<Person>() { 
                                        new Person() { Name = "Steve Tesich", Nationality = Nationality.Serbian }, 
                                        new Person() { Name = "Albert Camus", Nationality = Nationality.French }, 
                                        new Person() { Name = "Eiji Yoshikawa", Nationality = Nationality.Japanese },
                                        new Person() { Name = "Friedrich Nietzsche", Nationality = Nationality.German } 
            };
        }
    }
}
