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
    using System;
    using System.Collections;

    /// <summary>
    /// Provides a set of static methods, returning all an instance of a specific subtype of 
    /// <see cref="IFluentAssertion"/> to be used in order to make check(s) on the provided 
    /// system under test (sut) instance.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Returns a <see cref="IEnumerableFluentAssertion" /> instance that will provide assertion methods to be executed on the System Under Test enumerable instance.
        /// </summary>
        /// <param name="enumerable">The enumerable instance to be tested.</param>
        /// <returns>
        /// A <see cref="IEnumerableFluentAssertion" /> instance to use in order to assert things on the System Under Test enumerable instance.
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="IEnumerableFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static IEnumerableFluentAssertion That(IEnumerable enumerable)
        {
            return new EnumerableFluentAssertion(enumerable);
        }

        /// <summary>
        /// Returns a <see cref="IStringFluentAssertion" /> instance that will provide assertion methods to be executed on a given string instance.
        /// </summary>
        /// <param name="value">The string instance to be tested.</param>
        /// <returns>
        /// A <see cref="IStringFluentAssertion" /> instance to use in order to assert things on the given string instance.
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="IStringFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static IStringFluentAssertion That(string value)
        {
            return new StringFluentAssertion(value);
        }

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
        public static INumberFluentAssertion That(int value)
        {
            return new NumberFluentAssertion<int>(value);
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
        public static INumberFluentAssertion That(decimal value)
        {
            return new NumberFluentAssertion<decimal>(value);
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

        /// <summary>
        /// Returns a <see cref="IObjectFluentAssertion" /> instance that will provide assertion methods to be executed on a given object.
        /// </summary>
        /// <param name="value">The object to be tested.</param>
        /// <returns>
        /// A <see cref="IObjectFluentAssertion" /> instance to use in order to assert things on the object (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="IObjectFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static IObjectFluentAssertion That(object value)
        {
            return new ObjectFluentAssertion(value);
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="IDateTimeFluentAssertion" /> instance that will provide assertion methods to be executed on a given DateTime instance.
        /// </summary>
        /// <param name="value">The DateTime instance to be tested.</param>
        /// <returns>
        /// A <see cref="IDateTimeFluentAssertion" /> instance to use in order to assert things on the given DateTime instance.
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="IDateTimeFluentAssertion" /> instance will usually throw a <see cref="FluentAssertionException" /> when failing.
        /// </remarks>
        public static IDateTimeFluentAssertion That(DateTime value)
        {
            return new DateTimeFluentAssertion(value);
        }
    }
}