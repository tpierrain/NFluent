namespace NFluent.Tests
{
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class IntRelatedTests
    {
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
            const int Zero = 0;

            Check.That(Zero).IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[9] of type: [System.Int32]\nis not equal to zero.")]
        public void IsZeroThrowsExceptionWhenFails()
        {
            const int Nine = 9;

            Check.That(Nine).IsZero();
        }

        #endregion 

        #region IsNotZero

        [Test]
        public void IsNotZeroWorks()
        {
            const int IntValue = 2;

            Check.That(IntValue).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.Int32]\nis equal to zero.")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const int Zero = 0;

            Check.That(Zero).IsNotZero();
        }

        #endregion

        #region NotIsZero

        [Test]
        public void NotIsZeroWorks()
        {
            const int Two = 2;

            Check.That(Two).Not.IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to zero which is unexpected.")]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const int Zero = 0;

            Check.That(Zero).Not.IsZero();
        }

        #endregion

        #region NotIsNotZero

        [Test]
        public void NotIsNotZeroWorks()
        {
            const int IntZero = 0;

            Check.That(IntZero).Not.IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.Int32]\nis not equal to zero which is unexpected.")]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const int Two = 2;
            Check.That(Two).Not.IsNotZero();
        }

        #endregion

        #region IsPositive

        [Test]
        public void IsPositiveWorks()
        {
            const int Two = 2;

            Check.That(Two).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.Int32]\nis not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const int Zero = 0;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[-50] of type: [System.Int32]\nis not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenValueIsNegative()
        {
            const int MinusFifty = -50;

            Check.That(MinusFifty).IsPositive();
        }

        [Test]
        public void NotIsPositiveWorks()
        {
            const int MinusFifty = -50;

            Check.That(MinusFifty).Not.IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.Int32]\nis a strictly positive value, which is unexpected.")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const int Two = 2;

            Check.That(Two).Not.IsPositive();
        }

        #endregion

        #region IsLessThan & Co

        [Test]
        public void IsLessThanWorks()
        {
            const int One = 1;
            const int Twenty = 20;

            Check.That(One).IsLessThan(Twenty);
        }

        [Test]
        public void NotIsLessThanWorksForDouble()
        {
            const int One = 1;
            const int Twenty = 20;

            Check.That(Twenty).Not.IsLessThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis less than than:\n\t[20]\nwhich was not expected.")]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const int One = 1;
            const int Twenty = 20;

            Check.That(One).Not.IsLessThan(Twenty);
        }

        #endregion

        [Test]
        public void IsGreaterThanWorksForDouble()
        {
            const int One = 1;
            const int Twenty = 20;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis not greater than:\n\t[20].")]
        public void IsGreaterThanThrowsExceptionWhenFailingWithDouble()
        {
            const int One = 1;
            const int Twenty = 20;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[20]\nis greater than:\n\t[1]\nwhich is unexpected.")]
        public void NotIsGreaterThanThrowsExceptionWhenFailingWithDouble()
        {
            const int One = 1;
            const int Twenty = 20;

            Check.That(Twenty).Not.IsGreaterThan(One);
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const int Twenty = 20;

            Check.That(Twenty).IsNotZero().And.IsPositive();
            Check.That(Twenty).IsPositive().And.IsNotZero();
        }

        #region IsEqualTo / IsNotEqualTo

        [Test]
        public void NotIsEqualToWorks()
        {
            const int One = 1;
            const int Twenty = 20;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), MatchType = MessageMatch.Regex, ExpectedMessage = Blabla + "(\\[20])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + LineFeed + Blabla + "(\\[20\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + "unexpected.")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const int Twenty = 20;

            Check.That(Twenty).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[20] of type: [System.Int32].")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const int Twenty = 20;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis not equal to the expected one:\n\t[20].")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const int One = 1;
            const int Twenty = 20;

            Check.That(One).Not.IsNotEqualTo(Twenty);
        }

        #endregion
    }
}
