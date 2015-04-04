namespace NFluent.Tests.XUnit
{
    using NFluent;
    using Xunit;
    
    public class DemonstrationTest
    {
        [Fact]
        public void BasicCheck()
        {
            Check.That(10).IsEqualTo(10);
            Check.That("test").IsEqualTo("test");
        }

        [Fact]
        public void FailingTest()
        {
            //Check.That("test").IsEqualTo("tt");
        }
    }
}
