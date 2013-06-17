// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DoubleRelatedTests.cs" company="">
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
    public class DoubleRelatedTests
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------

        #pragma warning restore 169
        private const string Blabla = ".*?";
        private const string LineFeed = "\\n";
        private const string NumericalHashCodeWithinBrackets = "(\\[(\\d+)\\])";

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

        #region IsNotZero

        [Test]
        public void IsNotZeroWorks()
        {
            const double Two = 2D;

            Check.That(Two).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is equal to zero, whereas it must not.\nThe checked value:\n\t[0]")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const double Zero = 0D;

            Check.That(Zero).IsNotZero();
        }

        #endregion

        #region NotIsZero

        [Test]
        public void NotIsZeroWorks()
        {
            const double Two = 2D;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is equal to zero whereas it must not.")]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const double Zero = 0D;

            Check.That(Zero).Not.IsZero();
        }

        #endregion

        #region NotIsNotZero

        [Test]
        public void NotIsNotZeroWorks()
        {
            const double Zero = 0D;

            Check.That(Zero).Not.IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is different from zero.\nThe checked value:\n\t[2]")]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const double Two = 2D;
            Check.That(Two).Not.IsNotZero();
        }

        #endregion

        #region IsPositive

        [Test]
        public void IsPositiveWorks()
        {
            const double Two = 2D;

            Check.That(Two).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is not strictly positive.\nThe checked value:\n\t[0]")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const double Zero = 0D;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is positive, whereas it must not.\nThe checked value:\n\t[2]")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const double Two = 2D;

            Check.That(Two).Not.IsPositive();
        }

        #endregion

        #region IsLessThan & Co

        [Test]
        public void IsLessThanWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).IsLessThan(Twenty);
        }

        [Test]
        public void NotIsLessThanWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(Twenty).Not.IsLessThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).Not.IsLessThan(Twenty);
        }

        #endregion

        #region IsGreaterThan

        [Test]
        public void IsGreaterThanWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is greater than the threshold.\nThe checked value:\n\t[20]\nThe expected value: less than\n\t[1]")]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(Twenty).Not.IsGreaterThan(One);
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const double Twenty = 20D;

            Check.That(Twenty).IsNotZero().And.IsPositive();
            Check.That(Twenty).IsPositive().And.IsNotZero();
        }

        #region Equals / IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const double Twenty = 20D;
            const double OtherTwenty = 20D;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void EqualsWorksToo()
        {
            const double Twenty = 20D;
            const double OtherTwenty = 20D;

            Check.That(Twenty).Equals(OtherTwenty);

            // check the 'other implementation of equals
            Check.That(Twenty).IsPositive().And.Equals(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [System.Double]")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const double Twenty = 20D;

            Check.That(Twenty).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [System.Double]")]
        public void NotEqualsThrowsExceptionWhenFailing()
        {
            const double Twenty = 20D;

            Check.That(Twenty).Not.Equals(Twenty);
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [System.Double]")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const double Twenty = 20D;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[1]\nThe expected value:\n\t[20]")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).Not.IsNotEqualTo(Twenty);
        }

        #endregion

        #region Nullables

        #region HasAValue

        [Test]
        public void HasValueWorks()
        {
            double? one = 1D;

            Check.That(one).HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void HasValueThrowsExceptionWhenFailing()
        {
            double? noValue = null;

            Check.That(noValue).HasAValue();
        }

        [Test]
        public void NotHasValueWorks()
        {
            double? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            double? one = 1D;

            Check.That(one).Not.HasAValue();
        }

        [Test]
        public void HasValueSupportsToBeChainedWithTheWhichOperator()
        {
            double? one = 1D;

            Check.That(one).HasAValue().Which.IsPositive().And.IsEqualTo((double)1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable has no value to be checked.")]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            double? noValue = null;

            Check.That(noValue).Not.HasAValue().Which.IsPositive();
        }

        #endregion

        #region HasNoValue
        
        [Test]
        public void HasNoValueWorks()
        {
            double? noValue = null;

            Check.That(noValue).HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            double? one = 1D;

            Check.That(one).HasNoValue();
        }

        [Test]
        public void NotHasNoValueWorks()
        {
            double? one = 1D;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            double? noValue = null;

            Check.That(noValue).Not.HasNoValue();
        }

        #endregion

        #region IsInstanceOf (which is chainable)

        [Test]
        public void IsInstanceOfWorksWithNullable()
        {
            double? one = 1D;

            Check.That(one).IsInstanceOf<double?>().And.HasAValue().Which.IsEqualTo((double)1);
        }

        [Test]
        public void IsInstanceOfWithNullableIsChainable()
        {
            double? one = 1D;

            Check.That(one).IsInstanceOf<double?>().And.HasAValue().Which.IsEqualTo((double)1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is an instance of System.Nullable`1[System.Double] whereas it must not.\nThe checked value:\n\t[1] of type: [System.Nullable`1[System.Double]]\nThe expected type: different from\n\t[System.Nullable`1[System.Double]]")]
        public void NotIsInstanceOfWorksWithNullable()
        {
            double? one = 1D;

            Check.That(one).Not.IsInstanceOf<double?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is an instance of System.Nullable`1[System.Double] whereas it must not.\nThe checked value:\n\t[null] of type: [System.Nullable`1[System.Double]]\nThe expected type: different from\n\t[System.Nullable`1[System.Double]]")]
        public void NotIsInstanceOfWorksWithNullableWithoutValue()
        {
            double? noValue = null;

            Check.That(noValue).Not.IsInstanceOf<double?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is not an instance of System.String.\nThe checked value:\n\t[null] of type: [System.Nullable`1[System.Double]]\nThe expected type:\n\t[System.String]")]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            double? one = null;

            Check.That(one).IsInstanceOf<string>();
        }

        #endregion

        #endregion
    }
}
