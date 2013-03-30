// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ComparableFluentAssertionExtensions.cs" company="">
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
namespace Spike.Ext
{
    using System;
    using NFluent;

    /// <summary>
    /// Provides assertion methods to be executed on an <see cref="IComparable"/> instance.
    /// </summary>
    public static class ComparableFluentAssertionExtensions
    {
        /// <summary>
        /// Determines whether the specified value is before the other one.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="otherValue">The other value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="NFluent.FluentAssertionException">The current value is not before the other one.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<IComparable>> IsBefore(this IFluentAssertion<IComparable> fluentAssertion, IComparable otherValue)
        {
            if (fluentAssertion.Value.CompareTo(otherValue) >= 0)
            {
                throw new FluentAssertionException("is not before.");
            }

            return new ChainableFluentAssertion<IFluentAssertion<IComparable>>(fluentAssertion);
        }
    }
}