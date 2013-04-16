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
    internal static class EqualityHelper
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

                throw new FluentAssertionException(string.Format("\nWas:\n\t[{0}]{2}\ninstead of the expected:\n\t[{1}]{3}.", instance.ToStringProperlyFormated(), expected.ToStringProperlyFormated(), instanceTypeMessage, expectedTypeMessage));
            }
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
                throw new FluentAssertionException(string.Format("\nThe variable is equal to the unexpected value:\n\t[{0}]{1}.", instance.ToStringProperlyFormated(), instanceTypeMessage));
            }
        }

        internal static string BuildTypeDescriptionMessage(object expected)
        {
            return BuildTypeDescriptionMessage(expected, false);
        }

        internal static string BuildTypeDescriptionMessage(object expected, bool includeHashCode)
        {
            string expectedTypeMessage = string.Empty;
            if (expected != null)
            {
                if (includeHashCode)
                {
                    expectedTypeMessage = string.Format(" of type: [{0}] with HashCode: [{1}]", expected.GetType(), expected.GetHashCode());
                }
                else
                {
                    expectedTypeMessage = string.Format(" of type: [{0}]", expected.GetType());
                }
            }

            return expectedTypeMessage;
        }
    }
}