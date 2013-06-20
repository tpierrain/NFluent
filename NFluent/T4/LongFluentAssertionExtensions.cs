// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="LongFluentAssertionExtensions.cs" company="">
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
    /// Provides assertion methods to be executed on a long value.
    /// </summary>
    public static class LongFluentAssertionExtensions
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------

        #pragma warning restore 169

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public static IChainableFluentAssertion<ICheck<long>> IsEqualTo(this ICheck<long> check, object expected)
        {
            // TODO transform NumberFluentAssertion<T> into a static class with functions only?
            var numberAssertionStrategy = new NumberCheck<long>(check);
            return numberAssertionStrategy.IsEqualTo(expected);
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
        public static IChainableFluentAssertion<ICheck<long>> IsNotEqualTo(this ICheck<long> check, object expected)
        {
            var numberAssertionStrategy = new NumberCheck<long>(check);
            return numberAssertionStrategy.IsNotEqualTo(expected);
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
        public static IChainableFluentAssertion<ICheck<long>> IsInstanceOf<T>(this ICheck<long> check)
        {
            var numberAssertionStrategy = new NumberCheck<long>(check);
            return numberAssertionStrategy.IsInstanceOf<T>();
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
        public static IChainableFluentAssertion<ICheck<long?>> IsInstanceOf<T>(this ICheck<long?> check)
        {
            var assertionRunner = check as IFluentAssertionRunner<long?>;
            IRunnableAssertion<long?> runnableAssertion = check as IRunnableAssertion<long?>;

            assertionRunner.ExecuteAssertion(
                () =>
                {
                    IsInstanceHelper.IsSameType(typeof(Nullable<long>), typeof(T), runnableAssertion.Value);
                },
                IsInstanceHelper.BuildErrorMessageForNullable(typeof(Nullable<long>), typeof(T), runnableAssertion.Value, true));

            return new ChainableFluentAssertion<ICheck<long?>>(check);
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is of the provided type.</exception>
        public static IChainableFluentAssertion<ICheck<long>> IsNotInstanceOf<T>(this ICheck<long> check)
        {
            var numberAssertionStrategy = new NumberCheck<long>(check);
            return numberAssertionStrategy.IsNotInstanceOf<T>();
        }

        /// <summary>
        /// Checks that the actual value is equal to zero.
        /// </summary>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is not equal to zero.</exception>
        public static IChainableFluentAssertion<ICheck<long>> IsZero(this ICheck<long> check)
        {
            var numberAssertionStrategy = new NumberCheck<long>(check);
            return numberAssertionStrategy.IsZero();
        }

        /// <summary>
        /// Checks that the actual nullable value has a value and thus, is not null.
        /// </summary>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <returns>A chainable fluent assertion.</returns>
        /// <exception cref="FluentAssertionException">The value is null.</exception>
        public static IChainableNullableFluentAssertionOrNumberFluentAssertion<long> HasAValue(this ICheck<long?> check)
        {
            var assertionRunner = check as IFluentAssertionRunner<long?>;
            IRunnableAssertion<long?> runnableAssertion = check as IRunnableAssertion<long?>;

            assertionRunner.ExecuteAssertion(
                () =>
                {
                    if (runnableAssertion.Value == null)
                    {
                        throw new FluentAssertionException(string.Format("\nThe checked nullable value has no value, which is unexpected."));
                    }
                },
                string.Format("\nThe checked nullable value:\n\t[{0}]\nhas a value, which is unexpected.", runnableAssertion.Value.ToStringProperlyFormated()));

            return new ChainableNullableFluentAssertionOrNumberFluentAssertion<long>(check);
        }

        /// <summary>
        /// Checks that the actual nullable value has no value and thus, is null. 
        /// Note: this method does not return a chainable assertion since the nullable is null.
        /// </summary>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <exception cref="FluentAssertionException">The value is not null.</exception>
        public static void HasNoValue(this ICheck<long?> check)
        {
            var assertionRunner = check as IFluentAssertionRunner<long?>;
            IRunnableAssertion<long?> runnableAssertion = check as IRunnableAssertion<long?>;

            assertionRunner.ExecuteAssertion(
                () =>
                {
                    if (runnableAssertion.Value != null)
                    {
                        throw new FluentAssertionException(string.Format("\nThe checked nullable value:\n\t[{0}]\nhas a value, which is unexpected.", runnableAssertion.Value));
                    }
                },
                "\nThe checked nullable value has no value, which is unexpected.");
        }
        
        /// <summary>
        /// Checks that the actual value is NOT equal to zero.
        /// </summary>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <returns>
        ///   <returns>A chainable assertion.</returns>
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is equal to zero.</exception>
        public static IChainableFluentAssertion<ICheck<long>> IsNotZero(this ICheck<long> check)
        {
            var numberAssertionStrategy = new NumberCheck<long>(check);
            return numberAssertionStrategy.IsNotZero();
        }

        /// <summary>
        /// Checks that the actual value is strictly positive.
        /// </summary>
        /// <param name="check">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is not strictly positive.</exception>
        public static IChainableFluentAssertion<ICheck<long>> IsPositive(this ICheck<long> check)
        {
            var numberAssertionStrategy = new NumberCheck<long>(check);
            return numberAssertionStrategy.IsPositive();
        }

        /// <summary>
        /// Checks that the actual value is less than an operand.
        /// </summary>
        /// <param name="check">
        /// The Fluent assertion to be extended.
        /// </param>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The value is not less than the comparand.
        /// </exception>
        public static IChainableFluentAssertion<ICheck<long>> IsLessThan(this ICheck<long> check, long comparand)
        {
            var numberAssertionStrategy = new NumberCheck<long>(check);
            return numberAssertionStrategy.IsLessThan(comparand);
        }

        /// <summary>
        /// Checks that the actual value is more than an operand.
        /// </summary>
        /// <param name="check">
        /// The Fluent assertion to be extended.
        /// </param>
        /// <param name="comparand">
        /// Comparand to compare the value to.
        /// </param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The value is not less than the comparand.
        /// </exception>
        public static IChainableFluentAssertion<ICheck<long>> IsGreaterThan(this ICheck<long> check, long comparand)
        {
            var numberAssertionStrategy = new NumberCheck<long>(check);
            return numberAssertionStrategy.IsGreaterThan(comparand);
        }
    }
}
