﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="PersonExtensions.cs" company="">
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
namespace NFluent.Tests.Extensions
{
    using NFluent.Extensibility;

    /// <summary>
    /// Extensions methods to check <see cref="Person"/> type.
    /// </summary>
    public static class PersonExtensions
    {
        public static ICheckLink<ICheck<Person>> IsPortna(this ICheck<Person> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);

            return checker.ReturnValueForLinkage;
        }

        public static ICheckLink<ICheck<Person>> IsNawouak(this ICheck<Person> check)
        {
            var checker = ExtensibilityHelper.ExtractChecker(check);
            
            return checker.ReturnValueForLinkage;
        }
    }
}
