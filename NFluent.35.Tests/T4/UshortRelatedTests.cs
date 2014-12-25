// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="UshortRelatedTests.cs" company="">
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
    public class UshortRelatedTests
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
            const ushort Two = 2;

            Check.That(Two).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to zero, whereas it must not.\nThe checked value:\n\t[0]")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const ushort Zero = 0;

            Check.That(Zero).IsNotZero();
        }

        #endregion

        #region NotIsZero

        [Test]
        public void NotIsZeroWorks()
        {
            const ushort Two = 2;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to zero whereas it must not.")]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const ushort Zero = 0;

            Check.That(Zero).Not.IsZero();
        }

        #endregion

        #region NotIsNotZero

        [Test]
        public void NotIsNotZeroWorks()
        {
            const ushort Zero = 0;

            Check.That(Zero).Not.IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from zero.\nThe checked value:\n\t[2]")]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const ushort Two = 2;
            Check.That(Two).Not.IsNotZero();
        }

        #endregion

        #region IComparable checks

        [Test]
        public void IsBeforeWorks()
        {
            const ushort Two = 2;
            const ushort Twenty = 20;

            Check.That(Two).IsBefore(Twenty);
        }

        [Test]
        public void NotIsBeforeWorks()
        {
            const ushort Two = 2;
            const ushort Twenty = 20;

            Check.That(Twenty).Not.IsBefore(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not before the reference value.\nThe checked value:\n\t[20]\nThe expected value: before\n\t[2]")]
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            const ushort Two = 2;
            const ushort Twenty = 20;

            Check.That(Twenty).IsBefore(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not before the reference value.\nThe checked value:\n\t[2]\nThe expected value: before\n\t[2]")]
        public void IsBeforeThrowsExceptionWhenGivingTheSameValue()
        {
            const ushort Two = 2;

            Check.That(Two).IsBefore(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is before the reference value whereas it must not.\nThe checked value:\n\t[2]\nThe expected value: after\n\t[20]")]
        public void NotIsBeforeThrowsExceptionWhenFailing()
        {
            const ushort Two = 2;
            const ushort Twenty = 20;

            Check.That(Two).Not.IsBefore(Twenty);
        }

        [Test]
        public void IsAfterWorks()
        {
            const ushort Two = 2;
            const ushort Twenty = 20;

            Check.That(Twenty).IsAfter(Two);
        }

        [Test]
        public void NotIsAfterWorks()
        {
            const ushort Two = 2;
            const ushort Twenty = 20;

            Check.That(Two).Not.IsAfter(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not after the reference value.\nThe checked value:\n\t[2]\nThe expected value: after\n\t[2]")]
        public void IsAfterThrowsExceptionWhenFailing()
        {
            const ushort Two = 2;

            Check.That(Two).IsAfter(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is after the reference value whereas it must not.\nThe checked value:\n\t[20]\nThe expected value: before\n\t[2]")]
        public void NotIsAfterThrowsExceptionWhenFailing()
        {
            const ushort Two = 2;
            const ushort Twenty = 20;

            Check.That(Twenty).Not.IsAfter(Two);
        }

        #endregion

        #region IsLessThan

        [Test]
        public void IsLessThanWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).IsLessThan(Twenty);
        }

        [Test]
        public void NotIsLessThanWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(Twenty).Not.IsLessThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).Not.IsLessThan(Twenty);
        }

        #endregion

        #region IsStrictlyLessThan

        [Test]
        public void IsStrictlyLessThanWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).IsStrictlyLessThan(Twenty);
        }

        [Test]
        public void NotIsStrictlyLessThanWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(Twenty).Not.IsStrictlyLessThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly less than the comparand.\nThe checked value:\n\t[1]\nThe expected value: strictly less than\n\t[1]")]
        public void IsStrictlyLessThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;

            Check.That(One).IsStrictlyLessThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is strictly less than the comparand.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void NotIsStrictlyLessThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).Not.IsStrictlyLessThan(Twenty);
        }

        #endregion



        #region IsGreaterThan

        [Test]
        public void IsGreaterThanWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is greater than the threshold.\nThe checked value:\n\t[20]\nThe expected value: less than\n\t[1]")]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(Twenty).Not.IsGreaterThan(One);
        }

        #endregion

        #region IsStrictlyGreaterThan

        [Test]
        public void IsStrictlyGreaterThanWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(Twenty).IsStrictlyGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not strictly greater than the comparand.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[1]")]
        public void IsStrictlyGreaterThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;

            Check.That(One).IsStrictlyGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is strictly greater than the comparand.\nThe checked value:\n\t[20]\nThe expected value: less than or equal to\n\t[1]")]
        public void NotIsStrictlyGreaterThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(Twenty).Not.IsStrictlyGreaterThan(One);
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const ushort Twenty = 20;
            const ushort Zero = 0;

            Check.That(Twenty).IsNotZero().And.IsAfter(Zero);
            Check.That(Twenty).IsAfter(Zero).And.IsNotZero();
        }

        #region Equals / IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const ushort Twenty = 20;
            const ushort OtherTwenty = 20;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void EqualsWorksToo()
        {
            const ushort Twenty = 20;
            const ushort OtherTwenty = 20;
            const ushort Zero = 0;

            Check.That(Twenty).Equals(OtherTwenty);

            // check the 'other implementation of equals
            Check.That(Twenty).IsAfter(Zero).And.Equals(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [ushort]")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const ushort Twenty = 20;

            Check.That(Twenty).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [ushort]")]
        public void NotEqualsThrowsExceptionWhenFailing()
        {
            const ushort Twenty = 20;

            Check.That(Twenty).Not.Equals(Twenty);
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [ushort]")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const ushort Twenty = 20;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[1]\nThe expected value:\n\t[20]")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).Not.IsNotEqualTo(Twenty);
        }

        #endregion

        #region Nullables

        #region HasAValue

        [Test]
        public void HasValueWorks()
        {
            ushort? one = 1;

            Check.That(one).HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable has no value, which is unexpected.")]
        public void HasValueThrowsExceptionWhenFailing()
        {
            ushort? noValue = null;

            Check.That(noValue).HasAValue();
        }

        [Test]
        public void NotHasValueWorks()
        {
            ushort? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable has a value, which is unexpected.\nThe checked nullable:\n\t[1]")]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            ushort? one = 1;

            Check.That(one).Not.HasAValue();
        }

        [Test]
        public void HasValueSupportsToBeChainedWithTheWhichOperator()
        {
            ushort? one = 1;
            const ushort Zero = 0;

            Check.That(one).HasAValue().Which.IsAfter(Zero).And.IsEqualTo((ushort)1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable has no value to be checked.")]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            ushort? noValue = null;
            const ushort Zero = 0;

            Check.That(noValue).Not.HasAValue().Which.IsAfter(Zero);
        }

        #endregion

        #region HasNoValue
        
        [Test]
        public void HasNoValueWorks()
        {
            ushort? noValue = null;

            Check.That(noValue).HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value has a value, whereas it must not.\nThe checked value:\n\t[1]")]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            ushort? one = 1;

            Check.That(one).HasNoValue();
        }

        [Test]
        public void NotHasNoValueWorks()
        {
            ushort? one = 1;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable has no value, which is unexpected.")]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            ushort? noValue = null;

            Check.That(noValue).Not.HasNoValue();
        }

        #endregion

        #region IsInstanceOf (which is linkable)

        [Test]
        public void IsInstanceOfWorksWithNullable()
        {
            ushort? one = 1;

            Check.That(one).IsInstanceOf<ushort?>().And.HasAValue().Which.IsEqualTo((ushort)1);
        }

        [Test]
        public void IsInstanceOfWithNullableIsLinkable()
        {
            ushort? one = 1;

            Check.That(one).IsInstanceOf<ushort?>().And.HasAValue().Which.IsEqualTo((ushort)1);
            Check.That(one).HasAValue().And.IsInstanceOf<ushort?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [ushort?] whereas it must not.\nThe checked value:\n\t[1] of type: [ushort?]\nThe expected value: different from\n\tan instance of type: [ushort?]")]
        public void NotIsInstanceOfWorksWithNullable()
        {
            ushort? one = 1;

            Check.That(one).Not.IsInstanceOf<ushort?>();
        }

        [Test]
        public void IsInstanceOfWorksIfValueIsNullButOfSameNullableType()
        {
            ushort? noValue = null;

            Check.That(noValue).IsInstanceOf<ushort?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [ushort?] whereas it must not.\nThe checked value:\n\t[null] of type: [ushort?]\nThe expected value: different from\n\tan instance of type: [ushort?]")]
        public void NotIsInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            ushort? noValue = null;

            Check.That(noValue).Not.IsInstanceOf<ushort?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not an instance of [string].\nThe checked value:\n\t[null] of type: [ushort?]\nThe expected value:\n\tan instance of type: [string]")]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            ushort? one = null;

            Check.That(one).IsInstanceOf<string>();
        }

        #endregion

        #region IsNotInstance

        [Test]
        public void IsNotInstanceOfWorksWithNullable()
        {
            ushort? one = 1;

            Check.That(one).IsNotInstanceOf<ushort>().And.HasAValue().Which.IsEqualTo((ushort)1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [ushort?] whereas it must not.\nThe checked value:\n\t[1] of type: [ushort?]\nThe expected value: different from\n\tan instance of type: [ushort?]")]
        public void IsNotInstanceOfThrowsWithValueIsOfSameNullableType()
        {
            ushort? one = 1;

            Check.That(one).IsNotInstanceOf<ushort?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [ushort?] whereas it must not.\nThe checked value:\n\t[null] of type: [ushort?]\nThe expected value: different from\n\tan instance of type: [ushort?]")]
        public void IsNotInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            ushort? noValue = null;

            Check.That(noValue).IsNotInstanceOf<ushort?>();
        }

        #endregion

        #endregion
    }
}
