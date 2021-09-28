// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="RunTrace.cs" company="">
// //   Copyright 2014 Cyrille DUPUYDAUBY
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
    using System.Diagnostics;
#if !DOTNET_35
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
#endif

    /// <summary>
    /// This class stores trace information for a code evaluation.
    /// </summary>
    public class RunTrace
    {
        /// <summary>
        /// Gets or sets the execution time of the checked code.
        /// </summary>
        /// <value>
        /// The execution time.
        /// </value>
        public TimeSpan ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the raised exception.
        /// </summary>
        /// <value>
        /// The raised exception.
        /// </value>
        public Exception RaisedException { get; set; }

        /// <summary>
        /// Gets or sets the total processor time.
        /// </summary>
        /// <value>
        /// The total processor time.
        /// </value>
        public TimeSpan TotalProcessorTime { get; set; }

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

            CaptureTrace(() =>
            {
                result.Result = function();
#if !DOTNET_35
                if (!(result.Result is Task ta))
                {
                    return;
                }

                // we must check if the method is flagged async
                if (!FunctionIsAsync(function))
                {
                    return;
                }

                try
                {
                    ta.Wait();
                }
                catch (AggregateException exception)
                {
                    result.RaisedException = exception.InnerException;
                }
#endif
            }, result);
            return result;
        }

#if !DOTNET_35
        private static bool FunctionIsAsync<TU>(Func<TU> function)
        {
#if NETSTANDARD1_3
            return function.GetMethodInfo().GetCustomAttributes(typeof(AsyncStateMachineAttribute), false).Any();
#else
            return Attribute.GetCustomAttributes(function.GetMethodInfo(), typeof(AsyncStateMachineAttribute)).Any();
#endif
        }
#endif

        private static void CaptureTrace(Action action, RunTrace result)
        {
            var watch = new Stopwatch();
            var cpu = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Start();
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
                // ReSharper disable PossibleLossOfFraction
                result.ExecutionTime = TimeSpan.FromTicks(watch.ElapsedTicks);
                result.TotalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime - cpu;
            }
        }

#if !DOTNET_35
        internal static RunTrace GetAsyncTrace(Func<Task> awaitableMethod)
        {
            var result = new RunTrace();
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
            return result;
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