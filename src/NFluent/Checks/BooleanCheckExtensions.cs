 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="BooleanCheckExtensions.cs" company="">
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
namespace NFluent
{
    using Extensibility;
    using Kernel;

    /// <summary>
    /// Provides check methods to be executed on a boolean value.
    /// </summary>
    public static class BooleanCheckExtensions
    {
        /// <summary>
        /// Checks that the actual value is true.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not true.</exception>
        public static ICheckLink<ICheck<bool>> IsTrue(this ICheck<bool> check)
        {
            return check.Not.IsFalse();
        }

        /// <summary>
        /// Checks that the actual value is false.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The actual value is not false.</exception>
        public static ICheckLink<ICheck<bool>> IsFalse(this ICheck<bool> check)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(sut => sut, "The {0} is true whereas it must be false.")
                .OnNegate("The {0} is false whereas it must be true.").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }
    }
}