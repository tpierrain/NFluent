// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="RangeBlock.cs" company="NFluent">
//   Copyright 2019 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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

namespace NFluent.Messages
{
    using System;

    /// <summary>
    ///     Class representing a range of value
    /// </summary>
    internal class RangeBlock
    {
        private readonly double referenceValue;
        private readonly double tolerance;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="referenceValue">Reference value (mid point)</param>
        /// <param name="tolerance">Tolerance</param>
        /// <remarks>This represents a range of <see cref="referenceValue" /> +/- <see cref="tolerance" /></remarks>
        public RangeBlock(double referenceValue, double tolerance)
        {
            this.referenceValue = referenceValue;
            this.tolerance = tolerance;
        }

        public bool IsInRange(double value)
        {
            return Math.Abs(this.referenceValue - value) <= this.tolerance;
        }

        public override string ToString()
        {
            return $"{this.referenceValue} (+/- {this.tolerance})";
        }
    }
}