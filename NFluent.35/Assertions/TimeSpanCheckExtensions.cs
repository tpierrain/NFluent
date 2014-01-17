// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="TimeSpanCheckExtensions.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
    using NFluent.Helpers;

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
             var checker = ExtensibilityHelper.ExtractChecker(check);

             var testedDuration = new Duration(checker.Value, unit);
             var expected = new Duration(providedDuration, unit);
             var notMessage = FluentMessage.BuildMessage("The {0} is not more than the limit.")
                                            .On(testedDuration)
                                            .And.Expected(expected)
                                            .Comparison("more than or equal to");
             var message = FluentMessage.BuildMessage("The {0} is more than the limit.")
                                        .On(testedDuration)
                                        .And.Expected(expected).Comparison("less than");

             return checker.ExecuteCheck(
                    () =>
                        {
                            if (testedDuration >= expected)
                            {
                                throw new FluentCheckException(message.ToString());
                            }
                        }, 
                    notMessage.ToString());
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
             var checker = ExtensibilityHelper.ExtractChecker(check);

             var unit = TimeHelper.DiscoverUnit(comparand);

             var testedDuration = new Duration(checker.Value, unit);
             var expected = new Duration(comparand, unit);

             var notMessage =
                 FluentMessage.BuildMessage("The {0} is not more than the limit.")
                                .On(testedDuration)
                                .And.Expected(expected)
                                .Comparison("more than or equal to");
             var message =
                 FluentMessage.BuildMessage("The {0} is more than the limit.")
                                .On(testedDuration)
                                .And.Expected(expected).Comparison("less than");

             return checker.ExecuteCheck(
                 () =>
                 {
                     if (testedDuration >= expected)
                     {
                         throw new FluentCheckException(message.ToString());
                     }
                 }, 
                 notMessage.ToString());
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
             var checker = ExtensibilityHelper.ExtractChecker(check);

             var testedDuration = new Duration(checker.Value, unit);
             var expected = new Duration(providedDuration, unit);
             var message =
                 FluentMessage.BuildMessage("The {0} is not more than the limit.")
                                .On(testedDuration)
                                .And.Expected(expected)
                                .Comparison("less than or equal to");
             var notMessage =
                 FluentMessage.BuildMessage("The {0} is more than the limit.")
                                .On(testedDuration)
                                .And.Expected(expected).Comparison("more than");

             return checker.ExecuteCheck(
                 () =>
                     {
                         if (testedDuration <= expected)
                         {
                             throw new FluentCheckException(message.ToString());
                         }
                     }, 
                 notMessage.ToString());
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
             var checker = ExtensibilityHelper.ExtractChecker(check);
             
             TimeUnit unit = TimeHelper.DiscoverUnit(comparand);
             var testedDuration = new Duration(checker.Value, unit);
             var expected = new Duration(comparand, unit);

             var message =
             FluentMessage.BuildMessage("The {0} is not more than the limit.")
                            .On(testedDuration)
                            .And.Expected(expected)
                            .Comparison("more than");
             var notMessage =
                 FluentMessage.BuildMessage("The {0} is more than the limit.")
                                .On(testedDuration)
                                .And.Expected(expected).Comparison("less than or equal to");

             return checker.ExecuteCheck(
                 () =>
                 {
                     if (testedDuration <= expected)
                     {
                         throw new FluentCheckException(message.ToString());
                     }
                 }, 
                 notMessage.ToString());
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
             var checker = ExtensibilityHelper.ExtractChecker(check);

             var testedDuration = new Duration(checker.Value, unit);
             var expected = new Duration(duration, unit);

             var message =
             FluentMessage.BuildMessage("The {0} is different from the {1}.")
                            .On(testedDuration)
                            .And.Expected(expected);
             var notMessage =
                 FluentMessage.BuildMessage("The {0} is the same than {1}.")
                                .On(testedDuration)
                                .And.Expected(expected)
                                .Comparison("different than");

             return checker.ExecuteCheck(
                 () =>
                 {
                     if (testedDuration != expected)
                     {
                         throw new FluentCheckException(message.ToString());
                     }
                 }, 
                 notMessage.ToString());
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
             var checker = ExtensibilityHelper.ExtractChecker(check);

             TimeUnit unit = TimeHelper.DiscoverUnit(comparand);
             var testedDuration = new Duration(checker.Value, unit);
             var expected = new Duration(comparand, unit);

             var message =
             FluentMessage.BuildMessage("The {0} is different from the {1}.")
                            .On(testedDuration)
                            .And.Expected(expected);
             var notMessage =
                 FluentMessage.BuildMessage("The {0} is the same than {1}.")
                                .On(testedDuration)
                                .And.Expected(expected)
                                .Comparison("different than");

             return checker.ExecuteCheck(
                 () =>
                     {
                         if (checker.Value != comparand)
                         {
                             throw new FluentCheckException(message.ToString());
                         }
                     }, 
                 notMessage.ToString());
         }
    }
}