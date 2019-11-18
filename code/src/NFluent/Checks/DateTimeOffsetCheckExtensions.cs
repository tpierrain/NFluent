// --------------------------------  ------------------------------------------------------------------------------------
// <copyright file="DateTimeCheckExtensions.cs" company="">
//   Copyright 2013 Marc-Antoine LATOUR, Thomas PIERRAIN
//   2017 Cyrille DUPUYDAUBY
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
    using Helpers;
    using Kernel;
    using Messages;

    /// <summary>
    /// Provides check methods to be executed on a DateTimeOffset instance. 
    /// </summary>
    public static class DateTimeOffsetCheckExtensions
    {
        /// <summary>
        /// Checks that when the actual and given DateTimeOffset have same offset, year, month, day, hour, minute and second fields,
        /// (millisecond fields are ignored in comparison).
        /// Code example :
        /// <code>
        ///     // successful checks
        ///     DateTimeOffset dateTimeOffset1 = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffset2 = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 456), TimeSpan.Zero);
        ///     Check.That(dateTimeOffset1).IsEqualToIgnoringMillis(dateTimeOffset2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTimeOffset dateTimeOffsetA = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffsetB = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 0, 999), TimeSpan.Zero);
        ///     Check.That(dateTimeOffsetA).IsEqualToIgnoringMillis(dateTimeOffsetB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTimeOffset.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time offset is not equal to the given one with the milliseconds ignored.</exception>
        /// <remarks>
        /// Check can fail with dateTimes in same chronological millisecond time window, e.g :
        /// 2000-01-01T00:00:<b>01.000</b> and 2000-01-01T00:00:<b>00.999</b>.
        /// check fails as second fields differ even if time difference is only 1 millis.
        /// </remarks>
        public static ICheckLink<ICheck<DateTimeOffset>> IsEqualToIgnoringMillis(this ICheck<DateTimeOffset> check, DateTimeOffset other)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut.Offset != other.Offset,
                    "The {0} is not equal to the {1} (ignoring milliseconds). The offsets are different.")
                .FailWhen(sut => sut.DateTime.Round(TimeUnit.Days) != other.DateTime.Round(TimeUnit.Days),
                    "The {0} is not equal to the {1} (ignoring milliseconds). The dates are different.")
                .FailWhen(sut => sut.DateTime.Round(TimeUnit.Hours) != other.DateTime.Round(TimeUnit.Hours),
                    "The {0} is not equal to the {1} (ignoring milliseconds). The time of day is different (Hours).")
                .FailWhen(sut => sut.DateTime.Round(TimeUnit.Minutes) != other.DateTime.Round(TimeUnit.Minutes),
                    "The {0} is not equal to the {1} (ignoring milliseconds). The time of day is different (Minutes).")
                .FailWhen(sut => sut.DateTime.Round(TimeUnit.Seconds) != other.DateTime.Round(TimeUnit.Seconds),
                    "The {0} is not equal to the {1} (ignoring milliseconds). The time of day is different (Seconds).")
                .ComparingTo(other, "same second", "different second")
                .OnNegate("The {0} is equal to the {1} (ignoring milliseconds) whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that when the actual and given DateTimeOffset have same offset, year, month, day, hour and minute fields,
        /// (Seconds and millisecond fields are ignored in comparison).
        /// <code>
        /// check can fail with dateTimeOffsets in same chronological second time window, e.g :
        /// 2000-01-01T00:<b>01:00</b>.000 and 2000-01-01T00:<b>00:59</b>.000.
        /// check fails as minute fields differ even if time difference is only 1s.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successful checks
        ///     DateTimeOffset dateTimeOffset1 = new DateTimeOffset(new DateTime(2000, 1, 1, 23, 50, 0, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffset2 = new DateTimeOffset(new DateTime(2000, 1, 1, 23, 50, 10, 456), TimeSpan.Zero);
        ///     Check.That(dateTimeOffset1).IsEqualToIgnoringSeconds(dateTimeOffset2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTimeOffset dateTimeOffsetA = new DateTimeOffset(new DateTime(2000, 1, 1, 23, 50, 00, 000), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffsetB = new DateTimeOffset(new DateTime(2000, 1, 1, 23, 49, 59, 999), TimeSpan.Zero);
        ///     Check.That(dateTimeOffsetA).IsEqualToIgnoringSeconds(dateTimeOffsetB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTimeOffSet.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time offset is not equal to the given one with second and millisecond fields ignored.</exception>
        public static ICheckLink<ICheck<DateTimeOffset>> IsEqualToIgnoringSeconds(this ICheck<DateTimeOffset> check, DateTimeOffset other)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut.Offset != other.Offset,
                    "The {0} is not equal to the {1} (ignoring seconds). The offsets are different.")
                .FailWhen(sut => sut.DateTime.Round(TimeUnit.Days) != other.DateTime.Round(TimeUnit.Days),
                    "The {0} is not equal to the {1} (ignoring seconds). The dates are different.")
                .FailWhen(sut => sut.DateTime.Round(TimeUnit.Hours) != other.DateTime.Round(TimeUnit.Hours),
                    "The {0} is not equal to the {1} (ignoring seconds). The time of day is different (Hours).")
                .FailWhen(sut => sut.DateTime.Round(TimeUnit.Minutes) != other.DateTime.Round(TimeUnit.Minutes),
                    "The {0} is not equal to the {1} (ignoring seconds). The time of day is different (Minutes).")
                .ComparingTo(other, "same time up to the same minute", "different time up to the same minute")
                .OnNegate("The {0} is equal to the {1} (ignoring seconds) whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that when the actual and given DateTimeOffset have same offset, year, month, day and hour fields
        /// (Minutes, seconds and millisecond fields are ignored in comparison).
        /// * <code>
        /// check can fail with dateTimeOffsets in same chronological minute time window, e.g :
        /// 2000-01-<b>01T23:59</b>:00.000 and 2000-01-02T<b>00:00</b>:00.000.
        /// Time difference is only 1min but day fields differ.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successful checks
        ///     DateTimeOffset dateTimeOffset1 = new DateTimeOffset(new DateTime(2000, 1, 1, 23, 50, 0, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffset2 = new DateTimeOffset(new DateTime(2000, 1, 1, 23, 00, 2, 7), TimeSpan.Zero);
        ///     Check.That(dateTimeOffset1).IsEqualToIgnoringMinutes(dateTimeOffset2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTimeOffset dateTimeOffsetA = new DateTimeOffset(new DateTime(2000, 1, 1, 01, 00, 00, 000), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffsetB = new DateTimeOffset(new DateTime(2000, 1, 1, 00, 59, 59, 999), TimeSpan.Zero);
        ///     Check.That(dateTimeOffsetA).IsEqualToIgnoringMinutes(dateTimeOffsetB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTimeOffSet.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time offset is not equal to the given one with minute, second and millisecond fields ignored.</exception>
        public static ICheckLink<ICheck<DateTimeOffset>> IsEqualToIgnoringMinutes(this ICheck<DateTimeOffset> check, DateTimeOffset other)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut.Offset != other.Offset,
                    "The {0} is not equal to the {1} (ignoring hours). The offsets are different.")
                .FailWhen(sut => sut.DateTime.Round(TimeUnit.Days) != other.DateTime.Round(TimeUnit.Days),
                    "The {0} is not equal to the {1}. The dates are different.")
                .FailWhen(sut => sut.DateTime.Round(TimeUnit.Hours) != other.DateTime.Round(TimeUnit.Hours),
                    "The {0} is not equal to the {1}. The hours are different.")
                .ComparingTo(other, "same hour", "different hour")
                .OnNegate("The {0} is equal to the {1} (ignoring minutes) whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that when the actual and given DateTimeOffset have same offset, year, month and day fields
        /// * (Hours, minutes, seconds and millisecond fields are ignored in comparison).
        /// * <code>
        /// check can fail with dateTimeOffsets in same chronological minute time window, e.g :
        /// 2000-01-<b>01T23:59</b>:00.000 and 2000-01-02T<b>00:00</b>:00.000.
        /// Time difference is only 1min but day fields differ.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successful checks
        ///     DateTimeOffset dateTimeOffset1 = new DateTimeOffset(new DateTime(2000, 1, 1, 23, 59, 59, 999), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffset2 = new DateTimeOffset(new DateTime(2000, 1, 1, 00, 00, 00, 000), TimeSpan.Zero);
        ///     Check.That(dateTimeOffset1).IsEqualToIgnoringHours(dateTimeOffset2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTimeOffset dateTimeOffsetA = new DateTimeOffset(new DateTime(2000, 1, 2, 00, 00, 00, 000), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffsetB = new DateTimeOffset(new DateTime(2000, 1, 1, 23, 59, 59, 999), TimeSpan.Zero);
        ///     Check.That(dateTimeOffsetA).IsEqualToIgnoringHours(dateTimeOffsetB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTimeOffset.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time offset is not equal to the given one with hour, minute, second and millisecond fields ignored.</exception>
        public static ICheckLink<ICheck<DateTimeOffset>> IsEqualToIgnoringHours(this ICheck<DateTimeOffset> check, DateTimeOffset other)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut.Offset != other.Offset,
                    "The {0} is not equal to the {1} (ignoring hours). The offsets are different.")
                .FailWhen(sut => sut.DateTime.Round(TimeUnit.Days) != other.DateTime.Round(TimeUnit.Days),
                    "The {0} is not equal to the {1} (ignoring hours). The dates are different.")
                .ComparingTo(other, "same day", "different day")
                .OnNegate("The {0} is equal to the {1} (ignoring hours) whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that when the offset is taken into account the actual and given DateTimeOffset fields have the same Utc time values
        /// Code example :
        /// <code>
        ///     // successful checks
        ///     DateTimeOffset dateTimeOffset1 = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffset2 = new DateTimeOffset(new DateTime(2000, 1, 1, 1, 0, 1, 0), TimeSpan.FromHours(1));
        ///     Check.That(dateTimeOffset1).MatchTheSameUtcInstant(dateTimeOffset2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTimeOffset dateTimeOffsetA = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffset2 = new DateTimeOffset(new DateTime(2000, 1, 1, 1, 0, 1, 999), TimeSpan.FromHours(1));
        ///     Check.That(dateTimeOffsetA).MatchTheSameUtcInstant(dateTimeOffsetB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTimeOffset.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time offset Utc time is not equal to the given one with the milliseconds ignored.</exception>
        /// <remarks>
        /// Check can fail with dateTimes in same chronological millisecond time window, e.g :
        /// 2000-01-01T00:00:<b>01.000</b> and 2000-01-01T00:00:<b>00.999</b>.
        /// check fails as second fields differ even if time difference is only 1 millis.
        /// </remarks>
        public static ICheckLink<ICheck<DateTimeOffset>> MatchTheSameUtcInstant(this ICheck<DateTimeOffset> check, DateTimeOffset other)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut.UtcDateTime != other.UtcDateTime, "The {0} is not equal to the {1}.")
                .ComparingTo(other, "same time", "different time")
                .OnNegate("The {0} is equal to the {1} whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that when the offset is taken into account the actual and given DateTimeOffset fields have the same UTC year, month, day, hour, minute and second fields,
        /// (millisecond fields are ignored in comparison).
        /// Code example :
        /// <code>
        ///     // successful checks
        ///     DateTimeOffset dateTimeOffset1 = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffset2 = new DateTimeOffset(new DateTime(2000, 1, 1, 1, 0, 1, 999), TimeSpan.FromHours(1));
        ///     Check.That(dateTimeOffset1).MatchTheSameUtcInstantIgnoringMillis(dateTimeOffset2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTimeOffset dateTimeOffsetA = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffsetB = new DateTimeOffset(new DateTime(2000, 1, 1, 1, 0, 0, 999), TimeSpan.FromHours(1));
        ///     Check.That(dateTimeOffsetA).MatchTheSameUtcInstantIgnoringMillis(dateTimeOffsetB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTimeOffset.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time offset UTC time is not equal to the given one with the milliseconds ignored.</exception>
        /// <remarks>
        /// Check can fail with dateTimes in same chronological millisecond time window, e.g :
        /// 2000-01-01T00:00:<b>01.000</b> and 2000-01-01T00:00:<b>00.999</b>.
        /// check fails as second fields differ even if time difference is only 1 millis.
        /// </remarks>
        public static ICheckLink<ICheck<DateTimeOffset>> MatchTheSameUtcInstantIgnoringMillis(this ICheck<DateTimeOffset> check, DateTimeOffset other)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut.UtcDateTime.Round(TimeUnit.Seconds) != other.UtcDateTime.Round(TimeUnit.Seconds), "The {0} is not equal to the {1} (ignoring seconds).")
                .ComparingTo(other, "same second", "different second")
                .OnNegate("The {0} is equal to the {1} (ignoring milliseconds) whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that when the offset is taken into account the actual and given DateTimeOffset fields have the same UTC year, month, day, hour and minute fields,
        /// (Seconds and millisecond fields are ignored in comparison).
        /// <code>
        /// check can fail with dateTimeOffsets in same chronological second time window, e.g :
        /// 2000-01-01T00:<b>01:00</b>.000 and 2000-01-01T00:<b>00:59</b>.000.
        /// check fails as minute fields differ even if time difference is only 1s.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successful checks
        ///     DateTimeOffset dateTimeOffset1 = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffset2 = new DateTimeOffset(new DateTime(2000, 1, 1, 1, 0, 9, 999), TimeSpan.FromHours(1));
        ///     Check.That(dateTimeOffset1).MatchTheSameUtcInstantIgnoringSeconds(dateTimeOffset2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTimeOffset dateTimeOffsetA = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffsetB = new DateTimeOffset(new DateTime(2000, 1, 1, 1, 1, 1, 999), TimeSpan.FromHours(1));
        ///     Check.That(dateTimeOffsetA).MatchTheSameUtcInstantIgnoringSeconds(dateTimeOffsetB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTimeOffSet.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time offset UTC time is not equal to the given one with second and millisecond fields ignored.</exception>
        public static ICheckLink<ICheck<DateTimeOffset>> MatchTheSameUtcInstantIgnoringSeconds(this ICheck<DateTimeOffset> check, DateTimeOffset other)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut.UtcDateTime.Round(TimeUnit.Minutes) != other.UtcDateTime.Round(TimeUnit.Minutes), "The {0} is not equal to the {1} (ignoring seconds).")
                .ComparingTo(other, "same minute", "different minute")
                .OnNegate("The {0} is equal to the {1} (ignoring seconds) whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that when the offset is taken into account the actual and given DateTimeOffset fields have the same UTC year, month, day and hour fields,
        /// (Minutes, seconds and millisecond fields are ignored in comparison).
        /// * <code>
        /// check can fail with dateTimeOffsets in same chronological minute time window, e.g :
        /// 2000-01-<b>01T23:59</b>:00.000 and 2000-01-02T<b>00:00</b>:00.000.
        /// Time difference is only 1min but day fields differ.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successful checks
        ///     DateTimeOffset dateTimeOffset1 = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffset2 = new DateTimeOffset(new DateTime(2000, 1, 1, 1, 9, 9, 999), TimeSpan.FromHours(1));
        ///     Check.That(dateTimeOffset1).MatchTheSameUtcInstantIgnoringMinutes(dateTimeOffset2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTimeOffset dateTimeOffsetA = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffsetB = new DateTimeOffset(new DateTime(2000, 1, 1, 9, 9, 9, 999), TimeSpan.FromHours(1));
        ///     Check.That(dateTimeOffsetA).MatchTheSameUtcInstantIgnoringMinutes(dateTimeOffsetB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTimeOffSet.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time offset UTC time is not equal to the given one with minute, second and millisecond fields ignored.</exception>
        public static ICheckLink<ICheck<DateTimeOffset>> MatchTheSameUtcInstantIgnoringMinutes(this ICheck<DateTimeOffset> check, DateTimeOffset other)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut.UtcDateTime.Round(TimeUnit.Hours) != other.UtcDateTime.Round(TimeUnit.Hours), "The {0} is not equal to the {1} (ignoring minutes).")
                .ComparingTo(other, "same hour", "different hour")
                .OnNegate("The {0} is equal to the {1} (ignoring minutes) whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that when the offset is taken into account the actual and given DateTimeOffset fields have the same UTC year, month and day fields,
        /// * (Hours, minutes, seconds and millisecond fields are ignored in comparison).
        /// * <code>
        /// check can fail with dateTimeOffsets in same chronological minute time window, e.g :
        /// 2000-01-<b>01T23:59</b>:00.000 and 2000-01-02T<b>00:00</b>:00.000.
        /// Time difference is only 1min but day fields differ.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successful checks
        ///     DateTimeOffset dateTimeOffset1 = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffset2 = new DateTimeOffset(new DateTime(2000, 1, 1, 1, 9, 9, 999), TimeSpan.FromHours(1));
        ///     Check.That(dateTimeOffset1).MatchTheSameUtcInstantIgnoringHours(dateTimeOffset2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTimeOffset dateTimeOffsetA = new DateTimeOffset(new DateTime(2000, 1, 1, 0, 0, 1, 0), TimeSpan.Zero);
        ///     DateTimeOffset dateTimeOffsetB = new DateTimeOffset(new DateTime(2000, 1, 9, 9, 9, 9, 999), TimeSpan.FromHours(1));
        ///     Check.That(dateTimeOffsetA).MatchTheSameUtcInstantIgnoringHours(dateTimeOffsetB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTimeOffset.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time offset UTC time is not equal to the given one with hour, minute, second and millisecond fields ignored.</exception>
        public static ICheckLink<ICheck<DateTimeOffset>> MatchTheSameUtcInstantIgnoringHours(this ICheck<DateTimeOffset> check, DateTimeOffset other)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut.UtcDateTime.Round(TimeUnit.Days) != other.UtcDateTime.Round(TimeUnit.Days), "The {0} is not equal to the {1} (ignoring hours).")
                .ComparingTo(other, "same day", "different day")
                .OnNegate("The {0} is equal to the {1} (ignoring hours) whereas it must not.", MessageOption.NoCheckedBlock)
                .EndCheck();
            
            return ExtensibilityHelper.BuildCheckLink(check);
        }
        
        /// <summary>
        /// Determines whether the actual DateTimeOffset is close to an expected value within a given within.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="within">The within.</param>
        /// <returns>A continuation check.</returns>
        public static ICheckLink<ICheck<DateTimeOffset>> IsCloseTo(this ICheck<DateTimeOffset> check, DateTimeOffset expected, Duration within)
        {
            var range = new DateTimeOffsetRangeBlock(expected, within);
            
            ExtensibilityHelper.BeginCheck(check).
                                FailWhen((sut) => !range.IsInRange(sut), "The {0} is outside the expected value range.").
                                DefineExpectedValue(range).
                                OnNegate("The {0} is within the expected range, whereas it must not.").
                                EndCheck();
            
            return ExtensibilityHelper.BuildCheckLink(check);
        }
        
        /// <summary>
        /// Determines whether the actual DateTimeOffset is close to an expected value within a given within.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="within">The within.</param>
        /// <returns>A continuation check.</returns>
        public static ICheckLink<ICheck<DateTimeOffset>> IsCloseTo(this ICheck<DateTimeOffset> check, DateTimeOffset expected, TimeSpan within)
        {
            return check.IsCloseTo(expected, Duration.FromTimeSpan(within));
        }
    }
}