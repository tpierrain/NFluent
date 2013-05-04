// // --------------------------------------------------------------------------------------------------------------------
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

    /// <summary>
    /// Provides assertion methods to be executed on a date time instance. 
    /// </summary>
    public static class DateTimeFluentAssertionExtensions
    {
        #region Equality Assertions

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsEqualTo(this IFluentAssertion<DateTime> fluentAssertion, object expected)
        {
            Helpers.EqualityHelper.IsEqualTo(fluentAssertion.Value, expected);

            return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsNotEqualTo(this IFluentAssertion<DateTime> fluentAssertion, object expected)
        {
            Helpers.EqualityHelper.IsNotEqualTo(fluentAssertion.Value, expected);

            return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
        }

        #endregion

        #region IsInstance Assertions

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsInstanceOf<T>(this IFluentAssertion<DateTime> fluentAssertion)
        {
            if (fluentAssertion.Negated)
            {
                Helpers.IsInstanceHelper.IsNotInstanceOf(fluentAssertion.Value, typeof(T));
            }
            else
            {
                Helpers.IsInstanceHelper.IsInstanceOf(fluentAssertion.Value, typeof(T));
            }

            return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsNotInstanceOf<T>(this IFluentAssertion<DateTime> fluentAssertion)
        {
            if (fluentAssertion.Negated)
            {
                Helpers.IsInstanceHelper.IsInstanceOf(fluentAssertion.Value, typeof(T));
            }
            else
            {
                Helpers.IsInstanceHelper.IsNotInstanceOf(fluentAssertion.Value, typeof(T));
            }

            return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
        }

        #endregion

        /// <summary>
        /// Checks that the actual DateTime is strictly before the given one.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time is not before the given one.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsBefore(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {
            if (fluentAssertion.Value < other)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe actual date time:\n\t[{0}]\nis not before the given one:\n\t[{1}].", fluentAssertion.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is before or equals to the given one.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time is not before or equals to the given one.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsBeforeOrEqualTo(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {
            if (fluentAssertion.Value <= other)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe actual date time:\n\t[{0}]\nis not before or equals to the given one:\n\t[{1}].", fluentAssertion.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is strictly after the given one.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time is not after the given one.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsAfter(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {
            if (fluentAssertion.Value > other)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe actual date time:\n\t[{0}]\nis not after the given one:\n\t[{1}].", fluentAssertion.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is after or equals to the given one.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time is not after or equals to the given one.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsAfterOrEqualTo(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {
            if (fluentAssertion.Value >= other)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe actual date time:\n\t[{0}]\nis not after or equals to the given one:\n\t[{1}].", fluentAssertion.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        // TODO : replace the FEST assert assertThat samples by NFluent assertions

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day, hour, minute and second fields,
        /// (millisecond fields are ignored in comparison).
        /// Code example :
        /// <code>
        ///     // successfull assertions
        ///     DateTime dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, 0);
        ///     DateTime dateTime2 = new DateTime(2000, 1, 1, 0, 0, 1, 456);
        ///     assertThat(dateTime1).isEqualToIgnoringMillis(dateTime2);
        ///     // failing assertions (even if time difference is only 1ms)
        ///     DateTime dateTimeA = new DateTime(2000, 1, 1, 0, 0, 1, 0);
        ///     DateTime dateTimeB = new DateTime(2000, 1, 1, 0, 0, 0, 999);
        ///     assertThat(dateTimeA).isEqualToIgnoringMillis(dateTimeB);
        /// </code>
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time is not equal to the given one with the milliseconds ignored.</exception>
        /// <remarks>
        /// Assertion can fail with dateTimes in same chronological millisecond time window, e.g :
        /// 2000-01-01T00:00:<b>01.000</b> and 2000-01-01T00:00:<b>00.999</b>.
        /// Assertion fails as second fields differ even if time difference is only 1 millis.
        /// </remarks>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsEqualToIgnoringMillis(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {
            if (fluentAssertion.Value.Year == other.Year &&
                fluentAssertion.Value.Month == other.Month &&
                fluentAssertion.Value.Day == other.Day &&
                fluentAssertion.Value.Hour == other.Hour &&
                fluentAssertion.Value.Minute == other.Minute &&
                fluentAssertion.Value.Second == other.Second)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe actual date time:\n\t[{0}]\nis not equal to the given date time:\n\t[{1}]\nignoring milliseconds.", fluentAssertion.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day, hour and minute fields,
        /// (Seconds and millisecond fields are ignored in comparison).
        /// <code>
        /// Assertion can fail with DateTimes in same chronological second time window, e.g :
        /// 2000-01-01T00:<b>01:00</b>.000 and 2000-01-01T00:<b>00:59</b>.000.
        /// Assertion fails as minute fields differ even if time difference is only 1s.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successfull assertions
        ///     DateTime dateTime1 = new DateTime(2000, 1, 1, 23, 50, 0, 0);
        ///     DateTime dateTime2 = new DateTime(2000, 1, 1, 23, 50, 10, 456);
        ///     Check.That(dateTime1).IsEqualToIgnoringSeconds(dateTime2);
        ///     // failing assertions (even if time difference is only 1ms)
        ///     DateTime dateTimeA = new DateTime(2000, 1, 1, 23, 50, 00, 000);
        ///     DateTime dateTimeB = new DateTime(2000, 1, 1, 23, 49, 59, 999);
        ///     Check.That(dateTimeA).IsEqualToIgnoringSeconds(dateTimeB);
        /// </code>
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time is not equal to the given one with second and millisecond fields ignored.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsEqualToIgnoringSeconds(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {   
            if (fluentAssertion.Value.Year == other.Year &&
                fluentAssertion.Value.Month == other.Month &&
                fluentAssertion.Value.Day == other.Day &&
                fluentAssertion.Value.Hour == other.Hour &&
                fluentAssertion.Value.Minute == other.Minute)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe actual date time:\n\t[{0}]\nis not equal to the given date time:\n\t[{1}]\nignoring seconds.", fluentAssertion.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day and hour fields,
        /// (Minutes, seconds and millisecond fields are ignored in comparison).
        /// <code>
        /// Assertion can fail with dateTimes in same chronological second time window, e.g :
        /// 2000-01-01T<b>01:00</b>:00.000 and 2000-01-01T<b>00:59:59</b>.000.
        /// Time difference is only 1s but hour fields differ.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successfull assertions
        ///     DateTime dateTime1 = new DateTime(2000, 1, 1, 23, 50, 0, 0);
        ///     DateTime dateTime2 = new DateTime(2000, 1, 1, 23, 00, 2, 7);
        ///     Check.That(dateTime1).IsEqualToIgnoringMinutes(dateTime2);
        ///     // failing assertions (even if time difference is only 1ms)
        ///     DateTime dateTimeA = new DateTime(2000, 1, 1, 01, 00, 00, 000);
        ///     DateTime dateTimeB = new DateTime(2000, 1, 1, 00, 59, 59, 999);
        ///     Check.That(dateTimeA).IsEqualToIgnoringMinutes(dateTimeB);
        /// </code>
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time is not equal to the given one with minute, second and millisecond fields ignored.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsEqualToIgnoringMinutes(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {
            if (fluentAssertion.Value.Year == other.Year &&
                fluentAssertion.Value.Month == other.Month &&
                fluentAssertion.Value.Day == other.Day &&
                fluentAssertion.Value.Hour == other.Hour)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe actual date time:\n\t[{0}]\nis not equal to the given date time:\n\t[{1}]\nignoring minutes.", fluentAssertion.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month and day fields,
        /// * (Hours, minutes, seconds and millisecond fields are ignored in comparison).
        /// * <code>
        /// Assertion can fail with dateTimes in same chronological minute time window, e.g :
        /// 2000-01-<b>01T23:59</b>:00.000 and 2000-01-02T<b>00:00</b>:00.000.
        /// Time difference is only 1min but day fields differ.
        /// </code>
        /// Code example :
        /// <code>
        ///     // successfull assertions
        ///     DateTime dateTime1 = new DateTime(2000, 1, 1, 23, 59, 59, 999);
        ///     DateTime dateTime2 = new DateTime(2000, 1, 1, 00, 00, 00, 000);
        ///     CheckThat(dateTime1).IsEqualToIgnoringHours(dateTime2);
        ///     // failing assertions (even if time difference is only 1ms)
        ///     DateTime dateTimeA = new DateTime(2000, 1, 2, 00, 00, 00, 000);
        ///     DateTime dateTimeB = new DateTime(2000, 1, 1, 23, 59, 59, 999);
        ///     CheckThat(dateTimeA).IsEqualToIgnoringHours(dateTimeB);
        /// </code>
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time is not equal to the given one with hour, minute, second and millisecond fields ignored.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsEqualToIgnoringHours(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {
            if (fluentAssertion.Value.Year == other.Year &&
                fluentAssertion.Value.Month == other.Month &&
                fluentAssertion.Value.Day == other.Day)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe actual date time:\n\t[{0}]\nis not equal to the given date time:\n\t[{1}]\nignoring hours.", fluentAssertion.Value.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time year is not equal to the given year.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsInSameYearAs(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {
            if (fluentAssertion.Value.Year == other.Year)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe year of the actual date time:\n\t[{0}]\nis not equal to the year of the given date time:\n\t[{1}].", fluentAssertion.Value.Year.ToStringProperlyFormated(), other.Year.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same month.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time month is not equal to the given month.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsInSameMonthAs(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {
            if (fluentAssertion.Value.Month == other.Month)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe month of the actual date time:\n\t[{0}]\nis not equal to the month of the given date time:\n\t[{1}].", fluentAssertion.Value.Month.ToStringProperlyFormated(), other.Month.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same day.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="other">The other DateTime.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual date time day is not equal to the given day.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<DateTime>> IsInSameDayAs(this IFluentAssertion<DateTime> fluentAssertion, DateTime other)
        {
            if (fluentAssertion.Value.Day == other.Day)
            {
                return new ChainableFluentAssertion<IFluentAssertion<DateTime>>(fluentAssertion);
            }

            throw new FluentAssertionException(string.Format("\nThe day of the actual date time:\n\t[{0}]\nis not equal to the day of the given date time:\n\t[{1}].", fluentAssertion.Value.Day.ToStringProperlyFormated(), other.Day.ToStringProperlyFormated()));
        }
    }
}