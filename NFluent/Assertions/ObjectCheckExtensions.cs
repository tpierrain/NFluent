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
    using System.Text.RegularExpressions;

    using NFluent.Extensibility;
    using NFluent.Extensions;
    using NFluent.Helpers;

    /// <summary>
    /// Provides check methods to be executed on an object instance.
    /// </summary>
    public static class ObjectCheckExtensions
    {
        private static readonly Regex AutoPropertyMask;
        private static readonly Regex AnonymousTypeFieldMask;

        static ObjectCheckExtensions()
        {
            AutoPropertyMask = new Regex("^<(.*)>k_");
            AnonymousTypeFieldMask = new Regex("^<(.*)>i_");
        }

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
            var runnableCheck = ExtensibilityHelper<T>.ExtractRunnableCheck(check);

            return runnableCheck.ExecuteCheck(
                () =>
                    {
                        EqualityHelper.IsEqualTo(runnableCheck.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expected, true));
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
        public static ICheckLink<ICheck<T>> IsEqualTo<T>(this ICheck<T> check, T expected)
        {
            return IsEqualTo(check, (object)expected);
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
            var runnableCheck = ExtensibilityHelper<T>.ExtractRunnableCheck(check);

            return runnableCheck.ExecuteCheck(
                () =>
                    {
                        EqualityHelper.IsNotEqualTo(runnableCheck.Value, expected);
                    },
                EqualityHelper.BuildErrorMessage(runnableCheck.Value, expected, false));
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
            var runnableCheck = ExtensibilityHelper<object>.ExtractRunnableCheck(check);

            Type instanceType = runnableCheck.Value.GetTypeWithoutThrowingException();
            Type expectedBaseType = typeof(T);

            return runnableCheck.ExecuteCheck(
                () =>
                {
                    IsInstanceHelper.InheritsFrom(runnableCheck.Value, expectedBaseType);
                },
                string.Format("\nThe checked expression is part of the inheritance hierarchy or of the same type than the specified one.\nIndeed, checked expression type:\n\t[{0}]\nis a derived type of\n\t[{1}].", instanceType.ToStringProperlyFormated(), expectedBaseType.ToStringProperlyFormated()));
        }

        /// <summary>
        /// Checks that the actual expression is null.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">Is the value is not null.</exception>
        public static ICheckLink<ICheck<object>> IsNull(this ICheck<object> check)
        {
            var runnableCheck = ExtensibilityHelper<object>.ExtractRunnableCheck(check);
            var negated = runnableCheck.Negated;
            var value = runnableCheck.Value;

            var message = IsNullImpl(value, negated);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentCheckException(FluentMessage.BuildMessage(message).For("object").On(value).ToString());
            }

            return new CheckLink<ICheck<object>>(check);
        }

        /// <summary>
        /// Checks that the actual expression is not null.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check link.</returns>
        /// <exception cref="FluentCheckException">Is the value is null.</exception>
        public static ICheckLink<ICheck<object>> IsNotNull(this ICheck<object> check)
        {
            var runnableCheck = ExtensibilityHelper<object>.ExtractRunnableCheck(check);
            var negated = runnableCheck.Negated;
            var value = runnableCheck.Value;

            var message = IsNullImpl(value, !negated);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentCheckException(FluentMessage.BuildMessage(message).For("object").On(value).ToString());
            }

            return new CheckLink<ICheck<object>>(check);
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
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expected">The expected object.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not the same reference than the expected value.</exception>
        public static ICheckLink<ICheck<object>> IsSameReferenceThan(this ICheck<object> check, object expected)
        {
            var runnableCheck = ExtensibilityHelper<object>.ExtractRunnableCheck(check);
            var negated = runnableCheck.Negated;
            var value = runnableCheck.Value;

            string comparison;
            var message = SameReferenceImpl(expected, value, negated, out comparison);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentCheckException(FluentMessage.BuildMessage(message)
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
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="comparand">The expected value to be distinct from.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is the same instance than the comparand.</exception>
        public static ICheckLink<ICheck<object>> IsDistinctFrom(this ICheck<object> check, object comparand)
        {
            var runnableCheck = ExtensibilityHelper<object>.ExtractRunnableCheck(check);
            var negated = !runnableCheck.Negated;
            var value = runnableCheck.Value;

            string comparison;
            var message = SameReferenceImpl(comparand, value, negated, out comparison);
            if (!string.IsNullOrEmpty(message))
            {
                throw new FluentCheckException(FluentMessage.BuildMessage(message)
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
            var runnableCheck = ExtensibilityHelper<object>.ExtractRunnableCheck(check);
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
            var runnableCheck = ExtensibilityHelper<object>.ExtractRunnableCheck(check);
            var negated = !runnableCheck.Negated;
            var value = runnableCheck.Value;

            var message = CheckFieldEquality(expected, value, negated);

            if (message != null)
            {
                throw new FluentCheckException(message);
            }

            return new CheckLink<ICheck<object>>(check);
        }

        private static string CheckFieldEquality(object expected, object value, bool negated, string prefix = "")
        {
            // REFACTOR: this method which has too much lines
            string message = null;
            foreach (var fieldInfo in value.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                // check for auto properties
                string fieldLabel;
                var fieldname = BuildFieldDescription(prefix, fieldInfo, out fieldLabel);
                var otherField = FindField(expected.GetType(), fieldInfo.Name);
                if (otherField == null)
                {
                    // fields does not exist
                    if (!negated)
                    {
                        message = FluentMessage.BuildMessage(string.Format("The {{0}}'s {0} is absent from the {{1}}.", fieldLabel))
                                                .On(value)
                                                .And.Expected(expected)
                                                .ToString();
                    }

                    break;
                }

                // compare value
                var actualFieldValue = fieldInfo.GetValue(value);
                var expectedFieldValue = otherField.GetValue(expected);

                if (expectedFieldValue == null)
                {
                    if ((actualFieldValue == null) == negated)
                    {
                        if (!negated)
                        {
                            message = FluentMessage.BuildMessage(string.Format("The {{0}}'s {0} does not have the expected value.", fieldLabel))
                                                     .On(actualFieldValue)
                                                     .And.Expected(null)
                                                     .ToString();
                        }
                        else
                        {
                            message = FluentMessage.BuildMessage(string.Format("The {{0}}'s {0} has the same value in the comparand, whereas it must not.", fieldLabel))
                                                     .On(null)
                                                     .And.Expected(null)
                                                     .Comparison("different from")
                                                     .ToString();
                        }
                    }
                }
                else
                {
                    // determines how comparison will be made
                    if (!otherField.FieldType.ImplementsEquals())
                    {
                        // we recurse
                        message = CheckFieldEquality(
                            expectedFieldValue,
                            actualFieldValue,
                            negated,
                            string.Format("{0}.", fieldname));
                        if (!string.IsNullOrEmpty(message))
                        {
                            break;
                        }
                    }
                    else if (expectedFieldValue.Equals(actualFieldValue) == negated)
                    {
                        if (!negated)
                        {
                            message = FluentMessage.BuildMessage(string.Format("The {{0}}'s {0} does not have the expected value.", fieldLabel))
                                                 .On(actualFieldValue)
                                                 .And.Expected(expectedFieldValue)
                                                 .ToString();
                        }
                        else
                        {
                            message = FluentMessage.BuildMessage(string.Format("The {{0}}'s {0} has the same value in the comparand, whereas it must not.", fieldLabel))
                                                 .On(actualFieldValue)
                                                 .And.Expected(expectedFieldValue)
                                                 .Comparison("different from")
                                                 .ToString();
                        }

                        break;
                    }
                }
            }

            return message;
        }

        private static string BuildFieldDescription(
            string prefix, FieldInfo fieldInfo, out string fieldLabel)
        {
            var fieldname = fieldInfo.Name;
            var result = AutoPropertyMask.Match(fieldname);
            if (result.Length > 0 && result.Groups.Count == 2)
            {
                fieldname = string.Format(
                    "{0}{1}", prefix, fieldname.Substring(result.Groups[1].Index, result.Groups[1].Length));

                fieldLabel = string.Format("autoproperty '{0}' (field '{1}')", fieldname, fieldInfo.Name);
            }
            else
            {
                result = AnonymousTypeFieldMask.Match(fieldname);
                if (result.Length > 0 && result.Groups.Count == 2)
                {
                    fieldname = string.Format(
                        "{0}{1}",
                        prefix,
                        fieldname.Substring(result.Groups[1].Index, result.Groups[1].Length));

                    fieldLabel = string.Format("field '{0}'", fieldname);
                }
                else
                {
                    fieldname = string.Format("{0}{1}", prefix, fieldname);
                    fieldLabel = string.Format("field '{0}'", fieldname);
                }
            }

            return fieldname;
        }

        private static FieldInfo FindField(Type type, string name)
        {
            var result = type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
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
