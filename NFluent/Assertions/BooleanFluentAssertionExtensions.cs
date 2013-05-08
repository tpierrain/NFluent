// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="BooleanFluentAssertionExtensions.cs" company="">
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
    using NFluent.Extensions;
    using NFluent.Helpers;

    /// <summary>
    /// Provides assertion methods to be executed on a boolean value.
    /// </summary>
    public static class BooleanFluentAssertionExtensions
    {
        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<bool>> IsEqualTo(this IFluentAssertion<bool> fluentAssertion, object expected)
        {
            FluentAssertion<bool> assertion = fluentAssertion as FluentAssertion<bool>;

            if (assertion.Negated)
            {
                EqualityHelper.IsNotEqualTo(assertion.Value, expected);
            }
            else
            {
                EqualityHelper.IsEqualTo(assertion.Value, expected);
            }

            return new ChainableFluentAssertion<IFluentAssertion<bool>>(fluentAssertion);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<bool>> IsNotEqualTo(this IFluentAssertion<bool> fluentAssertion, object expected)
        {
            FluentAssertion<bool> assertion = fluentAssertion as FluentAssertion<bool>;

            if (assertion.Negated)
            {
                EqualityHelper.IsEqualTo(assertion.Value, expected);
            }
            else
            {
                EqualityHelper.IsNotEqualTo(assertion.Value, expected);
            }

            return new ChainableFluentAssertion<IFluentAssertion<bool>>(fluentAssertion);
        }

        /// <summary>
        /// Checks that the actual value is true.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not true.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<bool>> IsTrue(this IFluentAssertion<bool> fluentAssertion)
        {
            FluentAssertion<bool> assertion = fluentAssertion as FluentAssertion<bool>;

            if (assertion.Negated)
            {
                IsTrueNegatedImpl(fluentAssertion);
            }
            else
            {
                IsTrueImpl(fluentAssertion);
            }

            return new ChainableFluentAssertion<IFluentAssertion<bool>>(fluentAssertion);
        }

        private static void IsTrueImpl(IFluentAssertion<bool> fluentAssertion)
        {
            if (!fluentAssertion.Value)
            {
                throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]\nis not true.", fluentAssertion.Value.ToStringProperlyFormated()));
            }
        }

        private static void IsTrueNegatedImpl(IFluentAssertion<bool> fluentAssertion)
        {
            try
            {
                IsTrueImpl(fluentAssertion);
            }
            catch (FluentAssertionException)
            {
                return;
            }

            throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]\nis true.", fluentAssertion.Value.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual value is false.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not false.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<bool>> IsFalse(this IFluentAssertion<bool> fluentAssertion)
        {
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<bool>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<bool>;

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    if (runnableAssertion.Value)
                    {
                        throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]\nis not false.", runnableAssertion.Value.ToStringProperlyFormated()));
                    }
                },
                string.Format("\nThe actual value:\n\t[{0}]\nis false.", runnableAssertion.Value.ToStringProperlyFormated()));
        }

        // throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]\nis false.", fluentAssertion.Value.ToStringProperlyFormated()));

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<bool>> IsInstanceOf<T>(this IFluentAssertion<bool> fluentAssertion)
        {
            FluentAssertion<bool> assertion = fluentAssertion as FluentAssertion<bool>;

            if (assertion.Negated)
            {
                IsInstanceHelper.IsNotInstanceOf(assertion.Value, typeof(T));
            }
            else
            {
                IsInstanceHelper.IsInstanceOf(assertion.Value, typeof(T));
            }

            return new ChainableFluentAssertion<IFluentAssertion<bool>>(fluentAssertion);
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<bool>> IsNotInstanceOf<T>(this IFluentAssertion<bool> fluentAssertion)
        {
            FluentAssertion<bool> assertion = fluentAssertion as FluentAssertion<bool>;

            if (assertion.Negated)
            {
                IsInstanceHelper.IsInstanceOf(assertion.Value, typeof(T));
            }
            else
            {
                IsInstanceHelper.IsNotInstanceOf(assertion.Value, typeof(T));
            }

            return new ChainableFluentAssertion<IFluentAssertion<bool>>(fluentAssertion);
        }
    }
}