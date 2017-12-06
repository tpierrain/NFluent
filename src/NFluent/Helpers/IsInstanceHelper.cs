// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsInstanceHelper.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace NFluent.Helpers
{
    using System;

    using Extensibility;
    using Extensions;
#if NETSTANDARD1_3
    using System.Reflection;
#endif

    /// <summary>
    /// Helper class related to Is(Not)Instance methods (used like a traits).
    /// </summary>
    internal static class IsInstanceHelper
    {
        /// <summary>
        /// Checks that a type is the same as the expected one.
        /// </summary>
        /// <param name="instanceType">The type of the instance to be checked.</param>
        /// <param name="expectedType">The expected type for the instance to be checked.</param>
        /// <param name="value">The value of the instance to be checked (may be a nullable instance).</param>
        public static void IsSameType(Type instanceType, Type expectedType, object value)
        {
            if (instanceType != expectedType || (value == null && !instanceType.IsNullable()))
            {
                throw new FluentCheckException(BuildErrorMessageForNullable(instanceType, expectedType, value, false));
            }
        }

        /// <summary>
        /// Checks that a type is not the expected one.
        /// </summary>
        /// <param name="instanceType">The type of the instance to be checked.</param>
        /// <param name="expectedType">The expected type for the instance to be checked.</param>
        /// <param name="value">The value of the instance to be checked (may be a nullable instance).</param>
        public static void IsDifferentType(Type instanceType, Type expectedType, object value)
        {
            if (instanceType == expectedType)
            {
                throw new FluentCheckException(BuildErrorMessageForNullable(instanceType, expectedType, value, true));
            }
        }

        /// <summary>
        /// Checks that an instance is of the given expected type.
        /// </summary>
        /// <param name="instance">The instance to be checked.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <exception cref="FluentCheckException">The instance is not of the expected type.</exception>
        public static void IsInstanceOf(object instance, Type expectedType)
        {
            if (instance.GetTypeWithoutThrowingException() != expectedType)
            {
                throw new FluentCheckException(BuildErrorMessage(instance, expectedType, false));
            }
        }

        /// <summary>
        /// Checks that an instance is not of the given expected type.
        /// </summary>
        /// <param name="instance">The instance to be checked.</param>
        /// <param name="typeNotExpected">The type not expected.</param>
        /// <exception cref="FluentCheckException">The instance is of the type not expected.</exception>
        public static void IsNotInstanceOf(object instance, Type typeNotExpected)
        {
            if (instance.GetTypeWithoutThrowingException() == typeNotExpected)
            {
                throw new FluentCheckException(BuildErrorMessage(instance, typeNotExpected, true));
            }
        }

        /// <summary>
        /// Checks that an instance is in the inheritance hierarchy of a specified type.
        /// </summary>
        /// <param name="checker">The instance to be checked.</param>
        /// <param name="expectedBaseType">The Type which is expected to be a base Type of the instance.</param>
        /// <exception cref="FluentCheckException">The instance is not in the inheritance hierarchy of the specified type.</exception>
        public static void InheritsFrom<T>(IChecker<T, ICheck<T>> checker, Type expectedBaseType)
        {
            var instanceType = checker.Value.GetTypeWithoutThrowingException();
            if (expectedBaseType.IsAssignableFrom(instanceType))
            {
                return;
            }

            var message =
                checker.BuildMessage("The {0} does not have the expected inheritance.")
                    .For("expression type")
                    .On(instanceType)
                    .And.Expected(expectedBaseType)
                    .Comparison("inherits from");
            
            throw new FluentCheckException(message.ToString());
        }

        /// <summary>
        /// Builds the error message related to the type comparison. This should be called only if the test failed (no matter it is negated or not).
        /// Warning: Should not call this method with nullable types. Indeed, the Nullable types are treated specially by CLR and it is impossible to have a boxed instance of a nullable type.
        /// Instead, boxing a nullable type will result in a null reference (if HasValue is false), or the boxed value (if there is a value).
        /// </summary>
        /// <param name="value">The checked value.</param>
        /// <param name="typeOperand">The other type operand.</param>
        /// <param name="isSameType">A value indicating whether the two types are identical or not. <c>true</c> if they are equal; <c>false</c> otherwise.</param>
        /// <returns>
        /// The error message related to the type comparison.
        /// </returns>
        public static string BuildErrorMessage(object value, Type typeOperand, bool isSameType)
        {
            MessageBlock message;
            if (isSameType)
            {
                message = FluentMessage.BuildMessage(
                        $"The {{0}} is an instance of [{typeOperand.ToStringProperlyFormatted()}] whereas it must not.")
                                       .For("value")
                                       .On(value)
                                       .WithType()
                                       .And.ExpectedType(typeOperand)
                                       .WithType()
                                       .Comparison("different from");
            }
            else if (value != null && value.GetType().FullName == typeOperand.FullName)
            {
                // cannot discriminate from type name
                message = FluentMessage.BuildMessage("The {0} .")
                                       .On(value)
                                       .WithType(true, true)
                                       .And.ExpectedType(typeOperand)
                                       .WithType(true, true);
            }
            else
            {
                message =
                    FluentMessage.BuildMessage("The {0} is not an instance of the expected type.")
                                 .On(value)
                                 .WithType()
                                 .And.ExpectedType(typeOperand)
                                 .WithType();
            }
        
            return message.ToString();
        }

        public static string BuildErrorMessageForNullable(Type instanceType, Type expectedType, object value, bool isSameType)
        {
            MessageBlock message;
            if (isSameType)
            {
                message = FluentMessage.BuildMessage(
                        $"The {{0}} is an instance of [{expectedType.ToStringProperlyFormatted()}] whereas it must not.")
                    .For("value")
                    .On(value)
                    .OfType(instanceType)
                    .And.ExpectedType(expectedType)
                    .Comparison("different from").WithType();
            }
            else
            {
                message = FluentMessage.BuildMessage(
                        $"The {{0}} is not an instance of [{expectedType.ToStringProperlyFormatted()}].")
                    .For("value")
                    .On(value)
                    .OfType(instanceType)
                    .And.ExpectedType(expectedType).WithType();
            }

            return message.ToString();
        }
    }
}