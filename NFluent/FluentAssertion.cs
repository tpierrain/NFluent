// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FluentAssertion.cs" company="">
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

    /// <summary>
    /// Provides assertion methods to be executed on a given value.
    /// </summary>
    /// <typeparam name="T">Type of the value to assert on.</typeparam>
    public class FluentAssertion<T> : IFluentAssertion<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentAssertion{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public FluentAssertion(T value) : this(value, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentAssertion{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="negated">A boolean value indicating whether the assertion should be negated or not.</param>
        private FluentAssertion(T value, bool negated)
        {
            this.Value = value;
            this.Negated = negated;
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
        public bool Negated { get; private set;  }

        /// <summary>
        /// Negates the next assertion.
        /// </summary>
        /// <value>
        /// The next assertion negated.
        /// </value>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public IFluentAssertion<T> Not 
        { 
            get
            {
                bool negated = true;
                return new FluentAssertion<T>(this.Value, negated);
            }
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
            return new FluentAssertion<T>(this.Value);
        }
    }
}
