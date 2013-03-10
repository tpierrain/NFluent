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
    using NFluent.Helpers;

    /// <summary>
    /// Provides assertion methods to be executed on an object instance.
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

        #region IEqualityFluentAssertion members

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public void IsEqualTo(object expected)
        {
            EqualityHelper.IsEqualTo(this.sut, expected);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public void IsNotEqualTo(object expected)
        {
            EqualityHelper.IsNotEqualTo(this.sut, expected);
        }

        #endregion

        #region IInstanceTypeFluentAssertion members

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public IChainableFluentAssertion<IObjectFluentAssertion> IsInstanceOf<T>()
        {
            IsInstanceHelper.IsInstanceOf(this.sut, typeof(T));
            return new ChainableFluentAssertion<IObjectFluentAssertion>(this);
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        public IChainableFluentAssertion<IObjectFluentAssertion> IsNotInstanceOf<T>()
        {
            IsInstanceHelper.IsNotInstanceOf(this.sut, typeof(T));
            return new ChainableFluentAssertion<IObjectFluentAssertion>(this);
        }

        #endregion
    }
}