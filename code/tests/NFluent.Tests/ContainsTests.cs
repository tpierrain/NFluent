// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContainsTests.cs" company="">
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
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class ContainsTests
    {
        #region Contains with arrays

        [Test]
        public void ContainsWithIntArraysWorks()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).Contains(3, 5, 666);
        }

        [Test]
        public void ContainsWithStringArraysWorks()
        {
            var tresAmigos = new[] { "un", "dos", "tres" };
            Check.That(tresAmigos).Contains("dos");
        }

        [Test]
        public void ContainsWithArraysWorksWhateverTheOrder()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).Contains(666, 3, 5);
        }

        [Test]
        public void ContainsWithArraysWorksEvenWhenGivingSameExpectedValueMultipleTimes()
        {
            var integers = new[] { 1, 2, 3, 4, 5, 666 };
            Check.That(integers).Contains(5, 3, 666, 3, 3, 666);
        }

        [Test]
        public void ContainsWithArraysThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new[] { 1, 2, 3 };

            Check.ThatCode(() =>
            {
                Check.That(integers).Contains(3, 2, 666, 1974);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked enumerable does not contain the expected value(s):", 
                    "\t{666, 1974}", 
                    "The checked enumerable:", 
                    "\t{1, 2, 3} (3 items)", 
                    "The expected value(s):", 
                    "\t{3, 2, 666, 1974} (4 items)");
        }

        [Test]
        public void NotContainsWithArraysWorks()
        {
            var integers = new[] { 1, 2, 3 };
            Check.That(integers).Not.Contains(3, 2, 666, 1974);
        }

        [Test]
        public void NotContainsThrowsExceptionWhenFailingWithArrays()
        {
            var integers = new[] { 1, 2, 3 };

            Check.ThatCode(() =>
            {
                Check.That(integers).Not.Contains(3, 2, 1);
            })
            .IsAFailingCheckWithMessage("", 
                    "The checked enumerable contains all the given values whereas it must not.", 
                    "The checked enumerable:",
                    "\t{1, 2, 3} (3 items)", 
                    "The expected value(s): different from",
                    "\t{3, 2, 1} (3 items)");
        }

        #endregion

        #region Contains with IEnumerable

        [Test]
        public void ContainsWithEnumerableWorks()
        {
            var integers = new List<int> { 1, 2, 3, 1974 };
            IEnumerable expected = new List<int> { 3, 2, 1 };
            Check.That(integers).Contains(expected);
        }

        [Test]
        public void ContainsWithGenericEnumerableWorks()
        {
            var integers = new List<int> { 1, 2, 3, 1974 };
            IEnumerable<int> expected = new List<int> { 3, 2, 1 };
            Check.That(integers).Contains(expected);
        }

        [Test]
        public void ContainsWithEnumerableWorksWithSameContent()
        {
            var integers = new List<int> { 1, 2, 3 };
            IEnumerable expected = new List<int> { 1, 2, 3 };
            Check.That(integers).Contains(expected);
        }

        [Test]
        public void ContainsThrowsWithEmptyList()
        {
            var emptyList = new List<int>();

            Check.ThatCode(() =>
            {
                Check.That(emptyList).Contains("what da heck!");
            })
            .IsAFailingCheckWithMessage("",
                    "The checked enumerable does not contain the expected value(s):",
                    "\t{\"what da heck!\"}",
                    "The checked enumerable:",
                    "\t{} (0 item)",
                    "The expected value(s):",
                    "\t{\"what da heck!\"} (1 item)");
        }

        [Test]
        public void ContainsDoNotThrowIfBothValuesAreEmptyLists()
        {
            var emptyList = new List<int>();

            Check.That(emptyList).Contains(new List<int>());
        }

        [Test]
        public void ContainsThrowsWithNullAsCheckedValue()
        {
            Check.ThatCode(() =>
            {
                Check.That((List<int>)null).Contains("what da heck!");
            })
            .IsAFailingCheckWithMessage("",
                    "The checked enumerable is null and thus, does not contain the given expected value(s).",
                    "The checked enumerable:",
                    "\t[null]",
                    "The expected value(s):",
                    "\t{\"what da heck!\"} (1 item)");
        }

        [Test]
        public void ContainsDoNotThrowIfBothValuesAreNull()
        {
            Check.That((List<int>)null).Contains(null);
        }

        [Test]
        public void ContainsWithEnumerableThrowsExceptionWithClearStatusWhenFails()
        {
            var integers = new List<int> { 1, 2, 3 };
            IEnumerable expectedNumbers = new List<int> { 3, 2, 666, 1974 };

            Check.ThatCode(() =>
            {
                Check.That(integers).Contains(expectedNumbers);
            })
            .IsAFailingCheckWithMessage("",
                    "The checked enumerable does not contain the expected value(s):",
                    "\t{666, 1974}", 
                    "The checked enumerable:",
                    "\t{1, 2, 3} (3 items)",
                    "The expected value(s):", 
                    "\t{3, 2, 666, 1974} (4 items)");
        }

        [Test]
        public void ContainsWithEnumerableOfVariousObjectsTypesWorks()
        {
            var variousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjects = new ArrayList { "tres", 45.3F };
            Check.That(variousObjects).Contains(expectedVariousObjects);
        }


        [Test]
        public void ContainsFailsWithTruncatedTextsForLargeArray()
        {
            const string expected = "this is some expected long text";
            const string actual = "this is some actual long text .";
            Check.ThatCode(() =>
                Check.That(actual.ToCharArray()).IsEquivalentTo(expected.ToCharArray())
            ).IsAFailingCheckWithMessage("", 
                "The checked enumerable is not equivalent to the expected one. 6 differences found!", 
                "'a' should not exist (found in actual[13]); 'p' should be found instead.", 
                "'u' should not exist (found in actual[16]); 'e' should be found instead.", 
                "'a' should not exist (found in actual[17]); 'e' should be found instead.", 
                "'l' should not exist (found in actual[20]); 'd' should be found instead.", 
                "' ' should not exist (found in actual[29]); 'e' should be found instead.",
                "'.' should not exist (found in actual[30]); 'x' should be found instead.",
                "The checked enumerable:", 
                "\t{'t', 'h', 'i', 's', ' ', 'i', 's', ' ', 's', 'o', 'm', 'e', ' ', 'a', 'c', 't', 'u', 'a', 'l', ' ', ...} (31 items)", 
                "The expected enumerable:", 
                "\t{'t', 'h', 'i', 's', ' ', 'i', 's', ' ', 's', 'o', 'm', 'e', ' ', 'e', 'x', 'p', 'e', 'c', 't', 'e', ...} (31 items)");
        }

#endregion
    }
}