namespace NFluent.Tests
{
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

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
            const short Zero = 0;

            Check.That(Zero).IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is different from zero.\nThe checked value:\n\t[2]")]
        public void IsZeroThrowsExceptionWhenFails()
        {
            const short Two = 2;

            Check.That(Two).IsZero();
        }

        #endregion 

        #region IsNotZero

        [Test]
        public void IsNotZeroWorks()
        {
            const short Two = 2;

            Check.That(Two).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is equal to zero, whereas it must not.\nThe checked value:\n\t[0]")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const short Zero = 0;

            Check.That(Zero).IsNotZero();
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is equal to zero whereas it must not.")]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const short Zero = 0;

            Check.That(Zero).Not.IsZero();
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is different from zero.\nThe checked value:\n\t[2]")]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const short Two = 2;
            Check.That(Two).Not.IsNotZero();
        }

        #endregion

        #region IsPositive

        [Test]
        public void IsPositiveWorks()
        {
            const short Two = 2;

            Check.That(Two).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is not strictly positive.\nThe checked value:\n\t[0]")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const short Zero = 0;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is positive, whereas it must not.\nThe checked value:\n\t[2]")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const short Two = 2;

            Check.That(Two).Not.IsPositive();
        }

        #endregion

        #region IsLessThan & Co

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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).Not.IsLessThan(Twenty);
        }

        #endregion

        #region IsGreaterThan

        [Test]
        public void IsGreaterThanWorks()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is less than the threshold.\nThe checked value:\n\t[1]\nThe expected value: more than\n\t[20]")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is greater than the threshold.\nThe checked value:\n\t[20]\nThe expected value: less than\n\t[1]")]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(Twenty).Not.IsGreaterThan(One);
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const short Twenty = 20;

            Check.That(Twenty).IsNotZero().And.IsPositive();
            Check.That(Twenty).IsPositive().And.IsNotZero();
        }

        #region Equals should always throw

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nEquals method should not be called in this context since it is not a fluent assertion. Too bad we can't remove it from Intellisense or redirect it to the proper IsEqualTo method.")]
        public void EqualsShouldAlwaysThrowAnExceptionToAvoidConfusionWithIsEqualToAssertion()
        {
            const short Twenty = 20;
            const short OtherTwenty = 20;

            Check.That(Twenty).Equals(OtherTwenty);
        }

        #endregion

        #region IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const short Twenty = 20;
            const short OtherTwenty = 20;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [System.Int16]")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const short Twenty = 20;

            Check.That(Twenty).Not.IsEqualTo(Twenty);
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[20] of type: [System.Int16]")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const short Twenty = 20;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[1]\nThe expected value:\n\t[20]")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).Not.IsNotEqualTo(Twenty);
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void HasValueThrowsExceptionWhenFailing()
        {
            short? noValue = null;

            Check.That(noValue).HasAValue();
        }

        [Test]
        public void NotHasValueWorks()
        {
            short? noValue = null;

            Check.That(noValue).Not.HasAValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            short? one = 1;

            Check.That(one).Not.HasAValue();
        }

        [Test]
        public void HasValueSupportsToBeChainedWithTheWhichOperator()
        {
            short? one = 1;

            Check.That(one).HasAValue().Which.IsPositive().And.IsEqualTo((short)1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable has no value to be checked.")]
        public void TryingToChainANullableWithoutAValueIsPossibleButThrowsAnException()
        {
            short? noValue = null;

            Check.That(noValue).Not.HasAValue().Which.IsPositive();
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            short? one = 1;

            Check.That(one).HasNoValue();
        }

        [Test]
        public void NotHasNoValueWorks()
        {
            short? one = 1;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            short? noValue = null;

            Check.That(noValue).Not.HasNoValue();
        }

        #endregion

        #region IsInstanceOf (which is chainable)

        [Test]
        public void IsInstanceOfWorksWithNullable()
        {
            short? one = 1;

            Check.That(one).IsInstanceOf<short?>().And.HasAValue().Which.IsEqualTo((short)1);
        }

        [Test]
        public void IsInstanceOfWithNullableIsChainable()
        {
            short? one = 1;

            Check.That(one).IsInstanceOf<short?>().And.HasAValue().Which.IsEqualTo((short)1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is an instance of System.Nullable`1[System.Int16] whereas it must not.\nThe checked value:\n\t[1] of type: [System.Nullable`1[System.Int16]]\nThe expected type: different from\n\t[System.Nullable`1[System.Int16]]")]
        public void NotIsInstanceOfWorksWithNullable()
        {
            short? one = 1;

            Check.That(one).Not.IsInstanceOf<short?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is an instance of System.Nullable`1[System.Int16] whereas it must not.\nThe checked value:\n\t[null] of type: [System.Nullable`1[System.Int16]]\nThe expected type: different from\n\t[System.Nullable`1[System.Int16]]")]
        public void NotIsInstanceOfWorksWithNullableWithoutValue()
        {
            short? noValue = null;

            Check.That(noValue).Not.IsInstanceOf<short?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is not an instance of System.String.\nThe checked value:\n\t[null] of type: [System.Nullable`1[System.Int16]]\nThe expected type:\n\t[System.String]")]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            short? one = null;

            Check.That(one).IsInstanceOf<string>();
        }

        #endregion

        #endregion
    }
}
