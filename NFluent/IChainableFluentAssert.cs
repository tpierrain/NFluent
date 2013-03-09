// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IChainableFluentAssert.cs" company="">
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
    /// Allows to chain two <see cref="IFluentAssertion"/> instances. 
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="IFluentAssertion"/> to be chained.</typeparam>
    public interface IChainableFluentAssert<T> : IFluentAssertion where T : IFluentAssertion
    {
        T And { get; }
    }

    /// <summary>
    /// Represents a fluent assertion (marker interface).
    /// </summary>
    public interface IFluentAssertion
    {
    }

    public interface IInstanceTypeFluentAssertion<FAT> : IFluentAssertion where FAT : IFluentAssertion
    {
        /// <summary>
        /// Checks that the actual instance is an instance of the given expected type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <returns>A chainable fluent assertion.</returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the expected type.</exception>
        IChainableFluentAssert<FAT> IsInstanceOf<T>();

        /// <summary>
        /// Checks that the actual instance is not an instance of the given expected type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <returns>A chainable fluent assertion.</returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the expected type.</exception>
        IChainableFluentAssert<FAT> IsNotInstanceOf<T>();
    }
}