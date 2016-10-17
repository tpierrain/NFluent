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
            if (actual != expected)
            {
                var actualLines = actual.Split('\n');
                var expectedLines = expected.Split('\n');
                var result = new List<StringDifference>();
                for (var line = 0; line < Math.Min(actualLines.Length, expectedLines.Length); line++)
                {
                    var stringDifference = new StringDifference();
                    var actualLine = actualLines[line];
                    var expectedLine = expectedLines[line];
                    stringDifference.Line = line;
                    // scan the initial part of both strings
                    for (var i = 0;
                        i < Math.Min(actualLine.Length, expectedLine.Length);
                        i++)
                    {
                        if (actualLine[i] == expectedLine[i])
                        {
                            continue;
                        }
                        if (StringExtensions.CompareChar(actualLine[i], expectedLine[i], true))
                        {
                            if (stringDifference.Type != DifferenceMode.CaseDifference)
                            {
                                stringDifference.Type = DifferenceMode.CaseDifference;
                                stringDifference.Position = i;
                            }
                        }
                        else
                        {
                            stringDifference.Type = DifferenceMode.General;
                            stringDifference.Position = i;
                            break;
                        }
                    }
                    // strings are same so far
                    if (stringDifference.Type == DifferenceMode.NoDifference)
                    {
                        // the actualLine string is longer than expectedLine
                        if (actualLine.Length > expectedLine.Length)
                        {
                            if ((actualLine.Length == expectedLine.Length + 1) && actualLine.Last()=='\r')
                            {
                                stringDifference.Position = expectedLine.Length;
                                stringDifference.Type = DifferenceMode.EndOfLine;
                            }
                            else
                            {
                                stringDifference.Position = expectedLine.Length;
                                stringDifference.Type = DifferenceMode.Longer;
                            }
                        }
                        else if (actualLine.Length < expectedLine.Length)
                        {
                            if ((actualLine.Length +1 == expectedLine.Length) && expectedLine.Last() == '\r')
                            {
                                stringDifference.Position = actualLine.Length;
                                stringDifference.Type = DifferenceMode.EndOfLine;
                            }
                            else
                            {
                                stringDifference.Position = actualLine.Length;
                                stringDifference.Type = DifferenceMode.Shorter;
                            }
                        }
                    }
                    if (stringDifference.Type != DifferenceMode.NoDifference)
                    {
                        result.Add(stringDifference);
                    }
                }
                return result;
            }
            return null;
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
        EndOfLine
    }
}
