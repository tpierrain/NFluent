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
    using System.Collections.Generic;
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

            Check.That<IEnumerable<int>>(null).IsEquivalentTo(null);

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

            Check.ThatCode(() => Check.That<IEnumerable<int>>(null).IsEquivalentTo(new[] {1})).IsAFaillingCheck();
        }
    }
}
