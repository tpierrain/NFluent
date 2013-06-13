// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendableFluentAssertion.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
//   //   Licensed under the Apache License, Version 2.0 (the "License");
//   //   you may not use this file except in compliance with the License.
//   //   You may obtain a copy of the License at
//   //       http://www.apache.org/licenses/LICENSE-2.0
//   //   Unless required by applicable law or agreed to in writing, software
//   //   distributed under the License is distributed on an "AS IS" BASIS,
//   //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   //   See the License for the specific language governing permissions and
//   //   limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent
{
    using System;

    /// <summary>
    /// Provides an specific implementation for IEnumerable Fluent Assertion. Required to implement IEnumerable fluent API.
    /// </summary>
    /// <typeparam name="T">
    /// Type managed by this extension.
    /// </typeparam>
    internal class ExtendableFluentAssertion<T> : ChainableFluentAssertion<IFluentAssertion<T>>, IExtendableFluentAssertion<T>
    {
        private readonly T originalComparand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendableFluentAssertion{T}"/> class. 
        /// </summary>
        /// <param name="previousFluentAssertion">
        /// The previous fluent assertion.
        /// </param>
        /// <param name="originalComparand">
        /// Comparand used for the first check.
        /// </param>
        public ExtendableFluentAssertion(IForkableFluentAssertion previousFluentAssertion, T originalComparand)
            : base(previousFluentAssertion)
        {
            this.originalComparand = originalComparand;
        }

        /// <summary>
        /// Gets the initial list that was used in Contains.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// Temporary to bi completed.
        /// </exception>
        /// <value>
        /// Initial list that was used in Contains.
        /// </value>
        public T OriginalComparand
        {
            get
            {
                return this.originalComparand;
            }
        }
    }
}