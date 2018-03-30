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
        protected FluentCheck(T value, bool negated)
        {
            this.Value = value;
            this.Negated = negated;
            this.checker = new Checker<T, ICheck<T>>(this);
        }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public bool Negated { get; private set; }

        /// <inheritdoc />
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public ICheck<T> Not => new FluentCheck<T>(this.Value, !this.Negated);

        /// <inheritdoc />
        public T Value { get; }

        /// <inheritdoc />
        public IChecker<T, ICheck<T>> Checker => this.checker;

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
            this.IsEqualTo(obj);
 
            return true;
        }

        /// <inheritdoc />
        public ICheckLink<ICheck<T>> IsInstanceOf<TU>()
        {
            return this.IsInstanceOfType(typeof(TU));
        }

        /// <inheritdoc />
        public ICheckLink<ICheck<T>> IsNotInstanceOf<TU>()
        {
            return this.IsNoInstanceOfType(typeof(TU));
        }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc />
        object IForkableCheck.ForkInstance()
        {
            this.Negated = !CheckContext.DefaulNegated;
            return this; 
        }

        #endregion
    }
}