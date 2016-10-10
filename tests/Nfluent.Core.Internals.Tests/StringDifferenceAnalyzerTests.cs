namespace Nfluent.Tests
{
    using NFluent;
    using NFluent.Helpers;
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
        public void ShouldReportDifferenceForCaseSensitive()
        {
            var stringDifferences = StringDifferenceAnalyzer.Analyze("toto", "toTo");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(StringDifference.DifferenceMode.CaseDifference);
            Check.That(stringDifferences[0].Position).IsEqualTo(2);
        }

        [Test]
        public void ShouldReportDifferenceForGeneralEvenIfFirstDiffIsCase()
        {
            var stringDifferences = StringDifferenceAnalyzer.Analyze("toto", "toTd");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(StringDifference.DifferenceMode.General);
            Check.That(stringDifferences[0].Position).IsEqualTo(3);
        }
    }


}
