namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class StringRelatedTests
    {
        #region Public Methods and Operators

        [Test]
        public void ContainsWorksWithString()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("lmnop");
        }

        [Test]
        public void ContainsWithStringIsNegatable()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.Contains("1234");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\ndoes not contain the expected value(s):\n\t[\"C\", \"A\"].")]
        public void ContainsIsCaseSensitive()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("C", "a", "A", "z");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\ndoes not contain the expected value(s):\n\t[\"0\", \"4\"].")]
        public void ContainsThrowsExceptionWhenFails()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("c", "0", "4");
        }

        [Test]
        public void ContainsWorks()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("c", "z", "u");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\ndoes not start with:\n\t[\"ABCDEF\"].")]
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

        [Test]
        public void StartWithIsNegatable()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.StartsWith("hehehe");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nstarts with:\n\t[\"abcdef\"]\nwhich is not expected.")]
        public void NegatedStartWithThrowsException()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.StartsWith("abcdef");
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionsOnString()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("i").And.StartsWith("abcd").And.IsInstanceOf<string>().And.IsNotInstanceOf<int>().And.Not.IsNotInstanceOf<string>();
        }
    }
}