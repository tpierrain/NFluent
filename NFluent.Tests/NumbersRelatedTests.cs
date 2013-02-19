namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    [TestFixture]
    public class NumbersRelatedTests
    {
        [Test]
        public void IsZeroWorks()
        {
            int a = 0;

            Assert.That(a.IsZero());
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[9] is not equal to zero.")]
        public void IsZeroThrowsExceptionWhenFails()
        {
            int a = 9;

            Assert.That(a.IsZero());
        }
    }
}
