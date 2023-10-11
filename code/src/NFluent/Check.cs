// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Check.cs" company="NFluent">
//   Copyright 2021 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Extensibility;
    using Extensions;
    using Helpers;
    using Kernel;
#if !DOTNET_35
    using System.Threading.Tasks;
#endif

    /// <summary>
    ///     Provides <see cref="ICheck{T}" /> instances to be used in order to make
    ///     check(s) on the provided value.
    /// </summary>
    public static class Check
    {
        internal static readonly ContextualizedSingleton<IErrorReporter> ReporterStore = new ContextualizedSingleton<IErrorReporter>()
            { DefaultValue = new ExceptionReporter() };

        /// <summary>
        ///     Gets/sets how equality comparison are done.
        /// </summary>
        public static EqualityMode EqualMode { get; set; }

        /// <summary>
        ///     Gets or sets the default error report
        /// </summary>
        public static IErrorReporter Reporter
        {
            get => ReporterStore.Value;
            set => ReporterStore.DefaultValue = value;
        }

        /// <summary>
        ///     Set the error reporter and returns an <see cref="IDisposable" /> that restores the previous reporter (when
        ///     disposed)
        /// </summary>
        /// <param name="newReporter">error reporter to use for the following checks</param>
        /// <returns>An <see cref="IDisposable" /> instance that will restore the previous reporter on dispose.</returns>
        public static IDisposable ChangeReporterForScope(IErrorReporter newReporter)
        {
            return ReporterStore.ScopedCustomization(newReporter);
        }

        /// <summary>
        ///     Gets/Sets the truncation length for long string.
        /// </summary>
        public static int StringTruncationLength
        {
            get => ExtensionsCommonHelpers.StringTruncationLength;
            set => ExtensionsCommonHelpers.StringTruncationLength = value;
        }

        /// <summary>
        ///     Defines a custom error message on error.
        /// </summary>
        /// <param name="message">custom error message.</param>
        public static NFluentEntryPoint WithCustomMessage(string message)
        {
            return new NFluentEntryPoint(message, Reporter);
        }

        /// <summary>
        ///     Registers a custom comparer for a given type.
        /// </summary>
        /// <typeparam name="T">Type to register comparer for.</typeparam>
        /// <param name="comparer">Comparer implementation</param>
        /// <returns>Previous registered comparer, or null.</returns>
        public static IEqualityComparer RegisterComparer<T>(IEqualityComparer comparer)
        {
            return EqualityHelper.RegisterComparer<T>(comparer);
        }

        /// <summary>
        ///     Registers a custom comparer for a given type for a local scope, using IDisposable pattern
        /// </summary>
        /// <typeparam name="T">Type to register comparer for.</typeparam>
        /// <param name="comparer">Comparer implementation</param>
        /// <returns>A disposable object that de-register the comparer when disposed.</returns>
        public static IDisposable RegisterLocalComparer<T>(IEqualityComparer comparer)
        {
            return new LocalRegisterHandler<T>(comparer);
        }

        /// <summary>
        ///     Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given value.
        /// </summary>
        /// <typeparam name="T">Type of the value to be tested.</typeparam>
        /// <param name="value">The value to be tested.</param>
        /// <returns>
        ///     A <see cref="ICheck{T}" /> instance to use in order to check things on the given value.
        /// </returns>
        /// <remarks>
        ///     Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" />
        ///     when failing.
        /// </remarks>
        public static ICheck<T> That<T>(T value)
        {
            return new FluentCheck<T>(value);
        }

        /// <summary>
        ///     Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given value.
        /// </summary>
        /// <param name="value">The code to be tested.</param>
        /// <returns>
        ///     A <see cref="ICheck{RunTrace}" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        ///     Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" />
        ///     when failing.
        /// </remarks>
        public static ICheck<RunTrace> ThatCode(Action value)
        {
            return new FluentCheck<RunTrace>(RunTrace.GetTrace(value), Reporter);
        }

        /// <summary>
        ///     Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a lambda.
        /// </summary>
        /// <typeparam name="TU">Result type of the function.</typeparam>
        /// <param name="value">The code to be tested.</param>
        /// <returns>
        ///     A <see cref="ICheck{RunTrace}" /> instance to use in order to assert things on the lambda.
        /// </returns>
        /// <remarks>
        ///     Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" />
        ///     when failing.
        /// </remarks>
        public static ICheck<RunTraceResult<TU>> ThatCode<TU>(Func<TU> value)
        {
            return new FluentCheck<RunTraceResult<TU>>(RunTrace.GetTrace(value), Reporter);
        }

        /// <summary>
        ///     Returns a <see cref="ICheck{T}" /> instance that will provide check method on a type.
        /// </summary>
        /// <typeparam name="T">Type to be tested.</typeparam>
        /// <returns>
        ///     A <see cref="ICheck{Type}" /> instance to use in order to assert things on the given test.
        /// </returns>
        public static ICheck<Type> That<T>()
        {
            return new FluentCheck<Type>(typeof(T));
        }

#if !DOTNET_35
        /// <summary>
        ///     Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given value.
        /// </summary>
        /// <param name="awaitableFunc">The code to be tested.</param>
        /// <returns>
        ///     A <see cref="ICheck{RunTrace}" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        ///     Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" />
        ///     when failing.
        /// </remarks>
        public static ICheck<RunTrace> ThatCode(Func<Task> awaitableFunc)
        {
            return new FluentCheck<RunTrace>(RunTrace.GetTrace(awaitableFunc), Reporter);
        }

        /// <summary>
        ///     Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a lambda.
        /// </summary>
        /// <typeparam name="TU">Result type of the function.</typeparam>
        /// <param name="awaitableFunc">The code to be tested.</param>
        /// <returns>
        ///     A <see typeref="ICheck{RunTraceResult{TU}}" /> instance to use in order to assert things on the lambda.
        /// </returns>
        /// <remarks>
        ///     Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" />
        ///     when failing.
        /// </remarks>
        public static ICheck<RunTraceResult<TU>> ThatCode<TU>(Func<Task<TU>> awaitableFunc)
        {
            return new FluentCheck<RunTraceResult<TU>>(RunTrace.GetAsyncTrace(awaitableFunc), Reporter);
        }

        /// <summary>
        ///     Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given async code
        ///     (returning Task).
        /// </summary>
        /// <param name="awaitableMethod">The async code to be tested.</param>
        /// <returns>
        ///     A <see cref="ICheck{RunTrace}" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        ///     Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" />
        ///     when failing.
        /// </remarks>
        [ExcludeFromCodeCoverage]
        [Obsolete("Use ThatCode instead.")]
        public static ICheck<RunTrace> ThatAsyncCode(Func<Task> awaitableMethod)
        {
            return new FluentCheck<RunTrace>(RunTrace.GetTrace(awaitableMethod), Reporter);
        }

        /// <summary>
        ///     Returns a <see cref="ICheck{T}" /> instance that will provide check methods to be executed on a given async
        ///     function (returning Task{TResult}).
        /// </summary>
        /// <typeparam name="TResult">The type of the result for this asynchronous function.</typeparam>
        /// <param name="awaitableFunction">The asynchronous function to be tested.</param>
        /// <returns>
        ///     A <see cref="ICheck{RunTrace}" /> instance to use in order to assert things on the given value.
        /// </returns>
        /// <remarks>
        ///     Every method of the returned <see cref="ICheck{T}" /> instance will throw a <see cref="FluentCheckException" />
        ///     when failing.
        /// </remarks>
        [ExcludeFromCodeCoverage]
        [Obsolete("Use ThatCode instead.")]
        public static ICheck<RunTraceResult<TResult>> ThatAsyncCode<TResult>(Func<Task<TResult>> awaitableFunction)
        {
            return new FluentCheck<RunTraceResult<TResult>>(RunTrace.GetAsyncTrace(awaitableFunction), Reporter);
        }

        /// <summary>
        ///     Returns a <see cref="FluentDynamicCheck" /> instance that will provide check for dynamics.
        /// </summary>
        /// <param name="value">The code to be tested.</param>
        /// <returns>
        ///     A <see cref="FluentDynamicCheck" /> instance to use in order to assert things on the given test.
        /// </returns>
        public static FluentDynamicCheck ThatDynamic(dynamic value)
        {
            return new FluentDynamicCheck(value, Reporter);
        }
#endif
        /// <summary>
        /// Starts a batch of checks.
        /// </summary>
        /// <paramref name="message">Error message</paramref>/>
        /// <returns>an <see cref="IDisposable"/> object. Check result will be reported when it is disposed.</returns>
        public static IDisposable StartBatch(string message = null)
        {
            return new BatchOfChecks(message);
        }

        /// <summary>
        /// Declare a macro
        /// </summary>
        /// <typeparam name="T">Type to use this macro with.</typeparam>
        /// <typeparam name="T1">Type of first param</typeparam>
        /// <typeparam name="T2">Type of second param.</typeparam>
        /// <param name="function">Function that implements the desired checks.</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>An <see cref="MacroCheck{T, T1, T2}"/> instance.</returns>
        public static MacroCheck<T, T1, T2> DeclareMacro<T, T1, T2>(Action<T, T1, T2> function, string errorMessage) => new MacroCheck<T, T1, T2>(function, errorMessage);
        
        /// <summary>
        /// Declare a macro
        /// </summary>
        /// <typeparam name="T">Type to use this macro with.</typeparam>
        /// <typeparam name="T1">Type of first param</typeparam>
        /// <param name="function">Function that implements the desired checks.</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>An <see cref="MacroCheck{T, T1}"/> instance.</returns>
        public static MacroCheck<T, T1> DeclareMacro<T, T1>(Action<T, T1> function, string errorMessage) => new MacroCheck<T, T1>(function, errorMessage);

        /// <summary>
        /// Declare a macro
        /// </summary>
        /// <typeparam name="T">Type to use this macro with.</typeparam>
        /// <param name="function">Function that implements the desired checks.</param>
        /// <param name="errorMessage">Error message</param>
        /// <returns>An <see cref="MacroCheck{T}"/> instance.</returns>
        public static MacroCheck<T> DeclareMacro<T>(Action<T> function, string errorMessage) => new MacroCheck<T>(function, errorMessage);
    }
}