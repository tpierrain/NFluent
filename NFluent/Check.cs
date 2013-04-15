// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="Check.cs" company="">
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
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides <see cref="IFluentAssertion{T}"/> instances to be used in order to make 
    /// check(s) on the provided value.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Returns a <see cref="IFluentAssertion{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <typeparam name="T">Type of the value to be tested.</typeparam>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="IFluentAssertion{T}" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="IFluentAssertion{T}" /> instance will throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static IFluentAssertion<T> That<T>(T value)
        {
            return new FluentAssertion<T>(value);
        }

        /// <summary>
        /// Returns a <see cref="IFluentAssertion{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="IFluentAssertion{T}" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="IFluentAssertion{T}" /> instance will throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static LambdaAssertion That(Action value)
        {
            return new LambdaAssertion(value);
        }
    }
}