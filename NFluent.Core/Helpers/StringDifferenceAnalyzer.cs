using System;
using System.Collections.Generic;

namespace NFluent.Helpers
{
    using Extensions;

    internal class StringDifference
    {
        public DifferenceMode Type = DifferenceMode.NoDifference;
        public int Position;

        public int Line { get; internal set; }
        public string Expected { get; internal set; }
        public string Actual { get; internal set; }

        public StringDifference(int line, string actual, string expected)
        {
            this.Line = line;
            this.Expected = expected;
            this.Actual = actual;
            // do we have both strings?
            if (actual == null)
            {
                this.Type = DifferenceMode.MissingLines;
                return;
            }
            if (expected == null)
            {
                this.Type = DifferenceMode.ExtraLines;
                return;
            }
            // check the common part of both strings
            var sharedLine = CheckCommonPart();
            if (this.Type != DifferenceMode.NoDifference)
            {
                // we have difference, we can stop
                return;
            }

            // strings are same so far
            // the actualLine string is longer than expectedLine
            if (actual.Length > expected.Length)
            {
                this.Position = sharedLine;
                this.Type = actual.Substring(sharedLine)=="\r" ? DifferenceMode.EndOfLine : DifferenceMode.Longer;
            }
            else if (actual.Length < expected.Length)
            {
                this.Position = sharedLine;
                this.Type = expected.Substring(sharedLine) == "\r" ? DifferenceMode.EndOfLine : DifferenceMode.Shorter;
            }

        }

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
                var stringDifference = new StringDifference(line, actualLines[line], expectedLines[line]);
                if (stringDifference.Type != DifferenceMode.NoDifference)
                {
                    result.Add(stringDifference);
                }
            }
            if (expectedLines.Length > sharedLines)
            {
                result.Add(new StringDifference(sharedLines, null, expectedLines[sharedLines]));
            }
            else if (actualLines.Length > sharedLines)
            {
                result.Add(new StringDifference(sharedLines, actualLines[sharedLines], null));
            }
            return result;
        }

        private int CheckCommonPart()
        {
            var sharedLine = Math.Min(this.Actual.Length, this.Expected.Length);
            for (var i = 0;
                i < sharedLine;
                i++)
            {
                if (this.Actual[i] == this.Expected[i])
                {
                    // same char
                    continue;
                }
                if (StringExtensions.CompareChar(this.Actual[i], this.Expected[i], true))
                {
                    // difference in case only
                    if (this.Type == DifferenceMode.CaseDifference)
                    {
                        continue;
                    }
                    this.Type = DifferenceMode.CaseDifference;
                    this.Position = i;
                }
                else
                {
                    this.Type = DifferenceMode.General;
                    this.Position = i;
                    break;
                }
            }
            return sharedLine;
        }
    }

    internal enum DifferenceMode
    {
        NoDifference,
        General,
        CaseDifference,
        Longer,
        Shorter,
        EndOfLine,
        MissingLines,
        ExtraLines
    }
}
