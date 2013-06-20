// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ObjectFluentAssertionExtensions.cs" company="">
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
    using System;

    using NFluent.Extensions;
    using NFluent.Helpers;

    /// <summary>
    /// Provides assertion methods to be executed on an object instance.
    /// </summary>
    public static class ObjectFluentAssertionExtensions
    {
        // TODO: add IsNull()

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public static IChainableFluentAssertion<ICheck<object>> IsEqualTo(this ICheck<object> check, object expected)
        {
            var assertionRunner = check as IFluentAssertionRunner<object>;
            var runnableAssertion = check as IRunnableAssertion<object>;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        EqualityHelper.IsEqualTo(runnableAssertion.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableAssertion.Value, expected, true));
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public static IChainableFluentAssertion<ICheck<object>> IsNotEqualTo(this ICheck<object> check, object expected)
        {
            var assertionRunner = check as IFluentAssertionRunner<object>;
            var runnableAssertion = check as IRunnableAssertion<object>;

            return assertionRunner.ExecuteAssertion(
                () =>
                    {
                        EqualityHelper.IsNotEqualTo(runnableAssertion.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableAssertion.Value, expected, false));
        }

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public static IChainableFluentAssertion<ICheck<object>> IsInstanceOf<T>(this ICheck<object> check)
        {
            var assertionRunner = check as IFluentAssertionRunner<object>;
            var runnableAssertion = check as IRunnableAssertion<object>;

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    IsInstanceHelper.IsInstanceOf(runnableAssertion.Value, typeof(T));
                },
                IsInstanceHelper.BuildErrorMessage(runnableAssertion, typeof(T), true));
        }

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public static IChainableFluentAssertion<ICheck<object>> IsNotInstanceOf<T>(this ICheck<object> check)
        {
            var assertionRunner = check as IFluentAssertionRunner<object>;
            var runnableAssertion = check as IRunnableAssertion<object>;

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    IsInstanceHelper.IsNotInstanceOf(runnableAssertion.Value, typeof(T));
                },
                string.Format("\nThe actual value:\n\t[{0}]\nis not an instance of:\n\t[{1}]\nbut an instance of:\n\t[{2}]\ninstead.", runnableAssertion.Value.ToStringProperlyFormated(), typeof(T), runnableAssertion.Value.GetType()));
        }

        /// <summary>
        /// Checks that the actual expression is in the inheritance hierarchy of the given type or of the same type.
        /// </summary>
        /// <typeparam name="T">The Type which is expected to be a base Type of the actual expression.</typeparam>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The checked expression is not in the inheritance hierarchy of the given type.</exception>
        public static IChainableFluentAssertion<ICheck<object>> InheritsFrom<T>(this ICheck<object> check)
        {
            var assertionRunner = check as IFluentAssertionRunner<object>;
            var runnableAssertion = check as IRunnableAssertion<object>;

            Type instanceType = runnableAssertion.Value.GetTypeWithoutThrowingException();
            Type expectedBaseType = typeof(T);

            return assertionRunner.ExecuteAssertion(
                () =>
                {
                    IsInstanceHelper.InheritsFrom(runnableAssertion.Value, expectedBaseType);
                },
                string.Format("\nThe checked expression is part of the inheritance hierarchy or of the same type than the specified one.\nIndeed, checked expression type:\n\t[{0}]\nis a derived type of\n\t[{1}].", instanceType.ToStringProperlyFormated(), expectedBaseType.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual value has an expected reference.
        /// </summary>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected object.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not the same reference than the expected value.</exception>
        public static IChainableFluentAssertion<ICheck<object>> IsSameReferenceThan(
            this ICheck<object> check, object expected)
        {
            var runnableAssertion = check as IRunnableAssertion<object>;
            var negated = runnableAssertion.Negated;
            var value = runnableAssertion.Value;

            string comparison;
            var message = SameReferenceImpl(expected, value, negated, out comparison);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentAssertionException(
                    FluentMessage.BuildMessage(message)
                                 .For("object")
                                 .On(value)
                                 .And.Expected(expected)
                                 .Comparison(comparison)
                                 .ToString());
            }

            return new ChainableFluentAssertion<ICheck<object>>(check);
        }

        private static string SameReferenceImpl(object expected, object value, bool negated, out string comparison)
        {
            string message = null;
            comparison = null;

            if (object.ReferenceEquals(value, expected) == negated)
            {
                if (negated)
                {
                    message = "The {0} must have be an instance distinct from {1}.";
                    comparison = "distinct from";
                }
                else
                {
                    message = "The {0} must be the same instance than {1}.";
                    comparison = "same instance than";
                }
            }

            return message;
        }

        /// <summary>
        /// Checks that the actual value is a different instance than a comparand.
        /// </summary>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <param name="comparand">The expected value to be distinct from.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is the same instance than the comparand.</exception>
        public static IChainableFluentAssertion<ICheck<object>> IsDistinctFrom(
            this ICheck<object> check, object comparand)
        {
            var runnableAssertion = check as IRunnableAssertion<object>;
            var negated = !runnableAssertion.Negated;
            var value = runnableAssertion.Value;

            string comparison;
            var message = SameReferenceImpl(comparand, value, negated, out comparison);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentAssertionException(
                    FluentMessage.BuildMessage(message)
                                 .For("object")
                                 .On(value)
                                 .And.Expected(comparand)
                                 .Comparison(comparison)
                                 .ToString());
            }

            return new ChainableFluentAssertion<ICheck<object>>(check);
        }
    }
}
