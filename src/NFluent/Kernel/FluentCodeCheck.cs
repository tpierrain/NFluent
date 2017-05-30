// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentCodeCheck.cs" company="">
//   Copyright 2014 Cyrille DUPUYDAUBY
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

namespace NFluent.Kernel
{
    using System;
#if !PORTABLE
    using System.Diagnostics;
#endif
    using System.Diagnostics.CodeAnalysis;
#if !DOTNET_3_5
    using System.Threading.Tasks;
#endif
    using Extensibility;

    /// <summary>
    /// This class stores all required information to check code.
    /// </summary>
    /// <typeparam name="T">Code type.</typeparam>
    internal class FluentCodeCheck<T> : ICodeCheck<T>, IForkableCheck, ICheckForExtensibility<T, ICodeCheck<T>>
        where T : RunTrace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCodeCheck{T}"/> class.
        /// </summary>
        /// <param name="getTrace">The execution trace.</param>
        public FluentCodeCheck(T getTrace)
            : this(getTrace, !CheckContext.DefaulNegated)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentCodeCheck{T}"/> class.
        /// </summary>
        /// <param name="value">The execution trace.
        /// </param>
        /// <param name="negated">True if test must be negated.
        /// </param>
        private FluentCodeCheck(T value, bool negated)
        {
            this.Value = value;
            this.Negated = negated;
            this.Checker = new Checker<T, ICodeCheck<T>>(this);
        }

        /// <summary>
        /// Negates the next check.
        /// </summary>
        /// <value>
        /// The next check negated.
        /// </value>
        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1623:PropertySummaryDocumentationMustMatchAccessors",
            Justification =
                "Reviewed. Suppression is OK here since we want to trick and improve the auto-completion experience here.")]
        public ICodeCheck<T> Not => new FluentCodeCheck<T>(this.Value, CheckContext.DefaulNegated);

        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent check extension method.
        /// </value>
        public T Value { get; private set; }

        /// <summary>
        /// Gets the runnable check to use for checking something on a value of a given type.
        /// </summary>
        /// <value>
        /// The runnable check to use for checking something on a given type.
        /// </value>
        public IChecker<T, ICodeCheck<T>> Checker { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="FluentCheck{T}" /> should be negated or not.
        /// This property is useful when you implement check methods. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if all the methods applying to this check instance should be negated; <c>false</c> otherwise.
        /// </value>
        public bool Negated { get; private set; }

        /// <summary>
        /// Creates a new instance of the same fluent check type, injecting the same Value property 
        /// (i.e. the system under test), but with a false Negated property in any case.
        /// </summary>
        /// <remarks>This method is used during the chaining of multiple checks.</remarks>
        /// <returns>A new instance of the same fluent check type, with the same Value property.</returns>
        public object ForkInstance()
        {
            this.Negated = !CheckContext.DefaulNegated;
            return this;
        }

        /// <summary>
        /// Execute the action to capture the run.
        /// </summary>
        /// <param name="action">
        /// <see cref="Action"/> to be analyzed.
        /// </param>
        /// <returns>
        /// Return <see cref="RunTrace"/> describing the execution.
        /// </returns>
        internal static RunTrace GetTrace(Action action)
        {
            var result = new RunTrace();
            CaptureTrace(action, result);
            return result;
        }

        /// <summary>
        /// Execute the function to capture the run.
        /// </summary>
        /// <typeparam name="TU">Result type of the function.</typeparam>
        /// <param name="function">
        /// <see cref="Action"/> to be analyzed.
        /// </param>
        /// <returns>
        /// Return <see cref="RunTrace"/> describing the execution.
        /// </returns>
        internal static RunTraceResult<TU> GetTrace<TU>(Func<TU> function)
        {
            var result = new RunTraceResult<TU>();
            CaptureTrace(() => result.Result = function(), result);
            return result;
        }

        private static void CaptureTrace(Action action, RunTrace result)
        {
#if !PORTABLE
            var watch = new Stopwatch();
            var cpu = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Start();
#else
            var watchPortable = DateTime.UtcNow;
#endif
            try
            {
                action();
            }
            catch (Exception e)
            {
                result.RaisedException = e;
            }
            finally
            {
#if !PORTABLE
                watch.Stop();
                result.TotalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime - cpu;

                // ReSharper disable PossibleLossOfFraction
                result.ExecutionTime = TimeSpan.FromTicks(watch.ElapsedTicks);
#else
                result.ExecutionTime = DateTime.Now - watchPortable;
                result.TotalProcessorTime = result.ExecutionTime;
#endif
            }
        }

#if !DOTNET_3_5 && !DOTNET_40
        internal static RunTrace GetAsyncTrace(Func<Task> awaitableMethod)
        {
            var result = new RunTrace();
            CaptureAsyncTrace(awaitableMethod, result);
            return result;
        }

        private static void CaptureAsyncTrace(Func<Task> awaitableMethod, RunTrace result)
        {
            CaptureTrace(
                () =>
                    {
                        try
                        {
                            // starts and waits the completion of the awaitable method
                            awaitableMethod().Wait();
                        }
                        catch (AggregateException exception)
                        {
                            result.RaisedException = exception.InnerException;
                        }
                    },
                result);
        }

        /// <summary>
        /// Execute the function to capture the run.
        /// </summary>
        /// <typeparam name="TResult">Result type of the awaitable function.</typeparam>
        /// <param name="waitableFunction">
        /// <see cref="Action"/> to be analyzed.
        /// </param>
        /// <returns>
        /// Return <see cref="RunTrace"/> describing the execution.
        /// </returns>
        internal static RunTraceResult<TResult> GetAsyncTrace<TResult>(Func<Task<TResult>> waitableFunction)
        {
            var result = new RunTraceResult<TResult>();
            CaptureTrace(
                () =>
                    {
                        try
                        {
                            // starts and waits the completion of the awaitable method
                            waitableFunction().Wait();
                            result.Result = waitableFunction().Result;
                        }
                        catch (AggregateException agex)
                        {
                            result.RaisedException = agex.InnerException;
                        }
                    },
                result);
            return result;
        }

#endif
    }
}