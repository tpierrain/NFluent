// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExtendableFluentAssertion.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
//   //   //   Licensed under the Apache License, Version 2.0 (the "License");
//   //   //   you may not use this file except in compliance with the License.
//   //   //   You may obtain a copy of the License at
//   //   //       http://www.apache.org/licenses/LICENSE-2.0
//   //   //   Unless required by applicable law or agreed to in writing, software
//   //   //   distributed under the License is distributed on an "AS IS" BASIS,
//   //   //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   //   //   See the License for the specific language governing permissions and
//   //   //   limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent
{
    /// <summary>
    /// Provides an specific implementation for IEnumerable Fluent Assertion extension. Required to implement IEnumerable fluent API syntax.
    /// </summary>
    /// <typeparam name="T">
    /// Type managed by this extension.
    /// </typeparam>
    public interface IExtendableFluentAssertion<out T> : IChainableFluentAssertion<ICheck<T>>
    {
        /// <summary>
        /// Gets the initial list that was used in Contains.
        /// </summary>
        /// <value>
        /// Initial list that was used in Contains.
        /// </value>
        T OriginalComparand { get; }
    }

    /// <summary>
    /// Provides an specific implementation for IEnumerable Fluent Assertion extension. Required to implement IEnumerable fluent API syntax.
    /// </summary>
    /// <typeparam name="T">Type managed by this extension.</typeparam>
    /// <typeparam name="U">Type of the reference comparand.</typeparam>
    public interface IExtendableFluentAssertion<out T, out U> : IChainableFluentAssertion<ICheck<T>>
    {
        /// <summary>
        /// Gets the initial list that was used in Contains.
        /// </summary>
        /// <value>
        /// Initial list that was used in Contains.
        /// </value>
        U OriginalComparand { get; }
    }
}