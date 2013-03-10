namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class EqualRelatedTests
    {
        [Test]
        public void IsEqualToWorksWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo("Son of a test");
        }

        [Test]
        public void IsEqualToWorksWithArray()
        {
            var array = new int[] { 45, 43, 54, 666 };
            var otherReference = array;

            Check.That(array).IsEqualTo(array);
            Check.That(array).IsEqualTo(otherReference);
        }

        [Test]
        public void IsEqualToWorksObject()
        {
            var heroe = new Person() { Name = "Gandhi" };
            var otherReference = heroe;

            Check.That(heroe).IsEqualTo(otherReference);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException))]
        public void IsEqualToThrowsExceptionWhenFails()
        {
            var array = new int[] { 45, 43, 54, 666 };
            var otherSimilarButNotEqualArray = new int[] { 45, 43, 54, 666 };

            Check.That(array).IsEqualTo(otherSimilarButNotEqualArray);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""Son of a test""] not equals to the expected [""no way""]")]
        public void IsEqualToThrowsExceptionWithClearStatusWhenFails()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo("no way");
        }

        [Test]
        public void IsNotEqualToWorksWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsNotEqualTo("other text");
        }

        [Test]
        public void IsNotEqualToWorksWithArray()
        {
            var array = new int[] { 45, 43, 54, 666 };
            var otherArray = new int[] { 666, 74 };
            var similarButNotEqualArray = new int[] { 45, 43, 54, 666 };

            Check.That(array).IsNotEqualTo(otherArray);
            Check.That(array).IsNotEqualTo(similarButNotEqualArray);
        }

        [Test]
        public void IsNotEqualToWorksObject()
        {
            var heroe = new Person() { Name = "Gandhi" };
            var badGuy = new Person() { Name = "Pol Pot" };

            Check.That(heroe).IsNotEqualTo(badGuy);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""Son of a test""] equals to the value [""Son of a test""] which is not expected.")]
        public void IsNotEqualToThrowsExceptionWithClearStatusWhenFails()
        {
            var first = "Son of a test";
            var otherReferenceToSameObject = first;
            Check.That(first).IsNotEqualTo(otherReferenceToSameObject);
        }
    }
}