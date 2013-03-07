namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    using NUnit.Framework;

    using Assert = NFluent.Assert;

    public class ContainsTests
    {
        #region Contains with arrays

        [Test]
        public void ContainsWithIntArraysWorks()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers).Contains(3, 5, 666);
        }

        [Test]
        public void ContainsWithStringArraysWorks()
        {
            List<string> enumerable = new List<string>() { "un", "dos", "tres" };
            Assert.That(enumerable).Contains("dos");
        }

        [Test]
        public void ContainsWithArraysWorksWhateverTheOrder()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers).Contains(666, 3, 5);
        }

        [Test]
        public void ContainsWithArraysWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Assert.That(integers).Contains(5, 3, 666, 3, 3, 666);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The enumerable [1, 2, 3] does not contain the expected value(s): [666, 1974].")]
        public void ContainsWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new int[] { 1, 2, 3 };
            Assert.That(integers).Contains(3, 2, 666, 1974);
        }

        #endregion

        #region Contains with IEnumerable

        [Test]
        public void ContainsWithEnumerableWorks()
        {
            var integers = new List<int>() { 1, 2, 3, 1974 };
            IEnumerable expected = new List<int>() { 3, 2, 1 };
            Assert.That(integers).Contains(expected);
        }

        [Test]
        public void ContainsWithGenericEnumerableWorks()
        {
            var integers = new List<int>() { 1, 2, 3, 1974 };
            IEnumerable<int> expected = new List<int>() { 3, 2, 1 };
            Assert.That(integers).Contains(expected);
        }

        [Test]
        public void ContainsWithEnumerableWorksWithSameContent()
        {
            var integers = new List<int>() { 1, 2, 3 };
            IEnumerable expected = new List<int>() { 1, 2, 3 };
            Assert.That(integers).Contains(expected);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The enumerable [1, 2, 3] does not contain the expected value(s): [666, 1974].")]
        public void ContainsWithEnumerableThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new List<int>() { 1, 2, 3 };
            IEnumerable expected = new List<int>() { 3, 2, 666, 1974 };
            Assert.That(integers).Contains(expected);
        }

        // TODO: write a Contains test with IEnumerable containing of objects with various types
        #endregion
    }
}