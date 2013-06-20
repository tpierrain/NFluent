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
    /// Provides assertion methods to be executed on a number instance.
    /// </summary>
    /// <typeparam name="N">Type of the numerical value.</typeparam>
    public class NumberCheck<N> : ICheck<N>, IRunnableAssertion<N>, IFluentAssertionRunner<N> where N : IComparable
    {
        private const string MustBeZeroMessage = "The {0} is different from zero.";

        private readonly ICheck<N> check;
        private readonly FluentAssertionRunner<N> fluentAssertionRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberCheck{N}" /> class.
        /// </summary>
        /// <param name="check">The fluent assertion.</param>
        public NumberCheck(ICheck<N> check)
        {
            this.check = check;
            this.fluentAssertionRunner = new FluentAssertionRunner<N>(this);
        }

        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent assertion extension method.
        /// </value>
        public N Value 
        { 
            get
            {
                return ((IRunnableAssertion<N>)this.check).Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CheckImpl{T}" /> should be negated or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all the methods applying to this assertion instance should be negated; <c>false</c> otherwise.
        /// </value>
        public bool Negated { get; private set; }
        
        /// <summary>
        /// Negates the next assertion.
        /// </summary>
        /// <value>
        /// The next assertion negated.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public ICheck<N> Not { get; private set; }

        /// <summary>
        /// Executes the assertion provided as an happy-path lambda (vs lambda for negated version).
        /// </summary>
        /// <param name="action">The happy-path action (vs. the one for negated version which has not to be specified). This lambda should simply return if everything is ok, or throws a <see cref="FluentAssertionException"/> otherwise.</param>
        /// <param name="negatedExceptionMessage">The message for the negated exception.</param>
        /// <returns>
        /// A new chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The assertion fails.</exception>
        IChainableFluentAssertion<ICheck<N>> IFluentAssertionRunner<N>.ExecuteAssertion(Action action, string negatedExceptionMessage)
        {
            return this.fluentAssertionRunner.ExecuteAssertion(action, negatedExceptionMessage);
        }

        /// <summary>
        /// Creates a new instance of the same fluent assertion type, injecting the same Value property
        /// (i.e. the system under test), but with a false Negated property in any case.
        /// </summary>
        /// <returns>
        /// A new instance of the same fluent assertion type, with the same Value property.
        /// </returns>
        /// <remarks>
        /// This method is used during the chaining of multiple assertions.
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
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; throws a <see cref="FluentAssertionException"/> otherwise.
        /// </returns>
        /// <exception cref="FluentAssertionException">The specified <see cref="System.Object"/> is not equal to this instance.</exception>
        public new bool Equals(object obj)
        {
            var assertionRunner = this as IFluentAssertionRunner<N>;

            assertionRunner.ExecuteAssertion(
                () => EqualityHelper.IsEqualTo(this.Value, obj),
                EqualityHelper.BuildErrorMessage(this.Value, obj, true));

            return true;
        }

        /// <summary>
        /// Checks that the actual value is equal to zero.
        /// </summary>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is not equal to zero.</exception>
        public IChainableFluentAssertion<ICheck<N>> IsZero()
        {
            var assertionRunner = this.check as IFluentAssertionRunner<N>;
            IRunnableAssertion<N> runnableAssertion = this;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        var res = InternalIsZero(runnableAssertion.Value);

                        if (!res)
                        {
                            throw new FluentAssertionException(FluentMessage.BuildMessage(MustBeZeroMessage).On(runnableAssertion.Value).ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is equal to zero whereas it must not.").ToString());
        }

        /// <summary>
        /// Checks that the actual value is NOT equal to zero.
        /// </summary>
        /// <returns>
        /// <returns>A chainable assertion.</returns>
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is equal to zero.</exception>
        public IChainableFluentAssertion<ICheck<N>> IsNotZero()
        {
            var assertionRunner = this.check as IFluentAssertionRunner<N>;
            IRunnableAssertion<N> runnableAssertion = this;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        bool res = InternalIsZero(runnableAssertion.Value);

                        if (res)
                        {
                            throw new FluentAssertionException(
                                FluentMessage.BuildMessage("The {0} is equal to zero, whereas it must not.")
                                             .On(runnableAssertion.Value)
                                             .ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is different from zero.").On(runnableAssertion.Value).ToString());
        }

        /// <summary>
        /// Checks that the actual value is strictly positive.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The value is not strictly positive.</exception>
        public IChainableFluentAssertion<ICheck<N>> IsPositive()
        {
            var assertionRunner = this.check as IFluentAssertionRunner<N>;
            IRunnableAssertion<N> runnableAssertion = this;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        if (Convert.ToInt32(runnableAssertion.Value) <= 0)
                        {
                            throw new FluentAssertionException(FluentMessage.BuildMessage("The {0} is not strictly positive.").On(runnableAssertion.Value).ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is positive, whereas it must not.").On(runnableAssertion.Value).ToString());
        }

        /// <summary>
        /// Checks that the actual value is less than a comparand.
        /// </summary>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The value is not less than the comparand.
        /// </exception>
        public IChainableFluentAssertion<ICheck<N>> IsLessThan(N comparand)
        {
            var assertionRunner = this.check as IFluentAssertionRunner<N>;
            IRunnableAssertion<N> runnableAssertion = this;

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    if (runnableAssertion.Value.CompareTo(comparand) >= 0)
                    {
                        throw new FluentAssertionException(FluentMessage.BuildMessage("The {0} is greater than the threshold.").On(runnableAssertion.Value).And.Expected(comparand).Comparison("less than").ToString());
                    }
                },
                FluentMessage.BuildMessage("The {0} is less than the threshold.").On(runnableAssertion.Value).And.Expected(comparand).Comparison("more than").ToString());
        }

        /// <summary>
        /// Checks that the actual value is more than a comparand.
        /// </summary>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The value is not less than the comparand.
        /// </exception>
        public IChainableFluentAssertion<ICheck<N>> IsGreaterThan(N comparand)
        {
            var assertionRunner = this.check as IFluentAssertionRunner<N>;
            IRunnableAssertion<N> runnableAssertion = this;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        if (runnableAssertion.Value.CompareTo(comparand) <= 0)
                        {
                            throw new FluentAssertionException(FluentMessage.BuildMessage("The {0} is less than the threshold.").On(runnableAssertion.Value).And.Expected(comparand).Comparison("more than").ToString());
                        }
                    },
                FluentMessage.BuildMessage("The {0} is greater than the threshold.").On(runnableAssertion.Value).And.Expected(comparand).Comparison("less than").ToString());
        }

        #region IEqualityFluentAssertion members

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public IChainableFluentAssertion<ICheck<N>> IsEqualTo(object expected)
        {
            var assertionRunner = this.check as IFluentAssertionRunner<N>;
            IRunnableAssertion<N> runnableAssertion = this;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        EqualityHelper.IsEqualTo(runnableAssertion.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableAssertion.Value, expected, true));
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public IChainableFluentAssertion<ICheck<N>> IsNotEqualTo(object expected)
        {
            var assertionRunner = this.check as IFluentAssertionRunner<N>;
            IRunnableAssertion<N> runnableAssertion = this;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        EqualityHelper.IsNotEqualTo(runnableAssertion.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableAssertion.Value, expected, false));
        }

        #endregion

        #region IInstanceTypeFluentAssertion members

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public IChainableFluentAssertion<ICheck<N>> IsInstanceOf<T>()
        {
            var assertionRunner = this.check as IFluentAssertionRunner<N>;
            var runnableAssertion = this;

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    IsInstanceHelper.IsInstanceOf(runnableAssertion.Value, typeof(T));
                },
                IsInstanceHelper.BuildErrorMessage(runnableAssertion, typeof(T), true));
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        public IChainableFluentAssertion<ICheck<N>> IsNotInstanceOf<T>()
        {
            var assertionRunner = this.check as IFluentAssertionRunner<N>;
            var runnableAssertion = this;

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    IsInstanceHelper.IsNotInstanceOf(runnableAssertion.Value, typeof(T));
                },
                IsInstanceHelper.BuildErrorMessage(runnableAssertion, typeof(T), false));
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