namespace NFluent.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class SizeRelatedTests
    {
        [Test]
        public void HasSizeWorksWithArray()
        {
            var array = new int[] { 45, 43, 54, 666 };

            Assert.That(array.HasSize(4));
        }

        [Test]
        public void HasSizeWorksWithEnumerable()
        {
            var enumerable = new List<int>() { 45, 43, 54, 666 };

            Assert.That(enumerable.HasSize(4));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "Has [4] items instead of the expected value [2].")]
        public void HasSizeThrowsExceptionWithClearStatusWhenFails()
        {
            var enumerable = new List<int>() { 45, 43, 54, 666 };

            Assert.That(enumerable.HasSize(2));
        }
    }
}