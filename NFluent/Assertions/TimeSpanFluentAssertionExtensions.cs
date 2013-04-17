﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="TimeSpanFluentAssertionExtensions.cs" company="">
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
namespace NFluent
{
    using System;

    using NFluent.Helpers;

    /// <summary>
    /// Provides assertion methods to be executed on an <see cref="TimeSpan"/> instance.
    /// </summary>
    public static class TimeSpanFluentAssertionExtensions
    {
        /// <summary>
        /// Check that the actual duration is less (strictly) than a comparand.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion.</param>
        /// <param name="value">The duration.</param>
        /// <param name="unit">Unit in which the duration is expressed.</param>
        /// <returns>A chainable assertion.</returns>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsLessThan(
             this IFluentAssertion<TimeSpan> fluentAssertion, double value, TimeUnit unit)
         {
             TimeSpan comparand = TimeHelper.ToTimeSpan(value, unit);
             if (fluentAssertion.Value >= comparand)
             {
                 throw new FluentAssertionException(
                     string.Format(
                         "\nWas:\n\t[{0} {2}]\nExpected: Less than\n\t[{1} {2}]",
                         TimeHelper.Convert(fluentAssertion.Value, unit),
                         value,
                         unit));
             }

            return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }

         /// <summary>
         /// Check that the actual duration is less (strictly) than a comparand.
         /// </summary>
         /// <param name="fluentAssertion">
         /// The fluent assertion.
         /// </param>
         /// <param name="comparand">
         /// Value to compare to.
         /// </param>
         /// <returns>
         /// A chainable assertion.
         /// </returns>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsLessThan(
             this IFluentAssertion<TimeSpan> fluentAssertion, TimeSpan comparand)
         {
             if (fluentAssertion.Value >= comparand)
             {
                 TimeUnit unit = TimeHelper.DiscoverUnit(comparand);
                 throw new FluentAssertionException(
                     string.Format(
                         "\nWas:\n\t[{0} {2}]\nExpected: Less than\n\t[{1} {2}]",
                         TimeHelper.Convert(fluentAssertion.Value, unit),
                         TimeHelper.Convert(comparand, unit),
                         unit));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }

         /// <summary>
         /// Check that the actual duration is greater (strictly) than a comparand.
         /// </summary>
         /// <param name="fluentAssertion">The fluent assertion.</param>
         /// <param name="value">The duration.</param>
         /// <param name="unit">Unit in which the duration is expressed.</param>
         /// <returns>A chainable assertion.</returns>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsGreaterThan(
             this IFluentAssertion<TimeSpan> fluentAssertion, double value, TimeUnit unit)
         {
             TimeSpan comparand = TimeHelper.ToTimeSpan(value, unit);
             if (fluentAssertion.Value <= comparand)
             {
                 throw new FluentAssertionException(
                     string.Format(
                         "\nWas:\n\t[{0} {2}]\nExpected: Greater than\n\t[{1} {2}]",
                         TimeHelper.Convert(fluentAssertion.Value, unit),
                         value,
                         unit));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }

         /// <summary>
         /// Check that the actual duration is greater (strictly) than a comparand.
         /// </summary>
         /// <param name="fluentAssertion">The fluent assertion.</param>
         /// <param name="comparand">
         /// Value to compare to.
         /// </param>
         /// <returns>A chainable assertion.</returns>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsGreaterThan(
             this IFluentAssertion<TimeSpan> fluentAssertion, TimeSpan comparand)
         {
             if (fluentAssertion.Value <= comparand)
             {
                 TimeUnit unit = TimeHelper.DiscoverUnit(comparand);
                 throw new FluentAssertionException(
                     string.Format(
                         "\nWas:\n\t[{0} {2}]\nExpected: Greater than\n\t[{1} {2}]",
                         TimeHelper.Convert(fluentAssertion.Value, unit),
                         TimeHelper.Convert(comparand, unit),
                         unit));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }

         /// <summary>
         /// Checks that the actual duration is equal to a target duration.
         /// </summary>
         /// <param name="fluentAssertion">
         /// The fluent assertion.
         /// </param>
         /// <param name="duration">
         /// The duration.
         /// </param>
         /// <param name="unit">
         /// The <see cref="TimeUnit"/> in which duration is expressed.
         /// </param>
         /// <returns>
         /// A chainable assertion.
         /// </returns>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsEqualTo(
            this IFluentAssertion<TimeSpan> fluentAssertion, double duration, TimeUnit unit)
        {
            TimeSpan comparand = TimeHelper.ToTimeSpan(duration, unit);
            if (fluentAssertion.Value != comparand)
            {
                throw new FluentAssertionException(
                    string.Format(
                        "\nWas:\n\t[{0} {2}]\nExpected:\n\t[{1} {2}]",
                        TimeHelper.Convert(fluentAssertion.Value, unit),
                        duration,
                        unit));
            }

            return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
        }

         /// <summary>
         /// Checks that the actual duration is equal to a target duration.
         /// </summary>
         /// <param name="fluentAssertion">The fluent assertion.</param>
         /// <param name="comparand">
         /// Value to compare to.
         /// </param>
         /// <returns>A chainable assertion.</returns>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsEqualTo(
            this IFluentAssertion<TimeSpan> fluentAssertion, TimeSpan comparand)
         {
             if (fluentAssertion.Value != comparand)
             {
                 TimeUnit unit = TimeHelper.DiscoverUnit(comparand);
                 throw new FluentAssertionException(
                     string.Format(
                         "\nWas:\n\t[{0} {2}]\nExpected:\n\t[{1} {2}]",
                         TimeHelper.Convert(fluentAssertion.Value, unit),
                         TimeHelper.Convert(comparand, unit),
                         unit));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }
    }
}