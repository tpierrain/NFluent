namespace NFluent.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class ContainsRelatedTests
    {
        #region Contains with arrays

        [Test]
        public void ContainsWithArraysWorks()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers.Contains(3, 5, 666));

            var enumerable = new List<string>() { "un", "dos", "tres" };
            Assert.That(enumerable.Contains("dos"));
        }

        [Test]
        public void ContainsWithArraysWorksWhateverTheOrder()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers.Contains(666, 3, 5));
        }

        [Test]
        public void ContainsWithArraysWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers.Contains(5, 3, 666, 3, 3, 666));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The array does not contain the expected value(s): [666, 1974].")]
        public void ContainsWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new int[] { 1, 2, 3 };
            Assert.That(integers.Contains(3, 2, 666, 1974));
        }

        #endregion

        #region Contains with IEnumerable

        [Test]
        public void ContainsWithEnumerableWorks()
        {
            var integers = new List<int>() { 1, 2, 3, 1974 };
            Assert.That(integers.Contains(3, 2, 1));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The enumerable does not contain the expected value(s): [666, 1974].")]
        public void ContainsThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new List<int>() { 1, 2, 3 };
            Assert.That(integers.Contains(3, 2, 666, 1974));
        }

        #endregion

        #region ContainsExactly with arrays

        [Test]
        public void ContainsExactlyWithArraysWorks()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers.ContainsExactly(1, 2, 3, 4, 5, 666));

            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Assert.That(guitarHeroes.ContainsExactly("Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell"));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Found: [1, 2, 3, 4, 5, 666] (6 items) instead of the expected [42, 42, 42] (3 items).")]
        public void ContainsExactlyWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers.ContainsExactly(42, 42, 42));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"Found: [""Hendrix"", ""Paco de Lucia"", ""Django Reinhardt"", ""Baden Powell""] (4 items) instead of the expected [""Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell""] (1 item).")]
        public void ContainsExactlyWithArraysThrowsExceptionWithClearStatusWhenFailsWithStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Assert.That(guitarHeroes.ContainsExactly("Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell"));
        }

        #endregion

        #region ContainsExactly with IEnumerable

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

        #region test helpers

        private static IEnumerable<Person> InstantiateDirectors()
        {
            return new List<Person>()
                       {
                           new Person() { Name = "Michel Gondry", Nationality = Nationality.French },
                           new Person() { Name = "Joon-ho Bong", Nationality = Nationality.Korean },
                           new Person() { Name = "Darren Aronofsky", Nationality = Nationality.American }
                       };
        }

        private static IEnumerable<Person> InstantiateWriters()
        {
            return new List<Person>()
                       {
                           new Person() { Name = "Steve Tesich", Nationality = Nationality.Serbian },
                           new Person() { Name = "Albert Camus", Nationality = Nationality.French },
                           new Person() { Name = "Eiji Yoshikawa", Nationality = Nationality.Japanese },
                           new Person() { Name = "Friedrich Nietzsche", Nationality = Nationality.German }
                       };
        }

        #endregion
    }
}