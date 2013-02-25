namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using NUnit.Framework;

    using Assert = NFluent.Assert;

    [TestFixture]
    public class StringRelatedTests
    {
        [Test]
        public void ContainsWork()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Assert.That(alphabet).Contains("c", "z", "u");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"The string [""abcdefghijklmnopqrstuvwxyz""] does not contain the expected value(s): [""0"", ""4""].")]
        public void ContainsThrowsExceptionWhenFails()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Assert.That(alphabet).Contains("c", "0", "4");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"The string [""abcdefghijklmnopqrstuvwxyz""] does not contain the expected value(s): [""C"", ""A""].")]
        public void ContainsIsCaseSensitive()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            Assert.That(alphabet).Contains("C", "a", "A", "z");
        }

        [Test]
        public void StartWithWorks()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Assert.That(alphabet).StartsWith("abcdef");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"The string [""abcdefghijklmnopqrstuvwxyz""] does not start with [""ABCDEF""].")]
        public void StartWithIsCaseSensitive()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Assert.That(alphabet).StartsWith("ABCDEF");
        }
    }
}
