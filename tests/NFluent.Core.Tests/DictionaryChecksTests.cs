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

using System;

namespace NFluent.Tests
{
    using System.Collections.Generic;
    using ApiChecks;
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
        public void ContainsKeyFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.That(SimpleDico).ContainsKey("value");
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked dictionary does not contain the expected key." + Environment.NewLine + "The checked dictionary:" + Environment.NewLine + "\t[[demo, value]]" + Environment.NewLine + "Expected key:" + Environment.NewLine + "\t[\"value\"]");
        }

        [Test]
        public void NotContainsKeyWorksProperly()
        {
            Check.That(SimpleDico).Not.ContainsKey("value");
        }

        [Test]
        public void NotContainsKeyFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.That(SimpleDico).Not.ContainsKey("demo");
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked dictionary does contain the given key, whereas it must not." + Environment.NewLine + "The checked dictionary:" + Environment.NewLine + "\t[[demo, value]]" + Environment.NewLine + "Given key:" + Environment.NewLine + "\t[\"demo\"]");
        }

        [Test]
        public void ContainsValueWorks()
        {
            Check.That(SimpleDico).ContainsValue("value");
        }

        [Test]
        public void ContainsValueFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.That(SimpleDico).ContainsValue("demo");
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked dictionary does not contain the expected value." + Environment.NewLine + "The checked dictionary:" + Environment.NewLine + "\t[[demo, value]]" + Environment.NewLine + "Expected value:" + Environment.NewLine + "\t[\"demo\"]");
        }

        [Test]
        public void NotContainsValueWorksProperly()
        {
            Check.That(SimpleDico).Not.ContainsValue("demo");
        }

        [Test]
        public void NotContainsValueFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.That(SimpleDico).Not.ContainsValue("value");
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked dictionary does contain the given value, whereas it must not." + Environment.NewLine + "The checked dictionary:" + Environment.NewLine + "\t[[demo, value]]" + Environment.NewLine + "Expected value:" + Environment.NewLine + "\t[\"value\"]");
        }

        [Test]
        public void ContainsPairWorksProperly()
        {
            Check.That(SimpleDico).ContainsPair("demo", "value");
        }

        [Test]
        public void ContainsPairFailsAppropriately()
        {
            Check.ThatCode(() =>
                    Check.That(SimpleDico).ContainsPair("demo", "1")
                )
                .Throws<FluentCheckException>()
                .AndWhichMessage()
                .AsLines()
                .ContainsExactly(
                "",
                "The checked dictionary does not contain the expected key-value pair.",
                "The checked dictionary:",
                "\t[[demo, value]]",
                "Expected pair:",
                "\t[[demo, 1]]");
        }
    }
}
