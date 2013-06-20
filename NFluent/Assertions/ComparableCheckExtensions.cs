// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ComparableCheckExtensions.cs" company="">
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
    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on an <see cref="IComparable"/> instance.
    /// </summary>
    public static class ComparableCheckExtensions
    {
        /// <summary>
        /// Determines whether the specified value is before the other one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="otherValue">The other value.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The current value is not before the other one.</exception>
        public static ICheckLink<ICheck<IComparable>> IsBefore(this ICheck<IComparable> check, IComparable otherValue)
        {
            var checkRunner = check as ICheckRunner<IComparable>;
            var runnableCheck = check as IRunnableCheck<IComparable>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        if (runnableCheck.Value == null || runnableCheck.Value.CompareTo(otherValue) >= 0)
                        {
                            throw new FluentCheckException(
                                FluentMessage.BuildMessage("The {0} is after the reference value whereas it must not.")
                                             .On(runnableCheck.Value)
                                             .And.Expected(otherValue)
                                             .Comparison("before")
                                             .ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is before the reference value whereas it must not.").On(runnableCheck.Value).And.Expected(otherValue).Comparison("after").ToString());
        }
    }
}