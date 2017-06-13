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
#if DOTNET_45 || NETSTANDARD1_3
    using System.Reflection;
#endif
#if !DOTNET_30 && !DOTNET_20
    using System.Linq;
#endif

    using NFluent.Extensibility;
    using NFluent.Extensions;
    
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
            var checker = ExtensibilityHelper.ExtractChecker(check);
            var found = checker.Value.GetTypeInfo().GetCustomAttributes(false).Any(customAttribute => customAttribute.GetType() == typeof(T));
            return checker.ExecuteCheck(
                () =>
                    {
                        if (!found)
                        {
                            throw new FluentCheckException(checker.BuildMessage("The {0} does not have an attribute of the expected type.").ToString());
                        }
                    },
                checker.BuildMessage("The {0} does have an attribute of type {1} where as it should not.").ToString());
        }
    }
}
