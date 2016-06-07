using System;

namespace NFluent.Extensibility
{
    internal class RangeBlock
    {
        private double referenceValue;
        private double tolerance;

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
