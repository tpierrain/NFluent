// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IBooleanFluentAssertion.cs" company="">
// //   Copyright 2013 Marc-Antoine LATOUR
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
    /// Provides assertion methods to be executed on a boolean instance.
    /// </summary>
    public interface IBooleanFluentAssertion : IFluentAssertion, IEqualityFluentAssertionTrait<IBooleanFluentAssertion>
    {
        /// <summary>
        /// Checks that the actual value is true.
        /// </summary>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual value is not true.
        /// </exception>
        IChainableFluentAssertion<IBooleanFluentAssertion> IsTrue();

        /// <summary>
        /// Checks that the actual value is false.
        /// </summary>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual value is not false.
        /// </exception>
        IChainableFluentAssertion<IBooleanFluentAssertion> IsFalse();
    }
}