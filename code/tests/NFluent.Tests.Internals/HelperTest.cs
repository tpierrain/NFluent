using System.Collections.Generic;

namespace NFluent.Tests
{
    using Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class HelperTest
    {
        [Test]
        public void ShouldRecognizeSetClasses()
        {
            var test = new HashSet<int>();

            Check.That(test.GetType().IsASet()).IsTrue();
        }
    }
}
