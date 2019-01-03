// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanCheckExtensions.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
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

    /// <summary>
    /// Provides check methods to be executed on an <see cref="TimeSpan"/> instance.
    /// </summary>
    public static class TimeSpanCheckExtensions
    {
        /// <summary>
        /// Checks that the actual duration is less (strictly) than a comparand.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="providedDuration">The duration to compare to.</param>
        /// <param name="unit">The unit in which the duration is expressed.</param>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The actual value is not less than the provided duration.</exception>
         public static ICheckLink<ICheck<TimeSpan>> IsLessThan(this ICheck<TimeSpan> check, double providedDuration, TimeUnit unit)
         {
             var expected = new Duration(providedDuration, unit);
             ExtensibilityHelper.BeginCheck(check)
                 .CheckSutAttributes( sut => new Duration(sut, unit), "")
                 .PerformLessThan(expected);
             return ExtensibilityHelper.BuildCheckLink(check);
         }

        /// <summary>
        /// Checks that the actual duration is less (strictly) than a comparand.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="providedDuration">The duration to compare to.</param>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The actual value is not less than the provided duration.</exception>
        public static ICheckLink<ICheck<TimeSpan>> IsLessThan(this ICheck<TimeSpan> check, Duration providedDuration)
        {
            ExtensibilityHelper.BeginCheck(check)
                .CheckSutAttributes( sut => new Duration(sut, providedDuration.Unit), "")
                .PerformLessThan(providedDuration);
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        private static void PerformLessThan(this ICheckLogic<Duration> check, Duration providedDuration)
        {
            check.FailWhen(sut => sut >= providedDuration, "The {0} is more than the limit.")
                .OnNegate("The {0} is not more than the limit.")
                .ComparingTo(providedDuration, "less than", "more than or equal to")
                .EndCheck();
        }
         /// <summary>
         /// Checks that the actual duration is less (strictly) than a comparand.
         /// </summary>
         /// <param name="check">The fluent check to be extended.</param>
         /// <param name="comparand">The value to compare to.</param>
         /// <returns>A check link.</returns>
         /// <exception cref="FluentCheckException">The actual value is not less than the provided comparand.</exception>
         public static ICheckLink<ICheck<TimeSpan>> IsLessThan(this ICheck<TimeSpan> check, TimeSpan comparand)
         {
            var unit = TimeHelper.DiscoverUnit(comparand);
            return check.IsLessThan(new Duration(comparand, unit));
         }

         /// <summary>
         /// Checks that the actual duration is greater (strictly) than a comparand.
         /// </summary>
         /// <param name="check">The fluent check to be extended.</param>
         /// <param name="providedDuration">The duration to compare to.</param>
         /// <param name="unit">The unit in which the duration is expressed.</param>
         /// <returns>A check link.</returns>
         /// <exception cref="FluentCheckException">The actual value is not greater than the provided comparand.</exception>
         public static ICheckLink<ICheck<TimeSpan>> IsGreaterThan(this ICheck<TimeSpan> check, double providedDuration, TimeUnit unit)
         {
             var expected = new Duration(providedDuration, unit);
             ExtensibilityHelper.BeginCheck(check)
                 .CheckSutAttributes( sut => new Duration(sut, unit), "")
                 .PerformGreaterThan(expected);
             return ExtensibilityHelper.BuildCheckLink(check);
         }

        /// <summary>
        /// Checks that the actual duration is less (strictly) than a comparand.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="providedDuration">The duration to compare to.</param>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The actual value is not less than the provided duration.</exception>
        public static ICheckLink<ICheck<TimeSpan>> IsGreaterThan(this ICheck<TimeSpan> check, Duration providedDuration)
        {
            ExtensibilityHelper.BeginCheck(check)
                .CheckSutAttributes( sut => new Duration(sut, providedDuration.Unit), "")
                .PerformGreaterThan(providedDuration);
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        private static void PerformGreaterThan(this ICheckLogic<Duration> check, Duration providedDuration)
        {
            check.FailWhen(sut => sut <= providedDuration, "The {0} is not more than the limit.")
                .OnNegate("The {0} is more than the limit.")
                .ComparingTo(providedDuration, "more than", "less than or equal to")
                .EndCheck();
        }

         /// <summary>
         /// Checks that the actual duration is greater (strictly) than a comparand.
         /// </summary>
         /// <param name="check">The fluent check to be extended.</param>
         /// <param name="comparand">The value to compare to.</param>
         /// <returns>A check link.</returns>
         /// <exception cref="FluentCheckException">The actual value is not greater than the provided comparand.</exception>
         public static ICheckLink<ICheck<TimeSpan>> IsGreaterThan(this ICheck<TimeSpan> check, TimeSpan comparand)
         {
             var unit = TimeHelper.DiscoverUnit(comparand);
             return check.IsGreaterThan(new Duration(comparand, unit));
         }

         /// <summary>
         /// Checks that the actual duration is equal to a target duration.
         /// </summary>
         /// <param name="check">The fluent check to be extended.</param>
         /// <param name="duration">The duration to be compared to.</param>
         /// <param name="unit">The <see cref="TimeUnit" /> in which duration is expressed.</param>
         /// <returns>A check link.</returns>
         /// <exception cref="FluentCheckException">The actual value is not equal to the target duration.</exception>
         public static ICheckLink<ICheck<TimeSpan>> IsEqualTo(this ICheck<TimeSpan> check, double duration, TimeUnit unit)
         {
             var expected = new Duration(duration, unit);
             ExtensibilityHelper.BeginCheck(check)
                 .CheckSutAttributes( sut => new Duration(sut, unit), "")
                 .PerformEqual(expected);
             return ExtensibilityHelper.BuildCheckLink(check);
         }

        /// <summary>
        /// Checks that the actual duration is equal to a target duration.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The duration to be compared to.</param>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The actual value is not equal to the target duration.</exception>
        public static ICheckLink<ICheck<TimeSpan>> IsEqualTo(this ICheck<TimeSpan> check, Duration expected)
        {
            ExtensibilityHelper.BeginCheck(check)
                .CheckSutAttributes( sut => new Duration(sut, expected.Unit), "")
                .PerformEqual(expected);
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        private static void PerformEqual(this ICheckLogic<Duration> check, Duration expected)
        {
                check.FailWhen(sut => sut != expected, "The {0} is different from the {1}.")
                .OnNegate("The {0} is the same than {1}, whereas it must not.")
                .DefineExpectedValue(expected)
                .EndCheck();
        }
         /// <summary>
         /// Checks that the actual duration is equal to a target duration.
         /// </summary>
         /// <param name="check">The fluent check to be extended.</param>
         /// <param name="comparand">The duration to be compared to.</param>
         /// <returns>A check link.</returns>
         /// /// <exception cref="FluentCheckException">The actual value is not equal to the target duration.</exception>
         public static ICheckLink<ICheck<TimeSpan>> IsEqualTo(this ICheck<TimeSpan> check, TimeSpan comparand)
         {
             var unit = TimeHelper.DiscoverUnit(comparand);
             return check.IsEqualTo(new Duration(comparand, unit));
         }
    }
}