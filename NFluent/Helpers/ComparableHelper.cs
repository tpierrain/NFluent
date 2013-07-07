// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ComparableHelper.cs" company="">
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
namespace NFluent.Helpers
{
    using System;

    /// <summary>
    /// Helper class related to IComparable methods (used like a traits).
    /// </summary>
    public static class ComparableHelper
    {
        /// <summary>
        /// Checks that a comparable checked value is before another given value.
        /// </summary>
        /// <param name="checkedValue">The checked value.</param>
        /// <param name="givenValue">The other given value.</param>
        /// <exception cref="NFluent.FluentCheckException">The checked value is not before the given value.</exception>
        public static void IsBefore(IComparable checkedValue, IComparable givenValue)
        {
            if (checkedValue == null || checkedValue.CompareTo(givenValue) >= 0)
            {
                throw new FluentCheckException(FluentMessage.BuildMessage("The {0} is not before the reference value.")
                                                                .On(checkedValue)
                                                                .And.Expected(givenValue)
                                                                .Comparison("before").ToString());
            }
        }

        /// <summary>
        /// Checks that a comparable checked value is after another given value.
        /// </summary>
        /// <param name="checkedValue">The checked value.</param>
        /// <param name="givenValue">The other given value.</param>
        /// <exception cref="NFluent.FluentCheckException">The checked value is not after the given value.</exception>
        public static void IsAfter(IComparable checkedValue, IComparable givenValue)
        {
            if (checkedValue == null || checkedValue.CompareTo(givenValue) <= 0)
            {
                throw new FluentCheckException(FluentMessage.BuildMessage("The {0} is not after the reference value.").On(checkedValue).And.Expected(givenValue).Comparison("after").ToString());
            }
        }
    }
}