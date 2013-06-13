namespace NFluent.Tests
{
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class NumbersOfDifferentTypesRelatedTests
    {
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
        
        #region IsEqualTo / IsNotEqualTo

        [Test]
        public void NotIsEqualToWorksWithDifferentTypes()
        {
            const int IntValue = 42;
            const long LongValue = 21L;

            Check.That(IntValue).Not.IsEqualTo(LongValue);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException),
            ExpectedMessage = "The checked value is different from the expected one.\nThe checked value:\n\t[42] of type: [System.Int32]\nThe expected value:\n\t[42] of type: [System.Int64]")]
        public void IsEqualToThrowsWhenSameNumberOfDifferentTypes()
        {
            const int IntValue = 42;
            const long LongValue = 42L;

            Check.That(IntValue).IsEqualTo(LongValue);
        }
        
        #endregion
    }
}
