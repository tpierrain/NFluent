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

    using NFluent.Extensions;

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
            if (instanceType != expectedType)
            {
                throw new FluentAssertionException(BuildErrorMessageForNullable(instanceType, expectedType, value, false));
            }
        }

        /// <summary>
        /// Checks that an instance is of the given expected type.
        /// </summary>
        /// <param name="instance">The instance to be checked.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <exception cref="FluentAssertionException">The instance is not of the expected type.</exception>
        public static void IsInstanceOf(object instance, Type expectedType)
        {
            if (instance.GetType() != expectedType)
            {
                throw new FluentAssertionException(BuildErrorMessage(instance, expectedType, false));
            }
        }

        /// <summary>
        /// Checks that an instance is not of the given expected type.
        /// </summary>
        /// <param name="instance">The instance to be checked.</param>
        /// <param name="typeNotExpected">The type not expected.</param>
        /// <exception cref="FluentAssertionException">The instance is of the type not expected.</exception>
        public static void IsNotInstanceOf(object instance, Type typeNotExpected)
        {
            if (instance.GetType() == typeNotExpected)
            {
                throw new FluentAssertionException(BuildErrorMessage(instance, typeNotExpected, true));
            }
        }

        /// <summary>
        /// Checks that an instance is in the inheritance hierarchy of a specified type.
        /// </summary>
        /// <param name="instance">The instance to be checked.</param>
        /// <param name="expectedBaseType">The Type which is expected to be a base Type of the instance.</param>
        /// <exception cref="FluentAssertionException">The instance is not in the inheritance hierarchy of the specified type.</exception>
        public static void InheritsFrom(object instance, Type expectedBaseType)
        {
            var instanceType = instance.GetTypeWithoutThrowingException();
            if (expectedBaseType.IsAssignableFrom(instanceType))
            {
                return;
            }
            var message =
                FluentMessage.BuildMessage("The {0} does not have the expected inheritance.")
                             .For("expression type")
                             .On(instanceType)
                             .Label("Indeed, the {0} {1}")
                             .Expected(expectedBaseType)
                             .Label("is not a derived type of");
            throw new FluentAssertionException(message.ToString());
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
            if (isSameType)
            {
                var message = FluentMessage.BuildMessage(string.Format("The {{0}} is an instance of {0} whereas it must not.", typeOperand)).On(value).WithType().Expected(typeOperand).Label("The {0} type: different from");
                return message.ToString();
            }
            else
            {
                var message = FluentMessage.BuildMessage("The {0} is not an instance of the expected type.").On(value).WithType().Expected(typeOperand).Label("The {0} type:");
                return message.ToString();
            }
        }

        public static string BuildErrorMessageForNullable(Type instanceType, Type expectedType, object value, bool isSameType)
        {
            if (isSameType)
            {
                return string.Format("\nThe actual value:\n\t[{0}]\nis an instance of:\n\t[{1}]\nwhich was not expected.", value.ToStringProperlyFormated(), instanceType.ToStringProperlyFormated());
            }
            else
            {
                return string.Format("\nThe actual value:\n\t[{0}]\nis not an instance of:\n\t[{1}]\nbut an instance of:\n\t[{2}]\ninstead.", value.ToStringProperlyFormated(), expectedType.ToStringProperlyFormated(), instanceType.ToStringProperlyFormated());
            }
        }
    }
}