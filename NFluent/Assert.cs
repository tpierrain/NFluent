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
        /// Returns a <see cref="IEnumerableFluentAssert"/> instance that will provide assertion methods to be executed on the System Under Test enumerable instance.
        /// </summary>
        /// <remarks>
        ///     Methods of the returned <see cref="IEnumerableFluentAssert"/> instance will usually throw a <see cref="T:FluentAssertionException"/> when failing.
        /// </remarks>
        /// <param name="enumerable">The System Under Test enumerable instance.</param>
        /// <returns>A <see cref="IEnumerableFluentAssert"/> instance to use in order to assert things on the System Under Test enumerable instance.</returns>
        public static IEnumerableFluentAssert That(IEnumerable enumerable)
        {
            return new EnumerableFluentAssert(enumerable);
        }

        /// <summary>
        /// Returns a <see cref="IStringFluentAssert"/> instance that will provide assertion methods to be executed on the System Under Test string instance.
        /// </summary>
        /// <remarks>
        ///     Methods of the returned <see cref="IStringFluentAssert"/> instance will usually throw a <see cref="T:FluentAssertionException"/> when failing.
        /// </remarks>
        /// <param name="value">The System Under Test string instance.</param>
        /// <returns>A <see cref="IStringFluentAssert"/> instance to use in order to assert things on the System Under Test string instance.</returns>
        public static IStringFluentAssert That(string value)
        {
            return new StringFluentAssert(value);
        }
    }
}

