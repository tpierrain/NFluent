namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class NotRelatedTests
    {
        [Test]
        public void NotIsWorking()
        {
            Check.That("Batman and Robin").Not.Contains("Joker");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked string contains unauthorized value(s): \"Batman\"\nThe checked string:\n\t[\"Batman and Robin\"]\nThe unauthorized substring(s):\n\t[\"Batman\"]")]
        public void NotThrowsException()
        {
            Check.That("Batman and Robin").Not.Contains("Batman");
        }

        [Test]
        public void CanCombineNotAndAndOperatorsInTheSameCheckStatement()
        {
            Check.That("Batman and Robin").Not.Contains("Joker").And.StartsWith("Bat").And.Not.Contains("Gandhi");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked string contains unauthorized value(s): \"Robin\"\nThe checked string:\n\t[\"Batman and Robin\"]\nThe unauthorized substring(s):\n\t[\"Robin\"]")]
        public void ThrowsProperExceptionWhenCombineNotAndAndOperatorsInTheSameCheckStatement()
        {
            Check.That("Batman and Robin").Not.Contains("Joker").And.StartsWith("Bat").And.Not.Contains("Robin");
        }
    }
}
