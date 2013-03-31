// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Check.cs" company="">
//   Copyright 2013 Thomas PIERRAIN, Marc-Antoine LATOUR
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace NFluent
{
    /// <summary>
    /// Provides a set of static methods, returning all an instance of a specific subtype of 
    /// <see cref="IFluentAssertion"/> to be used in order to make check(s) on the provided 
    /// system under test (sut) instance.
    /// </summary>
    public static class Check
    {
        #region INumberFluentAssertion related

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertion" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertion" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssertion That(long value)
        {
            return new NumberFluentAssertion<long>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertion" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertion" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssertion That(double value)
        {
            return new NumberFluentAssertion<double>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertion" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertion" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssertion That(float value)
        {
            return new NumberFluentAssertion<float>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertion" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertion" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssertion That(short value)
        {
            return new NumberFluentAssertion<short>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertion" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertion" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssertion That(byte value)
        {
            return new NumberFluentAssertion<byte>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertion" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertion" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssertion That(uint value)
        {
            return new NumberFluentAssertion<uint>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertion" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertion" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssertion That(ushort value)
        {
            return new NumberFluentAssertion<ushort>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertion" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertion" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssertion That(ulong value)
        {
            return new NumberFluentAssertion<ulong>(value);
        }

        #endregion
    }
}