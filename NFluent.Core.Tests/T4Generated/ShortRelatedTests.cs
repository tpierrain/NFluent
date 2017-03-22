// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ShortRelatedTests.cs" company="">
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
    using NUnit.Framework;
    using NFluent.Tests.Helpers;

    [TestFixture]
    public class ShortRelatedTests
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------

        // Since this class is the model/template for the generation of the tests on all the other numbers types, don't forget to re-generate all the other classes every time you change this one. To do that, just save the .\T4" + Environment.NewLine + "umberTestsGenerator.tt file within Visual Studio 2012. This will trigger the T4 code generation process.
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
            const short Two = 2;

            Check.That(Two).IsNotZero();
        }

        [Test]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const short Zero = 0;

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
            const short Two = 2;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const short Zero = 0;

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
            const short Zero = 0;

            Check.That(Zero).Not.IsNotZero();
        }

        [Test]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const short Two = 2;

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
            const short Two = 2;
            const short Twenty = 20;

            Check.That(Two).IsBefore(Twenty);
        }

        [Test]
        public void NotIsBeforeWorks()
        {
            const short Two = 2;
            const short Twenty = 20;

            Check.That(Twenty).Not.IsBefore(Two);
        }

        [Test]
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            const short Two = 2;
            const short Twenty = 20;

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
            const short Two = 2;

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
            const short Two = 2;
            const short Twenty = 20;

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
            const short Two = 2;
            const short Twenty = 20;

            Check.That(Twenty).IsAfter(Two);
        }

        [Test]
        public void NotIsAfterWorks()
        {
            const short Two = 2;
            const short Twenty = 20;

            Check.That(Two).Not.IsAfter(Twenty);
        }

        [Test]
        public void IsAfterThrowsExceptionWhenFailing()
        {
            const short Two = 2;

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
            const short Two = 2;
            const short Twenty = 20;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsAfter(Two);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is after the reference value whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[20]" + Environment.NewLine + "The expected value: before" + Environment.NewLine + "\t[2]");
        }

        #endregion

        #region IsLessThan
#pragma warning disable 618

        [Test]
        public void IsLessThanWorks()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).IsLessThan(Twenty);
        }

        [Test]
        public void NotIsLessThanWorks()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(Twenty).Not.IsLessThan(One);
        }

        [Test]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.ThatCode(() =>
            {
                Check.That(One).Not.IsLessThan(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is less than the threshold." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1]" + Environment.NewLine + "The expected value: more than" + Environment.NewLine + "\t[20]");
        }

#pragma warning restore 618
        #endregion

        #region IsStrictlyLessThan

        [Test]
        public void IsStrictlyLessThanWorks()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).IsStrictlyLessThan(Twenty);
        }

        [Test]
        public void NotIsStrictlyLessThanWorks()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(Twenty).Not.IsStrictlyLessThan(One);
        }

        [Test]
        public void IsStrictlyLessThanThrowsExceptionWhenFailing()
        {
            const short One = 1;

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
            const short One = 1;
            const short Twenty = 20;

            Check.ThatCode(() =>
            {
                Check.That(One).Not.IsStrictlyLessThan(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is strictly less than the comparand." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1]" + Environment.NewLine + "The expected value: more than" + Environment.NewLine + "\t[20]");
        }

        #endregion



        #region IsGreaterThan
#pragma warning disable 618

        [Test]
        public void IsGreaterThanWorks()
        {
            const short One = 1;
            const short Twenty = 20;
            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const short One = 1;
            const short Twenty = 20;

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
            const short One = 1;
            const short Twenty = 20;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsGreaterThan(One);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is greater than the threshold." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[20]" + Environment.NewLine + "The expected value: less than" + Environment.NewLine + "\t[1]");
        }

#pragma warning restore 618
        #endregion

        #region IsStrictlyGreaterThan

        [Test]
        public void IsStrictlyGreaterThanWorks()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(Twenty).IsStrictlyGreaterThan(One);
        }

        [Test]
        public void IsStrictlyGreaterThanThrowsExceptionWhenFailing()
        {
            const short One = 1;

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
            const short One = 1;
            const short Twenty = 20;

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
            const short Twenty = 20;
            const short Zero = 0;

            Check.That(Twenty).IsNotZero().And.IsAfter(Zero);
            Check.That(Twenty).IsAfter(Zero).And.IsNotZero();
        }

        #region Equals / IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const short Twenty = 20;
            const short OtherTwenty = 20;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void IsEqualFailsWhenRelevant()
        {
            const short Twenty = 20;

            Check.ThatCode(() =>
                {
                    Check.That(Twenty).IsEqualTo(0);
                })
                .Throws<FluentCheckException>();
        }

        [Test]
        public void EqualsWorksToo()
        {
            const short Twenty = 20;
            const short OtherTwenty = 20;
            const short Zero = 0;

            Check.That(Twenty).Equals(OtherTwenty);

            // check the 'other implementation of equals
            Check.That(Twenty).IsAfter(Zero).And.Equals(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const short Twenty = 20;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[20] of type: [short]");
        }

        [Test]
        public void NotEqualsThrowsExceptionWhenFailing()
        {
            const short Twenty = 20;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.Equals(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[20] of type: [short]");
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const short Twenty = 20;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).IsNotEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[20] of type: [short]");
        }

        [Test]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const short One = 1;
            const short Twenty = 20;

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
            short? one = 1;

            Check.That(one).HasAValue();
        }

        [Test]
        public void HasValueThrowsExceptionWhenFailing()
        {
            short? noValue = null;

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
            short? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            short? one = 1;

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
            short? one = 1;
            const short Zero = 0;

            Check.That(one).HasAValue().Which.IsAfter(Zero).And.IsEqualTo((short)1);
        }

        [Test]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            short? noValue = null;
            const short Zero = 0;

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
            short? noValue = null;

            Check.That(noValue).HasNoValue();
        }

        [Test]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            short? one = 1;

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
            short? one = 1;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            short? noValue = null;

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
            short? one = 1;

            Check.That(one).IsInstanceOf<short?>().And.HasAValue().Which.IsEqualTo((short)1);
        }

        [Test]
        public void IsInstanceOfWithNullableIsLinkable()
        {
            short? one = 1;

            Check.That(one).IsInstanceOf<short?>().And.HasAValue().Which.IsEqualTo((short)1);
            Check.That(one).HasAValue().And.IsInstanceOf<short?>();
        }

        [Test]
        public void NotIsInstanceOfWorksWithNullable()
        {
            short? one = 1;

            Check.ThatCode(() =>
            {
                Check.That(one).Not.IsInstanceOf<short?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [short?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1] of type: [short?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [short?]");
        }

        [Test]
        public void IsInstanceOfWorksIfValueIsNullButOfSameNullableType()
        {
            short? noValue = null;

            Check.That(noValue).IsInstanceOf<short?>();
        }

        [Test]
        public void NotIsInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            short? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).Not.IsInstanceOf<short?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [short?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[null] of type: [short?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [short?]");
        }

        [Test]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            short? one = null;

            Check.ThatCode(() =>
            {
                Check.That(one).IsInstanceOf<string>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not an instance of [string]." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[null] of type: [short?]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\tan instance of type: [string]");
        }

        #endregion

        #region IsNotInstance

        [Test]
        public void IsNotInstanceOfWorksWithNullable()
        {
            short? one = 1;

            Check.That(one).IsNotInstanceOf<short>().And.HasAValue().Which.IsEqualTo((short)1);
        }

        [Test]
        public void IsNotInstanceOfThrowsWithValueIsOfSameNullableType()
        {
            short? one = 1;

            Check.ThatCode(() =>
            {
                Check.That(one).IsNotInstanceOf<short?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [short?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1] of type: [short?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [short?]");
        }

        [Test]
        public void IsNotInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            short? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).IsNotInstanceOf<short?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [short?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[null] of type: [short?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [short?]");
        }

        #endregion

        #endregion
    }
}
