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
#if !DOTNET_35
    using System.Threading.Tasks;
#endif
    internal static class ObjectExtensions
    {
#if !DOTNET_45
        /// <summary>
        /// Stub implementation for GetTypeInfo() for Net Framework.
        /// </summary>
        /// <param name="instance">Type to dig into.</param>
        /// <returns>An instance allowing to use reflection.</returns>
        public static Type GetTypeInfo(this Type instance)
        {
            return instance;
        }
#endif
        /// <summary>
        /// Gets the instance of the specified reference, or null if it is null.
        /// </summary>
        /// <param name="reference">The reference we interested in retrieving the instance (may be null).</param>
        /// <returns>
        /// The instance of the specified reference, or null if the reference is null.
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

        /// <summary>
        /// Returns true if the provided instance implements IEnumerable, disregarding well known enumeration (string).
        /// </summary>
        /// <typeparam name="T">instance of the provided instance</typeparam>
        /// <param name="instance">instance to asses</param>
        /// <param name="evenWellKnown">treat well known enumerations (string) as enumeration as well</param>
        /// <returns>true is <see paramref="instance"/> should treated as an enumeration.</returns>
        public static bool IsAnEnumeration<T>(this T instance, bool evenWellKnown)
        {
            return  instance!= null &&  instance.GetTypeWithoutThrowingException().IsAnEnumeration(evenWellKnown);
        }

        public static bool IsAwaitable<T>(this T instance, out Action waiter)
        {
#if !DOTNET_35
            if (instance is Task ta && ta.Status != TaskStatus.Created)
            {
                waiter = () => ta.Wait(); 
                return true;
            }
#endif
            waiter = () =>{};
            return false;
        }
    }
}