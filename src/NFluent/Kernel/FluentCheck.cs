// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentCheck.cs" company="">
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
namespace NFluent.Kernel
{
    using System.Diagnostics.CodeAnalysis;
    using Extensibility;
    using Extensions;
    using Helpers;

    /// <summary>
    /// Provides fluent check methods to be executed on a given value.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the value to assert on.
    /// </typeparam>
    internal class FluentCheck<T> : IForkableCheck, ICheck<T>, ICheckForExtensibility<T, ICheck<T>>
    {
        #region Fields

        /// <summary>
        /// The check runner.
        /// </summary>
        private readonly Checker<T, ICheck<T>> checker;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCheck{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public FluentCheck(T value) : this(value, !CheckContext.DefaulNegated)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCheck{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="negated">
        /// A boolean value indicating whether the check should be negated or not.
        /// </param>
        private FluentCheck(T value, bool negated)
        {
            this.Value = value;
            this.Negated = negated;
            this.checker = new Checker<T, ICheck<T>>(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether this <see cref="FluentCheck{T}"/> should be negated or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if all the methods applying to this check instance should be negated; <c>false</c> otherwise.
        /// </value>
        public bool Negated { get; private set; }

        /// <summary>
        /// Negates the next check.
        /// </summary>
        /// <value>
        /// The next check negated.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public ICheck<T> Not
        {
            get
            {
                return new FluentCheck<T>(this.Value, CheckContext.DefaulNegated);
            }
        }

        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent check extension method.
        /// </value>
        public T Value { get; private set; }

        /// <summary>
        /// Gets the runner to use for checking something on a given type.
        /// </summary>
        /// <value>
        /// The runner to use for checking something on a given type.
        /// </value>
        public IChecker<T, ICheck<T>> Checker
        {
            get
            {
                return this.checker;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Checks whether the specified <see cref="System.Object"/> is equal to this instance or not.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; throws a <see cref="FluentCheckException"/> otherwise.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The specified <see cref="System.Object"/> is not equal to this instance.
        /// </exception>
        public new bool Equals(object obj)
        {
            this.checker.ExecuteCheck(() => EqualityHelper.IsEqualTo(this.checker, obj), EqualityHelper.BuildErrorMessage(this.checker, obj, true, false));
 
            return true;
        }

        /// <summary>
        /// Checks whether if the checked value is of the given type.
        /// </summary>
        /// <typeparam name="TU">The given type to check the checked value against.</typeparam>
        /// <returns>A chainable check.</returns>
        /// <exception cref="FluentCheckException">The specified value is null (and not of the same nullable type) or not of the given type.</exception>
        public ICheckLink<ICheck<T>> IsInstanceOf<TU>()
        {
            if (typeof(T).IsNullable())
            {
                return this.checker.ExecuteCheck(
                    () => IsInstanceHelper.IsSameType(typeof(T), typeof(TU), this.Value), 
                    IsInstanceHelper.BuildErrorMessageForNullable(typeof(T), typeof(TU), this.Value, true));
            }

            return this.checker.ExecuteCheck(() => IsInstanceHelper.IsInstanceOf(this.Value, typeof(TU)), IsInstanceHelper.BuildErrorMessage(this.Value, typeof(TU), true));
        }

        /// <summary>
        /// Checks whether if the checked value is different from the given type.
        /// </summary>
        /// <typeparam name="TU">The given type to check the checked value against.</typeparam>
        /// <returns>A chainable check.</returns>
        /// <exception cref="FluentCheckException">The specified value is of the given type.</exception>
        public ICheckLink<ICheck<T>> IsNotInstanceOf<TU>()
        {
            if (typeof(T).IsNullable())
            {
                return this.checker.ExecuteCheck(
                    () => IsInstanceHelper.IsDifferentType(typeof(T), typeof(TU), this.Value),
                    IsInstanceHelper.BuildErrorMessageForNullable(typeof(T), typeof(TU), this.Value, false));
            }

            return this.checker.ExecuteCheck(
                () => IsInstanceHelper.IsNotInstanceOf(this.Value, typeof(TU)), IsInstanceHelper.BuildErrorMessage(this.Value, typeof(TU), false));
        }

        #endregion

        #region Explicit Interface Methods

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

        #endregion
    }
}