﻿// // --------------------------------------------------------------------------------------------------------------------
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
    public class NumberFluentAssertion<N> : IFluentAssertion<N>
        where N : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberFluentAssertion{N}" /> class.
        /// </summary>
        /// <param name="number">The number to assert on.</param>
        public NumberFluentAssertion(N number)
        {
            this.Value = number;
        }

        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent assertion extension method.
        /// </value>
        public N Value { get; private set; }

        /// <summary>
        /// Checks that the actual value is equal to zero.
        /// </summary>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is not equal to zero.</exception>
        public IChainableFluentAssertion<IFluentAssertion<N>> IsZero()
        {
            var res = InternalIsZero(this.Value);

            if (!res)
            {
                throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]{1}\nis not equal to zero.", this.Value, EqualityHelper.BuildTypeDescriptionMessage(this.Value)));
            }

            return new ChainableFluentAssertion<IFluentAssertion<N>>(this);
        }

        /// <summary>
        /// Checks that the actual value is NOT equal to zero.
        /// </summary>
        /// <returns>
        /// <returns>A chainable assertion.</returns>
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is equal to zero.</exception>
        public IChainableFluentAssertion<IFluentAssertion<N>> IsNotZero()
        {
            bool res = InternalIsZero(this.Value);

            if (res)
            {
                throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]{1}\nis equal to zero.", this.Value, EqualityHelper.BuildTypeDescriptionMessage(this.Value)));
            }

            return new ChainableFluentAssertion<IFluentAssertion<N>>(this);
        }

        /// <summary>
        /// Checks that the actual value is strictly positive.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <exception cref="FluentAssertionException">The value is not strictly positive.</exception>
        public IChainableFluentAssertion<IFluentAssertion<N>> IsPositive()
        {
            if (Convert.ToInt32(this.Value) <= 0)
            {
                throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]{1}\nis not a strictly positive value.", this.Value, EqualityHelper.BuildTypeDescriptionMessage(this.Value)));
            }

            return new ChainableFluentAssertion<IFluentAssertion<N>>(this);
        }

        /// <summary>
        /// Checks that the actual value is less than a comparand.
        /// </summary>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The value is not less than the comparand.
        /// </exception>
        public IChainableFluentAssertion<IFluentAssertion<N>> IsLessThan(N comparand)
        {
            if (this.Value.CompareTo(comparand) >= 0)
            {
                throw new FluentAssertionException(string.Format("[{0}] is not less than {1}.", this.Value, comparand));
            }

            return new ChainableFluentAssertion<IFluentAssertion<N>>(this);
        }

        /// <summary>
        /// Checks that the actual value is more than a comparand.
        /// </summary>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The value is not less than the comparand.
        /// </exception>
        public IChainableFluentAssertion<IFluentAssertion<N>> IsGreaterThan(N comparand)
        {
            if (this.Value.CompareTo(comparand) <= 0)
            {
                throw new FluentAssertionException(string.Format("[{0}] is not greater than {1}.", this.Value, comparand));
            }

            return new ChainableFluentAssertion<IFluentAssertion<N>>(this);
        }

        #region IEqualityFluentAssertion members

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public IChainableFluentAssertion<IFluentAssertion<N>> IsEqualTo(object expected)
        {
            EqualityHelper.IsEqualTo(this.Value, expected);
            return new ChainableFluentAssertion<IFluentAssertion<N>>(this);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <returns>A chainable assertion.</returns>
        /// <param name="expected">The expected value.</param>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public IChainableFluentAssertion<IFluentAssertion<N>> IsNotEqualTo(object expected)
        {
            EqualityHelper.IsNotEqualTo(this.Value, expected);
            return new ChainableFluentAssertion<IFluentAssertion<N>>(this);
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
        public IChainableFluentAssertion<IFluentAssertion<N>> IsInstanceOf<T>()
        {
            IsInstanceHelper.IsInstanceOf(this.Value, typeof(T));
            return new ChainableFluentAssertion<IFluentAssertion<N>>(this);
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        public IChainableFluentAssertion<IFluentAssertion<N>> IsNotInstanceOf<T>()
        {
            IsInstanceHelper.IsNotInstanceOf(this.Value, typeof(T));
            return new ChainableFluentAssertion<IFluentAssertion<N>>(this);
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