﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ExtensibilityHelper.cs" company="">
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
namespace NFluent.Extensibility
{
    /// <summary>
    /// Helper that allow to extract the checker to be used for and from any given fluent check instance.
    /// </summary>
    public static class ExtensibilityHelper
    {
        /// <summary>
        /// Extracts the checker to be used in order to check things on the value contained within
        /// the given fluent check.
        /// </summary>
        /// <typeparam name="T">The type of checked value.</typeparam>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <returns>
        /// The checker to be used to check things on the value contained in the fluent check.
        /// </returns>
        public static IChecker<T, ICheck<T>> ExtractChecker<T>(ICheck<T> check)
        {
            // ok this is a crappy cast, but it's for the good cause here (i.e. a clean and virgin intellisense for users)
            return (check as ICheckForExtensibility<T, ICheck<T>>).Checker;
        }

        /// <summary>
        /// Extracts the code checker.
        /// </summary>
        /// <typeparam name="TU">The type of checked value.</typeparam>
        /// <param name="check">The check.</param>
        /// <returns>The checker to be used to check things on the value contained in the fluent check.</returns>
        public static IChecker<TU, ICodeCheck<TU>> ExtractCodeChecker<TU>(ICodeCheck<TU> check) where TU : RunTrace
        {
            // ok this is a crappy cast, but it's for the good cause here (i.e. a clean and virgin intellisense for users)
            return (check as ICheckForExtensibility<TU, ICodeCheck<TU>>).Checker;
        }

        /// <summary>
        /// Extracts the checker to be used in order to check things on the struct instance contained within
        /// the given fluent check.
        /// </summary>
        /// <typeparam name="TS">The type of the struct to be checked.</typeparam>
        /// <param name="check">The fluent check instance to work on.</param>
        /// <returns>
        /// The checker to be used to check things on the value contained in the fluent check.
        /// </returns>
        public static IChecker<TS, IStructCheck<TS>> ExtractStructChecker<TS>(IStructCheck<TS> check) where TS : struct
        {
            // ok this is a crappy cast, but it's for the good cause here (i.e. a clean and virgin intellisense for users)
            return (check as ICheckForExtensibility<TS, IStructCheck<TS>>).Checker;
        }
    }
}