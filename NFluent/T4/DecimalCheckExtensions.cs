﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DecimalCheckExtensions.cs" company="">
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

    using NFluent.Extensions;
    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on an <see cref="decimal"/> value.
    /// </summary>
    public static class DecimalCheckExtensions
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------

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
            var checkRunner = check as ICheckRunner<decimal>;
            var runnableCheck = check as IRunnableCheck<decimal>;
            IComparable checkedValue = runnableCheck.Value as IComparable;

            return checkRunner.ExecuteCheck(
                () =>
                {
                    ComparableHelper.IsBefore(checkedValue, givenValue);
                },
                FluentMessage.BuildMessage("The {0} is before the reference value whereas it must not.").On(checkedValue).And.Expected(givenValue).Comparison("after").ToString());
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
        public static ICheckLink<ICheck<decimal>> IsAfter(this ICheck<decimal> check, IComparable givenValue)
        {
            var checkRunner = check as ICheckRunner<decimal>;
            var runnableCheck = check as IRunnableCheck<decimal>;
            IComparable checkedValue = runnableCheck.Value as IComparable;

            return checkRunner.ExecuteCheck(
                () =>
                {
                    ComparableHelper.IsAfter(checkedValue, givenValue);
                },
                FluentMessage.BuildMessage("The {0} is after the reference value whereas it must not.").On(checkedValue).And.Expected(givenValue).Comparison("before").ToString());
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
            var numberCheckStrategy = new NumberCheck<decimal>(check);
            return numberCheckStrategy.IsZero();
        }

        /// <summary>
        /// Checks that the actual nullable value has a value and thus, is not null.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is null.</exception>
        public static INullableOrNumberCheckLink<decimal> HasAValue(this ICheck<decimal?> check)
        {
            var checkRunner = check as ICheckRunner<decimal?>;
            IRunnableCheck<decimal?> runnableCheck = check as IRunnableCheck<decimal?>;

            checkRunner.ExecuteCheck(
                () =>
                {
                    if (runnableCheck.Value == null)
                    {
                        throw new FluentCheckException(string.Format("\nThe checked nullable value has no value, which is unexpected."));
                    }
                },
                string.Format("\nThe checked nullable value:\n\t[{0}]\nhas a value, which is unexpected.", runnableCheck.Value.ToStringProperlyFormated()));

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
            var checkRunner = check as ICheckRunner<decimal?>;
            IRunnableCheck<decimal?> runnableCheck = check as IRunnableCheck<decimal?>;

            checkRunner.ExecuteCheck(
                () =>
                {
                    if (runnableCheck.Value != null)
                    {
                        throw new FluentCheckException(string.Format("\nThe checked nullable value:\n\t[{0}]\nhas a value, which is unexpected.", runnableCheck.Value));
                    }
                },
                "\nThe checked nullable value has no value, which is unexpected.");
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
            var numberCheckStrategy = new NumberCheck<decimal>(check);
            return numberCheckStrategy.IsNotZero();
        }

        /// <summary>
        /// Checks that the actual value is strictly positive.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive.</exception>
        public static ICheckLink<ICheck<decimal>> IsPositive(this ICheck<decimal> check)
        {
            var numberCheckStrategy = new NumberCheck<decimal>(check);
            return numberCheckStrategy.IsPositive();
        }

        /// <summary>
        /// Checks that the actual value is strictly negative.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive.</exception>
        public static ICheckLink<ICheck<decimal>> IsNegative(this ICheck<decimal> check)
        {
            var numberCheckStrategy = new NumberCheck<decimal>(check);
            return numberCheckStrategy.IsNegative();
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
        public static ICheckLink<ICheck<decimal>> IsLessThan(this ICheck<decimal> check, decimal comparand)
        {
            var numberCheckStrategy = new NumberCheck<decimal>(check);
            return numberCheckStrategy.IsLessThan(comparand);
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
        public static ICheckLink<ICheck<decimal>> IsGreaterThan(this ICheck<decimal> check, decimal comparand)
        {
            var numberCheckStrategy = new NumberCheck<decimal>(check);
            return numberCheckStrategy.IsGreaterThan(comparand);
        }
    }
}
