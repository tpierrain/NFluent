 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="StringRelatedShould.cs" company="">
 //   Copyright 2013 Thomas PIERRAIN, Cyrille DUPUYDAUBY
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
    using ApiChecks;
    using Helpers;

    using System;
    using System.IO;
    using System.Text;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class StringChecksShould
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        [Test]
        public void ContainsWorksWithString()
        {
            Check.That(Alphabet).Contains("lmnop");
        }

        [Test]
        public void ContainsWithStringIsNegatable()
        {
            Check.That(Alphabet).Not.Contains("1234");

            Check.That((string) null).Not.Contains("test");
        }

        [Test]
        public void ContainsFailsProperlyOnNullString()
        {
            Check.ThatCode(() => { Check.That((string) null).Contains("fails", "anyway"); })
                .IsAFailingCheckWithMessage("", 
                    "The checked string is null.",
                             "The expected value(s): contains",
                    "\t{\"fails\", \"anyway\"} (2 items)");
        }

        [Test]
        public void ContainsIsCaseSensitive()
        {
            Check.ThatCode(() => { Check.That(Alphabet).Contains("C", "a", "A", "z"); })
                .IsAFailingCheckWithMessage("",
                             "The checked string does not contain the expected value(s): {\"C\", \"A\"}",
                              "The checked string:",
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]" ,
                    "The expected value(s): contains",
                    "\t{\"C\", \"a\", \"A\", \"z\"} (4 items)");
        }

        [Test]
        public void ContainsThrowsExceptionWhenFails()
        {
            Check.ThatCode(() => { Check.That(Alphabet).Contains("c", "0", "4"); })
                .IsAFailingCheckWithMessage("",
                             "The checked string does not contain the expected value(s): {\"0\", \"4\"}", 
                             "The checked string:",
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]",
                    "The expected value(s): contains",
                    "\t{\"c\", \"0\", \"4\"} (3 items)");
        }

        [Test]
        public void ContainsWorks()
        {
            Check.That(Alphabet).Contains("c", "z", "u");
        }

        [Test]
        public void DoesNotContainsWorks()
        {
            Check.That(Alphabet).DoesNotContain("one", "two", "three");
            Check.That((string) null).DoesNotContain("fails", "anyway");
        }

        [Test]
        public void DoesNotContainsFailWhenAppropriate()
        {
            Check.ThatCode(() => { Check.That(Alphabet).DoesNotContain("c", "z", "u"); })
                .IsAFailingCheckWithMessage("",
                             "The checked string contains unauthorized value(s): {\"c\", \"z\", \"u\"}",
                    "The checked string:",
                    "\t[\"abcdefghijklmnopqrstuvwxyz\"]",
                             "The expected value(s): does not contain",
                    "\t{\"c\", \"z\", \"u\"} (3 items)");
        }

        [Test]
        public void ContainsOnceWorksWithString()
        {
            Check.That(Alphabet).Contains("lmnop").Once();
        }

        [Test]
        public void ContainsOnceFailsProperly()
        {
            Check.ThatCode(() => { Check.That("abcdefghijklmnopqrstuvwxylmnopz").Contains("lmnop").Once(); })
                .IsAFailingCheckWithMessage("",
                    "The checked string contains \"lmnop\" at 11 and 25, where as it must contains it once.",
                    "The checked string:",
                    "\t[\"abcdefghijklmnopqrstuvwxylmnopz\"]",
                    "The expected value(s): once",
                    "\t{\"lmnop\"} (1 item)");
        }

        [Test]
        public void ContainsOnceFailsWhenNegated()
        {
            Check.ThatCode(()=>
            Check.That(Alphabet).Not.Contains("lmnop12").Once()).
                Throws<InvalidOperationException>().WithMessage("Once can't be used when negated");
        }

        [Test]
        public void ContainsInThatOrderWorksWithString()
        {
            Check.That(Alphabet).Contains("ab", "cd").InThatOrder();
        }

        [Test]
        public void ContainsInThatOrderFailsProperly()
        {
            Check.ThatCode(() => { Check.That(Alphabet).Contains("cd", "ab").InThatOrder(); })
                .IsAFailingCheckWithMessage("",
                            "The checked string does not contain the expected strings in the correct order.",
                            "The checked string:",
                            "\t[\"abcdefghijklmnopqrstuvwxyz\"]",
                            "The expected value(s): in this order",
                            "\t{\"cd\", \"ab\"} (2 items)");
        }

        [Test]
        public void ContainsInThatOrderFailsWhenNegated()
        {
            Check.ThatCode(()=>
                    Check.That(Alphabet).Not.Contains("lmnop12").InThatOrder()).
                Throws<InvalidOperationException>().WithMessage("InThatOrder can't be used when negated");
        }

        [Test]
        public void StartWithIsCaseSensitive()
        {
            Check.ThatCode(() => { Check.That(Alphabet).StartsWith("ABCDEF"); }).IsAFailingCheckWithMessage("",
                 "The checked string's start is different from the expected one.",
                 "The checked string:" ,
                 "\t[\"abcdefghijklmnopqrstuvwxyz\"]",
                 "The expected string: starts with" ,
                 "\t[\"ABCDEF\"]");
        }

        [Test]
        public void StartsWithFailsProperlyOnNullString()
        {
            Check.ThatCode(() => { Check.That((string) null).StartsWith("fails"); })
                .IsAFailingCheckWithMessage(Environment.NewLine + "The checked string is null." + Environment.NewLine +
                             "The expected string: starts with" + Environment.NewLine + "\t[\"fails\"]");
        }

        [Test]
        public void StartWithWorks()
        {
            Check.That(Alphabet).StartsWith("abcdef");
        }

        [Test]
        public void StartWithIsNegatable()
        {
            Check.That(Alphabet).Not.StartsWith("hehehe");
        }

        [Test]
        public void NegatedStartWithThrowsException()
        {
            Check.ThatCode(() => { Check.That(Alphabet).Not.StartsWith("abcdef"); })
                .IsAFailingCheckWithMessage("",
                    "The checked string starts with the given one, whereas it must not.",
                             "The checked string:",
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]",
                             "The expected string: does not start with",
                    "\t[\"abcdef\"]");
        }

        [Test]
        public void NegatedStartsWithWorks()
        {
            Check.That("test").Not.StartsWith("Toto");
            Check.That((string) null).Not.StartsWith("Toto");
        }

        [Test]
        public void EndsWithIsCaseSensitive()
        {
            Check.ThatCode(() => { Check.That(Alphabet).EndsWith("UWXYZ"); })
                .IsAFailingCheckWithMessage(Environment.NewLine + "The checked string's end is different from the expected one." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]" + Environment.NewLine +
                             "The expected string: ends with" + Environment.NewLine + "\t[\"UWXYZ\"]");
        }

        [Test]
        public void EndsWithFailsProperlyOnNullString()
        {
            Check.ThatCode(() => { Check.That((string) null).EndsWith("fails"); })
                .IsAFailingCheckWithMessage(Environment.NewLine + "The checked string is null." + Environment.NewLine +
                             "The expected string: ends with" + Environment.NewLine + "\t[\"fails\"]");
        }

        [Test]
        public void EndsWithWorks()
        {
            Check.That(Alphabet).EndsWith("uvwxyz");
        }

        [Test]
        public void EndsWithIsNegatable()
        {
            Check.That(Alphabet).Not.EndsWith("hehehe");
            Check.That((string) null).Not.EndsWith("test");
        }

        [Test]
        public void EndsWithIsNegatableFails()
        {
            Check.ThatCode(() => { Check.That(Alphabet).Not.EndsWith("vwxyz"); })
                .IsAFailingCheckWithMessage("",
                    "The checked string ends with given one, whereas it must not.",
                    "The checked string:",
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]",
                             "The expected string: does not end with",
                    "\t[\"vwxyz\"]");
        }

        [Test]
        public void EqualWorks()
        {
            var check = "toto";
            Check.That(check).IsEqualTo("toto");
        }

        [Test]
        public void EqualFailsWithSameLength()
        {
            var check = "toto";

            Check.ThatCode(() => { Check.That(check).IsEqualTo("tutu"); })
                .IsAFailingCheckWithMessage("",
                             "The checked string is different from the expected one but has same length.",
                             "The checked string:", "\t[\"toto\"]",
                             "The expected string:", "\t[\"tutu\"]");
        }

        [Test]
        public void EqualFailsWithDiffCase()
        {
            var check = "toto";
            Check.ThatCode(() => { Check.That(check).IsEqualTo("TOTO"); })
                .IsAFailingCheckWithMessage("",
                    "The checked string is different in case from the expected one.",
                    "The checked string:", "\t[\"toto\"]",
                    "The expected string:","\t[\"TOTO\"]");
        }

        [Test]
        public void EqualFailsInGeneral()
        {
            var check = "toto";

            Check.ThatCode(() => { Check.That(check).IsEqualTo("to o"); })
                .IsAFailingCheckWithMessage("", "The checked string is different from the expected one but has same length.", 
                    "The checked string:", "\t[\"toto\"]", 
                    "The expected string:","\t[\"to o\"]");

            Check.ThatCode(() => { Check.That(check).IsEqualTo("tititutu"); })
                .IsAFailingCheckWithMessage("", "The checked string is different from expected one.", 
                             "The checked string:", "\t[\"toto\"]", 
                             "The expected string:","\t[\"tititutu\"]");
        }

        [Test]
        public void EqualFailshWhenShorter()
        {
            var check = "titi";
            Check.ThatCode(() => { Check.That(check).IsEqualTo("tititutu"); })
                .IsAFailingCheckWithMessage("",
                             "The checked string is different from expected one, it is missing the end.",
                             "The checked string:", "\t[\"titi\"]", 
                             "The expected string:", "\t[\"tititutu\"]");

        }

        [Test]
        public void EqualFailshWhenStartSame()
        {
            var check = "tititutu";

            Check.ThatCode(() => { Check.That(check).IsEqualTo("titi"); })
                .IsAFailingCheckWithMessage("",
                             "The checked string is different from expected one, it contains extra text at the end.",
                             "The checked string:", "\t[\"tititutu\"]",
                             "The expected string:", "\t[\"titi\"]");
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionsOnString()
        {
            Check.That(Alphabet)
                .Contains("i")
                .And.StartsWith("abcd")
                .And.IsInstanceOf<string>()
                .And.IsNotInstanceOf<int>()
                .And.Not.IsNotInstanceOf<string>();
            Check.That(Alphabet).HasSize(26);
        }

        [Test]
        public void HasSizeTest()
        {
            Check.That(Alphabet).HasSize(26);
        }

        #region IsOneOfThese

        [Test]
        public void IsOneOfTheseWorks()
        {
            Check.That("The Black Keys").IsOneOfThese("Metronomy", "Sigur Ros", "The Black Keys", "Get Well Soon");
        }

        [Test]
        public void IsOneOfTheseWorksThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
                {
                    Check.That("The Black Keys").IsOneOfThese("Paco de Lucia", "Jimi Hendrix", "Baden Powell");
                })
                .IsAFailingCheckWithMessage("",
            "The checked string is not one of the possible elements.",
            "The checked string:",
            "\t[\"The Black Keys\"]",
            "The expected string: one of these",
            "\t{\"Paco de Lucia\", \"Jimi Hendrix\", \"Baden Powell\"}");
        }

        [Test]
        public void NotIsOneOfTheseWorks()
        {
            Check.That("The Black Keys").Not.IsOneOfThese("Paco de Lucia", "Jimi Hendrix", "Baden Powell");
        }

        [Test]
        public void IsOneOfTheseWorksWithNull()
        {
            Check.That((string) null).IsOneOfThese(null);
        }

        [Test]
        public void IsOneOfTheseThrowsProperExceptionWithNullAsExpectedValues()
        {
            Check.ThatCode(() => { Check.That("whatever").IsOneOfThese(null); })
                .IsAFailingCheckWithMessage("", "The checked string must be null as there is no other possible value.", "The checked string:", "\t[\"whatever\"]");
        }

        [Test]
        public void NotIsOneOfTheseThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
                {
                    Check.That("The Black Keys")
                        .Not.IsOneOfThese("Metronomy", "Sigur Ros", "The Black Keys", "Get Well Soon");
                })
                .IsAFailingCheckWithMessage("",
                             "The checked string is one of the possible elements whereas it must not.",
                    "The checked string:",
                    "\t[\"The Black Keys\"]",
                    "The expected string: none of these",
                    "\t{\"Metronomy\", \"Sigur Ros\", \"The Black Keys\", \"Get Well Soon\"}");
        }

        [Test]
        public void StringMatchesWorks()
        {
            Check.That("12 ac").Matches("[0-9]*. [a-z]*");
        }

        [Test]
        public void StringMatchesFails()
        {
            Check.ThatCode(() => { Check.That("AC 12").Matches("[0-9]. [a-z]*"); })
                .IsAFailingCheckWithMessage("", 
                    "The checked string does not match the expected one.", 
                    "The checked string:", 
                    "\t[\"AC 12\"]",
                "The expected string: matches", 
                    "\t[\"[0-9]. [a-z]*\"]");
        }

        [Test]
        public void StringMatchesWildcardWorks()
        {
            // * works
            Check.That("12 ac").MatchesWildcards("1*c");
            // ? works
            Check.That("12 ac").MatchesWildcards("12??c");
        }

        [Test]
        public void StringMatchesWildcardFails()
        {
            Check.ThatCode(() => { Check.That("AC 12").MatchesWildcards("1*c"); })
                .IsAFailingCheckWithMessage("", 
                    "The checked string does not match the expected one.", 
                    "The checked string:", "\t[\"AC 12\"]",
                "The expected string: matches", "\t[\"1*c\"]");
            // must match from first char
            Check.ThatCode(() => { Check.That(" AC 12").MatchesWildcards("AC*"); })
                .IsAFailingCheckWithMessage("", 
                    "The checked string does not match the expected one.", 
                    "The checked string:", "\t[\" AC 12\"]",
                    "The expected string: matches", "\t[\"AC*\"]");
            // ... to last char
            Check.ThatCode(() => { Check.That("AC 12 ").MatchesWildcards("AC??2"); })
                .IsAFailingCheckWithMessage("", 
                    "The checked string does not match the expected one.", 
                    "The checked string:", "\t[\"AC 12 \"]",
                    "The expected string: matches", "\t[\"AC??2\"]");
        }

        [Test]
        public void MatchesWildcardFailsWhenNegated()
        {
            Check.ThatCode(() => { Check.That("12 ac").Not.MatchesWildcards("1*c"); })
                .IsAFailingCheckWithMessage("", 
                    "The checked string matches the given one, whereas it must not.", 
                    "The checked string:", 
                    "\t[\"12 ac\"]",
                    "The expected string: does not match", 
                    "\t[\"1*c\"]");
        }

        [Test]
        public void StringMatchesFailsProperlyForNull()
        {
            Check.ThatCode(() => { Check.That((string) null).Matches("[0-9]. [a-z]*"); })
                .IsAFailingCheckWithMessage("", "The checked string is null.", "The expected string: matches", "\t[\"[0-9]. [a-z]*\"]");
        }

        [Test]
        public void NotStringMatchesWorks()
        {
            Check.That("AC 12").Not.Matches("[0-9]. [a-z]*");
            Check.That((string) null).Not.Matches("[0-9]. [a-z]*");
        }

        [Test]
        public void NotStringMatchesFails()
        {
            Check.ThatCode(() => { Check.That("12 ac").Not.Matches("[0-9]*. [a-z]*"); })
                .IsAFailingCheckWithMessage("", "The checked string matches the given one, whereas it must not.", "The checked string:", "\t[\"12 ac\"]", 
                    "The expected string: does not match", "\t[\"[0-9]*. [a-z]*\"]");
        }

        #endregion

        #region Match

        [Test]
        public void StringDoesNotMatchWorks()
        {
            Check.That("ac 12").DoesNotMatch("[0-9]. [a-z]*");
        }

        [Test]
        public void StringDoesNotMatchFails()
        {
            Check.ThatCode(() => { Check.That("12 AC").DoesNotMatch("[0-9]. [a-z]*"); })
                .IsAFailingCheckWithMessage("", "The checked string matches the given one, whereas it must not.", "The checked string:", "\t[\"12 AC\"]", 
                    "The expected string: does not match", "\t[\"[0-9]. [a-z]*\"]");
        }

        [Test]
        public void StringDoesNotMatchProperlyForNull()
        {
            Check.ThatCode(() => { Check.That((string) null).Matches("[0-9]. [a-z]*"); })
                .IsAFailingCheckWithMessage("", "The checked string is null.", "The expected string: matches", "\t[\"[0-9]. [a-z]*\"]");
        }

        [Test]
        public void NotStringDoesNotMatchWorks()
        {
            Check.That("AC 12").Not.Matches("[0-9]. [a-z]*");
            Check.That((string) null).Not.Matches("[0-9]. [a-z]*");
        }

        [Test]
        public void NotStringDoesNotMatchFails()
        {
            Check.ThatCode(() => { Check.That("12 ac").Not.Matches("[0-9]*. [a-z]*"); })
                .IsAFailingCheckWithMessage("", "The checked string matches the given one, whereas it must not.", "The checked string:", "\t[\"12 ac\"]",
                    "The expected string: does not match", "\t[\"[0-9]*. [a-z]*\"]");
        }

        [Test]
        public void IsEmptyWorks()
        {
            Check.That(string.Empty).IsEmpty();
            Check.That(string.Empty).Not.IsNotEmpty();
            Check.That("test").Not.IsEmpty();
            Check.That("test").IsNotEmpty();
        }

        [Test]
        public void IsEmptyFailsIfNotEmpty()
        {
            Check.ThatCode(() => { Check.That("test").IsEmpty(); })
                .IsAFailingCheckWithMessage(Environment.NewLine + "The checked string is not empty." + Environment.NewLine +
                             "The checked string:" + Environment.NewLine + "\t[\"test\"]");
        }

        [Test]
        public void IsEmptyFailsIfNnull()
        {
            Check.ThatCode(() => { Check.That((string) null).IsEmpty(); })
                .IsAFailingCheckWithMessage("", "The checked string is null instead of being empty.");
        }

        [Test]
        public void NegatedIsNotEmptyFailsIfNotEmpty()
        {
            Check.ThatCode(() => { Check.That("test").Not.IsNotEmpty(); })
                .IsAFailingCheckWithMessage(Environment.NewLine + "The checked string is not empty or null." + Environment.NewLine +
                             "The checked string:" + Environment.NewLine + "\t[\"test\"]");
        }

        [Test]
        public void IsNotEmptyFailsIfEmpty()
        {
            Check.ThatCode(() => { Check.That(string.Empty).IsNotEmpty(); })
                .IsAFailingCheckWithMessage(Environment.NewLine + "The checked string is empty, whereas it must not.");
        }

        [Test]
        public void NegatedIsNotEmptyFailsIfNull()
        {
            Check.That((string) null).Not.IsNotEmpty();
        }

        [TestCase("")]
        [TestCase(null)]
        public void IsNullOrEmptyWorks(string sut)
        {
            Check.That(sut).IsNullOrEmpty();
        }

        [Test]
        public void IsNullEmptyFailsIfNotEmpty()
        {
            Check.ThatCode(() => { Check.That("test").IsNullOrEmpty(); })
                .IsAFailingCheckWithMessage(Environment.NewLine + "The checked string is not empty or null." + Environment.NewLine +
                             "The checked string:" + Environment.NewLine + "\t[\"test\"]");
        }

        [TestCase("   ")]
        [TestCase("")]
        [TestCase(null)]
        public void IsNullOrWhiteSpacesWork(string sut)
        {
            Check.That(sut).IsNullOrWhiteSpace();
        }

        [Test]
        public void IsNullOrWhiteSpacesFailsWhenNegated()
        {
            Check.ThatCode(()=>
            Check.That("    ").Not.IsNullOrWhiteSpace()).IsAFailingCheckWithMessage(
                "", 
                "The checked string contains only whitespace characters, whereas it should not.", 
                "The checked string:", 
                "\t[\"    \"]");
            Check.ThatCode(()=>
                Check.That("").Not.IsNullOrWhiteSpace()).IsAFailingCheckWithMessage(
                "", 
                "The checked string is empty, whereas it should not.", 
                "The checked string:", 
                "\t[\"\"]");
            Check.ThatCode(()=>
                Check.That((string)null).Not.IsNullOrWhiteSpace()).IsAFailingCheckWithMessage(
                "", 
                "The checked string is null, whereas it should not.", 
                "The checked string:", 
                "\t[null]");
        }

        [Test]
        public void IsNullOrWhiteSpaceFailsWhenRelevant()
        {
            Check.ThatCode(() =>
                Check.That("non empty").IsNullOrWhiteSpace()).IsAFailingCheckWithMessage("", "The checked string contains non whitespace characters.", "The checked string:", "\t[\"non empty\"]");
        }

        [Test]
        public void HasContentWorks()
        {
            Check.That("test").HasContent();
        }

        [Test]
        public void HasContentFailsIfEmpty()
        {
            Check.ThatCode(() => { Check.That(string.Empty).HasContent(); })
                .IsAFailingCheckWithMessage(Environment.NewLine + "The checked string is empty, whereas it must not.");
        }

        [Test]
        public void HasContentFailsIfNull()
        {
            Check.ThatCode(() => { Check.That((string) null).HasContent(); })
                .IsAFailingCheckWithMessage(Environment.NewLine + "The checked string is null whereas it must have content.");
        }

        [Test]
        public void CompareNoCaseWorks()
        {
            Check.That("test").IsEqualIgnoringCase("TEST");
            Check.That("tESt").IsEqualIgnoringCase("TEst");
        }

        [Test]
        public void CompareNoCaseFails()
        {
            Check.ThatCode(() => { Check.That("test").IsEqualIgnoringCase("TOAST"); })
                .IsAFailingCheckWithMessage("", "The checked string is different from expected one.",
                             "The checked string:", "\t[\"test\"]",
                             "The expected string:", "\t[\"TOAST\"]");
        }

        [Test]
        public void CompareNoCaseFailsWithStartOnly()
        {
            Check.ThatCode(() => { Check.That("test").IsEqualIgnoringCase("Te"); })
                .IsAFailingCheckWithMessage("",
                             "The checked string is different from expected one, it contains extra text at the end.",
                             "The checked string:", "\t[\"test\"]", 
                             "The expected string:", "\t[\"Te\"]");
        }

        #endregion

        [Test]
        public void IdentifyExtraLines()
        {
            Check.ThatCode(() => Check.That("toto").IsEqualTo("toto\n")).IsAFailingCheckWithMessage("", 
                "The checked string is different from expected one, it is missing some line(s). At line 2, expected '' but line is missing.", 
                "The checked string:", 
                "\t[\"toto\"]", 
                "The expected string:", 
                "\t[\"toto\n\"]");
            Check.ThatCode(() => Check.That("\ntoto").IsEqualTo("\ntoto\n")).IsAFailingCheckWithMessage("", 
                "The checked string is different from expected one, it is missing some line(s). At line 3, expected '' but line is missing.", 
                "The checked string:", 
                "\t[\"\ntoto\"]", 
                "The expected string:", 
                "\t[\"\ntoto\n\"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsWhiteSpacesAndTabsDifference()
        {
            var withWSp = "Hello    How are you?";
            var withTab = "Hello\tHow are you?";

            Check.ThatCode(() => { Check.That(withTab).IsEqualTo(withWSp); })
                .IsAFailingCheckWithMessage("",
                    "The checked string has different spaces than expected one. At line 1, col 6, expected 'Hello    How are you?' was 'Hello<<tab>>How are you?'.",
                             "The checked string:",
                             "\t[\"Hello\tHow are you?\"]","The expected string:",
                            "\t[\"Hello    How are you?\"]");
            
            Check.ThatCode(() => { Check.That("toto ").IsEqualTo("toto  "); })
                .IsAFailingCheckWithMessage("",
                    "The checked string has different spaces than expected one.",
                    "The checked string:", "\t[\"toto \"]", 
                    "The expected string:", "\t[\"toto  \"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsAllWhiteSpacesAndTabsDifferences()
        {
            var withWSp = "Hello    How are you?    kiddo";
            var withTab = "Hello\t   How are you?\tkiddo";

            Check.ThatCode(() => { Check.That(withWSp).IsEqualTo(withTab); })
                .IsAFailingCheckWithMessage("",
                    "The checked string has different spaces than expected one. At line 1, col 9, expected 'Hello<<tab>>   How are you?<<tab>>kiddo' was 'Hello    How are you?    kiddo'.",
                    "The checked string:",
                    "\t[\"Hello    How are you?    kiddo\"]", "The expected string:",
                    "\t[\"Hello\t   How are you?\tkiddo\"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsAllWhiteSpacesAndTabsDifferencesEvenWhenOtherIssues()
        {
            var withWSp = "Hello    How are you?    kiddo";
            var withTab = "Hello\tHow are you?\tkiddo dingo";

            Check.ThatCode(() => { Check.That(withWSp).IsEqualTo(withTab); })
                .IsAFailingCheckWithMessage("", 
                    "The checked string is different from expected one, it is missing the end.", 
                    "The checked string:", 
                    "\t[\"Hello    How are you?    kiddo\"]", 
                    "The expected string:", 
                    "\t[\"Hello	How are you?	kiddo dingo\"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsLineFeedAndCarriageReturnLineFeed()
        {
            var withCRLF = "Hello\r\nHow are you?";
            var withLF = "Hello\nHow are you?";

            Check.ThatCode(() => { Check.That(withCRLF).IsEqualTo(withLF); })
                .IsAFailingCheckWithMessage("",
                    "The checked string has different end of line markers than expected one. At line 1, col 6, expected 'Hello<<LF>>' was 'Hello<<CRLF>>'.",
                    "The checked string:",
                    "\t[\"Hello",
                    "How are you?\"]",
                    "The expected string:",
                    "\t[\"Hello\nHow are you?\"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsOnlyTheFirstLineFeedAndCarriageReturnLineFeedDifference()
        {
            var withCRLF = "Hello\r\nHow are you?\r\nAre you kidding?";
            var withLF = "Hello\nHow are you?\nAre you kidding?";

            Check.ThatCode(() => { Check.That(withLF).IsEqualTo(withCRLF); })
                .IsAFailingCheckWithMessage(Environment.NewLine +
                             "The checked string has different end of line markers than expected one. At line 1, col 6, expected 'Hello<<CRLF>>' was 'Hello<<LF>>'." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "	[\"Hello\nHow are you?\nAre you kidding?\"]" + Environment.NewLine +
                             "The expected string:" + Environment.NewLine +
                             "	[\"Hello\r\nHow are you?\r\nAre you kidding?\"]");
        }

        [Test]
        public void IsEqualToErrorMessageWhenVariousDifferences()
        {
            var withCRLF = "Hello\r\nHow are you?\r\nare you kidding?";
            var withLF = "Hello\nHow are you?\nAre you kidding?";

            Check.ThatCode(() => { Check.That(withLF).IsEqualTo(withCRLF); })
                .IsAFailingCheckWithMessage("",
                             "The checked string is different from expected one. At line 1, col 6, expected 'Hello<<CRLF>>' was 'Hello<<LF>>'.",
                            "The checked string:",
                             "\t[\"Hello\nHow are you?\nAre you kidding?\"]",
                             "The expected string:",
                             "\t[\"Hello", "How are you?", "are you kidding?\"]");
        }

        [Test]
        public void ShouldDisplayFullLineIfItIsShort()
        {
            var multilineExpected = "Hello\nThis\nwill fail.";
            var multilineActual = "Hello\nIt has\nfailed";

            Check.ThatCode(() => Check.That(multilineActual).IsEqualTo(multilineExpected))
                .IsAFailingCheckWithMessage("",
                             "The checked string is different from expected one. At line 2, col 1, expected 'This<<LF>>' was 'It has<<LF>>'.",
                              "The checked string:",
                             "\t[\"Hello\nIt has\nfailed\"]",
                             "The expected string:",
                             "\t[\"Hello\nThis\nwill fail.\"]");
        }


        [Test]
        public void LongLineIsTruncated()
        {
            Check.ThatCode(() =>
                    Check.That("0123456789abcdefghijklmnopqrstuvwxyz()_-")
                        .IsEqualTo("0123456789abcdefghijklmnopqrstuvwxyz()-_"))
                .IsAFailingCheckWithMessage("", 
                    "The checked string is different from the expected one but has same length.", 
                    "The checked string:", 
                    "\t[\"0123456789abcdefghijklmnopqrstuvwxyz()_-\"]", 
                    "The expected string:", 
                    "\t[\"0123456789abcdefghijklmnopqrstuvwxyz()-_\"]");
        }

        [Test]
        public void VeryLongStringErrorMessageIsProperlyTruncated()
        {
            var curLen = Check.StringTruncationLength;
            try
            {
                Check.StringTruncationLength = 209;
                var checkString = File.ReadAllText(TestFiles.CheckedFile, Encoding.UTF8).Replace("\r\n", "");
                var expectedString = File.ReadAllText(TestFiles.ExpectedFile, Encoding.UTF8).Replace("\r\n", "");

                Check.ThatCode(() =>
                    {
                        Check.That(checkString).IsEqualTo(expectedString);
                    })
                    .IsAFailingCheckWithMessage("",
                        "The checked string is different from the expected one but has same length. At line 1, col 4554, expected '...IST>Joe Cooker</ARTI...' was '...IST>Joe Cocker</ARTI...'.",
                        "The checked string:",
                        "\t[\"<?xml version=\"1.0\" encoding=\"utf-8\" ?><!--  Edited by XMLSpy  --><CATALOG>  <CD>    <TITLE>Empire Burlesque</TITLE>    <ARTIST>Bob Dylan</ARTIST>    ...<<truncated>>...E>    <YEAR>1987</YEAR>  </CD></CATALOG>\"]",
                        "The expected string:",
                        "\t[\"<?xml version=\"1.0\" encoding=\"utf-8\" ?><!--  Edited by XMLSpy  --><CATALOG>  <CD>    <TITLE>Empire Burlesque</TITLE>    <ARTIST>Bob Dylan</ARTIST>    ...<<truncated>>...E>    <YEAR>1987</YEAR>  </CD></CATALOG>\"]"
                        );
            }
            finally
            {
                Check.StringTruncationLength = curLen;
            }
        }

        [Test]
        public void UserCanControlTruncationLength()
        {
            var curLen = Check.StringTruncationLength;
            try
            {
                // large truncation
                Check.StringTruncationLength = 10000;
                var checkString = File.ReadAllText(TestFiles.CheckedFile, Encoding.UTF8).Replace("\r\n", "");
                var expectedString = File.ReadAllText(TestFiles.ExpectedFile, Encoding.UTF8).Replace("\r\n", "");

                Check.ThatCode(() =>
                    {
                        Check.That(checkString).IsEqualTo(expectedString);
                    })
                    .ThrowsAny()
                    .AndWhichMessage()
                    .AsLines().HasElementAt(3).Which.HasSize(4684);

                // small truncation
                Check.StringTruncationLength = 25;
                Check.ThatCode(() =>
                    {
                        Check.That("abcdefghijklmnopqrstuvwxyz").IsEqualTo("abcdefghijklmnopqrstuvwxy");
                    })
                    .IsAFailingCheckWithMessage("",
                        "The checked string is different from expected one, it contains extra text at the end.", 
                        "The checked string:",
                        "\t[\"abcd...<<truncated>>...yz\"]", 
                        "The expected string:", 
                        "\t[\"abcdefghijklmnopqrstuvwxy\"]");
            }
            finally
            {
                Check.StringTruncationLength = curLen;
            }
        }


        [Test]
        public void ShouldReportExtraLines()
        {
            Check.ThatCode(() =>
                {
                    Check.That("This is one line.\nAnd another.").IsEqualTo("This is one line.");
                })
                .IsAFailingCheckWithMessage("",
                "The checked string is different from expected one, it contains extra lines at the end. Found line 2 'And another.'.",
                "The checked string:",
                "\t[\"This is one line.\nAnd another.\"]",
                "The expected string:",
                "\t[\"This is one line.\"]");
        }

        [Test]
        public void AsLinesShouldWorkWithNull()
        {
            Check.That((string)null).AsLines().HasSize(0);
        }

        [Test]
        public void AsLinesShouldWorkWithOneLine()
        {
            Check.That("coucou").AsLines().HasSize(1);
        }

        [Test]
        public void ShouldReportLongerLines()
        {

            Check.ThatCode(() => { Check.That("toto t").IsEqualTo("toto  "); })
                .IsAFailingCheckWithMessage("",
                    "The checked string is different from expected one, it contains extra text at the end.",
                    "The checked string:", "\t[\"toto t\"]", 
                    "The expected string:", "\t[\"toto  \"]");
            
            Check.ThatCode(() =>
                {
                    Check.That("This is one line.\nAnd another line.").IsEqualTo("This is one line.\nAnd another");
                })
                .IsAFailingCheckWithMessage("",
                    "The checked string is different from expected one, one line is longer. At line 2, col 12, expected '...nd another' was '...nd another line.'."
                    , "The checked string:",
                    "\t[\"This is one line.\nAnd another line.\"]",
                    "The expected string:",
                    "\t[\"This is one line.\nAnd another\"]");
        }

        [Test]
        public void ShouldReportShorterLines()
        {
            Check.ThatCode(() =>
                {
                    Check.That("This is a line.\nAnd a next").IsEqualTo("This is a line.\nAnd a next line.");
                })
                .IsAFailingCheckWithMessage("",
                    "The checked string is different from expected one, one line is shorter. At line 2, col 11, expected 'And a next line.' was 'And a next'.",
                    "The checked string:",
                "\t[\"This is a line.\nAnd a next\"]",
                    "The expected string:",
                "\t[\"This is a line.\nAnd a next line.\"]");
            Check.ThatCode(() =>
                {
                    Check.That("This is one line.\nAnd another").IsEqualTo("This is one line.\nAnd another line.");
                })
                .IsAFailingCheckWithMessage("",
                    "The checked string is different from expected one, one line is shorter. At line 2, col 12, expected '...nd another line.' was '...nd another'.",
                    "The checked string:",
                "\t[\"This is one line.\nAnd another\"]",
                    "The expected string:",
                "\t[\"This is one line.\nAnd another line.\"]");
        }

        [Test]
        public void ShouldReportMissingLines()
        {
            Check.ThatCode(() =>
                {
                    Check.That("This is one line.").IsEqualTo("This is one line.\nAnd another.");
                })
                .IsAFailingCheckWithMessage("",
                    "The checked string is different from expected one, it is missing some line(s). At line 2, expected 'And another.' but line is missing.",
                    "The checked string:",
                    "\t[\"This is one line.\"]",
                    "The expected string:",
                    "\t[\"This is one line.\nAnd another.\"]");
        }
    }
}