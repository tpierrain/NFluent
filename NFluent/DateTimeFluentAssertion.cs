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
    /// Please note that every DateTime are convert to UTC before being compare.
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
            if (expected is DateTime)
            {
                Helpers.EqualityHelper.IsEqualTo(this.dateTimeUnderTest.ToUniversalTime(), ((DateTime)expected).ToUniversalTime());
            }
            else
            {
                throw new FluentAssertionException(string.Format("[{0}] not equals to the expected [{1}]", this.dateTimeUnderTest.ToUniversalTime().ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
            }
            
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
            if (expected is DateTime)
            {
                Helpers.EqualityHelper.IsNotEqualTo(this.dateTimeUnderTest.ToUniversalTime(), ((DateTime)expected).ToUniversalTime());
            }
            else
            {
                Helpers.EqualityHelper.IsNotEqualTo(this.dateTimeUnderTest.ToUniversalTime(), expected);
            }

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
        /// Checks that the actual DateTime is strictly before the expected.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not before the expected.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsBefore(DateTime other)
        {
            if (this.dateTimeUnderTest.ToUniversalTime() < other.ToUniversalTime())
            {
                return this.ChainFluentAssertion();    
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not before the expected [{1}]", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is before or equals to the expected.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not before or equal to the expected.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsBeforeOrEqualTo(DateTime other)
        {
            if (this.dateTimeUnderTest.ToUniversalTime() <= other.ToUniversalTime())
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not before or equal to the expected [{1}]", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is strictly after the expected.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not after the expected.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsAfter(DateTime other)
        {
            if (this.dateTimeUnderTest.ToUniversalTime() > other.ToUniversalTime())
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not after the expected [{1}]", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual DateTime is after or equals to the expected.
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not after or equal to the expected.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsAfterOrEqualTo(DateTime other)
        {
            if (this.dateTimeUnderTest.ToUniversalTime() >= other.ToUniversalTime())
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not after or equal to the expected [{1}]", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day, hour, minute and second fields,
        /// * (millisecond fields are ignored in comparison).
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not equal to the expected with the milliseconds ignore.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringMillis(DateTime other)
        {
            var utc = this.dateTimeUnderTest.ToUniversalTime();
            var otherUtc = other.ToUniversalTime();
            if (utc.Year == otherUtc.Year &&
                utc.Month == otherUtc.Month &&
                utc.Day == otherUtc.Day &&
                utc.Hour == otherUtc.Hour &&
                utc.Minute == otherUtc.Minute &&
                utc.Second == otherUtc.Second)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not equal to the given date time [{1}] ignoring milliseconds", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day, hour and minute fields,
        /// * (Seconds and millisecond fields are ignored in comparison).
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not equal to the expected with second and millisecond fields ignore.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringSeconds(DateTime other)
        {
            var utc = this.dateTimeUnderTest.ToUniversalTime();
            var otherUtc = other.ToUniversalTime();
            if (utc.Year == otherUtc.Year &&
                utc.Month == otherUtc.Month &&
                utc.Day == otherUtc.Day &&
                utc.Hour == otherUtc.Hour &&
                utc.Minute == otherUtc.Minute)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not equal to the given date time [{1}] ignoring seconds", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month, day and hour fields,
        /// * (Minutes, seconds and millisecond fields are ignored in comparison).
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not equal to the expected with minute, second and millisecond fields ignore.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringMinutes(DateTime other)
        {
            var utc = this.dateTimeUnderTest.ToUniversalTime();
            var otherUtc = other.ToUniversalTime();

            if (utc.Year == otherUtc.Year &&
                utc.Month == otherUtc.Month &&
                utc.Day == otherUtc.Day &&
                utc.Hour == otherUtc.Hour)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual date time [{0}] is not equal to the given date time [{1}] ignoring minutes", this.dateTimeUnderTest.ToStringProperlyFormated(), other.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that actual and given DateTime have same year, month and day fields,
        /// * (Hours, minutes, seconds and millisecond fields are ignored in comparison).
        /// </summary>
        /// <param name="other">
        /// The other DateTime.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual date time is not equal to the expected with hour, minute, second and millisecond fields ignore.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringHours(DateTime other)
        {
            var utc = this.dateTimeUnderTest.ToUniversalTime();
            var otherUtc = other.ToUniversalTime();

            if (utc.Year == otherUtc.Year &&
                utc.Month == otherUtc.Month &&
                utc.Day == otherUtc.Day)
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
        /// The actual date time year is not equal to the given DateTime year.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsInSameYearAs(DateTime other)
        {
            if (this.dateTimeUnderTest.ToUniversalTime().Year == other.ToUniversalTime().Year)
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
        /// The actual date time month is not equal to the given DateTime month.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsInSameMonthAs(DateTime other)
        {
            if (this.dateTimeUnderTest.ToUniversalTime().Month == other.ToUniversalTime().Month)
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
        /// The actual date time day is not equal to the given DateTime day.
        /// </exception>
        public IChainableFluentAssertion<IDateTimeFluentAssertion> IsInSameDayAs(DateTime other)
        {
            if (this.dateTimeUnderTest.ToUniversalTime().Day == other.ToUniversalTime().Day)
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