namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class EnumerableRelatedTests
    {
        #region HasSize

        [Test]
        public void HasSizeWorksWithArray()
        {
            var array = new int[] { 45, 43, 54, 666 };

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
            IEnumerable enumerable = new List<int>() { 45, 43, 54, 666 };

            Check.That(enumerable).HasSize(4);
        }

        [Test]
        public void HasSizeWorksWithGenericEnumerable()
        {
            IEnumerable<int> enumerable = new List<int>() { 45, 43, 54, 666 };

            Check.That(enumerable).HasSize(4).And.Contains(666);
        }

        [Test]
        public void HasSizeWorksWithArrayList()
        {
            ArrayList arrayList = new ArrayList() { 45, 43, 54, 666 };

            Check.That(arrayList).HasSize(4);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nFound 1 element instead of 5.\nFound:\n\t[666]")]
        public void HasSizeThrowsExceptionWithClearStatusWhenFailsWithOneElementFound()
        {
            var enumerable = new List<int>() { 666 };

            Check.That(enumerable).HasSize(5);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nFound 4 elements instead of 1.\nFound:\n\t[45, 43, 54, 666]")]
        public void HasSizeThrowsExceptionWithClearStatusWhenFailsWithOneExpectedElement()
        {
            var enumerable = new List<int>() { 45, 43, 54, 666 };

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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable is not empty. Contains:\n\t[null, null, Thomas]")]
        public void IsEmptyThrowsExceptionWhenNotEmpty()
        {
            var persons = new List<Person>() { null, null, new Person() { Name = "Thomas" } };
            
            Check.That(persons).IsEmpty();
        }

        #endregion

        [Test]
        public void AndOperatorWorksWithAllMethodsOfEnumerableFluentAssertion()
        {
            var killingSeries = new List<string>() { "The wire", "Game of Thrones" };
            
            Check.That(killingSeries).HasSize(2).And.ContainsOnly("Game of Thrones", "The wire").And.ContainsExactly("The wire", "Game of Thrones");
            Check.That(killingSeries).Contains("The wire").And.ContainsOnly("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).ContainsExactly("The wire", "Game of Thrones").And.ContainsOnly("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).ContainsOnly("Game of Thrones", "The wire").And.Contains("The wire").And.HasSize(2);
            Check.That(killingSeries).IsEqualTo(killingSeries).And.Contains("The wire");
            Check.That(killingSeries).IsNotEqualTo(null).And.Contains("Game of Thrones");
            
            var integerEmptyList = new List<int>();
            Check.That(integerEmptyList).IsEmpty().And.HasSize(0);
        }

        [Test]
        public void AndOperatorWorksWithAllMethodsOfEnumerableFluentAssertionOnEnumerable()
        {
            IEnumerable killingSeries = new List<string>() { "The wire", "Game of Thrones" };

            Check.That(killingSeries).HasSize(2).And.ContainsOnly("Game of Thrones", "The wire").And.ContainsExactly("The wire", "Game of Thrones");
            Check.That(killingSeries).Contains("The wire").And.ContainsOnly("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).ContainsExactly("The wire", "Game of Thrones").And.ContainsOnly("Game of Thrones", "The wire").And.HasSize(2);
            Check.That(killingSeries).ContainsOnly("Game of Thrones", "The wire").And.Contains("The wire").And.HasSize(2);

            IEnumerable integerEmptyList = new List<int>();
            Check.That(integerEmptyList).IsEmpty().And.HasSize(0);
        }
    }
}