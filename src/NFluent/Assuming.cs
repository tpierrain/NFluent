// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Assume.cs" company="">
//   Copyright 2019 Cyrille DUPUYDAUBY
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
#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40
    using System.Threading.Tasks;
#endif
    using Kernel;

    /// <summary>
    /// Provides <see cref="ICheck{T}"/> instances to be used in order to make 
    /// assumption(s) on the provided value.
    /// </summary>
    public static class Assuming
    {
        /// <summary>
        /// Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given value.
        /// </summary>
        /// <typeparam name="T">Type of the value to be tested.</typeparam>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="ICheck{T}" /> instance to use in order to check things on the given value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        public static ICheck<T> That<T>(T value)
        {
            return new FluentCheck<T>(value, Reporter);
        }

#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40

        /// <summary>
        /// Returns a <see cref="FluentDynamicCheck"/> instance that will provide check for dynamics.
        /// </summary>
        /// <param name="value">The code to be tested.</param>
        /// <returns>
        /// A <see cref="FluentDynamicCheck" /> instance to use in order to assert things on the given test.
        /// </returns>
        public static FluentDynamicCheck ThatDynamic(dynamic value)
        {
            //return null;
            return new FluentDynamicCheck(value, Reporter);
        }
#endif

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
        public static ICodeCheck<RunTrace> ThatCode(Action value)
        {
            return new FluentCodeCheck<RunTrace>(FluentCodeCheck<RunTraceResult<Action>>.GetTrace(value), Reporter);
        }

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
        public static ICodeCheck<RunTraceResult<TU>> ThatCode<TU>(Func<TU> value)
        {
            return new FluentCodeCheck<RunTraceResult<TU>>(FluentCodeCheck<RunTraceResult<TU>>.GetTrace(value), Reporter);
        }

        /// <summary>
        /// Returns a <see cref="ICheck{T}"/> instance that will provide check method on a type.
        /// </summary>
        /// <typeparam name="T">Type to be tested.</typeparam>
        /// <returns>
        /// A <see cref="ICheck{Type}" /> instance to use in order to assert things on the given test.
        /// </returns>
        public static ICheck<Type> That<T>()
        {
            return new FluentCheck<Type>(typeof(T), Reporter);
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
        public static IStructCheck<T> ThatEnum<T>(T value)
            where T : struct
        {
            return new FluentStructCheck<T>(value, Reporter);
        }


        /// <summary>
        /// Defines a custom error message on error.
        /// </summary>
        /// <param name="message">custom error message.</param>
        public static NFluentEntryPoint WithCustomMessage(string message)
        {
            return new NFluentEntryPoint(message, Reporter);
        }

        /// <summary>
        /// Gets or sets the default error report
        /// </summary>
        public static IErrorReporter Reporter { get; set; } = new InconclusiveExceptionReporter();
    }
}