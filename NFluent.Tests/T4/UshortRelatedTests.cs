namespace NFluent.Tests
{
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

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
            const ushort Zero = 0;

            Check.That(Zero).IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[2] of type: [System.UInt16]\nis not equal to zero.")]
        public void IsZeroThrowsExceptionWhenFails()
        {
            const ushort Two = 2;

            Check.That(Two).IsZero();
        }

        #endregion 

        #region IsNotZero

        [Test]
        public void IsNotZeroWorks()
        {
            const ushort Two = 2;

            Check.That(Two).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.UInt16]\nis equal to zero.")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const ushort Zero = 0;

            Check.That(Zero).IsNotZero();
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to zero which is unexpected.")]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const ushort Zero = 0;

            Check.That(Zero).Not.IsZero();
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.UInt16]\nis not equal to zero which is unexpected.")]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const ushort Two = 2;
            Check.That(Two).Not.IsNotZero();
        }

        #endregion

        #region IsPositive

        [Test]
        public void IsPositiveWorks()
        {
            const ushort Two = 2;

            Check.That(Two).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.UInt16]\nis not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const ushort Zero = 0;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.UInt16]\nis a strictly positive value, which is unexpected.")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const ushort Two = 2;

            Check.That(Two).Not.IsPositive();
        }

        #endregion

        #region IsLessThan & Co

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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis less than than:\n\t[20]\nwhich was not expected.")]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).Not.IsLessThan(Twenty);
        }

        #endregion

        #region IsGreaterThan

        [Test]
        public void IsGreaterThanWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(Twenty).IsGreaterThan(One);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis not greater than:\n\t[20].")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).IsGreaterThan(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[20]\nis greater than:\n\t[1]\nwhich is unexpected.")]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(Twenty).Not.IsGreaterThan(One);
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const ushort Twenty = 20;

            Check.That(Twenty).IsNotZero().And.IsPositive();
            Check.That(Twenty).IsPositive().And.IsNotZero();
        }

        #region IsEqualTo / IsNotEqualTo

        [Test]
        public void IsEqualToWorksWithOtherSameValue()
        {
            const ushort Twenty = 20;
            const ushort OtherTwenty = 20;

            Check.That(Twenty).IsEqualTo(OtherTwenty);
        }

        [Test]
        public void NotIsEqualToWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), MatchType = MessageMatch.Regex, ExpectedMessage = Blabla + "(\\[20])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + LineFeed + Blabla + "(\\[20\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + "unexpected.")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const ushort Twenty = 20;

            Check.That(Twenty).Not.IsEqualTo(Twenty);
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[20] of type: [System.UInt16].")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const ushort Twenty = 20;

            Check.That(Twenty).IsNotEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis not equal to the expected one:\n\t[20].")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const ushort One = 1;
            const ushort Twenty = 20;

            Check.That(One).Not.IsNotEqualTo(Twenty);
        }

        #endregion

        #region Nullables

        #region IsNull (which is not chainable by the way; on purpose)

        [Test]
        public void IsNullWorksWithNullable()
        {
            ushort? noValue = null;

            Check.That(noValue).IsNull();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nis not null as expected.")]
        public void IsNullThrowsExceptionWhenFailingWithNullable()
        {
            ushort? one = 1;

            Check.That(one).IsNull();
        }

        [Test]
        public void NotIsNullWorksWithNullable()
        {
            ushort? one = 1;

            Check.That(one).Not.IsNull();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value is null which is unexpected.")]
        public void NotIsNullThrowsExceptionWhenFailingWithNullable()
        {
            ushort? noValue = null;

            Check.That(noValue).Not.IsNull();
        }

        #endregion

        #region IsNotNull (which is not chainable by the way, since Not.IsNotNull() can work with nullable without Value => which can't be chained afterward)

        [Test]
        public void IsNotNullWorks()
        {
            ushort? one = 1;

            Check.That(one).IsNotNull();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value is null which is unexpected.")]
        public void IsNotNullThrowsExceptionWhenFailing()
        {
            ushort? noValue = null;

            Check.That(noValue).IsNotNull();
        }

        [Test]
        public void NotIsNotNullWorks()
        {
            ushort? noValue = null;

            Check.That(noValue).Not.IsNotNull();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked nullable value:\n\t[1]\nis not null which is unexpected.")]
        public void NotIsNotNullThrowsExceptionWhenFailing()
        {
            ushort? one = 1;

            Check.That(one).Not.IsNotNull();
        }

        #endregion

        #region IsInstanceOf (which is chainable)

        [Test]
        public void IsInstanceOfWorksWithNullable()
        {
            ushort? one = 1;

            Check.That(one).IsInstanceOf<ushort?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis an instance of:\n\t[System.Nullable`1[System.UInt16]]\nwhich was not expected.")]
        public void NotIsInstanceOfWorksWithNullable()
        {
            ushort? one = 1;

            Check.That(one).Not.IsInstanceOf<ushort?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis an instance of:\n\t[System.Nullable`1[System.UInt16]]\nwhich was not expected.")]
        public void NotIsInstanceOfWorksWithNullableWithoutValue()
        {
            ushort? noValue = null;

            Check.That(noValue).Not.IsInstanceOf<ushort?>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis not an instance of:\n\t[System.String]\nbut an instance of:\n\t[System.Nullable`1[System.UInt16]]\ninstead.")]
        public void IsInstanceOfThowsExceptionWhenFailingWithNullable()
        {
            ushort? one = null;

            Check.That(one).IsInstanceOf<string>();
        }

        #endregion

        #region IsEqualTo

        [Test]
        public void IsEqualToWorksWithNullable()
        {
            ushort? one = 1;

            Check.That(one).IsEqualTo((ushort)1);
        }

        [Test]
        public void NotIsEqualToWorksWithNullable()
        {
            ushort? one = 1;
            const ushort Twenty = 20;

            Check.That(one).Not.IsEqualTo(Twenty).And.IsPositive().And.Not.IsGreaterThan(1).And.IsGreaterThan(0);
        }

        [Test]
        public void IsEqualToWithNullableIsChainableWithStandardIntFluentAssertions()
        {
            ushort? one = 1;

            Check.That(one).IsEqualTo(one).And.IsPositive().And.Not.IsGreaterThan(1).And.IsGreaterThan(0);
        }

        [Test]
        public void IsEqualToWorksWithNullableWithoutValue()
        {
            ushort? noValue = null;

            Check.That(noValue).IsEqualTo(null);
        }

        [Test]
        public void NotIsEqualToWorksWithNullableWithoutValue()
        {
            ushort? noValue = null;
            const ushort Twenty = 20;

            Check.That(noValue).Not.IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis not equal to the expected one:\n\t[20].")]
        public void IsEqualToThrowsExceptionWithNullableWithoutValue()
        {
            ushort? noValue = null;
            const ushort Twenty = 20;

            Check.That(noValue).IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis equal to:\n\t[null]\nwhich is unexpected.")]
        public void NotIsEqualToThrowsExceptionWithNullableWithoutValue()
        {
            ushort? noValue = null;

            Check.That(noValue).Not.IsEqualTo(null);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis not equal to the expected one:\n\t[20].")]
        public void IsEqualToThrowsExceptionWhenFailingWithNullableOfSameType()
        {
            ushort? one = 1;
            const ushort Twenty = 20;

            Check.That(one).IsEqualTo(Twenty);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), MatchType = MessageMatch.Regex, ExpectedMessage = Blabla + "(\\[1])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + LineFeed + Blabla + "(\\[1\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + "unexpected.")]
        public void NotIsEqualToThrowsExceptionWhenFailingWithNullableOfSameType()
        {
            ushort? one = 1;

            Check.That(one).Not.IsEqualTo((ushort)1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1] of type: [System.UInt16]\nis not equal to the expected one:\n\t[\"What da...\"] of type: [System.String].")]
        public void IsEqualToThrowsExceptionWhenFailingWithNullable()
        {
            ushort? one = 1;

            Check.That(one).IsEqualTo("What da...");
        }

        [Test]
        public void NotIsEqualToWorksWithNullableAndOtherTypeOfValue()
        {
            ushort? one = 1;

            Check.That(one).Not.IsEqualTo("What da...");
        }

        #endregion

        #endregion
    }
}
