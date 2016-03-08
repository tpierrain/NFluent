using System;

namespace NFluent
{
    /// <summary>
    /// Delta.
    /// </summary>
    public sealed class Delta
    {
        /// <summary>
        /// Value of delta
        /// </summary>
        public double Value { get; private set; }

        private Delta(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Create delta of given value
        /// </summary>
        /// <param name="value">value of delta</param>
        /// <returns>Delta</returns>
        public static Delta Of(double value)
        {
            return new Delta(value);
        }

        /// <summary>
        /// Create delta from values
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>Delta</returns>
        public static Delta Calculate(double first, double second)
        {
            var value = Math.Abs(first - second);
            return Delta.Of(value);
        }

        /// <summary>
        /// Equals.
        /// </summary>
        public override bool Equals(object obj)
        {
            var that = obj as Delta;
            return that != null &&
                   Math.Abs(that.Value - Value) <= 0D;
        }

        /// <summary>
        /// HashCode.
        /// </summary>
        public override int GetHashCode()
        {
            var hashCode = 31 + Value.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// ToString.
        /// </summary>
        public override string ToString()
        {
            return String.Format("Delta {{value={0}}}", Value);
        }

        /// <summary>
        /// Compare is this delta is greater than another
        /// </summary>
        /// <param name="thisDelta">delta</param>
        /// <param name="thatDelta">delta</param>
        /// <returns>bool</returns>
        public static bool operator >(Delta thisDelta, Delta thatDelta)
        {
            return thisDelta.Value > thatDelta.Value;
        }

        /// <summary>
        /// Compare is this delta is greater or equal another
        /// </summary>
        /// <param name="thisDelta">delta</param>
        /// <param name="thatDelta">delta</param>
        /// <returns>bool</returns>
        public static bool operator >=(Delta thisDelta, Delta thatDelta)
        {
            return thisDelta.Value >= thatDelta.Value;
        }

        /// <summary>
        /// Compare is this delta is lower than another
        /// </summary>
        /// <param name="thisDelta">delta</param>
        /// <param name="thatDelta">delta</param>
        /// <returns>bool</returns>
        public static bool operator <(Delta thisDelta, Delta thatDelta)
        {
            return thisDelta.Value < thatDelta.Value;
        }

        /// <summary>
        /// Compare is this delta is lower or equal another
        /// </summary>
        /// <param name="thisDelta">delta</param>
        /// <param name="thatDelta">delta</param>
        /// <returns>bool</returns>
        public static bool operator <=(Delta thisDelta, Delta thatDelta)
        {
            return thisDelta.Value <= thatDelta.Value;
        }
    }
}
