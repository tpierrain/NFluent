// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EqualityHelper.cs" company="">
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
namespace NFluent.Helpers
{
    using System;

    using NFluent.Extensions;

    /// <summary>
    /// Helper class related to Equality methods (used like a traits).
    /// </summary>
    public static class EqualityHelper
    {
        /// <summary>
        /// Checks that a given instance is considered to be equal to another expected instance. Throws <see cref="FluentAssertionException"/> otherwise.
        /// </summary>
        /// <param name="instance">The considered instance.</param>
        /// <param name="expected">The expected instance.</param>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public static void IsEqualTo(object instance, object expected)
        {
            if (!object.Equals(instance, expected))
            {
                // Should throw
                var errorMessage = BuildErrorMessage(instance, expected, false);

                throw new FluentAssertionException(errorMessage);
            }
        }

        /// <summary>
        /// Builds the error message related to the Equality verification. This should be called only if the test failed (no matter it is negated or not).
        /// </summary>
        /// <param name="instance">The checked instance.</param>
        /// <param name="expected">The other operand.</param>
        /// <param name="isEqual">A value indicating whether the two values are equal or not. <c>true</c> if they are equal; <c>false</c> otherwise.</param>
        /// <returns>The error message related to the Equality verification.</returns>
        public static string BuildErrorMessage(object instance, object expected, bool isEqual)
        {
            var expectedTypeMessage = string.Empty;
            var instanceTypeMessage = string.Empty;
            bool includeHashCode = false;

            if (instance.GetTypeWithoutThrowingException() != expected.GetTypeWithoutThrowingException())
            {
                expectedTypeMessage = BuildTypeDescriptionMessage(expected, includeHashCode);
                instanceTypeMessage = BuildTypeDescriptionMessage(instance, includeHashCode);
            }
            else
            {
                // same instance type. Do they have the same ToString() value? In that case we should include the hashcodex of each instance within the error message
                if (string.Compare(instance.ToString(), expected.ToString()) == 0)
                {
                    includeHashCode = true;
                    expectedTypeMessage = BuildTypeDescriptionMessage(expected, includeHashCode);
                    instanceTypeMessage = BuildTypeDescriptionMessage(instance, includeHashCode);
                }
            }

            string errorMessage;
            if (isEqual)
            {
                errorMessage = string.Format("\nThe actual value:\n\t[{0}]{2}\nis equal to:\n\t[{1}]{3}\nwhich is unexpected.", instance.ToStringProperlyFormated(), expected.ToStringProperlyFormated(), instanceTypeMessage, expectedTypeMessage);
            }
            else
            {
                errorMessage = string.Format("\nThe actual value:\n\t[{0}]{2}\nis not equal to the expected one:\n\t[{1}]{3}.", instance.ToStringProperlyFormated(), expected.ToStringProperlyFormated(), instanceTypeMessage, expectedTypeMessage); 
            }

            return errorMessage;
        }

        /// <summary>
        /// Checks that a given instance is not considered to be equal to another expected instance. Throws <see cref="FluentAssertionException"/> otherwise.
        /// </summary>
        /// <param name="instance">The considered instance.</param>
        /// <param name="expected">The expected instance.</param>
        /// <exception cref="FluentAssertionException">The actual value is not equal to the expected value.</exception>
        public static void IsNotEqualTo(object instance, object expected)
        {
            if (object.Equals(instance, expected))
            {
                var instanceTypeMessage = BuildTypeDescriptionMessage(expected, false);
                throw new FluentAssertionException(string.Format("\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[{0}]{1}.", instance.ToStringProperlyFormated(), instanceTypeMessage));
            }
        }

        // TODO: make internal methods visible?
        internal static string BuildTypeDescriptionMessage(object obj)
        {
            return BuildTypeDescriptionMessage(obj, false);
        }

        internal static string BuildTypeDescriptionMessage(object obj, bool includeHashCode)
        {
            string expectedTypeMessage = string.Empty;
            if (obj != null)
            {
                if (includeHashCode)
                {
                    expectedTypeMessage = string.Format(" of type: [{0}] with HashCode: [{1}]", obj.GetType(), obj.GetHashCode());
                }
                else
                {
                    expectedTypeMessage = string.Format(" of type: [{0}]", obj.GetType());
                }
            }

            return expectedTypeMessage;
        }
    }
}