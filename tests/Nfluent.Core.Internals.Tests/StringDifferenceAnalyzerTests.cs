using System;
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
            Check.That(StringDifferenceAnalyzer.Analyze("toto", "toto")).IsNull();
        }

        [Test]
        public void ShouldReportDifferentLineWhenOneLine()
        {
            var stringDifferences = StringDifferenceAnalyzer.Analyze("toto", "tutu");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(StringDifference.DifferenceMode.General);
            Check.That(stringDifferences[0].Position).IsEqualTo(1);
        }

        [Test]
        [Ignore("In progress")]
        public void ShouldReportDifferenceForCaseSensitive()
        {
            var stringDifferences = StringDifferenceAnalyzer.Analyze("toto", "toTo");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(StringDifference.DifferenceMode.CaseDifference);
            Check.That(stringDifferences[0].Position).IsEqualTo(2);
        }
    }

    internal static class StringDifferenceAnalyzer
    {
        public static IList<StringDifference> Analyze(string actual, string expected)
        {
            if (actual != expected)
            {
                var stringDifference = new StringDifference();
                int difPos;
                for (difPos = 0;
                    difPos < Math.Min(actual.Length, expected.Length) && actual[difPos] == expected[difPos];
                    difPos++) ;
                stringDifference.Position = difPos;
                var result = new List<StringDifference> { stringDifference };
                return result;
            }
            return null;
        }
    }

    public class StringDifference
    {
        public enum DifferenceMode
        {
            General,
            CaseDifference
        }

        public DifferenceMode Type;
        public int Position;
    }

}
