using System;

namespace NFluent.Messages
{
    /// <summary>
    /// Class representing a range of value
    /// </summary>
    internal class RangeBlock
    {
        private readonly double referenceValue;
        private readonly double tolerance;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="referenceValue">Reference value (mid point)</param>
        /// <param name="tolerance">Tolerance</param>
        /// <remarks>This represents a range of <see cref="referenceValue"/> +/- <see cref="tolerance"/></remarks>
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
            return string.Format("{0} (+/- {1})", this.referenceValue, this.tolerance);
        }
    }
}
