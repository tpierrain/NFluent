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
            int nine = 9;

            Check.That(nine).IsZero();
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
            double zero = 0D;

            Check.That(zero).IsNotZero();
        }

        #endregion 

        #region IsPositive

        [Test]
        public void IsPositiveWorks()
        {
            int two = 2;
            Check.That(two).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[0] is not a strictly positive value.")]
        public void IsPositiveThrowExceptionWhenEqualToZero()
        {
            float zero = 0F;
            Check.That(zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[-50] is not a strictly positive value.")]
        public void IsPositiveThrowExceptionWhenValueIsNegative()
        {
            double negativeDouble = -50D;

            Check.That(negativeDouble).IsPositive();
        }

        #endregion
    }
}
