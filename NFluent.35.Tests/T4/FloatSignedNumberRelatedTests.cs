// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FloatSignedNumberRelatedTests.cs" company="">
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
    public class FloatSignedNumberRelatedTests
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
            const float Two = 2F;

            Check.That(Two).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[0]")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const float Zero = 0F;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is strictly positive (i.e. greater than zero), whereas it must not.\nThe checked value:\n\t[2]")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const float Two = 2F;

            Check.That(Two).Not.IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[-50]")]
        public void IsPositiveThrowsExceptionWhenValueIsNegative()
        {
            const float MinusFifty = -50F;

            Check.That(MinusFifty).IsPositive();
        }

        [Test]
        public void NotIsPositiveWorks()
        {
            const float MinusFifty = -50F;

            Check.That(MinusFifty).Not.IsPositive();
        }

        #endregion

        #region IsStrictlyPositive

        [Test]
        public void IsStrictlyPositiveWorks()
        {
            const float Two = 2F;

            Check.That(Two).IsStrictlyPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[0]")]
        public void IsStrictlyPositiveThrowsExceptionWhenEqualToZero()
        {
            const float Zero = 0F;
            Check.That(Zero).IsStrictlyPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is strictly positive (i.e. greater than zero), whereas it must not.\nThe checked value:\n\t[2]")]
        public void NotIsStrictlyPositiveThrowsExceptionWhenFailing()
        {
            const float Two = 2F;

            Check.That(Two).Not.IsStrictlyPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly positive (i.e. greater than zero).\nThe checked value:\n\t[-50]")]
        public void IsStrictlyPositiveThrowsExceptionWhenValueIsNegative()
        {
            const float MinusFifty = -50F;

            Check.That(MinusFifty).IsStrictlyPositive();
        }

        [Test]
        public void NotIsStrictlyPositiveWorks()
        {
            const float MinusFifty = -50F;

            Check.That(MinusFifty).Not.IsStrictlyPositive();
        }

        #endregion

        #region IsNegative (obsolete)

        [Test]
        public void IsNegativeWorks()
        {
            const float MinusFifty = -50F;

            Check.That(MinusFifty).IsNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly negative.\nThe checked value:\n\t[0]")]
        public void IsNegativeThrowsExceptionWhenEqualToZero()
        {
            const float Zero = 0F;
            Check.That(Zero).IsNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is strictly negative, whereas it must not.\nThe checked value:\n\t[-50]")]
        public void NotIsNegativeThrowsExceptionWhenFailing()
        {
            const float MinusFifty = -50F;

            Check.That(MinusFifty).Not.IsNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly negative.\nThe checked value:\n\t[2]")]
        public void IsNegativeThrowsExceptionWhenValueIsPositive()
        {
            const float Two = 2F;

            Check.That(Two).IsNegative();
        }

        [Test]
        public void NotIsNegativeWorks()
        {
            const float Two = 2F;

            Check.That(Two).Not.IsNegative();
        }

        #endregion

        #region IsStrictlyNegative

        [Test]
        public void IsStrictyNegativeWorks()
        {
            const float MinusFifty = -50F;

            Check.That(MinusFifty).IsStrictlyNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly negative.\nThe checked value:\n\t[0]")]
        public void IsStrictyNegativeThrowsExceptionWhenEqualToZero()
        {
            const float Zero = 0F;
            Check.That(Zero).IsStrictlyNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is strictly negative, whereas it must not.\nThe checked value:\n\t[-50]")]
        public void NotIsStrictyNegativeThrowsExceptionWhenFailing()
        {
            const float MinusFifty = -50F;

            Check.That(MinusFifty).Not.IsStrictlyNegative();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly negative.\nThe checked value:\n\t[2]")]
        public void IsStrictyNegativeThrowsExceptionWhenValueIsPositive()
        {
            const float Two = 2F;

            Check.That(Two).IsStrictlyNegative();
        }

        [Test]
        public void NotIsStrictyNegativeWorks()
        {
            const float Two = 2F;

            Check.That(Two).Not.IsStrictlyNegative();
        }

        #endregion
    }
}
