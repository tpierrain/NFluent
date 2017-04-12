// -------------------------------------------------------------------------------------------------------------------
// <copyright file="StringDifferenceAnalyzerTests.cs" company="">
//   Copyright 2017 Cyrille Dupuydauby
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace NFluent.Tests
{
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class StringDifferenceAnalyzerTests
    {
        [Test]
        public void ShouldHandleNullString()
        {
            Check.That(StringDifference.Analyze(null, "foo", false)).HasSize(1);
        }

        [Test]
        public void ShouldNotReportWhenNoDifference()
        {
            Check.That(StringDifference.Analyze("foo", "foo", false)).IsNull();
        }

        [Test]
        public void ShouldReportDifferenceForCaseSensitive()
        {
            var stringDifferences = StringDifference.Analyze("foo", "foO", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.CaseDifference);
            Check.That(stringDifferences[0].Position).IsEqualTo(2);
        }

        [Test]
        public void ShouldNotReportDifferenceForCaseSensitiveWhenDisabled()
        {
            var stringDifferences = StringDifference.Analyze("foo", "foO", true);
            Check.That(stringDifferences).HasSize(0);
        }

        [Test]
        public void ShouldReportDifferenceForGeneralSameLength()
        {
            var stringDifferences = StringDifference.Analyze("food", "FoOG", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.GeneralSameLength);
            Check.That(stringDifferences[0].Position).IsEqualTo(3);
        }

        [Test]
        public void ShouldReportDifferenceForGeneralEvenIfFirstDiffIsCase()
        {
            var stringDifferences = StringDifference.Analyze("food", "FoOGd", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.General);
            Check.That(stringDifferences[0].Position).IsEqualTo(3);
        }

        [Test]
        public void ShouldReportDifferenceForLongerText()
        {
            var stringDifferences = StringDifference.Analyze("foo bar", "foo", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.Longer);
            Check.That(stringDifferences[0].Position).IsEqualTo(3);
        }

        [Test]
        public void ShouldReportDifferenceForLongerTextOnMultiline()
        {
            var stringDifferences = StringDifference.Analyze("foo bar\ntest", "foo\ntest", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.LongerLine);
            Check.That(stringDifferences[0].Position).IsEqualTo(3);
        }

        [Test]
        public void ShouldReportDifferenceForMultipleLines()
        {
            var stringDifferences = StringDifference.Analyze("toto\ntiti", "toto\ntata", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.GeneralSameLength);
            Check.That(stringDifferences[0].Position).IsEqualTo(1);
            Check.That(stringDifferences[0].Line).IsEqualTo(1);
            stringDifferences = StringDifference.Analyze("maybe\ntiti", "toto\ntatata", false);
            Check.That(stringDifferences).HasSize(2);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.General);
            Check.That(stringDifferences[0].Position).IsEqualTo(0);
            Check.That(stringDifferences[0].Line).IsEqualTo(0);
            Check.That(stringDifferences[1].Type).IsEqualTo(DifferenceMode.General);
            Check.That(stringDifferences[1].Position).IsEqualTo(1);
            Check.That(stringDifferences[1].Line).IsEqualTo(1);
        }

        [Test]
        public void ShouldProvideSyntheticView()
        {
            var stringDifferences = StringDifference.Analyze("toto\ntiti", "toto\nTiTi", false);

            Check.That(StringDifference.Summarize(stringDifferences)).IsEqualTo(DifferenceMode.CaseDifference);

            stringDifferences = StringDifference.Analyze("Toto\ntiti", "toto\ntata", false);
            Check.That(StringDifference.Summarize(stringDifferences)).IsEqualTo(DifferenceMode.General);
        }

        [Test]
        public void ShouldReportDifferenceForShorterText()
        {
            var stringDifferences = StringDifference.Analyze("toto", "toto et tata", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.Shorter);
            Check.That(stringDifferences[0].Position).IsEqualTo(4);
        }

        [Test]
        public void ShouldReportDifferenceForShorterTextOnMultiline()
        {
            var stringDifferences = StringDifference.Analyze("toto\ntest", "toto et tata\ntest", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.ShorterLine);
            Check.That(stringDifferences[0].Position).IsEqualTo(4);
        }

        [Test]
        public void ShouldReportDifferenceInSpaces()
        {
            var stringDifference = StringDifference.Analyze("toto  and tutu", "toto\tand\t tutu", false);
            Check.That(stringDifference).HasSize(1);
            Check.That(stringDifference[0].Type).IsEqualTo(DifferenceMode.Spaces);
        }

        [Test]
        public void ShouldReportDifferenceOfEoL()
        {
            var stringDifferences = StringDifference.Analyze("toto\ntiti", "toto\r\ntiti", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.EndOfLine);
            Check.That(stringDifferences[0].Position).IsEqualTo(4);
            Check.That(stringDifferences[0].Line).IsEqualTo(0);
        }

        [Test]
        public void ShouldReportDifferenceOfNumberOfLinesL()
        {
            var stringDifferences = StringDifference.Analyze("toto\ntiti", "toto", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.ExtraLines);
            Check.That(stringDifferences[0].Position).IsEqualTo(0);
            Check.That(stringDifferences[0].Line).IsEqualTo(1);
            stringDifferences = StringDifference.Analyze("toto", "toto\n", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.MissingLines);
            Check.That(stringDifferences[0].Position).IsEqualTo(0);
            Check.That(stringDifferences[0].Line).IsEqualTo(1);
        }

        [Test]
        public void ShouldReportDifferentLineWhenOneLine()
        {
            var stringDifferences = StringDifference.Analyze("foo", "far", false);
            Check.That(stringDifferences).HasSize(1);
            Check.That(stringDifferences[0].Type).IsEqualTo(DifferenceMode.GeneralSameLength);
            Check.That(stringDifferences[0].Position).IsEqualTo(1);
        }
   }
}