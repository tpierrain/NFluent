// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsInstanceHelper.cs" company="">
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
namespace NFluent.Helpers
{
    using System;

    using NFluent.Extensions;

    /// <summary>
    /// Helper class related to Is(Not)Instance methods (used like a traits).
    /// </summary>
    internal static class IsInstanceHelper
    {
        /// <summary>
        /// Checks that an instance is of the given expected type.
        /// </summary>
        /// <param name="instance">The instance to be checked.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <exception cref="FluentAssertionException">The instance is not of the expected type.</exception>
        public static void IsInstanceOf(object instance, Type expectedType)
        {
            if (instance.GetType() != expectedType)
            {
                throw new FluentAssertionException(string.Format("[{0}] is not an instance of the expected type [{1}] but of [{2}] instead.", instance.ToStringProperlyFormated(), expectedType, instance.GetType()));
            }
        }

        /// <summary>
        /// Checks that an instance is not of the given expected type.
        /// </summary>
        /// <param name="instance">The instance to be checked.</param>
        /// <param name="typeNotExpected">The type not expected.</param>
        /// <exception cref="FluentAssertionException">The instance is of the type not expected.</exception>
        public static void IsNotInstanceOf(object instance, Type typeNotExpected)
        {
            if (instance.GetType() == typeNotExpected)
            {
                throw new FluentAssertionException(string.Format("[{0}] is an instance of the type [{1}] which is not expected.", instance.ToStringProperlyFormated(), instance.GetType()));
            }
        }
    }
}