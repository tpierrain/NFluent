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
    using System.Linq;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class EnumerableCheckShould
    {
        [Test]
        public void
            FindDuplicates()
        {
            var array = new[] {1, 2, 3, 4};
            Check.That(array).ContainsNoDuplicateItem();

            Check.ThatCode(() => Check.That(new[] {1, 1, 2, 3}).ContainsNoDuplicateItem()).
                IsAFailingCheckWithMessage("", 
                    "The checked enumerable contains a duplicate item at position 1: [1].", 
                    "The checked enumerable:", 
                    "\t{1, 1, 2, 3} (4 items)");
        }

        [Test]
        public void
            VerifyDuplicatesAreHere()
        {
            var array = new[] {1, 1, 2, 3};
            Check.That(array).Not.ContainsNoDuplicateItem();

            Check.ThatCode(() => Check.That(new[] {1, 2, 3, 4}).Not.ContainsNoDuplicateItem()).
                IsAFailingCheckWithMessage("",
                    "The checked enumerable should contain duplicates.",
                    "The checked enumerable:",
                    "\t{1, 2, 3, 4} (4 items)");
        }

        [Test]
        public void
            CheckForPresenceOfNull()
        {
            var array = new [] {"test", "another"};
            Check.That(array).ContainsNoNull();

            Check.ThatCode(() => Check.That(new [] {"test", "another", null}).ContainsNoNull()).
                IsAFailingCheckWithMessage("",
                    "The checked enumerable contains a null item at position 2.",
                    "The checked enumerable:",
                    "\t{\"test\", \"another\", null} (3 items)");
        }

        [Test]
        public void
            CheckForAbsenceOfNull()
        {
            var array = new [] {"test", "another", null};
            Check.That(array).Not.ContainsNoNull();

            Check.ThatCode(() => Check.That(new [] {"test", "another"}).Not.ContainsNoNull()).
                IsAFailingCheckWithMessage("",
                    "The checked enumerable should contain at least one null entry.",
                    "The checked enumerable:",
                    "\t{\"test\", \"another\"} (2 items)");
        }

        [Test]
        public void
            CheckForPresenceOfWrongType()
        {
            var array = new [] {"test", "another"};
            Check.That(array).ContainsOnlyInstanceOfType(typeof(string));

            Check.ThatCode(() => Check.That(new List<object>() {"test", "another", 4}).ContainsOnlyInstanceOfType(typeof(string))).
                IsAFailingCheckWithMessage("",
                    "The checked enumerable contains an entry of a type different from String at position 2.",
                    "The checked enumerable:",
                    "\t{\"test\", \"another\", 4} (3 items)");
        }
        
        [Test]
        public void
            CheckForAbsenceofWrongType()
        {
            var array = new List<object> {"test", "another", 4};
            Check.That(array).Not.ContainsOnlyInstanceOfType(typeof(string));

            Check.ThatCode(() => Check.That(new List<object> {"test", "another"}).Not.ContainsOnlyInstanceOfType(typeof(string))).
                IsAFailingCheckWithMessage("",
                    "The checked enumerable should contain at least one entry of a type different from String.",
                    "The checked enumerable:",
                    "\t{\"test\", \"another\"} (2 items)");
        }

        [Test]
        public void
            CheckForEquivalency()
        {
            var array = new[] {1, 2, 3};

            Check.That(array).IsEquivalentTo(3, 2, 1);
            Check.That<IEnumerable<int>>(null).IsEquivalentTo((IEnumerable<int>) null);

            Check.ThatCode(() => Check.That(array).IsEquivalentTo(1, 2, 4)).IsAFailingCheck();

            var otherArray = new List<int>(new []{3, 2, 1});
            Check.That(array).IsEquivalentTo(otherArray);
        }

        [Test]
        public void SupportIEnumerableObject()
        {
            var collection = new List<object> {1, 2, 3, 4};
            Check.That(collection).IsEqualTo(new[] {1, 2, 3, 4});
            Check.That(collection).ContainsExactly(new[] {1, 2, 3, 4});
            Check.That(collection).Contains(new[] {1, 2});
            Check.That(collection).IsEquivalentTo(new[] {4, 3, 1, 2});
        }

        [Test]
        public void CheckIsEquivalentOnPlainEnumerable()
        {
            var array = new[] {1, 2, 3};
            Check.That((IEnumerable)array).IsEquivalentTo(3, 2, 1);
            Check.ThatCode(()=>
            Check.That((IEnumerable)array).IsEquivalentTo(3, 2)).
                IsAFailingCheckWithMessage("", 
                    "The checked enumerable is not equivalent to the expected value(s).", 
                    "actual.Dimension(0) = 3 instead of 2.", 
                    "The checked enumerable:", 
                    "\t{1, 2, 3} (3 items)", 
                    "The expected value(s):", 
                    "\t{3, 2} (2 items)");
        }

        [Test]
        public void CheckIsEquivalentOnNestedEnumerable()
        {
            var array = new[] {new []{1, 2, 3}, new []{4, 5, 6}};
            var altArray = new[] {new []{4, 5, 6}, new []{1, 2, 3}};
            Check.That((IEnumerable)array).IsEquivalentTo(altArray);
            Check.That((IEnumerable)array).IsEquivalentTo( new []{4, 5, 6}, new []{1, 3, 2});
        }
        
        [Test]
        public void CheckIsEquivalentOnNestedEquivalentEnumerable()
        {
            var altArray = new List<List<int>> {new List<int>{5, 4, 6}, new List<int>{3, 2, 1}};
            var array = new List<List<int>> {new List<int>{1, 2, 3}, new List<int>{4, 5, 6}};
            Check.That((IEnumerable)array).IsEquivalentTo(altArray);
            Check.ThatCode(()=>
                    Check.That((IEnumerable)array).IsEquivalentTo( new List<List<int>> {new List<int>{5, 4, 6}, new List<int>{3, 3, 1}})).
                IsAFailingCheckWithMessage(	"", 
                    "The checked enumerable is not equivalent to the expected value(s). 6 differences found!", 
                    "actual[0][0] = 1 instead of 5.", 
                    "actual[0][1] = 2 instead of 4.", 
                    "actual[0][2] = 3 instead of 6.", 
                    "actual[1][0] = 4 instead of 3.", 
                    "actual[1][1] = 5 instead of 3.", 
                    "... (1 differences omitted)", 
                    "The checked enumerable:", 
                    "\t{{1, 2, 3}, {4, 5, 6}} (2 items)", 
                    "The expected value(s):", 
                    "\t{{5, 4, 6}, {3, 3, 1}} (2 items)");
        }
        
        [Test]
        public void
            CheckForEquivalencyFailsWithProperMessage()
        {
            var array = new[] {1, 2, 3};

            Check.ThatCode(() => Check.That(array).IsEquivalentTo(1, 2, 4)).IsAFailingCheckWithMessage("", 
                "The checked enumerable does contain [3] whereas it should not.", 
                "The checked enumerable:",
                "\t{1, 2, 3} (3 items)", 
                "The expected value(s):", 
                "\t{1, 2, 4} (3 items)");

            Check.ThatCode(() => Check.That(array).IsEquivalentTo(1, 2, 3, 4)).IsAFailingCheckWithMessage("", 
                "The checked enumerable is missing: [4].", 
                "The checked enumerable:",
                "\t{1, 2, 3} (3 items)", 
                "The expected value(s):", 
                "\t{1, 2, 3, 4} (4 items)");

            Check.ThatCode(() => Check.That(array).IsEquivalentTo(1, 2, 3, 4, 5)).IsAFailingCheckWithMessage("", 
                "The checked enumerable is missing 2 items: {4, 5}.", 
                "The checked enumerable:",
                "\t{1, 2, 3} (3 items)", 
                "The expected value(s):", 
                "\t{1, 2, 3, 4, 5} (5 items)");

            Check.ThatCode(() => Check.That(array).IsEquivalentTo(1, 2)).IsAFailingCheckWithMessage("", 
                "The checked enumerable does contain [3] whereas it should not.", 
                "The checked enumerable:",
                "\t{1, 2, 3} (3 items)", 
                "The expected value(s):", 
                "\t{1, 2} (2 items)");

            Check.ThatCode(() => Check.That<IEnumerable<int>>(null).IsEquivalentTo(1)).IsAFailingCheckWithMessage("",
                "The checked enumerable is null whereas it should not.", 
                "The checked enumerable:", 
                "\t[null]", 
                "The expected value(s):", 
                "\t{1} (1 item)");

            Check.ThatCode(() => Check.That<IEnumerable<int>>(new[]{1}).IsEquivalentTo(null)).IsAFailingCheckWithMessage("",
                "The checked enumerable must be null.", 
                "The checked enumerable:", 
                "\t{1} (1 item)", 
                "The expected value(s):", 
                "\t[null]");
        }
        internal static IEnumerable<char> GetData()
        {
            yield return 't';
            yield return 'e';
            yield return 's';
            yield return 't';
        }

        [Test]
        // GH #299 issue with char enumerations
        public void GenerateProperErrorWithCharEnumerations()
        {
            Check.ThatCode(()=> Check.That(GetData()).ContainsExactly("testt")).IsAFailingCheckWithMessage("", 
                "The checked enumerable does not contain exactly the expected value(s). Elements are missing starting at index #4.", 
                "The checked enumerable:", 
                "\t{'t', 'e', 's', 't'} (4 items)", 
                "The expected value(s):", 
                "\t{'t', 'e', 's', 't', 't'} (5 items)");
        }

        [Test]
        public void FailsWithProperMessageWhenNegated()
        {
            var array = new[] {1, 2, 3};
            Check.ThatCode(() => Check.That(array).Not.IsEquivalentTo(1, 2, 3)).IsAFailingCheckWithMessage("", 
                "The checked enumerable is equivalent to the expected value(s) whereas it should not.", 
                "The checked enumerable:",
                "\t{1, 2, 3} (3 items)", 
                "The expected value(s): different from", 
                "\t{1, 2, 3} (3 items)");
        }

        [Test]
        public void
            VerifyAscendingOrder()
        {
            Check.That(new int?[] {0, 1, 2, 3, 5}).IsInAscendingOrder();
            Check.That(new int?[] {null, 1, 2, 3, 5}).IsInAscendingOrder();
            Check.ThatCode(() => Check.That(new int?[] {null, 1, 2, 3, 5, null, 5}).IsInAscendingOrder()).IsAFailingCheckWithMessage(
                "",
                "The checked enumerable is not in ascending order, whereas it should.",
                "At #5: [5] comes after [null].", 
                "The checked enumerable:", 
                "\t{null, 1, 2, 3, 5, null, 5} (7 items)");
            Check.ThatCode(() => Check.That(new int?[] {4, 1, 2, 3, 5, null, 5}).IsInAscendingOrder()).IsAFailingCheckWithMessage(
                "",
                "The checked enumerable is not in ascending order, whereas it should.",
                "At #1: [4] comes after [1].", "The checked enumerable:",
                "\t{4, 1, 2, 3, 5, null, 5} (7 items)");
        }

        [Test]
        public void FailsOnDifferentQueries()
        {
            var sut = Enumerable.Range(0,4);
            var expected = Enumerable.Range(0, 5);
            Check.ThatCode(() => { Check.That(sut).IsEqualTo(expected); })
                .IsAFailingCheckWithMessage(
                    "", 
                    "The checked enumerable is different from the expected one.", 
                    "actual[4] does not exist. Expected 4.",
                    "The checked enumerable:", 
                    "\t{0, 1, 2, 3} (4 items)", 
                    "The expected enumerable:", 
                    "\t{0, 1, 2, 3, 4} (5 items)");
        }

        class ComparerWithNullAtTheEnd: IComparer{
            public int Compare(object x, object y)
            {
                if (x == null)
                {
                    return 1;
                }

                if (y == null)
                {
                    return -1;
                }
                var comparable = x as IComparable;
                return comparable?.CompareTo(y) ?? -1;           
            }
        }

        [Test]
        public void AscendingSupportCustomComparer()
        {
            Check.That(new [] {0, 1, 2, 3, 4, 5}).IsInAscendingOrder(new ComparerWithNullAtTheEnd());
            Check.That(new [] {5, 4, 3, 2, 1, 0}).IsInAscendingOrder(new HardComparer(-1));
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
            Check.That(new[] {5, 4, 3, 2, 1, 1}).IsInDescendingOrder();
            Check.That(new int?[] {5, 4, 3, 2, 1, null}).IsInDescendingOrder();
            Check.ThatCode(() => Check.That(new int?[] {null, 1, 2, 3, 5, null, 5}).IsInDescendingOrder()).IsAFailingCheckWithMessage(
                "",
                "The checked enumerable is not in descending order, whereas it should.",
                "At #1: [null] comes before [1].", 
                "The checked enumerable:", 
                "\t{null, 1, 2, 3, 5, null, 5} (7 items)");
            Check.ThatCode(() => Check.That(new int?[] {4, 1, 2, 3, 5, null, 5}).IsInDescendingOrder()).IsAFailingCheckWithMessage(
                "",
                "The checked enumerable is not in descending order, whereas it should.",
                "At #2: [1] comes before [2].",
                "The checked enumerable:",
                "\t{4, 1, 2, 3, 5, null, 5} (7 items)");
        }
    }
}
