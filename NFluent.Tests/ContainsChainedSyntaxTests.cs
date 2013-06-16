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

namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ContainsChainedSyntaxTests
    {
        private readonly string[] tresAmigos = new[] { "un", "dos", "tres" };

        [Test]
        public void ContainsOnlySucessTest()
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable does not follow to the expected order. Item [\"un\"] appears too late in the list, at index '2'.\nThe checked enumerable:\n\t[\"un\", \"dos\", \"un\", \"tres\"]\nThe expected enumerable:\n\t[\"un\", \"dos\", \"tres\"]")]
        public void ContainsInThatOrderFails()
        {
            var tresAmigosAndMore = new[] { "un", "dos", "un", "tres" };

            Check.That(tresAmigosAndMore).Contains(this.tresAmigos).InThatOrder();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable does not follow to the expected order. Item [\"dos\"] appears too late in the list, at index '1'.\nThe checked enumerable:\n\t[\"un\", \"dos\", \"tres\"]\nThe expected enumerable:\n\t[\"dos\", \"un\", \"tres\"]")]
        public void ContainsInThatOrderFails2()
        {
            Check.That(this.tresAmigos).Contains("dos", "un", "tres").InThatOrder();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable does not follow to the expected order. Item [\"dos\"] appears too early in the list, at index '1'.\nThe checked enumerable:\n\t[\"un\", \"dos\", \"tres\"]\nThe expected enumerable:\n\t[\"un\", \"tres\", \"dos\"]")]
        public void ContainsInThatOrderFails3()
        {
            Check.That(this.tresAmigos).Contains("un", "tres", "dos").InThatOrder();
        }

        [Test]
        public void ContainsOnceSucceeds()
        {
            Check.That(this.tresAmigos).Contains(this.tresAmigos).Once();
        }

        [Test]
        public void ContainsOnceSucceedsWithMultipleOccurences()
        {
            var tresAmigosAndMore = new[] { "un", "dos", "tres", "tres" };
            Check.That(tresAmigosAndMore).Contains(tresAmigosAndMore).Once();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked enumerable has extra occurences of the expected items. Item [\"tres\"] at position 3 is redundant.\nThe checked enumerable:\n\t[\"un\", \"dos\", \"tres\", \"tres\"]\nThe expected enumerable:\n\t[\"un\", \"dos\", \"tres\"]")]
        public void ContainsOnceFails()
        {
            var tresAmigosAndMore = new[] { "un", "dos", "tres", "tres" };
            Check.That(tresAmigosAndMore).Contains(this.tresAmigos).Once();
        }
    }
}
