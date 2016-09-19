// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ShortSignedNumberRelatedTests.cs" company="">
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
    using System.Globalization;
    using NUnit.Framework;

    [TestFixture]
    public class ShortSignedNumberRelatedTests
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------

        private CultureInfo savedCulture;

        [OneTimeSetUp]
        public void ForceCulture()
        {
            this.savedCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = new CultureInfo("fr-FR");
        }

        [OneTimeTearDown]
        public void RestoreCulture()
        {
            CultureInfo.CurrentCulture = this.savedCulture;
        }


        #pragma warning restore 169
        #region IsPositive (obsolete)

        [Test]
        public void IsPositiveWorks()
        {
            const short Two = 2;

            Check.That(Two).IsPositive();
        }

        [Test]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const short Zero = 0;

            Check.ThatCode(() =>
            {
                Check.That(Zero).IsPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[0]");
        }

        [Test]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const short Two = 2;

            Check.ThatCode(() =>
            {
                Check.That(Two).Not.IsPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is strictly positive (i.e. greater than zero), whereas it must not.\nThe checked value:\n\t[2]");
        }

        [Test]
        public void IsPositiveThrowsExceptionWhenValueIsNegative()
        {
            const short MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).IsPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[-50]");
        }

        [Test]
        public void NotIsPositiveWorks()
        {
            const short MinusFifty = -50;

            Check.That(MinusFifty).Not.IsPositive();
        }

        #endregion

        #region IsStrictlyPositive

        [Test]
        public void IsStrictlyPositiveWorks()
        {
            const short Two = 2;

            Check.That(Two).IsStrictlyPositive();
        }

        [Test]
        public void IsStrictlyPositiveThrowsExceptionWhenEqualToZero()
        {
            const short Zero = 0;

            Check.ThatCode(() =>
            {
                Check.That(Zero).IsStrictlyPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[0]");
        }

        [Test]
        public void NotIsStrictlyPositiveThrowsExceptionWhenFailing()
        {
            const short Two = 2;

            Check.ThatCode(() =>
            {
                Check.That(Two).Not.IsStrictlyPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is strictly positive (i.e. greater than zero), whereas it must not.\nThe checked value:\n\t[2]");
        }

        [Test]
        public void IsStrictlyPositiveThrowsExceptionWhenValueIsNegative()
        {
            const short MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).IsStrictlyPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[-50]");
        }

        [Test]
        public void NotIsStrictlyPositiveWorks()
        {
            const short MinusFifty = -50;

            Check.That(MinusFifty).Not.IsStrictlyPositive();
        }

        #endregion

        #region IsPositiveOrZero

        [Test]
        public void IsPositiveOrZeroWorks()
        {
            const short Zero = 0;
            const short Two = 2;

            Check.That(Zero).IsPositiveOrZero();
            Check.That(Two).IsPositiveOrZero();
        }

        [Test]
        public void NotIsPositiveOrZeroThrowsExceptionWhenFailing()
        {
            const short Zero = 0;

            Check.ThatCode(() =>
            {
                Check.That(Zero).Not.IsPositiveOrZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is positive or equal to zero, whereas it must not.\nThe checked value:\n\t[0]");
        }

        [Test]
        public void IsPositiveOrZeroThrowsExceptionWhenValueIsNegative()
        {
            const short MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).IsPositiveOrZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not positive or equal to zero.\nThe checked value:\n\t[-50]");
        }

        [Test]
        public void NotIsPositiveOrZeroWorks()
        {
            const short MinusFifty = -50;

            Check.That(MinusFifty).Not.IsPositiveOrZero();
        }

        #endregion

        #region IsNegative (obsolete)

        [Test]
        public void IsNegativeWorks()
        {
            const short MinusFifty = -50;

            Check.That(MinusFifty).IsNegative();
        }

        [Test]
        public void IsNegativeThrowsExceptionWhenEqualToZero()
        {
            const short Zero = 0;

            Check.ThatCode(() =>
            {
                Check.That(Zero).IsNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not strictly negative.\nThe checked value:\n\t[0]");
        }

        [Test]
        public void NotIsNegativeThrowsExceptionWhenFailing()
        {
            const short MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).Not.IsNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is strictly negative, whereas it must not.\nThe checked value:\n\t[-50]");
        }

        [Test]
        public void IsNegativeThrowsExceptionWhenValueIsPositive()
        {
            const short Two = 2;

            Check.ThatCode(() =>
            {
                Check.That(Two).IsNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not strictly negative.\nThe checked value:\n\t[2]");
        }

        [Test]
        public void NotIsNegativeWorks()
        {
            const short Two = 2;

            Check.That(Two).Not.IsNegative();
        }

        #endregion

        #region IsStrictlyNegative

        [Test]
        public void IsStrictyNegativeWorks()
        {
            const short MinusFifty = -50;

            Check.That(MinusFifty).IsStrictlyNegative();
        }

        [Test]
        public void IsStrictyNegativeThrowsExceptionWhenEqualToZero()
        {
            const short Zero = 0;
            
            Check.ThatCode(() =>
            {
                Check.That(Zero).IsStrictlyNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not strictly negative.\nThe checked value:\n\t[0]");
        }

        [Test]
        public void NotIsStrictyNegativeThrowsExceptionWhenFailing()
        {
            const short MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).Not.IsStrictlyNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is strictly negative, whereas it must not.\nThe checked value:\n\t[-50]");
        }

        [Test]
        public void IsStrictyNegativeThrowsExceptionWhenValueIsPositive()
        {
            const short Two = 2;

            Check.ThatCode(() =>
            {
                Check.That(Two).IsStrictlyNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not strictly negative.\nThe checked value:\n\t[2]");
        }

        [Test]
        public void NotIsStrictyNegativeWorks()
        {
            const short Two = 2;

            Check.That(Two).Not.IsStrictlyNegative();
        }

        #endregion
        
        #region IsNegativeOrZero

        [Test]
        public void IsNegativeOrZeroWorks()
        {
            const short MinusFifty = -50;
            const short Zero = 0;

            Check.That(Zero).IsNegativeOrZero();
            Check.That(MinusFifty).IsNegativeOrZero();
        }

        [Test]
        public void NotIsNegativeOrZeroThrowsExceptionWhenFailing()
        {
            const short MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).Not.IsNegativeOrZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is negative or equal to zero, whereas it must not.\nThe checked value:\n\t[-50]");
        }

        [Test]
        public void IsIsNegativeOrZeroThrowsExceptionWhenValueIsPositive()
        {
            const short Two = 2;

            Check.ThatCode(() =>
            {
                Check.That(Two).IsNegativeOrZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not negative or equal to zero.\nThe checked value:\n\t[2]");
        }

        [Test]
        public void NotIsNegativeOrZeroWorks()
        {
            const short Two = 2;

            Check.That(Two).Not.IsNegativeOrZero();
        }

        #endregion
    }
}
