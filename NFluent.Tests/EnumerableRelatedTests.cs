namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Assert = NFluent.Assert;

    [TestFixture]
    public class EnumerableRelatedTests
    {
        #region HasSize

        [Test]
        public void HasSizeWorksWithArray()
        {
            var array = new int[] { 45, 43, 54, 666 };

            Assert.That(array).HasSize(4);
        }

        [Test]
        public void HasSizeGivesTheNumberOfElementsAndNotTheCapacity()
        {
            var enumerable = new List<string>(500);

            Assert.That(enumerable).HasSize(0);
        }

        [Test]
        public void HasSizeWorksWithEnumerable()
        {
            List<int> enumerable = new List<int>() { 45, 43, 54, 666 };

            Assert.That(enumerable).HasSize(4);
        }

        [Test]
        public void HasSizeWorksWithGenericEnumerable()
        {
            IEnumerable<int> enumerable = new List<int>() { 45, 43, 54, 666 };

            Assert.That(enumerable).HasSize(4);
        }

        [Test]
        public void HasSizeWorksWithArrayList()
        {
            ArrayList arrayList = new ArrayList() { 45, 43, 54, 666 };

            Assert.That(arrayList).HasSize(4);
        }

        

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Has [4] items instead of the expected value [2].")]
        public void HasSizeThrowsExceptionWithClearStatusWhenFails()
        {
            var enumerable = new List<int>() { 45, 43, 54, 666 };

            Assert.That(enumerable).HasSize(2);
        }

        #endregion

        #region IsEmpty

        [Test]
        public void IsEmptyWorks()
        {
            var emptyEnumerable = new List<int>();

            Assert.That(emptyEnumerable).IsEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"Enumerable not empty. Contains the element(s): [null, null, Thomas].")]
        public void IsEmptyThrowsExceptionWhenNotEmpty()
        {
            var nobody = new List<Person>() { null, null, new Person() { Name = "Thomas" } };
            
            Assert.That(nobody).IsEmpty();
        }

        #endregion
    }
}