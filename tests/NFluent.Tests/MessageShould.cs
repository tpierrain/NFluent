namespace NFluent.Tests
{
    using Extensibility;
    using NUnit.Framework;

    [TestFixture]
    class MessageShould
    {
        [Test]
        public void AvoidRepetition()
        {
            Check.That(FluentMessage.BuildMessage("{checked} and {expected}").ToString()).AsLines()
                .ContainsExactly("",  "checked value and expected one");
            Check.That(FluentMessage.BuildMessage("{expected} and {checked}").ToString()).AsLines()
                .ContainsExactly("",  "expected value and checked one");
        }
    }
}
