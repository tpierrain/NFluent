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
using System;

namespace NFluent.Tests
{
    using System.Globalization;
    using NUnit.Framework;
    using NFluent.Tests.Helpers;

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
        private const string LineFeed = "\n";
        private const string NumericalHashCodeWithinBrackets = "(\\[(\\d+)\\])";
        private CultureSession cultureSession;

        [OneTimeSetUp]
        public void ForceCulture()
        {
            this.cultureSession = new CultureSession("fr-FR");
        }

        [OneTimeTearDown]
        public void RestoreCulture()
        {
            this.cultureSession.Dispose();
        }


        #region IsNotZero

        [Test]
        public void IsNotZeroWorks()
        {
            const double Two = 2D;

            Check.That(Two).IsNotZero();
        }

        [Test]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const double Zero = 0D;

            Check.ThatCode(() =>
            {
                Check.That(Zero).IsNotZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to zero, whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[0]");
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
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const double Zero = 0D;

            Check.ThatCode(() =>
            {
                Check.That(Zero).Not.IsZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to zero whereas it must not.");
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
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const double Two = 2D;

            Check.ThatCode(() =>
            {
                Check.That(Two).Not.IsNotZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is different from zero." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[2]");
        }

        #endregion

        #region IComparable checks

        [Test]
        public void IsBeforeWorks()
        {
            const double Two = 2D;
            const double Twenty = 20D;

            Check.That(Two).IsBefore(Twenty);
        }

        [Test]
        public void NotIsBeforeWorks()
        {
            const double Two = 2D;
            const double Twenty = 20D;

            Check.That(Twenty).Not.IsBefore(Two);
        }

        [Test]
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            const double Two = 2D;
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).IsBefore(Two);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not before the reference value." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[20]" + Environment.NewLine + "The expected value: before" + Environment.NewLine + "\t[2]");
        }

        [Test]
        public void IsBeforeThrowsExceptionWhenGivingTheSameValue()
        {
            const double Two = 2D;

            Check.ThatCode(() =>
            {
                Check.That(Two).IsBefore(Two);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not before the reference value." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[2]" + Environment.NewLine + "The expected value: before" + Environment.NewLine + "\t[2]");
        }

        [Test]
        public void NotIsBeforeThrowsExceptionWhenFailing()
        {
            const double Two = 2D;
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(Two).Not.IsBefore(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is before the reference value whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[2]" + Environment.NewLine + "The expected value: after" + Environment.NewLine + "\t[20]");
        }

        [Test]
        public void IsAfterWorks()
        {
            const double Two = 2D;
            const double Twenty = 20D;

            Check.That(Twenty).IsAfter(Two);
        }

        [Test]
        public void NotIsAfterWorks()
        {
            const double Two = 2D;
            const double Twenty = 20D;

            Check.That(Two).Not.IsAfter(Twenty);
        }

        [Test]
        public void IsAfterThrowsExceptionWhenFailing()
        {
            const double Two = 2D;

            Check.ThatCode(() =>
            {
                Check.That(Two).IsAfter(Two);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not after the reference value." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[2]" + Environment.NewLine + "The expected value: after" + Environment.NewLine + "\t[2]");
        }

        [Test]
        public void NotIsAfterThrowsExceptionWhenFailing()
        {
            const double Two = 2D;
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsAfter(Two);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is after the reference value whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[20]" + Environment.NewLine + "The expected value: before" + Environment.NewLine + "\t[2]");
        }

        #endregion

        #region IsLessThan

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
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(One).Not.IsLessThan(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is less than the threshold." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1]" + Environment.NewLine + "The expected value: more than" + Environment.NewLine + "\t[20]");
        }

        #endregion

        #region IsStrictlyLessThan

        [Test]
        public void IsStrictlyLessThanWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).IsStrictlyLessThan(Twenty);
        }

        [Test]
        public void NotIsStrictlyLessThanWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(Twenty).Not.IsStrictlyLessThan(One);
        }

        [Test]
        public void IsStrictlyLessThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;

            Check.ThatCode(() =>
            {
                Check.That(One).IsStrictlyLessThan(One);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not strictly less than the comparand." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1]" + Environment.NewLine + "The expected value: strictly less than" + Environment.NewLine + "\t[1]");
        }

        [Test]
        public void NotIsStrictlyLessThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(One).Not.IsStrictlyLessThan(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is strictly less than the comparand." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1]" + Environment.NewLine + "The expected value: more than" + Environment.NewLine + "\t[20]");
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
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(One).IsGreaterThan(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is less than the threshold." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1]" + Environment.NewLine + "The expected value: more than" + Environment.NewLine + "\t[20]");
        }

        [Test]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsGreaterThan(One);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is greater than the threshold." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[20]" + Environment.NewLine + "The expected value: less than" + Environment.NewLine + "\t[1]");
        }

        #endregion

        #region IsStrictlyGreaterThan

        [Test]
        public void IsStrictlyGreaterThanWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(Twenty).IsStrictlyGreaterThan(One);
        }

        [Test]
        public void IsStrictlyGreaterThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;

            Check.ThatCode(() =>
            {
                Check.That(One).IsStrictlyGreaterThan(One);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not strictly greater than the comparand." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1]" + Environment.NewLine + "The expected value: more than" + Environment.NewLine + "\t[1]");
        }

        [Test]
        public void NotIsStrictlyGreaterThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsStrictlyGreaterThan(One);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is strictly greater than the comparand." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[20]" + Environment.NewLine + "The expected value: less than or equal to" + Environment.NewLine + "\t[1]");
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const double Twenty = 20D;
            const double Zero = 0D;

            Check.That(Twenty).IsNotZero().And.IsAfter(Zero);
            Check.That(Twenty).IsAfter(Zero).And.IsNotZero();
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
        public void IsEqualFailsWhenRelevant()
        {
            const double Twenty = 20D;

            Check.ThatCode(() =>
                {
                    Check.That(Twenty).IsEqualTo(0);
                })
                .Throws<FluentCheckException>();
        }

        [Test]
        public void EqualsWorksToo()
        {
            const double Twenty = 20D;
            const double OtherTwenty = 20D;
            const double Zero = 0D;

            Check.That(Twenty).Equals(OtherTwenty);

            // check the 'other implementation of equals
            Check.That(Twenty).IsAfter(Zero).And.Equals(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[20] of type: [double]");
        }

        [Test]
        public void NotEqualsThrowsExceptionWhenFailing()
        {
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.Equals(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[20] of type: [double]");
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).IsNotEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[20] of type: [double]");
        }

        [Test]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.ThatCode(() =>
            {
                Check.That(One).Not.IsNotEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is different from the expected one." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\t[20]");
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
        public void HasValueThrowsExceptionWhenFailing()
        {
            double? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).HasAValue();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked nullable has no value, which is unexpected.");
        }

        [Test]
        public void NotHasValueWorks()
        {
            double? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            double? one = 1D;

            Check.ThatCode(() =>
            {
                Check.That(one).Not.HasAValue();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked nullable has a value, which is unexpected." + Environment.NewLine + "The checked nullable:" + Environment.NewLine + "\t[1]");
        }

        [Test]
        public void HasValueSupportsToBeChainedWithTheWhichOperator()
        {
            double? one = 1D;
            const double Zero = 0D;

            Check.That(one).HasAValue().Which.IsAfter(Zero).And.IsEqualTo((double)1);
        }

        [Test]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            double? noValue = null;
            const double Zero = 0D;

            Check.ThatCode(() =>
            {
                Check.That(noValue).Not.HasAValue().Which.IsAfter(Zero);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked nullable has no value to be checked.");
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
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            double? one = 1D;

            Check.ThatCode(() =>
            {
                Check.That(one).HasNoValue();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value has a value, whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1]");
        }

        [Test]
        public void NotHasNoValueWorks()
        {
            double? one = 1D;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            double? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).Not.HasNoValue();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked nullable has no value, which is unexpected.");
        }

        #endregion

        #region IsInstanceOf (which is linkable)

        [Test]
        public void IsInstanceOfWorksWithNullable()
        {
            double? one = 1D;

            Check.That(one).IsInstanceOf<double?>().And.HasAValue().Which.IsEqualTo((double)1);
        }

        [Test]
        public void IsInstanceOfWithNullableIsLinkable()
        {
            double? one = 1D;

            Check.That(one).IsInstanceOf<double?>().And.HasAValue().Which.IsEqualTo((double)1);
            Check.That(one).HasAValue().And.IsInstanceOf<double?>();
        }

        [Test]
        public void NotIsInstanceOfWorksWithNullable()
        {
            double? one = 1D;

            Check.ThatCode(() =>
            {
                Check.That(one).Not.IsInstanceOf<double?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [double?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1] of type: [double?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [double?]");
        }

        [Test]
        public void IsInstanceOfWorksIfValueIsNullButOfSameNullableType()
        {
            double? noValue = null;

            Check.That(noValue).IsInstanceOf<double?>();
        }

        [Test]
        public void NotIsInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            double? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).Not.IsInstanceOf<double?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [double?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[null] of type: [double?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [double?]");
        }

        [Test]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            double? one = null;

            Check.ThatCode(() =>
            {
                Check.That(one).IsInstanceOf<string>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not an instance of [string]." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[null] of type: [double?]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\tan instance of type: [string]");
        }

        #endregion

        #region IsNotInstance

        [Test]
        public void IsNotInstanceOfWorksWithNullable()
        {
            double? one = 1D;

            Check.That(one).IsNotInstanceOf<double>().And.HasAValue().Which.IsEqualTo((double)1);
        }

        [Test]
        public void IsNotInstanceOfThrowsWithValueIsOfSameNullableType()
        {
            double? one = 1D;

            Check.ThatCode(() =>
            {
                Check.That(one).IsNotInstanceOf<double?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [double?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1] of type: [double?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [double?]");
        }

        [Test]
        public void IsNotInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            double? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).IsNotInstanceOf<double?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [double?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[null] of type: [double?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [double?]");
        }

        #endregion

        #endregion
    }
}
