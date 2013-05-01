namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class AndRelatedTests
    {
        [Test]
        public void CanUseAndInAnyOrderForAllFluentStringAssertOperations()
        {
            var heroes = "Batman and Robin";

            Check.That(heroes).StartsWith("Bat").And.Contains("Robin");
            Check.That(heroes).Contains("and").And.IsInstanceOf<string>().And.Not.IsInstanceOf<Person>().And.IsNotEqualTo(null);
            Check.That(heroes).Contains("Robin").And.StartsWith("Batman").And.IsInstanceOf<string>();
            Check.That(heroes).IsInstanceOf<string>().And.Contains("Batman", "Robin").And.StartsWith("Batm");
            Check.That(heroes).Contains("and").And.Not.IsInstanceOf<Person>().And.IsInstanceOf<string>();
        }
    }
}
