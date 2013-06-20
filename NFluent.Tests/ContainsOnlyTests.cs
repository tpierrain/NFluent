// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ContainsOnlyTests.cs" company="">
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
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
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
            var integers = new[] { 1, 2, 3 };
            Check.That(integers).ContainsOnly(3, 2, 1);
        }

        [Test]
        public void ContainsOnlyWithStringArraysWorks()
        {
            var tresAmigos = new[] { "un", "dos", "tres" };
            Check.That(tresAmigos).ContainsOnly("dos", "un", "tres");
        }

        [Test]
        public void ContainsOnlyWithArraysWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            var integers = new[] { 1, 2, 3 };
            Check.That(integers).ContainsOnly(3, 2, 3, 2, 2, 1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable does not contain only the given value(s).\nIt contains also other values:\n\t[666, 1974]\nThe checked enumerable:\n\t[3, 2, 666, 1974, 1]\nThe expected enumerable:\n\t[1, 2, 3]")]
        public void ContainsOnlyWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new[] { 3, 2, 666, 1974, 1 };
            Check.That(integers).ContainsOnly(1, 2, 3);
        }

        #endregion

        #region ContainsOnly with enumerable

        [Test]
        public void ContainsOnlyWithEnumerableWorks()
        {
            var integers = new List<int> { 1, 2, 3 };
            IEnumerable expectedIntegers = new List<int> { 3, 2, 1 };
            Check.That(integers).ContainsOnly(expectedIntegers);
        }

        [Test]
        public void ContainsOnlyWithGenericEnumerableWorks()
        {
            var integers = new List<int> { 1, 2, 3 };
            IEnumerable<int> expectedIntegers = new List<int> { 3, 2, 1 };
            Check.That(integers).ContainsOnly(expectedIntegers);
        }

        [Test]
        public void ContainsOnlyWithStringEnumerableWorks()
        {
            var tresAmigos = new List<string> { "un", "dos", "tres" };
            IEnumerable expectedValues = new List<string> { "un", "tres", "dos" };
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable does not contain only the given value(s).\nIt contains no value at all!\nThe checked enumerable:\n\t[]\nThe expected enumerable:\n\t[\"what da heck!\"]")]
        public void ContainsOnlyThrowsWithEmptyList()
        {
            var emptyList = new List<int>();

            Check.That(emptyList).ContainsOnly("what da heck!");
        }

        [Test]
        public void ContainsOnlyDoNotThrowIfBothValuesAreEmptyLists()
        {
            var emptyList = new List<int>();

            Check.That(emptyList).ContainsOnly(new List<int>());
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable is null and thus, does not contain exactly the given value(s).\nThe checked enumerable:\n\t[null]\nThe expected enumerable:\n\t[\"what da heck!\"]")]
        public void ContainsOnlyThrowsWithNullAsCheckedValue()
        {
            List<int> nullList = null;

            Check.That(nullList).ContainsOnly("what da heck!");
        }

        [Test]
        public void ContainsOnlyDoNotThrowIfBothValuesAreNull()
        {
            List<int> nullList = null;

            Check.That(nullList).ContainsOnly(null).And.IsEqualTo(null);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable does not contain only the given value(s).\nIt contains also other values:\n\t[666, 1974]\nThe checked enumerable:\n\t[3, 2, 666, 1974, 1]\nThe expected enumerable:\n\t[1, 2, 3]")]
        public void ContainsOnlyWithEnumerableThrowsExceptionWhenFailing()
        {
            var integers = new List<int> { 3, 2, 666, 1974, 1 };
            IEnumerable expectedValues = new List<int> { 1, 2, 3 };
            Check.That(integers).ContainsOnly(expectedValues);
        }

        [Test]
        public void NotContainsOnlyWithEnumerableWorks()
        {
            var integers = new List<int> { 3, 2, 666, 1974, 1 };
            IEnumerable expectedValues = new List<int> { 1, 666, 1974 };
            Check.That(integers).Not.ContainsOnly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable contains only the given values whereas it must not.\nThe checked enumerable:\n\t[3, 2, 666, 1974, 1]\nThe expected enumerable:\n\t[1, 2, 3, 666, 1974]")]
        public void NotContainsOnlyWithEnumerableThrowsExceptionWhenFailing()
        {
            var integers = new List<int> { 3, 2, 666, 1974, 1 };
            IEnumerable expectedValues = new List<int> { 1, 2, 3, 666, 1974 };
            Check.That(integers).Not.ContainsOnly(expectedValues);
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
            ArrayList expectedValues = new ArrayList { 3, 2, 3, 2, 2, 1 };

            Check.That(integers).ContainsOnly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable contains only the given values whereas it must not.\nThe checked enumerable:\n\t[1, 2, 3]\nThe expected enumerable:\n\t[3, 2, 3, 2, 2, 1]")]
        public void NotContainsOnlyWithArrayListThrowsWhenFailing()
        {
            List<int> integers = new List<int> { 1, 2, 3 };
            ArrayList expectedValues = new ArrayList { 3, 2, 3, 2, 2, 1 };

            Check.That(integers).Not.ContainsOnly(expectedValues);
        }

        [Test]
        public void ContainsOnlyWithStringCollectionWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            List<string> oneTwoThree = new List<string> { "one", "two", "three" };
            StringCollection expectedValues = new StringCollection { "three", "two", "three", "two", "two", "one" };

            Check.That(oneTwoThree).ContainsOnly(expectedValues);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable does not contain only the given value(s).\nIt contains also other values:\n\t[\"uno\", \"tres\"]\nThe checked enumerable:\n\t[1, \"uno\", \"tres\", 45,3]\nThe expected enumerable:\n\t[1, \"Tres\", 45,3]")]
        public void ContainsOnlyWithEnumerableThrowCaseSensitiveException()
        {
            var variousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjectsWithBadCase = new ArrayList { 1, "Tres", 45.3F };
            Check.That(variousObjects).ContainsOnly(expectedVariousObjectsWithBadCase);
        }

        [Test]
        public void ContainsOnlyWithEnumerableOfVariousObjectsTypesWorks()
        {
            var variousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjects = new ArrayList { 1, "uno", "uno", 45.3F, "tres" };
            Check.That(variousObjects).ContainsOnly(expectedVariousObjects);
        }

        #endregion
    }
}