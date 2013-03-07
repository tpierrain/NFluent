// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Assert.cs" company="">
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
    /// Provides a set of static methods that return fluent objects to be used then to assert things on a System Under Test (SUT) instance. 
    /// </summary>
    public static class Assert
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

        #region INumericalFluentAssert related

        /// <summary>
        /// Returns a <see cref="INumericalFluentAssert{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumericalFluentAssert{T}" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumericalFluentAssert{T}" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumericalFluentAssert That(int value)
        {
            return new NumericalFluentAssert<int>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumericalFluentAssert{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumericalFluentAssert{T}" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumericalFluentAssert{T}" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumericalFluentAssert That(long value)
        {
            return new NumericalFluentAssert<long>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumericalFluentAssert{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumericalFluentAssert{T}" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumericalFluentAssert{T}" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumericalFluentAssert That(double value)
        {
            return new NumericalFluentAssert<double>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumericalFluentAssert{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumericalFluentAssert{T}" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumericalFluentAssert{T}" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumericalFluentAssert That(decimal value)
        {
            return new NumericalFluentAssert<decimal>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumericalFluentAssert{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumericalFluentAssert{T}" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumericalFluentAssert{T}" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumericalFluentAssert That(float value)
        {
            return new NumericalFluentAssert<float>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumericalFluentAssert{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumericalFluentAssert{T}" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumericalFluentAssert{T}" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumericalFluentAssert That(short value)
        {
            return new NumericalFluentAssert<short>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumericalFluentAssert{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumericalFluentAssert{T}" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumericalFluentAssert{T}" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumericalFluentAssert That(byte value)
        {
            return new NumericalFluentAssert<byte>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumericalFluentAssert{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumericalFluentAssert{T}" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumericalFluentAssert{T}" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumericalFluentAssert That(uint value)
        {
            return new NumericalFluentAssert<uint>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumericalFluentAssert{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumericalFluentAssert{T}" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumericalFluentAssert{T}" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumericalFluentAssert That(ushort value)
        {
            return new NumericalFluentAssert<ushort>(value);
        }

        /// <summary>
        /// Returns a <see cref="INumericalFluentAssert{T}" /> instance that will provide assertion methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="INumericalFluentAssert{T}" /> instance to use in order to assert things on the value (System Under Test).
        /// </returns>
        /// <remarks>
        /// Methods of the returned <see cref="INumericalFluentAssert{T}" /> instance will usually throw a <see cref="T:FluentAssertionException" /> when failing.
        /// </remarks>
        public static INumericalFluentAssert That(ulong value)
        {
            return new NumericalFluentAssert<ulong>(value);
        }

        public static IFluentAssert That(object value)
        {
            return new FluentAssert(value);
        }

        #endregion
    }
}