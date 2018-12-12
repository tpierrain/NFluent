 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="DictionaryChecksShould.cs" company="">
 //   Copyright 2013-2018 Cyrille DUPUYDAUBY
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
#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40
    using System.Collections.ObjectModel;
#endif
    using ApiChecks;
    using NFluent.Helpers;
    using NUnit.Framework;
    using SutClasses;

    [TestFixture]
    public class DictionaryChecksShould
    {
        private static readonly Dictionary<string, string> SimpleDico;

        static DictionaryChecksShould()
        {
            SimpleDico = new Dictionary<string, string> {["demo"] = "value", ["other"] = "test"};
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

            Check.That(SimpleDico).HasSize(2);

            Check.That(SimpleDico).IsInstanceOf<Dictionary<string, string>>();

            Check.That(SimpleDico).IsNotEqualTo(4);

            Check.That(SimpleDico).IsSameReferenceAs(SimpleDico);
        }

        [Test]
        public void ContainsKeyFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.That(SimpleDico).ContainsKey("value");
            })
                .IsAFaillingCheckWithMessage("",
                    "The checked dictionary does not contain the expected key.",
                    "The checked dictionary:",
                    "\t[[demo, value], [other, test]] (2 items)",
                    "Expected key:",
                    "\t[\"value\"]");
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
            .IsAFaillingCheckWithMessage("",
                    "The checked dictionary does contain the given key, whereas it must not.",
                    "The checked dictionary:",
                    "\t[[demo, value], [other, test]] (2 items)",
                    "Forbidden key:",
                    "\t[\"demo\"]");
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
                .IsAFaillingCheckWithMessage("",
                    "The checked dictionary does not contain the expected value.",
                    "The checked dictionary:",
                    "\t[[demo, value], [other, test]] (2 items)",
                    "Expected value:",
                    "\t[\"demo\"]");
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
                .IsAFaillingCheckWithMessage("",
                    "The checked dictionary does contain the given value, whereas it must not.",
                    "The checked dictionary:",
                    "\t[[demo, value], [other, test]] (2 items)",
                    "Forbidden value:",
                    "\t[\"value\"]");
        }

        [Test]
        public void ContainsPairWorksProperly()
        {
            Check.That(SimpleDico).ContainsPair("demo", "value");
        }

        [Test]
        public void NotContainsPairFailsOnForbidenPair()
        {
            Check.ThatCode(() =>
                    Check.That(SimpleDico).Not.ContainsPair("demo", "value"))
                .IsAFaillingCheckWithMessage("",
                    "The checked dictionary does contain the given key-value pair, whereas it must not.",
                    "The checked dictionary:",
                    "\t[[demo, value], [other, test]] (2 items)",
                    "Forbidden pair:", 
                    "\t[[demo, value]]");
        }

        [Test]
        public void
            WorkWithEnumerationOfKeyValuePair()
        {
            var customDic = new List<KeyValuePair<string, int>> {
                new KeyValuePair<string, int>("otherKey", 15) ,
                new KeyValuePair<string, int>("key", 12)
                };
            Check.That(customDic).ContainsKey("key");
            Check.That(customDic).ContainsValue(12);
            Check.That(customDic).ContainsPair("key", 12);
            Check.ThatCode(() => Check.That(customDic).ContainsKey("missing")).IsAFaillingCheckWithMessage("",
                "The checked enumerable does not contain the expected key.",
                "The checked enumerable:",
                "\t[[otherKey, 15], [key, 12]] (2 items)",
                "Expected key:",
                "\t[\"missing\"]");
            // test with empty array
            Check.ThatCode(()=>
            Check.That(new List<KeyValuePair<string, int>>()).ContainsPair("key", 12)).IsAFaillingCheckWithMessage("",
                "The checked enumerable does not contain the expected key-value pair. The given key was not found.",
                "The checked enumerable:",
                "\t[] (0 item)",
                "Expected pair:",
                "\t[[key, 12]]");
        }

        [Test]
        public void
            WorkWithEmptyEnumerationOfKeyValuePair()
        {
            var customDic = new List<KeyValuePair<string, int>>();
            Check.ThatCode(() => Check.That(customDic).ContainsKey("missing")).IsAFaillingCheckWithMessage("",
                "The checked enumerable does not contain the expected key.",
                "The checked enumerable:",
                "\t[] (0 item)",
                "Expected key:",
                "\t[\"missing\"]");
        }

#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40
        // GH #222
        [Test]
        public void
            WorkWithReadonlyDictionary()
        {
            IReadOnlyDictionary<string, string> roDico = new RoDico(SimpleDico);
            Check.That(roDico).ContainsKey("demo");
            Check.That(roDico).ContainsPair("demo", "value");
            Check.ThatCode(() => Check.That(roDico).ContainsKey("missing")).IsAFaillingCheckWithMessage("",
            "The checked enumerable does not contain the expected key.",
                "The checked enumerable:",
                "\t[[demo, value], [other, test]] (2 items)",
           "Expected key:",
                "\t[\"missing\"]");
        }
#endif

#if !PORTABLE
        [Test]
        public void CompatibleWithHashtable()
        {
            var basic = new Hashtable {["foo"] = "bar"};

            Check.That(basic).ContainsKey("foo");
            Check.That(basic).ContainsValue("bar");
            Check.That(basic).ContainsPair("foo", "bar");

            Check.ThatCode(() => { Check.That(basic).ContainsKey("bar"); }).IsAFaillingCheck();
            Check.ThatCode(() => { Check.That(basic).ContainsValue("foo"); }).IsAFaillingCheck();
            Check.ThatCode(() => { Check.That(basic).ContainsPair("bar", "foo"); }).IsAFaillingCheck();
            Check.ThatCode(() => { Check.That(basic).ContainsPair("foo", "foo"); }).IsAFaillingCheck();
        }

        [Test]
        public void FailsOnHastable()
        {
            var basic = new Hashtable {["foo"] = "bar"};
            Check.ThatCode(()=>
            Check.That(basic).ContainsKey("bar")).IsAFaillingCheckWithMessage("",
                "The checked dictionary does not contain the expected key.",
                "The checked dictionary:",
                "\t[[foo, bar]] (1 item)",
                "Expected key:",
                "\t[\"bar\"]");
            Check.ThatCode(()=>
            Check.That(basic).ContainsValue("foo")).IsAFaillingCheckWithMessage("",
                "The checked dictionary does not contain the expected value.",
                "The checked dictionary:", 
                "\t[[foo, bar]] (1 item)", 
                "Expected value:", 
                "\t[\"foo\"]");
            Check.ThatCode(()=>
            Check.That(basic).ContainsPair("bar", "foo")).IsAFaillingCheckWithMessage("",
                "The checked dictionary does not contain the expected key-value pair. The given key was not found.",
                "The checked dictionary:", 
                "\t[[foo, bar]] (1 item)", 
                "Expected pair:", 
                "\t[[bar, foo]]");
            Check.ThatCode(()=>
                Check.That(basic).ContainsPair("foo", "foo")).IsAFaillingCheckWithMessage("",
                "The checked dictionary does not contain the expected value for the given key.",
                "The checked dictionary:", 
                "\t[[foo, bar]] (1 item)", 
                "Expected pair:", 
                "\t[[foo, foo]]");
        }

        [Test]
        public void ContainsKeyFailsWhenNegated()
        {
            var basic = new Hashtable {["foo"] = "bar"};
            Check.ThatCode(()=>
            Check.That(basic).Not.ContainsKey("foo")).IsAFaillingCheckWithMessage("",
                "The checked dictionary does contain the given key, whereas it must not.",
                "The checked dictionary:",
                "\t[[foo, bar]] (1 item)", 
                "Forbidden key:",
                "\t[\"foo\"]");
            Check.ThatCode(()=>
            Check.That(basic).Not.ContainsValue("bar")).IsAFaillingCheckWithMessage("",
                "The checked dictionary does contain the given value, whereas it must not.",
                "The checked dictionary:", 
                "\t[[foo, bar]] (1 item)", 
                "Forbidden value:", 
                "\t[\"bar\"]");
            Check.ThatCode(()=>
                Check.That(basic).Not.ContainsPair("foo", "bar")).IsAFaillingCheckWithMessage("",
                "The checked dictionary does contain the given key-value pair, whereas it must not.",
                "The checked dictionary:", 
                "\t[[foo, bar]] (1 item)", 
                "Forbidden pair:", 
                "\t[[foo, bar]]");       }
#endif
        [Test]
        public void ContainsPairFailsAppropriately()
        {
            Check.ThatCode(() =>
                    Check.That(SimpleDico).ContainsPair("demo", "1")
                )
                .IsAFaillingCheckWithMessage(
                "",
                "The checked dictionary does not contain the expected value for the given key.",
                "The checked dictionary:",
                "\t[[demo, value], [other, test]] (2 items)",
                "Expected pair:",
                "\t[[demo, 1]]");

            Check.ThatCode(() =>
                    Check.That(SimpleDico).ContainsPair("demo2", "1")
                )
                .IsAFaillingCheckWithMessage(
                "",
                "The checked dictionary does not contain the expected key-value pair. The given key was not found.",
                "The checked dictionary:",
                "\t[[demo, value], [other, test]] (2 items)",
                "Expected pair:",
                "\t[[demo2, 1]]");
        }
    }
}
