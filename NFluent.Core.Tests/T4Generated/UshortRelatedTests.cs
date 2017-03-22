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
using System;

namespace NFluent.Tests
{
    using NUnit.Framework;
    using NFluent.Tests.Helpers;

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
            const ushort Two = 2;

            Check.That(Two).IsNotZero();
        }

        [Test]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const ushort Zero = 0;

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
            const ushort Two = 2;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const ushort Zero = 0;

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
            const ushort Zero = 0;

            Check.That(Zero).Not.IsNotZero();
        }

        [Test]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const ushort Two = 2;

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
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            const ushort Two = 2;
            const ushort Twenty = 20;

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
            const ushort Two = 2;

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
            const ushort Two = 2;
            const ushort Twenty = 20;

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
        public void IsAfterThrowsExceptionWhenFailing()
        {
            const ushort Two = 2;

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
            const ushort Two = 2;
            const ushort Twenty = 20;

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
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

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
        public void IsStrictlyLessThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;

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
            const ushort One = 1;
            const ushort Twenty = 20;

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
            const ushort One = 1;
            const ushort Twenty = 20;
            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

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
            const ushort One = 1;
            const ushort Twenty = 20;

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
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(Twenty).IsStrictlyGreaterThan(One);
        }

        [Test]
        public void IsStrictlyGreaterThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;

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
            const ushort One = 1;
            const ushort Twenty = 20;

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
        public void IsEqualFailsWhenRelevant()
        {
            const ushort Twenty = 20;

            Check.ThatCode(() =>
                {
                    Check.That(Twenty).IsEqualTo(0);
                })
                .Throws<FluentCheckException>();
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
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const ushort Twenty = 20;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[20] of type: [ushort]");
        }

        [Test]
        public void NotEqualsThrowsExceptionWhenFailing()
        {
            const ushort Twenty = 20;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.Equals(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[20] of type: [ushort]");
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const ushort Twenty = 20;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).IsNotEqualTo(Twenty);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[20] of type: [ushort]");
        }

        [Test]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

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
            ushort? one = 1;

            Check.That(one).HasAValue();
        }

        [Test]
        public void HasValueThrowsExceptionWhenFailing()
        {
            ushort? noValue = null;

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
            ushort? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            ushort? one = 1;

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
            ushort? one = 1;
            const ushort Zero = 0;

            Check.That(one).HasAValue().Which.IsAfter(Zero).And.IsEqualTo((ushort)1);
        }

        [Test]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            ushort? noValue = null;
            const ushort Zero = 0;

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
            ushort? noValue = null;

            Check.That(noValue).HasNoValue();
        }

        [Test]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            ushort? one = 1;

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
            ushort? one = 1;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            ushort? noValue = null;

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
        public void NotIsInstanceOfWorksWithNullable()
        {
            ushort? one = 1;

            Check.ThatCode(() =>
            {
                Check.That(one).Not.IsInstanceOf<ushort?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [ushort?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1] of type: [ushort?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [ushort?]");
        }

        [Test]
        public void IsInstanceOfWorksIfValueIsNullButOfSameNullableType()
        {
            ushort? noValue = null;

            Check.That(noValue).IsInstanceOf<ushort?>();
        }

        [Test]
        public void NotIsInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            ushort? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).Not.IsInstanceOf<ushort?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [ushort?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[null] of type: [ushort?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [ushort?]");
        }

        [Test]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            ushort? one = null;

            Check.ThatCode(() =>
            {
                Check.That(one).IsInstanceOf<string>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not an instance of [string]." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[null] of type: [ushort?]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\tan instance of type: [string]");
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
        public void IsNotInstanceOfThrowsWithValueIsOfSameNullableType()
        {
            ushort? one = 1;

            Check.ThatCode(() =>
            {
                Check.That(one).IsNotInstanceOf<ushort?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [ushort?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[1] of type: [ushort?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [ushort?]");
        }

        [Test]
        public void IsNotInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            ushort? noValue = null;

            Check.ThatCode(() =>
            {
                Check.That(noValue).IsNotInstanceOf<ushort?>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [ushort?] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[null] of type: [ushort?]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [ushort?]");
        }

        #endregion

        #endregion
    }
}
