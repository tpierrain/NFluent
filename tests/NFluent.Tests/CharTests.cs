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
using System;

namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class CharTests
    {
        private const char FirstLetterLowerCase = 'a';

        #region IsInstanceOf ...

        [Test]
        public void IsInstanceOfWorks()
        {
            Check.That(FirstLetterLowerCase).IsInstanceOf<char>();
        }

        [Test]
        public void NotIsInstanceOfWorks()
        {
            Check.That(FirstLetterLowerCase).Not.IsInstanceOf<string>();
        }

        [Test]
        public void IsNotInstanceOfWorks()
        {
            Check.That(FirstLetterLowerCase).IsNotInstanceOf<string>();
        }

        [Test]
        public void NotIsInstanceOfThrows()
        {
            Check.ThatCode(() => Check.That(FirstLetterLowerCase).Not.IsInstanceOf<char>())
                    .Throws<FluentCheckException>()
                    .WithMessage(Environment.NewLine+ "The checked value is an instance of [char] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t['a'] of type: [char]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [char]");

        }

        [Test]
        public void IsNotInstanceOfThrows()
        {
            Check.ThatCode(() => Check.That(FirstLetterLowerCase).IsNotInstanceOf<char>())
                    .Throws<FluentCheckException>()
                    .WithMessage(Environment.NewLine+ "The checked value is an instance of [char] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t['a'] of type: [char]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [char]");
        }

        #endregion

        #region IsEqualTo ...

        [Test]
        public void IsEqualToWorks()
        {
            Check.That(FirstLetterLowerCase).IsEqualTo('a');
        }

        [Test]
        public void IsEqualToThrowsWithAnotherChar()
        {
            Check.ThatCode(() =>
            {
                Check.That(FirstLetterLowerCase).IsEqualTo('b');
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is different from the expected one." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['a']" + Environment.NewLine + "The expected char:" + Environment.NewLine + "\t['b']");
        }

        [Test]
        public void IsEqualToThrowsWithSameCharWithDifferentCase()
        {
            Check.ThatCode(() =>
            {
                Check.That(FirstLetterLowerCase).IsEqualTo('A');
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is different from the expected one." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['a']" + Environment.NewLine + "The expected char:" + Environment.NewLine + "\t['A']");
        }

        [Test]
        public void ACharIsNotEqualToTheSameCharAsString()
        {
            Check.ThatCode(() =>
            {
                Check.That(FirstLetterLowerCase).IsEqualTo("a");
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is different from the expected string." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['a'] of type: [char]" + Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"a\"] of type: [string]");
        }

        [Test]
        public void ACharIsIndeedNotEqualToTheSameCharAsString()
        {
            Check.That(FirstLetterLowerCase).Not.IsEqualTo("a");
        }

        [Test]
        public void ACharIsIndeedNotEqualToTheSameCharAsString2()
        {
            Check.That(FirstLetterLowerCase).IsNotEqualTo("a");
        }

        #endregion

        #region IsSameLetterButWithDifferentCaseAs

        [Test]
        public void IsSameLetterWithDifferentCaseWorks()
        {
            Check.That(FirstLetterLowerCase).IsSameLetterButWithDifferentCaseAs('A');
        }

        [Test]
        public void NotIsSameLetterWithDifferentCaseThrows()
        {
            Check.ThatCode(() =>
            {
                Check.That('A').Not.IsSameLetterButWithDifferentCaseAs('a');
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is the same letter as the given one but with different case, whereas it must not." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['A']" + Environment.NewLine + "The given char:" + Environment.NewLine + "\t['a']");
        }

        [Test]
        public void NotIsSameLetterWithDifferentCaseWorks()
        {
            Check.That('.').Not.IsSameLetterButWithDifferentCaseAs('.');

            Check.That(FirstLetterLowerCase).Not.IsSameLetterButWithDifferentCaseAs('a');
        }

        [Test]
        public void NotIsSameLetterWithDifferentCaseWorksWithADifferentLetterCasedDifferently()
        {
            Check.That('Z').Not.IsSameLetterButWithDifferentCaseAs('y');
        }

        [Test]
        public void IsSameLetterWithDifferentCaseThrowsWithSameCharWithSameCase()
        {
            Check.ThatCode(() =>
            {
                Check.That(FirstLetterLowerCase).IsSameLetterButWithDifferentCaseAs('a');
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is the same letter, but must have different case than the given one." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['a']" + Environment.NewLine + "The given char:" + Environment.NewLine + "\t['a']");
        }

        [Test]
        public void IsSameLetterWithDifferentCaseThrowsWithAnotherChar()
        {
            Check.ThatCode(() =>
            {
                Check.That(FirstLetterLowerCase).IsSameLetterButWithDifferentCaseAs('b');
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is different from the expected letter." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['a']" + Environment.NewLine + "The given char:" + Environment.NewLine + "\t['b']");
        }

        [Test]
        public void IsSameLetterWithDifferentCaseThrowsWithNotALetter()
        {
            Check.ThatCode(() =>
            {
                Check.That('.').IsSameLetterButWithDifferentCaseAs('B');
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is not a letter, where as it must." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['.']" + Environment.NewLine + "The given char:" + Environment.NewLine + "\t['B']");
        }

        #endregion

        #region IsSameLetterAs

        [Test]
        public void IsSameLetterWorksAlsoWhenCaseAreDifferent()
        {
            const char lowerCasedZ = 'z';
            const char upperCasedZ = 'Z';
            
            Check.That(upperCasedZ).IsSameLetterAs(lowerCasedZ);
            Check.That(upperCasedZ).IsSameLetterAs(upperCasedZ);
            Check.That(lowerCasedZ).IsSameLetterAs(lowerCasedZ);
        }

        [Test]
        public void NotIsSameLetterWorks()
        {
            Check.That(FirstLetterLowerCase).Not.IsSameLetterAs('z');
        }

        [Test]
        public void IsSameLetterThrowsWithDifferentLetters()
        {
            Check.ThatCode(() =>
            {
                Check.That(FirstLetterLowerCase).IsSameLetterAs('z');
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is not the same letter as the given one (whatever the case)." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['a']" + Environment.NewLine + "The given char:" + Environment.NewLine + "\t['z']");
        }

        [Test]
        public void IsSameLetterThrowsWithNonLetterChar()
        {
            Check.ThatCode(() =>
            {
                Check.That('/').IsSameLetterAs('/');
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is not the same letter as the given one (whatever the case)." + Environment.NewLine + "The checked char is not even a letter!" + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['/']" + Environment.NewLine + "The given char:" + Environment.NewLine + "\t['/']");
        }

        [Test]
        public void NotIsSameLetterThrows()
        {
            Check.ThatCode(() =>
            {
                Check.That(FirstLetterLowerCase).Not.IsSameLetterAs('A');
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is the same letter as the given one (whatever the case), whereas it must not." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['a']" + Environment.NewLine + "The given char:" + Environment.NewLine + "\t['A']");
        }

        #endregion

        #region IsALetter

        [Test]
        public void IsALetterWorks()
        {
            Check.That(FirstLetterLowerCase).IsALetter();
        }

        [Test]
        public void IsALetterThrowsWithNonLetterChar()
        {
            Check.ThatCode(() =>
            {
                Check.That('/').IsALetter();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is not a letter." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['/']");
        }

        [Test]
        public void NotIsALetterWorks()
        {
            Check.That('.').Not.IsALetter();
            Check.That('.').Not.IsALetter();
            Check.That('.').Not.IsALetter();
        }

        [Test]
        public void NotIsALetterThrows()
        {
            Check.ThatCode(() =>
            {
                Check.That(FirstLetterLowerCase).Not.IsALetter();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is a letter whereas it must not." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['a']");
        }

        #endregion

        #region IsADigit

        [Test]
        public void IsADigitWorks()
        {
            Check.That('1').IsADigit();
        }

        [Test]
        public void NotIsADigitWorks()
        {
            Check.That(FirstLetterLowerCase).Not.IsADigit();

            Check.That('-').Not.IsADigit();

            Check.That(' ').Not.IsADigit();
        }

        [Test]
        public void IsADigitThrows()
        {

            Check.ThatCode(() =>
            {
                Check.That('Z').IsADigit();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is not a decimal digit." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['Z']");
        }

        [Test]
        public void NotIsADigitThrows()
        {
            Check.ThatCode(() =>
            {
                Check.That('2').Not.IsADigit();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is a decimal digit whereas it must not." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['2']");
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
            Check.That('2').Not.IsAPunctuationMark();

            Check.That('Z').Not.IsAPunctuationMark();
        }

        [Test]
        public void IsPunctuationThrows()
        {
            Check.ThatCode(() =>
            {
                Check.That('2').IsAPunctuationMark();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is not a punctuation mark." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['2']");
        }

        [Test]
        public void NotIsPunctuationThrows()
        {
            Check.ThatCode(() =>
            {
                Check.That('-').Not.IsAPunctuationMark();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked char is a punctuation mark whereas it must not." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['-']");
        }

        #endregion
    }
}
