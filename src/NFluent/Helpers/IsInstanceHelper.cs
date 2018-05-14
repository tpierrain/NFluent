// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsInstanceHelper.cs" company="">
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
namespace NFluent.Helpers
{
    using System;

    using Extensibility;
    using Extensions;
#if NETSTANDARD1_3
    using System.Reflection;
#endif

    /// <summary>
    /// Helper class related to Is(Not)Instance methods (used like a traits).
    /// </summary>
    internal static class IsInstanceHelper
    {

        /// <summary>
        /// Checks that an instance is in the inheritance hierarchy of a specified type.
        /// </summary>
        /// <param name="checker">The instance to be checked.</param>
        /// <param name="expectedBaseType">The Type which is expected to be a base Type of the instance.</param>
        /// <exception cref="FluentCheckException">The instance is not in the inheritance hierarchy of the specified type.</exception>
        public static void InheritsFrom<T>(ICheck<T> checker, Type expectedBaseType)
        {
            ExtensibilityHelper.BeginCheck(checker)
                .CheckSutAttributes(sut => sut.GetTypeWithoutThrowingException(), "type")
                .DefineExpected(expectedBaseType, "inherits from", "does not inherits from")
                .FailWhen(sut => !expectedBaseType.IsAssignableFrom(sut), "The {0} does not have the expected inheritance.")
                .Negates("The {0} does inherits from the {1} where as it must not")
                .EndCheck();
        }

    }
}