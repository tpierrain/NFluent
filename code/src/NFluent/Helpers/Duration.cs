﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Duration.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
//         http://www.apache.org/licenses/LICENSE-2.0
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
// </copyright>

namespace NFluent.Helpers
{
    using System;

    /// <summary>
    /// Represents a duration as an unit and a quantity.
    /// </summary>
    public struct Duration
    {
        private readonly TimeSpan duration;

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
            this.duration = new TimeSpan();
            if (timeUnit == TimeUnit.Microseconds)
            {
                this.duration = TimeSpan.FromTicks((long) rawDuration * 10);
            }
            else if (timeUnit == TimeUnit.Milliseconds)
            {
                this.duration = TimeSpan.FromMilliseconds(rawDuration);
            }
            else if (timeUnit == TimeUnit.Seconds)
            {
                this.duration = TimeSpan.FromSeconds(rawDuration);
            }
            else if (timeUnit == TimeUnit.Minutes)
            {
                this.duration = TimeSpan.FromMinutes(rawDuration);
            }
            else if (timeUnit == TimeUnit.Hours)
            {
                this.duration = TimeSpan.FromHours(rawDuration);
            }
            else if (timeUnit == TimeUnit.Days)
            {
                this.duration = TimeSpan.FromDays(rawDuration);
            }
            else if (timeUnit == TimeUnit.Weeks)
            {
                this.duration = TimeSpan.FromDays(rawDuration * 7);
            }
            else
            {
                this.duration = TimeSpan.FromTicks((long) rawDuration / 100);
            }

            if (!Enum.IsDefined(typeof(TimeUnit), timeUnit))
            {
                throw new ArgumentException();
            }
            this.Unit = timeUnit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Duration"/> struct.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="unit">The time unit.</param>
        public Duration(TimeSpan timeSpan, TimeUnit unit)
        {
            this.duration = timeSpan;
            this.Unit = unit;
        }

        /// <summary>
        /// Gets the duration quantity expressed in the proper <see cref="NFluent.TimeUnit"/>.
        /// </summary>
        /// <value>
        /// The duration quantity expressed in the proper <see cref="NFluent.TimeUnit"/>.
        /// </value>
        public double RawDuration
        {
            get
            {
                if (this.Unit == TimeUnit.Microseconds)
                {
                    return this.duration.Ticks / 10.0;
                }
                else if (this.Unit == TimeUnit.Milliseconds)
                {
                    return this.duration.TotalMilliseconds;
                }
                else if (this.Unit == TimeUnit.Seconds)
                {
                    return this.duration.TotalSeconds;
                }
                else if (this.Unit == TimeUnit.Minutes)
                {
                    return this.duration.TotalMinutes;
                }
                else if (this.Unit == TimeUnit.Hours)
                {
                    return this.duration.TotalHours;
                }
                else if (this.Unit == TimeUnit.Days)
                {
                    return this.duration.TotalDays;
                }
                else if (this.Unit == TimeUnit.Weeks)
                {
                    return this.duration.TotalDays / 7;
                }
                else
                {
                    return this.duration.Ticks * 100;
                }
            }
        }

        /// <summary>
        /// Gets the unit in which the duration is express.
        /// </summary>
        /// <value>
        /// The unit used for the duration.
        /// </value>

        public TimeUnit Unit { get; }

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
        /// Creates a duration from a timespan by discovering the most appropriate <see cref="TimeUnit"/>.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>The created <see cref="Duration"/> instance</returns>
        public static Duration FromTimeSpan(TimeSpan timeSpan)
        {
            return new Duration(timeSpan, TimeHelper.DiscoverUnit(timeSpan));
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
            return a.duration < b.duration;
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
            return a.duration >= b.duration;            
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
            return a.duration <= b.duration;            
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
            return a.duration > b.duration;            
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
            return this.duration == other.duration;
        }

        /// <summary>
        /// Checks if the duration is equal to another object.
        /// </summary>
        /// <param name="obj">The duration to compare to.</param>
        /// <returns>True if both Durations represents the same duration.</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
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
            return this.duration.GetHashCode() + this.Unit.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.RawDuration} {this.Unit}";
        }

        /// <summary>
        /// Gets a new instance for the same duration expressed in another <see cref="NFluent.TimeUnit"/>
        /// </summary>
        /// <param name="newTmeUnit">The target time unit.</param>
        /// <returns>The new <see cref="Duration"/>.</returns>
        public Duration ConvertTo(TimeUnit newTmeUnit)
        {
            var newDuration = TimeHelper.GetInNanoSeconds(this.RawDuration, this.Unit);
            return new Duration(TimeHelper.GetFromNanoSeconds(newDuration, newTmeUnit), newTmeUnit);
        }

        /// <summary>
        /// Gets the duration as a number of milliseconds
        /// </summary>
        /// <returns></returns>
        public long ToMilliseconds()
        {
            return ConvertToMilliseconds(this.RawDuration, this.Unit);
        }
    }
}