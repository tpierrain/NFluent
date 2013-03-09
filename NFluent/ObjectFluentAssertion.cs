// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ObjectFluentAssertion.cs" company="">
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
    using System;

    using NFluent.Helpers;

    /// <summary>
    /// Provides core assertion methods to be executed on the System Under Test (SUT) instance.
    /// </summary>
    public class ObjectFluentAssertion : IObjectFluentAssertion
    {
        private readonly object sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectFluentAssertion" /> class.
        /// </summary>
        /// <param name="sut">The System Under Test.</param>
        public ObjectFluentAssertion(object sut)
        {
            this.sut = sut;
        }

        public IChainableFluentAssert<IObjectFluentAssertion> IsInstanceOf<T>()
        {
            IsInstanceHelper.IsInstanceOf(this.sut, typeof(T));
            return new ChainableFluentAssert<IObjectFluentAssertion>(this);
        }

        public IChainableFluentAssert<IObjectFluentAssertion> IsNotInstanceOf<T>()
        {
            IsInstanceHelper.IsNotInstanceOf(this.sut, typeof(T));
            return new ChainableFluentAssert<IObjectFluentAssertion>(this);
        }
    }
}