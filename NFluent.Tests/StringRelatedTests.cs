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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nstarts with:\n\t[\"abcdef\"]\nwhich was not expected.")]
        public void NegatedStartWithThrowsException()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.StartsWith("abcdef");
        }

        [Test]
        public void EqualWorks()
        {
            var check = "toto";
            Check.That(check).IsEqualTo("toto");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked string is different from the expected one but has same length.\nThe checked string:\n\t[\"toto\"]\nThe expected string:\n\t[\"tutu\"]")]
        public void EqualFailsWithSameLength()
        {
            var check = "toto";
            Check.That(check).IsEqualTo("tutu");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked string is different from the expected one but only in case.\nThe checked string:\n\t[\"toto\"]\nThe expected string:\n\t[\"TOTO\"]")]
        public void EqualFailsWithDiffCase()
        {
            var check = "toto";
            Check.That(check).IsEqualTo("TOTO");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked string is different from expected one.\nThe checked string:\n\t[\"toto\"]\nThe expected string:\n\t[\"tititutu\"]")]
        public void EqualFailsInGeneral()
        {
            var check = "toto";
            Check.That(check).IsEqualTo("tititutu");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked string is different from expected one, it is missing the end.\nThe checked string:\n\t[\"titi\"]\nThe expected string:\n\t[\"tititutu\"]")]
        public void EqualFailshWhenShorter()
        {
            var check = "titi";
            Check.That(check).IsEqualTo("tititutu");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked string is different from expected one, it contains extra text at the end.\nThe checked string:\n\t[\"tititutu\"]\nThe expected string:\n\t[\"titi\"]")]
        public void EqualFailshWhenStartSame()
        {
            var check = "tititutu";
            Check.That(check).IsEqualTo("titi");
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