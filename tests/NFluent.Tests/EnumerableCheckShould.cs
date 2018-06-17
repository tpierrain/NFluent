 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="AsOperatorTests.cs" company="">
 //   Copyright 2018 Cyrille DUPUYDAUBY
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Extensibility;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class EnumerableCheckShould
    {
        [Test]
        public void
            FindDuplicates()
        {
            var array = new int [] {1, 2, 3, 4};
            Check.That(array).ContainsNoDuplicateItem();

            Check.ThatCode(() => Check.That(new[] {1, 1, 2, 3}).ContainsNoDuplicateItem()).
                IsAFaillingCheckWithMessage("", 
                    "The checked enumerable contains a duplicate item at position 1: [1].", 
                    "The checked enumerable:", 
                    "\t[1, 1, 2, 3] (4 items)");
        }

        [Test]
        public void
            VerifyDuplicatesAreHere()
        {
            var array = new[] {1, 1, 2, 3};
            Check.That(array).Not.ContainsNoDuplicateItem();

            Check.ThatCode(() => Check.That(new[] {1, 2, 3, 4}).Not.ContainsNoDuplicateItem()).
                IsAFaillingCheckWithMessage("",
                    "The checked enumerable should contain duplicates.",
                    "The checked enumerable:",
                    "\t[1, 2, 3, 4] (4 items)");
        }

        [Test]
        public void
            CheckForPresenceOfNull()
        {
            var array = new [] {"test", "another"};
            Check.That(array).ContainsNoNull();

            Check.ThatCode(() => Check.That(new [] {"test", "another", null}).ContainsNoNull()).
                IsAFaillingCheckWithMessage("",
                    "The checked enumerable contains a null item at position 2.",
                    "The checked enumerable:",
                    "\t[\"test\", \"another\", null] (3 items)");
        }

        [Test]
        public void
            CheckForAbsenceOfNull()
        {
            var array = new [] {"test", "another", null};
            Check.That(array).Not.ContainsNoNull();

            Check.ThatCode(() => Check.That(new [] {"test", "another"}).Not.ContainsNoNull()).
                IsAFaillingCheckWithMessage("",
                    "The checked enumerable should contain at least one null entry.",
                    "The checked enumerable:",
                    "\t[\"test\", \"another\"] (2 items)");
        }

        [Test]
        public void
            CheckForPresenceOfWrongType()
        {
            var array = new [] {"test", "another"};
            Check.That(array).ContainsOnlyInstanceOfType(typeof(string));

            Check.ThatCode(() => Check.That(new List<object>() {"test", "another", 4}).ContainsOnlyInstanceOfType(typeof(string))).
                IsAFaillingCheckWithMessage("",
                    "The checked enumerable contains an entry of a type different from String at position 2.",
                    "The checked enumerable:",
                    "\t[\"test\", \"another\", 4] (3 items)");
        }
        
        [Test]
        public void
            CheckForAbsencefWrongType()
        {
            var array = new List<object> {"test", "another", 4};
            Check.That(array).Not.ContainsOnlyInstanceOfType(typeof(string));

            Check.ThatCode(() => Check.That(new List<object> {"test", "another"}).Not.ContainsOnlyInstanceOfType(typeof(string))).
                IsAFaillingCheckWithMessage("",
                    "The checked enumerable should contain at least one entry of a type different from String.",
                    "The checked enumerable:",
                    "\t[\"test\", \"another\"] (2 items)");
        }

        [Test]
        public void
            CheckForEquivalency()
        {
            var array = new[] {1, 2, 3};

            Check.That(array).IsEquivalentTo(3, 2, 1);
            IEnumerable<int> nullArray = null;
            Check.That<IEnumerable<int>>(null).IsEquivalentTo(nullArray);

            Check.ThatCode(() => Check.That(array).IsEquivalentTo(1, 2, 4)).IsAFaillingCheck();

            var otherArray = new List<int>(new []{3, 2, 1});
            Check.That(array).IsEquivalentTo(otherArray);
        }
        
        [Test]
        public void
            CheckForEquivalencyFailsWithProperMessage()
        {
            var array = new[] {1, 2, 3};

            Check.ThatCode(() => Check.That(array).IsEquivalentTo(new[] {1, 2, 4})).IsAFaillingCheckWithMessage("", 
                "The checked enumerable does contain [3] whereas it should not.", 
                "The checked enumerable:",
                "\t[1, 2, 3] (3 items)", 
                "The expected value(s):", 
                "\t[1, 2, 4] (3 items)");

            Check.ThatCode(() => Check.That(array).IsEquivalentTo(new[] {1, 2, 3, 4})).IsAFaillingCheckWithMessage("", 
                "The checked enumerable is missing: [4].", 
                "The checked enumerable:",
                "\t[1, 2, 3] (3 items)", 
                "The expected value(s):", 
                "\t[1, 2, 3, 4] (4 items)");

            Check.ThatCode(() => Check.That(array).IsEquivalentTo(new[] {1, 2, 3, 4, 5})).IsAFaillingCheckWithMessage("", 
                "The checked enumerable is missing 2 items: [4, 5].", 
                "The checked enumerable:",
                "\t[1, 2, 3] (3 items)", 
                "The expected value(s):", 
                "\t[1, 2, 3, 4, 5] (5 items)");

            Check.ThatCode(() => Check.That(array).IsEquivalentTo(new[] {1, 2})).IsAFaillingCheckWithMessage("", 
                "The checked enumerable does contain [3] whereas it should not.", 
                "The checked enumerable:",
                "\t[1, 2, 3] (3 items)", 
                "The expected value(s):", 
                "\t[1, 2] (2 items)");

            Check.ThatCode(() => Check.That<IEnumerable<int>>(null).IsEquivalentTo(1)).IsAFaillingCheck();
        }

        [Test]
        public void
            VerifyAscendingOrder()
        {
            Check.That(new int?[] {0, 1, 2, 3, 5}).IsInAscendingOrder();
            Check.That(new int?[] {null, 1, 2, 3, 5}).IsInAscendingOrder();
            Check.ThatCode(() => Check.That(new int?[] {null, 1, 2, 3, 5, null, 5}).IsInAscendingOrder()).IsAFaillingCheckWithMessage(
                "",
                "The checked enumerable is not in ascending order, whereas it should.",
                "At #5: [5] comes after [null].", 
                "The checked enumerable:", 
                "\t[null, 1, 2, 3, 5, null, 5] (7 items)");
            Check.ThatCode(() => Check.That(new int?[] {4, 1, 2, 3, 5, null, 5}).IsInAscendingOrder()).IsAFaillingCheckWithMessage(
                "",
                "The checked enumerable is not in ascending order, whereas it should.",
                "At #1: [4] comes after [1].", "The checked enumerable:",
                "\t[4, 1, 2, 3, 5, null, 5] (7 items)");
        }

        [Test]
        public void AscendingSupportCustomComparer()
        {
            var custom = new HardComparer(-1);
            Check.That(new [] {0, 1, 2, 4, 3, 5}).IsInAscendingOrder(custom);
            Check.That(new [] {5, 4, 3, 2, 1, 0}).IsInAscendingOrder(custom);
        }

        [Test]
        public void DescendingSupportCustomComparer()
        {
            var custom = new HardComparer(1);
            Check.That(new [] {0, 1, 2, 4, 3, 5}).IsInDescendingOrder(custom);
            Check.That(new [] {5, 4, 3, 2, 1, 0}).IsInDescendingOrder(custom);
        }

        class HardComparer: IComparer
        {
            private readonly int result;

            public HardComparer(int result)
            {
                this.result = result;
            }

            public int Compare(object x, object y)
            {
                return this.result;
            }
        }

        [Test]
        public void
            AscendingOrderSupportEdgeCases()
        {
            Check.That(new[] {new object(), new object()}).IsInAscendingOrder();
            Check.That(new[] {new object(), null}).IsInAscendingOrder();
        }

        [Test]
        public void
            VerifyDescendingOrder()
        {
            Check.That(new[] {5, 4, 3, 2, 1, 0}).IsInDescendingOrder();
            Check.That(new int?[] {5, 4, 3, 2, 1, null}).IsInDescendingOrder();
            Check.ThatCode(() => Check.That(new int?[] {null, 1, 2, 3, 5, null, 5}).IsInDescendingOrder()).IsAFaillingCheckWithMessage(
                "",
                "The checked enumerable is not in descending order, whereas it should.",
                "At #1: [null] comes before [1].", 
                "The checked enumerable:", 
                "\t[null, 1, 2, 3, 5, null, 5] (7 items)");
            Check.ThatCode(() => Check.That(new int?[] {4, 1, 2, 3, 5, null, 5}).IsInDescendingOrder()).IsAFaillingCheckWithMessage(
                "",
                "The checked enumerable is not in descending order, whereas it should.",
                "At #2: [1] comes before [2].",
                "The checked enumerable:",
                "\t[4, 1, 2, 3, 5, null, 5] (7 items)");
        }
    }
}
