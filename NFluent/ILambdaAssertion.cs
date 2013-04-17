// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ILambdaAssertion.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
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
    using NFluent.Helpers;

    /// <summary>
    /// Implements lambda/action specific assertion.
    /// </summary>
    public interface ILambdaAssertion : IFluentAssertionBase
    {
        /// <summary>
        /// Checks that the execution time is below a specified threshold.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="timeUnit">
        /// The time Unit.
        /// </param>
        /// <exception cref="FluentAssertionException">
        /// When execution is strictly above limit.
        /// </exception>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        IChainableFluentAssertion<LambdaAssertion> LastsLessThan(double value, TimeUnit timeUnit);

        /// <summary>
        /// Check that the code does not throw.
        /// </summary>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// When the code raises an exception.
        /// </exception>
        IChainableFluentAssertion<LambdaAssertion> DoesNotThrow();

        /// <summary>
        /// Checks if the code did throw an exception.
        /// </summary>
        /// <typeparam name="T">
        /// Expected exception type.
        /// </typeparam>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// Code did not raised an exception or not of the expected type.
        /// </exception>
        IChainableFluentAssertion<LambdaAssertion> Throws<T>();

        /// <summary>
        /// Checks if the code did throw an exception.
        /// </summary>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// Code did not raised an exception or not of the expected type.
        /// </exception>
        IChainableFluentAssertion<LambdaAssertion> ThrowsAny();
    }
}