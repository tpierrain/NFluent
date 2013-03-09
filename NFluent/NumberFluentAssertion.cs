// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="NumberFluentAssertion.cs" company="">
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
    /// Provides assertion methods to be executed on a number instance.
    /// </summary>
    /// <typeparam name="N">Type of the numerical value.</typeparam>
    public class NumberFluentAssertion<N> : INumberFluentAssertion
    {
        private readonly N number;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberFluentAssertion{N}" /> class.
        /// </summary>
        /// <param name="number">The number to assert on.</param>
        public NumberFluentAssertion(N number)
        {
            this.number = number;
        }

        /// <summary>
        /// Verifies that the actual value is equal to zero.
        /// </summary>
        /// <exception cref="FluentAssertionException">The value is not equal to zero.</exception>
        public void IsZero()
        {
            var res = InternalIsZero(this.number);

            if (!res)
            {
                throw new FluentAssertionException(string.Format("[{0}] is not equal to zero.", this.number));
            }
        }

        /// <summary>
        /// Verifies that the actual value is NOT equal to zero.
        /// </summary>
        /// <exception cref="FluentAssertionException">The value is equal to zero.</exception>
        public void IsNotZero()
        {
            bool res = InternalIsZero(this.number);

            if (res)
            {
                throw new FluentAssertionException(string.Format("[{0}] is equal to zero.", this.number));
            }
        }

        /// <summary>
        /// Verifies that the actual value is strictly positive.
        /// </summary>
        /// <exception cref="FluentAssertionException">The value is not strictly positive.</exception>
        public void IsPositive()
        {
            if (Convert.ToInt32(this.number) <= 0)
            {
                throw new FluentAssertionException(string.Format("[{0}] is not a strictly positive value.", this.number));
            }
        }

        #region IInstanceTypeFluentAssertion members

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public IChainableFluentAssertion<INumberFluentAssertion> IsInstanceOf<T>()
        {
            IsInstanceHelper.IsInstanceOf(this.number, typeof(T));
            return new ChainableFluentAssertion<INumberFluentAssertion>(this);
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        public IChainableFluentAssertion<INumberFluentAssertion> IsNotInstanceOf<T>()
        {
            IsInstanceHelper.IsNotInstanceOf(this.number, typeof(T));
            return new ChainableFluentAssertion<INumberFluentAssertion>(this);
        }

        #endregion

        /// <summary>
        /// Checks whether a given value is equal to zero.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is equal to zero; false otherwise.
        /// </returns>
        private static bool InternalIsZero(N value)
        {
            return Convert.ToInt64(value) == 0;
        }
    }
}