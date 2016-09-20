// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Duration.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
//   // //   Licensed under the Apache License, Version 2.0 (the "License");
//   // //   you may not use this file except in compliance with the License.
//   // //   You may obtain a copy of the License at
//   // //       http://www.apache.org/licenses/LICENSE-2.0
//   // //   Unless required by applicable law or agreed to in writing, software
//   // //   distributed under the License is distributed on an "AS IS" BASIS,
//   // //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   // //   See the License for the specific language governing permissions and
//   // //   limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Helpers
{
    using System;

    /// <summary>
    /// Represents a duration as an unit and a quantity.
    /// </summary>
    public struct Duration
    {
        #region fields

        private readonly double rawDuration;

        private readonly TimeUnit timeUnit;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Duration"/> struct. 
        /// </summary>
        /// <param name="rawDuration">
        /// Duration of the raw.
        /// </param>
        /// <param name="timeUnit">
        /// The time unit.
        /// </param>
        public Duration(double rawDuration, TimeUnit timeUnit)
        {
            this.rawDuration = rawDuration;
            this.timeUnit = timeUnit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Duration"/> struct.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="unit">The time unit.</param>
        public Duration(TimeSpan timeSpan, TimeUnit unit)
        {
            this.rawDuration = timeSpan.TotalMilliseconds * TimeHelper.GetConversionFactor(TimeUnit.Milliseconds)
                               / TimeHelper.GetConversionFactor(unit);
            this.timeUnit = unit;
        }

        /// <summary>
        /// Gets the duration quantity expressed in the proper <see cref="TimeUnit"/>.
        /// </summary>
        /// <value>
        /// The duration quantity expressed in the proper <see cref="TimeUnit"/>.
        /// </value>
        public double RawDuration
        {
            get
            {
                return this.rawDuration;
            }
        }

        /// <summary>
        /// Gets the unit in which the duration is express.
        /// </summary>
        /// <value>
        /// The unit used for the duration.
        /// </value>
        public TimeUnit Unit
        {
            get
            {
                return this.timeUnit;
            }
        }

        /// <summary>
        /// Converts a given duration to a number of milliseconds.
        /// </summary>
        /// <param name="duration">The duration value.</param>
        /// <param name="durationTimeUnit">The time unit of the duration.</param>
        /// <returns>The number of milliseconds corresponding to that duration.</returns>
        public static int ConvertToMilliseconds(double duration, TimeUnit durationTimeUnit)
        {
            var durationInst = new Duration(duration, durationTimeUnit);
            var rawDurationInMsec = durationInst.ConvertTo(TimeUnit.Milliseconds).RawDuration;
            return Convert.ToInt32(rawDurationInMsec);
        }

        /// <summary>
        /// Checks if duration a is less than duration b.
        /// </summary>
        /// <param name="a">
        /// First operand.
        /// </param>
        /// <param name="b">
        /// Second operand.
        /// </param>
        /// <returns>
        /// True if a is less than b.
        /// </returns>
        public static bool operator <(Duration a, Duration b)
        {
            var firstFactor = TimeHelper.GetConversionFactor(a.Unit);
            var secondFactor = TimeHelper.GetConversionFactor(b.Unit);
            return a.RawDuration < (b.RawDuration / firstFactor * secondFactor);
        }

        /// <summary>
        /// Checks if duration a is greater than or equal to duration b.
        /// </summary>
        /// <param name="a">
        /// First operand.
        /// </param>
        /// <param name="b">
        /// Second operand.
        /// </param>
        /// <returns>
        /// True if a is greater than or equal to b.
        /// </returns>
        public static bool operator >=(Duration a, Duration b)
        {
            var firstFactor = TimeHelper.GetConversionFactor(a.Unit);
            var secondFactor = TimeHelper.GetConversionFactor(b.Unit);
            return a.RawDuration >= (b.RawDuration / firstFactor * secondFactor);            
        }

        /// <summary>
        /// Checks if duration a is less than or equal to duration b.
        /// </summary>
        /// <param name="a">
        /// First operand.
        /// </param>
        /// <param name="b">
        /// Second operand.
        /// </param>
        /// <returns>
        /// True if a is less than or equal to b.
        /// </returns>
        public static bool operator <=(Duration a, Duration b)
        {
            var firstFactor = TimeHelper.GetConversionFactor(a.Unit);
            var secondFactor = TimeHelper.GetConversionFactor(b.Unit);
            return a.RawDuration <= (b.RawDuration / firstFactor * secondFactor);
        }

        /// <summary>
        /// Checks if duration a is greater than duration b.
        /// </summary>
        /// <param name="a">
        /// First operand.
        /// </param>
        /// <param name="b">
        /// Second operand.
        /// </param>
        /// <returns>
        /// True if a is greater than b.
        /// </returns>
        public static bool operator >(Duration a, Duration b)
        {
            var firstFactor = TimeHelper.GetConversionFactor(a.Unit);
            var secondFactor = TimeHelper.GetConversionFactor(b.Unit);
            return a.RawDuration > (b.RawDuration / firstFactor * secondFactor);
        }

        /// <summary>
        /// Checks if the duration is equal to another object.
        /// </summary>
        /// <param name="left">
        /// First comparand.
        /// </param>
        /// <param name="right">
        /// Second comparand.
        /// </param>
        /// <returns>
        /// True if both operand represents the same duration.
        /// </returns>
        public static bool operator ==(Duration left, Duration right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if the duration is different from another object.
        /// </summary>
        /// <param name="left">
        /// First comparand.
        /// </param>
        /// <param name="right">
        /// Second comparand.
        /// </param>
        /// <returns>
        /// True if both operand represents different duration.
        /// </returns>
        public static bool operator !=(Duration left, Duration right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Checks if the duration is equal to another one.
        /// </summary>
        /// <param name="other">The duration to compare to.</param>
        /// <returns>True if both Durations represents the same duration.</returns>
        public bool Equals(Duration other)
        {
            var firstFactor = TimeHelper.GetConversionFactor(this.Unit);
            var secondFactor = TimeHelper.GetConversionFactor(other.Unit);
            return this.RawDuration == (other.RawDuration / firstFactor * secondFactor);
        }

        /// <summary>
        /// Checks if the duration is equal to another object.
        /// </summary>
        /// <param name="obj">The duration to compare to.</param>
        /// <returns>True if both Durations represents the same duration.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Duration && this.Equals((Duration)obj);
        }

        /// <summary>
        /// Gets the hash value of this instance.
        /// </summary>
        /// <returns>
        /// A 32 bits integer representing the hash value.
        /// </returns>
        public override int GetHashCode()
        {
            return this.rawDuration.GetHashCode() + this.timeUnit.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", this.rawDuration, this.timeUnit);
        }

        /// <summary>
        /// Gets a new instance for the same duration expressed in another <see cref="TimeUnit"/>
        /// </summary>
        /// <param name="newTmeUnit">The target time unit.</param>
        /// <returns>The new <see cref="Duration"/>.</returns>
        public Duration ConvertTo(TimeUnit newTmeUnit)
        {
            var newDuration = TimeHelper.GetInNanoSeconds(this.rawDuration, this.timeUnit);
            return new Duration(TimeHelper.GetFromNanoSeconds(newDuration, newTmeUnit), newTmeUnit);
        }
    }
}