// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumbersRelatedExtension.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace NFluent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extension methods for exploiting enumerable content in a fluent manner (i.e. with auto completion support and in an english readable way).
    /// </summary>
    public static class NumbersRelatedExtension
    {
        /// <summary>
        /// Verifies that the actual value is equal to zero.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is zero; throws a <see cref="FluentAssertionException"/> otherwise.
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is not equal to zero.</exception>
        public static bool IsZero(this int value)
        {
            if (!int.Equals(0, value))
            {
                throw new FluentAssertionException(string.Format("[{0}] is not equal to zero.", value));
            }

            return true;
        }
    }
}
