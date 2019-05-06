// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NFluentEntryPoint.cs" company="NFluent">
//   Copyright 2019 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
#if !DOTNET_20
    using System;
 #endif   
    using Kernel;

    /// <summary>
    /// Provides NFluent entry points through an instance.
    /// </summary>
    /// <remarks>This is an helper class.</remarks>
    public class NFluentEntryPoint
    {
        private readonly string message;
        private readonly IErrorReporter reporter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="reporter"></param>
        public NFluentEntryPoint(string message, IErrorReporter reporter)
        {
            this.message = message;
            this.reporter = reporter;
        }

        /// <summary>
        /// Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given value.
        /// </summary>
        /// <typeparam name="T">Type of the value to be tested.</typeparam>
        /// <param name="sut">The value to be tested.</param>
        /// <returns>
        /// A <see cref="ICheck{T}" /> instance to use in order to check things on the given value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        public ICheck<T> That<T>(T sut)
        {
            return new FluentCheck<T>(sut, this.reporter) {CustomMessage = this.message};
        }

        /// <summary>
        /// Returns a <see cref="IStructCheck{T}" /> instance that will provide check methods to be executed on a given enum or struct value.
        /// </summary>
        /// <typeparam name="T">Type of the enum or structure value to be tested.</typeparam>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="IStructCheck{T}" /> instance to use in order to assert things on the given enum or struct value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="IStructCheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        public  IStructCheck<T> ThatEnum<T>(T value)
            where T : struct
        {
            return new FluentStructCheck<T>(value, this.reporter) {CustomMessage = this.message};
        }

        /// <summary>
        /// Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The code to be tested.</param>
        /// <returns>
        /// A <see cref="ICheck{RunTrace}" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        public ICheck<RunTrace> ThatCode(Action value)
        {
            return new FluentCheck<RunTrace>(RunTrace.GetTrace(value), this.reporter)
            {
                CustomMessage = this.message
            };
        }

#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public FluentDynamicCheck ThatDynamic(dynamic value)
        {
            return new FluentDynamicCheck(value, this.reporter) {CustomMessage = this.message};
        }
#endif
        /// <summary>
        /// Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a lambda.
        /// </summary>
        /// <typeparam name="TU">Result type of the function.</typeparam>
        /// <param name="value">The code to be tested.</param>
        /// <returns>
        /// A <see cref="ICheck{RunTrace}" /> instance to use in order to assert things on the lambda.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        public ICheck<RunTraceResult<TU>> ThatCode<TU>(Func<TU> value)
        {
            return new FluentCheck<RunTraceResult<TU>>(RunTrace.GetTrace(value), this.reporter)
            {
                CustomMessage = this.message
            };
        }

    }
}