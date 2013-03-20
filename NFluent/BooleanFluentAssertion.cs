// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="BooleanFluentAssertion.cs" company="">
// //   Copyright 2013 Marc-Antoine LATOUR
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
    using NFluent.Extensions;
    using NFluent.Helpers;

    /// <summary>
    /// Provides assertion methods to be executed on a boolean instance.
    /// </summary>
    public class BooleanFluentAssertion : IBooleanFluentAssertion
    {
        private readonly bool booleanUnderTest;

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanFluentAssertion"/> class. 
        /// </summary>
        /// <param name="booleanUnderTest">
        /// The boolean under test.
        /// </param>
        public BooleanFluentAssertion(bool booleanUnderTest)
        {
            this.booleanUnderTest = booleanUnderTest;
        }

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public IChainableFluentAssertion<IBooleanFluentAssertion> IsEqualTo(object expected)
        {
            EqualityHelper.IsEqualTo(this.booleanUnderTest, expected);
            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected.</param>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public IChainableFluentAssertion<IBooleanFluentAssertion> IsNotEqualTo(object expected)
        {
            EqualityHelper.IsNotEqualTo(this.booleanUnderTest, expected);
            return this.ChainFluentAssertion();
        }

        /// <summary>
        /// Checks that the actual value is true.
        /// </summary>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual value is not true.
        /// </exception>
        public IChainableFluentAssertion<IBooleanFluentAssertion> IsTrue()
        {
            if (this.booleanUnderTest)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual boolean value [{0}] is not true", this.booleanUnderTest.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual value is false.
        /// </summary>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The actual value is not false.
        /// </exception>
        public IChainableFluentAssertion<IBooleanFluentAssertion> IsFalse()
        {
            if (this.booleanUnderTest == false)
            {
                return this.ChainFluentAssertion();
            }

            throw new FluentAssertionException(string.Format("The actual boolean value [{0}] is not false", this.booleanUnderTest.ToStringProperlyFormated()));
        }

        private IChainableFluentAssertion<IBooleanFluentAssertion> ChainFluentAssertion()
        {
            return new ChainableFluentAssertion<IBooleanFluentAssertion>(this);
        }
    }
}