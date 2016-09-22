// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StringRelatedTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN, Cyrille DUPUYDAUBY
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

using NFluent.ApiChecks;
using NFluent.Tests.Helpers;

namespace NFluent.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using NFluent.Extensibility;
    using NUnit.Framework;

    [TestFixture]
    public class StringRelatedTests
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

            Check.That((string)null).Not.Contains("test");
        }

        [Test]
        public void ContainsFailsProperlyOnNullString()
        {
            Check.ThatCode(() =>
            {
                Check.That((string)null).Contains("fails", "anyway");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is null.\nThe expected substring(s):\n\t[\"fails\", \"anyway\"]");
        }

        [Test]
        public void ContainsIsCaseSensitive()
        {
            Check.ThatCode(() =>
            {
                Check.That(Alphabet).Contains("C", "a", "A", "z");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string does not contains the expected value(s): \"C\", \"A\"\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected substring(s):\n\t[\"C\", \"a\", \"A\", \"z\"]");
        }

        [Test]
        public void ContainsThrowsExceptionWhenFails()
        {
            Check.ThatCode(() =>
            {
                Check.That(Alphabet).Contains("c", "0", "4");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string does not contains the expected value(s): \"0\", \"4\"\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected substring(s):\n\t[\"c\", \"0\", \"4\"]");
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
            Check.That((string)null).DoesNotContain("fails", "anyway");
        }

        [Test]
        public void DoesNotContainsFailsWhenAppropriate()
        {
            Check.ThatCode(() =>
            {
                Check.That(Alphabet).DoesNotContain("c", "z", "u");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string contains unauthorized value(s): \"c\", \"z\", \"u\"\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe unauthorized substring(s):\n\t[\"c\", \"z\", \"u\"]");
        }

        [Test]
        public void ContainsOnceWorksWithString()
        {
            Check.That(Alphabet).Contains("lmnop").Once();
        }

        [Test]
        public void ContainsOnceFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.That("abcdefghijklmnopqrstuvwxylmnopz").Contains("lmnop").Once();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string contains \"lmnop\" at 11 and 25, where as it must contains it once.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxylmnopz\"]\nThe expected string: one\n\t[\"lmnop\"]");
        }

        [Test]
        public void ContainsInThatOrderWorksWithString()
        {
            Check.That(Alphabet).Contains("ab", "cd").InThatOrder();
        }

        [Test]
        public void ContainsInThatOrderFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.That(Alphabet).Contains("cd", "ab").InThatOrder();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string does not contain the expected strings in the correct order.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nExpected content: \n\t[\"cd\", \"ab\"]");
        }

        [Test]
        public void StartWithIsCaseSensitive()
        {
            Check.ThatCode(() =>
            {
                Check.That(Alphabet).StartsWith("ABCDEF");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string's start is different from the expected one.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: starts with\n\t[\"ABCDEF\"]");
        }

        [Test]
        public void StartsWithFailsProperlyOnNullString()
        {
            Check.ThatCode(() =>
            {
                Check.That((string)null).StartsWith("fails");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is null.\nThe expected string: starts with\n\t[\"fails\"]");
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
            Check.ThatCode(() =>
            {
                Check.That(Alphabet).Not.StartsWith("abcdef");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string starts with expected one, whereas it must not.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: does not start with\n\t[\"abcdef\"]");
        }

        [Test]
        public void NegatedStartsWithWorks()
        {
            Check.That("test").Not.StartsWith("Toto");
            Check.That((string)null).Not.StartsWith("Toto");
        }

        [Test]
        public void EndsWithIsCaseSensitive()
        {
            Check.ThatCode(() =>
            {
                Check.That(Alphabet).EndsWith("UWXYZ");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string's end is different from the expected one.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: ends with\n\t[\"UWXYZ\"]");
        }

        [Test]
        public void EndsWithFailsProperlyOnNullString()
        {
            Check.ThatCode(() =>
            {
                Check.That((string)null).EndsWith("fails");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is null.\nThe expected string: ends with\n\t[\"fails\"]");
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
            Check.That((string)null).Not.EndsWith("test");
        }

        [Test]
        public void EndsWithIsNegatableFails()
        {
            Check.ThatCode(() =>
            {
                Check.That(Alphabet).Not.EndsWith("vwxyz");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string ends with expected one, whereas it must not.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: does not end with\n\t[\"vwxyz\"]");
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

            Check.ThatCode(() =>
            {
                Check.That(check).IsEqualTo("tutu");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one but has same length. At 1, expected 'tutu' was 'toto'\nThe checked string:\n\t[\"toto\"]\nThe expected string:\n\t[\"tutu\"]");
        }

        [Test]
        public void EqualFailsWithDiffCase()
        {
            var check = "toto";

            Check.ThatCode(() =>
            {
                Check.That(check).IsEqualTo("TOTO");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one but only in case. At 0, expected 'TOTO' was 'toto'\nThe checked string:\n\t[\"toto\"]\nThe expected string:\n\t[\"TOTO\"]");
        }

        [Test]
        public void EqualFailsInGeneral()
        {
            var check = "toto";

            Check.ThatCode(() =>
            {
                Check.That(check).IsEqualTo("tititutu");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one.\nThe checked string:\n\t[\"toto\"]\nThe expected string:\n\t[\"tititutu\"]");
        }

        [Test]
        public void EqualFailshWhenShorter()
        {
            var check = "titi";

            Check.ThatCode(() =>
            {
                Check.That(check).IsEqualTo("tititutu");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one, it is missing the end.\nThe checked string:\n\t[\"titi\"]\nThe expected string:\n\t[\"tititutu\"]");
        }

        [Test]
        public void EqualFailshWhenStartSame()
        {
            var check = "tititutu";

            Check.ThatCode(() =>
            {
                Check.That(check).IsEqualTo("titi");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one, it contains extra text at the end.\nThe checked string:\n\t[\"tititutu\"]\nThe expected string:\n\t[\"titi\"]");
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionsOnString()
        {
            Check.That(Alphabet).Contains("i").And.StartsWith("abcd").And.IsInstanceOf<string>().And.IsNotInstanceOf<int>().And.Not.IsNotInstanceOf<string>();
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
            .WithMessage("\nThe checked string is not one of the possible elements.\nThe checked string:\n\t[\"The Black Keys\"]\nThe possible elements:\n\t[\"Paco de Lucia\", \"Jimi Hendrix\", \"Baden Powell\"]");
        }

        [Test]
        public void NotIsOneOfTheseWorks()
        {
            Check.That("The Black Keys").Not.IsOneOfThese("Paco de Lucia", "Jimi Hendrix", "Baden Powell");
        }

        [Test]
        public void IsOneOfTheseWorksWithNull()
        {
            string nullString = null;
            Check.That(nullString).IsOneOfThese(null);
        }

        [Test]
        public void IsOneOfTheseThrowsProperExceptionWithNullAsExpectedValues()
        {
            Check.ThatCode(() =>
            {
                Check.That("whatever").IsOneOfThese(null);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not one of the possible elements.\nThe checked value:\n\t[\"whatever\"]\nThe possible elements:\n\t[null]");
        }

        [Test]
        public void NotIsOneOfTheseThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That("The Black Keys").Not.IsOneOfThese("Metronomy", "Sigur Ros", "The Black Keys", "Get Well Soon");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is one of the possible elements whereas it must not.\nThe checked string:\n\t[\"The Black Keys\"]\nThe possible elements:\n\t[\"Metronomy\", \"Sigur Ros\", \"The Black Keys\", \"Get Well Soon\"]");
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
            Check.ThatCode(() =>
            {
                Check.That("AC 12").Matches("[0-9]. [a-z]*");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string does not match the expected one.\nThe checked string:\n\t[\"AC 12\"]\nThe expected string: matches\n\t[\"[0-9]. [a-z]*\"]");
        }

        [Test]
        public void StringMatchesFailsProperlyForNull()
        {
            Check.ThatCode(() =>
            {
                Check.That((string)null).Matches("[0-9]. [a-z]*");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is null.\nThe expected string: matches\n\t[\"[0-9]. [a-z]*\"]");
        }

        [Test]
        public void NotStringMatchesWorks()
        {
            Check.That("AC 12").Not.Matches("[0-9]. [a-z]*");
            Check.That((string)null).Not.Matches("[0-9]. [a-z]*");
        }

        [Test]
        public void NotStringMatchesFails()
        {
            Check.ThatCode(() =>
            {
                Check.That("12 ac").Not.Matches("[0-9]*. [a-z]*");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string matches expected one, whereas it must not.\nThe checked string:\n\t[\"12 ac\"]\nThe expected string: does not match\n\t[\"[0-9]*. [a-z]*\"]");
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
            Check.ThatCode(() =>
            {
                Check.That("12 AC").DoesNotMatch("[0-9]. [a-z]*");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string matches expected one, whereas it must not.\nThe checked string:\n\t[\"12 AC\"]\nThe expected string: does not match\n\t[\"[0-9]. [a-z]*\"]");
        }

        [Test]
        public void StringDoesNotMatchProperlyForNull()
        {
            Check.ThatCode(() =>
            {
                Check.That((string)null).Matches("[0-9]. [a-z]*");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is null.\nThe expected string: matches\n\t[\"[0-9]. [a-z]*\"]");
        }

        [Test]
        public void NotStringDoesNotMatchWorks()
        {
            Check.That("AC 12").Not.Matches("[0-9]. [a-z]*");
            Check.That((string)null).Not.Matches("[0-9]. [a-z]*");
        }

        [Test]
        public void NotStringDoesNotMatchFails()
        {
            Check.ThatCode(() =>
            {
                Check.That("12 ac").Not.Matches("[0-9]*. [a-z]*");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string matches expected one, whereas it must not.\nThe checked string:\n\t[\"12 ac\"]\nThe expected string: does not match\n\t[\"[0-9]*. [a-z]*\"]");
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
            Check.ThatCode(() =>
            {
                Check.That("test").IsEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is not empty or null.\nThe checked string:\n\t[\"test\"]");
        }

        [Test]
        public void IsEmptyFailsIfNnull()
        {
            Check.ThatCode(() =>
            {
                Check.That((string)null).IsEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is null instead of being empty.");
        }

        [Test]
        public void NegatedIsNotEmptyFailsIfNotEmpty()
        {
            Check.ThatCode(() =>
            {
                Check.That("test").Not.IsNotEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is not empty or null.\nThe checked string:\n\t[\"test\"]");
        }

        [Test]
        public void IsNotEmptyFailsIfEmpty()
        {
            Check.ThatCode(() =>
            {
                Check.That(string.Empty).IsNotEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is empty, whereas it must not.");
        }

        [Test]
        public void NegatedIsNotEmptyFailsIfNull()
        {
            Check.ThatCode(() =>
            {
                Check.That((string)null).Not.IsNotEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is null instead of being empty.");
        }

        [Test]
        public void IsNullOrEmptyWorks()
        {
            Check.That(string.Empty).IsNullOrEmpty();
            Check.That((string)null).IsNullOrEmpty();
        }

        [Test]
        public void IsNullEmptyFailsIfNotEmpty()
        {
            Check.ThatCode(() =>
            {
                Check.That("test").IsNullOrEmpty();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is not empty or null.\nThe checked string:\n\t[\"test\"]");
        }

        [Test]
        public void HasContentWorks()
        {
            Check.That("test").HasContent();
        }

        [Test]
        public void HasContentFailsIfEmpty()
        {
            Check.ThatCode(() =>
            {
                Check.That(string.Empty).HasContent();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is empty, whereas it must not.");
        }

        [Test]
        public void HasContentFailsIfNull()
        {
            Check.ThatCode(() =>
            {
                Check.That((string)null).HasContent();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is null whereas it must have content.");
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
            Check.ThatCode(() =>
            {
                Check.That("test").IsEqualIgnoringCase("TOAST");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one.\nThe checked string:\n\t[\"test\"]\nThe expected string:\n\t[\"TOAST\"]");
        }

        [Test]
        public void CompareNoCaseFailsWithStartOnly()
        {
            Check.ThatCode(() =>
            {
                Check.That("test").IsEqualIgnoringCase("Te");
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one, it contains extra text at the end.\nThe checked string:\n\t[\"test\"]\nThe expected string:\n\t[\"Te\"]");
        }

        #endregion

        [Test]
        public void IsEqualToErrorMessageHighlightsWhiteSpacesAndTabsDifference()
        {
            string withWSp = "Hello    How are you?";
            string withTab = "Hello\tHow are you?";

            Check.ThatCode(() =>
            {
                Check.That(withTab).IsEqualTo(withWSp);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one.\nThe checked string:\n\t[\"Hello<<tab>>How are you?\"]\nThe expected string:\n\t[\"Hello    How are you?\"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsAllWhiteSpacesAndTabsDifferences()
        {
            string withWSp = "Hello    How are you?    kiddo";
            string withTab = "Hello\tHow are you?\tkiddo";

            Check.ThatCode(() =>
            {
                Check.That(withWSp).IsEqualTo(withTab);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one.\nThe checked string:\n\t[\"Hello    How are you?    kiddo\"]\nThe expected string:\n\t[\"Hello<<tab>>How are you?<<tab>>kiddo\"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsLineFeedAndCarriageReturnLineFeed()
        {
            string withCRLF = "Hello\r\nHow are you?";
            string withLF = "Hello\nHow are you?";

            Check.ThatCode(() =>
            {
                Check.That(withCRLF).IsEqualTo(withLF);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one.\nThe checked string:\n	[\"Hello<<CRLF>>\r\nHow are you?\"]\nThe expected string:\n	[\"Hello<<LF>>\nHow are you?\"]");
        }

        [Test]
        public void IsEqualToErrorMessageHighlightsOnlyTheFirstLineFeedAndCarriageReturnLineFeedDifference()
        {
            string withCRLF = "Hello\r\nHow are you?\r\nAre you kidding?";
            string withLF = "Hello\nHow are you?\nAre you kidding?";

            Check.ThatCode(() =>
            {
                Check.That(withLF).IsEqualTo(withCRLF);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked string is different from expected one.\nThe checked string:\n	[\"Hello<<LF>>\nHow are you?\nAre you kidding?\"]\nThe expected string:\n	[\"Hello<<CRLF>>\r\nHow are you?\r\nAre you kidding?\"]");
        }

        [Test]
        public void LongStringErrorMessageIsProperlyTruncated()
        {
            var checkString = File.ReadAllText(TestFiles.CheckedFile, Encoding.UTF8);
            var expectedString = File.ReadAllText(TestFiles.ExpectedFile, Encoding.UTF8);

            Check.ThatCode(() =>
            {
                Check.That(checkString).IsEqualTo(expectedString);
                // ReverseEngineeringExceptionMessagesHelper.DumpReadyToCopyAndPasteExceptionMessageInAFile(() => Check.That(checkString).IsEqualTo(expectedString));
            })
            .Throws<FluentCheckException>().AndWhichMessage()
            .IsEqualTo("\nThe checked string is different from expected one but has same length. At 4963, expected '...IST>Joe Cooker</ARTI...' was '...IST>Joe Cocker</ARTI...'\nThe checked string:\n\t[\"<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<!--  Edited by XMLSpy  -->\r\n<CATALOG>\r\n  <CD>\r\n    <TITLE>Empire Burlesque</TITLE>\r\n    <ARTIST>Bob Dylan</A...<<truncated>>...  <YEAR>1987</YEAR>\r\n  </CD>\r\n</CATALOG>\"]\nThe expected string:\n\t[\"<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<!--  Edited by XMLSpy  -->\r\n<CATALOG>\r\n  <CD>\r\n    <TITLE>Empire Burlesque</TITLE>\r\n    <ARTIST>Bob Dylan</A...<<truncated>>...  <YEAR>1987</YEAR>\r\n  </CD>\r\n</CATALOG>\"]");

        }
    }
}