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
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class ContainsChainedSyntaxTests
    {
        [Test]
        public void ContainsOnlySucessTest()
        {
           var tresAmigos = new string[] { "un", "dos", "tres" };
           Check.That(tresAmigos).Contains("dos", "un", "tres").Only();
        }

        [Test]
        public void ContainsInThatOrderSucessTest()
        {
            var tresAmigos = new[] { "un", "dos", "tres" };

            Check.That(tresAmigos).Contains("un", "dos").InThatOrder();
            Check.That(tresAmigos).Contains("un", "dos", "tres").InThatOrder();
            Check.That(tresAmigos).Contains("un", "un", "dos", "tres").InThatOrder();
            Check.That(tresAmigos).Contains("dos", "tres").InThatOrder();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException))]
        public void ContainsInThatOrderFails()
        {
            var tresAmigos = new[] { "un", "dos", "un", "tres" };
            Check.That(tresAmigos).Contains("un", "dos", "tres").InThatOrder();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException))]
        public void ContainsInThatOrderFails2()
        {
            var tresAmigos = new[] { "un", "dos", "tres" };
            Check.That(tresAmigos).Contains("dos", "un", "tres").InThatOrder();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException))]
        public void ContainsInThatOrderFails3()
        {
            var tresAmigos = new[] { "un", "dos", "tres" };
            Check.That(tresAmigos).Contains("un", "tres", "dos").InThatOrder();
        }
    }
}
