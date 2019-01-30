// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContainsChainedSyntaxTests.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
//   Licensed under the Apache License, Version 2.0 (the "License");
//      you may not use this file except in compliance with the License.
//      You may obtain a copy of the License at
//         http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NFluent.Tests
{
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class ContainsChainedSyntaxTests
    {
        private readonly string[] tresAmigos = new[] { "un", "dos", "tres" };

        [Test]
        public void IsOnlyMadeOfSuccessTest()
        {
           Check.That(this.tresAmigos).Contains("dos", "un", "tres").Only();
        }

        [Test]
        public void ContainsInThatOrderSuccessTest()
        {
            Check.That(this.tresAmigos).Contains("un", "dos", "tres").InThatOrder();
            Check.That(this.tresAmigos).Contains("un", "dos").InThatOrder();
            Check.That(this.tresAmigos).Contains("un", "un", "dos", "tres").InThatOrder();
            Check.That(this.tresAmigos).Contains("dos", "tres").InThatOrder();
        }

        [Test]
        public void ContainsInThatOrderFails()
        {
            var tresAmigosAndMore = new[] { "un", "dos", "un", "tres" };

            Check.ThatCode(() =>
                    {
                        Check.That(tresAmigosAndMore).Contains(this.tresAmigos).InThatOrder();
                    }).
                    IsAFailingCheckWithMessage("",
                    "The checked enumerable does not follow to the expected order. Item [\"un\"] appears too late in the list, at index '2'.",
                    "The checked enumerable:",
                    "\t{\"un\", \"dos\", \"un\", \"tres\"} (4 items)",
                    "The expected enumerable: in that order",
                    "\t{\"un\", \"dos\", \"tres\"}");
        }

       [Test]
        public void ContainsInThatOrderFails2()
        {
            Check.ThatCode(() =>
            {
                Check.That(this.tresAmigos).Contains("dos", "un", "tres").InThatOrder();
            })
            .IsAFailingCheckWithMessage("",
                    "The checked enumerable does not follow to the expected order. Item [\"dos\"] appears too late in the list, at index '1'.",
                    "The checked enumerable:",
                    "\t{\"un\", \"dos\", \"tres\"} (3 items)",
                    "The expected enumerable: in that order",
                    "\t{\"dos\", \"un\", \"tres\"}");
        }

        [Test]
        public void ContainsInThatOrderFails3()
        {
            Check.ThatCode(() =>
            {
                Check.That(this.tresAmigos).Contains("un", "tres", "dos").InThatOrder();
            })
            .IsAFailingCheckWithMessage(
                    "",
                    "The checked enumerable does not follow to the expected order. Item [\"dos\"] appears too early in the list, at index '1'.",
                    "The checked enumerable:",
                    "\t{\"un\", \"dos\", \"tres\"} (3 items)",
                    "The expected enumerable: in that order",
                    "\t{\"un\", \"tres\", \"dos\"}");
        }

        [Test]
        public void ContainsOnceSucceeds()
        {
            Check.That(this.tresAmigos).Contains(this.tresAmigos).Once();
        }

        [Test]
        public void ContainsOnceCantBeNegated()
        {
            Check.ThatCode(() => Check.That(this.tresAmigos).Not.Contains("toto").Once())
                .Throws<InvalidOperationException>().WithMessage("Once can't be used when negated");
        }

        [Test]
        public void ContainsInThatOrderCantBeNegated()
        {
            Check.ThatCode(() => Check.That(this.tresAmigos).Not.Contains("toto").InThatOrder())
                .Throws<InvalidOperationException>().WithMessage("InThatOrder can't be used when negated");
        }

        [Test]
        public void ContainsOnceSucceedsWithMultipleOccurrences()
        {
            var tresAmigosAndMore = new[] { "un", "dos", "tres", "tres" };
            Check.That(tresAmigosAndMore).Contains(tresAmigosAndMore).Once();
        }

        [Test]
        public void ContainsOnceSucceedsWithMissingEntry()
        {
            var tresAmigosAndMore = new[] { "un", "dos", "tres", "four" };
            Check.That(tresAmigosAndMore).Contains(tresAmigos).Once();
        }

        [Test]
        public void ContainsOnceFails()
        {
            var tresAmigosAndMore = new[] { "un", "dos", "tres", "tres" };
            Check.ThatCode(() =>
            {
                Check.That(tresAmigosAndMore).Contains(this.tresAmigos).Once();
            })
            .IsAFailingCheckWithMessage("",
                    "The checked enumerable has extra occurrences of the expected items. Item [\"tres\"] at position 3 is redundant.",
                    "The checked enumerable:",
                    "\t{\"un\", \"dos\", \"tres\", \"tres\"} (4 items)",
                    "The expected enumerable: once of",
                    "\t{\"un\", \"dos\", \"tres\"}");
        }
    }
}
