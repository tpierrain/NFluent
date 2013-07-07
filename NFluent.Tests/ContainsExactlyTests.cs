// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ContainsExactlyTests.cs" company="">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using NUnit.Framework;

    [TestFixture]
    public class ContainsExactlyTests
    {
        #region ContainsExactly with arrays

        [Test]
        public void ContainsExactlyWorksWithArrayOfInt()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).ContainsExactly(1, 2, 3, 4, 5, 666);
        }

        [Test]
        public void ContainsExactlyWorksWithArrayOfStrings()
        {
            var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
            Check.That(guitarHeroes).ContainsExactly("Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[\"Luke\", \"Yoda\", \"Chewie\"] (3 items)\nThe expected value(s):\n\t[\"Luke\", \"Yoda\", \"Chewie\", \"Vador\"] (4 items)")]
        public void ContainsExactlyThrowsExceptionWhenMoreItemsAreIndicated()
        {
            var heroes = new[] { "Luke", "Yoda", "Chewie" };
            Check.That(heroes).ContainsExactly("Luke", "Yoda", "Chewie", "Vador");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[\"Luke\", \"Yoda\", \"Chewie\"] (3 items)\nThe expected value(s):\n\t[\"Luke\", \"Yoda\"] (2 items)")]
        public void ContainsExactlyThrowsExceptionWhenItemsAreMissing()
        {
            var heroes = new[] { "Luke", "Yoda", "Chewie" };
            Check.That(heroes).ContainsExactly("Luke", "Yoda");
        }
        
        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[1, 2, 3, 4, 5, 666] (6 items)\nThe expected value(s):\n\t[666, 3, 1, 2, 4, 5] (6 items)")]
        public void ContainsExactlyWithArraysThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).ContainsExactly(666, 3, 1, 2, 4, 5);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[1, 2, 3, 4, 5, 666] (6 items)\nThe expected value(s):\n\t[42, 42, 42] (3 items)")]
        public void ContainsExactlyWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).ContainsExactly(42, 42, 42);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[\"Hendrix\", \"Paco de Lucia\", \"Django Reinhardt\", \"Baden Powell\"] (4 items)\nThe expected value(s):\n\t[\"Hendrix, Paco de Lucia, Django Reinhardt, Baden Powell\"] (1 item)")]
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[] (0 item)\nThe expected value(s):\n\t[\"what da heck!\"] (1 item)")]
        public void ContainsExactlyThrowsWithEmptyList()
        {
            var emptyList = new List<int>();

            Check.That(emptyList).ContainsExactly("what da heck!");
        }

        [Test]
        public void ContainsExactlyDoNotThrowIfBothValuesAreEmptyLists()
        {
            var emptyList = new List<int>();

            Check.That(emptyList).ContainsExactly(new List<int>());
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is null and thus, does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[null]\nThe expected value(s):\n\t[\"what da heck!\"]")]
        public void ContainsExactlyThrowsWithNullAsCheckedValue()
        {
            List<int> nullList = null;

            Check.That(nullList).ContainsExactly("what da heck!");
        }

        [Test]
        public void ContainsExactlyDoNotThrowIfBothValuesAreNull()
        {
            List<int> nullList = null;

            Check.That(nullList).ContainsExactly(null);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[1, 2, 3, 4, 5, 666] (6 items)\nThe expected value(s):\n\t[666, 3, 1, 2, 4, 5] (6 items)")]
        public void ContainsExactlyWithEnumerableThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new List<int> { 1, 2, 3, 4, 5, 666 };
            IEnumerable expectedValues = new List<int> { 666, 3, 1, 2, 4, 5 };
            Check.That(integers).ContainsExactly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[1, 2, 3, 4, 5, 666] (6 items)\nThe expected value(s):\n\t[666, 3, 1, 2, 4, 5] (6 items)")]
        public void ContainsExactlyWithGenericEnumerableThrowsExceptionWhenSameItemsInWrongOrder()
        {
            var integers = new List<int> { 1, 2, 3, 4, 5, 666 };
            IEnumerable<int> expectedValues = new List<int> { 666, 3, 1, 2, 4, 5 };
            Check.That(integers).ContainsExactly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[\"Michel Gondry\", \"Joon-ho Bong\", \"Darren Aronofsky\"] (3 items)\nThe expected value(s):\n\t[\"Steve Tesich\", \"Albert Camus\", \"Eiji Yoshikawa\", \"Friedrich Nietzsche\"] (4 items)")]
        public void ContainsExactlyThrowsExceptionWhenFailingWithEnumerable()
        {
            IEnumerable writersNames = InstantiateWriters().Properties("Name");
            Check.That(InstantiateDirectors().Properties("Name")).ContainsExactly(writersNames);
        }

        [Test]
        public void NotContainsExactlyWorksWithEnumerable()
        {
            IEnumerable writersNames = InstantiateWriters().Properties("Name");
            IEnumerable directorsNames = InstantiateDirectors().Properties("Name");
            
            Check.That(directorsNames).Not.ContainsExactly(writersNames);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable contains exactly the given values whereas it must not.\nThe checked enumerable:\n\t[\"Steve Tesich\", \"Albert Camus\", \"Eiji Yoshikawa\", \"Friedrich Nietzsche\"] (4 items)")]
        public void NotContainsExactlyThrowsExceptionWhenFailingWithEnumerable()
        {
            IEnumerable writersNames = InstantiateWriters().Properties("Name");
            Check.That(writersNames).Not.ContainsExactly(writersNames);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[\"Michel Gondry\", \"Joon-ho Bong\", \"Darren Aronofsky\"] (3 items)\nThe expected value(s):\n\t[null] (0 item)")]
        public void ContainsExactlyThrowsExceptionWithClearStatusWhenFailsWithNullEnumerable()
        {
            IEnumerable nullEnumerable = null;
            Check.That(InstantiateDirectors().Properties("Name")).ContainsExactly(nullEnumerable);
        }

        [Test]
        public void ContainsExactlyWithEnumerableOfVariousObjectsTypesWorks()
        {
            var variousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            Check.That(variousObjects).ContainsExactly(expectedVariousObjects);
        }

        #endregion

        #region test helpers

        private static IEnumerable<Person> InstantiateDirectors()
        {
            return new List<Person> {
                           new Person { Name = "Michel Gondry", Nationality = Nationality.French }, 
                           new Person { Name = "Joon-ho Bong", Nationality = Nationality.Korean }, 
                           new Person { Name = "Darren Aronofsky", Nationality = Nationality.American }
                       };
        }

        private static IEnumerable<Person> InstantiateWriters()
        {
            return new List<Person> {
                           new Person { Name = "Steve Tesich", Nationality = Nationality.Serbian }, 
                           new Person { Name = "Albert Camus", Nationality = Nationality.French }, 
                           new Person { Name = "Eiji Yoshikawa", Nationality = Nationality.Japanese }, 
                           new Person { Name = "Friedrich Nietzsche", Nationality = Nationality.German }
                       };
        }

        #endregion
    }
}