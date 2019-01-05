// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="">
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

namespace NFluent.Extensions
{
    using System;
#if DOTNET_45 || NETSTANDARD1_3
    using System.Reflection;
#endif
#if !DOTNET_30 && !DOTNET_20
    using System.Linq;
#endif

    internal static class ObjectExtensions
    {

        /// <summary>
        /// Gets the type of the specified reference, or null if it is null.
        /// </summary>
        /// <param name="reference">The reference we interested in retrieving the type (may be null).</param>
        /// <returns>
        /// The type of the specified reference, or null if the reference is null.
        /// </returns>
        public static Type GetTypeWithoutThrowingException<T>(this T reference)
        {
            var defaultType = typeof(T);
            if (defaultType.IsNullable())
            {
                return defaultType;
            }
            return reference?.GetType() ?? defaultType;
        }

#if !NETSTANDARD1_3 && !DOTNET_45
        /// <summary>
        /// Stub implementation for GetTypeInfo() for Net Framework.
        /// </summary>
        /// <param name="type">Type to dig into.</param>
        /// <returns>An instance allowing to use reflection.</returns>
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }
#endif
        /// <summary>
        /// Checks if a type has at least one attribute of a give type.
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <param name="attribute">Attribute type to check for.</param>
        /// <returns>True if <paramref name="type"/> cref="type"/> has a least one attribute of type <paramref name="attribute"/>, false otherwise.</returns>
        public static bool TypeHasAttribute(this Type type, Type attribute)
        {
            return type.GetTypeInfo().GetCustomAttributes(false)
                .Any(customAttribute => customAttribute.GetType() == attribute);
        }
    }
}