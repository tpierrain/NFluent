// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="NumberFluentAssert.cs" company="">
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
    /// Provides assertion methods to be executed on the numerical System Under Test (SUT) instance.
    /// </summary>
    /// <typeparam name="T">Type of the numerical value.</typeparam>
    public class NumberFluentAssert<T> : INumberFluentAssert
    {
        private readonly T sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberFluentAssert{T}" /> class.
        /// </summary>
        /// <param name="sut">The System Under Test.</param>
        public NumberFluentAssert(T sut)
        {
            this.sut = sut;
        }

        /// <summary>
        /// Verifies that the actual value is equal to zero.
        /// </summary>
        /// <exception cref="FluentAssertionException">The value is not equal to zero.</exception>
        public void IsZero()
        {
            var res = InternalIsZero(this.sut);

            if (!res)
            {
                throw new FluentAssertionException(string.Format("[{0}] is not equal to zero.", this.sut));
            }
        }

        /// <summary>
        /// Verifies that the actual value is NOT equal to zero.
        /// </summary>
        /// <exception cref="FluentAssertionException">The value is equal to zero.</exception>
        public void IsNotZero()
        {
            bool res = InternalIsZero(this.sut);

            if (res)
            {
                throw new FluentAssertionException(string.Format("[{0}] is equal to zero.", this.sut));
            }
        }

        /// <summary>
        /// Verifies that the actual value is strictly positive.
        /// </summary>
        /// <exception cref="FluentAssertionException">The value is not strictly positive.</exception>
        public void IsPositive()
        {
            if (Convert.ToInt32(this.sut) <= 0)
            {
                throw new FluentAssertionException(string.Format("[{0}] is not a strictly positive value.", this.sut));
            }
        }

        #region IFluentAssert members

        /// <summary>
        /// Checks that the actual is an instance of the given expected type.
        /// </summary>
        /// <param name="expectedType">The expected type.</param>
        /// <exception cref="FluentAssertionException">The actual instance is not of the expected type.</exception>
        public void IsInstanceOf(Type expectedType)
        {
            IsInstanceHelper.IsInstanceOf(this.sut, expectedType);
        }

        /// <summary>
        /// Checks that the actual instance is an instance of the given expected type.
        /// </summary>
        /// <typeparam name="U">The expected Type of the instance.</typeparam>
        /// <exception cref="FluentAssertionException">The actual instance is not of the expected type.</exception>
        public void IsInstanceOf<U>()
        {
            IsInstanceHelper.IsInstanceOf(this.sut, typeof(U));
        }

        /// <summary>
        /// Checks that the actual is not an instance of the given expected type.
        /// </summary>
        /// <param name="expectedType">The expected type.</param>
        /// <exception cref="FluentAssertionException">The actual instance is of the expected type.</exception>
        public void IsNotInstanceOf(Type expectedType)
        {
            IsInstanceHelper.IsNotInstanceOf(this.sut, expectedType);
        }

        #endregion

        /// <summary>
        /// Checks whether a given value is equal to zero.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is equal to zero; false otherwise.
        /// </returns>
        private static bool InternalIsZero(T value)
        {
            return Convert.ToInt64(value) == 0;
        }
    }
}