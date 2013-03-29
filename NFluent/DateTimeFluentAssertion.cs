// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DateTimeFluentAssertion.cs" company="">
// //   Copyright 2013 Marc-Antoine LATOUR
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
    public class DateTimeFluentAssertion : IDateTimeFluentAssertion
    {
        private readonly DateTime dateTimeUnderTest;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeFluentAssertion"/> class. 
        /// </summary>
        /// <param name="dateTimeUnderTest">The DateTime to assert on. </param>
        public DateTimeFluentAssertion(DateTime dateTimeUnderTest)
        {
            this.dateTimeUnderTest = dateTimeUnderTest;
        }

        #region Equality Assertions
        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualTo(object expected)
        {
            Helpers.EqualityHelper.IsEqualTo(this.dateTimeUnderTest, expected);
            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected.</param>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsNotEqualTo(object expected)
        {
            Helpers.EqualityHelper.IsNotEqualTo(this.dateTimeUnderTest, expected);
            return this.ChainFluentAssertion();
        }
        #endregion

        #region IsInstance Assertions

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsInstanceOf<T>()
        {
            Helpers.IsInstanceHelper.IsInstanceOf(this.dateTimeUnderTest, typeof(T));
            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsNotInstanceOf<T>()
        {
            Helpers.IsInstanceHelper.IsNotInstanceOf(this.dateTimeUnderTest, typeof(T));
            return this.ChainFluentAssertion();
        }
        #endregion

        /// <summary>
        /// Checks that the actual DateTime is strictly before the given one.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not before the given one.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsBefore(DateTime other)
        {
            if (this.dateTimeUnderTest < other)
            {
                return this.ChainFluentAssertion();    
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not before the given one [{1}]", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is before or equals to the given one.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not before or equals to the given one.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsBeforeOrEqualTo(DateTime other)
        {
            if (this.dateTimeUnderTest <= other)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not before or equals to the given one [{1}]", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is strictly after the given one.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not after the given one.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsAfter(DateTime other)
        {
            if (this.dateTimeUnderTest > other)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not after the given one [{1}]", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is after or equals to the given one.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not after or equals to the given one.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsAfterOrEqualTo(DateTime other)
        {
            if (this.dateTimeUnderTest >= other)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not after or equals to the given one [{1}]", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day, hour, minute and second fields,
        ///  (millisecond fields are ignored in comparison).
        ///  Code example :
        ///   <code>
        ///   // successfull assertions
        ///   DateTime dateTime1 = new DateTime(2000, 1, 1, 0, 0, 1, 0);
        ///   DateTime dateTime2 = new DateTime(2000, 1, 1, 0, 0, 1, 456);
        ///   assertThat(dateTime1).isEqualToIgnoringMillis(dateTime2);
        ///   
        ///   // failing assertions (even if time difference is only 1ms)
        ///   DateTime dateTimeA = new DateTime(2000, 1, 1, 0, 0, 1, 0);
        ///   DateTime dateTimeB = new DateTime(2000, 1, 1, 0, 0, 0, 999);
        ///   assertThat(dateTimeA).isEqualToIgnoringMillis(dateTimeB);
        ///   </code>
        /// </summary>
        /// <remarks>
        ///  Assertion can fail with dateTimes in same chronological millisecond time window, e.g :
        ///   2000-01-01T00:00:<b>01.000</b> and 2000-01-01T00:00:<b>00.999</b>.
        ///   Assertion fails as second fields differ even if time difference is only 1 millis.
        /// </remarks>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not equal to the given one with the milliseconds ignored.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringMillis(DateTime other)
        {
            if (this.dateTimeUnderTest.Year == other.Year &&
                this.dateTimeUnderTest.Month == other.Month &&
                this.dateTimeUnderTest.Day == other.Day &&
                this.dateTimeUnderTest.Hour == other.Hour &&
                this.dateTimeUnderTest.Minute == other.Minute &&
                this.dateTimeUnderTest.Second == other.Second)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not equal to the given date time [{1}] ignoring milliseconds", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day, hour and minute fields,
        /// (Seconds and millisecond fields are ignored in comparison).
        /// <code>
        ///  Assertion can fail with DateTimes in same chronological second time window, e.g :
        ///  2000-01-01T00:<b>01:00</b>.000 and 2000-01-01T00:<b>00:59</b>.000.
        ///  Assertion fails as minute fields differ even if time difference is only 1s.
        ///  </code>
        ///  Code example :
        ///  <code>
        ///  // successfull assertions
        ///  DateTime dateTime1 = new DateTime(2000, 1, 1, 23, 50, 0, 0);
        ///  DateTime dateTime2 = new DateTime(2000, 1, 1, 23, 50, 10, 456);
        ///  Check.That(dateTime1).IsEqualToIgnoringSeconds(dateTime2);
        ///   
        ///  // failing assertions (even if time difference is only 1ms)
        ///  DateTime dateTimeA = new DateTime(2000, 1, 1, 23, 50, 00, 000);
        ///  DateTime dateTimeB = new DateTime(2000, 1, 1, 23, 49, 59, 999);
        ///  Check.That(dateTimeA).IsEqualToIgnoringSeconds(dateTimeB);
        ///  </code>
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not equal to the given one with second and millisecond fields ignored.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringSeconds(DateTime other)
        {   
            if (this.dateTimeUnderTest.Year == other.Year &&
                this.dateTimeUnderTest.Month == other.Month &&
                this.dateTimeUnderTest.Day == other.Day &&
                this.dateTimeUnderTest.Hour == other.Hour &&
                this.dateTimeUnderTest.Minute == other.Minute)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not equal to the given date time [{1}] ignoring seconds", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day and hour fields,
        /// (Minutes, seconds and millisecond fields are ignored in comparison).
        ///  <code>
        ///  Assertion can fail with dateTimes in same chronological second time window, e.g :
        ///  2000-01-01T<b>01:00</b>:00.000 and 2000-01-01T<b>00:59:59</b>.000.
        ///  Time difference is only 1s but hour fields differ.
        ///  </code>
        ///  Code example :
        ///  <code>
        ///  // successfull assertions
        ///  DateTime dateTime1 = new DateTime(2000, 1, 1, 23, 50, 0, 0);
        ///  DateTime dateTime2 = new DateTime(2000, 1, 1, 23, 00, 2, 7);
        ///  Check.That(dateTime1).IsEqualToIgnoringMinutes(dateTime2);
        ///  
        ///  // failing assertions (even if time difference is only 1ms)
        ///  DateTime dateTimeA = new DateTime(2000, 1, 1, 01, 00, 00, 000);
        ///  DateTime dateTimeB = new DateTime(2000, 1, 1, 00, 59, 59, 999);
        ///  Check.That(dateTimeA).IsEqualToIgnoringMinutes(dateTimeB);
        ///  </code>
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not equal to the given one with minute, second and millisecond fields ignored.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringMinutes(DateTime other)
        {
            if (this.dateTimeUnderTest.Year == other.Year &&
                this.dateTimeUnderTest.Month == other.Month &&
                this.dateTimeUnderTest.Day == other.Day &&
                this.dateTimeUnderTest.Hour == other.Hour)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not equal to the given date time [{1}] ignoring minutes", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month and day fields,
        /// * (Hours, minutes, seconds and millisecond fields are ignored in comparison).
        /// * <code>
        ///  Assertion can fail with dateTimes in same chronological minute time window, e.g :
        ///  2000-01-<b>01T23:59</b>:00.000 and 2000-01-02T<b>00:00</b>:00.000.
        ///  Time difference is only 1min but day fields differ.
        ///  </code>
        ///  Code example :
        ///  <code>
        ///  // successfull assertions
        ///  DateTime dateTime1 = new DateTime(2000, 1, 1, 23, 59, 59, 999);
        ///  DateTime dateTime2 = new DateTime(2000, 1, 1, 00, 00, 00, 000);
        ///  CheckThat(dateTime1).IsEqualToIgnoringHours(dateTime2);
        ///  
        ///  // failing assertions (even if time difference is only 1ms)
        ///  DateTime dateTimeA = new DateTime(2000, 1, 2, 00, 00, 00, 000);
        ///  DateTime dateTimeB = new DateTime(2000, 1, 1, 23, 59, 59, 999);
        ///  CheckThat(dateTimeA).IsEqualToIgnoringHours(dateTimeB);
        ///  </code>
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not equal to the given one with hour, minute, second and millisecond fields ignored.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringHours(DateTime other)
        {
            if (this.dateTimeUnderTest.Year == other.Year &&
                this.dateTimeUnderTest.Month == other.Month &&
                this.dateTimeUnderTest.Day == other.Day)
            {
                return this.ChainFluentAssertion();
            }
            
            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not equal to the given date time [{1}] ignoring hours", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time year is not equal to the given year.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsInSameYearAs(DateTime other)
        {
            if (this.dateTimeUnderTest.Year == other.Year)
            {
                return this.ChainFluentAssertion();
            }
            
            throw new FluentAssertionException(string.Format("The actual year [{0}] is not equal to the given date time year [{1}]", this.dateTimeUnderTest.Year.ToStringProperlyFormated(), other.Year.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same month.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time month is not equal to the given month.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsInSameMonthAs(DateTime other)
        {
            if (this.dateTimeUnderTest.Month == other.Month)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual month [{0}] is not equal to the given date time month [{1}]", this.dateTimeUnderTest.Month.ToStringProperlyFormated(), other.Month.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same day.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time day is not equal to the given day.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsInSameDayAs(DateTime other)
        {
            if (this.dateTimeUnderTest.Day == other.Day)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual day [{0}] is not equal to the given date time day [{1}]", this.dateTimeUnderTest.Day.ToStringProperlyFormated(), other.Day.ToStringProperlyFormated()));
        }

        private IChainableFluentAssertion<IDateTimeFluentAssertion> ChainFluentAssertion()
        {
            return new ChainableFluentAssertion<IDateTimeFluentAssertion>(this);
        }
    }
}