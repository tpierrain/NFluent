using System.Globalization;

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
            Check.That(StringDifference.Analyze("toto", "toto")).IsNull();
        }

        [Test]
        public void ShouldReportDifferentLineWhenOneLine()
        {
            var stringDifferences = StringDifference.Analyze("toto", "tutu");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.General);
            Check.That(stringDifferences[0].Position).IsEqualTo(1);
        }

        [Test]
        public void ShouldReportDifferenceForCaseSensitive()
        {
            var stringDifferences = StringDifference.Analyze("toto", "toTO");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.CaseDifference);
            Check.That(stringDifferences[0].Position).IsEqualTo(2);
        }

        [Test]
        public void ShouldReportDifferenceForGeneralEvenIfFirstDiffIsCase()
        {
            var stringDifferences = StringDifference.Analyze("toto", "toTd");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.General);
            Check.That(stringDifferences[0].Position).IsEqualTo(3);
        }

        [Test]
        public void ShouldReportDifferenceForLongerText()
        {
            var stringDifferences = StringDifference.Analyze("toto et tutu", "toto");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.Longer);
            Check.That(stringDifferences[0].Position).IsEqualTo(4);
        }

        [Test]
        public void ShouldReportDifferenceForShorterText()
        {
            var stringDifferences = StringDifference.Analyze("toto", "toto et tata");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.Shorter);
            Check.That(stringDifferences[0].Position).IsEqualTo(4);
        }

        [Test]
        public void ShouldReportDifferenceInSpaces()
        {
            var stringDifference = StringDifference.Analyze("toto  and tutu", "toto\tand\t tutu");
            Check.That(stringDifference).HasSize(1);
            Check.That(stringDifference[0].Type).IsEqualTo(DifferenceMode.Spaces);
        }

        [Test]
        public void ShouldReportDifferenceForMultipleLines()
        {
            var stringDifferences = StringDifference.Analyze("toto\ntiti", "toto\ntata");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.General);
            Check.That(stringDifferences[0].Position).IsEqualTo(1);
            Check.That(stringDifferences[0].Line).IsEqualTo(1);
            stringDifferences = StringDifference.Analyze("maybe\ntiti", "toto\ntata");
            Check.That(stringDifferences).HasSize(2);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.General);
            Check.That(stringDifferences[0].Position).IsEqualTo(0);
            Check.That(stringDifferences[0].Line).IsEqualTo(0);
            Check.That(stringDifferences[1].Type).IsEqualTo(DifferenceMode.General);
            Check.That(stringDifferences[1].Position).IsEqualTo(1);
            Check.That(stringDifferences[1].Line).IsEqualTo(1);
        }

        [Test]
        public void ShouldReportDifferenceOfEoL()
        {
            var stringDifferences = StringDifference.Analyze("toto\ntiti", "toto\r\ntiti");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.EndOfLine);
            Check.That(stringDifferences[0].Position).IsEqualTo(4);
            Check.That(stringDifferences[0].Line).IsEqualTo(0);
        }

        [Test]
        public void ShouldReportDifferenceOfNumberOfLinesL()
        {
            var stringDifferences = StringDifference.Analyze("toto\ntiti", "toto");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.ExtraLines);
            Check.That(stringDifferences[0].Position).IsEqualTo(0);
            Check.That(stringDifferences[0].Line).IsEqualTo(1);
            stringDifferences = StringDifference.Analyze("toto", "toto\n");
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.MissingLines);
            Check.That(stringDifferences[0].Position).IsEqualTo(0);
            Check.That(stringDifferences[0].Line).IsEqualTo(1);
        }
    }
}
