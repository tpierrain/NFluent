// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EnumerableRelatedTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN, Cyrille DUPUYDAUBY
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

    using NUnit.Framework;

    [TestFixture]
    public class EnumerableRelatedTests
    {
        private const string Blabla = ".*?";
        private const string LineFeed = "\\n";
        private const string NumericalHashCodeWithinBrackets = "(\\[(\\d+)\\])";
        private static readonly List<int> EmptyEnumerable = new List<int>();

        #region HasSize

        [Test]
        public void HasSizeWorksWithArray()
        {
            var array = new[] { 45, 43, 54, 666 };

            Check.That(array).HasSize(4);
        }

        [Test]
        public void HasSizeGivesTheNumberOfElementsAndNotTheCapacity()
        {
            var enumerable = new List<string>(500);

            Check.That(enumerable).HasSize(0);
        }

        [Test]
        public void HasSizeWorksWithEnumerable()
        {
            IEnumerable enumerable = new List<int> { 45, 43, 54, 666 };

            Check.That(enumerable).HasSize(4);
        }

        [Test]
        public void HasSizeWorksWithGenericEnumerable()
        {
            IEnumerable<int> enumerable = new List<int> { 45, 43, 54, 666 };

            Check.That(enumerable).HasSize(4).And.Contains(666);
        }

        [Test]
        public void HasSizeWorksWithArrayList()
        {
            var arrayList = new ArrayList { 45, 43, 54, 666 };

            Check.That(arrayList).HasSize(4);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe actual enumerable has 1 element instead of 5.\nActual content is:\n\t[666].")]
        public void HasSizeThrowsExceptionWhenFailingWithOneElementFound()
        {
            var enumerable = new List<int> { 666 };

            Check.That(enumerable).HasSize(5);
        }

        [Test]
        public void NotHasSizeWorks()
        {
            var enumerable = new List<int> { 666 };
            
            Check.That(enumerable).Not.HasSize(5);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe actual enumerable has 1 element which is unexpected.\nActual content is:\n\t[666].")]
        public void NotHasSizeThrowsExceptionWhenFailing()
        {
            var enumerable = new List<int> { 666 };

            Check.That(enumerable).Not.HasSize(1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe actual enumerable has 4 elements instead of 1.\nActual content is:\n\t[45, 43, 54, 666].")]
        public void HasSizeThrowsExceptionWithClearStatusWhenFailsWithOneExpectedElement()
        {
            var enumerable = new List<int> { 45, 43, 54, 666 };

            Check.That(enumerable).HasSize(1);
        }

        #endregion

        #region IsEmpty

        [Test]
        public void IsEmptyWorks()
        {
            var emptyEnumerable = new List<int>();

            Check.That(emptyEnumerable).IsEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe actual enumerable is not empty. Contains:\n\t[null, null, Thomas]")]
        public void IsEmptyThrowsExceptionWhenNotEmpty()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            
            Check.That(persons).IsEmpty();
        }

        [Test]
        public void IsEmptyOrNullWorks()
        {
            Check.That(EmptyEnumerable).IsEmptyOrNull();
            Check.That((IEnumerable)null).IsEmptyOrNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked IEnumerable contains items, whereas it must be empty or null.\nThe checked IEnumerable:\n\t[null, null, Thomas]")]
        public void IsEmptyOrNullFailsAppropriately()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            Check.That(persons).IsEmptyOrNull();
        }

        [Test]
        public void NotIsEmptyOrNullWorks()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            Check.That(persons).Not.IsEmptyOrNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked IEnumerable is empty, where as it must contain at least one item.")]
        public void NotIsEmptyOrNullFailsIfEmpty()
        {
            Check.That(EmptyEnumerable).Not.IsEmptyOrNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked IEnumerable is null, where as it must contain at least one item.")]
        public void NotIsEmptyOrNullFailsIfNull()
        {
            Check.That((IEnumerable)null).Not.IsEmptyOrNull();
        }

        [Test]
        public void NotIsEmptyWorks()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            
            Check.That(persons).Not.IsEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe actual enumerable is empty, which is unexpected.")]
        public void NotIsEmptyThrowsExceptionWhenFailing()
        {
            var persons = new List<Person>();

            Check.That(persons).Not.IsEmpty();
        }

        #endregion

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[45, 43, 54, 666] of type: [List<int>]")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            IEnumerable enumerable = new List<int> { 45, 43, 54, 666 };
            Check.That(enumerable).Not.IsEqualTo(enumerable);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[45, 43, 54, 666] of type: [List<int>]\nThe expected value:\n\t[null]")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            IEnumerable enumerable = new List<int> { 45, 43, 54, 666 };
            Check.That(enumerable).Not.IsNotEqualTo(null);
        }

        [Test]
        public void AndOperatorWorksWithAllMethodsOfEnumerableFluentAssertion()
        {
            var killingSeries = new List<string> { "The wire", "Game of Thrones" };
            
            Check.That(killingSeries).HasSize(2).And.IsOnlyMadeOf("Game of Thrones", "The wire").And.ContainsExactly("The wire", "Game of Thrones");
            Check.That(killingSeries).Contains("The wire").And.IsOnlyMadeOf("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).ContainsExactly("The wire", "Game of Thrones").And.IsOnlyMadeOf("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).IsOnlyMadeOf("Game of Thrones", "The wire").And.Contains("The wire").And.HasSize(2);
            Check.That(killingSeries).IsEqualTo(killingSeries).And.Contains("The wire");
            Check.That(killingSeries).IsNotEqualTo(null).And.Contains("Game of Thrones");
            
            var integerEmptyList = new List<int>();
            Check.That(integerEmptyList).IsEmpty().And.HasSize(0);
        }

        [Test]
        public void AndOperatorWorksWithAllMethodsOfEnumerableFluentAssertionOnEnumerable()
        {
            IEnumerable killingSeries = new List<string> { "The wire", "Game of Thrones" };

            Check.That(killingSeries).HasSize(2).And.IsOnlyMadeOf("Game of Thrones", "The wire").And.ContainsExactly("The wire", "Game of Thrones");
            Check.That(killingSeries).Contains("The wire").And.IsOnlyMadeOf("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).ContainsExactly("The wire", "Game of Thrones").And.IsOnlyMadeOf("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).IsOnlyMadeOf("Game of Thrones", "The wire").And.Contains("The wire").And.HasSize(2);

            IEnumerable integerEmptyList = new List<int>();
            Check.That(integerEmptyList).IsEmpty().And.HasSize(0);
        }
    }
}