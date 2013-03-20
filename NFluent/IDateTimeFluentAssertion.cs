// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IDateTimeFluentAssertion.cs" company="">
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

    /// <summary>
    /// Provides assertion methods to be executed on a date time instance.
    /// </summary>
    public interface IDateTimeFluentAssertion : IFluentAssertion, IEqualityFluentAssertionTrait<IDateTimeFluentAssertion>, IInstanceTypeFluentAssertionTrait<IDateTimeFluentAssertion>
    {
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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsBefore(DateTime other);

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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsBeforeOrEqualTo(DateTime other);

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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsAfter(DateTime other);

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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsAfterOrEqualTo(DateTime other);

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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringMillis(DateTime other);

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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringSeconds(DateTime other);

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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringMinutes(DateTime other);

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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsEqualToIgnoringHours(DateTime other);

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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsInSameYearAs(DateTime other);

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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsInSameMonthAs(DateTime other);

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
        IChainableFluentAssertion<IDateTimeFluentAssertion> IsInSameDayAs(DateTime other);
    }
}