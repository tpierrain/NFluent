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
    using System.Diagnostics.CodeAnalysis;
    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on a number instance.
    /// </summary>
    /// <typeparam name="N">Type of the numerical value.</typeparam>
    public class NumberCheck<N> : ICheck<N>, IRunnableCheck<N>, ICheckRunner<N> where N : IComparable
    {
        private const string MustBeZeroMessage = "The {0} is different from zero.";

        private readonly ICheck<N> check;
        private readonly CheckRunner<N> checkRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberCheck{N}" /> class.
        /// </summary>
        /// <param name="check">The fluent check.</param>
        public NumberCheck(ICheck<N> check)
        {
            this.check = check;
            this.checkRunner = new CheckRunner<N>(this);
        }

        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent check extension method.
        /// </value>
        public N Value 
        { 
            get
            {
                return ((IRunnableCheck<N>)this.check).Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="FluentCheck{T}" /> should be negated or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all the methods applying to this check instance should be negated; <c>false</c> otherwise.
        /// </value>
        public bool Negated { get; private set; }
        
        /// <summary>
        /// Negates the next check.
        /// </summary>
        /// <value>
        /// The next check negated.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public ICheck<N> Not { get; private set; }

        /// <summary>
        /// Executes the check provided as an happy-path lambda (vs lambda for negated version).
        /// </summary>
        /// <param name="action">The happy-path action (vs. the one for negated version which has not to be specified). This lambda should simply return if everything is ok, or throws a <see cref="FluentCheckException"/> otherwise.</param>
        /// <param name="negatedExceptionMessage">The message for the negated exception.</param>
        /// <returns>
        /// A new chainable fluent check.
        /// </returns>
        /// <exception cref="FluentCheckException">The check fails.</exception>
        ICheckLink<ICheck<N>> ICheckRunner<N>.ExecuteCheck(Action action, string negatedExceptionMessage)
        {
            return this.checkRunner.ExecuteCheck(action, negatedExceptionMessage);
        }

        /// <summary>
        /// Creates a new instance of the same fluent check type, injecting the same Value property
        /// (i.e. the system under test), but with a false Negated property in any case.
        /// </summary>
        /// <returns>
        /// A new instance of the same fluent check type, with the same Value property.
        /// </returns>
        /// <remarks>
        /// This method is used during the chaining of multiple checks.
        /// </remarks>
        public object ForkInstance()
        {
            return new NumberCheck<N>(this.check);
        }

        /// <summary>
        /// Checks whether the specified <see cref="System.Object" /> is equal to this instance or not.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; throws a <see cref="FluentCheckException"/> otherwise.
        /// </returns>
        /// <exception cref="FluentCheckException">The specified <see cref="System.Object"/> is not equal to this instance.</exception>
        public new bool Equals(object obj)
        {
            var checkRunner = this as ICheckRunner<N>;

            checkRunner.ExecuteCheck(
                () => EqualityHelper.IsEqualTo(this.Value, obj),
                EqualityHelper.BuildErrorMessage(this.Value, obj, true));

            return true;
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
            var checkRunner = this.check as ICheckRunner<N>;
            IRunnableCheck<N> runnableCheck = this;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        var res = InternalIsZero(runnableCheck.Value);

                        if (!res)
                        {
                            throw new FluentCheckException(FluentMessage.BuildMessage(MustBeZeroMessage).On(runnableCheck.Value).ToString());
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
            var checkRunner = this.check as ICheckRunner<N>;
            IRunnableCheck<N> runnableCheck = this;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        bool res = InternalIsZero(runnableCheck.Value);

                        if (res)
                        {
                            throw new FluentCheckException(
                                FluentMessage.BuildMessage("The {0} is equal to zero, whereas it must not.")
                                             .On(runnableCheck.Value)
                                             .ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is different from zero.").On(runnableCheck.Value).ToString());
        }

        /// <summary>
        /// Checks that the actual value is strictly positive.
        /// </summary>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">The value is not strictly positive.</exception>
        public ICheckLink<ICheck<N>> IsPositive()
        {
            var checkRunner = this.check as ICheckRunner<N>;
            IRunnableCheck<N> runnableCheck = this;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        if (Convert.ToInt32(runnableCheck.Value) <= 0)
                        {
                            throw new FluentCheckException(FluentMessage.BuildMessage("The {0} is not strictly positive.").On(runnableCheck.Value).ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is positive, whereas it must not.").On(runnableCheck.Value).ToString());
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
            var checkRunner = this.check as ICheckRunner<N>;
            IRunnableCheck<N> runnableCheck = this;

            return checkRunner.ExecuteCheck(
                () =>
                {
                    if (runnableCheck.Value.CompareTo(comparand) >= 0)
                    {
                        throw new FluentCheckException(FluentMessage.BuildMessage("The {0} is greater than the threshold.").On(runnableCheck.Value).And.Expected(comparand).Comparison("less than").ToString());
                    }
                },
                FluentMessage.BuildMessage("The {0} is less than the threshold.").On(runnableCheck.Value).And.Expected(comparand).Comparison("more than").ToString());
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
            var checkRunner = this.check as ICheckRunner<N>;
            IRunnableCheck<N> runnableCheck = this;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        if (runnableCheck.Value.CompareTo(comparand) <= 0)
                        {
                            throw new FluentCheckException(FluentMessage.BuildMessage("The {0} is less than the threshold.").On(runnableCheck.Value).And.Expected(comparand).Comparison("more than").ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is greater than the threshold.").On(runnableCheck.Value).And.Expected(comparand).Comparison("less than").ToString());
        }

        #region IEqualityFluentAssertion members

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <returns>A check link.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public ICheckLink<ICheck<N>> IsEqualTo(object expected)
        {
            var checkRunner = this.check as ICheckRunner<N>;
            IRunnableCheck<N> runnableCheck = this;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        EqualityHelper.IsEqualTo(runnableCheck.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expected, true));
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <returns>A check link.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentCheckException">The actual value is equal to the expected value.</exception>
        public ICheckLink<ICheck<N>> IsNotEqualTo(object expected)
        {
            var checkRunner = this.check as ICheckRunner<N>;
            IRunnableCheck<N> runnableCheck = this;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        EqualityHelper.IsNotEqualTo(runnableCheck.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expected, false));
        }

        #endregion

        #region IInstanceTypeFluentAssertion members

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <returns>
        /// A chainable fluent check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual instance is not of the provided type.</exception>
        public ICheckLink<ICheck<N>> IsInstanceOf<T>()
        {
            var checkRunner = this.check as ICheckRunner<N>;
            var runnableCheck = this;

            return checkRunner.ExecuteCheck(
                () =>
                {
                    IsInstanceHelper.IsInstanceOf(runnableCheck.Value, typeof(T));
                },
                IsInstanceHelper.BuildErrorMessage(runnableCheck, typeof(T), true));
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <returns>
        /// A chainable fluent check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual instance is of the provided type.</exception>
        public ICheckLink<ICheck<N>> IsNotInstanceOf<T>()
        {
            var checkRunner = this.check as ICheckRunner<N>;
            var runnableCheck = this;

            return checkRunner.ExecuteCheck(
                () =>
                {
                    IsInstanceHelper.IsNotInstanceOf(runnableCheck.Value, typeof(T));
                },
                IsInstanceHelper.BuildErrorMessage(runnableCheck, typeof(T), false));
        }

        #endregion

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