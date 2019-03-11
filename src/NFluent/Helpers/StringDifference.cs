// -------------------------------------------------------------------------------------------------------------------
// <copyright file="StringDifference.cs" company="">
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

namespace NFluent.Helpers
{
    using System;
    using System.Collections.Generic;
    using Extensions;

    /// <summary>
    /// Describes difference between strings.
    /// </summary>
    internal enum DifferenceMode
    {
        /// <summary>
        /// Strings are same.
        /// </summary>
        NoDifference,

        /// <summary>
        /// General difference.
        /// </summary>
        General,

        /// <summary>
        /// Difference only in case (e.g. Foo vs foo).
        /// </summary>
        CaseDifference,

        /// <summary>
        /// Contains at least one longer line.
        /// </summary>
        LongerLine,

        /// <summary>
        /// Contains at least one shorter line. 
        /// </summary>
        ShorterLine,

        /// <summary>
        /// End of line marker is different.
        /// </summary>
        EndOfLine,

        /// <summary>
        /// Line(s) are missing.
        /// </summary>
        MissingLines,

        /// <summary>
        /// Extra lines found.
        /// </summary>
        ExtraLines,

        /// <summary>
        /// Different in spaces (one vs many, tabs vs spaces...).
        /// </summary>
        Spaces,

        /// <summary>
        /// String is longer.
        /// </summary>
        Longer,

        /// <summary>
        /// String is shorter.
        /// </summary>
        Shorter,

        /// <summary>
        /// Strings have the same length but are different.
        /// </summary>
        GeneralSameLength
    }

    internal class StringDifference
    {
        private const char Linefeed = '\n';
        private const char CarriageReturn = '\r';

        private StringDifference(DifferenceMode mode, int line, int position, string actual, string expected, bool isFulltext)
        {
            this.Kind = mode;
            this.Line = line;
            this.Position = position;
            this.Expected = expected;
            this.Actual = actual;
            this.IsFullText = isFulltext;
        }

        public DifferenceMode Kind { get; }

        public int Line { get; }

        public string Expected { get; internal set; }

        public string Actual { get; internal set; }

        public int Position { get; }

        private bool IsFullText { get; }

        private static string[] SplitLines(string text)
        {
            var temp = new List<string>();
            var index = 0;
            while (index <=text.Length)
            {
                var start = index;
                if (index == text.Length)
                {
                    temp.Add(string.Empty);
                    break;
                }
                index = text.IndexOf(Linefeed, start);
                if (index < 0)
                {
                    temp.Add(text.Substring(start));
                    index = text.Length+1;
                }
                else
                {
                    temp.Add(text.Substring(start, index+1-start));
                    index = index + 1;
                }
            }
            return temp.ToArray();
        }

        public static IList<StringDifference> Analyze(string actual, string expected, bool caseInsensitive)
        {
            if (actual == expected)
            {
                return null;
            }

            var result = new List<StringDifference>();
            var actualLines = SplitLines(actual);
            var expectedLines = SplitLines(expected);
            var sharedLines = Math.Min(actualLines.Length, expectedLines.Length);
            var boolSingleLine =  expectedLines.Length == 1;
            for (var line = 0; line < sharedLines; line++)
            {
                var stringDifference = Build(line, actualLines[line], expectedLines[line], caseInsensitive, boolSingleLine);
                if (stringDifference.Kind != DifferenceMode.NoDifference)
                {
                    result.Add(stringDifference);
                }
            }

            if (expectedLines.Length > sharedLines)
            {
                result.Add(Build(sharedLines, null, expectedLines[sharedLines], caseInsensitive, false));
            }
            else if (actualLines.Length > sharedLines)
            {
                result.Add(Build(sharedLines, actualLines[sharedLines], null, caseInsensitive, false));
            }
            return result;
        }

        /// <summary>
        /// Summarize a list of issues to a single difference code.
        /// </summary>
        /// <param name="stringDifferences">
        /// List of differences.
        /// </param>
        /// <returns>
        /// A <see cref="DifferenceMode"/> value describing the overall differences.
        /// </returns>
        /// <remarks>
        /// Returns <see cref="DifferenceMode.General"/> unless all differences are of same kind.
        /// </remarks>
        public static DifferenceMode Summarize(IEnumerable<StringDifference> stringDifferences)
        {
            var result = DifferenceMode.NoDifference;
            foreach (var stringDifference in stringDifferences)
            {
                if (result == DifferenceMode.NoDifference)
                {
                    result = stringDifference.Kind;
                }
                else if (result != stringDifference.Kind)
                {
                    result = DifferenceMode.General;
                    break;
                }
            }

            return result;
        }

        public static string SummaryMessage(IList<StringDifference> differences)
        {
            return differences[0].GetErrorMessage(Summarize(differences));
        }

        /// <summary>
        /// Transform a string to identify not printable difference
        /// </summary>
        /// <param name="textToScan"></param>
        /// <returns></returns>
        public string HighLightForDifference(string textToScan)
        {
            switch (this.Kind)
            {
                case DifferenceMode.Spaces:
                    textToScan = HighlightTabsIfAny(textToScan);
                    break;
                case DifferenceMode.EndOfLine:
                    textToScan = HighlightCrlfOrLfIfAny(textToScan);
                    break;
            }

            return textToScan;
        }

        private string GetErrorMessage(DifferenceMode summary)
        {
            var mainText = GetMessage(summary);
 
            const int extractLength = 20;
            string actual;
            string expected;
            if (this.Line > 0 || this.Expected.Length > extractLength * 2 || this.Actual.Length > extractLength * 2)
            {
                actual = this.Actual.Extract(this.Position, extractLength);
                expected = this.Expected.Extract(this.Position, extractLength);
            }
            else
            {
                actual = this.Actual;
                expected = this.Expected;
            }
            actual = this.HighLightForDifference(actual);
            expected = this.HighLightForDifference(expected);

            if (!this.IsFullText || this.Actual != actual || this.Expected != expected)
            {
                if (summary == DifferenceMode.MissingLines)
                {
                    mainText+=
                        $" At line {this.Line + 1}, expected '{HighlightCrlfOrLfIfAny(expected)}' but line is missing.";
                }
                else if (summary == DifferenceMode.ExtraLines)
                {
                    mainText+=
                        $" Found line {this.Line + 1} '{HighlightCrlfOrLfIfAny(actual)}'.";
                }
                else
                {
                    mainText += string.Format(
                    " At line {0}, col {3}, expected '{1}' was '{2}'.",
                    this.Line + 1,
                    HighlightCrlfOrLfIfAny(expected),
                    HighlightCrlfOrLfIfAny(actual),
                    this.Position + 1);
                }
            }

            return mainText;
        }

        /// <summary>
        /// Inserts &lt;&lt;CRLF&gt;&gt; before the first CRLF or &lt;&lt;LF&gt;&gt; before the first LF.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The same string but with &lt;&lt;CRLF&gt;&gt; inserted before the first CRLF or &lt;&lt;LF&gt;&gt; inserted before the first LF.</returns>
        private static string HighlightCrlfOrLfIfAny(string str)
        {
            str = str.Replace("\r\n", "<<CRLF>>");
            //str = str.Replace("\r", "<<CR>>"); //==> isolated CR are not considered as EOL markers
            str = str.Replace("\n", "<<LF>>");
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

        private static StringDifference Build(int line, string actual, string expected, bool ignoreCase, bool isFullText)
        {
            if (actual == null)
            {
                return new StringDifference(DifferenceMode.MissingLines, line, 0, null, expected, isFullText);
            }

            if (expected == null)
            {
                return new StringDifference(DifferenceMode.ExtraLines, line, 0, actual, null, isFullText);
            }

            // check the common part of both strings
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
                if (char.IsWhiteSpace(actualChar) && char.IsWhiteSpace(expectedChar))
                {
                    // special case for end of line markers
                    if (IsEol(actualChar))
                    {
                        if (expectedChar == actualChar)
                        {
                            continue;
                        }

                        return new StringDifference(
                            IsEol(expectedChar) ? DifferenceMode.EndOfLine : DifferenceMode.ShorterLine, line, i,
                            actual, expected, isFullText);
                    }

                    if (IsEol(expectedChar))
                    {
                        return new StringDifference(DifferenceMode.LongerLine, line, i, actual, expected, isFullText);
                    }
                    var actualStart = i;
                    var expectedStart = j;

                    // we skip all spaces
                    while (ContainsWhiteSpaceAt(actual, i+1))
                    {
                        i++;
                    }

                    while (ContainsWhiteSpaceAt(expected, j+1))
                    {
                        j++;
                    }

                    if (actual.Substring(actualStart, i - actualStart)
                        != expected.Substring(expectedStart, j - expectedStart))
                    {
                        if (type != DifferenceMode.Spaces)
                        {       
                            type = DifferenceMode.Spaces;
                            position = i;
                        }
                    }
                }
                else if (actualChar == expectedChar)
                {
                }
                else if (StringExtensions.CompareCharIgnoringCase(actualChar, expectedChar))
                {
                    if (ignoreCase)
                    {
                        continue;
                    }

                    // difference in case only
                    if (type == DifferenceMode.CaseDifference)
                    {
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
            }

            switch (type)
            {
                case DifferenceMode.General:
                    if (actual.Length == expected.Length)
                    {
                        type = DifferenceMode.GeneralSameLength;
                    }

                    return new StringDifference(type, line, position, actual, expected, isFullText);
                case DifferenceMode.Spaces when i == actual.Length && j == expected.Length:
                    return new StringDifference(type, line, position, actual, expected, isFullText);
            }

            // strings are same so far
            // the actualLine string is longer than expectedLine
            if (i < actual.Length)
            {
                DifferenceMode difference;
                if (IsEol(actual[i]))
                {
                    // lines are missing, the error will be reported at next line
                    difference = DifferenceMode.NoDifference;
                }
                else
                    difference = isFullText ? DifferenceMode.Longer : DifferenceMode.LongerLine;

                return new StringDifference(
                    difference, 
                    line,
                    i,
                    actual, 
                    expected, 
                    isFullText);
            }

            if (j < expected.Length)
            {
                DifferenceMode difference;
                if (IsEol(expected[j]))
                {
                    // lines are missing, the error will be reported at next sline
                    difference = DifferenceMode.NoDifference;
                }
                else
                    difference = isFullText ? DifferenceMode.Shorter : DifferenceMode.ShorterLine;
                return new StringDifference(
                    difference,
                    line,
                    j,
                    actual,
                    expected, 
                    isFullText);
            }

            return new StringDifference(type, line, position, actual, expected, isFullText);
        }

        private static bool ContainsWhiteSpaceAt(string actual, int i)
        {
            return i < actual.Length && char.IsWhiteSpace(actual[i]) && !IsEol(actual[i]);
        }

        private static bool IsEol(char theChar)
        {
            return theChar == CarriageReturn || theChar == Linefeed;
        }

        /// <summary>
        /// Get general message
        /// </summary>
        /// <param name="summary">Synthetic error</param>
        /// <returns></returns>
        public static string GetMessage(DifferenceMode summary)
        {
            string message;
            switch (summary)
            {
                default:
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

        #endregion
    }
}