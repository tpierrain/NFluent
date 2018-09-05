//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EntityNamer.cs" company="">
//    Copyright 2014 Cyrille DUPUYDAUBY
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    /// <summary>
    ///     Hosts all string related extensions used by NFluent
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Doubles the curly braces in the string.
        /// </summary>
        /// <returns>The string having curly braces doubled.</returns>
        /// <param name="value">String to correct.</param>
        public static string DoubleCurlyBraces(this string value)
        {
            return value.Replace("{", "{{").Replace("}", "}}");
        }
        
        /// <summary>
        /// Compare char in a case sensitive or insensitive way.
        /// </summary>
        /// <param name="carA">first char</param>
        /// <param name="carB">second char</param>
        /// <returns>true if chars are the same.</returns>
        public static bool CompareCharIgnoringCase(char carA, char carB)
        {
            return string.Compare(carA.ToString(), carB.ToString(), StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        /// <summary>
        /// Extracts a sub string based on a middle position and a length.
        /// If truncation was needed, three dots are added were appropriate.
        /// </summary>
        /// <param name="text">Text to extract from</param>
        /// <param name="middle">Middle position</param>
        /// <param name="len">Length of the extract</param>
        /// <returns>the desired substring.</returns>
        public static string Extract(this string text, int middle, int len)
        {
            if (text == null)
                return null;
            var result = new StringBuilder(len);
            middle -= len / 2;
            if (middle > 0)
            {
                result.Append("...");
            }
            else
            {
                middle = 0;
            }
            if (middle + len >= text.Length)
            {
                len = text.Length - middle;
            }
            result.Append(text.Substring(middle, len).DoubleCurlyBraces());
            if (middle + len < text.Length)
            {
                result.Append("...");
            }
            return result.ToString();
        }

        /// <summary>
        /// Transform a string to an <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="checkedString"></param>
        /// <returns></returns>
        public static IList<string> SplitAsLines(this string checkedString)
        {
            IList<string> next;
            if (checkedString != null)
            {
                var start = 0;
                var retLines = new List<string>();
                var newLineLength = Environment.NewLine.Length;
                while (start < checkedString.Length)
                {
                    var indexOf = checkedString.IndexOf(Environment.NewLine, start, StringComparison.Ordinal);
                    if (indexOf == -1)
                    {
                        indexOf = checkedString.Length;
                    }

                    retLines.Add(checkedString.Substring(start, indexOf - start));
                    start = indexOf + newLineLength;
                }

                next = retLines;
            }
            else
            {
                next = new List<string>();
            }

            return next;
        }
    }
}