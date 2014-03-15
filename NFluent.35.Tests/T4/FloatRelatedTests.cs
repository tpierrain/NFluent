﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FloatRelatedTests.cs" company="">
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
    public class FloatRelatedTests
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
            const float Two = 2F;

            Check.That(Two).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to zero, whereas it must not.\nThe checked value:\n\t[0]")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const float Zero = 0F;

            Check.That(Zero).IsNotZero();
        }

        #endregion

        #region NotIsZero

        [Test]
        public void NotIsZeroWorks()
        {
            const float Two = 2F;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to zero whereas it must not.")]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const float Zero = 0F;

            Check.That(Zero).Not.IsZero();
        }

        #endregion

        #region NotIsNotZero

        [Test]
        public void NotIsNotZeroWorks()
        {
            const float Zero = 0F;

            Check.That(Zero).Not.IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from zero.\nThe checked value:\n\t[2]")]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const float Two = 2F;
            Check.That(Two).Not.IsNotZero();
        }

        #endregion

        #region IComparable checks

        [Test]
        public void IsBeforeWorks()
        {
            const float Two = 2F;
            const float Twenty = 20F;

            Check.That(Two).IsBefore(Twenty);
        }

        [Test]
        public void NotIsBeforeWorks()
        {
            const float Two = 2F;
            const float Twenty = 20F;

            Check.That(Twenty).Not.IsBefore(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not before the reference value.\nThe checked value:\n\t[20]\nThe expected value: before\n\t[2]")]
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            const float Two = 2F;
            const float Twenty = 20F;

            Check.That(Twenty).IsBefore(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not before the reference value.\nThe checked value:\n\t[2]\nThe expected value: before\n\t[2]")]
        public void IsBeforeThrowsExceptionWhenGivingTheSameValue()
        {
            const float Two = 2F;

            Check.That(Two).IsBefore(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is before the reference value whereas it must not.\nThe checked value:\n\t[2]\nThe expected value: after\n\t[20]")]
        public void NotIsBeforeThrowsExceptionWhenFailing()
        {
            const float Two = 2F;
            const float Twenty = 20F;

            Check.That(Two).Not.IsBefore(Twenty);
        }

        [Test]
        public void IsAfterWorks()
        {
            const float Two = 2F;
            const float Twenty = 20F;

            Check.That(Twenty).IsAfter(Two);
        }

        [Test]
        public void NotIsAfterWorks()
        {
            const float Two = 2F;
            const float Twenty = 20F;

            Check.That(Two).Not.IsAfter(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not after the reference value.\nThe checked value:\n\t[2]\nThe expected value: after\n\t[2]")]
        public void IsAfterThrowsExceptionWhenFailing()
        {
            const float Two = 2F;

            Check.That(Two).IsAfter(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is after the reference value whereas it must not.\nThe checked value:\n\t[20]\nThe expected value: before\n\t[2]")]
        public void NotIsAfterThrowsExceptionWhenFailing()
        {
            const float Two = 2F;
            const float Twenty = 20F;

            Check.That(Twenty).Not.IsAfter(Two);
        }

        #endregion

        #region IsLessThan & Co

        [Test]
        public void IsLessThanWorks()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(One).IsLessThan(Twenty);
        }

        [Test]
        public void NotIsLessThanWorks()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(Twenty).Not.IsLessThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(One).Not.IsLessThan(Twenty);
        }

        #endregion

        #region IsGreaterThan

        [Test]
        public void IsGreaterThanWorks()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is greater than the threshold.\nThe checked value:\n\t[20]\nThe expected value: less than\n\t[1]")]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(Twenty).Not.IsGreaterThan(One);
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const float Twenty = 20F;
            const float Zero = 0F;

            Check.That(Twenty).IsNotZero().And.IsAfter(Zero);
            Check.That(Twenty).IsAfter(Zero).And.IsNotZero();
        }

        #region Equals / IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const float Twenty = 20F;
            const float OtherTwenty = 20F;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void EqualsWorksToo()
        {
            const float Twenty = 20F;
            const float OtherTwenty = 20F;
            const float Zero = 0F;

            Check.That(Twenty).Equals(OtherTwenty);

            // check the 'other implementation of equals
            Check.That(Twenty).IsAfter(Zero).And.Equals(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [float]")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const float Twenty = 20F;

            Check.That(Twenty).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [float]")]
        public void NotEqualsThrowsExceptionWhenFailing()
        {
            const float Twenty = 20F;

            Check.That(Twenty).Not.Equals(Twenty);
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [float]")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const float Twenty = 20F;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[1]\nThe expected value:\n\t[20]")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(One).Not.IsNotEqualTo(Twenty);
        }

        #endregion

        #region Nullables

        #region HasAValue

        [Test]
        public void HasValueWorks()
        {
            float? one = 1F;

            Check.That(one).HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void HasValueThrowsExceptionWhenFailing()
        {
            float? noValue = null;

            Check.That(noValue).HasAValue();
        }

        [Test]
        public void NotHasValueWorks()
        {
            float? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            float? one = 1F;

            Check.That(one).Not.HasAValue();
        }

        [Test]
        public void HasValueSupportsToBeChainedWithTheWhichOperator()
        {
            float? one = 1F;
            const float Zero = 0F;

            Check.That(one).HasAValue().Which.IsAfter(Zero).And.IsEqualTo((float)1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable has no value to be checked.")]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            float? noValue = null;
            const float Zero = 0F;

            Check.That(noValue).Not.HasAValue().Which.IsAfter(Zero);
        }

        #endregion

        #region HasNoValue
        
        [Test]
        public void HasNoValueWorks()
        {
            float? noValue = null;

            Check.That(noValue).HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            float? one = 1F;

            Check.That(one).HasNoValue();
        }

        [Test]
        public void NotHasNoValueWorks()
        {
            float? one = 1F;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            float? noValue = null;

            Check.That(noValue).Not.HasNoValue();
        }

        #endregion

        #region IsInstanceOf (which is linkable)

        [Test]
        public void IsInstanceOfWorksWithNullable()
        {
            float? one = 1F;

            Check.That(one).IsInstanceOf<float?>().And.HasAValue().Which.IsEqualTo((float)1);
        }

        [Test]
        public void IsInstanceOfWithNullableIsLinkable()
        {
            float? one = 1F;

            Check.That(one).IsInstanceOf<float?>().And.HasAValue().Which.IsEqualTo((float)1);
            Check.That(one).HasAValue().And.IsInstanceOf<float?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [float?] whereas it must not.\nThe checked value:\n\t[1] of type: [float?]\nThe expected value: different from\n\tan instance of type: [float?]")]
        public void NotIsInstanceOfWorksWithNullable()
        {
            float? one = 1F;

            Check.That(one).Not.IsInstanceOf<float?>();
        }

        [Test]
        public void IsInstanceOfWorksIfValueIsNullButOfSameNullableType()
        {
            float? noValue = null;

            Check.That(noValue).IsInstanceOf<float?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [float?] whereas it must not.\nThe checked value:\n\t[null] of type: [float?]\nThe expected value: different from\n\tan instance of type: [float?]")]
        public void NotIsInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            float? noValue = null;

            Check.That(noValue).Not.IsInstanceOf<float?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not an instance of [string].\nThe checked value:\n\t[null] of type: [float?]\nThe expected value:\n\tan instance of type: [string]")]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            float? one = null;

            Check.That(one).IsInstanceOf<string>();
        }

        #endregion

        #region IsNotInstance

        [Test]
        public void IsNotInstanceOfWorksWithNullable()
        {
            float? one = 1F;

            Check.That(one).IsNotInstanceOf<float>().And.HasAValue().Which.IsEqualTo((float)1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [float?] whereas it must not.\nThe checked value:\n\t[1] of type: [float?]\nThe expected value: different from\n\tan instance of type: [float?]")]
        public void IsNotInstanceOfThrowsWithValueIsOfSameNullableType()
        {
            float? one = 1F;

            Check.That(one).IsNotInstanceOf<float?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [float?] whereas it must not.\nThe checked value:\n\t[null] of type: [float?]\nThe expected value: different from\n\tan instance of type: [float?]")]
        public void IsNotInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            float? noValue = null;

            Check.That(noValue).IsNotInstanceOf<float?>();
        }

        #endregion

        #endregion
    }
}
