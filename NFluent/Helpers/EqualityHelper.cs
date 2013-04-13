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
                var expectedTypeMessage = string.Empty;
                var instanceTypeMessage = string.Empty;

                if (instance.GetTypeWithoutThrowingException() != expected.GetTypeWithoutThrowingException())
                {
                    expectedTypeMessage = BuildTypeDescriptionMessage(expected);
                    instanceTypeMessage = BuildTypeDescriptionMessage(instance);    
                }

                throw new FluentAssertionException(string.Format("\nExpecting:\n\t[{0}]{2}\n but was\n\t[{1}]{3}.", expected.ToStringProperlyFormated(), instance.ToStringProperlyFormated(), expectedTypeMessage, instanceTypeMessage));
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
                throw new FluentAssertionException(string.Format("[{0}] equals to the value [{1}] which is not expected.", instance.ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
            }
        }

        private static string BuildTypeDescriptionMessage(object expected)
        {
            string expectedTypeMessage = string.Empty;
            if (expected != null)
            {
                expectedTypeMessage = string.Format(" of type: {0}", expected.GetType());
            }

            return expectedTypeMessage;
        }
    }
}