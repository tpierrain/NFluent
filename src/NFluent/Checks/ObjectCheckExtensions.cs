// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ObjectCheckExtensions.cs" company="">
// //   Copyright 2014 Thomas PIERRAIN, Cyrille DUPUYDAUBY
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

    using Extensibility;
    using Extensions;
    using Helpers;

    /// <summary>
    /// Provides check methods to be executed on an object instance.
    /// </summary>
    public static class ObjectCheckExtensions
    {
        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The actual value is not equal to the expected value.
        /// </exception>
        public static ICheckLink<ICheck<T>> IsEqualTo<T>(this ICheck<T> check, T expected)
        {
            return IsEqualTo(check, (object)expected);
        }

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The actual value is not equal to the expected value.
        /// </exception>
        public static ICheckLink<ICheck<T>> IsEqualTo<T>(this ICheck<T> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return EqualityHelper.PerformEqualCheck(checker, expected, false);
        }

        /// <summary>
        /// Checks that the actual value is equal to another expected value using operator==.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The actual value is not equal to the expected value.
        /// </exception>
        public static ICheckLink<ICheck<T>> HasSameValueAs<T>(this ICheck<T> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return EqualityHelper.PerformEqualCheck(checker, expected, true);
        }

        /// <summary>
        /// Checks that the actual value is different from another expected value using operator!=.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The actual value is equal to the expected value.
        /// </exception>
        public static ICheckLink<ICheck<T>> HasDifferentValueThan<T>(this ICheck<T> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return EqualityHelper.PerformEqualCheck(checker, expected, true, true);
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is equal to the expected value.</exception>
        public static ICheckLink<ICheck<T>> IsNotEqualTo<T>(this ICheck<T> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ExecuteCheck(
                () => EqualityHelper.IsNotEqualTo(checker, expected, false),
                EqualityHelper.BuildErrorMessage(checker, expected, false, false));
        }

        /// <summary>
        /// Checks that the actual value is not equal to another expected value.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is equal to the expected value.</exception>
        public static ICheckLink<ICheck<T>> IsNotEqualTo<T>(this ICheck<T> check, T expected)
        {
            return IsNotEqualTo(check, (object)expected);
        }

        /// <summary>
        /// Checks that the actual expression is in the inheritance hierarchy of the given kind or of the same kind.
        /// </summary>
        /// <typeparam name="T">The Type which is expected to be a base Type of the actual expression.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <exception cref="FluentCheckException">The checked expression is not in the inheritance hierarchy of the given kind.</exception>
        public static void InheritsFrom<T>(this ICheck<object> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            Type instanceType = checker.Value.GetTypeWithoutThrowingException();
            Type expectedBaseType = typeof(T);

            checker.ExecuteNotChainableCheck(
                () => IsInstanceHelper.InheritsFrom(checker, expectedBaseType),
                string.Format(Environment.NewLine+ "The checked expression is part of the inheritance hierarchy or of the same type than the specified one." + Environment.NewLine + "Indeed, checked expression type:" + Environment.NewLine + "\t[{0}]" + Environment.NewLine + "is a derived type of" + Environment.NewLine + "\t[{1}].", instanceType.ToStringProperlyFormated(), expectedBaseType.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual expression is null.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked value is not null.</exception>
        public static ICheckLink<ICheck<T>> IsNull<T>(this ICheck<T> check) where T : class
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var negated = checker.Negated;
            var value = checker.Value;

            var message = IsNullImpl(value, negated);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentCheckException(checker.BuildMessage(message).For("object").ToString());
            }

            return checker.BuildChainingObject();
        }

        /// <summary>
        /// Checks that the actual Nullable value is null.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked value is not null.</exception>
        public static ICheckLink<ICheck<T?>> IsNull<T>(this ICheck<T?> check) where T : struct
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheck(
                () =>
                {
                    if (checker.Value != null)
                    {
                        var message = checker.BuildMessage("The checked nullable value must be null.").ToString();
                        throw new FluentCheckException(message);
                    }
                },
                checker.BuildShortMessage("The checked nullable value is null whereas it must not.").ToString());
        }

        /// <summary>
        /// Checks that the actual Nullable value is not null.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked value is null.</exception>
        public static ICheckLink<ICheck<T?>> IsNotNull<T>(this ICheck<T?> check) where T : struct
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            return checker.ExecuteCheck(
                () =>
                {
                    if (checker.Value != null)
                    {
                        return;
                    }

                    var message = checker.BuildShortMessage("The checked nullable value is null whereas it must not.").ToString();
                    throw new FluentCheckException(message);
                },
                checker.BuildMessage("The checked nullable value must be null.").ToString());
        }

        /// <summary>
        /// Checks that the actual expression is not null.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">Is the value is null.</exception>
        public static ICheckLink<ICheck<T>> IsNotNull<T>(this ICheck<T> check) where T : class
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var negated = checker.Negated;
            var value = checker.Value;

            var message = IsNullImpl(value, !negated);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentCheckException(checker.BuildMessage(message).For("object").On(value).ToString());
            }

            return checker.BuildChainingObject();
        }

        private static string IsNullImpl(object value, bool negated)
        {
            if (!negated)
            {
                return value == null ? null : "The {0} must be null.";
            }

            return value == null ? "The {0} must not be null." : null;
        }

        /// <summary>
        /// Checks that the actual value has an expected reference.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected object.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not the same reference than the expected value.</exception>
        public static ICheckLink<ICheck<T>> IsSameReferenceThan<T>(this ICheck<T> check, object expected)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var negated = checker.Negated;
            var value = checker.Value;

            string comparison;
            var message = SameReferenceImpl(expected, value, negated, out comparison);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentCheckException(checker.BuildMessage(message)
                                                             .For("object")
                                                             .Expected(expected)
                                                             .Comparison(comparison)
                                                             .ToString());
            }

            return checker.BuildChainingObject();
        }

        private static string SameReferenceImpl(object expected, object value, bool negated, out string comparison)
        {
            string message = null;
            comparison = null;

            if (ReferenceEquals(value, expected) == negated)
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
        /// <typeparam name="T">
        /// Type of the checked value.
        /// </typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="comparand">The expected value to be distinct from.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is the same instance than the comparand.</exception>
        public static ICheckLink<ICheck<T>> IsDistinctFrom<T>(this ICheck<T> check, object comparand)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var negated = !checker.Negated;
            var value = checker.Value;

            string comparison;
            var message = SameReferenceImpl(comparand, value, negated, out comparison);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentCheckException(checker.BuildMessage(message)
                                                             .For("object")
                                                             .Expected(comparand)
                                                             .Comparison(comparison)
                                                             .ToString());
            }

            return checker.BuildChainingObject();
        }
    }
}
