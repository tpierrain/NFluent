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
        NanoSeconds,

        /// <summary>
        /// The Microseconds.
        /// </summary>
        MicroSeconds,

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
        /// <summary>
        /// Converts a duration in nanoseconds.
        /// </summary>
        /// <param name="value">
        /// Number of time units.
        /// </param>
        /// <param name="unit">
        /// Time unit in which value is expressed.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Raised if time unit is not recognized.
        /// </exception>
        /// <returns>
        /// The number of nanoseconds.
        /// </returns>
        public static double GetInNanoSeconds(double value, TimeUnit unit)
        {
            if (unit == TimeUnit.NanoSeconds)
            {
                return value;
            }

            value = value * 1000;
            if (unit == TimeUnit.MicroSeconds)
            {
                return value;
            }

            value = value * 1000;
            if (unit == TimeUnit.Milliseconds)
            {
                return value;
            }

            value = value * 1000;
            if (unit == TimeUnit.Seconds)
            {
                return value;
            }

            value = value * 60;
            if (unit == TimeUnit.Minutes)
            {
                return value;
            }

            value = value * 60;
            if (unit == TimeUnit.Hours)
            {
                return value;
            }

            value = value * 24;
            if (unit == TimeUnit.Days)
            {
                return value;
            }

            value = value * 7;
            if (unit == TimeUnit.Weeks)
            {
                return value;
            }

            throw new InvalidOperationException(string.Format("{0} is not a supported time unit.", unit));
        }
         
    }
}