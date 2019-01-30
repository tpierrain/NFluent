// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableRelatedTests.cs" company="">
//   Copyright 2013 Thomas PIERRAIN, Cyrille DUPUYDAUBY
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using ApiChecks;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class EnumerableRelatedTests
    {
        private static readonly List<int> EmptyEnumerable = new List<int>();

        #region HasSize

        [Test]
        public void HasSizeWorksWithArray()
        {
            var array = new[] { 45, 43, 54, 666 };

            Check.That(array).HasSize(4);
        }

        [Test]
        // issue #121
        public void CountIsWorksWithArray()
        {
            var array = new[] { 45, 43, 54, 666 };

            Check.That(array).CountIs(4);
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
        public void HasSizeThrowsExceptionWhenFailingWithOneElementFound()
        {
            var enumerable = new List<int> { 666 };

            Check.ThatCode(() => { Check.That(enumerable).HasSize(5); }).IsAFailingCheckWithMessage(
                "", "The checked enumerable has 1 element instead of 5.",
                "The checked enumerable:", "\t{666} (1 item)");
        }

        [Test]
        public void NotHasSizeWorks()
        {
            var enumerable = new List<int> { 666 };

            Check.That(enumerable).Not.HasSize(5);
        }

        [Test]
        public void NotHasSizeThrowsExceptionWhenFailing()
        {
            var enumerable = new List<int> { 666 };

            Check.ThatCode(() => { Check.That(enumerable).Not.HasSize(1); }).IsAFailingCheckWithMessage(
                "",
            "The checked enumerable has 1 element which is unexpected.",
            "The checked enumerable:",
                "\t{666} (1 item)");
        }

        [Test]
        public void HasSizeThrowsExceptionWithClearStatusWhenFailsWithOneExpectedElement()
        {
            var enumerable = new List<int> { 45, 43, 54, 666 };

            Check.ThatCode(() => { Check.That(enumerable).HasSize(1); }).IsAFailingCheckWithMessage(
                "",
                "The checked enumerable has 4 elements instead of 1.",
                "The checked enumerable:",
                "\t{45, 43, 54, 666} (4 items)");
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
        public void IsEmptyThrowsExceptionWhenNotEmpty()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };

            Check.ThatCode(() => { Check.That(persons).IsEmpty(); }).IsAFailingCheckWithMessage(
                "", "The checked enumerable is not empty.",
                "The checked enumerable:",
                "\t{null, null, Thomas} (3 items)");
        }

        [Test]
        public void IsNullOrEmptyWorks()
        {
            Check.That(EmptyEnumerable).IsNullOrEmpty();
            Check.That((IEnumerable)null).IsNullOrEmpty();
        }

        [Test]
        public void IsNullOrEmptyFailsAppropriately()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };

            Check.ThatCode(() => { Check.That(persons).IsNullOrEmpty(); }).IsAFailingCheckWithMessage(
                "", 
                "The checked enumerable contains elements, whereas it must be null or empty."
                ,"The checked enumerable:", 
                "\t{null, null, Thomas} (3 items)");
        }

        [Test]
        public void NotIsNullOrEmptyWorks()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };
            Check.That(persons).Not.IsNullOrEmpty();
        }

        [Test]
        public void NotIsNullOrEmptyFailsIfEmpty()
        {
            Check.ThatCode(() => { Check.That(EmptyEnumerable).Not.IsNullOrEmpty(); }).IsAFailingCheckWithMessage(
                    "", "The checked enumerable is empty, where as it must contain at least one element.");
        }

        [Test]
        public void NotIsNullOrEmptyFailsIfNull()
        {
            Check.ThatCode(() => { Check.That((IEnumerable)null).Not.IsNullOrEmpty(); }).IsAFailingCheckWithMessage(
                    "", "The checked enumerable is null, where as it must contain at least one element.");
        }

        [Test]
        public void NotIsEmptyWorks()
        {
            var persons = new List<Person> { null, null, new Person { Name = "Thomas" } };

            Check.That(persons).Not.IsEmpty();
        }

        [Test]
        public void NotIsEmptyThrowsExceptionWhenFailing()
        {
            var persons = new List<Person>();

            Check.ThatCode(() => { Check.That(persons).Not.IsEmpty(); }).IsAFailingCheckWithMessage(
                "", "The checked enumerable is empty, which is unexpected.");
        }

        #endregion

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            IEnumerable enumerable = new List<int> { 45, 43, 54, 666 };

            Check.ThatCode(() => { Check.That(enumerable).Not.IsEqualTo(enumerable); }).IsAFailingCheckWithMessage("",
                "The checked enumerable is equal to the given one whereas it must not.",
                    "The expected enumerable: different from",
                    "\t{45, 43, 54, 666} of type: [System.Collections.Generic.List<int>]");
        }

        [Test]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            IEnumerable enumerable = new List<int> { 45, 43, 54, 666 };

            Check.ThatCode(() => { Check.That(enumerable).Not.IsNotEqualTo(null); }).IsAFailingCheckWithMessage(
                    "",
                    "The checked enumerable is different from the given one.",
                    "The checked enumerable:",
                    "\t{45, 43, 54, 666} (4 items)",
                    "The expected enumerable:",
                    "\t[null]");
        }

        [Test]
        public void AndOperatorWorksWithAllMethodsOfEnumerableFluentAssertion()
        {
            var killingSeries = new List<string> { "The wire", "Game of Thrones" };

            Check.That(killingSeries).HasSize(2).And.IsOnlyMadeOf("Game of Thrones", "The wire").And
                .ContainsExactly("The wire", "Game of Thrones");
            Check.That(killingSeries).Contains("The wire").And.IsOnlyMadeOf("Game of Thrones", "The wire").And
                .HasSize(2);
            Check.That(killingSeries).ContainsExactly("The wire", "Game of Thrones").And
                .IsOnlyMadeOf("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).IsOnlyMadeOf("Game of Thrones", "The wire").And.Contains("The wire").And
                .HasSize(2);
            Check.That(killingSeries).IsEqualTo(killingSeries).And.Contains("The wire");
            Check.That(killingSeries).IsNotEqualTo(null).And.Contains("Game of Thrones");

            var integerEmptyList = new List<int>();
            Check.That(integerEmptyList).IsEmpty().And.HasSize(0);
        }

        [Test]
        public void AndOperatorWorksWithAllMethodsOfEnumerableFluentAssertionOnEnumerable()
        {
            IEnumerable killingSeries = new List<string> { "The wire", "Game of Thrones" };
            
            Check.That(killingSeries).HasSize(2).And.IsOnlyMadeOf("Game of Thrones", "The wire").And
                .ContainsExactly("The wire", "Game of Thrones");
            Check.That(killingSeries).Contains("The wire").And.IsOnlyMadeOf("Game of Thrones", "The wire").And
                .HasSize(2);
            Check.That(killingSeries).ContainsExactly("The wire", "Game of Thrones").And
                .IsOnlyMadeOf("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).IsOnlyMadeOf("Game of Thrones", "The wire").And.Contains("The wire").And
                .HasSize(2);

            IEnumerable integerEmptyList = new List<int>();
            Check.That(integerEmptyList).IsEmpty().And.HasSize(0);
        }

        #region HasFirstElement

        [Test]
        public void HasFirstElementWorks()
        {
            var enumerable = new List<int> { 42, 43 };
            Check.That(enumerable).HasFirstElement().Which.IsEqualTo(42);
        }

        [Test]
        public void HasFirstElementWorksWithWhich()
        {
            var enumerable = new List<int> { 42, 43 };
            Check.ThatCode(() => 
            Check.That(enumerable).HasFirstElement().Which.IsEqualTo(43)).
                IsAFailingCheckWithMessage("", 
                    "The checked [First element] is different from the expected one.", 
                    "The checked [First element]:", 
                    "\t[42]", 
                    "The expected [First element]:", 
                    "\t[43]");
        }

        [Test]
        public void NotHasFirstElementWorks()
        {
            var enumerable = new List<int> { };
            Check.That(enumerable).Not.HasFirstElement();
        }

        [Test]
        public void HasFirstElementThrowsWhenCollectionIsEmpty()
        {
            Check.ThatCode(() =>
            Check.That(EmptyEnumerable).HasFirstElement())
            .IsAFailingCheckWithMessage("",
                "The checked enumerable is empty, whereas it must have a first element.");
        }

        [Test]
        public void HasFirstElementThrowsWhenCollectionIsNull()
        {
            var nullEnumerable = (List<int>)null;
            // ReSharper disable once ExpressionIsAlwaysNull
            Check.ThatCode(() =>
            Check.That(nullEnumerable).HasFirstElement())
                .IsAFailingCheckWithMessage("", "The checked enumerable is null, whereas it must have a first element.");
        }

        [Test]
        public void
            NegatedWhichRaisesException()
        {
            var enumerable = new List<int> {};
            Check.ThatCode(() => { Check.That(enumerable).Not.HasFirstElement().Which.IsEqualTo(44); }).
                IsAFailingCheckWithMessage("",
            "The checked [First element] is different from the expected one.",
                "The checked [First element]:",
                "\t[0]",
            "The expected [First element]:",
                "\t[44]");
        }

        #endregion

        #region HasLastElement

        [Test]
        public void HasLastElementWorksForList()
        {
            var enumerable = new List<int> { 42, 43 };
            Check.That(enumerable).HasLastElement().Which.IsEqualTo(43);
        }

        [Test]
        public void NotHasLastElementWorks()
        {
            var enumerable = new List<int> { };
            Check.That(enumerable).Not.HasLastElement();
        }

        [Test]
        public void NotHasLastElementWorksForNonList()
        {
            var enumerable = (new List<int> { }).Where((_) => true);
            Check.That(enumerable).Not.HasLastElement();
        }

        [Test]
        public void HasLastElementWorksForEnumerable()
        {
            var enumerable = new List<int> { 4, 3, 1, 2 }.OrderBy(_ => _);

            Check.That(enumerable).HasLastElement().Which.IsEqualTo(4);
        }

        [Test]
        public void HasLastElementWorksWithWhich()
        {
            var enumerable = new List<int> { 42, 43 };
            Check.ThatCode(() => 
                    Check.That(enumerable).HasLastElement().Which.IsEqualTo(42)).
                IsAFailingCheckWithMessage("", 
                    "The checked [Last element] is different from the expected one.", 
                    "The checked [Last element]:", 
                    "\t[43]", 
                    "The expected [Last element]:", 
                    "\t[42]");
        }

        [Test]
        public void HasLastElementThrowsWhenCollectionIsEmpty()
        {
            Check.ThatCode(() =>
           Check.That(EmptyEnumerable).HasLastElement())
                .IsAFailingCheckWithMessage(
                "",
                "The checked enumerable is empty, whereas it must have a last element.");
        }

        [Test]
        public void HasLastElementThrowsWhenCollectionIsNull()
        {
            var nullEnumerable = (List<int>)null;
            // ReSharper disable once ExpressionIsAlwaysNull
            Check.ThatCode(() =>
            Check.That(nullEnumerable).HasLastElement())
                .IsAFailingCheckWithMessage("", "The checked enumerable is null, whereas it must have a last element.");
        }

        #endregion

        #region HasElementNumber

        [Test]
        public void HasElementNumberWorksForList()
        {
            var enumerable = new List<int> { 42, 43, 44 };
            Check.That(enumerable).HasElementAt(1).Which.IsEqualTo(43);
        }

        [Test]
        public void HasElementNumberWorksForEnumerable()
        {
            var enumerable = new List<int> { 4, 3, 1, 2 }.OrderBy(x => x);

            Check.That(enumerable).HasElementAt(2).Which.IsEqualTo(3);
        }

        [Test]
        public void NotHasElementNumberWorksForEnumerable()
        {
            var enumerable = new List<int> { 4, 3, 1, 2 }.OrderBy(x => x);

            Check.That(enumerable).Not.HasElementAt(5);
        }

        [Test]
        public void HasElementNumberThrowsWhenCollectionHasEnoughElementsIsEmpty()

        {
            Check.ThatCode(() =>
                    Check.That(EmptyEnumerable).HasElementAt(0))
                .IsAFailingCheckWithMessage(
                "",
                "The checked enumerable does not have an element at index 0.",
                "The checked enumerable:",
                "\t{} (0 item)");
        }

        [TestCase(-1)]
        public void HasElementNumberThrowsWhenNumberIsInvalid(int number)
        {
            var enumerable = new List<int> { 42 };

            Check.ThatCode(() => Check.That(enumerable).HasElementAt(number)).Throws<ArgumentOutOfRangeException>()
                .AndWhichMessage().AsLines().Contains("The specified number is less than zero, whereas it must be a 0-based index.");
        }

        [Test]
        public void HasElementNumberThrowsWhenCollectionIsNull()
        {
            var nullEnumerable = (List<int>)null;
            // ReSharper disable once ExpressionIsAlwaysNull
            Check.ThatCode(() => Check.That(nullEnumerable).HasElementAt(1)).IsAFailingCheckWithMessage(
                    "",
                    "The checked enumerable is null, whereas it must have an element with number 1.");
        }

        [Test]
        public void HasElementNumberThrowsWhenCollectionHasNotEnoughElements()
        {
            var enumerable = new List<int> { 42, 43 };
            Check.ThatCode(() =>
            Check.That(enumerable).HasElementAt(2))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked enumerable does not have an element at index 2.",
                    "The checked enumerable:",
                    "\t{42, 43} (2 items)");
        }

        #endregion

        #region HasOneElementOnly

        [Test]
        public void HasOneElementOnlyWorks()
        {
            var enumerable = new List<int> { 42 };
            Check.That(enumerable).HasOneElementOnly().Which.IsEqualTo(42);
        }

        [Test]
        public void HasOneElementOnlyWorksWithWhich()
        {
            var enumerable = new List<int> { 42 };
            Check.ThatCode(() =>
            Check.That(enumerable).HasOneElementOnly().Which.IsEqualTo(43)).IsAFailingCheckWithMessage("", 
                "The checked [single element] is different from the expected one.", 
                "The checked [single element]:", 
                "\t[42]", 
                "The expected [single element]:", 
                "\t[43]");
        }

        [Test]
        public void NotHasOneElementOnlyWorks()
        {
            var enumerable = new List<int> { };
            Check.That(enumerable).Not.HasOneElementOnly();
        }

        [Test]
        public void HasOneElementOnlyThrowsWhenCollectionIsEmpty()
        {
            Check.ThatCode(() => Check.That(EmptyEnumerable).HasOneElementOnly()).IsAFailingCheckWithMessage(
                    "",
                    "The checked enumerable is empty, whereas it must have one element.",
                    "The checked enumerable:",
                    "\t{} (0 item)");
        }

        [Test]
        public void HasOneElementOnlyThrowsWhenCollectionIsNull()
        {
            var nullEnumerable = (List<int>)null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Check.ThatCode(() => Check.That(nullEnumerable).HasOneElementOnly()).IsAFailingCheckWithMessage(
                    "",
                    "The checked enumerable is null, whereas it must have one element.");
        }

        [Test]
        public void HasOneElementOnlyThrowsWhenCollectionHasMoreThanOneElement()
        {
            var enumerable = new List<int> { 42, 43, 1000 };
            Check.ThatCode(() =>
                    Check.That(enumerable).HasOneElementOnly())
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked enumerable contains more than one element, whereas it must have one element only.",
                    "The checked enumerable:",
                    "\t{42, 43, 1000} (3 items)");

        }

        #endregion

        #region ContainsOnlyElement

        [Test]
        public void ContainsOnlyElementShouldNotFailIfCheckIsEmpty()
        {
            IEnumerable<string> emptyList = new List<string> { };
            Check.That(emptyList).ContainsOnlyElementsThatMatch(item => true);
        }

        [Test]
        public void ContainsOnlyElementShouldNotFailIfAllElementMatch()
        {
            IEnumerable<int> list = new List<int> {4,6,8 };
            Check.That(list).ContainsOnlyElementsThatMatch(item => item % 2 == 0);
        }

        [Test]
        public void ContainsOnlyElementShouldFailIfAnElementDoesNotMatch()
        {
            IEnumerable<int> list = new List<int> { 4, 5, 8 };
            Check.ThatCode(() => Check.That(list).ContainsOnlyElementsThatMatch(item => item % 2 == 0))
                .IsAFailingCheckWithMessage("",
                    "The checked enumerable does contain an element at index #1 that does not match the given predicate: (5).",
                    "The checked enumerable:" ,
                    "\t{4, 5, 8} (3 items)");
        }

        [Test]
        public void ContainsOnlyElementShouldWorkWhenNegated()
        {
            IEnumerable<int> list = new List<int> { 4, 5, 8 };
            Check.ThatCode(() => Check.That(list).Not.ContainsOnlyElementsThatMatch(item => item % 2 == 0).Which.IsEqualTo(2))
                .IsAFailingCheckWithMessage("", 
                    "The checked [element #1] is different from the expected one.", 
                    "The checked [element #1]:", 
                    "\t[5]", 
                    "The expected [element #1]:", 
                    "\t[2]");
        }

        [Test]
        public void ContainsOnlyElementAndWhich()
        {
            IEnumerable<int> list = new List<int> { 4, 6, 8 };
            Check.ThatCode(() => Check.That(list).ContainsOnlyElementsThatMatch(item => item % 2 == 0).Which.IsEqualTo(2))
                .IsAFailingCheckWithMessage("",
                    "The checked [default element] is different from the expected one.", 
                    "The checked [default element]:", 
                    "\t[0]", 
                    "The expected [default element]:", 
                    "\t[2]");
        }

        #endregion

        // item #183
        [Test]
        public void ShouldProvideItemExploration()
        {
            IEnumerable<string> randomWords = new List<string> { "yes", "foo", "bar", "nope" };
            Check.ThatCode(() => Check.That(randomWords).HasElementAt(3).Which.IsEqualTo("hope"))
                .IsAFailingCheckWithMessage(
                    string.Empty,
                    "The checked [element #3] is different from the expected one but has same length.",
                    "The checked [element #3]:",
                    "\t[\"nope\"]",
                    "The expected [element #3]:",
                    "\t[\"hope\"]");
        }

        [Test]
        public void HasItemShouldFailWhenNoItemAtIndex()
        {
            IEnumerable<string> randomWords = new List<string> { "yes", "foo", "bar" };
            Check.ThatCode(() => Check.That(randomWords).HasElementAt(3).Which.IsEqualTo("hope"))
                .IsAFailingCheckWithMessage(
                    string.Empty,
                    "The checked enumerable does not have an element at index 3.",
                    "The checked enumerable:",
                    "\t{\"yes\", \"foo\", \"bar\"} (3 items)");
            Check.ThatCode(() => Check.That(randomWords).HasElementAt(2).Which.IsEqualTo("hope"))
                .IsAFailingCheckWithMessage(
                "", 
                "The checked [element #2] is different from expected one.", 
                "The checked [element #2]:", 
                "\t[\"bar\"]", 
                "The expected [element #2]:", 
                "\t[\"hope\"]");
        }

        [Test]
        public void
            HasItemShouldHandleNegation()
        {
            IEnumerable<string> randomWords = new List<string> { "yes", "foo", "bar" };
            Check.That(randomWords).Not.HasElementAt(3);
        }

        // GH #183 
        [Test]
        public void ShouldProvideFilterSupport()
        {
            IEnumerable<string> randomWords = new List<string> { "yes", "foo", "bar" };
            Check.That(randomWords).HasElementThatMatches((_) => _.StartsWith("ye"));
        }
        
        [Test]
        public void FilterShouldSupportWhich()
        {
            IEnumerable<string> randomWords = new List<string> { "yes", "foo", "bar" };
            Check.ThatCode(()=>
            Check.That(randomWords).HasElementThatMatches((_) => _.StartsWith("fo")).Which.IsEqualTo("yes")).
                IsAFailingCheckWithMessage("", 
                    "The checked [element #1] is different from the expected one but has same length.", 
                    "The checked [element #1]:", 
                    "\t[\"foo\"]", 
                    "The expected [element #1]:", 
                    "\t[\"yes\"]");
        }

        [Test]
        public void FilterSupportShouldFailWhenRelevant()
        {
            IEnumerable<string> randomWords = new List<string> { "yes", "foo", "bar" };
            Check.ThatCode(() => Check.That(randomWords).HasElementThatMatches(item => item.StartsWith("to")))
                .IsAFailingCheckWithMessage("",
            "The checked enumerable does not contain any element that matches the given predicate.",
                "The checked enumerable:",
                "\t{\"yes\", \"foo\", \"bar\"} (3 items)");
        }

        [Test]
        public void NegatedFilterWorksAsExpected()
        {
            IEnumerable<string> randomWords = new List<string> { "yes", "foo", "bar" };
            Check.That(randomWords).Not.HasElementThatMatches((_) => _.StartsWith("to"));
        }

        [Test]
        public void NegatedFilterFailsAsExpected()
        {
            IEnumerable<string> randomWords = new List<string> { "yes", "foo", "bar" };
            Check.ThatCode(() => Check.That(randomWords).Not.HasElementThatMatches((_) => _.StartsWith("ye")))
                .IsAFailingCheckWithMessage("","The checked enumerable contains element(s) that matches the given predicate, whereas it must not.",
                "The checked enumerable:",
                "\t{\"yes\", \"foo\", \"bar\"} (3 items)");
        }

        [Test]
        public void IsSubSetOfWorksWhenExpected()
        {
            IEnumerable<string> randomWords = new List<string> { "yes", "foo"};
            IEnumerable<string> expectedWords = new List<string> { "yes", "foo", "bar" };

            Check.That(randomWords).IsSubSetOf(expectedWords);
        }

        [Test]
        public void IsSubSetOfWorksOnEdgeCase()
        {
            IEnumerable<string> expectedWords = new List<string> { "yes", "foo", "bar" };

            Check.That(new List<string>()).IsSubSetOf(expectedWords);
            Check.That(new List<string>()).IsSubSetOf(new List<string>());
            Check.That(expectedWords).IsSubSetOf(expectedWords);
        }

        [Test]
        public void IsSubSetOfFailsWhenExpected()
        {
            IEnumerable<string> randomWords = new List<string> { "yes", "foo"};
            IEnumerable<string> expectedWords = new List<string> { "yes", "foo", "bar" };
            Check.ThatCode(() =>
                Check.That(expectedWords).IsSubSetOf(randomWords)).
                IsAFailingCheckWithMessage("", 
                    "The checked enumerable contains \"bar\" which is absent from expected value(s).", 
                    "The checked enumerable:", 
                    "\t{\"yes\", \"foo\", \"bar\"} (3 items)", 
            "The expected value(s):", 
            "\t{\"yes\", \"foo\"} (2 items)");
        }
    }
}