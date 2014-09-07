// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FluentStructCheck.cs" company="">
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
    using System.Diagnostics.CodeAnalysis;

    using NFluent.Extensibility;
    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on a given struct value.
    /// </summary>
    /// <typeparam name="T">Type of the struct value to assert on.</typeparam>
    public sealed class FluentStructCheck<T> : IForkableCheck, IStructCheck<T>, ICheckForExtensibility<T, IStructCheck<T>> where T : struct
    {
        private readonly Checker<T, IStructCheck<T>> structChecker;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentStructCheck{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public FluentStructCheck(T value) : this(value, !CheckContext.DefaulNegated)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentStructCheck{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="negated">A boolean value indicating whether the check should be negated or not.</param>
        private FluentStructCheck(T value, bool negated)
        {
            this.Value = value;
            this.Negated = negated;
            this.structChecker = new Checker<T, IStructCheck<T>>(this);
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
                bool negated = CheckContext.DefaulNegated;
                return new FluentStructCheck<T>(this.Value, negated);
            }
        }

        /// <summary>
        /// Gets the runner to use for checking something on a given type.
        /// </summary>
        /// <value>
        /// The runner to use for checking something on a given type.
        /// </value>
        public IChecker<T, IStructCheck<T>> Checker
        {
            get
            {
                return this.structChecker;
            }
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
            this.Negated = !CheckContext.DefaulNegated;
            return this;
        }

        /// <summary>
        /// Checks whether if the checked value is of the given type.
        /// </summary>
        /// <typeparam name="U">The given type to check the checked value against.</typeparam>
        /// <returns>A chainable check.</returns>
        /// <exception cref="FluentCheckException">The specified value is not of the given type.</exception>
        public ICheckLink<IStructCheck<T>> IsInstanceOf<U>() where U : struct
        {
            return this.structChecker.ExecuteCheck(
                () => IsInstanceHelper.IsInstanceOf(this.Value, typeof(U)), 
                IsInstanceHelper.BuildErrorMessage(this.Value, typeof(U), true));
        }

        /// <summary>
        /// Checks whether if the checked value is different from the given type.
        /// </summary>
        /// <typeparam name="U">The given type to check the checked value against.</typeparam>
        /// <returns>A chainable check.</returns>
        /// <exception cref="FluentCheckException">The specified value is of the given type.</exception>
        public ICheckLink<IStructCheck<T>> IsNotInstanceOf<U>() where U : struct
        {
            return this.structChecker.ExecuteCheck(
                () => IsInstanceHelper.IsNotInstanceOf(this.Value, typeof(U)),
                IsInstanceHelper.BuildErrorMessage(this.Value, typeof(U), false));
        }
    }
}