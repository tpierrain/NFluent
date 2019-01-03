// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsOnlyMadeOfTests.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using Helpers;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class IsOnlyMadeOfTests
    {
        private CultureSession savedCulture;

        [OneTimeSetUp]
        public void ForceCulture()
        {
            this.savedCulture = new CultureSession("fr-FR");
        }

        [OneTimeTearDown]
        public void RestoreCulture()
        {
            this.savedCulture.Dispose();
        }


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
            .IsAFaillingCheckWithMessage("",
                    "The checked enumerable does not contain only the given value(s).",
                    "It contains also other values:",
                    "\t{666, 1974}",
                    "The checked enumerable:",
                    "\t{3, 2, 666, 1974, 1} (5 items)",
                    "The expected value(s): only elements from",
                    "\t{1, 2, 3} (3 items)");
        }

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
            Check.ThatCode(() =>
            {
                Check.That((List<int>) null).IsOnlyMadeOf("what da heck!");
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked enumerable is null and thus, does not contain exactly the given value(s).",
                    "The checked enumerable:", 
                    "\t[null]", 
                    "The expected value(s): only elements from",
                    "\t{\"what da heck!\"} (1 item)");
        }

        [Test]
        public void IsOnlyMadeOfDoNotThrowIfBothValuesAreNull()
        {
            Check.That((List<int>) null).IsOnlyMadeOf(null).And.IsEqualTo(null);
        }

        [Test]
        public void 
            ShouldFailWhenUsingNull()
        {
            var integers = new List<int> { 3, 2, 666, 1974, 1 };
            Check.ThatCode( () =>
                Check.That(integers).IsOnlyMadeOf(null)).IsAFaillingCheckWithMessage("",
            "The checked enumerable does not contain only the given value(s).",
                "It contains also other values:",
                "\t{3, 2, 666, 1974, 1}",
            "The checked enumerable:",
                "\t{3, 2, 666, 1974, 1} (5 items)",
                "The expected value(s): only elements from",
                "\t[null] (0 item)");
            Check.ThatCode( () =>
                Check.That(new []{1}).IsOnlyMadeOf(null)).IsAFaillingCheckWithMessage("",
                "The checked enumerable does not contain only the given value(s).",
                "It contains also other values:",
                "\t{1}",
                "The checked enumerable:",
                "\t{1} (1 item)",
                "The expected value(s): only elements from",
                "\t[null] (0 item)");
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
            .IsAFaillingCheckWithMessage("",
                    "The checked enumerable does not contain only the given value(s).",
                    "It contains also other values:",
                    "\t{666, 1974}",
                    "The checked enumerable:",
                    "\t{3, 2, 666, 1974, 1} (5 items)",
                    "The expected value(s): only elements from" ,
                    "\t{1, 2, 3} (3 items)");
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
            .IsAFaillingCheckWithMessage("","The checked enumerable contains only the given values whereas it must not.",
                    "The checked enumerable:", 
                    "\t{3, 2, 666, 1974, 1} (5 items)",
                    "The expected value(s): at least one element different from",
                    "\t{1, 2, 3, 666, 1974} (5 items)");
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
            .IsAFaillingCheckWithMessage("",
                    "The checked enumerable contains only the given values whereas it must not.",
                    "The checked enumerable:",
                    "\t{1, 2, 3} (3 items)",
                    "The expected value(s): at least one element different from",
                    "\t{3, 2, 3, 2, 2, 1} (6 items)");
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
            .IsAFaillingCheckWithMessage("", 
                    "The checked enumerable does not contain only the given value(s).",
                    "It contains also other values:",
                    "\t{\"uno\", \"tres\"}",
                    "The checked enumerable:",
                    "\t{1, \"uno\", \"tres\", 45.3} (4 items)",
                    "The expected value(s): only elements from",
                    "\t{1, \"Tres\", 45.3} (3 items)");
        }

        [Test]
        public void IsOnlyMadeOfWithEnumerableOfVariousObjectsTypesWorks()
        {
            var variousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjects = new ArrayList { 1, "uno", "uno", 45.3F, "tres" };
            Check.That(variousObjects).IsOnlyMadeOf(expectedVariousObjects);
        }

    }
}