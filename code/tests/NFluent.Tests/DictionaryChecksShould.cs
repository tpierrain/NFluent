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
                .IsAFailingCheckWithMessage("",
                    "The checked dictionary does not contain the expected key.",
                    "The checked dictionary:",
                    "\t{[demo, value], [other, test]} (2 items)",
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
            .IsAFailingCheckWithMessage("",
                    "The checked dictionary does contain the given key, whereas it must not.",
                    "The checked dictionary:",
                    "\t{[demo, value], [other, test]} (2 items)",
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
                .IsAFailingCheckWithMessage("",
                    "The checked dictionary does not contain the expected value.",
                    "The checked dictionary:",
                    "\t{[demo, value], [other, test]} (2 items)",
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
                .IsAFailingCheckWithMessage("",
                    "The checked dictionary does contain the given value, whereas it must not.",
                    "The checked dictionary:",
                    "\t{[demo, value], [other, test]} (2 items)",
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
                .IsAFailingCheckWithMessage("",
                    "The checked dictionary does contain the given key-value pair, whereas it must not.",
                    "The checked dictionary:",
                    "\t{[demo, value], [other, test]} (2 items)",
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
            Check.ThatCode(() => Check.That(customDic).ContainsKey("missing")).IsAFailingCheckWithMessage("",
                "The checked enumerable does not contain the expected key.",
                "The checked enumerable:",
                "\t{[otherKey, 15], [key, 12]} (2 items)",
                "Expected key:",
                "\t[\"missing\"]");
            // test with empty array
            Check.ThatCode(()=>
            Check.That(new List<KeyValuePair<string, int>>()).ContainsPair("key", 12)).IsAFailingCheckWithMessage("",
                "The checked enumerable does not contain the expected key-value pair. The given key was not found.",
                "The checked enumerable:",
                "\t{} (0 item)",
                "Expected pair:",
                "\t[[key, 12]]");
        }

        [Test]
        public void
            WorkWithEmptyEnumerationOfKeyValuePair()
        {
            var customDic = new List<KeyValuePair<string, int>>();
            Check.ThatCode(() => Check.That(customDic).ContainsKey("missing")).IsAFailingCheckWithMessage("",
                "The checked enumerable does not contain the expected key.",
                "The checked enumerable:",
                "\t{} (0 item)",
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
            Check.ThatCode(() => Check.That(roDico).ContainsKey("missing")).IsAFailingCheckWithMessage("",
            "The checked enumerable does not contain the expected key.",
                "The checked enumerable:",
                "\t{[demo, value], [other, test]} (2 items)",
           "Expected key:",
                "\t[\"missing\"]");
        }
#endif

        [Test]
        public void CompatibleWithHashtable()
        {
            var basic = new Hashtable {["foo"] = "bar"};

            Check.That(basic).ContainsKey("foo");
            Check.That(basic).ContainsValue("bar");
            Check.That(basic).ContainsPair("foo", "bar");

            Check.ThatCode(() => { Check.That(basic).ContainsKey("bar"); }).IsAFailingCheck();
            Check.ThatCode(() => { Check.That(basic).ContainsValue("foo"); }).IsAFailingCheck();
            Check.ThatCode(() => { Check.That(basic).ContainsPair("bar", "foo"); }).IsAFailingCheck();
            Check.ThatCode(() => { Check.That(basic).ContainsPair("foo", "foo"); }).IsAFailingCheck();
        }

        [Test]
        public void FailsOnHastable()
        {
            var basic = new Hashtable {["foo"] = "bar"};
            Check.ThatCode(()=>
            Check.That(basic).ContainsKey("bar")).IsAFailingCheckWithMessage("",
                "The checked dictionary does not contain the expected key.",
                "The checked dictionary:",
                "\t{[foo, bar]} (1 item)",
                "Expected key:",
                "\t[\"bar\"]");
            Check.ThatCode(()=>
            Check.That(basic).ContainsValue("foo")).IsAFailingCheckWithMessage("",
                "The checked dictionary does not contain the expected value.",
                "The checked dictionary:", 
                "\t{[foo, bar]} (1 item)", 
                "Expected value:", 
                "\t[\"foo\"]");
            Check.ThatCode(()=>
            Check.That(basic).ContainsPair("bar", "foo")).IsAFailingCheckWithMessage("",
                "The checked dictionary does not contain the expected key-value pair. The given key was not found.",
                "The checked dictionary:", 
                "\t{[foo, bar]} (1 item)", 
                "Expected pair:", 
                "\t[[bar, foo]]");
            Check.ThatCode(()=>
                Check.That(basic).ContainsPair("foo", "foo")).IsAFailingCheckWithMessage("",
                "The checked dictionary does not contain the expected value for the given key.",
                "The checked dictionary:", 
                "\t{[foo, bar]} (1 item)", 
                "Expected pair:", 
                "\t[[foo, foo]]");
        }

        [Test]
        public void ContainsKeyFailsWhenNegated()
        {
            var basic = new Hashtable {["foo"] = "bar"};
            Check.ThatCode(()=>
            Check.That(basic).Not.ContainsKey("foo")).IsAFailingCheckWithMessage("",
                "The checked dictionary does contain the given key, whereas it must not.",
                "The checked dictionary:",
                "\t{[foo, bar]} (1 item)", 
                "Forbidden key:",
                "\t[\"foo\"]");
            Check.ThatCode(()=>
            Check.That(basic).Not.ContainsValue("bar")).IsAFailingCheckWithMessage("",
                "The checked dictionary does contain the given value, whereas it must not.",
                "The checked dictionary:", 
                "\t{[foo, bar]} (1 item)", 
                "Forbidden value:", 
                "\t[\"bar\"]");
            Check.ThatCode(()=>
                Check.That(basic).Not.ContainsPair("foo", "bar")).IsAFailingCheckWithMessage("",
                "The checked dictionary does contain the given key-value pair, whereas it must not.",
                "The checked dictionary:", 
                "\t{[foo, bar]} (1 item)", 
                "Forbidden pair:", 
                "\t[[foo, bar]]");
        }

        [Test]
        public void ContainsPairFailsAppropriately()
        {
            Check.ThatCode(() =>
                    Check.That(SimpleDico).ContainsPair("demo", "1")
                )
                .IsAFailingCheckWithMessage(
                "",
                "The checked dictionary does not contain the expected value for the given key.",
                "The checked dictionary:",
                "\t{[demo, value], [other, test]} (2 items)",
                "Expected pair:",
                "\t[[demo, 1]]");

            Check.ThatCode(() =>
                    Check.That(SimpleDico).ContainsPair("demo2", "1")
                )
                .IsAFailingCheckWithMessage(
                "",
                "The checked dictionary does not contain the expected key-value pair. The given key was not found.",
                "The checked dictionary:",
                "\t{[demo, value], [other, test]} (2 items)",
                "Expected pair:",
                "\t[[demo2, 1]]");
        }

        [Test]
        public void IsEqualToWorks()
        {
            var dict = new Dictionary<string, object> { ["foo"] = new[] { "bar", "baz" } };
            var expected = new Dictionary<string, object> { ["foo"] = new[] { "bar", "baz" } };
            Check.That(dict).IsEqualTo(expected);
        }

        [Test]
        public void IsEqualToFailsOnDifference()
        {
            // just change the order
            var expected = new Dictionary<string, int> { ["bar"] = 1,  ["foo"] = 0 };
            Check.ThatCode( () =>
            Check.That(new Dictionary<string, int> { ["foo"] = 0, ["bar"] = 1 }).IsEqualTo(expected)).IsAFailingCheckWithMessage("", 
                "The checked dictionary is different from the expected one. 2 differences found! But they are equivalent.", 
                "actual key[0] = \"foo\" instead of \"bar\".", 
                "actual key[1] = \"bar\" instead of \"foo\".",                
                "The checked dictionary:", 
                "\t{[foo, 0], [bar, 1]} (2 items)", 
                "The expected dictionary:", 
                "\t{[bar, 1], [foo, 0]} (2 items)");
            Check.ThatCode( () =>
            Check.That(new Dictionary<string, int> { ["foo"] = 1, ["bar"] = 1 }).IsEqualTo(expected)).IsAFailingCheckWithMessage("", 
                "The checked dictionary is different from the expected one. 2 differences found!", 
                "actual key[0] = \"foo\" instead of \"bar\".", 
                "actual key[1] = \"bar\" instead of \"foo\".", 
                "The checked dictionary:", 
                "\t{[foo, 1], [bar, 1]} (2 items)", 
                "The expected dictionary:", 
                "\t{[bar, 1], [foo, 0]} (2 items)");
            Check.ThatCode( () =>
            Check.That(new Dictionary<string, int> { ["bar"] = 1, ["foo"] = 1 }).IsEqualTo(expected)).IsAFailingCheckWithMessage("", 
                "The checked dictionary is different from the expected one.", 
                "actual[\"foo\"] = 1 instead of 0.",
                "The checked dictionary:", 
                "\t{[bar, 1], [foo, 1]} (2 items)", 
                "The expected dictionary:", 
                "\t{[bar, 1], [foo, 0]} (2 items)");
            Check.ThatCode( () =>
            Check.That(new Dictionary<string, int> { ["bar!"] = 1, ["foo"] = 1 }).IsEqualTo(expected)).IsAFailingCheckWithMessage("", 
                "The checked dictionary is different from the expected one. 2 differences found!", 
                "actual key[0] = \"bar!\" instead of \"bar\".", 
                "actual[\"foo\"] = 1 instead of 0.",
                "The checked dictionary:", 
                "\t{[bar!, 1], [foo, 1]} (2 items)", 
                "The expected dictionary:", 
                "\t{[bar, 1], [foo, 0]} (2 items)");
            Check.ThatCode( () =>
            Check.That(new Dictionary<string, int> { ["bar"] = 1}).IsEqualTo(expected)).IsAFailingCheckWithMessage("", 
                "The checked dictionary is different from the expected one.", 
                "actual[\"foo\"] does not exist. Expected 0.", 
                "The checked dictionary:", 
                "\t{[bar, 1]} (1 item)", 
                "The expected dictionary:", 
                "\t{[bar, 1], [foo, 0]} (2 items)");
            Check.ThatCode( () =>
                Check.That(new Dictionary<string, int> { ["bar"] = 1, ["foo"] = 0, ["extra"] = 2 }).IsEqualTo(expected)).IsAFailingCheckWithMessage("", 
                "The checked dictionary is different from the expected one.", 
                "actual[\"extra\"] should not exist (value null)",
                "The checked dictionary:", 
                "\t{[bar, 1], [foo, 0], [extra, 2]} (3 items)", 
                "The expected dictionary:", 
                "\t{[bar, 1], [foo, 0]} (2 items)");
        }

        [Test]
        public void IsEqualToDealWithRecursion()
        {
            var dico = new Dictionary<string, object>();
            dico["first"] = dico;
            var expectedDico = new Dictionary<string, object>();
            expectedDico["first"] = expectedDico;

            Check.ThatCode(() => Check.That(dico).IsEqualTo(expectedDico)).IsAFailingCheckWithMessage("",
                "The checked dictionary is different from the expected one.", 
                "The checked dictionary:", 
                "\t{[first, System.Collections.Generic.Dictionary`2[System.String,System.Object]]} (1 item)", 
                "The expected dictionary:", 
                "\t{[first, System.Collections.Generic.Dictionary`2[System.String,System.Object]]} (1 item)");
        }

        [Test]
        public void IsEquivalentToWorks()
        {
            var dict = new Dictionary<string, object> { ["foo"] = new[] { "bar", "baz" } };
            var expected = new Dictionary<string, object> { ["foo"] = new[] { "bar", "baz" } };
            Check.That(dict).IsEquivalentTo(expected);
            Check.That((IDictionary<string, string>) null).IsEquivalentTo((IDictionary<string, string>)null);
#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40
            IReadOnlyDictionary<string, string> value = new RoDico(SimpleDico);
            Check.That(value).IsEqualTo(SimpleDico);
            Check.That(value).IsEquivalentTo(SimpleDico);
#endif
        }

        [Test]
        public void IsEquivalentToWorksWithCustomDics()
        {
            var customDic = new List<KeyValuePair<string, int>> {
                new KeyValuePair<string, int>("otherKey", 15) ,
                new KeyValuePair<string, int>("key", 12)
            };
            var dic = new Dictionary<string, int>{["otherKey"]= 15, ["key"] = 12};
            Check.That(customDic).IsEquivalentTo(dic);
            Check.That(dic).IsEquivalentTo(customDic);
            dic["extra"] = 20;
            Check.ThatCode(() => Check.That(customDic).IsEquivalentTo(dic)).IsAFailingCheckWithMessage("", 
                "The checked enumerable is not equivalent to the expected dictionary. Missing entry (\"extra\", 20).",
                "The checked enumerable:", 
                "\t{[otherKey, 15], [key, 12]} (2 items)", 
                "The expected dictionary:", 
                "\t{[otherKey, 15], [key, 12], [extra, 20]} (3 items)");
        }

        [Test]
        public void IsIsEquivalentFailsWithProperErrorMessage()
        {
            var expected = new Dictionary<string, object> { ["foo"] = new[] { "bar", "baz" } };
            Check.ThatCode(() =>
                Check.That(new Dictionary<string, object> { ["bar"] = new[] { "bar", "baz" } }).IsEquivalentTo(expected)).IsAFailingCheckWithMessage(
                "", 
                "The checked dictionary is not equivalent to the expected one. Missing entry (\"foo\", {\"bar\", \"baz\"}).",
                "The checked dictionary:", 
                "\t{[bar, System.String[]]} (1 item)", 
                "The expected dictionary:", 
                "\t{[foo, System.String[]]} (1 item)");
            Check.ThatCode(() =>
                Check.That(new Dictionary<string, object> { ["foo"] = new[] { "bar", "bar" } }).IsEquivalentTo(expected)).IsAFailingCheckWithMessage("", 
                "The checked dictionary is not equivalent to the expected one. Entry (\"foo\") does not have the expected value.", 
                "Expected:", 
                "\t{\"bar\", \"baz\"}", 
                "Actual:", 
                "\t{\"bar\", \"bar\"}", 
                "The checked dictionary:", 
                "\t{[foo, System.String[]]} (1 item)", 
                "The expected dictionary:", 
                "\t{[foo, System.String[]]} (1 item)");
            // added due to GH #307
            Check.ThatCode(() =>
                Check.That(expected).IsEquivalentTo(new Dictionary<string, object>())).IsAFailingCheckWithMessage("", 
                "The checked dictionary is not equivalent to the expected one. Extra entry present(\"foo\", {\"bar\", \"baz\"}).",
                "The checked dictionary:", 
                "\t{[foo, System.String[]]} (1 item)", 
                "The expected dictionary:", 
                "\t{} (0 item)");
            Check.ThatCode(() =>
                Check.That((IDictionary<string, object>) null).IsEquivalentTo(expected)).IsAFailingCheckWithMessage("", 
                "The checked enumerable is null whereas it should not.", 
                "The checked enumerable:", 
                "\t[null]", 
                "The expected dictionary:",
                "\t{[foo, System.String[]]} (1 item)");
            Check.ThatCode(() =>
                Check.That(new Dictionary<string, object> { ["foo"] = new[] { "bar", "bar" } }).IsEquivalentTo((IDictionary<string, object>)null)).IsAFailingCheckWithMessage(
                "", 
                "The checked dictionary must be null.", 
                "The checked dictionary:", 
                "\t{[foo, System.String[]]} (1 item)", 
                "The expected enumerable:", 
                "\t[null]");
        }

        [Test]
        public void IsIsEquivalentFailsWithProperErrorMessageWhenNegated()
        {
            var dict = new Dictionary<string, object> { ["foo"] = new[] { "bar", "baz" } };
            var expected = new Dictionary<string, object> { ["foo"] = new[] { "bar", "baz" } };
            Check.ThatCode(() =>
                Check.That(dict).Not.IsEquivalentTo(expected)).IsAFailingCheckWithMessage("", 
                "The checked dictionary is equivalent to the expected one, whereas it should not!", 
                "The checked dictionary:", 
                "\t{[foo, System.String[]]} (1 item)");
        }
    }
}
