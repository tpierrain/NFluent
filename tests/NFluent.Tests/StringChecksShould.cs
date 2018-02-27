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
    using NUnit.Framework;

    [TestFixture]
    public class StringChecksShould
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        #region Public Methods and Operators

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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is null." + Environment.NewLine +
                             "The expected substring(s):" + Environment.NewLine + "\t[\"fails\", \"anyway\"]");
        }

        [Test]
        public void ContainsIsCaseSensitive()
        {
            Check.ThatCode(() => { Check.That(Alphabet).Contains("C", "a", "A", "z"); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string does not contains the expected value(s): \"C\", \"A\"" +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]" + Environment.NewLine + "The expected substring(s):" +
                             Environment.NewLine + "\t[\"C\", \"a\", \"A\", \"z\"]");
        }

        [Test]
        public void ContainsThrowsExceptionWhenFails()
        {
            Check.ThatCode(() => { Check.That(Alphabet).Contains("c", "0", "4"); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string does not contains the expected value(s): \"0\", \"4\"" +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]" + Environment.NewLine + "The expected substring(s):" +
                             Environment.NewLine + "\t[\"c\", \"0\", \"4\"]");
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
        public void DoesNotContainsFailsWhenAppropriate()
        {
            Check.ThatCode(() => { Check.That(Alphabet).DoesNotContain("c", "z", "u"); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string contains unauthorized value(s): \"c\", \"z\", \"u\"" +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]" + Environment.NewLine +
                             "The unauthorized substring(s):" + Environment.NewLine + "\t[\"c\", \"z\", \"u\"]");
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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string contains \"lmnop\" at 11 and 25, where as it must contains it once." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"abcdefghijklmnopqrstuvwxylmnopz\"]" + Environment.NewLine +
                             "The expected string: one" + Environment.NewLine + "\t[\"lmnop\"]");
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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string does not contain the expected strings in the correct order." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]" + Environment.NewLine + "Expected content: " +
                             Environment.NewLine + "\t[\"cd\", \"ab\"]");
        }

        [Test]
        public void StartWithIsCaseSensitive()
        {
            Check.ThatCode(() => { Check.That(Alphabet).StartsWith("ABCDEF"); }).FailsWithMessage("",
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
                .FailsWithMessage(Environment.NewLine + "The checked string is null." + Environment.NewLine +
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
                .FailsWithMessage(Environment.NewLine + "The checked string starts with expected one, whereas it must not." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]" + Environment.NewLine +
                             "The expected string: does not start with" + Environment.NewLine + "\t[\"abcdef\"]");
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
                .FailsWithMessage(Environment.NewLine + "The checked string's end is different from the expected one." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]" + Environment.NewLine +
                             "The expected string: ends with" + Environment.NewLine + "\t[\"UWXYZ\"]");
        }

        [Test]
        public void EndsWithFailsProperlyOnNullString()
        {
            Check.ThatCode(() => { Check.That((string) null).EndsWith("fails"); })
                .FailsWithMessage(Environment.NewLine + "The checked string is null." + Environment.NewLine +
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
                .FailsWithMessage(Environment.NewLine + "The checked string ends with expected one, whereas it must not." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"abcdefghijklmnopqrstuvwxyz\"]" + Environment.NewLine +
                             "The expected string: does not end with" + Environment.NewLine + "\t[\"vwxyz\"]");
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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string is different from the expected one but has same length." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"toto\"]" +
                             Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"tutu\"]");
        }

        [Test]
        public void EqualFailsWithDiffCase()
        {
            var check = "toto";

            Check.ThatCode(() => { Check.That(check).IsEqualTo("TOTO"); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is different in case from the expected one." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"toto\"]" +
                             Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"TOTO\"]");
        }

        [Test]
        public void EqualFailsInGeneral()
        {
            var check = "toto";

            Check.ThatCode(() => { Check.That(check).IsEqualTo("tititutu"); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is different from expected one." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"toto\"]" +
                             Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"tititutu\"]");
        }

        [Test]
        public void EqualFailshWhenShorter()
        {
            var check = "titi";

            Check.ThatCode(() => { Check.That(check).IsEqualTo("tititutu"); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string is different from expected one, it is missing the end." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"titi\"]" +
                             Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"tititutu\"]");
        }

        [Test]
        public void EqualFailshWhenStartSame()
        {
            var check = "tititutu";

            Check.ThatCode(() => { Check.That(check).IsEqualTo("titi"); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string is different from expected one, it contains extra text at the end." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"tititutu\"]" +
                             Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"titi\"]");
        }

        #endregion

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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is not one of the possible elements." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"The Black Keys\"]" + Environment.NewLine + "The possible elements:" +
                             Environment.NewLine + "\t[\"Paco de Lucia\", \"Jimi Hendrix\", \"Baden Powell\"]");
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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked value is not one of the possible elements." +
                             Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[\"whatever\"]" +
                             Environment.NewLine + "The possible elements:" + Environment.NewLine + "\t[null]");
        }

        [Test]
        public void NotIsOneOfTheseThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
                {
                    Check.That("The Black Keys")
                        .Not.IsOneOfThese("Metronomy", "Sigur Ros", "The Black Keys", "Get Well Soon");
                })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string is one of the possible elements whereas it must not." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"The Black Keys\"]" + Environment.NewLine + "The possible elements:" +
                             Environment.NewLine +
                             "\t[\"Metronomy\", \"Sigur Ros\", \"The Black Keys\", \"Get Well Soon\"]");
        }

        #endregion

        #region Match

        [Test]
        public void StringMatchesWorks()
        {
            Check.That("12 ac").Matches("[0-9]*. [a-z]*");
        }

        [Test]
        public void StringMatchesFails()
        {
            Check.ThatCode(() => { Check.That("AC 12").Matches("[0-9]. [a-z]*"); })
                .FailsWithMessage("", "The checked string does not match the expected one.", "The checked string:", "\t[\"AC 12\"]",
                "The expected string: matches", "\t[\"[0-9]. [a-z]*\"]");
        }

        [Test]
        public void StringMatchesFailsProperlyForNull()
        {
            Check.ThatCode(() => { Check.That((string) null).Matches("[0-9]. [a-z]*"); })
                .FailsWithMessage("", "The checked string is null.", "The expected string: matches", "\t[\"[0-9]. [a-z]*\"]");
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
                .FailsWithMessage("", "The checked string matches expected one, whereas it must not.", "The checked string:", "\t[\"12 ac\"]", 
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
                .FailsWithMessage("", "The checked string matches expected one, whereas it must not.", "The checked string:", "\t[\"12 AC\"]", 
                    "The expected string: does not match", "\t[\"[0-9]. [a-z]*\"]");
        }

        [Test]
        public void StringDoesNotMatchProperlyForNull()
        {
            Check.ThatCode(() => { Check.That((string) null).Matches("[0-9]. [a-z]*"); })
                .FailsWithMessage("", "The checked string is null.", "The expected string: matches", "\t[\"[0-9]. [a-z]*\"]");
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
                .FailsWithMessage("", "The checked string matches expected one, whereas it must not.", "The checked string:", "\t[\"12 ac\"]",
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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is not empty or null." + Environment.NewLine +
                             "The checked string:" + Environment.NewLine + "\t[\"test\"]");
        }

        [Test]
        public void IsEmptyFailsIfNnull()
        {
            Check.ThatCode(() => { Check.That((string) null).IsEmpty(); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is null instead of being empty.");
        }

        [Test]
        public void NegatedIsNotEmptyFailsIfNotEmpty()
        {
            Check.ThatCode(() => { Check.That("test").Not.IsNotEmpty(); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is not empty or null." + Environment.NewLine +
                             "The checked string:" + Environment.NewLine + "\t[\"test\"]");
        }

        [Test]
        public void IsNotEmptyFailsIfEmpty()
        {
            Check.ThatCode(() => { Check.That(string.Empty).IsNotEmpty(); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is empty, whereas it must not.");
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
            Check.That(sut).IsNullOrEmpty();
        }

        [Test]
        public void IsNullEmptyFailsIfNotEmpty()
        {
            Check.ThatCode(() => { Check.That("test").IsNullOrEmpty(); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is not empty or null." + Environment.NewLine +
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
        public void IsNullOrWhiteSpaceFaiplsWhenRelevant()
        {
            Check.ThatCode(() =>
                Check.That("non empty").IsNullOrWhiteSpace()).FailsWithMessage("", "The checked string contains non whitespace characters.", "The checked string:", "\t[\"non empty\"]");
        }

        [Test]
        public void
            IsNullOrWhiteSpaceWorksWhenNegated()
        {

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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is empty, whereas it must not.");
        }

        [Test]
        public void HasContentFailsIfNull()
        {
            Check.ThatCode(() => { Check.That((string) null).HasContent(); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is null whereas it must have content.");
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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string is different from expected one." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"test\"]" +
                             Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"TOAST\"]");
        }

        [Test]
        public void CompareNoCaseFailsWithStartOnly()
        {
            Check.ThatCode(() => { Check.That("test").IsEqualIgnoringCase("Te"); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string is different from expected one, it contains extra text at the end." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"test\"]" +
                             Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"Te\"]");
        }

        #endregion

        [Test]
        public void IsEqualToErrorMessageHighlightsWhiteSpacesAndTabsDifference()
        {
            var withWSp = "Hello    How are you?";
            var withTab = "Hello\tHow are you?";

            Check.ThatCode(() => { Check.That(withTab).IsEqualTo(withWSp); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string has different spaces than expected one." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"Hello<<tab>>How are you?\"]" + Environment.NewLine + "The expected string:" +
                             Environment.NewLine + "\t[\"Hello    How are you?\"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsAllWhiteSpacesAndTabsDifferences()
        {
            var withWSp = "Hello    How are you?    kiddo";
            var withTab = "Hello\tHow are you?\tkiddo";

            Check.ThatCode(() => { Check.That(withWSp).IsEqualTo(withTab); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine + "The checked string has different spaces than expected one." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"Hello    How are you?    kiddo\"]" + Environment.NewLine + "The expected string:" +
                             Environment.NewLine + "\t[\"Hello<<tab>>How are you?<<tab>>kiddo\"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsLineFeedAndCarriageReturnLineFeed()
        {
            var withCRLF = "Hello\r\nHow are you?";
            var withLF = "Hello\nHow are you?";

            Check.ThatCode(() => { Check.That(withCRLF).IsEqualTo(withLF); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string has different end of line markers than expected one." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "\t[\"Hello<<CRLF>>\"]" + Environment.NewLine + "The expected string:" +
                             Environment.NewLine + "\t[\"Hello<<LF>>\"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsOnlyTheFirstLineFeedAndCarriageReturnLineFeedDifference()
        {
            var withCRLF = "Hello\r\nHow are you?\r\nAre you kidding?";
            var withLF = "Hello\nHow are you?\nAre you kidding?";

            Check.ThatCode(() => { Check.That(withLF).IsEqualTo(withCRLF); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string has different end of line markers than expected one." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "	[\"Hello<<LF>>\"]" + Environment.NewLine +
                             "The expected string:" + Environment.NewLine +
                             "	[\"Hello<<CRLF>>\"]");
        }

        [Test]
        public void IsEqualToErrorMessageWhenVariousDifferences()
        {
            var withCRLF = "Hello\r\nHow are you?\r\nare you kidding?";
            var withLF = "Hello\nHow are you?\nAre you kidding?";

            Check.ThatCode(() => { Check.That(withLF).IsEqualTo(withCRLF); })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string is different from expected one." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "	[\"Hello<<LF>>\"]" + Environment.NewLine +
                             "The expected string:" + Environment.NewLine +
                             "	[\"Hello<<CRLF>>\"]");
        }

        [Test]
        public void ShouldDisplayFullLineIfItIsShort()
        {
            var multilineExpected = "Hello\nThis\nwill fail.";
            var multileActual = "Hello\nIt has\nfailed";

            Check.ThatCode(() => Check.That(multileActual).IsEqualTo(multilineExpected))
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine +
                             "The checked string is different from expected one. At line 2, col 1, expected 'This' was 'It has'." +
                             Environment.NewLine + "The checked string:" + Environment.NewLine +
                             "	[\"It has\"]" + Environment.NewLine +
                             "The expected string:" + Environment.NewLine +
                             "	[\"This\"]");
        }

        [Test]
        public void LongStringErrorMessageIsProperlyTruncated()
        {
            var checkString = File.ReadAllText(TestFiles.CheckedFile, Encoding.UTF8).Replace("\r\n", "");
            var expectedString = File.ReadAllText(TestFiles.ExpectedFile, Encoding.UTF8).Replace("\r\n", "");

            Check.ThatCode(() =>
                {
                    Check.That(checkString).IsEqualTo(expectedString);
                })
                .Throws<FluentCheckException>()
                .AndWhichMessage()
                .AsLines()
                .ContainsExactly("",
                    "The checked string is different from the expected one but has same length. At line 1, col 4554, expected '...IST>Joe Cooker</ARTI...' was '...IST>Joe Cocker</ARTI...'.",
                    "The checked string:",
                    "\t[\"<?xml version=\"1.0\" encoding=\"utf-8\" ?><!--  Edited by XMLSpy  --><CATALOG>  <CD>    <TITLE>Empire Burlesque</TITLE>    <ARTIST>Bob Dylan</ARTIST>    ...<<truncated>>...E>    <YEAR>1987</YEAR>  </CD></CATALOG>\"]",
                    "The expected string:",
                    "\t[\"<?xml version=\"1.0\" encoding=\"utf-8\" ?><!--  Edited by XMLSpy  --><CATALOG>  <CD>    <TITLE>Empire Burlesque</TITLE>    <ARTIST>Bob Dylan</ARTIST>    ...<<truncated>>...E>    <YEAR>1987</YEAR>  </CD></CATALOG>\"]"
                    );
        }

        [Test]
        public void UserCanControlTruncationLength()
        {
            var curLen = Check.StringTruncationLength;
            try
            {
                Check.StringTruncationLength = 10000;
                var checkString = File.ReadAllText(TestFiles.CheckedFile, Encoding.UTF8).Replace("\r\n", "");
                var expectedString = File.ReadAllText(TestFiles.ExpectedFile, Encoding.UTF8).Replace("\r\n", "");

                Check.ThatCode(() =>
                    {
                        Check.That(checkString).IsEqualTo(expectedString);
                    })
                    .Throws<FluentCheckException>()
                    .AndWhichMessage()
                    .AsLines().HasElementAt(3).Which.HasSize(4684);
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
                .Throws<FluentCheckException>()
                .AndWhichMessage().AsLines().ContainsExactly("",
                "The checked value is different from expected one, it contains extra lines at the end. At line 2, col 1, expected '' was 'And another.'.",
                "The checked value:",
                "\t[\"And another.\"]",
                "The expected value:",
                "\t[null]");
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
            Check.ThatCode(() =>
                {
                    Check.That("This is one line.\nAnd another line.").IsEqualTo("This is one line.\nAnd another");
                })
                .Throws<FluentCheckException>()
                .AndWhichMessage().AsLines().Contains(
                    "The checked string is different from expected one, one line is longer. At line 2, col 12, expected '...nd another' was '...nd another line.'.",
                "\t[\"And another\"]",
                "\t[\"And another line.\"]");
        }

        [Test]
        public void ShouldReportShorterLines()
        {
            Check.ThatCode(() =>
                {
                    Check.That("This is one line.\nAnd another").IsEqualTo("This is one line.\nAnd another line.");
                })
                .Throws<FluentCheckException>()
                .AndWhichMessage().AsLines().Contains(
                    "The checked string is different from expected one, one line is shorter. At line 2, col 12, expected '...nd another line.' was '...nd another'.",
                "\t[\"And another\"]",
                "\t[\"And another line.\"]");
        }

        [Test]
        public void ShouldReportMissingLines()
        {
            Check.ThatCode(() =>
                {
                    Check.That("This is one line.").IsEqualTo("This is one line.\nAnd another.");
                })
                .Throws<FluentCheckException>()
                .AndWhichMessage()
                .AsLines()
                .ContainsExactly("",
                    "The checked string is different from expected one, it is missing some line(s). At line 2, col 1, expected 'And another.' was ''.",
                    "The checked string:",
                    "\t[null]",
                    "The expected string:",
                    "\t[\"And another.\"]");
        }
    }
}