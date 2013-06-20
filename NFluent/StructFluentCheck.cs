// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StructFluentCheck.cs" company="">
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
    /// Provides check methods to be executed on a given struct value.
    /// </summary>
    /// <typeparam name="T">Type of the struct value to assert on.</typeparam>
    public class StructFluentCheck<T> : IStructCheck<T>, IStructCheckRunner<T>, IRunnableCheck<T> where T : struct
    {
        private StructCheckRunner<T> checkRunner;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructFluentCheck{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public StructFluentCheck(T value) : this(value, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructFluentCheck{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="negated">A boolean value indicating whether the check should be negated or not.</param>
        private StructFluentCheck(T value, bool negated)
        {
            this.Value = value;
            this.Negated = negated;
            this.checkRunner = new StructCheckRunner<T>(this);
        }

        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent check extension method.
        /// </value>
        public T Value { get; private set; }

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
        public IStructCheck<T> Not
        {
            get
            {
                bool negated = true;
                return new StructFluentCheck<T>(this.Value, negated);
            }
        }

        /// <summary>
        /// Executes the check provided as an happy-path lambda (vs lambda for negated version).
        /// </summary>
        /// <param name="action">The happy-path action (vs. the one for negated version which has not to be specified). This lambda should simply return if everything is ok, or throws a <see cref="FluentCheckException"/> otherwise.</param>
        /// <param name="negatedExceptionMessage">The message for the negated exception.</param>
        /// <returns>
        /// A new chainable fluent check for struct or enum.
        /// </returns>
        /// <exception cref="FluentCheckException">The check fails.</exception>
        ICheckLink<IStructCheck<T>> IStructCheckRunner<T>.ExecuteCheck(Action action, string negatedExceptionMessage)
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
        object IForkableCheck.ForkInstance()
        {
            return new StructFluentCheck<T>(this.Value);
        }
    }
}