namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class NumbersRelatedTests
    {
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[9] is not equal to zero.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[0] is equal to zero.")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            const double Zero = 0D;

            Check.That(Zero).IsNotZero();
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[0] is not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const float Zero = 0F;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[-50] is not a strictly positive value.")]
        public void IsPositiveThrowsExceptionWhenValueIsNegative()
        {
            const double NegativeDouble = -50D;

            Check.That(NegativeDouble).IsPositive();
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnNumber()
        {
            const double DoubleNumber = 37.2D;

            Check.That(DoubleNumber).IsNotZero().And.IsPositive();
            Check.That(DoubleNumber).IsPositive().And.IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nWas:\n\t[42] of type: [System.Int32]\n instead of the expected:\n\t[42] of type: [System.Int64].")]
        public void IsEqualToThrowsWhenSameNumberOfDifferentTypes()
        {
            const int IntValue = 42;
            const long LongValue = 42L;

            Check.That(IntValue).IsEqualTo(LongValue);
        }
    }
}
