// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IFluentAssertion.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN, Rui CARVALHO
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
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides assertion methods to be executed on a given value.
    /// </summary>
    /// <typeparam name="T">Type of the value to assert on.</typeparam>
    public interface IFluentAssertion<out T> : IForkableFluentAssertion
    {
        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent assertion extension method.
        /// </value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        T Value { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="FluentAssertion{T}" /> should be negated or not. 
        /// Property only useful for those that write assertion methods.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all the methods applying to this assertion instance should be negated; <c>false</c> otherwise.
        /// </value>
        bool Negated { get;  }

        /// <summary>
        /// Negates the next assertion.
        /// </summary>
        /// <value>
        /// The next assertion negated.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        IFluentAssertion<T> Not { get; }
    }
}