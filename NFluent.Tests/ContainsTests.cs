namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class ContainsTests
    {
        #region Contains with arrays

        [Test]
        public void ContainsWithIntArraysWorks()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).Contains(3, 5, 666);
        }

        [Test]
        public void ContainsWithStringArraysWorks()
        {
            var tresAmigos = new string[] { "un", "dos", "tres" };
            Check.That(tresAmigos).Contains("dos");
        }

        [Test]
        public void ContainsWithArraysWorksWhateverTheOrder()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).Contains(666, 3, 5);
        }

        [Test]
        public void ContainsWithArraysWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            var integers = new int[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).Contains(5, 3, 666, 3, 3, 666);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual enumerable:\n\t[1, 2, 3]\ndoes not contain the expected value(s):\n\t[666, 1974]")]
        public void ContainsWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new int[] { 1, 2, 3 };
            Check.That(integers).Contains(3, 2, 666, 1974);
        }

        #endregion

        #region Contains with IEnumerable

        [Test]
        public void ContainsWithEnumerableWorks()
        {
            var integers = new List<int>() { 1, 2, 3, 1974 };
            IEnumerable expected = new List<int>() { 3, 2, 1 };
            Check.That(integers).Contains(expected);
        }

        [Test]
        public void ContainsWithGenericEnumerableWorks()
        {
            var integers = new List<int>() { 1, 2, 3, 1974 };
            IEnumerable<int> expected = new List<int>() { 3, 2, 1 };
            Check.That(integers).Contains(expected);
        }

        [Test]
        public void ContainsWithEnumerableWorksWithSameContent()
        {
            var integers = new List<int>() { 1, 2, 3 };
            IEnumerable expected = new List<int>() { 1, 2, 3 };
            Check.That(integers).Contains(expected);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual enumerable:\n\t[1, 2, 3]\ndoes not contain the expected value(s):\n\t[666, 1974]")]
        public void ContainsWithEnumerableThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new List<int>() { 1, 2, 3 };
            IEnumerable expectedNumbers = new List<int>() { 3, 2, 666, 1974 };
            Check.That(integers).Contains(expectedNumbers);
        }

        [Test]
        public void ContainsWithEnumerableOfVariousObjectsTypesWorks()
        {
            var variousObjects = new ArrayList() { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjects = new ArrayList() { "tres", 45.3F };
            Check.That(variousObjects).Contains(expectedVariousObjects);
        }

        #endregion
    }
}