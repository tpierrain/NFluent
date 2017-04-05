// -------------------------------------------------------------------------------------------------------------------
// <copyright file="StringDifferenceAnalyzer.cs" company="">
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
using System;
using System.Collections.Generic;
using NFluent.Extensions;

namespace NFluent.Helpers
{
    internal enum DifferenceMode
    {
        NoDifference,
        General,
        CaseDifference,
        LongerLine,
        ShorterLine,
        EndOfLine,
        MissingLines,
        ExtraLines,
        Spaces,
        Longer,
        Shorter
    }

    internal class StringDifference
    {
        private const char Separator = '\n';
        public int Position;
        public readonly DifferenceMode Type;

        public int Line { get; private set; }
        public string Expected { get; internal set; }
        public string Actual { get; internal set; }

        private StringDifference(DifferenceMode mode, int line, int position , string actual, string expected)
        {
            this.Type = mode;
            this.Line = line;
            this.Position = position;
            this.Expected = expected;
            this.Actual = actual;
        }

        private static StringDifference Build(int line, string actual, string expected, bool ignoreCase)
        {
            if (actual == null)
            {
                return new StringDifference(DifferenceMode.MissingLines, line, 0, null, expected);
            }
            if (expected == null)
            {
                return new StringDifference(DifferenceMode.ExtraLines, line, 0, actual, null);
            }
            // check the common part of both strings
            var lastCharWasSpace = true;
            var j = 0;
            var i = 0;
            var type = DifferenceMode.NoDifference;
            var position = 0;
            for (;
                i < actual.Length && j < expected.Length;
                i++, j++)
            {
                var actualChar = actual[i];
                var expectedChar = expected[j];
                if (actualChar == expectedChar)
                {
                    // same char
                }
                else if (char.IsWhiteSpace(actualChar) && char.IsWhiteSpace(expectedChar)
                         || lastCharWasSpace && (char.IsWhiteSpace(actualChar) || char.IsWhiteSpace(expectedChar)))
                {
                    //we skip all spaces
                    while (i + 1 < actual.Length && char.IsWhiteSpace(actual[i + 1]))
                        i++;
                    while (j + 1 < expected.Length && char.IsWhiteSpace(expected[j + 1]))
                        j++;
                    if (type == DifferenceMode.NoDifference)
                        type = DifferenceMode.Spaces;
                }
                else if (StringExtensions.CompareChar(actualChar, expectedChar, true))
                {
                    if (ignoreCase)
                        continue;
                    // difference in case only
                    if (type == DifferenceMode.CaseDifference)
                    {
                        lastCharWasSpace = char.IsWhiteSpace(actualChar);
                        continue;
                    }
                    type = DifferenceMode.CaseDifference;
                    position = i;
                }
                else
                {
                    type = DifferenceMode.General;
                    position = i;
                    break;
                }
                lastCharWasSpace = char.IsWhiteSpace(actualChar);
            }
            if (type == DifferenceMode.General)
                return new StringDifference(type, line, position, actual, expected);

            // strings are same so far
            // the actualLine string is longer than expectedLine
            if (i < actual.Length)
            {
                return new StringDifference(actual[i] == '\r' ? DifferenceMode.EndOfLine : DifferenceMode.LongerLine, 
                    line, i, actual, expected);
            }
            if (j < expected.Length)
            {
                return new StringDifference(expected[j] == '\r' ? DifferenceMode.EndOfLine : DifferenceMode.ShorterLine,
                    line, i, actual, expected);
            }
            return new StringDifference(type, line, position, actual, expected);
        }


        public static IList<StringDifference> Analyze(string actual, string expected, bool caseInsensitive)
        {
            if (actual == expected)
                return null;
            var result = new List<StringDifference>();
            // handle null cases
            if (actual == null)
            {
                result.Add(Build(0, null, expected, caseInsensitive));
                return result;
            }
            if (expected == null)
            {
                result.Add(Build(0, actual, null, caseInsensitive));
                return result;
            }
            // perform a per line analysis
            var actualLines = actual.Split(Separator);
            var expectedLines = expected.Split(Separator);
            var sharedLines = Math.Min(actualLines.Length, expectedLines.Length);
            for (var line = 0; line < sharedLines; line++)
            {
                var stringDifference = Build(line, actualLines[line], expectedLines[line], caseInsensitive);
                if (stringDifference.Type != DifferenceMode.NoDifference)
                    result.Add(stringDifference);
            }
            if (expectedLines.Length > sharedLines)
                result.Add(Build(sharedLines, null, expectedLines[sharedLines], caseInsensitive));
            else if (actualLines.Length > sharedLines)
                result.Add(Build(sharedLines, actualLines[sharedLines], null, caseInsensitive));
            else if (sharedLines == 1 && result.Count==1)
            {
                if (result[0].Type == DifferenceMode.LongerLine)
                {
                    // replace
                    var newDiff = new StringDifference(DifferenceMode.Longer,
                        0,
                        result[0].Position,
                        result[0].Actual,
                        result[0].Expected);
                    result[0] = newDiff;
                }
                else if (result[0].Type == DifferenceMode.ShorterLine)
                {
                    // replace
                    var newDiff = new StringDifference(DifferenceMode.Shorter,
                        0,
                        result[0].Position,
                        result[0].Actual,
                        result[0].Expected);
                    result[0] = newDiff;

                }
            }
            return result;
        }

        public static DifferenceMode Summarize(IEnumerable<StringDifference> stringDifferences)
        {
            var result = DifferenceMode.NoDifference;
            foreach (var stringDifference in stringDifferences)
            {
                if (result == DifferenceMode.NoDifference)
                {
                    result = stringDifference.Type;
                }
                else if (result != stringDifference.Type)
                {
                    result  = DifferenceMode.General;
                    break;
                }
            }
            return result;
        }
    }
}