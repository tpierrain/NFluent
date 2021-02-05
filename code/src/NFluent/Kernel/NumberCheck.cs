// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="NumberCheck.cs" company="">
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
namespace NFluent.Kernel
{
    using System;
    using Extensibility;

    /// <summary>
    /// Provides check methods to be executed on a number instance.
    /// </summary>
    /// <typeparam name="TN">Type of the numerical value.</typeparam>
    public class NumberCheck<TN> where TN : IComparable
    {
        private readonly ICheck<TN> check;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberCheck{TN}" /> class.
        /// </summary>
        /// <param name="check">The fluent check.</param>
        public NumberCheck(ICheck<TN> check)
        {
            this.check = check;
        }

        /// <summary>
        /// Checks that the actual value is equal to zero.
        /// </summary>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not equal to zero.</exception>
        public ICheckLink<ICheck<TN>> IsZero()
        {
            ExtensibilityHelper.BeginCheck(this.check)
                .FailWhen(sut => !InternalIsZero(sut), "The {0} is different from zero.")
                .OnNegate("The {0} is equal to zero whereas it must not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(this.check);
        }

        /// <summary>
        /// Checks that the actual value is strictly positive (i.e. greater than zero).
        /// </summary>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive (i.e. greater than zero).</exception>
        public ICheckLink<ICheck<TN>> IsStrictlyPositive()
        {
            ExtensibilityHelper.BeginCheck(this.check)
                .FailWhen(sut => Convert.ToDouble(sut) <=0, "The {0} is not strictly positive (i.e. greater than zero).")
                .OnNegate("The {0} is strictly positive (i.e. greater than zero), whereas it must not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(this.check);
        }

        /// <summary>
        /// Checks that the actual value is positive or equal to zero.
        /// </summary>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is not positive or equal to zero.</exception>
        public ICheckLink<ICheck<TN>> IsPositiveOrZero()
        {
            ExtensibilityHelper.BeginCheck(this.check)
                .FailWhen(sut => Convert.ToDouble(sut) <0, "The {0} is not positive or equal to zero.")
                .OnNegate("The {0} is positive or equal to zero, whereas it must not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(this.check);
        }

        /// <summary>
        /// Checks that the actual value is strictly negative.
        /// </summary>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive.</exception>
        public ICheckLink<ICheck<TN>> IsStrictlyNegative()
        {
            ExtensibilityHelper.BeginCheck(this.check)
                .FailWhen(sut => Convert.ToDouble(sut) >=0, "The {0} is not strictly negative.")
                .OnNegate("The {0} is strictly negative, whereas it must not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(this.check);
        }

        /// <summary>
        /// Checks that the actual value is negative or equal to zero.
        /// </summary>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is not negative or equal to zero.</exception>
        public ICheckLink<ICheck<TN>> IsNegativeOrZero()
        {
            ExtensibilityHelper.BeginCheck(this.check)
                .FailWhen(sut => Convert.ToDouble(sut) >0, "The {0} is not negative or equal to zero.")
                .OnNegate("The {0} is negative or equal to zero, whereas it must not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(this.check);
        }


        /// <summary>
        /// Checks that the checked value is strictly less than the comparand.
        /// </summary>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The checked value is not strictly less than the comparand.
        /// </exception>
        public ICheckLink<ICheck<TN>> IsStrictlyLessThan(TN comparand)
        {
            ExtensibilityHelper.BeginCheck(this.check)
                .ComparingTo(comparand, "strictly less than", "greater than")
                .FailWhen(sut => sut.CompareTo(comparand) > 0, "The {0} is greater than the {1}.")
                .FailWhen(sut => sut.CompareTo(comparand) == 0, "The {0} is equal to the {1}.")
                .OnNegate("The {0} is strictly less than the {1}.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(this.check);
        }

        /// <summary>
        /// Checks that the checked value is strictly less than the comparand.
        /// </summary>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The checked value is not strictly less than the comparand.
        /// </exception>
        public ICheckLink<ICheck<TN>> IsLessOrEqualThan(TN comparand)
        {
            ExtensibilityHelper.BeginCheck(check)
                .ComparingTo(comparand, "less than", "greater than")
                .FailWhen(sut => sut.CompareTo(comparand) > 0, "The {0} is greater than the {1}.")
                .OnNegateWhen( sut => sut.CompareTo(comparand) == 0, "The {0} is equal to the {1} whereas it must be greater.")
                .OnNegate("The {0} is less than the {1} whereas it must be greater.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(this.check);
        }

        /// <summary>
        /// Checks that the actual value is more than a comparand.
        /// </summary>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The value is not less than the comparand.
        /// </exception>
        public ICheckLink<ICheck<TN>> IsStrictlyGreaterThan(TN comparand)
        {
            ExtensibilityHelper.BeginCheck(this.check)
                .ComparingTo(comparand, "strictly greater than", "less than or equal to")
                .FailWhen(sut => sut.CompareTo(comparand) < 0, "The {0} is less than the {1}.")
                .FailWhen(sut => sut.CompareTo(comparand) == 0, "The {0} is equal to the {1}.")
                .OnNegate("The {0} is greater than the {1}.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(this.check);
        }

        /// <summary>
        /// Checks that the actual value is more than a comparand.
        /// </summary>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The value is not less than the comparand.
        /// </exception>
        public ICheckLink<ICheck<TN>> IsGreaterOrEqualThan(TN comparand)
        {
            ExtensibilityHelper.BeginCheck(this.check)
                .ComparingTo(comparand, "greater than", "less than")
                .FailWhen(sut => sut.CompareTo(comparand) < 0, "The {0} is less than the {1}.")
                .OnNegateWhen(sut => sut.CompareTo(comparand) == 0, "The {0} is equal to the {1} whereas it must not.")
                .OnNegate("The {0} is greater than the {1}.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(this.check);
        }

        /// <summary>
        /// Checks whether a given value is equal to zero.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is equal to zero; false otherwise.
        /// </returns>
        private static bool InternalIsZero(TN value)
        {
            return Convert.ToDecimal(value) == 0;
        }
    }
}