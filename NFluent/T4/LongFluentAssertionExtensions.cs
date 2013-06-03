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
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<long>> IsEqualTo(this IFluentAssertion<long> fluentAssertion, object expected)
        {
            // TODO transform NumberFluentAssertion<T> into a static class with functions only?
            var numberAssertionStrategy = new NumberFluentAssertion<long>(fluentAssertion);
            return numberAssertionStrategy.IsEqualTo(expected);
        }

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<long>> IsEqualTo(this IFluentAssertion<long?> fluentAssertion, object expected)
        {
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<long?>;
            IRunnableAssertion<long?> runnableAssertion = fluentAssertion as IRunnableAssertion<long?>;
            
            object value = null;
            if (runnableAssertion.Value.HasValue)
            {
                value = runnableAssertion.Value.Value;
            }

            assertionRunner.ExecuteAssertion(
                () =>
                {
                    if (value == null)
                    {
                        if (expected != null)
                        {
                            throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[null]\nis not equal to the expected one:\n\t[{0}].", expected.ToStringProperlyFormated()));
                        }
                    }
                    else
                    {
                        EqualityHelper.IsEqualTo(value, expected);
                    }
                },
                EqualityHelper.BuildErrorMessage(value, expected, true));

            if (value != null)
            {
                IFluentAssertion<long> fakePreviousAssertion = new FluentAssertion<long>((long)value);
                return new ChainableFluentAssertion<IFluentAssertion<long>>(fakePreviousAssertion);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual value is equal to the expected value.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<long>> IsNotEqualTo(this IFluentAssertion<long> fluentAssertion, object expected)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<long>(fluentAssertion);
            return numberAssertionStrategy.IsNotEqualTo(expected);
        }

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<long>> IsInstanceOf<T>(this IFluentAssertion<long> fluentAssertion)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<long>(fluentAssertion);
            return numberAssertionStrategy.IsInstanceOf<T>();
        }

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The actual instance is not of the provided type.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<long?>> IsInstanceOf<T>(this IFluentAssertion<long?> fluentAssertion)
        {
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<long?>;
            IRunnableAssertion<long?> runnableAssertion = fluentAssertion as IRunnableAssertion<long?>;

            assertionRunner.ExecuteAssertion(
                () =>
                {
                    IsInstanceHelper.IsSameType(typeof(Nullable<long>), typeof(T), runnableAssertion.Value);
                },
                IsInstanceHelper.BuildErrorMessageForNullable(typeof(Nullable<long>), typeof(T), runnableAssertion.Value, true));

            return new ChainableFluentAssertion<IFluentAssertion<long?>>(fluentAssertion);
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
        public static IChainableFluentAssertion<IFluentAssertion<long>> IsNotInstanceOf<T>(this IFluentAssertion<long> fluentAssertion)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<long>(fluentAssertion);
            return numberAssertionStrategy.IsNotInstanceOf<T>();
        }

        /// <summary>
        /// Checks that the actual value is equal to zero.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is not equal to zero.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<long>> IsZero(this IFluentAssertion<long> fluentAssertion)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<long>(fluentAssertion);
            return numberAssertionStrategy.IsZero();
        }

        /// <summary>
        /// Checks that the actual nullable value is null. 
        /// Note: this method does not return a chainable assertion.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <exception cref="FluentAssertionException">The value is not null.</exception>
        public static void IsNull(this IFluentAssertion<long?> fluentAssertion)
        {
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<long?>;
            IRunnableAssertion<long?> runnableAssertion = fluentAssertion as IRunnableAssertion<long?>;

            assertionRunner.ExecuteAssertion(
                () =>
                {
                    if (runnableAssertion.Value != null)
                    {
                        throw new FluentAssertionException(string.Format("\nThe checked nullable value:\n\t[{0}]\nis not null as expected.", runnableAssertion.Value));
                    }
                },
                "\nThe checked nullable value is null which is unexpected.");
        }

        /// <summary>
        /// Checks that the actual nullable value is not null. 
        /// Note: this method does not return a chainable assertion since it may lead to problem when calling Not.IsNotNull() with a nullable with null as Value.
        /// </summary>
        /// <remarks>Could return a chainable assertion only if we disable the Not operator for this method (to be investigated).</remarks>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <exception cref="FluentAssertionException">The value is null.</exception>
        public static void IsNotNull(this IFluentAssertion<long?> fluentAssertion)
        {
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<long?>;
            IRunnableAssertion<long?> runnableAssertion = fluentAssertion as IRunnableAssertion<long?>;

            assertionRunner.ExecuteAssertion(
                () =>
                {
                    if (runnableAssertion.Value == null)
                    {
                        throw new FluentAssertionException(string.Format("\nThe checked nullable value is null which is unexpected."));
                    }
                },
                string.Format("\nThe checked nullable value:\n\t[{0}]\nis not null which is unexpected.", runnableAssertion.Value.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual value is NOT equal to zero.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        ///   <returns>A chainable assertion.</returns>
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is equal to zero.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<long>> IsNotZero(this IFluentAssertion<long> fluentAssertion)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<long>(fluentAssertion);
            return numberAssertionStrategy.IsNotZero();
        }

        /// <summary>
        /// Checks that the actual value is strictly positive.
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion to be extended.</param>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The value is not strictly positive.</exception>
        public static IChainableFluentAssertion<IFluentAssertion<long>> IsPositive(this IFluentAssertion<long> fluentAssertion)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<long>(fluentAssertion);
            return numberAssertionStrategy.IsPositive();
        }

        /// <summary>
        /// Checks that the actual value is less than an operand.
        /// </summary>
        /// <param name="fluentAssertion">
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
        public static IChainableFluentAssertion<IFluentAssertion<long>> IsLessThan(this IFluentAssertion<long> fluentAssertion, long comparand)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<long>(fluentAssertion);
            return numberAssertionStrategy.IsLessThan(comparand);
        }

        /// <summary>
        /// Checks that the actual value is more than an operand.
        /// </summary>
        /// <param name="fluentAssertion">
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
        public static IChainableFluentAssertion<IFluentAssertion<long>> IsGreaterThan(this IFluentAssertion<long> fluentAssertion, long comparand)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<long>(fluentAssertion);
            return numberAssertionStrategy.IsGreaterThan(comparand);
        }
    }
}
