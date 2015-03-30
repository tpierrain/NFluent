// // --------------------------------  ------------------------------------------------------------------------------------
// // <copyright file="DateTimeCheckExtensions.cs" company="">
// //   Copyright 2013 Marc-Antoine LATOUR, Thomas PIERRAIN
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
namespace NFluent
{
    using System;

    using NFluent.Extensibility;
    using NFluent.Extensions;

    /// <summary>
    /// Provides check methods to be executed on a date time instance. 
    /// </summary>
    public static class DateTimeCheckExtensions
    {
        /// <summary>
        /// Checks that the actual DateTime is strictly before the given one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time is not before the given one.</exception>
        public static ICheckLink<ICheck<DateTime>> IsBefore(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value >= other)
                        {
                            var message = checker.BuildMessage("The {0} is not before the {1}.").WithGivenValue(other).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} is before the {1} whereas it must not.").WithGivenValue(other).ToString());
        }

        /// <summary>
        /// Checks that the actual DateTime is before or equals to the given one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time is not before or equals to the given one.</exception>
        public static ICheckLink<ICheck<DateTime>> IsBeforeOrEqualTo(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                {
                    if (checker.Value > other)
                    {
                        var message = checker.BuildMessage("The {0} is not before or equals to the {1}.").WithGivenValue(other).ToString();
                        throw new FluentCheckException(message);
                    }
                },
                checker.BuildMessage("The {0} is before or equals to the {1} whereas it must not.").WithGivenValue(other).ToString());
        }

        /// <summary>
        /// Checks that the actual DateTime is strictly after the given one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time is not after the given one.</exception>
        public static ICheckLink<ICheck<DateTime>> IsAfter(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value <= other)
                        {
                            var message = checker.BuildMessage("The {0} is not after the {1}.").WithGivenValue(other).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} is after the {1} whereas it must not.").WithGivenValue(other).ToString());
        }

        /// <summary>
        /// Checks that the actual DateTime is after or equals to the given one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time is not after or equals to the given one.</exception>
        public static ICheckLink<ICheck<DateTime>> IsAfterOrEqualTo(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value < other)
                        {
                            var message = checker.BuildMessage("The {0} is not after or equals to the {1}.").WithGivenValue(other).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} is after or equals to the {1} whereas it must not.").WithGivenValue(other).ToString());
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day, hour, minute and second fields,
        /// (millisecond fields are ignored in comparison).
        /// Code example :
        /// <code>
        ///     // successfull checks
        ///     DateTime dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, 0);
        ///     DateTime dateTime2 = new DateTime(2000, 1, 1, 0, 0, 1, 456);
        ///     Check.That(dateTime1).IsEqualToIgnoringMillis(dateTime2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTime dateTimeA = new DateTime(2000, 1, 1, 0, 0, 1, 0);
        ///     DateTime dateTimeB = new DateTime(2000, 1, 1, 0, 0, 0, 999);
        ///     Check.That(dateTimeA).IsEqualToIgnoringMillis(dateTimeB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time is not equal to the given one with the milliseconds ignored.</exception>
        /// <remarks>
        /// Check can fail with dateTimes in same chronological millisecond time window, e.g :
        /// 2000-01-01T00:00:<b>01.000</b> and 2000-01-01T00:00:<b>00.999</b>.
        /// check fails as second fields differ even if time difference is only 1 millis.
        /// </remarks>
        public static ICheckLink<ICheck<DateTime>> IsEqualToIgnoringMillis(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.Year != other.Year || checker.Value.Month != other.Month || checker.Value.Day != other.Day || checker.Value.Hour != other.Hour || checker.Value.Minute != other.Minute || checker.Value.Second != other.Second)
                        {
                            var message = checker.BuildMessage("The {0} is not equal to the {1} (ignoring milliseconds).").WithGivenValue(other).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} is equal to the {1} (ignoring milliseconds) whereas it must not.").WithGivenValue(other).ToString());
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day, hour and minute fields,
        /// (Seconds and millisecond fields are ignored in comparison).
        /// <code>
        /// check can fail with DateTimes in same chronological second time window, e.g :
        /// 2000-01-01T00:<b>01:00</b>.000 and 2000-01-01T00:<b>00:59</b>.000.
        /// check fails as minute fields differ even if time difference is only 1s.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successfull checks
        ///     DateTime dateTime1 = new DateTime(2000, 1, 1, 23, 50, 0, 0);
        ///     DateTime dateTime2 = new DateTime(2000, 1, 1, 23, 50, 10, 456);
        ///     Check.That(dateTime1).IsEqualToIgnoringSeconds(dateTime2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTime dateTimeA = new DateTime(2000, 1, 1, 23, 50, 00, 000);
        ///     DateTime dateTimeB = new DateTime(2000, 1, 1, 23, 49, 59, 999);
        ///     Check.That(dateTimeA).IsEqualToIgnoringSeconds(dateTimeB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time is not equal to the given one with second and millisecond fields ignored.</exception>
        public static ICheckLink<ICheck<DateTime>> IsEqualToIgnoringSeconds(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.Year != other.Year || checker.Value.Month != other.Month || checker.Value.Day != other.Day || checker.Value.Hour != other.Hour || checker.Value.Minute != other.Minute)
                        {
                            var message = checker.BuildMessage("The {0} is not equal to the {1} (ignoring seconds).").WithGivenValue(other).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} is equal to the {1} (ignoring seconds) whereas it must not.").WithGivenValue(other).ToString());
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day and hour fields,
        /// (Minutes, seconds and millisecond fields are ignored in comparison).
        /// <code>
        /// check can fail with dateTimes in same chronological second time window, e.g :
        /// 2000-01-01T<b>01:00</b>:00.000 and 2000-01-01T<b>00:59:59</b>.000.
        /// Time difference is only 1s but hour fields differ.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successfull checks
        ///     DateTime dateTime1 = new DateTime(2000, 1, 1, 23, 50, 0, 0);
        ///     DateTime dateTime2 = new DateTime(2000, 1, 1, 23, 00, 2, 7);
        ///     Check.That(dateTime1).IsEqualToIgnoringMinutes(dateTime2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTime dateTimeA = new DateTime(2000, 1, 1, 01, 00, 00, 000);
        ///     DateTime dateTimeB = new DateTime(2000, 1, 1, 00, 59, 59, 999);
        ///     Check.That(dateTimeA).IsEqualToIgnoringMinutes(dateTimeB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time is not equal to the given one with minute, second and millisecond fields ignored.</exception>
        public static ICheckLink<ICheck<DateTime>> IsEqualToIgnoringMinutes(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.Year != other.Year || checker.Value.Month != other.Month || checker.Value.Day != other.Day || checker.Value.Hour != other.Hour)
                        {
                            var message = checker.BuildMessage("The {0} is not equal to the {1} (ignoring minutes).").WithGivenValue(other).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} is equal to the {1} (ignoring minutes) whereas it must not.").WithGivenValue(other).ToString());
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month and day fields,
        /// * (Hours, minutes, seconds and millisecond fields are ignored in comparison).
        /// * <code>
        /// check can fail with dateTimes in same chronological minute time window, e.g :
        /// 2000-01-<b>01T23:59</b>:00.000 and 2000-01-02T<b>00:00</b>:00.000.
        /// Time difference is only 1min but day fields differ.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successfull checks
        ///     DateTime dateTime1 = new DateTime(2000, 1, 1, 23, 59, 59, 999);
        ///     DateTime dateTime2 = new DateTime(2000, 1, 1, 00, 00, 00, 000);
        ///     CheckThat(dateTime1).IsEqualToIgnoringHours(dateTime2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTime dateTimeA = new DateTime(2000, 1, 2, 00, 00, 00, 000);
        ///     DateTime dateTimeB = new DateTime(2000, 1, 1, 23, 59, 59, 999);
        ///     CheckThat(dateTimeA).IsEqualToIgnoringHours(dateTimeB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time is not equal to the given one with hour, minute, second and millisecond fields ignored.</exception>
        public static ICheckLink<ICheck<DateTime>> IsEqualToIgnoringHours(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.Year != other.Year || checker.Value.Month != other.Month || checker.Value.Day != other.Day)
                        {
                            var message = checker.BuildMessage("The {0} is not equal to the {1} (ignoring hours).").WithGivenValue(other).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} is equal to the {1} (ignoring hours) whereas it must not.").WithGivenValue(other).ToString());
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time year is not equal to the given year.</exception>
        public static ICheckLink<ICheck<DateTime>> IsInSameYearAs(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.Year != other.Year)
                        {
                            var message = checker.BuildMessage(string.Format("The {{0}} does not have the same year as the {{1}}.\nYear of the checked date time:\n\t[{0}]\nYear of the given date time:\n\t[{1}]", checker.Value.Year.ToStringProperlyFormated(), other.Year.ToStringProperlyFormated())).WithGivenValue(other).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage(string.Format("The {{0}} has the same year as the {{1}} whereas it must not.\nYear of the checked date time:\n\t[{0}]\nYear of the given date time:\n\t[{1}]", checker.Value.Year.ToStringProperlyFormated(), other.Year.ToStringProperlyFormated())).WithGivenValue(other).ToString());
        }

        /// <summary>
        /// Checks that actual and given DateTime have same month, whatever the year.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time month is not equal to the given month, whatever the year.</exception>
        public static ICheckLink<ICheck<DateTime>> IsInSameMonthAs(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.Month != other.Month)
                        {
                            var message = checker.BuildMessage(string.Format("The {{0}} does not have the same month as the {{1}}.\nMonth of the checked date time:\n\t[{0}]\nMonth of the given date time:\n\t[{1}]", checker.Value.Month.ToStringProperlyFormated(), other.Month.ToStringProperlyFormated())).WithGivenValue(other).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage(string.Format("The {{0}} has the same month as the {{1}} whereas it must not.\nMonth of the checked date time:\n\t[{0}]\nMonth of the given date time:\n\t[{1}]", checker.Value.Month.ToStringProperlyFormated(), other.Month.ToStringProperlyFormated())).WithGivenValue(other).ToString());
        }

        /// <summary>
        /// Checks that actual and given DateTime have same day, whatever the year or the month.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked date time day is not equal to the given day, whatever the year or the month.</exception>
        public static ICheckLink<ICheck<DateTime>> IsInSameDayAs(this ICheck<DateTime> check, DateTime other)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.Day != other.Day)
                        {
                            var message = checker.BuildMessage(string.Format("The {{0}} does not have the same day as the {{1}}.\nDay of the checked date time:\n\t[{0}]\nDay of the given date time:\n\t[{1}]", checker.Value.Day.ToStringProperlyFormated(), other.Day.ToStringProperlyFormated())).WithGivenValue(other).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage(string.Format("The {{0}} has the same day as the {{1}} whereas it must not.\nDay of the checked date time:\n\t[{0}]\nDay of the given date time:\n\t[{1}]", checker.Value.Day.ToStringProperlyFormated(), other.Day.ToStringProperlyFormated())).WithGivenValue(other).ToString());
        }
    }
}