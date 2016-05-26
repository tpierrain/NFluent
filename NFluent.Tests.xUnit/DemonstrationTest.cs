namespace NFluent.Tests.XUnitTest
{
    using NFluent;
    using Xunit;
    
    public class DemonstrationTest
    {
        [Xunit.Fact]
        public void BasicCheck()
        {
            Check.That(10).IsEqualTo(10);
            Check.That("test").IsEqualTo("test");
        }

        [Xunit.Fact]
        public void FailingTest()
        {
            // Check.That("test").IsEqualTo("tt");
        }
    }
}
