// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DoubleSpecificCheckExtensions.cs" company="">
// //   Copyright 2014 Thomas PIERRAIN
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
    using Extensibility;
    using Kernel;
    using Messages;

    /// <summary>
    /// Provides specific check methods to be executed on an <see cref="double"/> value.
    /// </summary>
    public static class DoubleSpecificCheckExtensions
    {
        // DoNotChangeOrRemoveThisLine

        /// <summary>
        /// Determines whether the specified number evaluates to a value that is not a number (NaN).
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The current value is a number.</exception>
        public static ICheckLink<ICheck<double>> IsNaN(this ICheck<double> check)
        {
            ExtensibilityHelper.BeginCheck(check).
                SetSutName("double value").
                FailWhen((sut) => !double.IsNaN(sut), "The {0} is a number whereas it must not.").
                OnNegate("The {0} is not a number (NaN) whereas it must.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Determines whether the specified number evaluates to a value that is finite (i.e. not infinity).
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The specified number evaluates to a value that is infinite (i.e. equals to infinity).</exception>
        public static ICheckLink<ICheck<double>> IsFinite(this ICheck<double> check)
        {
            ExtensibilityHelper.BeginCheck(check).
                SetSutName("double value").
                FailWhen(double.IsInfinity, "The {0} is an infinite number whereas it must not.").
                OnNegate("The {0} is a finite number whereas it must not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Determines whether the actual number is close to an expected value within a given within.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="within">The within.</param>
        /// <returns>A continuation check.</returns>
        public static ICheckLink<ICheck<double>> IsCloseTo(this ICheck<double> check, Double expected, Double within)
        {
            var range = new RangeBlock(expected, within);
            ExtensibilityHelper.BeginCheck(check).
                FailWhen((sut) => !range.IsInRange(sut), "The {0} is outside the expected value range.").
                DefineExpectedValue(range).
                OnNegate("The {0} is within the expected range, whereas it must not.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }
    }
}
