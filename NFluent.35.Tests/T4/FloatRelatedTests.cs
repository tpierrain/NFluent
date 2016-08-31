// // --------------------------------------------------------------------------------------------------------------------
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
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const float Zero = 0F;

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
            const float Two = 2F;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const float Zero = 0F;

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
            const float Zero = 0F;

            Check.That(Zero).Not.IsNotZero();
        }

        [Test]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const float Two = 2F;

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
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            const float Two = 2F;
            const float Twenty = 20F;

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
            const float Two = 2F;

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
            const float Two = 2F;
            const float Twenty = 20F;

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
        public void IsAfterThrowsExceptionWhenFailing()
        {
            const float Two = 2F;

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
            const float Two = 2F;
            const float Twenty = 20F;

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
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const float One = 1F;
            const float Twenty = 20F;

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
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(One).IsStrictlyLessThan(Twenty);
        }

        [Test]
        public void NotIsStrictlyLessThanWorks()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(Twenty).Not.IsStrictlyLessThan(One);
        }

        [Test]
        public void IsStrictlyLessThanThrowsExceptionWhenFailing()
        {
            const float One = 1F;

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
            const float One = 1F;
            const float Twenty = 20F;

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
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const float One = 1F;
            const float Twenty = 20F;

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
            const float One = 1F;
            const float Twenty = 20F;

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
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(Twenty).IsStrictlyGreaterThan(One);
        }

        [Test]
        public void IsStrictlyGreaterThanThrowsExceptionWhenFailing()
        {
            const float One = 1F;

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
            const float One = 1F;
            const float Twenty = 20F;

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
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const float Twenty = 20F;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [float]");
        }

        [Test]
        public void NotEqualsThrowsExceptionWhenFailing()
        {
            const float Twenty = 20F;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.Equals(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [float]");
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const float One = 1F;
            const float Twenty = 20F;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const float Twenty = 20F;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).IsNotEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [float]");
        }

        [Test]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const float One = 1F;
            const float Twenty = 20F;

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
            float? one = 1F;

            Check.That(one).HasAValue();
        }

        [Test]
        public void HasValueThrowsExceptionWhenFailing()
        {
            float? noValue = null;

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
            float? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            float? one = 1F;

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
            float? one = 1F;
            const float Zero = 0F;

            Check.That(one).HasAValue().Which.IsAfter(Zero).And.IsEqualTo((float)1);
        }

        [Test]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            float? noValue = null;
            const float Zero = 0F;

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
            float? noValue = null;

            Check.That(noValue).HasNoValue();
        }

        [Test]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            float? one = 1F;

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
            float? one = 1F;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            float? noValue = null;

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
        public void NotIsInstanceOfWorksWithNullable()
        {
            float? one = 1F;

            Check.ThatCode(() =>
            {
                Check.That(one).Not.IsInstanceOf<float?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is an instance of [float?] whereas it must not.\nThe checked value:\n\t[1] of type: [float?]\nThe expected value: different from\n\tan instance of type: [float?]");
        }

        [Test]
        public void IsInstanceOfWorksIfValueIsNullButOfSameNullableType()
        {
            float? noValue = null;

            Check.That(noValue).IsInstanceOf<float?>();
        }

        [Test]
        public void NotIsInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            float? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).Not.IsInstanceOf<float?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is an instance of [float?] whereas it must not.\nThe checked value:\n\t[null] of type: [float?]\nThe expected value: different from\n\tan instance of type: [float?]");
        }

        [Test]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            float? one = null;

            Check.ThatCode(() =>
            {
                Check.That(one).IsInstanceOf<string>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is not an instance of [string].\nThe checked value:\n\t[null] of type: [float?]\nThe expected value:\n\tan instance of type: [string]");
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
        public void IsNotInstanceOfThrowsWithValueIsOfSameNullableType()
        {
            float? one = 1F;

            Check.ThatCode(() =>
            {
                Check.That(one).IsNotInstanceOf<float?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is an instance of [float?] whereas it must not.\nThe checked value:\n\t[1] of type: [float?]\nThe expected value: different from\n\tan instance of type: [float?]");
        }

        [Test]
        public void IsNotInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            float? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).IsNotInstanceOf<float?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value is an instance of [float?] whereas it must not.\nThe checked value:\n\t[null] of type: [float?]\nThe expected value: different from\n\tan instance of type: [float?]");
        }

        #endregion

        #endregion
    }
}
