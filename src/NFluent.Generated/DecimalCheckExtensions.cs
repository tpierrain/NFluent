 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="DecimalCheckExtensions.cs" company="">
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

    using Kernel;

    /// <summary>
    /// Provides check methods to be executed on an <see cref="decimal"/> value.
    /// </summary>
    public static class DecimalCheckExtensions
    {
        // DoNotChangeOrRemoveThisLine

        #pragma warning restore 169

        /// <summary>
        /// Determines whether the specified value is before the other one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="givenValue">The other value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The current value is not before the other one.</exception>
        public static ICheckLink<ICheck<decimal>> IsBefore(this ICheck<decimal> check, decimal givenValue)
        {
            ExtensibilityHelper.BeginCheck(check)
                .ComparingTo(givenValue, "before", "after")
                .FailsIf(sut => sut.CompareTo(givenValue) >= 0, "The {0} is not before the reference value.")
                .Negates("The {0} is before the reference value whereas it must not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Determines whether the specified value is after the other one.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="givenValue">The other value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The current value is not after the other one.</exception>
        public static ICheckLink<ICheck<decimal>> IsAfter(this ICheck<decimal> check, decimal givenValue)
        {
            ExtensibilityHelper.BeginCheck(check)
                .ComparingTo(givenValue, "after", "before")
                .FailsIf(sut => sut.CompareTo(givenValue) <= 0, "The {0} is not after the reference value.")
                .Negates("The {0} is after the reference value whereas it must not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Checks that the actual value is equal to zero.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not equal to zero.</exception>
        public static ICheckLink<ICheck<decimal>> IsZero(this ICheck<decimal> check)
        {
            return new NumberCheck<decimal>(check).IsZero();
        }

        /// <summary>
        /// Checks that the actual nullable value has a value and thus, is not null.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is null.</exception>
        public static INullableOrNumberCheckLink<decimal> HasAValue(this ICheck<decimal?> check)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SutNameIs("nullable")
                .FailsIfNull("The {0} has no value, which is unexpected.")
                .Negates("The {0} has a value, whereas it must not.")
                .EndCheck();

            return new NullableOrNumberCheckLink<decimal>(check);
        }

        /// <summary>
        /// Checks that the actual nullable value has no value and thus, is null. 
        /// Note: this method does not return A check link since the nullable is null.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <exception cref="FluentCheckException">The value is not null.</exception>
        public static void HasNoValue(this ICheck<decimal?> check)
        {
            check.Not.HasAValue();
        }

        /// <summary>
        /// Checks that the actual value is NOT equal to zero.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        ///   <returns>A check link.</returns>
        /// </returns>
        /// <exception cref="FluentCheckException">The value is equal to zero.</exception>
        public static ICheckLink<ICheck<decimal>> IsNotZero(this ICheck<decimal> check)
        {
            return check.Not.IsZero();
        }

        /// <summary>
        /// Checks that the actual value is less than an operand.
        /// </summary>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The value is not less than the comparand.
        /// </exception>
        [Obsolete("Use IsStrictlyLessThan instead.")]
        public static ICheckLink<ICheck<decimal>> IsLessThan(this ICheck<decimal> check, decimal comparand)
        {
            return check.Not.IsStrictlyGreaterThan(comparand);
        }

        /// <summary>
        /// Checks that the checked value is strictly less than the comparand.
        /// </summary>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The value is not strictly less than the comparand.
        /// </exception>
        public static ICheckLink<ICheck<decimal>> IsStrictlyLessThan(this ICheck<decimal> check, decimal comparand)
        {
            return new NumberCheck<decimal>(check).IsStrictlyLessThan(comparand);
        }

        /// <summary>
        /// Checks that the actual value is more than an operand.
        /// </summary>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The value is not less than the comparand.
        /// </exception>
        [Obsolete("Use IsStrictlyGreaterThan instead.")]
        public static ICheckLink<ICheck<decimal>> IsGreaterThan(this ICheck<decimal> check, decimal comparand)
        {
            return check.Not.IsStrictlyLessThan(comparand);
        }

        /// <summary>
        /// Checks that the checked value is strictly greater than the comparand.
        /// </summary>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The checked value is not strictly greater than the comparand.
        /// </exception>
        public static ICheckLink<ICheck<decimal>> IsStrictlyGreaterThan(this ICheck<decimal> check, decimal comparand)
        {
            return new NumberCheck<decimal>(check).IsStrictlyGreaterThan(comparand);
        }

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The actual value is not equal to the expected value.
        /// </exception>
        public static ICheckLink<ICheck<decimal>> IsEqualTo(this ICheck<decimal> check, decimal expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return EqualityHelper.PerformEqualCheck(checker, expected);
        }
    }
}
