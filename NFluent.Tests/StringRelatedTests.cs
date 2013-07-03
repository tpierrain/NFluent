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
namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class StringRelatedTests
    {
        #region Public Methods and Operators

        [Test]
        public void ContainsWorksWithString()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("lmnop");
        }

        [Test]
        public void ContainsWithStringIsNegatable()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.Contains("1234");

            Check.That((string)null).Not.Contains("test");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is null.\nThe expected substring(s):\n\t[\"fails\", \"anyway\"]")]
        public void ContainsFailsProperlyOnNullString()
        {
            Check.That((string)null).Contains("fails", "anyway");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string does not contains the expected value(s): \"C\", \"A\"\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected substring(s):\n\t[\"C\", \"a\", \"A\", \"z\"]")]
        public void ContainsIsCaseSensitive()
        {
            const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(Alphabet).Contains("C", "a", "A", "z");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string does not contains the expected value(s): \"0\", \"4\"\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected substring(s):\n\t[\"c\", \"0\", \"4\"]")]
        public void ContainsThrowsExceptionWhenFails()
        {
            const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(Alphabet).Contains("c", "0", "4");
        }

        [Test]
        public void ContainsWorks()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("c", "z", "u");
        }

        [Test]
        public void DoesNotContainsWorks()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).DoesNotContain("one", "two", "three");
            Check.That((string)null).DoesNotContain("fails", "anyway");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string contains unauthorized value(s): \"c\", \"z\", \"u\"\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe unauthorized substring(s):\n\t[\"c\", \"z\", \"u\"]")]
        public void DoesNotContainsFailsWhenAppropriate()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).DoesNotContain("c", "z", "u");
        }

        [Test]
        public void ContainsOnceWorksWithString()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("lmnop").Once();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string contains \"lmnop\" at 11 and 25, where as it must contains it once.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxylmnopz\"]\nExpected content once\n\t[\"lmnop\"]")]
        public void ContainsOnceFailsProperly()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxylmnopz";
            Check.That(alphabet).Contains("lmnop").Once();
        }

        [Test]
        public void ContainsInThatOrderWorksWithString()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("ab", "cd").InThatOrder();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string does not contain the expected strings in the correct order.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxylmnopz\"]\nExpected content: \n\t[\"cd\", \"ab\"]")]
        public void ContainsInThatOrderFailsProperly()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxylmnopz";
            Check.That(alphabet).Contains("cd", "ab").InThatOrder();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string's start is different from the expected one.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: starts with\n\t[\"ABCDEF\"]")]
        public void StartWithIsCaseSensitive()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).StartsWith("ABCDEF");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is null.\nThe expected string: starts with\n\t[\"fails\"]")]
        public void StartsWithFailsProperlyOnNullString()
        {
            Check.That((string)null).StartsWith("fails");
        }

        [Test]
        public void StartWithWorks()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).StartsWith("abcdef");
        }

        [Test]
        public void StartWithIsNegatable()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.StartsWith("hehehe");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string starts with expected one, whereas it must not.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: does not start with\n\t[\"abcdef\"]")]
        public void NegatedStartWithThrowsException()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.StartsWith("abcdef");
        }

        [Test]
        public void NegatedStartsWithWorks()
        {
            Check.That("test").Not.StartsWith("Toto");
            Check.That((string)null).Not.StartsWith("Toto");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string's end is different from the expected one.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: ends with\n\t[\"UWXYZ\"]")]
        public void EndsWithIsCaseSensitive()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).EndsWith("UWXYZ");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is null.\nThe expected string: ends with\n\t[\"fails\"]")]
        public void EndsWithFailsProperlyOnNullString()
        {
            Check.That((string)null).EndsWith("fails");
        }

        [Test]
        public void EndsWithWorks()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).EndsWith("uvwxyz");
        }

        [Test]
        public void EndsWithIsNegatable()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.EndsWith("hehehe");
            Check.That((string)null).Not.EndsWith("test");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string ends with expected one, whereas it must not.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: does not end with\n\t[\"vwxyz\"]")]
        public void EndsWithIsNegatableFails()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.EndsWith("vwxyz");
        }

        [Test]
        public void EqualWorks()
        {
            var check = "toto";
            Check.That(check).IsEqualTo("toto");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from the expected one but has same length.\nThe checked string:\n\t[\"toto\"]\nThe expected string:\n\t[\"tutu\"]")]
        public void EqualFailsWithSameLength()
        {
            var check = "toto";
            Check.That(check).IsEqualTo("tutu");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from the expected one but only in case.\nThe checked string:\n\t[\"toto\"]\nThe expected string:\n\t[\"TOTO\"]")]
        public void EqualFailsWithDiffCase()
        {
            var check = "toto";
            Check.That(check).IsEqualTo("TOTO");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from expected one.\nThe checked string:\n\t[\"toto\"]\nThe expected string:\n\t[\"tititutu\"]")]
        public void EqualFailsInGeneral()
        {
            var check = "toto";
            Check.That(check).IsEqualTo("tititutu");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from expected one, it is missing the end.\nThe checked string:\n\t[\"titi\"]\nThe expected string:\n\t[\"tititutu\"]")]
        public void EqualFailshWhenShorter()
        {
            var check = "titi";
            Check.That(check).IsEqualTo("tititutu");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from expected one, it contains extra text at the end.\nThe checked string:\n\t[\"tititutu\"]\nThe expected string:\n\t[\"titi\"]")]
        public void EqualFailshWhenStartSame()
        {
            var check = "tititutu";
            Check.That(check).IsEqualTo("titi");
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionsOnString()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("i").And.StartsWith("abcd").And.IsInstanceOf<string>().And.IsNotInstanceOf<int>().And.Not.IsNotInstanceOf<string>();
            Check.That(alphabet).HasSize(26);
        }

        [Test]
        public void HasSizeTest()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).HasSize(26);
        }

        #region Match
        [Test]
        public void StringMatchesWorks()
        {
            Check.That("12 ac").Matches("[0-9]*. [a-z]*");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string does not match the expected one.\nThe checked string:\n\t[\"AC 12\"]\nThe expected string: matches\n\t[\"[0-9]. [a-z]*\"]")]
        public void StringMatchesFails()
        {
            Check.That("AC 12").Matches("[0-9]. [a-z]*");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is null.\nThe expected string: matches\n\t[\"[0-9]. [a-z]*\"]")]
        public void StringMatchesFailsProperlyForNull()
        {
            Check.That((string)null).Matches("[0-9]. [a-z]*");
        }

        [Test]
        public void NotStringMatchesWorks()
        {
            Check.That("AC 12").Not.Matches("[0-9]. [a-z]*");
            Check.That((string)null).Not.Matches("[0-9]. [a-z]*");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string matches expected one, whereas it must not.\nThe checked string:\n\t[\"12 ac\"]\nThe expected string: does not match\n\t[\"[0-9]*. [a-z]*\"]")]
        public void NotStringMatchesFails()
        {
            Check.That("12 ac").Not.Matches("[0-9]*. [a-z]*");
        }
 
        #endregion

        #region Match
        [Test]
        public void StringDoesNotMatchWorks()
        {
            Check.That("ac 12").DoesNotMatch("[0-9]. [a-z]*");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string matches expected one, whereas it must not.\nThe checked string:\n\t[\"12 AC\"]\nThe expected string: does not match\n\t[\"[0-9]. [a-z]*\"]")]
        public void StringDoesNotMatchFails()
        {
            Check.That("12 AC").DoesNotMatch("[0-9]. [a-z]*");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is null.\nThe expected string: matches\n\t[\"[0-9]. [a-z]*\"]")]
        public void StringDoesNotMatchProperlyForNull()
        {
            Check.That((string)null).Matches("[0-9]. [a-z]*");
        }

        [Test]
        public void NotStringDoesNotMatchWorks()
        {
            Check.That("AC 12").Not.Matches("[0-9]. [a-z]*");
            Check.That((string)null).Not.Matches("[0-9]. [a-z]*");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string matches expected one, whereas it must not.\nThe checked string:\n\t[\"12 ac\"]\nThe expected string: does not match\n\t[\"[0-9]*. [a-z]*\"]")]
        public void NotStringDoesNotMatchFails()
        {
            Check.That("12 ac").Not.Matches("[0-9]*. [a-z]*");
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is not empty or null.\nThe checked string:\n\t[\"test\"]")]
        public void IsEmptyFailsIfNotEmpty()
        {
            Check.That("test").IsEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is null instead of being empty.")]
        public void IsEmptyFailsIfNnull()
        {
            Check.That((string)null).IsEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is not empty or null.\nThe checked string:\n\t[\"test\"]")]
        public void NegatedIsNotEmptyFailsIfNotEmpty()
        {
            Check.That("test").Not.IsNotEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is empty, whereas it must not.")]
        public void IsNotEmptyFailsIfEmpty()
        {
            Check.That(string.Empty).IsNotEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is null instead of being empty.")]
        public void NegatedIsNotEmptyFailsIfNull()
        {
            Check.That((string)null).Not.IsNotEmpty();
        }

        [Test]
        public void IsNullOrEmptyWorks()
        {
            Check.That(string.Empty).IsNullOrEmpty();
            Check.That((string)null).IsNullOrEmpty();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is not empty or null.\nThe checked string:\n\t[\"test\"]")]
        public void IsNullEmptyFailsIfNotEmpty()
        {
            Check.That("test").IsNullOrEmpty();
        }

        [Test]
        public void HasContentWorks()
        {
            Check.That("test").HasContent();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is empty, whereas it must not.")]
        public void HasContentFailsIfEmpty()
        {
            Check.That(string.Empty).HasContent();
        }
        
        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is null whereas it must have content.")]
        public void HasContentFailsIfNull()
        {
            Check.That((string)null).HasContent();
        }

        [Test]
        public void CompareNoCaseWorks()
        {
            Check.That("test").IsEqualIgnoringCase("TEST");
            Check.That("tESt").IsEqualIgnoringCase("TEst");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from expected one.\nThe checked string:\n\t[\"test\"]\nThe expected string:\n\t[\"TOAST\"]")]
        public void CompareNoCaseFails()
        {
            Check.That("test").IsEqualIgnoringCase("TOAST");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from expected one, it contains extra text at the end.\nThe checked string:\n\t[\"test\"]\nThe expected string:\n\t[\"Te\"]")]
        public void CompareNoCaseFailsWithStartOnly()
        {
            Check.That("test").IsEqualIgnoringCase("Te");
        }

        #endregion
    }
}