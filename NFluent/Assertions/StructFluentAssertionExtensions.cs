// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="StructFluentAssertionExtensions.cs" company="">
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
    using NFluent.Helpers;

    /// <summary>
    /// Provides assertion methods to be executed on an struct instance.
    /// </summary>
    public static class StructFluentAssertionExtensions
    {
        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <typeparam name="T">Type of the struct or the enum to assert on.</typeparam>
        /// <param name="fluentAssertion">The fluent fluent assertion.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public static IChainableFluentAssertion<IStructFluentAssertion<T>> IsEqualTo<T>(this IStructFluentAssertion<T> fluentAssertion, T expected) where T : struct
        {
            StructFluentAssertion<T> assertion = fluentAssertion as StructFluentAssertion<T>;
            if (assertion.Negated)
            {
                EqualityHelper.IsNotEqualTo(assertion.Value, expected);
            }
            else
            {
                EqualityHelper.IsEqualTo(assertion.Value, expected);
            }

            return new ChainableFluentAssertion<IStructFluentAssertion<T>>(fluentAssertion);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <typeparam name="T">Type of the struct or the enum to assert on.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public static IChainableFluentAssertion<IStructFluentAssertion<T>> IsNotEqualTo<T>(this IStructFluentAssertion<T> fluentAssertion, object expected) where T : struct
        {
            StructFluentAssertion<T> assertion = fluentAssertion as StructFluentAssertion<T>;

            if (assertion.Negated)
            {
                EqualityHelper.IsEqualTo(assertion.Value, expected);
            }
            else
            {
                EqualityHelper.IsNotEqualTo(assertion.Value, expected);
            }
            
            return new ChainableFluentAssertion<IStructFluentAssertion<T>>(fluentAssertion);
        }
    }
}
