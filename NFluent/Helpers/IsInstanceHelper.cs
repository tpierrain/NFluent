// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentAssert.cs" company="">
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
    using System;

    /// <summary>
    /// Helper class related to Is(Not)Instance methods (used like a traits).
    /// </summary>
    internal class IsInstanceHelper
    {
        /// <summary>
        /// Determines whether an object is an instance of a given type, and throws a <see cref="FluentAssertionException"/> with proper details if not the case.
        /// </summary>
        /// <param name="expectedType">The type we expect the object to be.</param>
        /// <returns>
        ///   <c>true</c> if this object is an instance of this type; otherwise, throws a <see cref="FluentAssertionException"/> with proper details.
        /// </returns>
        /// <exception cref="FluentAssertionException">The object is not an instance of this type.</exception>
        internal static void IsInstanceOf(object sut, Type expectedType)
        {
            if (sut.GetType() != expectedType)
            {
                throw new FluentAssertionException(string.Format("[{0}] is not an instance of the expectedType [{1}] but of [{2}] instead.", sut.ToStringProperlyFormated(), expectedType, sut.GetType()));
            }
        }

        /// <summary>
        /// Verifies that an object is not an instance of a given type, and throws a <see cref="FluentAssertionException"/> with proper details if not the case.
        /// </summary>
        /// <param name="obj">The current object instance to check.</param>
        /// <param name="expectedType">The type we expect the object to be.</param>
        /// <returns>
        ///   <c>true</c> if this object is not an instance of this type; otherwise, throws a <see cref="FluentAssertionException"/> with proper details.
        /// </returns>
        /// <exception cref="FluentAssertionException">The object is an instance of this type (which is not expected).</exception>
        internal static void IsNotInstanceOf(object sut, Type expectedType)
        {
            if (sut.GetType() == expectedType)
            {
                throw new FluentAssertionException(string.Format("[{0}] is an instance of the type [{1}] which is not expected.", sut.ToStringProperlyFormated(), sut.GetType()));
            }
        }
    }
}