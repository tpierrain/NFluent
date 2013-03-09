namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class ContainsExactlyTests
    {
        #region ContainsExactly with arrays

        [Test]
        public void ContainsExactlyWorksWithArrayOfInt()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).ContainsExactly(1, 2, 3, 4, 5, 666);
        }

        [Test]
        public void ContainsExactlyWorksWithArrayOfStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Check.That(guitarHeroes).ContainsExactly("Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Found: [1, 2, 3, 4, 5, 666] (6 items) instead of the expected [666, 3, 1, 2, 4, 5] (6 items).")]
        public void ContainsExactlyWithArraysThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).ContainsExactly(666, 3, 1, 2, 4, 5);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Found: [1, 2, 3, 4, 5, 666] (6 items) instead of the expected [42, 42, 42] (3 items).")]
        public void ContainsExactlyWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).ContainsExactly(42, 42, 42);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"Found: [""Hendrix"", ""Paco de Lucia"", ""Django Reinhardt"", ""Baden Powell""] (4 items) instead of the expected [""Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell""] (1 item).")]
        public void ContainsExactlyWithArraysThrowsExceptionWithClearStatusWhenFailsWithStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Check.That(guitarHeroes).ContainsExactly("Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell");
        }

        #endregion

        #region ContainsExactly with IEnumerable

        [Test]
        public void ContainsExactlyAlsoWorksWithEnumerableParameter()
        {
            Check.That(InstantiateDirectors().Properties("Name")).ContainsExactly(InstantiateDirectors().Properties("Name"));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Found: [1, 2, 3, 4, 5, 666] (6 items) instead of the expected [666, 3, 1, 2, 4, 5] (6 items).")]
        public void ContainsExactlyWithEnumerableThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new List<int>() { 1, 2, 3, 4, 5, 666 };
            IEnumerable expectedValues = new List<int>() { 666, 3, 1, 2, 4, 5 };
            Check.That(integers).ContainsExactly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Found: [1, 2, 3, 4, 5, 666] (6 items) instead of the expected [666, 3, 1, 2, 4, 5] (6 items).")]
        public void ContainsExactlyWithGenericEnumerableThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new List<int>() { 1, 2, 3, 4, 5, 666 };
            IEnumerable<int> expectedValues = new List<int>() { 666, 3, 1, 2, 4, 5 };
            Check.That(integers).ContainsExactly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"Found: [""Michel Gondry"", ""Joon-ho Bong"", ""Darren Aronofsky""] (3 items) instead of the expected [""Steve Tesich"", ""Albert Camus"", ""Eiji Yoshikawa"", ""Friedrich Nietzsche""] (4 items).")]
        public void ContainsExactlyThrowsExceptionWithClearStatusWhenFailsWithEnumerable()
        {
            IEnumerable expectedValues = InstantiateWriters().Properties("Name");
            Check.That(InstantiateDirectors().Properties("Name")).ContainsExactly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"Found: [""Michel Gondry"", ""Joon-ho Bong"", ""Darren Aronofsky""] (3 items) instead of the expected [null] (0 item).")]
        public void ContainsExactlyThrowsExceptionWithClearStatusWhenFailsWithNullEnumerable()
        {
            IEnumerable expectedValues = null;
            Check.That(InstantiateDirectors().Properties("Name")).ContainsExactly(expectedValues);
        }

        // TODO: write a ContainsExactly test with IEnumerable containing of objects with various types
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