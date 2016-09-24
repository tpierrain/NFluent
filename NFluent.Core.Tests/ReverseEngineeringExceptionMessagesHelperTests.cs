using System;

namespace NFluent.Tests
{
    using NFluent.Extensibility;
    using NUnit.Framework;

    [TestFixture]
    public class ReverseEngineeringExceptionMessagesHelperTests
    {
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

        [Test]
        [Ignore("It's maybe time to get rid of this helper class since R# is able to transform text to a regular string")]
        public void Should_Escape_CRLF()
        {
            Check.That(ReverseEngineeringExceptionMessagesHelper.GetProperlyEscapedMessage(@"CRLF is:
.")).IsEqualTo(@"CRLF is:\r\n.");
        }
    }
}