// -------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionChecks.cs" company="">
//   Copyright 2017 Cyrille Dupuydauby
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.ApiChecks
{
    using System;
    using System.Diagnostics;
    using Kernel;

    /// <summary>
    ///     This class hosts extensions methods related to exception related checks for APIs/lib.
    /// </summary>
    public static class ExceptionChecks
    {
        /// <summary>
        /// Provides access to the error message of the exception.
        /// </summary>
        /// <typeparam name="T"> Exception type.
        /// </typeparam>
        /// <param name="check">
        /// Exception checker.
        /// </param>
        /// <returns>
        /// A string checker.
        /// </returns>
        public static ICheck<string> AndWhichMessage<T>(this ILambdaExceptionCheck<T> check)
            where T : Exception
        {
            var checker = check as LambdaExceptionCheck<T>;
            return new FluentCheck<string>(checker.Value.Message);
        }
    }
}