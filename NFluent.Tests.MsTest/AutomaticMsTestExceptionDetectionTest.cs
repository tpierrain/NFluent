namespace NFluent.Tests.MsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NFluent.Helpers;

    [TestClass]
    public class AutomaticMsTestExceptionDetectionTest
    {
        [TestMethod]
        public void MsTestDetection()
        {
            var ex = ExceptionHelper.BuildException("the message");
            Assert.IsInstanceOfType(ex, typeof(AssertFailedException));
        }
    }
}
