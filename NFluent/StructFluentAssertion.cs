// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StructFluentAssertion.cs" company="">
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

    /// <summary>
    /// Provides assertion methods to be executed on a given struct value.
    /// </summary>
    /// <typeparam name="T">Type of the struct value to assert on.</typeparam>
    public class StructFluentAssertion<T> : IStructFluentAssertion<T>, IFluentAssertionRunner<T>, IRunnableAssertion where T : struct
    {
        private FluentAssertionRunner<T> fluentAssertionRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructFluentAssertion{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public StructFluentAssertion(T value) : this(value, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructFluentAssertion{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="negated">A boolean value indicating whether the assertion should be negated or not.</param>
        private StructFluentAssertion(T value, bool negated)
        {
            this.Value = value;
            this.Negated = negated;
            this.fluentAssertionRunner = new FluentAssertionRunner<T>(this);
        }

        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent assertion extension method.
        /// </value>
        public T Value { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="FluentAssertion{T}" /> should be negated or not.
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
        public IStructFluentAssertion<T> Not
        {
            get
            {
                bool negated = true;
                return new StructFluentAssertion<T>(this.Value, negated);
            }
        }

        /// <summary>
        /// Executes the assertion provided as an happy-path lambda (vs lambda for negated version).
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="negatedExceptionMessage">The message for the negated exception.</param>
        /// <returns>
        /// A new chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The assertion fails.</exception>
        IChainableFluentAssertion<IFluentAssertion<T>> IFluentAssertionRunner<T>.ExecuteAssertion(Action action, string negatedExceptionMessage)
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
        object IForkableFluentAssertion.ForkInstance()
        {
            return new StructFluentAssertion<T>(this.Value);
        }
    }
}