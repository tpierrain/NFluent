// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContainsChainedSyntaxTests.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
//   // //   Licensed under the Apache License, Version 2.0 (the "License");
//   // //   you may not use this file except in compliance with the License.
//   // //   You may obtain a copy of the License at
//   // //       http://www.apache.org/licenses/LICENSE-2.0
//   // //   Unless required by applicable law or agreed to in writing, software
//   // //   distributed under the License is distributed on an "AS IS" BASIS,
//   // //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   // //   See the License for the specific language governing permissions and
//   // //   limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ContainsChainedSyntaxTests
    {
        private readonly string[] tresAmigos = new[] { "un", "dos", "tres" };

        [Test]
        public void IsOnlyMadeOfSucessTest()
        {
           Check.That(this.tresAmigos).Contains("dos", "un", "tres").Only();
        }

        [Test]
        public void ContainsInThatOrderSucessTest()
        {
            Check.That(this.tresAmigos).Contains("un", "dos").InThatOrder();
            Check.That(this.tresAmigos).Contains("un", "dos", "tres").InThatOrder();
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
                    })
                    .Throws<FluentCheckException>()
                    .WithMessage(Environment.NewLine+ "The checked enumerable does not follow to the expected order. Item [\"un\"] appears too late in the list, at index '2'." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t[\"un\", \"dos\", \"un\", \"tres\"]" + Environment.NewLine + "The expected value(s):" + Environment.NewLine + "\t[\"un\", \"dos\", \"tres\"]");
        }

        [Test]
        public void ContainsInThatOrderFails2()
        {
            Check.ThatCode(() =>
            {
                Check.That(this.tresAmigos).Contains("dos", "un", "tres").InThatOrder();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable does not follow to the expected order. Item [\"dos\"] appears too late in the list, at index '1'." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t[\"un\", \"dos\", \"tres\"]" + Environment.NewLine + "The expected value(s):" + Environment.NewLine + "\t[\"dos\", \"un\", \"tres\"]");
        }

        [Test]
        public void ContainsInThatOrderFails3()
        {
            Check.ThatCode(() =>
            {
                Check.That(this.tresAmigos).Contains("un", "tres", "dos").InThatOrder();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable does not follow to the expected order. Item [\"dos\"] appears too early in the list, at index '1'." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t[\"un\", \"dos\", \"tres\"]" + Environment.NewLine + "The expected value(s):" + Environment.NewLine + "\t[\"un\", \"tres\", \"dos\"]");
        }

        [Test]
        public void ContainsOnceSucceeds()
        {
            Check.That(this.tresAmigos).Contains(this.tresAmigos).Once();
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
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked enumerable has extra occurrences of the expected items. Item [\"tres\"] at position 3 is redundant." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t[\"un\", \"dos\", \"tres\", \"tres\"]" + Environment.NewLine + "The expected value(s):" + Environment.NewLine + "\t[\"un\", \"dos\", \"tres\"]");
        }
    }
}
