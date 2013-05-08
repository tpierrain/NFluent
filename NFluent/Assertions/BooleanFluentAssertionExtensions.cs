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
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<bool>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<bool>;
            
            var instanceTypeMessage = EqualityHelper.BuildTypeDescriptionMessage(expected, false);

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    EqualityHelper.IsEqualTo(runnableAssertion.Value, expected);
                },
                string.Format("\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[{0}]{1}.", runnableAssertion.ToStringProperlyFormated(), instanceTypeMessage));
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
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<bool>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<bool>;

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    EqualityHelper.IsNotEqualTo(runnableAssertion.Value, expected);
                },
                EqualityHelper.BuildErrorMessage(runnableAssertion.Value, expected));
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
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<bool>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<bool>;

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    if (!fluentAssertion.Value)
                    {
                        throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]\nis not true.", runnableAssertion.Value.ToStringProperlyFormated()));
                    }
                },
                string.Format("\nThe actual value:\n\t[{0}]\nis true.", runnableAssertion.Value.ToStringProperlyFormated()));
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
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<bool>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<bool>;

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    IsInstanceHelper.IsInstanceOf(runnableAssertion.Value, typeof(T));
                },
                string.Format("\nThe actual value:\n\t[{0}]\nis an instance of:\n\t[{1}]\nwhich is not expected.", runnableAssertion.Value.ToStringProperlyFormated(), runnableAssertion.Value.GetType()));
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
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<bool>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<bool>;
            
            var expectedType = typeof(T);

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    IsInstanceHelper.IsNotInstanceOf(runnableAssertion.Value, expectedType);
                },
                string.Format("\nThe actual value:\n\t[{0}]\nis not an instance of:\n\t[{1}]\nbut an instance of:\n\t[{2}]\ninstead.", runnableAssertion.Value.ToStringProperlyFormated(), expectedType, runnableAssertion.Value.GetType()));
        }
    }
}