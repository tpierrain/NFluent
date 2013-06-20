namespace NFluent.Tests.MsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DemonstrationTest
    {
        [TestMethod]
        public void BasicCheck()
        {
            Check.That(10).IsEqualTo(10);
            Check.That("test").IsEqualTo("test");
        }

        [TestMethod]
        [ExpectedException(typeof(FluentCheckException))]
        public void FailingTest()
        {
            Check.That("test").IsEqualTo("tt");
        }
    }
}
