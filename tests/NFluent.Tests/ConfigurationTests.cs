using System;
using System.Collections.Generic;
using System.Text;

namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void CanConfigureTruncationLength()
        {
            var current = Check.StringTruncationLength;
            try
            {
                Check.StringTruncationLength = 20;
                Check.That(Check.StringTruncationLength).IsEqualTo(20);

                Check.ThatCode(() => Check.StringTruncationLength = 5).Throws<ArgumentException>();

            }
            finally
            {
                Check.StringTruncationLength = current;
            }

        }
    }
}
