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
using NFluent.Extensibility;
using NFluent.Extensions;

namespace NFluent.Helpers
{
    /// <summary>
    /// Describes difference between strings
    /// </summary>
    internal enum DifferenceMode
    {
        /// <summary>
        /// Strings are same
        /// </summary>
        NoDifference,
        /// <summary>
        /// General difference
        /// </summary>
        General,
        /// <summary>
        /// Difference only in case (eg. Foo vs foo)
        /// </summary>
        CaseDifference,
        /// <summary>
        /// Contains at least one longer line
        /// </summary>
        LongerLine,
        /// <summary>
        /// Contains at least one shorter line 
        /// </summary>
        ShorterLine,
        /// <summary>
        /// End of line marker is different.
        /// </summary>
        EndOfLine,
        /// <summary>
        /// Line(s) are missing
        /// </summary>
        MissingLines,
        /// <summary>
        /// Extra lines found
        /// </summary>
        ExtraLines,
        /// <summary>
        /// Different in spaces (one vs many, tabs vs spaces...)
        /// </summary>
        Spaces,
        /// <summary>
        /// String is longer
        /// </summary>
        Longer,
        /// <summary>
        /// String is shorter
        /// </summary>
        Shorter,
        /// <summary>
        /// Strings have the same length but are different.
        /// </summary>
        GeneralSameLength
    }

    internal class StringDifference
    {
        private const char Separator = '\n';
        public int Position;
        public readonly DifferenceMode Type;
        private const char CarriageReturn='\r';

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

        /// <summary>
        /// Summarize a list of issues to a single difference code.
        /// </summary>
        /// <param name="stringDifferences">list of differences</param>
        /// <returns>a <see cref="DifferenceMode"/> value describing the overall differences.</returns>
        /// <remarks>Returns <see cref="DifferenceMode.General"/> unless all differences are of same kind.</remarks>
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
                    result = DifferenceMode.General;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Builds a <see cref="FluentMessage"/> instance describing the difference.
        /// </summary>
        /// <returns>A <see cref="FluentMessage"/> instance.</returns>
        public FluentMessage BuildMessage(DifferenceMode summary)
        {
            var message = GetMessage(summary);
            var actual = this.Actual;
            var expected = this.Expected;
            if (summary == DifferenceMode.Spaces)
            {
                actual = HighlightTabsIfAny(actual);
                expected = HighlightTabsIfAny(expected);
            }
            else if (summary == DifferenceMode.EndOfLine)
            {
                actual = HighlightFirstCrlfOrLfIfAny(actual);
                expected = HighlightFirstCrlfOrLfIfAny(expected);
            }
            else
            {
                actual = actual?.TrimEnd('\r');
                expected = expected?.TrimEnd('\r');
            }
            var extractLength = 20;
            if (this.Line > 0 || this.Expected.Length>extractLength*2 || this.Actual.Length>extractLength*2)
            {
                message += string.Format(
                    " At line {0}, col {3}, expected '{1}' was '{2}'.",
                    this.Line+1,
                    expected.Extract(this.Position, extractLength),
                    actual.Extract(this.Position, extractLength),
                    this.Position+1);

            }
            var updatedMessage = FluentMessage.BuildMessage(message);
            updatedMessage.On(actual).And.Expected(expected);
            return updatedMessage;
        }

        /// <summary>
        /// Inserts &lt;&lt;CRLF&gt;&gt; before the first CRLF or &lt;&lt;LF&gt;&gt; before the first LF.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The same string but with &lt;&lt;CRLF&gt;&gt; inserted before the first CRLF or &lt;&lt;LF&gt;&gt; inserted before the first LF.</returns>
        private static string HighlightFirstCrlfOrLfIfAny(string str)
        {
            if (str.EndsWith("\r"))
            {
                str = str.Substring(0, str.Length - 1) + "<<CRLF>>";
            }
            else
            {
                str = str + "<<LF>>";
            }
            return str;
        }

        /// <summary>
        /// Replace every tab char by "&lt;&lt;tab&gt;&gt;".
        /// </summary>
        /// <param name="str">The original string.</param>
        /// <returns>The original string with every \t replaced with "&lt;&lt;tab&gt;&gt;".</returns>
        private static string HighlightTabsIfAny(string str)
        {
            return str.Replace("\t", "<<tab>>");
        }

        #region Static Methods

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
                else if (StringExtensions.CompareCharIgnoringCase(actualChar, expectedChar))
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
            {
                if (actual.Length == expected.Length)
                    type = DifferenceMode.GeneralSameLength;
                return new StringDifference(type, line, position, actual, expected);
            }

            // strings are same so far
            // the actualLine string is longer than expectedLine
            if (i < actual.Length)
            {
                return new StringDifference(actual[i] == CarriageReturn ? DifferenceMode.EndOfLine : DifferenceMode.LongerLine, 
                    line, i, actual, expected);
            }
            if (j < expected.Length)
            {
                return new StringDifference(expected[j] == CarriageReturn ? DifferenceMode.EndOfLine : DifferenceMode.ShorterLine,
                    line, i, actual, expected);
            }
            return new StringDifference(type, line, position, actual, expected);
        }

        private static string GetMessage(DifferenceMode summary)
        {
            string message = string.Empty;
            switch (summary)
            {
                case DifferenceMode.General:
                    message = "The {0} is different from {1}.";
                    break;
                case DifferenceMode.GeneralSameLength:
                    message = "The {0} is different from the {1} but has same length.";
                    break;
                case DifferenceMode.Spaces:
                    message = "The {0} has different spaces than {1}.";
                    break;
                case DifferenceMode.EndOfLine:
                    message = "The {0} has different end of line markers than {1}.";
                    break;
                case DifferenceMode.CaseDifference:
                    message = "The {0} is different in case from the {1}.";
                    break;
                case DifferenceMode.ExtraLines:
                    message = "The {0} is different from {1}, it contains extra lines at the end.";
                    break;
                case DifferenceMode.LongerLine:
                    message = "The {0} is different from {1}, one line is longer.";
                    break;
                case DifferenceMode.ShorterLine:
                    message = "The {0} is different from {1}, one line is shorter.";
                    break;
                case DifferenceMode.MissingLines:
                    message = "The {0} is different from {1}, it is missing some line(s).";
                    break;
                case DifferenceMode.Longer:
                    message = "The {0} is different from {1}, it contains extra text at the end.";
                    break;
                case DifferenceMode.Shorter:
                    message = "The {0} is different from {1}, it is missing the end.";
                    break;
            }
            return message;
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
        #endregion
    }
}