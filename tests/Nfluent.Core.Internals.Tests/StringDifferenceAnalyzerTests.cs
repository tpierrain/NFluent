using System.Collections.Generic;

namespace Nfluent.Tests
{
    using NFluent;
    using NUnit.Framework;

    [TestFixture]
    public class StringDifferenceAnalyzerTests
    {
        [Test]
        public void ShouldNotReportWhenNoDifference()
        {
            Check.That(StringDifferenceAnalyzer.Analyze("toto", "tata")).IsNull();
        }
    }

    public static class StringDifferenceAnalyzer
    {
        public static IList<StringDifference> Analyze(string actual, string expected)
        {
            return null;
        }
    }

    public class StringDifference
    {
    }
}
