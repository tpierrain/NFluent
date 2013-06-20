// // --------------------------------  ------------------------------------------------------------------------------------
// // <copyright file="DateTimeFluentAssertionExtensions.cs" company="">
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

    using NFluent.Extensions;
    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on a date time instance. 
    /// </summary>
    public static class DateTimeFluentAssertionExtensions
    {
        #region Equality checks

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsEqualTo(this ICheck<DateTime> check, object expected)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        EqualityHelper.IsEqualTo(runnableCheck.Value, expected);
                    },
                string.Format("\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[{0}]{1}.", runnableCheck.Value.ToStringProperlyFormated(), EqualityHelper.BuildTypeDescriptionMessage(expected)));
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is equal to the expected value.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsNotEqualTo(this ICheck<DateTime> check, object expected)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        EqualityHelper.IsNotEqualTo(runnableCheck.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expected, false));
        }

        #endregion

        #region IsInstance checks

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A chainable fluent check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual instance is not of the provided type.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsInstanceOf<T>(this ICheck<DateTime> check)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        IsInstanceHelper.IsInstanceOf(runnableCheck.Value, typeof(T));
                    },
                    IsInstanceHelper.BuildErrorMessage(runnableCheck, typeof(T), true));
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A chainable fluent check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual instance is of the provided type.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsNotInstanceOf<T>(this ICheck<DateTime> check)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            var expectedType = typeof(T);
            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        IsInstanceHelper.IsNotInstanceOf(runnableCheck.Value, expectedType);
                    },
                string.Format("\nThe actual value:\n\t[{0}]\nis not an instance of:\n\t[{1}]\nbut an instance of:\n\t[{2}]\ninstead.", runnableCheck.Value.ToStringProperlyFormated(), expectedType, runnableCheck.Value.GetType()));
        }

        #endregion

        /// <summary>
        /// Checks that the actual DateTime is strictly before the given one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time is not before the given one.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsBefore(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value >= other)
                        {
                            throw new FluentCheckException(string.Format("\nThe actual date time:\n\t[{0}]\nis not before the given one:\n\t[{1}].", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe actual date time:\n\t[{0}]\nis before the given one:\n\t[{1}].", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is before or equals to the given one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time is not before or equals to the given one.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsBeforeOrEqualTo(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value > other)
                        {
                            throw new FluentCheckException(string.Format("\nThe actual date time:\n\t[{0}]\nis not before or equals to the given one:\n\t[{1}].", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe actual date time:\n\t[{0}]\nis before or equals to the given one:\n\t[{1}].", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is strictly after the given one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time is not after the given one.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsAfter(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value <= other)
                        {
                            throw new FluentCheckException(string.Format("\nThe actual date time:\n\t[{0}]\nis not after the given one:\n\t[{1}].", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe actual date time:\n\t[{0}]\nis after the given one:\n\t[{1}].", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is after or equals to the given one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time is not after or equals to the given one.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsAfterOrEqualTo(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value < other)
                        {
                            throw new FluentCheckException(string.Format("\nThe actual date time:\n\t[{0}]\nis not after or equals to the given one:\n\t[{1}].", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe actual date time:\n\t[{0}]\nis after or equals to the given one:\n\t[{1}].", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        // TODO : replace the FEST assert assertThat samples by Nfluent checks

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day, hour, minute and second fields,
        /// (millisecond fields are ignored in comparison).
        /// Code example :
        /// <code>
        ///     // successfull checks
        ///     DateTime dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, 0);
        ///     DateTime dateTime2 = new DateTime(2000, 1, 1, 0, 0, 1, 456);
        ///     assertThat(dateTime1).isEqualToIgnoringMillis(dateTime2);
        ///     // failing checks (even if time difference is only 1ms)
        ///     DateTime dateTimeA = new DateTime(2000, 1, 1, 0, 0, 1, 0);
        ///     DateTime dateTimeB = new DateTime(2000, 1, 1, 0, 0, 0, 999);
        ///     assertThat(dateTimeA).isEqualToIgnoringMillis(dateTimeB);
        /// </code>
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time is not equal to the given one with the milliseconds ignored.</exception>
        /// <remarks>
        /// Check can fail with dateTimes in same chronological millisecond time window, e.g :
        /// 2000-01-01T00:00:<b>01.000</b> and 2000-01-01T00:00:<b>00.999</b>.
        /// check fails as second fields differ even if time difference is only 1 millis.
        /// </remarks>
        public static IChainableCheck<ICheck<DateTime>> IsEqualToIgnoringMillis(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value.Year != other.Year || runnableCheck.Value.Month != other.Month || runnableCheck.Value.Day != other.Day || runnableCheck.Value.Hour != other.Hour || runnableCheck.Value.Minute != other.Minute || runnableCheck.Value.Second != other.Second)
                        {
                            throw new FluentCheckException(string.Format("\nThe actual date time:\n\t[{0}]\nis not equal to the given date time:\n\t[{1}]\nignoring milliseconds.", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe actual date time:\n\t[{0}]\nis equal to the given date time:\n\t[{1}]\nignoring milliseconds.", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
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
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time is not equal to the given one with second and millisecond fields ignored.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsEqualToIgnoringSeconds(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value.Year != other.Year || runnableCheck.Value.Month != other.Month || runnableCheck.Value.Day != other.Day || runnableCheck.Value.Hour != other.Hour || runnableCheck.Value.Minute != other.Minute)
                        {
                            throw new FluentCheckException(string.Format("\nThe actual date time:\n\t[{0}]\nis not equal to the given date time:\n\t[{1}]\nignoring seconds.", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe actual date time:\n\t[{0}]\nis equal to the given date time:\n\t[{1}]\nignoring seconds.", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
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
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time is not equal to the given one with minute, second and millisecond fields ignored.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsEqualToIgnoringMinutes(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value.Year != other.Year || runnableCheck.Value.Month != other.Month || runnableCheck.Value.Day != other.Day || runnableCheck.Value.Hour != other.Hour)
                        {
                            throw new FluentCheckException(string.Format("\nThe actual date time:\n\t[{0}]\nis not equal to the given date time:\n\t[{1}]\nignoring minutes.", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe actual date time:\n\t[{0}]\nis equal to the given date time:\n\t[{1}]\nignoring minutes.", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
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
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time is not equal to the given one with hour, minute, second and millisecond fields ignored.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsEqualToIgnoringHours(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value.Year != other.Year || runnableCheck.Value.Month != other.Month || runnableCheck.Value.Day != other.Day)
                        {
                            throw new FluentCheckException(string.Format("\nThe actual date time:\n\t[{0}]\nis not equal to the given date time:\n\t[{1}]\nignoring hours.", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe actual date time:\n\t[{0}]\nis equal to the given date time:\n\t[{1}]\nignoring hours.", runnableCheck.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time year is not equal to the given year.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsInSameYearAs(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value.Year != other.Year)
                        {
                            throw new FluentCheckException(string.Format("\nThe year of the actual date time:\n\t[{0}]\nis not equal to the year of the given date time:\n\t[{1}].", runnableCheck.Value.Year.ToStringProperlyFormated(), other.Year.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe year of the actual date time:\n\t[{0}]\nis equal to the year of the given date time:\n\t[{1}].", runnableCheck.Value.Year.ToStringProperlyFormated(), other.Year.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same month, whatever the year.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time month is not equal to the given month, whatever the year.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsInSameMonthAs(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value.Month != other.Month)
                        {
                            throw new FluentCheckException(string.Format("\nThe month of the actual date time:\n\t[{0}]\nis not equal to the month of the given date time:\n\t[{1}].", runnableCheck.Value.Month.ToStringProperlyFormated(), other.Month.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe month of the actual date time:\n\t[{0}]\nis equal to the month of the given date time:\n\t[{1}].", runnableCheck.Value.Month.ToStringProperlyFormated(), other.Month.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same day, whatever the year or the month.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual date time day is not equal to the given day, whatever the year or the month.</exception>
        public static IChainableCheck<ICheck<DateTime>> IsInSameDayAs(this ICheck<DateTime> check, DateTime other)
        {
            var checkRunner = check as ICheckRunner<DateTime>;
            var runnableCheck = check as IRunnableCheck<DateTime>;

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableCheck.Value.Day != other.Day)
                        {
                            throw new FluentCheckException(string.Format("\nThe day of the actual date time:\n\t[{0}]\nis not equal to the day of the given date time:\n\t[{1}].", runnableCheck.Value.Day.ToStringProperlyFormated(), other.Day.ToStringProperlyFormated()));
                        }
                    },
                string.Format("\nThe day of the actual date time:\n\t[{0}]\nis equal to the day of the given date time:\n\t[{1}].", runnableCheck.Value.Day.ToStringProperlyFormated(), other.Day.ToStringProperlyFormated()));
        }
    }
}