// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StringDifferenceAnalyzer.cs" company="">
// //   Copyright 2017 Cyrille Dupuydauby
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using NFluent.Extensions;

namespace NFluent.Helpers
{
    internal class StringDifference
    {
        private const char Separator = '\n';
        public int Position;
        public DifferenceMode Type = DifferenceMode.NoDifference;

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
            var sharedLine = this.CheckCommonPart();
            if (this.Type != DifferenceMode.NoDifference)
                return;

            // strings are same so far
            // the actualLine string is longer than expectedLine
            if (actual.Length > expected.Length)
            {
                this.Position = sharedLine;
                this.Type = actual.Substring(sharedLine) == "\r" ? DifferenceMode.EndOfLine : DifferenceMode.Longer;
            }
            else if (actual.Length < expected.Length)
            {
                this.Position = sharedLine;
                this.Type = expected.Substring(sharedLine) == "\r" ? DifferenceMode.EndOfLine : DifferenceMode.Shorter;
            }
        }

        public int Line { get; internal set; }
        public string Expected { get; internal set; }
        public string Actual { get; internal set; }

        public static IList<StringDifference> Analyze(string actual, string expected)
        {
            if (actual == expected)
                return null;
            // perform a per line analysis
            var actualLines = actual.Split(Separator);
            var expectedLines = expected.Split(Separator);
            var result = new List<StringDifference>();
            var sharedLines = Math.Min(actualLines.Length, expectedLines.Length);
            for (var line = 0; line < sharedLines; line++)
            {
                var stringDifference = new StringDifference(line, actualLines[line], expectedLines[line]);
                if (stringDifference.Type != DifferenceMode.NoDifference)
                    result.Add(stringDifference);
            }
            if (expectedLines.Length > sharedLines)
                result.Add(new StringDifference(sharedLines, null, expectedLines[sharedLines]));
            else if (actualLines.Length > sharedLines)
                result.Add(new StringDifference(sharedLines, actualLines[sharedLines], null));
            return result;
        }

        private int CheckCommonPart()
        {
            var sharedLine = Math.Min(this.Actual.Length, this.Expected.Length);
            var lastCharWasSpace = true;
            var j = 0;
            for (var i = 0;
                i < this.Actual.Length && j < this.Expected.Length;
                i++, j++)
            {
                var actualChar = this.Actual[i];
                var expectedChar = this.Expected[j];
                if (actualChar == expectedChar)
                {
                    // same char
                }
                else if (char.IsWhiteSpace(actualChar) && char.IsWhiteSpace(expectedChar)
                         || lastCharWasSpace && (char.IsWhiteSpace(actualChar) || char.IsWhiteSpace(expectedChar)))
                {
                    //we skip all spaces
                    while (i + 1 < this.Actual.Length && char.IsWhiteSpace(this.Actual[i + 1]))
                        i++;
                    while (j + 1 < this.Expected.Length && char.IsWhiteSpace(this.Expected[j + 1]))
                        j++;
                    if (this.Type == DifferenceMode.NoDifference)
                        this.Type = DifferenceMode.Spaces;
                }
                else if (StringExtensions.CompareChar(actualChar, expectedChar, true))
                {
                    // difference in case only
                    if (this.Type == DifferenceMode.CaseDifference)
                    {
                        lastCharWasSpace = char.IsWhiteSpace(actualChar);
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
                lastCharWasSpace = char.IsWhiteSpace(actualChar);
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
        ExtraLines,
        Spaces
    }
}