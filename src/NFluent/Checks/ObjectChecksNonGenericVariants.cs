// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ObjectChecksNonGenericVariants.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
    using Helpers;

    /// <summary>
    /// Hosts non generic variants of some checks.
    /// </summary>
    public static class ObjectChecksNonGenericVariants
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="check"></param>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICheckLink<ICheck<T>> IsInstanceOfType<T>(this ICheck<T> check, Type type)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            if (typeof(T).IsNullable())
            {
                return checker.ExecuteCheck(
                    () => IsInstanceHelper.IsSameType(typeof(T), type, checker.Value), 
                    IsInstanceHelper.BuildErrorMessageForNullable(typeof(T), type, checker.Value, true));
            }

            return checker.ExecuteCheck(() => IsInstanceHelper.IsInstanceOf(checker.Value, type),
                IsInstanceHelper.BuildErrorMessage(checker.Value, type, true));
        }

    }
}