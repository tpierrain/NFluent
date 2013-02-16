namespace NFluent.Tests
{
    using NUnit.Framework;
    using Assert = NFluent.Assert; // alias needed here since NUnit uses the same name; but useless for other unit test frameworks.

    [TestFixture]
    public class AssertThatTests
    {
        [Test]
        public void AssertThatWorks()
        {
            Assert.That(true);
        }
        
        [Test]
        [ExpectedException(typeof(FluentAssertionException))]
        public void AssertThatThowsExceptionWhenFails()
        {
            Assert.That(false);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Dumbest test ever")]
        public void AssertThatThowsExceptionWithIndicatedMessageWhenFails()
        {
            Assert.That(false, "Dumbest test ever");
        }
    }
}
