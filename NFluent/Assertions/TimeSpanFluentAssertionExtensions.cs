// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="TimeSpanFluentAssertionExtensions.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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
        /// Checks that the actual duration is less (strictly) than a comparand.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="providedDuration">The duration to compare to.</param>
        /// <param name="unit">The unit in which the duration is expressed.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The actual value is not less than the provided duration.</exception>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsLessThan(this IFluentAssertion<TimeSpan> fluentAssertion, double providedDuration, TimeUnit unit)
         {
             TimeSpan comparand = TimeHelper.ToTimeSpan(providedDuration, unit);
             if (fluentAssertion.Value >= comparand)
             {
                 throw new FluentAssertionException(string.Format("\nThe actual value of:\n\t[{0} {2}]\nis not less than:\n\t[{1} {2}]\nas expected.", TimeHelper.Convert(fluentAssertion.Value, unit), providedDuration, unit));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }

         /// <summary>
         /// Checks that the actual duration is less (strictly) than a comparand.
         /// </summary>
         /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
         /// <param name="comparand">The value to compare to.</param>
         /// <returns>A chainable assertion.</returns>
         /// <exception cref="FluentAssertionException">The actual value is not less than the provided comparand.</exception>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsLessThan(this IFluentAssertion<TimeSpan> fluentAssertion, TimeSpan comparand)
         {
             if (fluentAssertion.Value >= comparand)
             {
                 TimeUnit unit = TimeHelper.DiscoverUnit(comparand);
                 throw new FluentAssertionException(string.Format("\nThe actual value of:\n\t[{0} {2}]\nis not less than:\n\t[{1} {2}]\nas expected.", TimeHelper.Convert(fluentAssertion.Value, unit), TimeHelper.Convert(comparand, unit), unit));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }

         /// <summary>
         /// Checks that the actual duration is greater (strictly) than a comparand.
         /// </summary>
         /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
         /// <param name="providedDuration">The duration to compare to.</param>
         /// <param name="unit">The unit in which the duration is expressed.</param>
         /// <returns>A chainable assertion.</returns>
         /// <exception cref="FluentAssertionException">The actual value is not greater than the provided comparand.</exception>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsGreaterThan(this IFluentAssertion<TimeSpan> fluentAssertion, double providedDuration, TimeUnit unit)
         {
             TimeSpan comparand = TimeHelper.ToTimeSpan(providedDuration, unit);
             if (fluentAssertion.Value <= comparand)
             {
                 throw new FluentAssertionException(string.Format("\nThe actual value of:\n\t[{0} {2}]\nis not greater than:\n\t[{1} {2}]\nas expected.", TimeHelper.Convert(fluentAssertion.Value, unit), TimeHelper.Convert(comparand, unit), unit));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }

         /// <summary>
         /// Checks that the actual duration is greater (strictly) than a comparand.
         /// </summary>
         /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
         /// <param name="comparand">The value to compare to.</param>
         /// <returns>A chainable assertion.</returns>
         /// <exception cref="FluentAssertionException">The actual value is not greater than the provided comparand.</exception>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsGreaterThan(this IFluentAssertion<TimeSpan> fluentAssertion, TimeSpan comparand)
         {
             if (fluentAssertion.Value <= comparand)
             {
                 TimeUnit unit = TimeHelper.DiscoverUnit(comparand);
                 throw new FluentAssertionException(string.Format("\nThe actual value of:\n\t[{0} {2}]\nis not greater than:\n\t[{1} {2}]\nas expected.", TimeHelper.Convert(fluentAssertion.Value, unit), TimeHelper.Convert(comparand, unit), unit));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }

         /// <summary>
         /// Checks that the actual duration is equal to a target duration.
         /// </summary>
         /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
         /// <param name="duration">The duration to be compared to.</param>
         /// <param name="unit">The <see cref="TimeUnit" /> in which duration is expressed.</param>
         /// <returns>A chainable assertion.</returns>
         /// <exception cref="FluentAssertionException">The actual value is not equal to the target duration.</exception>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsEqualTo(this IFluentAssertion<TimeSpan> fluentAssertion, double duration, TimeUnit unit)
         {
            TimeSpan comparand = TimeHelper.ToTimeSpan(duration, unit);
            if (fluentAssertion.Value != comparand)
            {
                throw new FluentAssertionException(string.Format("\nThe actual value of:\n\t[{0} {2}]\nis not equal to:\n\t[{1} {2}]\nas expected.", TimeHelper.Convert(fluentAssertion.Value, unit), TimeHelper.Convert(comparand, unit), unit));
            }

            return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }

         /// <summary>
         /// Checks that the actual duration is equal to a target duration.
         /// </summary>
         /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
         /// <param name="comparand">The duration to be compared to.</param>
         /// <returns>A chainable assertion.</returns>
         /// /// <exception cref="FluentAssertionException">The actual value is not equal to the target duration.</exception>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsEqualTo(this IFluentAssertion<TimeSpan> fluentAssertion, TimeSpan comparand)
         {
             if (fluentAssertion.Value != comparand)
             {
                 TimeUnit unit = TimeHelper.DiscoverUnit(comparand);
                 throw new FluentAssertionException(string.Format("\nThe actual value of:\n\t[{0} {2}]\nis not equal to:\n\t[{1} {2}]\nas expected.", TimeHelper.Convert(fluentAssertion.Value, unit), TimeHelper.Convert(comparand, unit), unit));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }

         /// <summary>
         /// Checks that the actual instance is an instance of the given type.
         /// </summary>
         /// <typeparam name="T">The expected Type of the instance.</typeparam>
         /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
         /// <returns>
         /// A chainable fluent assertion.
         /// </returns>
         /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsInstanceOf<T>(this IFluentAssertion<TimeSpan> fluentAssertion)
         {
             if (fluentAssertion.Negated)
             {
                 IsInstanceHelper.IsNotInstanceOf(fluentAssertion.Value, typeof(T));
             }
             else
             {
                 IsInstanceHelper.IsInstanceOf(fluentAssertion.Value, typeof(T));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
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
         public static IChainableFluentAssertion<IFluentAssertion<TimeSpan>> IsNotInstanceOf<T>(this IFluentAssertion<TimeSpan> fluentAssertion)
         {
             if (fluentAssertion.Negated)
             {
                 Helpers.IsInstanceHelper.IsInstanceOf(fluentAssertion.Value, typeof(T));
             }
             else
             {
                 Helpers.IsInstanceHelper.IsNotInstanceOf(fluentAssertion.Value, typeof(T));
             }

             return new ChainableFluentAssertion<IFluentAssertion<TimeSpan>>(fluentAssertion);
         }
    }
}