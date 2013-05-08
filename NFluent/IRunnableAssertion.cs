// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IRunnableAssertion.cs" company="">
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
    using System.ComponentModel;

    /// <summary>
    /// Fluent assertion which is runnable by a <see cref="IFluentAssertionRunner{T}"/>.
    /// </summary>
    public interface IRunnableAssertion<out T> : IForkableFluentAssertion, INegated
    {
        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent assertion extension method.
        /// </value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        T Value { get; }
    }
}