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

        #region IsPositive (obsolete)

        [Test]
        public void IsPositiveWorks()
        {
            const long Two = 2L;

            Check.That(Two).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[0]")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const long Zero = 0L;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is strictly positive (i.e. greater than zero), whereas it must not.\nThe checked value:\n\t[2]")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const long Two = 2L;

            Check.That(Two).Not.IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[-50]")]
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

        #region IsStrictlyPositive

        [Test]
        public void IsStrictlyPositiveWorks()
        {
            const long Two = 2L;

            Check.That(Two).IsStrictlyPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[0]")]
        public void IsStrictlyPositiveThrowsExceptionWhenEqualToZero()
        {
            const long Zero = 0L;
            Check.That(Zero).IsStrictlyPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is strictly positive (i.e. greater than zero), whereas it must not.\nThe checked value:\n\t[2]")]
        public void NotIsStrictlyPositiveThrowsExceptionWhenFailing()
        {
            const long Two = 2L;

            Check.That(Two).Not.IsStrictlyPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[-50]")]
        public void IsStrictlyPositiveThrowsExceptionWhenValueIsNegative()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).IsStrictlyPositive();
        }

        [Test]
        public void NotIsStrictlyPositiveWorks()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).Not.IsStrictlyPositive();
        }

        #endregion

        #region IsPositiveOrZero

        [Test]
        public void IsPositiveOrZeroWorks()
        {
            const long Zero = 0L;
            const long Two = 2L;

            Check.That(Zero).IsPositiveOrZero();
            Check.That(Two).IsPositiveOrZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is positive or equal to zero, whereas it must not.\nThe checked value:\n\t[0]")]
        public void NotIsPositiveOrZeroThrowsExceptionWhenFailing()
        {
            const long Zero = 0L;

            Check.That(Zero).Not.IsPositiveOrZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not positive or equal to zero.\nThe checked value:\n\t[-50]")]
        public void IsPositiveOrZeroThrowsExceptionWhenValueIsNegative()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).IsPositiveOrZero();
        }

        [Test]
        public void NotIsPositiveOrZeroWorks()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).Not.IsPositiveOrZero();
        }

        #endregion

        #region IsNegative (obsolete)

        [Test]
        public void IsNegativeWorks()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).IsNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly negative.\nThe checked value:\n\t[0]")]
        public void IsNegativeThrowsExceptionWhenEqualToZero()
        {
            const long Zero = 0L;
            Check.That(Zero).IsNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is strictly negative, whereas it must not.\nThe checked value:\n\t[-50]")]
        public void NotIsNegativeThrowsExceptionWhenFailing()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).Not.IsNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly negative.\nThe checked value:\n\t[2]")]
        public void IsNegativeThrowsExceptionWhenValueIsPositive()
        {
            const long Two = 2L;

            Check.That(Two).IsNegative();
        }

        [Test]
        public void NotIsNegativeWorks()
        {
            const long Two = 2L;

            Check.That(Two).Not.IsNegative();
        }

        #endregion

        #region IsStrictlyNegative

        [Test]
        public void IsStrictyNegativeWorks()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).IsStrictlyNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly negative.\nThe checked value:\n\t[0]")]
        public void IsStrictyNegativeThrowsExceptionWhenEqualToZero()
        {
            const long Zero = 0L;
            Check.That(Zero).IsStrictlyNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is strictly negative, whereas it must not.\nThe checked value:\n\t[-50]")]
        public void NotIsStrictyNegativeThrowsExceptionWhenFailing()
        {
            const long MinusFifty = -50L;

            Check.That(MinusFifty).Not.IsStrictlyNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly negative.\nThe checked value:\n\t[2]")]
        public void IsStrictyNegativeThrowsExceptionWhenValueIsPositive()
        {
            const long Two = 2L;

            Check.That(Two).IsStrictlyNegative();
        }

        [Test]
        public void NotIsStrictyNegativeWorks()
        {
            const long Two = 2L;

            Check.That(Two).Not.IsStrictlyNegative();
        }

        #endregion
    }
}
