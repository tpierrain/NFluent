// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="CharTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
    public class CharTests
    {
        #region IsInstanceOf ...

        [Test]
        public void IsInstanceOfWorks()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).IsInstanceOf<char>();
        }

        [Test]
        public void NotIsInstanceOfWorks()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).Not.IsInstanceOf<string>();
        }

        [Test]
        public void IsNotInstanceOfWorks()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).IsNotInstanceOf<string>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [char] whereas it must not.\nThe checked value:\n\t['a'] of type: [char]\nThe expected value: different from\n\tan instance of type: [char]")]
        public void NotIsInstanceOfThrows()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).Not.IsInstanceOf<char>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [char] whereas it must not.\nThe checked value:\n\t['a'] of type: [char]\nThe expected value: different from\n\tan instance of type: [char]")]
        public void IsNotInstanceOfThrows()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).IsNotInstanceOf<char>();
        }

        #endregion
        
        #region IsEqualTo ...

        [Test]
        public void IsEqualToWorks()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).IsEqualTo('a');
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is different from the expected one.\nThe checked char:\n\t['a']\nThe expected char:\n\t['b']")]
        public void IsEqualToThrowsWithAnotherChar()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).IsEqualTo('b');
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is different from the expected one.\nThe checked char:\n\t['a']\nThe expected char:\n\t['A']")]
        public void IsEqualToThrowsWithSameCharWithDifferentCase()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).IsEqualTo('A');
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is different from the expected string.\nThe checked char:\n\t['a'] of type: [char]\nThe expected string:\n\t[\"a\"] of type: [string]")]
        public void ACharIsNotEqualToTheSameCharAsString()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).IsEqualTo("a");
        }

        [Test]
        public void ACharIsIndeedNotEqualToTheSameCharAsString()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).Not.IsEqualTo("a");
        }

        [Test]
        public void ACharIsIndeedNotEqualToTheSameCharAsString2()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).IsNotEqualTo("a");
        }

        #endregion

        #region IsSameLetterButWithDifferentCaseAs

        [Test]
        public void IsSameLetterWithDifferentCaseWorks()
        {
            const char FirstLetterLowerCase = 'a';
            Check.That(FirstLetterLowerCase).IsSameLetterButWithDifferentCaseAs('A');
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is the same letter as the given one but with different case, whereas it must not.\nThe checked char:\n\t['A']\nThe given char:\n\t['a']")]
        public void NotIsSameLetterWithDifferentCaseThrows()
        {
            const char FirstLetterLowerCase = 'A';
            Check.That(FirstLetterLowerCase).Not.IsSameLetterButWithDifferentCaseAs('a');
        }

        [Test]
        public void NotIsSameLetterWithDifferentCaseWorks()
        {
            const char DotCharacter = '.';
            Check.That(DotCharacter).Not.IsSameLetterButWithDifferentCaseAs('.');

            const char LowerCasedChar = 'a';
            Check.That(LowerCasedChar).Not.IsSameLetterButWithDifferentCaseAs('a');
        }

        [Test]
        public void NotIsSameLetterWithDifferentCaseWorksWithADifferentLetterCasedDifferently()
        {
            const char UpperCasedChar = 'Z';
            Check.That(UpperCasedChar).Not.IsSameLetterButWithDifferentCaseAs('y');
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is not the same letter but with different case as the given one.\nThe checked char:\n\t['a']\nThe given char:\n\t['a']")]
        public void IsSameLetterWithDifferentCaseThrowsWithSameCharWithSameCase()
        {
            const char LowerCasedChar = 'a';
            Check.That(LowerCasedChar).IsSameLetterButWithDifferentCaseAs('a');
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is not the same letter but with different case as the given one.\nThe checked char:\n\t['a']\nThe given char:\n\t['b']")]
        public void IsSameLetterWithDifferentCaseThrowsWithAnotherChar()
        {
            const char LowerCasedChar = 'a';
            Check.That(LowerCasedChar).IsSameLetterButWithDifferentCaseAs('b');
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is not the same letter but with different case as the given one.\nThe checked char:\n\t['a']\nThe given char:\n\t['B']")]
        public void IsSameLetterWithDifferentCaseThrowsWithAnotherCharCasedDifferently()
        {
            const char LowerCasedChar = 'a';
            Check.That(LowerCasedChar).IsSameLetterButWithDifferentCaseAs('B');
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is not the same letter but with different case as the given one.\nThe checked char:\n\t['.']\nThe given char:\n\t['.']")]
        public void IsSameLetterWithDifferentCaseThrowsWithNonLetter()
        {
            const char NonLetterChar = '.';
            Check.That(NonLetterChar).IsSameLetterButWithDifferentCaseAs('.');
        }

        #endregion

        #region IsSameLetterAs

        [Test]
        public void IsSameLetterWorksAlsoWhenCaseAreDifferent()
        {
            const char LowerCasedZ = 'z';
            const char UpperCasedZ = 'Z';
            
            Check.That(UpperCasedZ).IsSameLetterAs(LowerCasedZ);
            Check.That(UpperCasedZ).IsSameLetterAs(UpperCasedZ);
            Check.That(LowerCasedZ).IsSameLetterAs(LowerCasedZ);
        }

        [Test]
        public void NotIsSameLetterWorks()
        {
            const char FirstLetter = 'a';
            const char LatestLetter = 'z';

            Check.That(FirstLetter).Not.IsSameLetterAs(LatestLetter);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is not the same letter as the given one (whatever the case).\nThe checked char:\n\t['a']\nThe given char:\n\t['z']")]
        public void IsSameLetterThrowsWithDifferentLetters()
        {
            const char LowerCasedA = 'a';
            Check.That(LowerCasedA).IsSameLetterAs('z');
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is not the same letter as the given one (whatever the case).\nThe checked char is not even a letter!\nThe checked char:\n\t['/']\nThe given char:\n\t['/']")]
        public void IsSameLetterThrowsWithNonLetterChar()
        {
            const char NonLetterChar = '/';
            Check.That(NonLetterChar).IsSameLetterAs('/');
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is the same letter as the given one (whatever the case), whereas it must not.\nThe checked char:\n\t['a']\nThe given char:\n\t['A']")]
        public void NotIsSameLetterThrows()
        {
            const char LowerCasedA = 'a';
            Check.That(LowerCasedA).Not.IsSameLetterAs('A');
        }

        #endregion

        #region IsALetter

        [Test]
        public void IsALetterWorks()
        {
            const char LowerCasedLetter = 'q';
            Check.That(LowerCasedLetter).IsALetter();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is not a letter.\nThe checked char:\n\t['/']")]
        public void IsALetterThrowsWithNonLetterChar()
        {
            const char NonLetterChar = '/';
            Check.That(NonLetterChar).IsALetter();
        }

        [Test]
        public void NotIsALetterWorks()
        {
            char nonLetterChar = '.';
            Check.That(nonLetterChar).Not.IsALetter();

            nonLetterChar = ' ';
            Check.That(nonLetterChar).Not.IsALetter();

            nonLetterChar = '-';
            Check.That(nonLetterChar).Not.IsALetter();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is a letter whereas it must not.\nThe checked char:\n\t['a']")]
        public void NotIsALetterThrows()
        {
            const char LowerCasedLetter = 'a';
            Check.That(LowerCasedLetter).Not.IsALetter();
        }

        #endregion

        #region IsADigit

        [Test]
        public void IsADigitWorks()
        {
            const char DigitChar = '2';
            Check.That(DigitChar).IsADigit();
        }

        [Test]
        public void NotIsADigitWorks()
        {
            const char Letter = 'a';
            Check.That(Letter).Not.IsADigit();

            const char Punctuation = '-';
            Check.That(Punctuation).Not.IsADigit();

            const char Space = ' ';
            Check.That(Space).Not.IsADigit();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is not a decimal digit.\nThe checked char:\n\t['Z']")]
        public void IsADigitThrows()
        {
            const char Letter = 'Z';
            Check.That(Letter).IsADigit();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is a decimal digit whereas it must not.\nThe checked char:\n\t['2']")]
        public void NotIsADigitThrows()
        {
            const char DigitChar = '2';
            Check.That(DigitChar).Not.IsADigit();
        }

        #endregion

        #region IsAPunctuationMark

        [Test]
        public void IsPunctuationWorks()
        {
            char punctuation = '-';
            Check.That(punctuation).IsAPunctuationMark();

            punctuation = '/';
            Check.That(punctuation).IsAPunctuationMark();
        }

        [Test]
        public void NotIsPunctuationWorks()
        {
            const char DigitChar = '2';
            Check.That(DigitChar).Not.IsAPunctuationMark();

            const char Letter = 'Z';
            Check.That(Letter).Not.IsAPunctuationMark();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is not a punctuation mark.\nThe checked char:\n\t['2']")]
        public void IsPunctuationThrows()
        {
            const char DigitChar = '2';
            Check.That(DigitChar).IsAPunctuationMark();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked char is a punctuation mark whereas it must not.\nThe checked char:\n\t['-']")]
        public void NotIsPunctuationThrows()
        {
            char punctuation = '-';
            Check.That(punctuation).Not.IsAPunctuationMark();
        }

        #endregion
    }
}
