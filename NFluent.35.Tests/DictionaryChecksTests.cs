// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DictionaryChecksTests.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
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
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class DictionaryChecksTests
    {
        private static readonly Dictionary<string, string> SimpleDico;

        static DictionaryChecksTests()
        {
            SimpleDico = new Dictionary<string, string>();
            SimpleDico["demo"] = "value";
        }

        [Test]
        public void ContainsKeyWorks()
        {
            Check.That(SimpleDico).ContainsKey("demo");
        }

        [Test]
        public void InheritedChecks()
        {
            Check.That(SimpleDico).Equals(SimpleDico);

            Check.That(SimpleDico).HasSize(1);

            Check.That(SimpleDico).IsInstanceOf<Dictionary<string, string>>();

            Check.That(SimpleDico).IsNotEqualTo(4);

            Check.That(SimpleDico).IsSameReferenceThan(SimpleDico);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked dictionary does not contain the expected key.\nThe checked dictionary:\n\t[[demo, value]]\nExpected key:\n\t[\"value\"]")]
        public void ContainsKeyFailsProperly()
        {
            Check.That(SimpleDico).ContainsKey("value");
        }

        [Test]
        public void NotContainsKeyWorksProperly()
        {
            Check.That(SimpleDico).Not.ContainsKey("value");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked dictionary does contain the given key, whereas it must not.\nThe checked dictionary:\n\t[[demo, value]]\nGiven key:\n\t[\"demo\"]")]
        public void NotContainsKeyFailsProperly()
        {
            Check.That(SimpleDico).Not.ContainsKey("demo");
        }

        [Test]
        public void ContainsValueWorks()
        {
            Check.That(SimpleDico).ContainsValue("value");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked dictionary does not contain the expected value.\nThe checked dictionary:\n\t[[demo, value]]\nExpected value:\n\t[\"demo\"]")]
        public void ContainsValueFailsProperly()
        {
            Check.That(SimpleDico).ContainsValue("demo");
        }

        [Test]
        public void NotContainsValueWorksProperly()
        {
            Check.That(SimpleDico).Not.ContainsValue("demo");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked dictionary does contain the given value, whereas it must not.\nThe checked dictionary:\n\t[[demo, value]]\nExpected value:\n\t[\"value\"]")]
        public void NotContainsValueFailsProperly()
        {
            Check.That(SimpleDico).Not.ContainsValue("value");
        }
    }
}
