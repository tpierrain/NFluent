namespace NFluent.Tests
{
    using Extensibility;
    using NUnit.Framework;

    [TestFixture]
    public class ReverseEngineeringExceptionMessagesHelperTests
    {
        [Test]
        public void Should_Escape_eol()
        {
            Check.That(ReverseEngineeringExceptionMessagesHelper.GetProperlyEscapedMessage("\r\n")).IsEqualTo(@"\r\n");
        }

        [Test]
        public void Should_NotEscape_everything()
        {
            Check.That(ReverseEngineeringExceptionMessagesHelper.GetProperlyEscapedMessage("\r\xB\xC\n")).IsEqualTo("\\r\xB\xC\\n");
        }

        [Test]
        public void Should_Escape_tabs()
        {
            Check.That(ReverseEngineeringExceptionMessagesHelper.GetProperlyEscapedMessage("tab is:	.")).IsEqualTo(@"tab is:\t.");
        }

        [Test]
        public void Should_Escape_quote()
        {
            Check.That(ReverseEngineeringExceptionMessagesHelper.GetProperlyEscapedMessage("quote is:\"like this\".")).IsEqualTo(@"quote is:\""like this\"".");
        }
    }
}