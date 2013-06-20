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
    /// <summary>
    /// Helper class related to Equality methods (used like a traits).
    /// </summary>
    internal static class EqualityHelper
    {
        /// <summary>
        /// Checks that a given instance is considered to be equal to another expected instance. Throws <see cref="FluentCheckException"/> otherwise.
        /// </summary>
        /// <param name="instance">The considered instance.</param>
        /// <param name="expected">The expected instance.</param>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public static void IsEqualTo(object instance, object expected)
        {
            if (FluentEquals(instance, expected))
            {
                return;
            }

            // Should throw
            var errorMessage = BuildErrorMessage(instance, expected, false);

            throw new FluentCheckException(errorMessage);
        }

        public static bool FluentEquals(object instance, object expected)
        {
            return object.Equals(instance, expected);
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
            string message;
            if (isEqual)
            {
                message = FluentMessage.BuildMessage("The {0} is equal to the {1} whereas it must not.")
                    .Expected(expected)
                    .Comparison("different from")
                    .WithType()
                    .ToString();                
            }
            else
            {
                // shall we display the type as well?
                var withType = (instance != null && expected != null && instance.GetType() != expected.GetType()) || (instance == null) || (expected == null);

                // shall we display the hash too
                var withHash = instance != null && expected != null && instance.GetType() == expected.GetType() && instance.ToString() == expected.ToString();
                message = FluentMessage.BuildMessage("The {0} is different from the {1}.")
                    .On(instance)
                    .WithType(withType)
                    .WithHashCode(withHash)
                    .And.Expected(expected)
                    .WithType(withType)
                    .WithHashCode(withHash).ToString();                
            }

            return message;
        }

        /// <summary>
        /// Checks that a given instance is not considered to be equal to another expected instance. Throws <see cref="FluentCheckException"/> otherwise.
        /// </summary>
        /// <param name="instance">The considered instance.</param>
        /// <param name="expected">The expected instance.</param>
        /// <exception cref="FluentCheckException">The actual value is not equal to the expected value.</exception>
        public static void IsNotEqualTo(object instance, object expected)
        {
            if (object.Equals(instance, expected))
            {
                throw new FluentCheckException(BuildErrorMessage(instance, expected, true));
            }
        }

        // TODO: make internal methods visible?
        internal static string BuildTypeDescriptionMessage(object obj)
        {
            var expectedTypeMessage = string.Empty;
            if (obj != null)
            {
                    expectedTypeMessage = string.Format(" of type: [{0}]", obj.GetType());
            }

            return expectedTypeMessage;
        }
    }
}