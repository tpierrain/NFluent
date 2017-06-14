namespace NFluent.Tests.MsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NFluent.Helpers;

    [TestClass]
    public class MsTestCompatibility
    {
        [TestMethod]
        public void BasicTest()
        {
            Check.That("MsTest").IsNotEmpty();

            Check.ThatCode(() => Check.That("MsTest").IsEqualTo("great")).Throws<FluentCheckException>();
        }

        [TestMethod]
        public void ExceptionScanTest()
        {
            Check.That(ExceptionHelper.BuildException("Test")).IsInstanceOf<AssertFailedException>();
        }
    }
}
