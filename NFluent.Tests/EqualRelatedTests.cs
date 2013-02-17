namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class EqualRelatedTests
    {
        [Test]
        public void IsEqualToWorks()
        {
            var first = "Son of a test";
            Assert.That(first.IsEqualTo("Son of a test"));

            var array = new int[] { 45, 43, 54, 666 };
            var otherReference = array;

            Assert.That(array.IsEqualTo(array));
            Assert.That(array.IsEqualTo(otherReference));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException))]
        public void IsEqualToThrowsExceptionWhenFails()
        {
            var array = new int[] { 45, 43, 54, 666 };
            var otherSimilarButNotEqualArray = new int[] { 45, 43, 54, 666 };

            Assert.That(array.IsEqualTo(otherSimilarButNotEqualArray));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""Son of a test""] not equals to the expected [""no way""]")]
        public void IsEqualToThrowsExceptionWithClearStatusWhenFails()
        {
            var first = "Son of a test";
            Assert.That(first.IsEqualTo("no way"));
        }

        [Test]
        public void IsNotEqualToWorks()
        {
            var first = "Son of a test";
            Assert.That(first.IsNotEqualTo("other text"));

            var array = new int[] { 45, 43, 54, 666 };
            var otherArray = new int[] { 666, 74 };
            var similarButNotEqualArray = new int[] { 45, 43, 54, 666 };

            Assert.That(array.IsNotEqualTo(otherArray));
            Assert.That(array.IsNotEqualTo(similarButNotEqualArray));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""Son of a test""] equals to the value [""Son of a test""] which is not expected.")]
        public void IsNotEqualToThrowsExceptionWithClearStatusWhenFails()
        {
            var first = "Son of a test";
            var otherReferenceToSameObject = first;
            Assert.That(first.IsNotEqualTo(otherReferenceToSameObject));
        }
    }
}