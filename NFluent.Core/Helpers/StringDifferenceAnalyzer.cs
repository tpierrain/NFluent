using System;
using System.Collections.Generic;

namespace NFluent.Helpers
{
    using System.Linq;
    using Extensions;

    internal static class StringDifferenceAnalyzer
    {
        public static IList<StringDifference> Analyze(string actual, string expected)
        {
            if (actual == expected)
            {
                // strings are same, so no need to parse
                return null;
            }
            // perform a per line analysis
            var actualLines = actual.Split('\n');
            var expectedLines = expected.Split('\n');
            var result = new List<StringDifference>();
            var sharedLines = Math.Min(actualLines.Length, expectedLines.Length);
            for (var line = 0; line < sharedLines; line++)
            {
                var actualLine = actualLines[line];
                var expectedLine = expectedLines[line];
                var stringDifference = StringDifferenceAnalyzer.AnalyseLine(line, actualLine, expectedLine);
                if (stringDifference.Type != DifferenceMode.NoDifference)
                {
                    result.Add(stringDifference);
                }
            }
            if (expectedLines.Length > sharedLines)
            {
                result.Add(new StringDifference {Line = sharedLines, Type = DifferenceMode.MissingLine});
            }
            else if (actualLines.Length > sharedLines)
            {
                result.Add(new StringDifference { Line = sharedLines, Type = DifferenceMode.ExtraLines });
            }
            return result;
        }

        private static StringDifference AnalyseLine(int line, string actualLine, string expectedLine)
        {
            var stringDifference = new StringDifference {Line = line};
            // scan the initial part of both strings
            var sharedLine = Math.Min(actualLine.Length, expectedLine.Length);
            for (var i = 0;
                i < sharedLine;
                i++)
            {
                if (actualLine[i] == expectedLine[i])
                {
                    continue;
                }
                if (StringExtensions.CompareChar(actualLine[i], expectedLine[i], true))
                {
                    if (stringDifference.Type == DifferenceMode.CaseDifference)
                    {
                        continue;
                    }
                    stringDifference.Type = DifferenceMode.CaseDifference;
                    stringDifference.Position = i;
                }
                else
                {
                    stringDifference.Type = DifferenceMode.General;
                    stringDifference.Position = i;
                    break;
                }
            }
            // strings are same so far
            if (stringDifference.Type != DifferenceMode.NoDifference)
            {
                return stringDifference;
            }

            // the actualLine string is longer than expectedLine
            if (actualLine.Length > expectedLine.Length)
            {
                stringDifference.Position = expectedLine.Length;
                if ((actualLine.Length == expectedLine.Length + 1) && actualLine.Last() == '\r')
                {
                    stringDifference.Type = DifferenceMode.EndOfLine;
                }
                else
                {
                    stringDifference.Type = DifferenceMode.Longer;
                }
            }
            else if (actualLine.Length < expectedLine.Length)
            {
                stringDifference.Position = actualLine.Length;
                if ((actualLine.Length + 1 == expectedLine.Length) && expectedLine.Last() == '\r')
                {
                    stringDifference.Type = DifferenceMode.EndOfLine;
                }
                else
                {
                    stringDifference.Type = DifferenceMode.Shorter;
                }
            }
            return stringDifference;
        }
    }

    internal class StringDifference
    {
        public DifferenceMode Type = DifferenceMode.NoDifference;
        public int Position;
        public int Line { get; internal set; }
    }

    internal enum DifferenceMode
    {
        NoDifference,
        General,
        CaseDifference,
        Longer,
        Shorter,
        EndOfLine,
        MissingLine,
        ExtraLines
    }
}
