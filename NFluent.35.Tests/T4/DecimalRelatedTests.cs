﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DecimalRelatedTests.cs" company="">
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
    public class DecimalRelatedTests
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
            const decimal Two = 2M;

            Check.That(Two).IsNotZero();
        }

        [Test]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const decimal Zero = 0M;

            Check.ThatCode(() =>
            {
                Check.That(Zero).IsNotZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is equal to zero, whereas it must not.\nThe checked value:\n\t[0]");
        }

        #endregion

        #region NotIsZero

        [Test]
        public void NotIsZeroWorks()
        {
            const decimal Two = 2M;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const decimal Zero = 0M;

            Check.ThatCode(() =>
            {
                Check.That(Zero).Not.IsZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is equal to zero whereas it must not.");
        }

        #endregion

        #region NotIsNotZero

        [Test]
        public void NotIsNotZeroWorks()
        {
            const decimal Zero = 0M;

            Check.That(Zero).Not.IsNotZero();
        }

        [Test]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const decimal Two = 2M;

            Check.ThatCode(() =>
            {
                Check.That(Two).Not.IsNotZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is different from zero.\nThe checked value:\n\t[2]");
        }

        #endregion

        #region IComparable checks

        [Test]
        public void IsBeforeWorks()
        {
            const decimal Two = 2M;
            const decimal Twenty = 20M;

            Check.That(Two).IsBefore(Twenty);
        }

        [Test]
        public void NotIsBeforeWorks()
        {
            const decimal Two = 2M;
            const decimal Twenty = 20M;

            Check.That(Twenty).Not.IsBefore(Two);
        }

        [Test]
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            const decimal Two = 2M;
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).IsBefore(Two);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not before the reference value.\nThe checked value:\n\t[20]\nThe expected value: before\n\t[2]");
        }

        [Test]
        public void IsBeforeThrowsExceptionWhenGivingTheSameValue()
        {
            const decimal Two = 2M;

            Check.ThatCode(() =>
            {
                Check.That(Two).IsBefore(Two);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not before the reference value.\nThe checked value:\n\t[2]\nThe expected value: before\n\t[2]");
        }

        [Test]
        public void NotIsBeforeThrowsExceptionWhenFailing()
        {
            const decimal Two = 2M;
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(Two).Not.IsBefore(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is before the reference value whereas it must not.\nThe checked value:\n\t[2]\nThe expected value: after\n\t[20]");
        }

        [Test]
        public void IsAfterWorks()
        {
            const decimal Two = 2M;
            const decimal Twenty = 20M;

            Check.That(Twenty).IsAfter(Two);
        }

        [Test]
        public void NotIsAfterWorks()
        {
            const decimal Two = 2M;
            const decimal Twenty = 20M;

            Check.That(Two).Not.IsAfter(Twenty);
        }

        [Test]
        public void IsAfterThrowsExceptionWhenFailing()
        {
            const decimal Two = 2M;

            Check.ThatCode(() =>
            {
                Check.That(Two).IsAfter(Two);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not after the reference value.\nThe checked value:\n\t[2]\nThe expected value: after\n\t[2]");
        }

        [Test]
        public void NotIsAfterThrowsExceptionWhenFailing()
        {
            const decimal Two = 2M;
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsAfter(Two);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is after the reference value whereas it must not.\nThe checked value:\n\t[20]\nThe expected value: before\n\t[2]");
        }

        #endregion

        #region IsLessThan

        [Test]
        public void IsLessThanWorks()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.That(One).IsLessThan(Twenty);
        }

        [Test]
        public void NotIsLessThanWorks()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.That(Twenty).Not.IsLessThan(One);
        }

        [Test]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(One).Not.IsLessThan(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]");
        }

        #endregion

        #region IsStrictlyLessThan

        [Test]
        public void IsStrictlyLessThanWorks()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.That(One).IsStrictlyLessThan(Twenty);
        }

        [Test]
        public void NotIsStrictlyLessThanWorks()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.That(Twenty).Not.IsStrictlyLessThan(One);
        }

        [Test]
        public void IsStrictlyLessThanThrowsExceptionWhenFailing()
        {
            const decimal One = 1M;

            Check.ThatCode(() =>
            {
                Check.That(One).IsStrictlyLessThan(One);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not strictly less than the comparand.\nThe checked value:\n\t[1]\nThe expected value: strictly less than\n\t[1]");
        }

        [Test]
        public void NotIsStrictlyLessThanThrowsExceptionWhenFailing()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(One).Not.IsStrictlyLessThan(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is strictly less than the comparand.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]");
        }

        #endregion



        #region IsGreaterThan

        [Test]
        public void IsGreaterThanWorks()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(One).IsGreaterThan(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]");
        }

        [Test]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsGreaterThan(One);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is greater than the threshold.\nThe checked value:\n\t[20]\nThe expected value: less than\n\t[1]");
        }

        #endregion

        #region IsStrictlyGreaterThan

        [Test]
        public void IsStrictlyGreaterThanWorks()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.That(Twenty).IsStrictlyGreaterThan(One);
        }

        [Test]
        public void IsStrictlyGreaterThanThrowsExceptionWhenFailing()
        {
            const decimal One = 1M;

            Check.ThatCode(() =>
            {
                Check.That(One).IsStrictlyGreaterThan(One);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not strictly greater than the comparand.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[1]");
        }

        [Test]
        public void NotIsStrictlyGreaterThanThrowsExceptionWhenFailing()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsStrictlyGreaterThan(One);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is strictly greater than the comparand.\nThe checked value:\n\t[20]\nThe expected value: less than or equal to\n\t[1]");
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const decimal Twenty = 20M;
            const decimal Zero = 0M;

            Check.That(Twenty).IsNotZero().And.IsAfter(Zero);
            Check.That(Twenty).IsAfter(Zero).And.IsNotZero();
        }

        #region Equals / IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const decimal Twenty = 20M;
            const decimal OtherTwenty = 20M;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void IsEqualFailsWhenRelevant()
        {
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
                {
                    Check.That(Twenty).IsEqualTo(0);
                })
                .Throws<FluentCheckException>();
        }

        [Test]
        public void EqualsWorksToo()
        {
            const decimal Twenty = 20M;
            const decimal OtherTwenty = 20M;
            const decimal Zero = 0M;

            Check.That(Twenty).Equals(OtherTwenty);

            // check the 'other implementation of equals
            Check.That(Twenty).IsAfter(Zero).And.Equals(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [decimal]");
        }

        [Test]
        public void NotEqualsThrowsExceptionWhenFailing()
        {
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.Equals(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [decimal]");
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).IsNotEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [decimal]");
        }

        [Test]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const decimal One = 1M;
            const decimal Twenty = 20M;

            Check.ThatCode(() =>
            {
                Check.That(One).Not.IsNotEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is different from the expected one.\nThe checked value:\n\t[1]\nThe expected value:\n\t[20]");
        }

        #endregion

        #region Nullables

        #region HasAValue

        [Test]
        public void HasValueWorks()
        {
            decimal? one = 1M;

            Check.That(one).HasAValue();
        }

        [Test]
        public void HasValueThrowsExceptionWhenFailing()
        {
            decimal? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).HasAValue();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked nullable has no value, which is unexpected.");
        }

        [Test]
        public void NotHasValueWorks()
        {
            decimal? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            decimal? one = 1M;

            Check.ThatCode(() =>
            {
                Check.That(one).Not.HasAValue();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked nullable has a value, which is unexpected.\nThe checked nullable:\n\t[1]");
        }

        [Test]
        public void HasValueSupportsToBeChainedWithTheWhichOperator()
        {
            decimal? one = 1M;
            const decimal Zero = 0M;

            Check.That(one).HasAValue().Which.IsAfter(Zero).And.IsEqualTo((decimal)1);
        }

        [Test]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            decimal? noValue = null;
            const decimal Zero = 0M;

            Check.ThatCode(() =>
            {
                Check.That(noValue).Not.HasAValue().Which.IsAfter(Zero);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked nullable has no value to be checked.");
        }

        #endregion

        #region HasNoValue
        
        [Test]
        public void HasNoValueWorks()
        {
            decimal? noValue = null;

            Check.That(noValue).HasNoValue();
        }

        [Test]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            decimal? one = 1M;

            Check.ThatCode(() =>
            {
                Check.That(one).HasNoValue();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value has a value, whereas it must not.\nThe checked value:\n\t[1]");
        }

        [Test]
        public void NotHasNoValueWorks()
        {
            decimal? one = 1M;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            decimal? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).Not.HasNoValue();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked nullable has no value, which is unexpected.");
        }

        #endregion

        #region IsInstanceOf (which is linkable)

        [Test]
        public void IsInstanceOfWorksWithNullable()
        {
            decimal? one = 1M;

            Check.That(one).IsInstanceOf<decimal?>().And.HasAValue().Which.IsEqualTo((decimal)1);
        }

        [Test]
        public void IsInstanceOfWithNullableIsLinkable()
        {
            decimal? one = 1M;

            Check.That(one).IsInstanceOf<decimal?>().And.HasAValue().Which.IsEqualTo((decimal)1);
            Check.That(one).HasAValue().And.IsInstanceOf<decimal?>();
        }

        [Test]
        public void NotIsInstanceOfWorksWithNullable()
        {
            decimal? one = 1M;

            Check.ThatCode(() =>
            {
                Check.That(one).Not.IsInstanceOf<decimal?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is an instance of [decimal?] whereas it must not.\nThe checked value:\n\t[1] of type: [decimal?]\nThe expected value: different from\n\tan instance of type: [decimal?]");
        }

        [Test]
        public void IsInstanceOfWorksIfValueIsNullButOfSameNullableType()
        {
            decimal? noValue = null;

            Check.That(noValue).IsInstanceOf<decimal?>();
        }

        [Test]
        public void NotIsInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            decimal? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).Not.IsInstanceOf<decimal?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is an instance of [decimal?] whereas it must not.\nThe checked value:\n\t[null] of type: [decimal?]\nThe expected value: different from\n\tan instance of type: [decimal?]");
        }

        [Test]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            decimal? one = null;

            Check.ThatCode(() =>
            {
                Check.That(one).IsInstanceOf<string>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not an instance of [string].\nThe checked value:\n\t[null] of type: [decimal?]\nThe expected value:\n\tan instance of type: [string]");
        }

        #endregion

        #region IsNotInstance

        [Test]
        public void IsNotInstanceOfWorksWithNullable()
        {
            decimal? one = 1M;

            Check.That(one).IsNotInstanceOf<decimal>().And.HasAValue().Which.IsEqualTo((decimal)1);
        }

        [Test]
        public void IsNotInstanceOfThrowsWithValueIsOfSameNullableType()
        {
            decimal? one = 1M;

            Check.ThatCode(() =>
            {
                Check.That(one).IsNotInstanceOf<decimal?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is an instance of [decimal?] whereas it must not.\nThe checked value:\n\t[1] of type: [decimal?]\nThe expected value: different from\n\tan instance of type: [decimal?]");
        }

        [Test]
        public void IsNotInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            decimal? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).IsNotInstanceOf<decimal?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is an instance of [decimal?] whereas it must not.\nThe checked value:\n\t[null] of type: [decimal?]\nThe expected value: different from\n\tan instance of type: [decimal?]");
        }

        #endregion

        #endregion
    }
}
