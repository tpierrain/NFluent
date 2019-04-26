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

    /// <summary>
    /// Provides fluent check methods to be executed on a given value.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the value to assert on.
    /// </typeparam>
    internal class FluentCheck<T> : FluentSut<T>, IForkableCheck, ICheck<T>, ICheckForExtensibility<T, ICheck<T>>
    {
        /// <summary>
        /// The check runner.
        /// </summary>
        private readonly Checker<T, ICheck<T>> checker;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCheck{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public FluentCheck(T value) : this(value, !CheckContext.DefaultNegated)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCheck{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="reporter"></param>
        /// <param name="negated"></param>
        private FluentCheck(T value, IErrorReporter reporter, bool negated) : base(value, reporter, negated)
        {
            this.checker = new Checker<T, ICheck<T>>(this, this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCheck{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="reporter">error reporter</param>
        public FluentCheck(T value, IErrorReporter reporter) : this(value, reporter, !CheckContext.DefaultNegated)
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
        protected FluentCheck(T value, bool negated) : this(value, Check.Reporter, negated)
        {
        }

        internal FluentCheck(FluentSut<T> copy, bool negated): base(copy, negated)
        {
            this.checker = new Checker<T, ICheck<T>>(this, this);
        }

         /// <inheritdoc />
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public ICheck<T> Not => new FluentCheck<T>(this, !Negated);

         /// <inheritdoc />
        public IChecker<T, ICheck<T>> Checker => this.checker;

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
            return false;
        }

        /// <inheritdoc />
        public ICheckLinkWhich<ICheck<T>, ICheck<TU>> IsInstanceOf<TU>()
        {
            this.IsInstanceOfType(typeof(TU));

            // TODO: restore pattern matching version when appveyor is upgraded
            if (Value is TU)
            {
                return ExtensibilityHelper.BuildCheckLinkWhich(this, (TU) (object) Value, SutName);
            }
            return ExtensibilityHelper.BuildCheckLinkWhich(this, default(TU), SutName);
        }

        /// <inheritdoc />
        public ICheckLink<ICheck<T>> IsNotInstanceOf<TU>()
        {
            return this.IsNoInstanceOfType(typeof(TU));
        }

        /// <inheritdoc />
        object IForkableCheck.ForkInstance()
        {
            Negated = !CheckContext.DefaultNegated;
            return this; 
        }
    }
}