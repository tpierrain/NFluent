// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="LongSignedNumberRelatedTests.cs" company="">
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
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class LongSignedNumberRelatedTests
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------

        #pragma warning restore 169
        private CultureInfo savedCulture;

        [SetUp]
        public void SetUp()
        {
            // Important so that ToString() versions of decimal works whatever the current culture.
            this.savedCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
        }

        [TearDown]
        public void TearDown()
        {
            // Boy scout rule ;-)
            Thread.CurrentThread.CurrentCulture = this.savedCulture;
        }

        #region IsGreaterThanZero (obsolete)

        [Test]
        public void IsPositiveWorks()
        {
            const long Two = 2L;

            Check.That(Two).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not greater than zero.\nThe checked value:\n\t[0]")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const long Zero = 0L;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is greater than zero, whereas it must not.\nThe checked value:\n\t[2]")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const long Two = 2L;

            Check.That(Two).Not.IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not greater than zero.\nThe checked value:\n\t[-50]")]
        public void IsPositiveThrowsExceptionWhenValueIsNegative()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).IsPositive();
        }

        [Test]
        public void NotIsPositiveWorks()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).Not.IsPositive();
        }

        #endregion

        #region IsGreaterThanZero

        [Test]
        public void IsGreaterThanZeroWorks()
        {
            const long Two = 2L;

            Check.That(Two).IsGreaterThanZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not greater than zero.\nThe checked value:\n\t[0]")]
        public void IsGreaterThanZeroThrowsExceptionWhenEqualToZero()
        {
            const long Zero = 0L;
            Check.That(Zero).IsGreaterThanZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is greater than zero, whereas it must not.\nThe checked value:\n\t[2]")]
        public void NotIsGreaterThanZeroThrowsExceptionWhenFailing()
        {
            const long Two = 2L;

            Check.That(Two).Not.IsGreaterThanZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not greater than zero.\nThe checked value:\n\t[-50]")]
        public void IsGreaterThanZeroThrowsExceptionWhenValueIsNegative()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).IsGreaterThanZero();
        }

        [Test]
        public void NotIsGreaterThanZeroWorks()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).Not.IsGreaterThanZero();
        }

        #endregion

        #region IsNegative (obsolete)

        [Test]
        public void IsNegativeWorks()
        {
            const long Two = -2;

            Check.That(Two).IsNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly negative.\nThe checked value:\n\t[0]")]
        public void IsNegativeThrowsExceptionWhenEqualToZero()
        {
            const long Zero = 0L;
            Check.That(Zero).IsNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is negative, whereas it must not.\nThe checked value:\n\t[-2]")]
        public void NotIsNegativeThrowsExceptionWhenFailing()
        {
            const long Two = -2;

            Check.That(Two).Not.IsNegative();
        }

        #endregion

        #region IsLessThanZero

        [Test]
        public void IsLessThanZeroWorks()
        {
            const long Two = -2;

            Check.That(Two).IsLessThanZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly negative.\nThe checked value:\n\t[0]")]
        public void IsLessThanZeroThrowsExceptionWhenEqualToZero()
        {
            const long Zero = 0L;
            Check.That(Zero).IsLessThanZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is negative, whereas it must not.\nThe checked value:\n\t[-2]")]
        public void NotIsLessThanZeroThrowsExceptionWhenFailing()
        {
            const long Two = -2;

            Check.That(Two).Not.IsLessThanZero();
        }

        #endregion
    }
}
