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

        // TODO: improves the error messages of ContainsExactly: [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[\"Luke\", \"Yoda\", \"Chewie\"] (3 items)\ndoes not contain exactly the expected value(s):\n\t[\"Luke\", \"Yoda\", \"Chewie\", \"Vador\"] (4 items).\nIt contains also:\n\t[\"Vador\"]")]
        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[\"Luke\", \"Yoda\", \"Chewie\"] (3 items)\ndoes not contain exactly the expected value(s):\n\t[\"Luke\", \"Yoda\", \"Chewie\", \"Vador\"] (4 items).")]
        public void ContainsExactlyThrowsExceptionWhenMoreItemsAreIndicated()
        {
            var heroes = new[] { "Luke", "Yoda", "Chewie" };
            Check.That(heroes).ContainsExactly("Luke", "Yoda", "Chewie", "Vador");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[1, 2, 3, 4, 5, 666] (6 items)\ndoes not contain exactly the expected value(s):\n\t[666, 3, 1, 2, 4, 5] (6 items).")]
        public void ContainsExactlyWithArraysThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).ContainsExactly(666, 3, 1, 2, 4, 5);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[1, 2, 3, 4, 5, 666] (6 items)\ndoes not contain exactly the expected value(s):\n\t[42, 42, 42] (3 items).")]
        public void ContainsExactlyWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).ContainsExactly(42, 42, 42);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[\"Hendrix\", \"Paco de Lucia\", \"Django Reinhardt\", \"Baden Powell\"] (4 items)\ndoes not contain exactly the expected value(s):\n\t[\"Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell\"] (1 item).")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[1, 2, 3, 4, 5, 666] (6 items)\ndoes not contain exactly the expected value(s):\n\t[666, 3, 1, 2, 4, 5] (6 items).")]
        public void ContainsExactlyWithEnumerableThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new List<int>() { 1, 2, 3, 4, 5, 666 };
            IEnumerable expectedValues = new List<int>() { 666, 3, 1, 2, 4, 5 };
            Check.That(integers).ContainsExactly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[1, 2, 3, 4, 5, 666] (6 items)\ndoes not contain exactly the expected value(s):\n\t[666, 3, 1, 2, 4, 5] (6 items).")]
        public void ContainsExactlyWithGenericEnumerableThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new List<int>() { 1, 2, 3, 4, 5, 666 };
            IEnumerable<int> expectedValues = new List<int>() { 666, 3, 1, 2, 4, 5 };
            Check.That(integers).ContainsExactly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[\"Michel Gondry\", \"Joon-ho Bong\", \"Darren Aronofsky\"] (3 items)\ndoes not contain exactly the expected value(s):\n\t[\"Steve Tesich\", \"Albert Camus\", \"Eiji Yoshikawa\", \"Friedrich Nietzsche\"] (4 items).")]
        public void ContainsExactlyThrowsExceptionWithClearStatusWhenFailsWithEnumerable()
        {
            IEnumerable writersNames = InstantiateWriters().Properties("Name");
            Check.That(InstantiateDirectors().Properties("Name")).ContainsExactly(writersNames);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"Found: [""Michel Gondry"", ""Joon-ho Bong"", ""Darren Aronofsky""] (3 items) instead of the expected [null] (0 item).")]
        public void ContainsExactlyThrowsExceptionWithClearStatusWhenFailsWithNullEnumerable()
        {
            IEnumerable nullEnumerable = null;
            Check.That(InstantiateDirectors().Properties("Name")).ContainsExactly(nullEnumerable);
        }

        [Test]
        public void ContainsExactlyWithEnumerableOfVariousObjectsTypesWorks()
        {
            var variousObjects = new ArrayList() { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjects = new ArrayList() { 1, "uno", "tres", 45.3F };
            Check.That(variousObjects).ContainsExactly(expectedVariousObjects);
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