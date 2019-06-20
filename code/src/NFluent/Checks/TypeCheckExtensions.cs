// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeCheckExtensions.cs" company="">
//   Copyright 2014 Cyrille DUPUYDAUBY
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
    using System;
    using Extensibility;
    using Extensions;
    
    /// <summary>
    /// This class hosts Type related checks.
    /// </summary>
    public static class TypeCheckExtensions
    {
        /// <summary>
        /// Checks if a given type has an attribute of a given type.
        /// </summary>
        /// <typeparam name="T">Expected attribute type.</typeparam>
        /// <param name="check">Check component.</param>
        /// <returns>A check to link checks.</returns>
        public static ICheckLink<ICheck<Type>> HasAttribute<T>(this ICheck<Type> check) where T : Attribute
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailIfNull()
                .FailWhen(
                    sut => !sut.TypeHasAttribute(typeof(T)),
                    "The {0} does not have an attribute of the expected type.")
                .OnNegate("The {0} does have an attribute of type {1} where as it should not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }
    }
}
