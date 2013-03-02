namespace NFluent.Tests
{
    using NUnit.Framework;

    using Assert = NFluent.Assert;

    [TestFixture]
    public class NumbersRelatedTests
    {
        #region IsZero

        [Test]
        public void IsZeroWorks()
        {
            int intValue = 0;
            long longValue = 0L;
            double doubleValue = 0D;
            decimal decimalValue = 0M;
            float floatValue = 0F;
            short shortValue = 0;
            byte byteValue = 0;
            uint uintValue = 0;
            ushort ushortValue = 0;
            ulong ulongValue = 0;

            Assert.That(intValue).IsZero();
            Assert.That(longValue).IsZero();
            Assert.That(doubleValue).IsZero();
            Assert.That(decimalValue).IsZero();
            Assert.That(floatValue).IsZero();
            Assert.That(shortValue).IsZero();
            Assert.That(byteValue).IsZero();
            Assert.That(uintValue).IsZero();
            Assert.That(ushortValue).IsZero();
            Assert.That(ulongValue).IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[9] is not equal to zero.")]
        public void IsZeroThrowsExceptionWhenFails()
        {
            int a = 9;

            Assert.That(a).IsZero();
        }

        #endregion 

        #region IsNotZero

        [Test]
        public void IsNotZeroWorks()
        {
            int intValue = 2;
            long longValue = 2L;
            double doubleValue = 2D;
            decimal decimalValue = 2M;
            float floatValue = 2F;
            short shortValue = 2;
            byte byteValue = 2;
            uint uintValue = 2;
            ushort ushortValue = 2;
            ulong ulongValue = 2;

            Assert.That(intValue).IsNotZero();
            Assert.That(longValue).IsNotZero();
            Assert.That(doubleValue).IsNotZero();
            Assert.That(decimalValue).IsNotZero();
            Assert.That(floatValue).IsNotZero();
            Assert.That(shortValue).IsNotZero();
            Assert.That(byteValue).IsNotZero();
            Assert.That(uintValue).IsNotZero();
            Assert.That(ushortValue).IsNotZero();
            Assert.That(ulongValue).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[0] is equal to zero.")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            double a = 0D;

            Assert.That(a).IsNotZero();
        }

        #endregion 

        #region IsPositive

        [Test]
        public void IsPositiveWorks()
        {
            int a = 2;
            Assert.That(a).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[0] is not a strictly positive value.")]
        public void IsPositiveThrowExceptionWhenEqualToZero()
        {
            float a = 0F;
            Assert.That(a).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[-50] is not a strictly positive value.")]
        public void IsPositiveThrowExceptionWhenValueIsNegative()
        {
            double a = -50D;

            Assert.That(a).IsPositive();
        }

        #endregion
    }
}
