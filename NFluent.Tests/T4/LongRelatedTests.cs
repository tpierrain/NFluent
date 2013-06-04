namespace NFluent.Tests
{
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class LongRelatedTests
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
            const long Zero = 0L;

            Check.That(Zero).IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[2] of type: [System.Int64]\nis not equal to zero.")]
        public void IsZeroThrowsExceptionWhenFails()
        {
            const long Two = 2L;

            Check.That(Two).IsZero();
        }

        #endregion 

        #region IsNotZero

        [Test]
        public void IsNotZeroWorks()
        {
            const long Two = 2L;

            Check.That(Two).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.Int64]\nis equal to zero.")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const long Zero = 0L;

            Check.That(Zero).IsNotZero();
        }

        #endregion

        #region NotIsZero

        [Test]
        public void NotIsZeroWorks()
        {
            const long Two = 2L;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to zero which is unexpected.")]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const long Zero = 0L;

            Check.That(Zero).Not.IsZero();
        }

        #endregion

        #region NotIsNotZero

        [Test]
        public void NotIsNotZeroWorks()
        {
            const long Zero = 0L;

            Check.That(Zero).Not.IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.Int64]\nis not equal to zero which is unexpected.")]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const long Two = 2L;
            Check.That(Two).Not.IsNotZero();
        }

        #endregion

        #region IsPositive

        [Test]
        public void IsPositiveWorks()
        {
            const long Two = 2L;

            Check.That(Two).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.Int64]\nis not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const long Zero = 0L;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.Int64]\nis a strictly positive value, which is unexpected.")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const long Two = 2L;

            Check.That(Two).Not.IsPositive();
        }

        #endregion

        #region IsLessThan & Co

        [Test]
        public void IsLessThanWorks()
        {
            const long One = 1L;
            const long Twenty = 20L;

            Check.That(One).IsLessThan(Twenty);
        }

        [Test]
        public void NotIsLessThanWorks()
        {
            const long One = 1L;
            const long Twenty = 20L;

            Check.That(Twenty).Not.IsLessThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis less than than:\n\t[20]\nwhich was not expected.")]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const long One = 1L;
            const long Twenty = 20L;

            Check.That(One).Not.IsLessThan(Twenty);
        }

        #endregion

        #region IsGreaterThan

        [Test]
        public void IsGreaterThanWorks()
        {
            const long One = 1L;
            const long Twenty = 20L;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis not greater than:\n\t[20].")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const long One = 1L;
            const long Twenty = 20L;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[20]\nis greater than:\n\t[1]\nwhich is unexpected.")]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            const long One = 1L;
            const long Twenty = 20L;

            Check.That(Twenty).Not.IsGreaterThan(One);
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const long Twenty = 20L;

            Check.That(Twenty).IsNotZero().And.IsPositive();
            Check.That(Twenty).IsPositive().And.IsNotZero();
        }

        #region IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const long Twenty = 20L;
            const long OtherTwenty = 20L;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const long One = 1L;
            const long Twenty = 20L;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), MatchType = MessageMatch.Regex, ExpectedMessage = Blabla + "(\\[20])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + LineFeed + Blabla + "(\\[20\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + "unexpected.")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const long Twenty = 20L;

            Check.That(Twenty).Not.IsEqualTo(Twenty);
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const long One = 1L;
            const long Twenty = 20L;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[20] of type: [System.Int64].")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const long Twenty = 20L;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis not equal to the expected one:\n\t[20].")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const long One = 1L;
            const long Twenty = 20L;

            Check.That(One).Not.IsNotEqualTo(Twenty);
        }

        #endregion

        #region Nullables

        #region HasValue

        [Test]
        public void HasValueWorks()
        {
            long? one = 1L;

            Check.That(one).HasValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void HasValueThrowsExceptionWhenFailing()
        {
            long? noValue = null;

            Check.That(noValue).HasValue();
        }

        [Test]
        public void NotHasValueWorks()
        {
            long? noValue = null;

            Check.That(noValue).Not.HasValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void NotHasValueThrowsExceptionWhenFailing()
        {
            long? one = 1L;

            Check.That(one).Not.HasValue();
        }

        // TODO add Which statement support here
        #endregion

        #region HasNoValue
        
        [Test]
        public void HasNoValueWorks()
        {
            long? noValue = null;

            Check.That(noValue).HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nhas a value, which is unexpected.")]
        public void HasNoValueThrowsExceptionWhenFailing()
        {
            long? one = 1L;

            Check.That(one).HasNoValue();
        }

        [Test]
        public void NotHasNoValueWorks()
        {
            long? one = 1L;

            Check.That(one).Not.HasNoValue();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value has no value, which is unexpected.")]
        public void NotHasNoValueThrowsExceptionWhenFailing()
        {
            long? noValue = null;

            Check.That(noValue).Not.HasNoValue();
        }

        #endregion

        #region IsInstanceOf (which is chainable)

        [Test]
        public void IsInstanceOfWorksWithNullable()
        {
            long? one = 1L;

            Check.That(one).IsInstanceOf<long?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis an instance of:\n\t[System.Nullable`1[System.Int64]]\nwhich was not expected.")]
        public void NotIsInstanceOfWorksWithNullable()
        {
            long? one = 1L;

            Check.That(one).Not.IsInstanceOf<long?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis an instance of:\n\t[System.Nullable`1[System.Int64]]\nwhich was not expected.")]
        public void NotIsInstanceOfWorksWithNullableWithoutValue()
        {
            long? noValue = null;

            Check.That(noValue).Not.IsInstanceOf<long?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis not an instance of:\n\t[System.String]\nbut an instance of:\n\t[System.Nullable`1[System.Int64]]\ninstead.")]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            long? one = null;

            Check.That(one).IsInstanceOf<string>();
        }

        #endregion

        #region IsEqualTo

        [Test]
        public void IsEqualToWorksWithNullable()
        {
            long? one = 1L;

            Check.That(one).IsEqualTo((long)1);
        }

        [Test]
        public void NotIsEqualToWorksWithNullable()
        {
            long? one = 1L;
            const long Twenty = 20L;

            Check.That(one).Not.IsEqualTo(Twenty).And.IsPositive().And.Not.IsGreaterThan(1L).And.IsGreaterThan(0);
        }

        [Test]
        public void IsEqualToWithNullableIsChainableWithStandardIntFluentAssertions()
        {
            long? one = 1L;

            Check.That(one).IsEqualTo(one).And.IsPositive().And.Not.IsGreaterThan(1L).And.IsGreaterThan(0);
        }

        [Test]
        public void IsEqualToWorksWithNullableWithoutValue()
        {
            long? noValue = null;

            Check.That(noValue).IsEqualTo(null);
        }

        [Test]
        public void NotIsEqualToWorksWithNullableWithoutValue()
        {
            long? noValue = null;
            const long Twenty = 20L;

            Check.That(noValue).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis not equal to the expected one:\n\t[20].")]
        public void IsEqualToThrowsExceptionWithNullableWithoutValue()
        {
            long? noValue = null;
            const long Twenty = 20L;

            Check.That(noValue).IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis equal to:\n\t[null]\nwhich is unexpected.")]
        public void NotIsEqualToThrowsExceptionWithNullableWithoutValue()
        {
            long? noValue = null;

            Check.That(noValue).Not.IsEqualTo(null);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis not equal to the expected one:\n\t[20].")]
        public void IsEqualToThrowsExceptionWhenFailingWithNullableOfSameType()
        {
            long? one = 1L;
            const long Twenty = 20L;

            Check.That(one).IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), MatchType = MessageMatch.Regex, ExpectedMessage = Blabla + "(\\[1])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + LineFeed + Blabla + "(\\[1\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + "unexpected.")]
        public void NotIsEqualToThrowsExceptionWhenFailingWithNullableOfSameType()
        {
            long? one = 1L;

            Check.That(one).Not.IsEqualTo((long)1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1] of type: [System.Int64]\nis not equal to the expected one:\n\t[\"What da...\"] of type: [System.String].")]
        public void IsEqualToThrowsExceptionWhenFailingWithNullable()
        {
            long? one = 1L;

            Check.That(one).IsEqualTo("What da...");
        }

        [Test]
        public void NotIsEqualToWorksWithNullableAndOtherTypeOfValue()
        {
            long? one = 1L;

            Check.That(one).Not.IsEqualTo("What da...");
        }

        #endregion

        #endregion
    }
}
