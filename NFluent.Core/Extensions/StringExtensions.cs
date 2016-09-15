// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StringExtensions.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
namespace NFluent
{
    using System.Linq;

    /// <summary>
    /// Extension methods for string type.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Upper case only the first letter of a string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The string with the first letter upper cased.</returns>
        public static string ToUpperFirstOnly(this string input)
        {
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
