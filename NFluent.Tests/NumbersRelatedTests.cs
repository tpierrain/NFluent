namespace NFluent.Tests
{
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class NumbersRelatedTests
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
            const int IntZero = 0;
            const long LongZero = 0L;
            const double DoubleZero = 0D;
            const decimal DecimalZero = 0M;
            const float FloatZero = 0F;
            const short ShortZero = 0;
            const byte ByteZero = 0;
            const uint UintZero = 0;
            const ushort UshortZero = 0;
            const ulong UlongZero = 0;

            Check.That(IntZero).IsZero();
            Check.That(LongZero).IsZero();
            Check.That(DoubleZero).IsZero();

            Check.That(DecimalZero).IsZero();
            Check.That(FloatZero).IsZero();
            Check.That(ShortZero).IsZero();
            Check.That(ByteZero).IsZero();
            Check.That(UintZero).IsZero();
            Check.That(UshortZero).IsZero();
            Check.That(UlongZero).IsZero();
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
            const long LongValue = 2L;
            const double DoubleValue = 2D;
            const decimal DecimalValue = 2M;
            const float FloatValue = 2F;
            const short ShortValue = 2;
            const byte ByteValue = 2;
            const uint UintValue = 2;
            const ushort UshortValue = 2;
            const ulong UlongValue = 2;

            Check.That(IntValue).IsNotZero();
            Check.That(LongValue).IsNotZero();
            Check.That(DoubleValue).IsNotZero();
            Check.That(DecimalValue).IsNotZero();
            Check.That(FloatValue).IsNotZero();
            Check.That(ShortValue).IsNotZero();
            Check.That(ByteValue).IsNotZero();
            Check.That(UintValue).IsNotZero();
            Check.That(UshortValue).IsNotZero();
            Check.That(UlongValue).IsNotZero();
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
            const int IntValue = 2;
            const long LongValue = 2L;
            const double DoubleValue = 2D;
            const decimal DecimalValue = 2M;
            const float FloatValue = 2F;
            const short ShortValue = 2;
            const byte ByteValue = 2;
            const uint UintValue = 2;
            const ushort UshortValue = 2;
            const ulong UlongValue = 2;

            Check.That(IntValue).Not.IsZero();
            Check.That(LongValue).Not.IsZero();
            Check.That(DoubleValue).Not.IsZero();
            Check.That(DecimalValue).Not.IsZero();
            Check.That(FloatValue).Not.IsZero();
            Check.That(ShortValue).Not.IsZero();
            Check.That(ByteValue).Not.IsZero();
            Check.That(UintValue).Not.IsZero();
            Check.That(UshortValue).Not.IsZero();
            Check.That(UlongValue).Not.IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to zero which is unexpected.")]
        public void NotIsZeroThrowsExceptionWhenFailing()
        {
            const byte ByteValue = 0;
            Check.That(ByteValue).Not.IsZero();
        }

        #endregion

        #region NotIsNotZero

        [Test]
        public void NotIsNotZeroWorks()
        {
            const int IntZero = 0;
            const long LongZero = 0L;
            const double DoubleZero = 0D;
            const decimal DecimalZero = 0M;
            const float FloatZero = 0F;
            const short ShortZero = 0;
            const byte ByteZero = 0;
            const uint UintZero = 0;
            const ushort UshortZero = 0;
            const ulong UlongZero = 0;

            Check.That(IntZero).Not.IsNotZero();
            Check.That(LongZero).Not.IsNotZero();
            Check.That(DoubleZero).Not.IsNotZero();

            Check.That(DecimalZero).Not.IsNotZero();
            Check.That(FloatZero).Not.IsNotZero();
            Check.That(ShortZero).Not.IsNotZero();
            Check.That(ByteZero).Not.IsNotZero();
            Check.That(UintZero).Not.IsNotZero();
            Check.That(UshortZero).Not.IsNotZero();
            Check.That(UlongZero).Not.IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[2] of type: [System.Byte]\nis not equal to zero which is unexpected.")]
        public void NotIsNotZeroThrowsExceptionWhenFailing()
        {
            const byte ByteValue = 2;
            Check.That(ByteValue).Not.IsNotZero();
        }

        #endregion

        #region IsPositive

        [Test]
        public void IsPositiveWorks()
        {
            const int Two = 2;
            Check.That(Two).IsPositive();

            Check.That((byte)1).IsPositive();

            Check.That((decimal)1).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[0] of type: [System.Single]\nis not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const float Zero = 0F;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[-50] of type: [System.Double]\nis not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenValueIsNegative()
        {
            const double NegativeDouble = -50D;

            Check.That(NegativeDouble).IsPositive();
        }

        [Test]
        public void NotIsPositiveWorks()
        {
            const double NegativeDouble = -50D;

            Check.That(NegativeDouble).Not.IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[3] of type: [System.Double]\nis a strictly positive value, which is unexpected.")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const double PositiveDouble = 3D;

            Check.That(PositiveDouble).Not.IsPositive();
        }

        #endregion

        #region IsLessThan & Co

        [Test]
        public void IsLessThanWorksForDouble()
        {
            const double SmallDouble = 1.0D;
            const double BigDouble = 37.2D;

            Check.That(SmallDouble).IsLessThan(BigDouble);
        }

        [Test]
        public void NotIsLessThanWorksForDouble()
        {
            const double SmallDouble = 1.0D;
            const double BigDouble = 37.2D;

            Check.That(BigDouble).Not.IsLessThan(SmallDouble);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis less than than:\n\t[37,2]\nwhich was not expected.")]
        public void NotIsLessThanThrowsExceptionWhenFailing()
        {
            const double SmallDouble = 1.0D;
            const double BigDouble = 37.2D;

            Check.That(SmallDouble).Not.IsLessThan(BigDouble);
        }

        #endregion

        [Test]
        public void IsGreaterThanWorksForDouble()
        {
            const double SmallDouble = 1.0D;
            const double BigDouble = 37.2D;

            Check.That(BigDouble).IsGreaterThan(SmallDouble);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[1]\nis not greater than:\n\t[37,2].")]
        public void IsGreaterThanThrowsExceptionWhenFailingWithDouble()
        {
            const double SmallDouble = 1.0D;
            const double BigDouble = 37.2D;

            Check.That(SmallDouble).IsGreaterThan(BigDouble);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value:\n\t[37,2]\nis greater than:\n\t[1]\nwhich is unexpected.")]
        public void NotIsGreaterThanThrowsExceptionWhenFailingWithDouble()
        {
            const double SmallDouble = 1.0D;
            const double BigDouble = 37.2D;

            Check.That(BigDouble).Not.IsGreaterThan(SmallDouble);
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const double DoubleNumber = 37.2D;

            Check.That(DoubleNumber).IsNotZero().And.IsPositive();
            Check.That(DoubleNumber).IsPositive().And.IsNotZero();
        }

        #region IsEqualTo / IsNotEqualTo

        [Test]
        public void NotIsEqualToWorks()
        {
            const int IntValue = 42;
            const long LongValue = 21L;

            Check.That(IntValue).Not.IsEqualTo(LongValue);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), MatchType = MessageMatch.Regex, ExpectedMessage = Blabla + "(\\[42])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + LineFeed + Blabla + "(\\[42\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + "unexpected.")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            const int IntValue = 42;

            Check.That(IntValue).Not.IsEqualTo(IntValue);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[42] of type: [System.Int32]\nis not equal to the expected one:\n\t[42] of type: [System.Int64].")]
        public void IsEqualToThrowsWhenSameNumberOfDifferentTypes()
        {
            const int IntValue = 42;
            const long LongValue = 42L;

            Check.That(IntValue).IsEqualTo(LongValue);
        }

        [Test]
        public void IsEqualToWorksWithDecimal()
        {
            const decimal Value = 42;
            const decimal SameValue = 42;

            Check.That(Value).IsEqualTo(SameValue);
        }

        [Test]
        public void IsNotEqualToWorksWithDecimal()
        {
            const decimal Value = 42;
            const decimal DifferentValue = 13;

            Check.That(Value).IsNotEqualTo(DifferentValue);
        }

        [Test]
        public void IsEqualToWorksWithByte()
        {
            const byte Value = 2;
            const byte SameValue = 2;

            Check.That(Value).IsEqualTo(SameValue);
        }

        [Test]
        public void IsNotEqualToWorksWithByte()
        {
            const byte Value = 42;
            const byte DifferentValue = 13;

            Check.That(Value).IsNotEqualTo(DifferentValue);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[42] of type: [System.Byte].")]
        public void IsNotEqualToThrowsExceptionWhenFailing()
        {
            const byte Value = 42;

            Check.That(Value).IsNotEqualTo(Value);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[42]\nis not equal to the expected one:\n\t[13].")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            const byte Value = 42;
            const byte DifferentValue = 13;

            Check.That(Value).Not.IsNotEqualTo(DifferentValue);
        }

        #endregion
    }
}
