// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LongSignedCheckExtensions.cs" company="">
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
    using Kernel;

    /// <summary>
    /// Provides check methods to be executed on an <see cref="long"/> value that is considered as a signed number.
    /// </summary>
    public static class LongSignedCheckExtensions
    {
        // DoNotChangeOrRemoveThisLine

        #pragma warning restore 169

        /// <summary>
        /// Checks that the actual value is strictly positive.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive.</exception>
        [Obsolete("Use IsStrictlyPositive instead.")]
        public static ICheckLink<ICheck<long>> IsPositive(this ICheck<long> check)
        {
            return new NumberCheck<long>(check).IsStrictlyPositive();
        }

        /// <summary>
        /// Checks that the actual value is strictly positive.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive.</exception>
        public static ICheckLink<ICheck<long>> IsStrictlyPositive(this ICheck<long> check)
        {
            return new NumberCheck<long>(check).IsStrictlyPositive();
        }

        /// <summary>
        /// Checks that the actual value is positive or equal to zero.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not positive or equal to zero.</exception>
        public static ICheckLink<ICheck<long>> IsPositiveOrZero(this ICheck<long> check)
        {
            return new NumberCheck<long>(check).IsPositiveOrZero();
        }

        /// <summary>
        /// Checks that the actual value is strictly negative.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not strictly negative.</exception>
        [Obsolete("Use IsStrictlyNegative instead.")]
        public static ICheckLink<ICheck<long>> IsNegative(this ICheck<long> check)
        {
            return new NumberCheck<long>(check).IsStrictlyNegative();
        }

        /// <summary>
        /// Checks that the actual value is strictly negative.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not strictly negative.</exception>
        public static ICheckLink<ICheck<long>> IsStrictlyNegative(this ICheck<long> check)
        {
            return new NumberCheck<long>(check).IsStrictlyNegative();
        }

        /// <summary>
        /// Checks that the actual value is negative or equal to zero.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not negative or equal to zero.</exception>
        public static ICheckLink<ICheck<long>> IsNegativeOrZero(this ICheck<long> check)
        {
            return new NumberCheck<long>(check).IsNegativeOrZero();
        }

    }
}
