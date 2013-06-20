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
    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on a boolean value.
    /// </summary>
    public static class BooleanFluentAssertionExtensions
    {
        // message when the value must be false
        private const string MustBeFalseMessage = "The {0} is true whereas it must be false.";

        // message when the value must be true
        private const string MustBeTrueMessage = "The {0} is false whereas it must be true.";

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public static IChainableCheck<ICheck<bool>> IsEqualTo(this ICheck<bool> check, object expected)
        {
            var checkRunner = check as ICheckRunner<bool>;
            var runnableCheck = check as IRunnableCheck<bool>;
            
            return checkRunner.ExecuteAssertion(
                () =>
                {
                    EqualityHelper.IsEqualTo(runnableCheck.Value, expected);
                },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expected, true));
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is equal to the expected value.</exception>
        public static IChainableCheck<ICheck<bool>> IsNotEqualTo(this ICheck<bool> check, object expected)
        {
            var checkRunner = check as ICheckRunner<bool>;
            var runnableCheck = check as IRunnableCheck<bool>;

            return checkRunner.ExecuteAssertion(
                () =>
                {
                    EqualityHelper.IsNotEqualTo(runnableCheck.Value, expected);
                },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expected, false));
        }

        /// <summary>
        /// Checks that the actual value is true.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not true.</exception>
        public static IChainableCheck<ICheck<bool>> IsTrue(this ICheck<bool> check)
        {
            var checkRunner = check as ICheckRunner<bool>;
            var runnableCheck = check as IRunnableCheck<bool>;

            return checkRunner.ExecuteAssertion(
                () =>
                {
                    if (!runnableCheck.Value)
                    {
                        throw new FluentCheckException(FluentMessage.BuildMessage(MustBeTrueMessage).For("boolean").On(runnableCheck.Value).ToString());
                    }
                },
                FluentMessage.BuildMessage(MustBeFalseMessage).For("boolean").On(runnableCheck.Value).ToString());
        }

        /// <summary>
        /// Checks that the actual value is false.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A chainable check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not false.</exception>
        public static IChainableCheck<ICheck<bool>> IsFalse(this ICheck<bool> check)
        {
            var checkRunner = check as ICheckRunner<bool>;
            var runnableCheck = check as IRunnableCheck<bool>;

            return checkRunner.ExecuteAssertion(
                () =>
                {
                    if (runnableCheck.Value)
                    {
                        throw new FluentCheckException(FluentMessage.BuildMessage(MustBeFalseMessage).For("boolean").On(runnableCheck.Value).ToString());
                    }
                },
                FluentMessage.BuildMessage(MustBeTrueMessage).For("boolean").On(runnableCheck.Value).ToString());
        }

        /// <summary>
        /// Checks that the actual instance is an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The expected Type of the instance.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A chainable fluent check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual instance is not of the provided type.</exception>
        public static IChainableCheck<ICheck<bool>> IsInstanceOf<T>(this ICheck<bool> check)
        {
            var checkRunner = check as ICheckRunner<bool>;
            var runnableCheck = check as IRunnableCheck<bool>;

            return checkRunner.ExecuteAssertion(
                () =>
                {
                    IsInstanceHelper.IsInstanceOf(runnableCheck.Value, typeof(T));
                },
                IsInstanceHelper.BuildErrorMessage(runnableCheck, typeof(T), true));
        }

        /// <summary>
        /// Checks that the actual instance is not an instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type not expected for this instance.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A chainable fluent check.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual instance is of the provided type.</exception>
        public static IChainableCheck<ICheck<bool>> IsNotInstanceOf<T>(this ICheck<bool> check)
        {
            var checkRunner = check as ICheckRunner<bool>;
            var runnableCheck = check as IRunnableCheck<bool>;
            
            var expectedType = typeof(T);

            return checkRunner.ExecuteAssertion(
                () =>
                    {
                        IsInstanceHelper.IsNotInstanceOf(runnableCheck.Value, expectedType);
                    },
                IsInstanceHelper.BuildErrorMessage(runnableCheck.Value, typeof(T), true));
        }
    }
}