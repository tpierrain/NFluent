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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual string:\n\t[\"Batman and Robin\"]\n contains the value(s):\n\t[\"Batman\"]\nwhich was not expected.")]
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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual string:\n\t[\"Batman and Robin\"]\n contains the value(s):\n\t[\"Robin\"]\nwhich was not expected.")]
        public void ThrowsProperExceptionWhenCombineNotAndAndOperatorsInTheSameCheckStatement()
        {
            Check.That("Batman and Robin").Not.Contains("Joker").And.StartsWith("Bat").And.Not.Contains("Robin");
        }
    }
}
