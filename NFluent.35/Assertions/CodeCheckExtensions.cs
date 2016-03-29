﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="CodeCheckExtensions.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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
#if !(PORTABLE)
namespace NFluent
{
    using System;
    using System.Diagnostics;
    using System.Threading;
#if DOTNET_40
    using System.Threading.Tasks;
#endif

    using NFluent.Extensibility;
    using NFluent.Helpers;

    /// <summary>
    /// Static class hosting extension methods in relation with checks for code.
    /// </summary>
    public static class CodeCheckExtensions
    {
        #region fields

        private const string LabelForExecTime = "execution time";

        private const string LabelForCpuTime = "cpu time";

        private const string LabelForLessThan = "less than";

        private const string LabelForMoreThan = "more than";

        private const string LabelForCode = "code";

        #endregion

        #region Methods

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

        private static void CaptureTrace(Action action, RunTrace result)
        {
            var watch = new Stopwatch();
            var cpu = Process.GetCurrentProcess().TotalProcessorTime;
            try
            {
                watch.Start();
                action();
            }
            catch (Exception e)
            {
                result.RaisedException = e;
            }
            finally
            {
                watch.Stop();
                result.TotalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime - cpu;
            }

            // ReSharper disable PossibleLossOfFraction
            result.ExecutionTime = TimeSpan.FromTicks(watch.ElapsedTicks);
        }

#if DOTNET_40
        internal static RunTrace GetAsyncTrace(Func<Task> awaitableMethod)
        {
            var result = new RunTrace();
            CaptureAsyncTrace(awaitableMethod, result);
            return result;
        }

        private static void CaptureAsyncTrace(Func<Task> awaitableMethod, RunTrace result)
        {
            var watch = new Stopwatch();
            var cpu = Process.GetCurrentProcess().TotalProcessorTime;
            try
            {
                watch.Start();

                // starts and waits the completion of the awaitable method
                awaitableMethod().Wait();
            }
            catch (AggregateException agex)
            {
                result.RaisedException = agex.InnerException;
            }
            catch (Exception ex)
            {
                result.RaisedException = ex;
            }
            finally
            {
                watch.Stop();
                result.TotalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime - cpu;
            }

            // AFAIK, ObjectDisposedException should never happen here

            // ReSharper disable PossibleLossOfFraction
            result.ExecutionTime = TimeSpan.FromTicks(watch.ElapsedTicks);
        }

        /// <summary>
        /// Execute the function to capture the run.
        /// </summary>
        /// <typeparam name="TResult">Result type of the awaitable function.</typeparam>
        /// <param name="awaitableFunction">
        /// <see cref="Action"/> to be analyzed.
        /// </param>
        /// <returns>
        /// Return <see cref="RunTrace"/> describing the execution.
        /// </returns>
        internal static RunTraceResult<TResult> GetAsyncTrace<TResult>(Func<Task<TResult>> awaitableFunction)
        {
            var result = new RunTraceResult<TResult>();
            CaptureAsyncTrace(awaitableFunction, result);
            return result;
        }

        private static void CaptureAsyncTrace<TResult>(Func<Task<TResult>> awaitableFunction, RunTraceResult<TResult> result)
        {
            var watch = new Stopwatch();
            var cpu = Process.GetCurrentProcess().TotalProcessorTime;
            try
            {
                watch.Start();

                // starts and waits the completion of the awaitable method
                awaitableFunction().Wait();
                result.Result = awaitableFunction().Result;
            }
            catch (AggregateException agex)
            {
                result.RaisedException = agex.InnerException;
            }
            finally
            {
                watch.Stop();
                result.TotalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime - cpu;
            }

            // AFAIK, ObjectDisposedException should never happen here

            // ReSharper disable PossibleLossOfFraction
            result.ExecutionTime = TimeSpan.FromTicks(watch.ElapsedTicks);
        }
#endif

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

        /// <summary>
        /// Checks that the execution time is below a specified threshold.
        /// </summary>
        /// <typeparam name="T">Type of the checked type.</typeparam>
        /// <param name="check">The fluent check to be extended.
        /// </param>
        /// <param name="threshold">
        /// The threshold.
        /// </param>
        /// <param name="timeUnit">
        /// The time unit of the given threshold.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// Execution was strictly above limit.
        /// </exception>
        public static ICheckLink<ICodeCheck<T>> LastsLessThan<T>(
            this ICodeCheck<T> check, double threshold, TimeUnit timeUnit) where T : RunTrace
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
            var comparand = new Duration(checker.Value.ExecutionTime, timeUnit);
            var durationThreshold = new Duration(threshold, timeUnit);  

            checker.ExecuteCheck(
                () =>
                    {
                        if (comparand > durationThreshold)
                        {
                            var message =
                                checker.BuildMessage(
                                    "The checked code took too much time to execute.")
                                             .For(LabelForExecTime)
                                             .Expected(durationThreshold)
                                             .Comparison(LabelForLessThan)
                                             .ToString();

                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The checked code took too little time to execute.").For(LabelForExecTime).Expected(durationThreshold).Comparison(LabelForMoreThan).ToString());

            return new CheckLink<ICodeCheck<T>>(check);
        }

        /// <summary>
        /// Checks that the CPU time is below a specified threshold.
        /// </summary>
        /// <typeparam name="T">Type of the checked type.</typeparam>
        /// <param name="check">The fluent check to be extended.
        /// </param>
        /// <param name="threshold">
        /// The threshold.
        /// </param>
        /// <param name="timeUnit">
        /// The time unit of the given threshold.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// Execution was strictly above limit.
        /// </exception>
        public static ICheckLink<ICodeCheck<T>> ConsumesLessThan<T>(
            this ICodeCheck<T> check, double threshold, TimeUnit timeUnit) where T : RunTrace
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
            var comparand = new Duration(checker.Value.TotalProcessorTime, timeUnit);
            var durationThreshold = new Duration(threshold, timeUnit);

            checker.ExecuteCheck(
                () =>
                {
                    if (comparand > durationThreshold)
                    {
                        var message =
                            checker.BuildMessage(
                                "The checked code consumed too much CPU time.")
                                         .For(LabelForCpuTime)
                                         .Expected(durationThreshold)
                                         .Comparison(LabelForLessThan)
                                         .ToString();

                        throw new FluentCheckException(message);
                    }
                },
                checker.BuildMessage("The checked code took too little cpu time to execute.").For(LabelForCpuTime).Expected(durationThreshold).Comparison(LabelForMoreThan).ToString());

            return new CheckLink<ICodeCheck<T>>(check);
        }

        /// <summary>
        /// Check that the code does not throw an exception.
        /// </summary>
        /// <param name="check">The fluent check to be extended.
        /// </param>
        /// <typeparam name="T">Inferred type of the code.</typeparam>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The code raised an exception.</exception>
        public static ICheckLink<ICodeCheck<T>> DoesNotThrow<T>(this ICodeCheck<T> check) where T : RunTrace
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
 
            checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.RaisedException != null)
                        {
                            var message =
                                checker.BuildMessage(
                                    "The {0} raised an exception, whereas it must not.")
                                             .For(LabelForCode)
                                             .On(checker.Value.RaisedException)
                                             .Label("The raised exception:")
                                             .ToString();

                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} did not raise an exception, whereas it must.").For(LabelForCode).ToString());
            return new CheckLink<ICodeCheck<T>>(check);
        }

        /// <summary>
        /// Checks that the code did throw an exception of a specified type.
        /// </summary>
        /// <typeparam name="T">Expected exception type.</typeparam>
        /// <param name="check">The fluent check to be extended.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The code did not raised an exception of the specified type, or did not raised an exception at all.</exception>
        public static ILambdaExceptionCheck<RunTrace> Throws<T>(this ICodeCheck<RunTrace> check) where T : Exception
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);

            checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.RaisedException == null)
                        {
                            var message =
                                checker.BuildShortMessage(
                                    "The {0} did not raise an exception, whereas it must.")
                                             .For(LabelForCode)
                                             .ExpectedType(typeof(T)).WithType()
                                             .Label("The {0} exception:")
                                             .ToString();
                            throw new FluentCheckException(message);
                        }

                        if (!(checker.Value.RaisedException is T))
                        {
                            var message =
                                checker.BuildShortMessage(
                                    "The {0} raised an exception of a different type than expected.")
                                             .For(LabelForCode)
                                             .On(checker.Value.RaisedException)
                                             .Label("Raised Exception")
                                             .And.ExpectedType(typeof(T)).WithType()
                                             .Label("The {0} exception:")
                                             .ToString();

                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} raised an exception of the forbidden type.").For(LabelForCode).On(checker.Value.RaisedException).Label("Raised Exception").ToString());
            return new LambdaExceptionCheck<RunTrace>(checker.Value.RaisedException);
        }

        /// <summary>
        /// Checks that the code did throw an exception of a specified type.
        /// </summary>
        /// <param name="check">
        /// The fluent check to be extended.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The code did not raised an exception of the specified type, or did not raised an exception at all.
        /// </exception>
        public static ILambdaExceptionCheck<RunTrace> ThrowsAny(this ICodeCheck<RunTrace> check)
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);

            checker.ExecuteCheck(
                () =>
                {
                    if (checker.Value.RaisedException == null)
                    {
                        var message =
                            checker.BuildShortMessage(
                                "The {0} did not raise an exception, whereas it must.")
                                         .For(LabelForCode)
                                         .ToString();
                        throw new FluentCheckException(message);
                    }
                },
                checker.BuildMessage("The {0} raised an exception, whereas it must not.").For(LabelForCode).On(checker.Value.RaisedException).Label("Raised Exception").ToString());
            return new LambdaExceptionCheck<RunTrace>(checker.Value.RaisedException);
        }

        /// <summary>
        /// Allows to perform checks on the result value.
        /// </summary>
        /// <typeparam name="T">Type of the code result. Should be inferred.</typeparam>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check object for the result.</returns>
        public static ICheck<T> WhichResult<T>(this ICodeCheck<RunTraceResult<T>> check)
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
            return new FluentCheck<T>(checker.Value.Result);
        }
        #endregion
    }
}
#endif
