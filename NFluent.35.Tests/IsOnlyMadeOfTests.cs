// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IsOnlyMadeOfTests.cs" company="">
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
    public class IsOnlyMadeOfTests
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

        #region IsOnlyMadeOf with arrays

        [Test]
        public void IsOnlyMadeOfWithIntArrayWorks()
        {
            var integers = new[] { 1, 2, 3 };
            Check.That(integers).IsOnlyMadeOf(3, 2, 1);
        }

        [Test]
        public void IsOnlyMadeOfWithStringArraysWorks()
        {
            var tresAmigos = new[] { "un", "dos", "tres" };
            Check.That(tresAmigos).IsOnlyMadeOf("dos", "un", "tres");
        }

        [Test]
        public void IsOnlyMadeOfWithArraysWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            var integers = new[] { 1, 2, 3 };
            Check.That(integers).IsOnlyMadeOf(3, 2, 3, 2, 2, 1);
        }

        [Test]
        public void IsOnlyMadeOfWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new[] { 3, 2, 666, 1974, 1 };

            Check.ThatCode(() =>
            {
                Check.That(integers).IsOnlyMadeOf(1, 2, 3);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable does not contain only the given value(s).\nIt contains also other values:\n\t[666, 1974]\nThe checked enumerable:\n\t[3, 2, 666, 1974, 1]\nThe expected value(s):\n\t[1, 2, 3]");
        }

        #endregion

        #region IsOnlyMadeOf with enumerable

        [Test]
        public void IsOnlyMadeOfWithEnumerableWorks()
        {
            var integers = new List<int> { 1, 2, 3 };
            IEnumerable expectedIntegers = new List<int> { 3, 2, 1 };
            Check.That(integers).IsOnlyMadeOf(expectedIntegers);
        }

        [Test]
        public void IsOnlyMadeOfWithGenericEnumerableWorks()
        {
            var integers = new List<int> { 1, 2, 3 };
            IEnumerable<int> expectedIntegers = new List<int> { 3, 2, 1 };
            Check.That(integers).IsOnlyMadeOf(expectedIntegers);
        }

        [Test]
        public void IsOnlyMadeOfWithStringEnumerableWorks()
        {
            var tresAmigos = new List<string> { "un", "dos", "tres" };
            IEnumerable expectedValues = new List<string> { "un", "tres", "dos" };
            Check.That(tresAmigos).IsOnlyMadeOf(expectedValues);
        }

        [Test]
        public void IsOnlyMadeOfWithEnumerableWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            var integers = new List<int> { 1, 2, 3 };
            IEnumerable expectedValues = new List<int> { 3, 2, 3, 2, 2, 1 };

            Check.That(integers).IsOnlyMadeOf(expectedValues);
        }

        [Test]
        public void IsOnlyMadeOfWorksWithAnEmptyList()
        {
            var emptyList = new List<int>();

            Check.That(emptyList).IsOnlyMadeOf("what da heck!");
        }

        [Test]
        public void IsOnlyMadeOfDoNotThrowIfBothValuesAreEmptyLists()
        {
            var emptyList = new List<int>();

            Check.That(emptyList).IsOnlyMadeOf(new List<int>());
        }

        [Test]
        public void IsOnlyMadeOfThrowsWithNullAsCheckedValue()
        {
            List<int> nullList = null;

            Check.ThatCode(() =>
            {
                Check.That(nullList).IsOnlyMadeOf("what da heck!");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable is null and thus, does not contain exactly the given value(s).\nThe checked enumerable:\n\t[null]\nThe expected value(s):\n\t[\"what da heck!\"]");
        }

        [Test]
        public void IsOnlyMadeOfDoNotThrowIfBothValuesAreNull()
        {
            List<int> nullList = null;

            Check.That(nullList).IsOnlyMadeOf(null).And.IsEqualTo(null);
        }

        [Test]
        public void IsOnlyMadeOfWithEnumerableThrowsExceptionWhenFailing()
        {
            var integers = new List<int> { 3, 2, 666, 1974, 1 };
            IEnumerable expectedValues = new List<int> { 1, 2, 3 };

            Check.ThatCode(() =>
            {
                Check.That(integers).IsOnlyMadeOf(expectedValues);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable does not contain only the given value(s).\nIt contains also other values:\n\t[666, 1974]\nThe checked enumerable:\n\t[3, 2, 666, 1974, 1]\nThe expected value(s):\n\t[1, 2, 3]");
        }

        [Test]
        public void NotIsOnlyMadeOfWithEnumerableWorks()
        {
            var integers = new List<int> { 3, 2, 666, 1974, 1 };
            IEnumerable expectedValues = new List<int> { 1, 666, 1974 };
            Check.That(integers).Not.IsOnlyMadeOf(expectedValues);
        }

        [Test]
        public void NotIsOnlyMadeOfWithEnumerableThrowsExceptionWhenFailing()
        {
            var integers = new List<int> { 3, 2, 666, 1974, 1 };
            IEnumerable expectedValues = new List<int> { 1, 2, 3, 666, 1974 };

            Check.ThatCode(() =>
            {
                Check.That(integers).Not.IsOnlyMadeOf(expectedValues);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable contains only the given values whereas it must not.\nThe checked enumerable:\n\t[3, 2, 666, 1974, 1]\nThe expected value(s):\n\t[1, 2, 3, 666, 1974]");
        }

        [Test]
        public void IsOnlyMadeOfWithGenericListWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            List<int> integers = new List<int> { 1, 2, 3 };
            List<int> expectedValues = new List<int> { 3, 2, 3, 2, 2, 1 };

            Check.That(integers).IsOnlyMadeOf(expectedValues);
        }

        [Test]
        public void IsOnlyMadeOfWithArrayListWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            List<int> integers = new List<int> { 1, 2, 3 };
            ArrayList expectedValues = new ArrayList { 3, 2, 3, 2, 2, 1 };

            Check.That(integers).IsOnlyMadeOf(expectedValues);
        }

        [Test]
        public void NotIsOnlyMadeOfWithArrayListThrowsWhenFailing()
        {
            List<int> integers = new List<int> { 1, 2, 3 };
            ArrayList expectedValues = new ArrayList { 3, 2, 3, 2, 2, 1 };

            Check.ThatCode(() =>
            {
                Check.That(integers).Not.IsOnlyMadeOf(expectedValues);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable contains only the given values whereas it must not.\nThe checked enumerable:\n\t[1, 2, 3]\nThe expected value(s):\n\t[3, 2, 3, 2, 2, 1]");
        }

        [Test]
        public void IsOnlyMadeOfWithStringCollectionWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            List<string> oneTwoThree = new List<string> { "one", "two", "three" };
            StringCollection expectedValues = new StringCollection { "three", "two", "three", "two", "two", "one" };

            Check.That(oneTwoThree).IsOnlyMadeOf(expectedValues);
        }

        [Test]
        public void IsOnlyMadeOfWithEnumerableThrowCaseSensitiveException()
        {
            var variousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjectsWithBadCase = new ArrayList { 1, "Tres", 45.3F };

            Check.ThatCode(() =>
            {
                Check.That(variousObjects).IsOnlyMadeOf(expectedVariousObjectsWithBadCase);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable does not contain only the given value(s).\nIt contains also other values:\n\t[\"uno\", \"tres\"]\nThe checked enumerable:\n\t[1, \"uno\", \"tres\", 45.3]\nThe expected value(s):\n\t[1, \"Tres\", 45.3]");
        }

        [Test]
        public void IsOnlyMadeOfWithEnumerableOfVariousObjectsTypesWorks()
        {
            var variousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjects = new ArrayList { 1, "uno", "uno", 45.3F, "tres" };
            Check.That(variousObjects).IsOnlyMadeOf(expectedVariousObjects);
        }

        #endregion
    }
}