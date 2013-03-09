// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Check.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
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
    using System.Collections;

    /// <summary>
    /// Provides a set of static methods that always return an <see cref="IFluentAssert"/> instance to be used in order to make a check on the provided System Under Test (SUT) instance. 
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Returns a <see cref="IEnumerableFluentAssert" /> instance that will provide assertion methods to be executed on the System Under Test enumerable instance.
        /// </summary>
        /// <param name="enumerable">The enumerable instance to be tested.</param>
        /// <returns>
        /// A <see cref="IEnumerableFluentAssert" /> instance to use in order to assert things on the System Under Test enumerable instance.
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="IEnumerableFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static IEnumerableFluentAssert That(IEnumerable enumerable)
        {
            return new EnumerableFluentAssert(enumerable);
        }

        /// <summary>
        /// Returns a <see cref="IStringFluentAssert" /> instance that will provide assertion methods to be executed on a given string instance.
        /// </summary>
        /// <param name="value">The string instance to be tested.</param>
        /// <returns>
        /// A <see cref="IStringFluentAssert" /> instance to use in order to assert things on the given string instance.
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="IStringFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static IStringFluentAssert That(string value)
        {
            return new StringFluentAssert(value);
        }

        #region INumberFluentAssert related

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertberFluentAssert" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertberFluentAssert" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertberFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssert That(int value)
        {
            return new NumberFluentAssert<int>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertberFluentAssert" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertberFluentAssert" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertberFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssert That(long value)
        {
            return new NumberFluentAssert<long>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertberFluentAssert" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertberFluentAssert" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertberFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssert That(double value)
        {
            return new NumberFluentAssert<double>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertberFluentAssert" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertberFluentAssert" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertberFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssert That(decimal value)
        {
            return new NumberFluentAssert<decimal>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertberFluentAssert" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertberFluentAssert" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertberFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssert That(float value)
        {
            return new NumberFluentAssert<float>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertberFluentAssert" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertberFluentAssert" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertberFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssert That(short value)
        {
            return new NumberFluentAssert<short>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertberFluentAssert" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertberFluentAssert" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertberFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssert That(byte value)
        {
            return new NumberFluentAssert<byte>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertberFluentAssert" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertberFluentAssert" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertberFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssert That(uint value)
        {
            return new NumberFluentAssert<uint>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertberFluentAssert" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertberFluentAssert" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertberFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssert That(ushort value)
        {
            return new NumberFluentAssert<ushort>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumberFluentAssertberFluentAssert" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumberFluentAssertberFluentAssert" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumberFluentAssertberFluentAssert" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumberFluentAssert That(ulong value)
        {
            return new NumberFluentAssert<ulong>(value);
        }

        public static IFluentAssertion That(object value)
        {
            return new FluentAssertion(value);
        }

        #endregion
    }
}