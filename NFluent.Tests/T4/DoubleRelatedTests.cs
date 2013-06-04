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

        #region IsZero

        [Test]
        public void IsZeroWorks()
        {
            const double Zero = 0D;

            Check.That(Zero).IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[2] of type: [System.Double]\nis not equal to zero.")]
        public void IsZeroThrowsExceptionWhenFails()
        {
            const double Two = 2D;

            Check.That(Two).IsZero();
        }

        #endregion 

        #region IsNotZero

        [Test]
        public void IsNotZeroWorks()
        {
            const double Two = 2D;

            Check.That(Two).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.Double]\nis equal to zero.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to zero which is unexpected.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.Double]\nis not equal to zero which is unexpected.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.Double]\nis not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const double Zero = 0D;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.Double]\nis a strictly positive value, which is unexpected.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis less than than:\n\t[20]\nwhich was not expected.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis not greater than:\n\t[20].")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[20]\nis greater than:\n\t[1]\nwhich is unexpected.")]
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

        #region IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const double Twenty = 20D;
            const double OtherTwenty = 20D;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"The checked value is equal to the expected one whereas it must not.
The expected value: different from
	[20] of type: [System.Double]")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const double Twenty = 20D;

            Check.That(Twenty).Not.IsEqualTo(Twenty);
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const double One = 1D;
            const double Twenty = 20D;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"The checked value is equal to the expected one whereas it must not.
The expected value: different from
	[20] of type: [System.Double]")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const double Twenty = 20D;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"The checked value is different from the expected one.
The checked value:
	[1]
The expected value:
	[20]")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis an instance of:\n\t[System.Nullable`1[System.Double]]\nwhich was not expected.")]
        public void NotIsInstanceOfWorksWithNullable()
        {
            double? one = 1D;

            Check.That(one).Not.IsInstanceOf<double?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis an instance of:\n\t[System.Nullable`1[System.Double]]\nwhich was not expected.")]
        public void NotIsInstanceOfWorksWithNullableWithoutValue()
        {
            double? noValue = null;

            Check.That(noValue).Not.IsInstanceOf<double?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis not an instance of:\n\t[System.String]\nbut an instance of:\n\t[System.Nullable`1[System.Double]]\ninstead.")]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            double? one = null;

            Check.That(one).IsInstanceOf<string>();
        }

        #endregion

        #endregion
    }
}
