namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class AndRelatedTests
    {
        [Test]
        public void CanUseAndInAnyOrderForAllFluentStringAssertOperations()
        {
            const string Heroes = "Batman and Robin";

            Check.That(Heroes).Not.Contains("Joker").And.StartsWith("Bat").And.Contains("Robin");

            Check.That(Heroes).Contains("and").And.IsInstanceOf<string>().And.IsNotInstanceOf<Person>().And.IsNotEqualTo(null);
            Check.That(Heroes).Contains("Robin").And.StartsWith("Batman").And.IsInstanceOf<string>();
            Check.That(Heroes).IsInstanceOf<string>().And.Contains("Batman", "Robin").And.StartsWith("Batm");
            Check.That(Heroes).Contains("and").And.IsNotInstanceOf<Person>().And.IsInstanceOf<string>();
        }
    }
}
