namespace NFluent.Tests
{
    using NUnit.Framework;
    using Spike.Ext;
    using Check = Spike.Check;

    [TestFixture]
    public class StringRelatedTests
    {
        #region Public Methods and Operators

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"The string [""abcdefghijklmnopqrstuvwxyz""] does not contain the expected value(s): [""C"", ""A""].")]
        public void ContainsIsCaseSensitive()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("C", "a", "A", "z");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"The string [""abcdefghijklmnopqrstuvwxyz""] does not contain the expected value(s): [""0"", ""4""].")]
        public void ContainsThrowsExceptionWhenFails()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("c", "0", "4");
        }

        [Test]
        public void ContainsWork()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("c", "z", "u");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"The string [""abcdefghijklmnopqrstuvwxyz""] does not start with [""ABCDEF""].")]
        public void StartWithIsCaseSensitive()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).StartsWith("ABCDEF");
        }

        [Test]
        public void StartWithWorks()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).StartsWith("abcdef");
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionsOnString()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("i").And.StartsWith("abcd").And.IsInstanceOf<string>();
        }
    }
}