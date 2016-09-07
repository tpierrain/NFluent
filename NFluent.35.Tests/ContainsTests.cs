﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ContainsTests.cs" company="">
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
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable does not contain the expected value(s):\n\t[666, 1974]\nThe checked enumerable:\n\t[1, 2, 3]\nThe expected value(s):\n\t[3, 2, 666, 1974]");
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
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable contains all the given values whereas it must not.\nThe checked enumerable:\n\t[1, 2, 3]\nThe expected value(s):\n\t[3, 2, 1]");
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
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable does not contain the expected value(s):\n\t[\"what da heck!\"]\nThe checked enumerable:\n\t[]\nThe expected value(s):\n\t[\"what da heck!\"]");
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
            List<int> nullList = null;

            Check.ThatCode(() =>
            {
                Check.That(nullList).Contains("what da heck!");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable is null and thus, does not contain the given expected value(s).\nThe checked enumerable:\n\t[null]\nThe expected value(s):\n\t[\"what da heck!\"]");
        }

        [Test]
        public void ContainsDoNotThrowIfBothValuesAreNull()
        {
            List<int> nullList = null;

            Check.That(nullList).Contains(null);
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
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable does not contain the expected value(s):\n\t[666, 1974]\nThe checked enumerable:\n\t[1, 2, 3]\nThe expected value(s):\n\t[3, 2, 666, 1974]");
        }

        [Test]
        public void ContainsWithEnumerableOfVariousObjectsTypesWorks()
        {
            var variousObjects = new ArrayList { 1, "uno", "tres", 45.3F };
            IEnumerable expectedVariousObjects = new ArrayList { "tres", 45.3F };
            Check.That(variousObjects).Contains(expectedVariousObjects);
        }

        #endregion
    }
}