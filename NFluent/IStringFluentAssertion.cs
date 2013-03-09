// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IStringFluentAssertion.cs" company="">
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
    /// Provides assertion methods to be executed on a string instance.
    /// </summary>
    public interface IStringFluentAssertion : IFluentAssertion, IEqualityFluentAssertion, IInstanceTypeFluentAssertion<IStringFluentAssertion>
    {
        /// <summary>
        /// Checks that the string contains the given expected values, in any order.
        /// </summary>
        /// <param name="values">The expected values to be found.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The string does not contains all the given strings in any order.</exception>
        IChainableFluentAssertion<IStringFluentAssertion> Contains(params string[] values);

        /// <summary>
        /// Checks that the string starts with the given expected prefix.
        /// </summary>
        /// <param name="expectedPrefix">The expected prefix.</param>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The string does not start with the expected prefix.</exception>
        IChainableFluentAssertion<IStringFluentAssertion> StartsWith(string expectedPrefix);
    }
}