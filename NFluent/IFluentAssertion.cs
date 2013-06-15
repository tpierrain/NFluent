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
    /// <summary>
    /// Provides assertion methods to be executed on a given value.
    /// </summary>
    /// <typeparam name="T">Type of the value to assert on.</typeparam>
    public interface IFluentAssertion<out T> : IForkableFluentAssertion, INegateableFluentAssertion<IFluentAssertion<T>>
    {
        /// <summary>
        /// Checks whether the specified <see cref="System.Object" /> is equal to this instance or not.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; throws a <see cref="FluentAssertionException"/> otherwise.
        /// </returns>
        /// <exception cref="FluentAssertionException">The specified <see cref="System.Object"/> is not equal to this instance.</exception>
        bool Equals(object obj);
    }
}