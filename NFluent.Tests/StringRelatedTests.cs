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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is null.\nThe expected substrin(s):\n\t[\"fails\", \"anyway\"]")]
        public void ContainsFailsProperlyOnNullString()
        {
            Check.That((string)null).Contains("fails", "anyway");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked string does not contains the expected value(s): \"C\", \"A\"\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected substring(s):\n\t[\"C\", \"a\", \"A\", \"z\"]")]
        public void ContainsIsCaseSensitive()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Contains("C", "a", "A", "z");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked string does not contains the expected value(s): \"0\", \"4\"\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected substring(s):\n\t[\"c\", \"0\", \"4\"]")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked string's start is different from the expected one.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: starts with\n\t[\"ABCDEF\"]")]
        public void StartWithIsCaseSensitive()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).StartsWith("ABCDEF");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is null.\nThe expected value: starts with\n\t[\"fails\"]")]
        public void StartsWithFailsProperlyOnNullString()
        {
            Check.That((string)null).StartsWith("fails");
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked string starts with expected one, whereas it must not.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: does not start with\n\t[\"abcdef\"]")]
        public void NegatedStartWithThrowsException()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.StartsWith("abcdef");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked string's end is different from the expected one.\nThe checked string:\n\t[\"abcdefghijklmnopqrstuvwxyz\"]\nThe expected string: ends with\n\t[\"UWXYZ\"]")]
        public void EndsWithIsCaseSensitive()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).EndsWith("UWXYZ");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is null.\nThe expected value: ends with\n\t[\"fails\"]")]
        public void EndsWithFailsProperlyOnNullString()
        {
            Check.That((string)null).EndsWith("fails");
        }

        [Test]
        public void EndsWithWorks()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).EndsWith("uvwxyz");
        }

        [Test]
        public void EndsWithIsNegatable()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Check.That(alphabet).Not.EndsWith("hehehe");
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