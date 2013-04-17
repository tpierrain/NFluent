// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="TimeHelper.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
namespace NFluent.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Enumerate the available time unit.
    /// </summary>
    public enum TimeUnit : long
    {
        /// <summary>
        /// The nanoseconds.
        /// </summary>
        Nanoseconds,

        /// <summary>
        /// The Microseconds.
        /// </summary>
        Microseconds,

        /// <summary>
        /// The Milliseconds.
        /// </summary>
        Milliseconds,

        /// <summary>
        /// The seconds.
        /// </summary>
        Seconds,

        /// <summary>
        /// The minutes.
        /// </summary>
        Minutes,

        /// <summary>
        /// The hours.
        /// </summary>
        Hours,

        /// <summary>
        /// The days.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1632:DocumentationTextMustMeetMinimumCharacterLength", Justification = "Reviewed. Suppression is OK here.")]
        Days,

        /// <summary>
        /// The weeks.
        /// </summary>
        Weeks
    }

    /// <summary>
    /// Static class hosting various time helper.
    /// </summary>
    public static class TimeHelper
    {
        private const int NanoSecondsInOneMicroSecond = 1000;

        private const int MicroSecondsInOneMillisecond = 1000;

        private const int MillisecondInOneSecond = 1000;

        private const int SecondsInOneMinute = 60;

        private const int MinutesInOneHour = 60;

        private const int HoursInOneDay = 24;

        private const int DaysInWeek = 7;

        private const int NanoSecondsPerTick = 100;

        private const int MinimumUnitAmount = 2;

        /// <summary>
        /// Converts a duration in nanoseconds.
        /// </summary>
        /// <param name="value">Number of time units.</param>
        /// <param name="unit">Time unit in which duration is expressed.</param>
        /// <returns>
        /// The number of nanoseconds.
        /// </returns>
        /// <exception cref="InvalidOperationException">Raised if time unit is not recognized.</exception>
        public static double GetInNanoSeconds(double value, TimeUnit unit)
        {
            return value * GetConversionFactor(unit);
        }

        /// <summary>
        /// Creates a <see cref="TimeSpan"/> representing the duration expressed in <see cref="TimeUnit"/>.
        /// </summary>
        /// <param name="value">
        /// Duration duration.
        /// </param>
        /// <param name="timeUnit">
        /// Duration unit.
        /// </param>
        /// <returns>
        /// A <see cref="TimeSpan"/> instance of that duration.
        /// </returns>
        /// <exception cref="InvalidOperationException">Raised if time unit is not recognized.</exception>
        public static TimeSpan ToTimeSpan(double value, TimeUnit timeUnit)
        {
            return TimeSpan.FromTicks((long)(GetInNanoSeconds(value, timeUnit) / NanoSecondsPerTick));
        }

        /// <summary>
        /// Expresses a <see cref="TimeSpan"/> duration in the desired <see cref="TimeUnit"/>.
        /// </summary>
        /// <param name="value">Duration to convert.</param>
        /// <param name="unit"><see cref="TimeUnit"/> to convert to.</param>
        /// <returns>The duration in <see cref="TimeUnit"/>.</returns>
        /// <exception cref="InvalidOperationException">Raised if time unit is not recognized.</exception>
        public static double Convert(TimeSpan value, TimeUnit unit)
        {
            return ((double)value.Ticks) * NanoSecondsPerTick / GetConversionFactor(unit);
        }

        /// <summary>
        /// Discover the most appropriate <see cref="TimeUnit"/> to express the given duration.
        /// </summary>
        /// <param name="timeSpan">
        /// Duration to analyze.
        /// </param>
        /// <returns>
        /// The most appropriate <see cref="TimeUnit"/>.
        /// </returns>
        public static TimeUnit DiscoverUnit(TimeSpan timeSpan)
        {
            double nanoseconds = Convert(timeSpan, TimeUnit.Nanoseconds);

            // if at least two weeks
            if (nanoseconds >= GetConversionFactor(TimeUnit.Weeks) * MinimumUnitAmount)
            {
                return TimeUnit.Weeks;
            }

            // if at least two days
            if (nanoseconds >= GetConversionFactor(TimeUnit.Days) * MinimumUnitAmount)
            {
                return TimeUnit.Days;
            }

            // if at least two hours
            if (nanoseconds >= GetConversionFactor(TimeUnit.Hours) * MinimumUnitAmount)
            {
                return TimeUnit.Hours;
            }

            // if at least two hours
            if (nanoseconds >= GetConversionFactor(TimeUnit.Minutes) * MinimumUnitAmount)
            {
                return TimeUnit.Minutes;
            }

            // if at least two hours
            if (nanoseconds >= GetConversionFactor(TimeUnit.Seconds) * MinimumUnitAmount)
            {
                return TimeUnit.Seconds;
            }

            // if at least two hours
            if (nanoseconds >= GetConversionFactor(TimeUnit.Milliseconds) * MinimumUnitAmount)
            {
                return TimeUnit.Milliseconds;
            }

            // if at least two hours
            if (nanoseconds >= GetConversionFactor(TimeUnit.Microseconds) * MinimumUnitAmount)
            {
                return TimeUnit.Microseconds;
            }

            return TimeUnit.Nanoseconds;
        }

        private static long GetConversionFactor(TimeUnit unit)
        {
            long value = 1;
            if (unit == TimeUnit.Nanoseconds)
            {
                return value;
            }

            value = value * NanoSecondsInOneMicroSecond;
            if (unit == TimeUnit.Microseconds)
            {
                return value;
            }

            value = value * MicroSecondsInOneMillisecond;
            if (unit == TimeUnit.Milliseconds)
            {
                return value;
            }

            value = value * MillisecondInOneSecond;
            if (unit == TimeUnit.Seconds)
            {
                return value;
            }

            value = value * SecondsInOneMinute;
            if (unit == TimeUnit.Minutes)
            {
                return value;
            }

            value = value * MinutesInOneHour;
            if (unit == TimeUnit.Hours)
            {
                return value;
            }

            value = value * HoursInOneDay;
            if (unit == TimeUnit.Days)
            {
                return value;
            }

            value = value * DaysInWeek;
            if (unit == TimeUnit.Weeks)
            {
                return value;
            }

            throw new InvalidOperationException(string.Format("{0} is not a supported time unit.", unit));
        }
    }
}