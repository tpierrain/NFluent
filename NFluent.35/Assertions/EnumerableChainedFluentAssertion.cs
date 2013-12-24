// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableChainedFluentAssertion.cs" company="">
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
    using System.Collections;

    /// <summary>
    /// Provides an specific implementation for IEnumerable Fluent Assertion. Required to implement IEnumerable fluent API.
    /// </summary>
    public class EnumerableChainedFluentAssertion : ChainableFluentAssertion<IFluentAssertion<IEnumerable>>
    {
        private readonly IEnumerable originalComparand;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerableChainedFluentAssertion"/> class. 
        /// </summary>
        /// <param name="previousFluentAssertion">
        /// The previous fluent assert.
        /// </param>
        /// <param name="originalComparand">
        /// Comparand used for the first check.
        /// </param>
        public EnumerableChainedFluentAssertion(IFluentAssertion<IEnumerable> previousFluentAssertion, IEnumerable originalComparand)
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
        public IEnumerable OriginalComparand
        {
            get
            {
                return this.originalComparand;
            }
        }
    }
}