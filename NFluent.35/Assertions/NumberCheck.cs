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
namespace NFluent
{
    using System;

    using NFluent.Extensibility;

    /// <summary>
    /// Provides check methods to be executed on a number instance.
    /// </summary>
    /// <typeparam name="TN">Type of the numerical value.</typeparam>
    public class NumberCheck<TN> where TN : IComparable
    {
        private const string MustBeZeroMessage = "The {0} is different from zero.";

        // private readonly N value;
        private readonly Checker<TN, ICheck<TN>> checker;
        private readonly ICheckForExtensibility<TN, ICheck<TN>> checkForExtensibility;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberCheck{TN}" /> class.
        /// </summary>
        /// <param name="check">The fluent check.</param>
        public NumberCheck(ICheck<TN> check)
        {
            // this.value = ((ICheckForExtensibility<N>)check).Value;
            this.checker = new Checker<TN, ICheck<TN>>(check as ICheckForExtensibility<TN, ICheck<TN>>);
            this.checkForExtensibility = check as ICheckForExtensibility<TN, ICheck<TN>>;
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
            return this.checker.ExecuteCheck(
                () =>
                    {
                        var res = InternalIsZero(this.checker.Value);

                        if (!res)
                        {
                            throw new FluentCheckException(checker.BuildMessage(MustBeZeroMessage).ToString());
                        }
                    },
                this.checker.BuildShortMessage("The {0} is equal to zero whereas it must not.").ToString());
        }

        /// <summary>
        /// Checks that the actual value is NOT equal to zero.
        /// </summary>
        /// <returns>
        /// <returns>A check link.</returns>
        /// </returns>
        /// <exception cref="FluentCheckException">The value is equal to zero.</exception>
        public ICheckLink<ICheck<TN>> IsNotZero()
        {
            return this.checker.ExecuteCheck(
                () =>
                    {
                        bool res = InternalIsZero(this.checker.Value);

                        if (res)
                        {
                            throw new FluentCheckException(checker.BuildMessage("The {0} is equal to zero, whereas it must not.").ToString());
                        }
                    },
                this.checker.BuildMessage("The {0} is different from zero.").ToString());
        }

        /// <summary>
        /// Checks that the actual value is strictly positive (i.e. greater than zero).
        /// </summary>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive (i.e. greater than zero).</exception>
        public ICheckLink<ICheck<TN>> IsStrictlyPositive()
        {
            return this.checker.ExecuteCheck(
                () =>
                    {
                        if (Convert.ToDouble(this.checker.Value) <= 0)
                        {
                            throw new FluentCheckException(
                                this.checker.BuildMessage("The {0} is not strictly positive (i.e. greater than zero).").ToString());
                        }
                    },
                this.checker.BuildMessage("The {0} is strictly positive (i.e. greater than zero), whereas it must not.").ToString());
        }

        /// <summary>
        /// Checks that the actual value is strictly negative.
        /// </summary>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive.</exception>
        public ICheckLink<ICheck<TN>> IsLessThanZero()
        {
            return this.checker.ExecuteCheck(
                () =>
                {
                    if (Convert.ToDouble(this.checker.Value) >= 0)
                    {
                        throw new FluentCheckException(checker.BuildMessage("The {0} is not less than zero.").ToString());
                    }
                },
                this.checker.BuildMessage("The {0} is less than zero, whereas it must not.").ToString());
        }

        /// <summary>
        /// Checks that the actual value is less than a comparand.
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
        public ICheckLink<ICheck<TN>> IsLessThan(TN comparand)
        {
            return this.checker.ExecuteCheck(
                () =>
                {
                    if (this.checker.Value.CompareTo(comparand) >= 0)
                    {
                        throw new FluentCheckException(checker.BuildMessage("The {0} is greater than the threshold.").Expected(comparand).Comparison("less than").ToString());
                    }
                },
                this.checker.BuildMessage("The {0} is less than the threshold.").Expected(comparand).Comparison("more than").ToString());
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
        public ICheckLink<ICheck<TN>> IsGreaterThan(TN comparand)
        {
            return this.checker.ExecuteCheck(
                () =>
                    {
                        if (this.checker.Value.CompareTo(comparand) <= 0)
                        {
                            throw new FluentCheckException(checker.BuildMessage("The {0} is less than the threshold.").Expected(comparand).Comparison("more than").ToString());
                        }
                    },
                this.checker.BuildMessage("The {0} is greater than the threshold.").Expected(comparand).Comparison("less than").ToString());
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
            return Convert.ToInt64(value) == 0;
        }
    }
}