namespace NFluent.Tests
{
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class UintRelatedTests
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

        #region IsZero

        [Test]
        public void IsZeroWorks()
        {
            const uint Zero = 0;

            Check.That(Zero).IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[2] of type: [System.UInt32]\nis not equal to zero.")]
        public void IsZeroThrowsExceptionWhenFails()
        {
            const uint Two = 2;

            Check.That(Two).IsZero();
        }

        #endregion 

        #region IsNotZero

        [Test]
        public void IsNotZeroWorks()
        {
            const uint Two = 2;

            Check.That(Two).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.UInt32]\nis equal to zero.")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const uint Zero = 0;

            Check.That(Zero).IsNotZero();
        }

        #endregion

        #region NotIsZero

        [Test]
        public void NotIsZeroWorks()
        {
            const uint Two = 2;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to zero which is unexpected.")]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const uint Zero = 0;

            Check.That(Zero).Not.IsZero();
        }

        #endregion

        #region NotIsNotZero

        [Test]
        public void NotIsNotZeroWorks()
        {
            const uint Zero = 0;

            Check.That(Zero).Not.IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.UInt32]\nis not equal to zero which is unexpected.")]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const uint Two = 2;
            Check.That(Two).Not.IsNotZero();
        }

        #endregion

        #region IsPositive

        [Test]
        public void IsPositiveWorks()
        {
            const uint Two = 2;

            Check.That(Two).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.UInt32]\nis not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const uint Zero = 0;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.UInt32]\nis a strictly positive value, which is unexpected.")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const uint Two = 2;

            Check.That(Two).Not.IsPositive();
        }

        #endregion

        #region IsLessThan & Co

        [Test]
        public void IsLessThanWorks()
        {
            const uint One = 1;
            const uint Twenty = 20;

            Check.That(One).IsLessThan(Twenty);
        }

        [Test]
        public void NotIsLessThanWorks()
        {
            const uint One = 1;
            const uint Twenty = 20;

            Check.That(Twenty).Not.IsLessThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis less than than:\n\t[20]\nwhich was not expected.")]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const uint One = 1;
            const uint Twenty = 20;

            Check.That(One).Not.IsLessThan(Twenty);
        }

        #endregion

        #region IsGreaterThan

        [Test]
        public void IsGreaterThanWorks()
        {
            const uint One = 1;
            const uint Twenty = 20;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis not greater than:\n\t[20].")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const uint One = 1;
            const uint Twenty = 20;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[20]\nis greater than:\n\t[1]\nwhich is unexpected.")]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            const uint One = 1;
            const uint Twenty = 20;

            Check.That(Twenty).Not.IsGreaterThan(One);
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const uint Twenty = 20;

            Check.That(Twenty).IsNotZero().And.IsPositive();
            Check.That(Twenty).IsPositive().And.IsNotZero();
        }

        #region IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const uint Twenty = 20;
            const uint OtherTwenty = 20;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const uint One = 1;
            const uint Twenty = 20;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [System.UInt32]")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const uint Twenty = 20;

            Check.That(Twenty).Not.IsEqualTo(Twenty);
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const uint One = 1;
            const uint Twenty = 20;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [System.UInt32]")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const uint Twenty = 20;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is different from the expected one.\nThe checked value:\n\t[1]\nThe expected value:\n\t[20]")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const uint One = 1;
            const uint Twenty = 20;

            Check.That(One).Not.IsNotEqualTo(Twenty);
        }

        #endregion

        #region Nullables

        #region HasAValue

        [Test]
        public void HasValueWorks()
        {
            uint? one = 1;

            Check.That(one).HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void HasValueThrowsExceptionWhenFailing()
        {
            uint? noValue = null;

            Check.That(noValue).HasAValue();
        }

        [Test]
        public void NotHasValueWorks()
        {
            uint? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            uint? one = 1;

            Check.That(one).Not.HasAValue();
        }

        [Test]
        public void HasValueSupportsToBeChainedWithTheWhichOperator()
        {
            uint? one = 1;

            Check.That(one).HasAValue().Which.IsPositive().And.IsEqualTo((uint)1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable has no value to be checked.")]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            uint? noValue = null;

            Check.That(noValue).Not.HasAValue().Which.IsPositive();
        }

        #endregion

        #region HasNoValue
        
        [Test]
        public void HasNoValueWorks()
        {
            uint? noValue = null;

            Check.That(noValue).HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            uint? one = 1;

            Check.That(one).HasNoValue();
        }

        [Test]
        public void NotHasNoValueWorks()
        {
            uint? one = 1;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            uint? noValue = null;

            Check.That(noValue).Not.HasNoValue();
        }

        #endregion

        #region IsInstanceOf (which is chainable)

        [Test]
        public void IsInstanceOfWorksWithNullable()
        {
            uint? one = 1;

            Check.That(one).IsInstanceOf<uint?>().And.HasAValue().Which.IsEqualTo((uint)1);
        }

        [Test]
        public void IsInstanceOfWithNullableIsChainable()
        {
            uint? one = 1;

            Check.That(one).IsInstanceOf<uint?>().And.HasAValue().Which.IsEqualTo((uint)1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is an instance of System.Nullable`1[System.UInt32] whereas it must not.\nThe checked value:\n\t[1] of type: [System.Nullable`1[System.UInt32]]\nThe expected type: different from\n\t[System.Nullable`1[System.UInt32]]")]
        public void NotIsInstanceOfWorksWithNullable()
        {
            uint? one = 1;

            Check.That(one).Not.IsInstanceOf<uint?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is an instance of System.Nullable`1[System.UInt32] whereas it must not.\nThe checked value:\n\t[null] of type: [System.Nullable`1[System.UInt32]]\nThe expected type: different from\n\t[System.Nullable`1[System.UInt32]]")]
        public void NotIsInstanceOfWorksWithNullableWithoutValue()
        {
            uint? noValue = null;

            Check.That(noValue).Not.IsInstanceOf<uint?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is not an instance of System.String.\nThe checked value:\n\t[null] of type: [System.Nullable`1[System.UInt32]]\nThe expected type:\n\t[System.String]")]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            uint? one = null;

            Check.That(one).IsInstanceOf<string>();
        }

        #endregion

        #endregion
    }
}
