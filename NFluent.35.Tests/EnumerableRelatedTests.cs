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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable has 1 element instead of 5.\nThe checked enumerable:\n\t[666]")]
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable has 1 element which is unexpected.\nThe checked enumerable:\n\t[666]")]
        public void NotHasSizeThrowsExceptionWhenFailing()
        {
            var enumerable = new List<int> { 666 };

            Check.That(enumerable).Not.HasSize(1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable has 4 elements instead of 1.\nThe checked enumerable:\n\t[45, 43, 54, 666]")]
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is not empty.\nThe checked enumerable:\n\t[null, null, Thomas]")]
        public void IsEmptyThrowsExceptionWhenNotEmpty()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            
            Check.That(persons).IsEmpty();
        }

        [Test]
        public void IsNullOrEmptyWorks()
        {
            Check.That(EmptyEnumerable).IsNullOrEmpty();
            Check.That((IEnumerable)null).IsNullOrEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable contains items, whereas it must be null or empty.\nThe checked enumerable:\n\t[null, null, Thomas]")]
        public void IsNullOrEmptyFailsAppropriately()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            Check.That(persons).IsNullOrEmpty();
        }

        [Test]
        public void NotIsNullOrEmptyWorks()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            Check.That(persons).Not.IsNullOrEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is empty, where as it must contain at least one item.")]
        public void NotIsNullOrEmptyFailsIfEmpty()
        {
            Check.That(EmptyEnumerable).Not.IsNullOrEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is null, where as it must contain at least one item.")]
        public void NotIsNullOrEmptyFailsIfNull()
        {
            Check.That((IEnumerable)null).Not.IsNullOrEmpty();
        }

        [Test]
        public void NotIsEmptyWorks()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            
            Check.That(persons).Not.IsEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is empty, which is unexpected.")]
        public void NotIsEmptyThrowsExceptionWhenFailing()
        {
            var persons = new List<Person>();

            Check.That(persons).Not.IsEmpty();
        }

        #endregion

        #region HasFirstElement

        [Test]
        public void HasFirstElementWorks()
        {
            var enumerable = new List<int> { 42, 43 };

            Check.That(enumerable).HasFirstElement().That.IsEqualTo(42);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is empty, where as it must have a first element.")]
        public void HasFirstElementThrowsWhenCollectionIsEmpty()
        {
            Check.That(EmptyEnumerable).HasFirstElement();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is null, where as it must have a first element.")]
        public void HasFirstElementThrowsWhenCollectionIsNull()
        {
            var nullEnumerable = (List<int>)null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Check.That(nullEnumerable).HasFirstElement();
        }

        #endregion

        #region HasLastElement

        [Test]
        public void HasLastElementWorksForList()
        {
            var enumerable = new List<int> { 42, 43 };

            Check.That(enumerable).HasLastElement().That.IsEqualTo(43);
        }

        [Test]
        public void HasLastElementWorksForEnumerable()
        {
            var enumerable = new List<int> { 4, 3, 1, 2 }.OrderBy(x => x);

            Check.That(enumerable).HasLastElement().That.IsEqualTo(4);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is empty, where as it must have a last element.")]
        public void HasLastElementThrowsWhenCollectionIsEmpty()
        {
            Check.That(EmptyEnumerable).HasLastElement();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is null, where as it must have a last element.")]
        public void HasLastElementThrowsWhenCollectionIsNull()
        {
            var nullEnumerable = (List<int>)null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Check.That(nullEnumerable).HasLastElement();
        }

        #endregion

        #region HasElementNumber

        [Test]
        public void HasElementNumberWorksForList()
        {
            var enumerable = new List<int> { 42, 43, 44 };

            Check.That(enumerable).HasElementNumber(2).That.IsEqualTo(43);
        }

        [Test]
        public void HasElementNumberWorksForEnumerable()
        {
            var enumerable = new List<int> { 4, 3, 1, 2 }.OrderBy(x => x);

            Check.That(enumerable).HasElementNumber(3).That.IsEqualTo(3);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable has less that 1 elements, where as it must have an element with number 1.\nThe checked enumerable:\n\t[]")]
        public void HasElementNumberThrowsWhenCollectionHasEnoughElementsIsEmpty()
        {
            Check.That(EmptyEnumerable).HasElementNumber(1);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = "The specified number is less than or equal to zero, where as it must be a 1-based index.\r\nParameter name: number")]
        public void HasElementNumberThrowsWhenNumberIsInvalid(int number)
        {
            var enumerable = new List<int> { 42 };

            Check.That(enumerable).HasElementNumber(number);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is null, where as it must have an element with number 1.")]
        public void HasElementNumberThrowsWhenCollectionIsNull()
        {
            var nullEnumerable = (List<int>)null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Check.That(nullEnumerable).HasElementNumber(1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable has less that 3 elements, where as it must have an element with number 3.\nThe checked enumerable:\n\t[42, 43]")]
        public void HasElementNumberThrowsWhenCollectionHasNotEnoughElements()
        {
            var enumerable = new List<int> { 42, 43 };

            Check.That(enumerable).HasElementNumber(3);
        }

        #endregion

        #region HasOneElementOnly

        [Test]
        public void HasOneElementOnlyWorks()
        {
            var enumerable = new List<int> { 42 };

            Check.That(enumerable).HasOneElementOnly().That.IsEqualTo(42);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is empty, where as it must have one element.")]
        public void HasOneElementOnlyThrowsWhenCollectionIsEmpty()
        {
            Check.That(EmptyEnumerable).HasOneElementOnly();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is null, where as it must have one element.")]
        public void HasOneElementOnlyThrowsWhenCollectionIsNull()
        {
            var nullEnumerable = (List<int>)null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Check.That(nullEnumerable).HasOneElementOnly();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable contains more than one element, where as it must have one element only.\nThe checked enumerable:\n\t[42, 43, 1000]")]
        public void HasOneElementOnlyThrowsWhenCollectionHasMoreThanOneElement()
        {
            var enumerable = new List<int> { 42, 43, 1000 };

            Check.That(enumerable).HasOneElementOnly();
        }

        #endregion

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable is equal to the expected one whereas it must not.\nThe expected enumerable: different from\n\t[45, 43, 54, 666] of type: [System.Collections.Generic.List<int>]")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            IEnumerable enumerable = new List<int> { 45, 43, 54, 666 };
            Check.That(enumerable).Not.IsEqualTo(enumerable);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[45, 43, 54, 666]\nThe expected value:\n\t[null]")]
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