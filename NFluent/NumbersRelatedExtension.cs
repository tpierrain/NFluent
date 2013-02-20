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
        /// <typeparam name="T">Type of the actual value.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is equal to zero; throws a <see cref="FluentAssertionException" /> otherwise.
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is not equal to zero.</exception>
        public static bool IsZero<T>(this T value)
        {
            bool res = InternalIsZero(value);

            if (!res)
            {
                throw new FluentAssertionException(string.Format("[{0}] is not equal to zero.", value));
            }

            return true;
        }

        /// <summary>
        /// Verifies that the actual value is NOT equal to zero.
        /// </summary>
        /// <typeparam name="T">Type of the actual value.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is NOT equal to zero; throws a <see cref="FluentAssertionException" /> otherwise.
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is equal to zero.</exception>
        public static bool IsNotZero<T>(this T value)
        {
            bool res = InternalIsZero(value);

            if (res)
            {
                throw new FluentAssertionException(string.Format("[{0}] is equal to zero.", value));
            }

            return true;
        }

        //// public static bool IsPositive<T>(this T value)
        //// {
        ////    if (value < 0)
        ////    {
        ////        throw new FluentAssertionException(string.Format("[{0}] is not positive.", value));
        ////    }
        ////    return true;
        //// }

        /// <summary>
        /// Checks whether a given value is equal to zero.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is equal to zero; false otherwise.</returns>
        private static bool InternalIsZero<T>(T value)
        {
            bool res = false;
            if (value is int)
            {
                res = int.Equals(value, 0);
            }
            else if (value is long)
            {
                res = long.Equals(value, 0L);
            }
            else if (value is double)
            {
                res = double.Equals(value, 0D);
            }
            else if (value is decimal)
            {
                res = decimal.Equals(value, 0M);
            }
            else if (value is float)
            {
                res = float.Equals(value, 0F);
            }
            else if (value is short)
            {
                res = short.Equals(value, (short)0.0);
            }
            else if (value is byte)
            {
                res = byte.Equals(value, (byte)0);
            }
            else if (value is uint)
            {
                res = uint.Equals(value, (uint)0);
            }
            else if (value is ushort)
            {
                res = ushort.Equals(value, (ushort)0);
            }
            else if (value is ulong)
            {
                res = ulong.Equals(value, (ulong)0);
            }

            return res;
        }
    }
}
