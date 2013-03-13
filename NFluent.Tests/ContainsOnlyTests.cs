namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using NUnit.Framework;

    public class ContainsOnlyTests
    {
        #region ContainsOnly with arrays

        [Test]
        public void ContainsOnlyWithIntArrayWorks()
        {
            var integers = new int[] { 1, 2, 3 };
            Check.That(integers).ContainsOnly(3, 2, 1);
        }

        [Test]
        public void ContainsOnlyWithStringArraysWorks()
        {
            var tresAmigos = new string[] { "un", "dos", "tres" };
            Check.That(tresAmigos).ContainsOnly("dos", "un", "tres");
        }

        [Test]
        public void ContainsOnlyWithArraysWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            var integers = new int[] { 1, 2, 3 };
            Check.That(integers).ContainsOnly(3, 2, 3, 2, 2, 1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The enumerable [3, 2, 666, 1974, 1] does not contain only the expected value(s). It contains also other values: [666, 1974].")]
        public void ContainsOnlyWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new int[] { 3, 2, 666, 1974, 1 };
            Check.That(integers).ContainsOnly(1, 2, 3);
        }

        #endregion

        #region ContainsOnly with enumerable

        [Test]
        public void ContainsOnlyWithEnumerableWorks()
        {
            var integers = new List<int>() { 1, 2, 3 };
            IEnumerable expectedIntegers = new List<int>() { 3, 2, 1 };
            Check.That(integers).ContainsOnly(expectedIntegers);
        }

        [Test]
        public void ContainsOnlyWithGenericEnumerableWorks()
        {
            var integers = new List<int>() { 1, 2, 3 };
            IEnumerable<int> expectedIntegers = new List<int>() { 3, 2, 1 };
            Check.That(integers).ContainsOnly(expectedIntegers);
        }

        [Test]
        public void ContainsOnlyWithStringEnumerableWorks()
        {
            var tresAmigos = new List<string>() { "un", "dos", "tres" };
            IEnumerable expectedValues = new List<string>() { "un", "tres", "dos" };
            Check.That(tresAmigos).ContainsOnly(expectedValues);
        }

        [Test]
        public void ContainsOnlyWithEnumerableWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            var integers = new List<int> { 1, 2, 3 };
            IEnumerable expectedValues = new List<int> { 3, 2, 3, 2, 2, 1 };

            Check.That(integers).ContainsOnly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The enumerable [3, 2, 666, 1974, 1] does not contain only the expected value(s). It contains also other values: [666, 1974].")]
        public void ContainsOnlyWithEnumerableThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new List<int> { 3, 2, 666, 1974, 1 };
            IEnumerable expectedValues = new List<int> { 1, 2, 3 };
            Check.That(integers).ContainsOnly(expectedValues);
        }

        [Test]
        public void ContainsOnlyWithGenericListWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            List<int> integers = new List<int> { 1, 2, 3 };
            List<int> expectedValues = new List<int> { 3, 2, 3, 2, 2, 1 };

            Check.That(integers).ContainsOnly(expectedValues);
        }

        [Test]
        public void ContainsOnlyWithArrayListWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            List<int> integers = new List<int> { 1, 2, 3 };
            ArrayList expectedValues = new ArrayList() { 3, 2, 3, 2, 2, 1 };

            Check.That(integers).ContainsOnly(expectedValues);
        }

        [Test]
        public void ContainsOnlyWithStringCollectionWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            List<string> oneTwoThree = new List<string> { "one", "two", "three" };
            StringCollection expectedValues = new StringCollection() { "three", "two", "three", "two", "two", "one" };

            Check.That(oneTwoThree).ContainsOnly(expectedValues);
        }

        // TODO: write a ContainsOnly test with IEnumerable containing of objects with various types
        #endregion
    }
}