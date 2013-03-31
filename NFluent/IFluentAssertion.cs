// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IFluentAssertion.cs" company="">
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
    // TODO: dismantle this useless interface (when spike will be merged)

    /// <summary>
    /// Provides fluent assertion methods to be executed on the system under test (sut).
    /// Every method should return a <see cref="IChainableFluentAssertion{T}"/> instance
    /// of the same fluent assertion type (closure of operations), or throw an 
    /// <see cref="FluentAssertionException"/> when failing.
    /// This 'marker' interface is mandatory for the chainable assertion mechanism. 
    /// </summary>
    public interface IFluentAssertion
    {
    }
}