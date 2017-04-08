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

using System.Text;

namespace NFluent.Extensions
{
    using System;

    /// <summary>
    ///     Hosts all string related extensions used by NFluent
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        ///     Generates an espaced copy of a chain for use in format (e.G. { replaced by {{).
        /// </summary>
        /// <param name="toEscape">String to be escaped</param>
        /// <returns>Escaped version of the string.</returns>
        public static string Escaped(this string toEscape)
        {
            return toEscape.Replace("{", "{{").Replace("}", "}}");
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
        /// If truncation was needeed, three dots are added were appropriate.
        /// </summary>
        /// <param name="texte">Texte to extract from</param>
        /// <param name="middle">Middle position</param>
        /// <param name="len">Length of the extract</param>
        /// <returns>the desired substring.</returns>
        public static string Extract(this string texte, int middle, int len)
        {
            if (texte == null)
                return texte;
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
            if (middle + len >= texte.Length)
            {
                len = texte.Length - middle;
            }
            result.Append(texte.Substring(middle, len).Escaped());
            if (middle + len < texte.Length)
            {
                result.Append("...");
            }
            return result.ToString();
        }

    }
}