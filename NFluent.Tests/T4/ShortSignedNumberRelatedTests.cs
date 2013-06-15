namespace NFluent.Tests
{
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class ShortSignedNumberRelatedTests
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------

        #pragma warning restore 169
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

        #region IsPositive

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is not strictly positive.\nThe checked value:\n\t[0]")]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const short Zero = 0;
            Check.That(Zero).IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is not strictly positive.\nThe checked value:\n\t[-50]")]
        public void IsPositiveThrowsExceptionWhenValueIsNegative()
        {
            const short MinusFifty = -50;

            Check.That(MinusFifty).IsPositive();
        }

        [Test]
        public void NotIsPositiveWorks()
        {
            const short MinusFifty = -50;

            Check.That(MinusFifty).Not.IsPositive();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked value is positive, whereas it must not.\nThe checked value:\n\t[2]")]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const short Two = 2;

            Check.That(Two).Not.IsPositive();
        }

        #endregion
    }
}
