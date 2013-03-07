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
namespace NFluent
{
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
                throw new FluentAssertionException(string.Format("[{0}] not equals to the expected [{1}]", instance.ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
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
    }
}