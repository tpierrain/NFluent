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

            Check.That(intValue).IsZero();
            Check.That(longValue).IsZero();
            Check.That(doubleValue).IsZero();
            Check.That(decimalValue).IsZero();
            Check.That(floatValue).IsZero();
            Check.That(shortValue).IsZero();
            Check.That(byteValue).IsZero();
            Check.That(uintValue).IsZero();
            Check.That(ushortValue).IsZero();
            Check.That(ulongValue).IsZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[9] is not equal to zero.")]
        public void IsZeroThrowsExceptionWhenFails()
        {
            int a = 9;

            Check.That(a).IsZero();
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

            Check.That(intValue).IsNotZero();
            Check.That(longValue).IsNotZero();
            Check.That(doubleValue).IsNotZero();
            Check.That(decimalValue).IsNotZero();
            Check.That(floatValue).IsNotZero();
            Check.That(shortValue).IsNotZero();
            Check.That(byteValue).IsNotZero();
            Check.That(uintValue).IsNotZero();
            Check.That(ushortValue).IsNotZero();
            Check.That(ulongValue).IsNotZero();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[0] is equal to zero.")]
        public void IsNotZeroThrowsExceptionWhenFails()
        {
            double a = 0D;

            Check.That(a).IsNotZero();
        }

        #endregion 

        #region IsPositive

        [Test]
        public void IsPositiveWorks()
        {
            int a = 2;
            Check.That(a).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[0] is not a strictly positive value.")]
        public void IsPositiveThrowExceptionWhenEqualToZero()
        {
            float a = 0F;
            Check.That(a).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[-50] is not a strictly positive value.")]
        public void IsPositiveThrowExceptionWhenValueIsNegative()
        {
            double a = -50D;

            Check.That(a).IsPositive();
        }

        #endregion
    }
}
