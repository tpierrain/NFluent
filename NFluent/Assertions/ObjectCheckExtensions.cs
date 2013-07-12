// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ObjectCheckExtensions.cs" company="">
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
    using System.Reflection;

    using NFluent.Extensions;
    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on an object instance.
    /// </summary>
    public static class ObjectCheckExtensions
    {
        // TODO: add IsNull()

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
            var checkRunner = check as ICheckRunner<T>;
            var runnableCheck = check as IRunnableCheck<T>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        EqualityHelper.IsEqualTo(runnableCheck.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expected, true));
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
            var checkRunner = check as ICheckRunner<T>;
            var runnableCheck = check as IRunnableCheck<T>;

            return checkRunner.ExecuteCheck(
                () =>
                    {
                        EqualityHelper.IsNotEqualTo(runnableCheck.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expected, false));
        }

        /// <summary>
        /// Checks that the actual expression is in the inheritance hierarchy of the given type or of the same type.
        /// </summary>
        /// <typeparam name="T">The Type which is expected to be a base Type of the actual expression.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The checked expression is not in the inheritance hierarchy of the given type.</exception>
        public static ICheckLink<ICheck<object>> InheritsFrom<T>(this ICheck<object> check)
        {
            var checkRunner = check as ICheckRunner<object>;
            var runnableCheck = check as IRunnableCheck<object>;

            Type instanceType = runnableCheck.Value.GetTypeWithoutThrowingException();
            Type expectedBaseType = typeof(T);

            return checkRunner.ExecuteCheck(
                () =>
                {
                    IsInstanceHelper.InheritsFrom(runnableCheck.Value, expectedBaseType);
                },
                string.Format("\nThe checked expression is part of the inheritance hierarchy or of the same type than the specified one.\nIndeed, checked expression type:\n\t[{0}]\nis a derived type of\n\t[{1}].", instanceType.ToStringProperlyFormated(), expectedBaseType.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual value has an expected reference.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected object.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not the same reference than the expected value.</exception>
        public static ICheckLink<ICheck<object>> IsSameReferenceThan(
            this ICheck<object> check, object expected)
        {
            var runnableCheck = check as IRunnableCheck<object>;
            var negated = runnableCheck.Negated;
            var value = runnableCheck.Value;

            string comparison;
            var message = SameReferenceImpl(expected, value, negated, out comparison);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentCheckException(
                    FluentMessage.BuildMessage(message)
                                 .For("object")
                                 .On(value)
                                 .And.Expected(expected)
                                 .Comparison(comparison)
                                 .ToString());
            }

            return new CheckLink<ICheck<object>>(check);
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
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="comparand">The expected value to be distinct from.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is the same instance than the comparand.</exception>
        public static ICheckLink<ICheck<object>> IsDistinctFrom(
            this ICheck<object> check, object comparand)
        {
            var runnableCheck = check as IRunnableCheck<object>;
            var negated = !runnableCheck.Negated;
            var value = runnableCheck.Value;

            string comparison;
            var message = SameReferenceImpl(comparand, value, negated, out comparison);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentCheckException(
                    FluentMessage.BuildMessage(message)
                                 .For("object")
                                 .On(value)
                                 .And.Expected(comparand)
                                 .Comparison(comparison)
                                 .ToString());
            }

            return new CheckLink<ICheck<object>>(check);
        }

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        /// <remarks>The comparison is done field by field.</remarks>
        public static ICheckLink<ICheck<object>> HasFieldsEqualToThose(this ICheck<object> check, object expected)
        {
            var runnableCheck = check as IRunnableCheck<object>;
            var negated = runnableCheck.Negated;
            var value = runnableCheck.Value;

            var message = CheckFieldEquality(expected, value, negated);

            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return new CheckLink<ICheck<object>>(check);
        }

        /// <summary>
        /// Checks that the actual value is equal to another expected value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        /// <remarks>The comparison is done field by field.</remarks>
        public static ICheckLink<ICheck<object>> HasFieldsNotEqualToThose(this ICheck<object> check, object expected)
        {
            var runnableCheck = check as IRunnableCheck<object>;
            var negated = !runnableCheck.Negated;
            var value = runnableCheck.Value;

            var message = CheckFieldEquality(expected, value, negated);

            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return new CheckLink<ICheck<object>>(check);
        }

        private static string CheckFieldEquality(object expected, object value, bool negated)
        {
            string message = null;

            foreach (var fieldInfo in
                value.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                var otherField = FindField(expected.GetType(), fieldInfo.Name);
                if (otherField == null)
                {
                    if (!negated)
                    {
                        message =
                            FluentMessage.BuildMessage(
                                string.Format("The {{0}} has a field that is absent from the {{1}}: {0}.", fieldInfo.Name))
                                         .On(value)
                                         .And.Expected(expected)
                                         .ToString();
                    }

                    break;
                }

                // compare value
                if (fieldInfo.GetValue(value).Equals(otherField.GetValue(expected)) == negated)
                {
                    if (!negated)
                    {
                        message =
                            FluentMessage.BuildMessage(
                                string.Format("The {{0}}'s field {0} does not have the expected value.", fieldInfo.Name))
                                         .On(value)
                                         .And.Expected(expected)
                                         .ToString();
                    }
                    else
                    {
                        message =
                            FluentMessage.BuildMessage(string.Format("The {{0}}'s field {0} has the same value in the comparand, whereas it must not.", fieldInfo.Name))
                                         .On(value)
                                         .And.Expected(expected)
                                         .Comparison("different from")
                                         .ToString();
                    }

                    break;
                }
            }

            return message;
        }

        private static FieldInfo FindField(Type type, string name)
        {
            var result = type
                                 .GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (result != null)
            {
                return result;
            }
            
            if (type.BaseType == null)
            {
                return null;
            }

            return FindField(type.BaseType, name);
        }
    }
}
