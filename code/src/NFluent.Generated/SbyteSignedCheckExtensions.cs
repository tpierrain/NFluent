﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SbyteSignedCheckExtensions.cs" company="">
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
    using Kernel;

    /// <summary>
    /// Provides check methods to be executed on an <see cref="sbyte"/> value that is considered as a signed number.
    /// </summary>
    public static class SbyteSignedCheckExtensions
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
        public static ICheckLink<ICheck<sbyte>> IsStrictlyPositive(this ICheck<sbyte> check)
        {
            return new NumberCheck<sbyte>(check).IsStrictlyPositive();
        }

        /// <summary>
        /// Checks that the actual value is positive or equal to zero.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not positive or equal to zero.</exception>
        public static ICheckLink<ICheck<sbyte>> IsPositiveOrZero(this ICheck<sbyte> check)
        {
            return new NumberCheck<sbyte>(check).IsPositiveOrZero();
        }

        /// <summary>
        /// Checks that the actual value is strictly negative.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not strictly negative.</exception>
        public static ICheckLink<ICheck<sbyte>> IsStrictlyNegative(this ICheck<sbyte> check)
        {
            return new NumberCheck<sbyte>(check).IsStrictlyNegative();
        }

        /// <summary>
        /// Checks that the actual value is negative or equal to zero.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not negative or equal to zero.</exception>
        public static ICheckLink<ICheck<sbyte>> IsNegativeOrZero(this ICheck<sbyte> check)
        {
            return new NumberCheck<sbyte>(check).IsNegativeOrZero();
        }
    }
}
