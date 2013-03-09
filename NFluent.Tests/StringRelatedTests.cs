namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class StringRelatedTests
    {
        #region Contains related tests

        [Test]
        public void ContainsWork()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("c", "z", "u");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException),
            ExpectedMessage =
                @"The string [""abcdefghijklmnopqrstuvwxyz""] does not contain the expected value(s): [""0"", ""4""].")]
        public void ContainsThrowsExceptionWhenFails()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("c", "0", "4");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException),
            ExpectedMessage =
                @"The string [""abcdefghijklmnopqrstuvwxyz""] does not contain the expected value(s): [""C"", ""A""].")]
        public void ContainsIsCaseSensitive()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("C", "a", "A", "z");
        }

        #endregion

        #region StartsWith related tests

        [Test]
        public void StartWithWorks()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).StartsWith("abcdef");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException),
            ExpectedMessage = @"The string [""abcdefghijklmnopqrstuvwxyz""] does not start with [""ABCDEF""].")]
        public void StartWithIsCaseSensitive()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).StartsWith("ABCDEF");
        }

        #endregion
    }
}
