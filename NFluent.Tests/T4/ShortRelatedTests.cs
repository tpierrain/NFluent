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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[2] of type: [System.Int16]\nis not equal to zero.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.Int16]\nis equal to zero.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to zero which is unexpected.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.Int16]\nis not equal to zero which is unexpected.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.Int16]\nis not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const short Zero = 0;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.Int16]\nis a strictly positive value, which is unexpected.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis less than than:\n\t[20]\nwhich was not expected.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis not greater than:\n\t[20].")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[20]\nis greater than:\n\t[1]\nwhich is unexpected.")]
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
        [ExpectedException(typeof(FluentAssertionException), MatchType = MessageMatch.Regex, ExpectedMessage = Blabla + "(\\[20])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + LineFeed + Blabla + "(\\[20\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + "unexpected.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[20] of type: [System.Int16].")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const short Twenty = 20;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis not equal to the expected one:\n\t[20].")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const short One = 1;
            const short Twenty = 20;

            Check.That(One).Not.IsNotEqualTo(Twenty);
        }

        #endregion

        #region Nullables

        #region HasValue

        [Test]
        public void HasValueWorks()
        {
            short? one = 1;

            Check.That(one).HasValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void HasValueThrowsExceptionWhenFailing()
        {
            short? noValue = null;

            Check.That(noValue).HasValue();
        }

        [Test]
        public void NotHasValueWorks()
        {
            short? noValue = null;

            Check.That(noValue).Not.HasValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            short? one = 1;

            Check.That(one).Not.HasValue();
        }

        // TODO add Which statement support here
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

            Check.That(one).IsInstanceOf<short?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis an instance of:\n\t[System.Nullable`1[System.Int16]]\nwhich was not expected.")]
        public void NotIsInstanceOfWorksWithNullable()
        {
            short? one = 1;

            Check.That(one).Not.IsInstanceOf<short?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis an instance of:\n\t[System.Nullable`1[System.Int16]]\nwhich was not expected.")]
        public void NotIsInstanceOfWorksWithNullableWithoutValue()
        {
            short? noValue = null;

            Check.That(noValue).Not.IsInstanceOf<short?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis not an instance of:\n\t[System.String]\nbut an instance of:\n\t[System.Nullable`1[System.Int16]]\ninstead.")]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            short? one = null;

            Check.That(one).IsInstanceOf<string>();
        }

        #endregion

        #region IsEqualTo

        [Test]
        public void IsEqualToWorksWithNullable()
        {
            short? one = 1;

            Check.That(one).IsEqualTo((short)1);
        }

        [Test]
        public void NotIsEqualToWorksWithNullable()
        {
            short? one = 1;
            const short Twenty = 20;

            Check.That(one).Not.IsEqualTo(Twenty).And.IsPositive().And.Not.IsGreaterThan(1).And.IsGreaterThan(0);
        }

        [Test]
        public void IsEqualToWithNullableIsChainableWithStandardIntFluentAssertions()
        {
            short? one = 1;

            Check.That(one).IsEqualTo(one).And.IsPositive().And.Not.IsGreaterThan(1).And.IsGreaterThan(0);
        }

        [Test]
        public void IsEqualToWorksWithNullableWithoutValue()
        {
            short? noValue = null;

            Check.That(noValue).IsEqualTo(null);
        }

        [Test]
        public void NotIsEqualToWorksWithNullableWithoutValue()
        {
            short? noValue = null;
            const short Twenty = 20;

            Check.That(noValue).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis not equal to the expected one:\n\t[20].")]
        public void IsEqualToThrowsExceptionWithNullableWithoutValue()
        {
            short? noValue = null;
            const short Twenty = 20;

            Check.That(noValue).IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis equal to:\n\t[null]\nwhich is unexpected.")]
        public void NotIsEqualToThrowsExceptionWithNullableWithoutValue()
        {
            short? noValue = null;

            Check.That(noValue).Not.IsEqualTo(null);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis not equal to the expected one:\n\t[20].")]
        public void IsEqualToThrowsExceptionWhenFailingWithNullableOfSameType()
        {
            short? one = 1;
            const short Twenty = 20;

            Check.That(one).IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), MatchType = MessageMatch.Regex, ExpectedMessage = Blabla + "(\\[1])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + LineFeed + Blabla + "(\\[1\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + "unexpected.")]
        public void NotIsEqualToThrowsExceptionWhenFailingWithNullableOfSameType()
        {
            short? one = 1;

            Check.That(one).Not.IsEqualTo((short)1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1] of type: [System.Int16]\nis not equal to the expected one:\n\t[\"What da...\"] of type: [System.String].")]
        public void IsEqualToThrowsExceptionWhenFailingWithNullable()
        {
            short? one = 1;

            Check.That(one).IsEqualTo("What da...");
        }

        [Test]
        public void NotIsEqualToWorksWithNullableAndOtherTypeOfValue()
        {
            short? one = 1;

            Check.That(one).Not.IsEqualTo("What da...");
        }

        #endregion

        #endregion
    }
}
