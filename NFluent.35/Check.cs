// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="Check.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN, Rui CARVALHO
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
namespace NFluent
{
    using System;
    using System.ComponentModel;

#if (DOTNET_40)
    using System.Threading.Tasks;
#endif

    /// <summary>
    /// Provides <see cref="ICheck{T}"/> instances to be used in order to make 
    /// check(s) on the provided value.
    /// </summary>
    public static class Check
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
            return new FluentCheck<T>(value);
        }

#if (DOTNET_40)

        /// <summary>
        /// Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given async code (returning Task).
        /// </summary>
        /// <param name="awaitableMethod">The async code to be tested.</param>
        /// <returns>
        /// A <see cref="ICheck{RunTrace}" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        public static ICodeCheck<RunTrace> ThatAsyncCode(Func<Task> awaitableMethod)
        {
            return new FluentCodeCheck<RunTrace>(CodeCheckExtensions.GetAsyncTrace(awaitableMethod));
        }

        /// <summary>
        /// Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given async function (returning Task{TResult}).
        /// </summary>
        /// <typeparam name="TResult">The type of the result for this asynchronous function.</typeparam>
        /// <param name="awaitableFunction">The asynchronous function to be tested.</param>
        /// <returns>
        /// A <see cref="ICheck{RunTrace}" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        public static ICodeCheck<RunTraceResult<TResult>> ThatAsyncCode<TResult>(Func<Task<TResult>> awaitableFunction)
        {
            return new FluentCodeCheck<RunTraceResult<TResult>>(CodeCheckExtensions.GetAsyncTrace(awaitableFunction));
        }
#endif

#if !PORTABLE
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
            return new FluentCodeCheck<RunTrace>(CodeCheckExtensions.GetTrace(value));
        }

        /// <summary>
        /// Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given value.
        /// </summary>
        /// <typeparam name="TU">Result type of the function.</typeparam>
        /// <param name="value">The code to be tested.</param>
        /// <returns>
        /// A <see cref="ICheck{RunTrace}" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        public static ICodeCheck<RunTraceResult<TU>> ThatCode<TU>(Func<TU> value)
        {
            return new FluentCodeCheck<RunTraceResult<TU>>(CodeCheckExtensions.GetTrace(value));
        }

        /// <summary>
        /// Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given value.
        /// </summary>
        /// <typeparam name="T">Type of the value returned by the <see cref="System.Func{TResult}"/> to be checked.</typeparam>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="ILambdaCheck" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        //ncrunch: no coverage start
        // coverage disabled as this code cannot be executed and is to be removed at a later stage
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use ThatCode instead.", true)]
        public static ILambdaCheck That<T>(Func<T> value)
        {
            return null; //ncrunch: no coverage
        }

        /// <summary>
        /// Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="ILambdaCheck" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use ThatCode instead.", true)]
        public static ILambdaCheck That(Action value)
        {
            return null;
        }
        //ncrunch: no coverage end
#endif
        /// <summary>
        /// Returns a <see cref="IStructCheck{T}" /> instance that will provide check methods to be executed on a given enum or struct value.
        /// </summary>
        /// <typeparam name="T">Type of the enum or struct value to be tested.</typeparam>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        /// A <see cref="IStructCheck{T}" /> instance to use in order to assert things on the given enum or struct value.
        /// </returns>
        /// <remarks>
        /// Every method of the returned <see cref="IStructCheck{T}" /> instance will throw a <see cref="FluentCheckException" /> when failing.
        /// </remarks>
        public static IStructCheck<T> ThatEnum<T>(T value) where T : struct
        {
            return new FluentStructCheck<T>(value);
        }
    }
}