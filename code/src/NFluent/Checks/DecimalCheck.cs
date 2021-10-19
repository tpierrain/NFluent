// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DecimalCheck.cs" company="NFluent">
//   Copyright 2020 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
    using Extensibility;

    /// <summary>
    /// Hosts Decimal specific checks
    /// </summary>
    public static class DecimalCheck
    {
        /// <summary>
        /// Determines whether the actual number is close to an expected value within a given within.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="within">The within.</param>
        /// <returns>A continuation check.</returns>
        public static ICheckLink<ICheck<decimal>> IsCloseTo(this ICheck<decimal> check, decimal expected, decimal within)
        {
            var range = new RangeBlock(expected, within);
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => !range.IsInRange(sut), "The {0} is outside the expected value range.")
                .DefineExpectedValue(range).OnNegate("The {0} is within the expected range, whereas it must not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        private class RangeBlock
        {
            private readonly decimal referenceValue;
            private readonly decimal tolerance;

            /// <summary>
            ///     Constructor
            /// </summary>
            /// <param name="referenceValue">Reference value (mid point)</param>
            /// <param name="tolerance">Tolerance</param>
            /// <remarks>This represents a range of <see cref="referenceValue" /> +/- <see cref="tolerance" /></remarks>
            public RangeBlock(decimal referenceValue, decimal tolerance)
            {
                this.referenceValue = referenceValue;
                this.tolerance = tolerance;
            }

            public bool IsInRange(decimal value)
            {
                return Math.Abs(this.referenceValue - value) <= this.tolerance;
            }

            public override string ToString()
            {
                return $"{this.referenceValue} (+/- {this.tolerance})";
            }
        }
    }
}