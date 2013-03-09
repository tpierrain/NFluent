// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StringFluentAssert.cs" company="">
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
    using System.Collections.Generic;

    using NFluent.Helpers;

    /// <summary>
    /// Provides assertion methods to be executed on the string System Under Test (SUT) instance.
    /// </summary>
    internal class StringFluentAssert : IStringFluentAssert
    {
        private readonly string sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringFluentAssert" /> class.
        /// </summary>
        /// <param name="value">The String Under Test.</param>
        public StringFluentAssert(string value)
        {
            this.sut = value;
        }

       #region IEqualityFluentAssert members

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
        /// Checks that the actual instance is an instance of the given expected type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the expected type.</exception>
        public IChainableFluentAssert<IStringFluentAssert> IsInstanceOf<T>()
        {
            IsInstanceHelper.IsInstanceOf(this.sut, typeof(T));
            return new ChainableFluentAssert<IStringFluentAssert>(this);
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given expected type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <returns>A chainable fluent assertion.</returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the expected type.</exception>
        public IChainableFluentAssert<IStringFluentAssert> IsNotInstanceOf<T>()
        {
            IsInstanceHelper.IsNotInstanceOf(this.sut, typeof(T));
            return new ChainableFluentAssert<IStringFluentAssert>(this);
        }

        #endregion

        #region IStringFluentAssert members

        /// <summary>
        /// Checks that the string contains the given expected values, in any order.
        /// </summary>
        /// <param name="values">The expected values to be found.</param>
        /// <exception cref="FluentAssertionException">The string does not contains all the given strings in any order.</exception>
        public IChainableFluentAssert<IStringFluentAssert> Contains(params string[] values)
        {
            var notFound = new List<string>();
            foreach (string value in values)
            {
                if (!this.sut.Contains(value))
                {
                    notFound.Add(value);
                }
            }

            if (notFound.Count > 0)
            {
                throw new FluentAssertionException(
                    string.Format(
                        @"The string [""{0}""] does not contain the expected value(s): [{1}].",
                        this.sut,
                        notFound.ToEnumeratedString()));
            }

            return new ChainableFluentAssert<IStringFluentAssert>(this);
        }

        /// <summary>
        /// Checks that the string starts with the given expected prefix.
        /// </summary>
        /// <param name="expectedPrefix">The expected prefix.</param>
        /// <exception cref="FluentAssertionException">The string does not start with the expected prefix.</exception>
        public IChainableFluentAssert<IStringFluentAssert> StartsWith(string expectedPrefix)
        {
            if (!this.sut.StartsWith(expectedPrefix))
            {
                throw new FluentAssertionException(
                    string.Format(@"The string [""{0}""] does not start with [""{1}""].", this.sut, expectedPrefix));
            }

            return new ChainableFluentAssert<IStringFluentAssert>(this);
        }

        #endregion
    }
}