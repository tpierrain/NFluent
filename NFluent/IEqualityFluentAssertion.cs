// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IEqualityFluentAssertion.cs" company="">
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
    /// <summary>
    /// Provides assertion methods related to the equality of the object instance.
    /// </summary>
    /// <typeparam name="T">Type of the fluent assertion to be chained.</typeparam>
    public interface IEqualityFluentAssertion<T> : IFluentAssertion where T : IFluentAssertion
    {
        // TODO: make IsEqualTo handling a typeparam instead of object if possible.

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        IChainableFluentAssertion<T> IsEqualTo(object expected);

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected.</param>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        IChainableFluentAssertion<T> IsNotEqualTo(object expected);
    }
}