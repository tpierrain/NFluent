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
    using Helpers;

    /// <summary>
    ///     Class representing a range of value
    /// </summary>
    internal class DateTimeRangeBlock
    {
        private readonly DateTime referenceValue;
        private readonly TimeSpan tolerance;
        private readonly string toleranceString;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="referenceValue">Reference value (mid point)</param>
        /// <param name="tolerance">Tolerance</param>
        /// <remarks>This represents a range of <see cref="referenceValue" /> +/- <see cref="tolerance" /></remarks>
        public DateTimeRangeBlock(DateTime referenceValue, TimeSpan tolerance)
        {
            this.referenceValue = referenceValue;
            this.tolerance = tolerance;
            this.toleranceString = tolerance.ToString();
        }
        
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="referenceValue">Reference value (mid point)</param>
        /// <param name="tolerance">Tolerance</param>
        /// <remarks>This represents a range of <see cref="referenceValue" /> +/- <see cref="tolerance" /></remarks>
        public DateTimeRangeBlock(DateTime referenceValue, Duration tolerance)
        {
            this.referenceValue = referenceValue;
            this.tolerance = tolerance.ToTimeSpan();
            this.toleranceString = tolerance.ToString();
        }

        public bool IsInRange(DateTime value)
        {
            return (this.referenceValue - value).Duration() <= this.tolerance;
        }

        public override string ToString()
        {
            return $"{this.referenceValue} (+/- {this.toleranceString})";
        }
    }
}