namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    public class ContainsOnlyTests
    {
        private CultureInfo savedCulture;

        [SetUp]
        public void SetUp()
        {
            // Important so that ToString() versions of decimal works whatever the current culture.
            this.savedCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
        }

        [TearDown]
        public void TearDown()
        {
            // Boy scout rule ;-)
            Thread.CurrentThread.CurrentCulture = this.savedCulture;
        }

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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[3, 2, 666, 1974, 1]\ndoes not contain only the expected value(s):\n\t[1, 2, 3].\nIt contains also other values:\n\t[666, 1974]")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[3, 2, 666, 1974, 1]\ndoes not contain only the expected value(s):\n\t[1, 2, 3].\nIt contains also other values:\n\t[666, 1974]")]
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

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe enumerable:\n\t[1, \"uno\", \"tres\", 45,3]\ndoes not contain only the expected value(s):\n\t[1, \"Tres\", 45,3].\nIt contains also other values:\n\t[\"uno\", \"tres\"]")]
        public void ContainsOnlyWithEnumerableThrowCaseSensitiveException()
        {
            var variousObjects = new ArrayList() { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjectsWithBadCase = new ArrayList() { 1, "Tres", 45.3F };
            Check.That(variousObjects).ContainsOnly(expectedVariousObjectsWithBadCase);
        }

        [Test]
        public void ContainsOnlyWithEnumerableOfVariousObjectsTypesWorks()
        {
            var variousObjects = new ArrayList() { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjects = new ArrayList() { 1, "uno", "uno", 45.3F, "tres" };
            Check.That(variousObjects).ContainsOnly(expectedVariousObjects);
        }

        #endregion
    }
}