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
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Contains a collection of static methods that implement the only assertions you need to be fluent (if your favorite .NET unit test framework doesn't provide it).
    /// If you are using NUnit, this <see cref="T:NFluent.Assert"/> class is useless (since NUnit provides now few Assert.That() overloads).
    /// On the other hand, xUnit users may benefit to this class.
    /// </summary>
    public static class Assert
    {
        ///// <summary>
        ///// Asserts that a condition is true. If the condition is false the method throw a <see cref="T:FluentAssertionException"/>.
        ///// </summary>
        ///// <param name="condition">The evaluated condition.</param>
        //public static void That(bool condition)
        //{
        //    // TODO: improve the default message of failing Assert.That
        //    That(condition, string.Empty);
        //}

        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throw a <see cref="T:FluentAssertionException" />.
        /// </summary>
        /// <param name="condition">The evaluated condition.</param>
        /// <param name="message">The message for the <see cref="FluentAssertionException"/> if assertion fails.</param>
        /// <exception cref="FluentAssertionException">The assertion fails.</exception>
        public static void That(bool condition, string message)
        {
            if (!condition)
            {
                throw new FluentAssertionException(message);
            }
        }

        public static IFluentEnumerable<T> That<T>(IEnumerable<T> enumerable)
        {
            return new FluentEnumerable<T>(enumerable);
        }

        public static IFluentString That(string value)
        {
            return new FluentString(value);
        }
    }
}

