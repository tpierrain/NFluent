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
    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on a number instance.
    /// </summary>
    /// <typeparam name="N">Type of the numerical value.</typeparam>
    public class NumberCheck<N> where N : IComparable
    {
        private const string MustBeZeroMessage = "The {0} is different from zero.";

        // private readonly N value;
        private readonly Checker<N, ICheck<N>> checker;
        private readonly ICheckForExtensibility<N, ICheck<N>> checkForExtensibility;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberCheck{N}" /> class.
        /// </summary>
        /// <param name="check">The fluent check.</param>
        public NumberCheck(ICheck<N> check)
        {
            // this.value = ((ICheckForExtensibility<N>)check).Value;
            this.checker = new Checker<N, ICheck<N>>(check as ICheckForExtensibility<N, ICheck<N>>);
            this.checkForExtensibility = check as ICheckForExtensibility<N, ICheck<N>>;
        }

        /// <summary>
        /// Checks that the actual value is equal to zero.
        /// </summary>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The value is not equal to zero.</exception>
        public ICheckLink<ICheck<N>> IsZero()
        {
            return this.checker.ExecuteCheck(
                () =>
                    {
                        var res = InternalIsZero(this.checkForExtensibility.Value);

                        if (!res)
                        {
                            throw new FluentCheckException(FluentMessage.BuildMessage(MustBeZeroMessage).On(this.checkForExtensibility.Value).ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is equal to zero whereas it must not.").ToString());
        }

        /// <summary>
        /// Checks that the actual value is NOT equal to zero.
        /// </summary>
        /// <returns>
        /// <returns>A check link.</returns>
        /// </returns>
        /// <exception cref="FluentCheckException">The value is equal to zero.</exception>
        public ICheckLink<ICheck<N>> IsNotZero()
        {
            return this.checker.ExecuteCheck(
                () =>
                    {
                        bool res = InternalIsZero(this.checkForExtensibility.Value);

                        if (res)
                        {
                            throw new FluentCheckException(
                                FluentMessage.BuildMessage("The {0} is equal to zero, whereas it must not.")
                                             .On(this.checkForExtensibility.Value)
                                             .ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is different from zero.").On(this.checkForExtensibility.Value).ToString());
        }

        /// <summary>
        /// Checks that the actual value is strictly positive.
        /// </summary>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive.</exception>
        public ICheckLink<ICheck<N>> IsPositive()
        {
            return this.checker.ExecuteCheck(
                () =>
                    {
                        if (Convert.ToInt32(this.checkForExtensibility.Value) <= 0)
                        {
                            throw new FluentCheckException(FluentMessage.BuildMessage("The {0} is not strictly positive.").On(this.checkForExtensibility.Value).ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is positive, whereas it must not.").On(this.checkForExtensibility.Value).ToString());
        }

        /// <summary>
        /// Checks that the actual value is strictly negative.
        /// </summary>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive.</exception>
        public ICheckLink<ICheck<N>> IsNegative()
        {
            return this.checker.ExecuteCheck(
                () =>
                {
                    if (Convert.ToInt32(this.checkForExtensibility.Value) >= 0)
                    {
                        throw new FluentCheckException(FluentMessage.BuildMessage("The {0} is not strictly negative.").On(this.checkForExtensibility.Value).ToString());
                    }
                },
                FluentMessage.BuildMessage("The {0} is negative, whereas it must not.").On(this.checkForExtensibility.Value).ToString());
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
        public ICheckLink<ICheck<N>> IsLessThan(N comparand)
        {
            return this.checker.ExecuteCheck(
                () =>
                {
                    if (this.checkForExtensibility.Value.CompareTo(comparand) >= 0)
                    {
                        throw new FluentCheckException(FluentMessage.BuildMessage("The {0} is greater than the threshold.").On(this.checkForExtensibility.Value).And.Expected(comparand).Comparison("less than").ToString());
                    }
                },
                FluentMessage.BuildMessage("The {0} is less than the threshold.").On(this.checkForExtensibility.Value).And.Expected(comparand).Comparison("more than").ToString());
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
        public ICheckLink<ICheck<N>> IsGreaterThan(N comparand)
        {
            return this.checker.ExecuteCheck(
                () =>
                    {
                        if (this.checkForExtensibility.Value.CompareTo(comparand) <= 0)
                        {
                            throw new FluentCheckException(FluentMessage.BuildMessage("The {0} is less than the threshold.").On(this.checkForExtensibility.Value).And.Expected(comparand).Comparison("more than").ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is greater than the threshold.").On(this.checkForExtensibility.Value).And.Expected(comparand).Comparison("less than").ToString());
        }

        /// <summary>
        /// Checks whether a given value is equal to zero.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is equal to zero; false otherwise.
        /// </returns>
        private static bool InternalIsZero(N value)
        {
            return Convert.ToInt64(value) == 0;
        }
    }
}