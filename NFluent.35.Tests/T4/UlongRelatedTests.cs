// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="UlongRelatedTests.cs" company="">
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
    public class UlongRelatedTests
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
            const ulong Two = 2;

            Check.That(Two).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to zero, whereas it must not.\nThe checked value:\n\t[0]")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const ulong Zero = 0;

            Check.That(Zero).IsNotZero();
        }

        #endregion

        #region NotIsZero

        [Test]
        public void NotIsZeroWorks()
        {
            const ulong Two = 2;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to zero whereas it must not.")]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const ulong Zero = 0;

            Check.That(Zero).Not.IsZero();
        }

        #endregion

        #region NotIsNotZero

        [Test]
        public void NotIsNotZeroWorks()
        {
            const ulong Zero = 0;

            Check.That(Zero).Not.IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from zero.\nThe checked value:\n\t[2]")]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const ulong Two = 2;
            Check.That(Two).Not.IsNotZero();
        }

        #endregion

        #region IComparable checks

        [Test]
        public void IsBeforeWorks()
        {
            const ulong Two = 2;
            const ulong Twenty = 20;

            Check.That(Two).IsBefore(Twenty);
        }

        [Test]
        public void NotIsBeforeWorks()
        {
            const ulong Two = 2;
            const ulong Twenty = 20;

            Check.That(Twenty).Not.IsBefore(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not before the reference value.\nThe checked value:\n\t[20]\nThe expected value: before\n\t[2]")]
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            const ulong Two = 2;
            const ulong Twenty = 20;

            Check.That(Twenty).IsBefore(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not before the reference value.\nThe checked value:\n\t[2]\nThe expected value: before\n\t[2]")]
        public void IsBeforeThrowsExceptionWhenGivingTheSameValue()
        {
            const ulong Two = 2;

            Check.That(Two).IsBefore(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is before the reference value whereas it must not.\nThe checked value:\n\t[2]\nThe expected value: after\n\t[20]")]
        public void NotIsBeforeThrowsExceptionWhenFailing()
        {
            const ulong Two = 2;
            const ulong Twenty = 20;

            Check.That(Two).Not.IsBefore(Twenty);
        }

        [Test]
        public void IsAfterWorks()
        {
            const ulong Two = 2;
            const ulong Twenty = 20;

            Check.That(Twenty).IsAfter(Two);
        }

        [Test]
        public void NotIsAfterWorks()
        {
            const ulong Two = 2;
            const ulong Twenty = 20;

            Check.That(Two).Not.IsAfter(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not after the reference value.\nThe checked value:\n\t[2]\nThe expected value: after\n\t[2]")]
        public void IsAfterThrowsExceptionWhenFailing()
        {
            const ulong Two = 2;

            Check.That(Two).IsAfter(Two);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is after the reference value whereas it must not.\nThe checked value:\n\t[20]\nThe expected value: before\n\t[2]")]
        public void NotIsAfterThrowsExceptionWhenFailing()
        {
            const ulong Two = 2;
            const ulong Twenty = 20;

            Check.That(Twenty).Not.IsAfter(Two);
        }

        #endregion

        #region IsLessThan & Co

        [Test]
        public void IsLessThanWorks()
        {
            const ulong One = 1;
            const ulong Twenty = 20;

            Check.That(One).IsLessThan(Twenty);
        }

        [Test]
        public void NotIsLessThanWorks()
        {
            const ulong One = 1;
            const ulong Twenty = 20;

            Check.That(Twenty).Not.IsLessThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const ulong One = 1;
            const ulong Twenty = 20;

            Check.That(One).Not.IsLessThan(Twenty);
        }

        #endregion

        #region IsGreaterThan

        [Test]
        public void IsGreaterThanWorks()
        {
            const ulong One = 1;
            const ulong Twenty = 20;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const ulong One = 1;
            const ulong Twenty = 20;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is greater than the threshold.\nThe checked value:\n\t[20]\nThe expected value: less than\n\t[1]")]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            const ulong One = 1;
            const ulong Twenty = 20;

            Check.That(Twenty).Not.IsGreaterThan(One);
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const ulong Twenty = 20;
            const ulong Zero = 0;

            Check.That(Twenty).IsNotZero().And.IsAfter(Zero);
            Check.That(Twenty).IsAfter(Zero).And.IsNotZero();
        }

        #region Equals / IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const ulong Twenty = 20;
            const ulong OtherTwenty = 20;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void EqualsWorksToo()
        {
            const ulong Twenty = 20;
            const ulong OtherTwenty = 20;
            const ulong Zero = 0;

            Check.That(Twenty).Equals(OtherTwenty);

            // check the 'other implementation of equals
            Check.That(Twenty).IsAfter(Zero).And.Equals(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const ulong One = 1;
            const ulong Twenty = 20;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [ulong]")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const ulong Twenty = 20;

            Check.That(Twenty).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [ulong]")]
        public void NotEqualsThrowsExceptionWhenFailing()
        {
            const ulong Twenty = 20;

            Check.That(Twenty).Not.Equals(Twenty);
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const ulong One = 1;
            const ulong Twenty = 20;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [ulong]")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const ulong Twenty = 20;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[1]\nThe expected value:\n\t[20]")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const ulong One = 1;
            const ulong Twenty = 20;

            Check.That(One).Not.IsNotEqualTo(Twenty);
        }

        #endregion

        #region Nullables

        #region HasAValue

        [Test]
        public void HasValueWorks()
        {
            ulong? one = 1;

            Check.That(one).HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void HasValueThrowsExceptionWhenFailing()
        {
            ulong? noValue = null;

            Check.That(noValue).HasAValue();
        }

        [Test]
        public void NotHasValueWorks()
        {
            ulong? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            ulong? one = 1;

            Check.That(one).Not.HasAValue();
        }

        [Test]
        public void HasValueSupportsToBeChainedWithTheWhichOperator()
        {
            ulong? one = 1;
            const ulong Zero = 0;

            Check.That(one).HasAValue().Which.IsAfter(Zero).And.IsEqualTo((ulong)1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable has no value to be checked.")]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            ulong? noValue = null;
            const ulong Zero = 0;

            Check.That(noValue).Not.HasAValue().Which.IsAfter(Zero);
        }

        #endregion

        #region HasNoValue
        
        [Test]
        public void HasNoValueWorks()
        {
            ulong? noValue = null;

            Check.That(noValue).HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            ulong? one = 1;

            Check.That(one).HasNoValue();
        }

        [Test]
        public void NotHasNoValueWorks()
        {
            ulong? one = 1;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            ulong? noValue = null;

            Check.That(noValue).Not.HasNoValue();
        }

        #endregion

        #region IsInstanceOf (which is linkable)

        [Test]
        public void IsInstanceOfWorksWithNullable()
        {
            ulong? one = 1;

            Check.That(one).IsInstanceOf<ulong?>().And.HasAValue().Which.IsEqualTo((ulong)1);
        }

        [Test]
        public void IsInstanceOfWithNullableIsLinkable()
        {
            ulong? one = 1;

            Check.That(one).IsInstanceOf<ulong?>().And.HasAValue().Which.IsEqualTo((ulong)1);
            Check.That(one).HasAValue().And.IsInstanceOf<ulong?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [ulong?] whereas it must not.\nThe checked value:\n\t[1] of type: [ulong?]\nThe expected value: different from\n\tan instance of type: [ulong?]")]
        public void NotIsInstanceOfWorksWithNullable()
        {
            ulong? one = 1;

            Check.That(one).Not.IsInstanceOf<ulong?>();
        }

        [Test]
        public void IsInstanceOfWorksIfValueIsNullButOfSameNullableType()
        {
            ulong? noValue = null;

            Check.That(noValue).IsInstanceOf<ulong?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [ulong?] whereas it must not.\nThe checked value:\n\t[null] of type: [ulong?]\nThe expected value: different from\n\tan instance of type: [ulong?]")]
        public void NotIsInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            ulong? noValue = null;

            Check.That(noValue).Not.IsInstanceOf<ulong?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not an instance of [string].\nThe checked value:\n\t[null] of type: [ulong?]\nThe expected value:\n\tan instance of type: [string]")]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            ulong? one = null;

            Check.That(one).IsInstanceOf<string>();
        }

        #endregion

        #region IsNotInstance

        [Test]
        public void IsNotInstanceOfWorksWithNullable()
        {
            ulong? one = 1;

            Check.That(one).IsNotInstanceOf<ulong>().And.HasAValue().Which.IsEqualTo((ulong)1);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [ulong?] whereas it must not.\nThe checked value:\n\t[1] of type: [ulong?]\nThe expected value: different from\n\tan instance of type: [ulong?]")]
        public void IsNotInstanceOfThrowsWithValueIsOfSameNullableType()
        {
            ulong? one = 1;

            Check.That(one).IsNotInstanceOf<ulong?>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is an instance of [ulong?] whereas it must not.\nThe checked value:\n\t[null] of type: [ulong?]\nThe expected value: different from\n\tan instance of type: [ulong?]")]
        public void IsNotInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            ulong? noValue = null;

            Check.That(noValue).IsNotInstanceOf<ulong?>();
        }

        #endregion

        #endregion
    }
}
