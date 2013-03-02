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
        /// Checks that an object is equal to another instance, and throws a <see cref="FluentAssertionException"/> if they are not equal.
        /// This method is provided as a replacement to the classic Equals() method because we need it to throw exception with proper message (which is of course not the case of the .NET Equals methods).
        /// </summary>
        /// <remarks>This method is not named EqualsOrThrowException to ensure english readability when used within an Assert.That() statement.</remarks>
        /// <param name="obj">The current object instance.</param>
        /// <param name="expected">The object that we expect to be equal.</param>
        /// <returns><c>true</c> if the two objects are equal, or throws a <see cref="FluentAssertionException"/> otherwise.</returns>
        /// <exception cref="NFluent.FluentAssertionException">The two objects are not equal.</exception>
        public static void IsEqualTo(object sut, object expected)
        {
            if (!object.Equals(sut, expected))
            {
                throw new FluentAssertionException(string.Format("[{0}] not equals to the expected [{1}]", sut.ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
            }
        }

        /// <summary>
        /// Checks that an object is NOT equal to another instance, and throws a <see cref="FluentAssertionException"/> if they are equal.
        /// This method is provided as a replacement to the classic Equals() method because we need it to throw exception with proper message (which is of course not the case of the .NET Equals methods).
        /// </summary>
        /// <remarks>This method is not named NotEqualsOrThrowException to ensure english readability when used within an Assert.That() statement.</remarks>
        /// <param name="obj">The current object instance.</param>
        /// <param name="expected">The object that we expect to be NOT equal.</param>
        /// <returns><c>true</c> if the two objects are not equal, or throws a <see cref="FluentAssertionException"/> otherwise.</returns>
        /// <exception cref="NFluent.FluentAssertionException">The two objects are equal.</exception>
        public static void IsNotEqualTo(object sut, object expected)
        {
            if (object.Equals(sut, expected))
            {
                throw new FluentAssertionException(string.Format("[{0}] equals to the value [{1}] which is not expected.", sut.ToStringProperlyFormated(), expected.ToStringProperlyFormated()));
            }
        }
    }
}