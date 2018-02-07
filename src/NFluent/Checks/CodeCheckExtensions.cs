// // --------------------------------------------------------------------------------------------------------------------
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

namespace NFluent
{
    using System;
#if NETSTANDARD1_3
    using System.Reflection;
#endif
    using Extensibility;

    using Helpers;

    using Kernel;

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
            this ICodeCheck<T> check,
            double threshold,
            TimeUnit timeUnit)
            where T : RunTrace
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
            var comparand = new Duration(checker.Value.ExecutionTime, timeUnit);
            var durationThreshold = new Duration(threshold, timeUnit);

            checker.ExecuteCheck(
                () =>
                    {
                        if (comparand > durationThreshold)
                        {
                            var message = checker.BuildMessage("The checked code took too much time to execute.")
                                .For(LabelForExecTime).On(comparand).And.Expected(durationThreshold)
                                .Comparison(LabelForLessThan).ToString();

                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The checked code took too little time to execute.").For(LabelForExecTime)
                    .Expected(durationThreshold).Comparison(LabelForMoreThan).ToString());

            return checker.BuildChainingObject();
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
            this ICodeCheck<T> check,
            double threshold,
            TimeUnit timeUnit)
            where T : RunTrace
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
            var comparand = new Duration(checker.Value.TotalProcessorTime, timeUnit);
            var durationThreshold = new Duration(threshold, timeUnit);

            checker.ExecuteCheck(
                () =>
                    {
                        if (comparand > durationThreshold)
                        {
                            var message = checker.BuildMessage("The checked code consumed too much CPU time.")
                                .For(LabelForCpuTime).Expected(durationThreshold).Comparison(LabelForLessThan)
                                .ToString();

                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The checked code took too little cpu time to execute.").For(LabelForCpuTime)
                    .Expected(durationThreshold).Comparison(LabelForMoreThan).ToString());

            return checker.BuildChainingObject();
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
        public static ICheckLink<ICodeCheck<T>> DoesNotThrow<T>(this ICodeCheck<T> check)
            where T : RunTrace
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);

            checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.RaisedException != null)
                        {
                            var message = checker.BuildMessage("The {0} raised an exception, whereas it must not.")
                                .For(LabelForCode).On(checker.Value.RaisedException).Label("The raised exception:")
                                .ToString();

                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} did not raise an exception, whereas it must.").For(LabelForCode)
                    .ToString());
            return checker.BuildChainingObject();
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
        public static ILambdaExceptionCheck<T> Throws<T>(this ICodeCheck<RunTrace> check)
            where T : Exception
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
            CheckException(checker, typeof(T));
            return new LambdaExceptionCheck<T>((T)checker.Value.RaisedException);
        }

        /// <summary>
        /// Checks that the code did throw an exception of a specified type.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="exceptionType">Expected exception type.</param>
        /// <returns>A check link.</returns>
        public static ILambdaExceptionCheck<Exception> ThrowsType(this ICodeCheck<RunTrace> check, Type exceptionType)
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
            CheckException(checker, exceptionType);
            return new LambdaExceptionCheck<Exception>(checker.Value.RaisedException);
        }

        private static void CheckException(IChecker<RunTrace, ICodeCheck<RunTrace>> checker, Type exceptionType)
        {
            checker.ExecuteNotChainableCheck(
                () =>
                {
                    string message;
                    if (checker.Value.RaisedException == null)
                    {
                        message = checker
                            .BuildShortMessage("The {0} did not raise an exception, whereas it must.")
                            .For(LabelForCode).ExpectedType(exceptionType).Label("The {0} exception:").ToString();
                        throw new FluentCheckException(message);
                    }

                    if (exceptionType.IsInstanceOfType(checker.Value.RaisedException))
                    {
                        return;
                    }

                    message = checker
                        .BuildShortMessage("The {0} raised an exception of a different type than expected.")
                        .For(LabelForCode).On(checker.Value.RaisedException).Label("Raised Exception").And
                        .ExpectedType(exceptionType).Label("The {0} exception:").ToString();

                    throw new FluentCheckException(message);
                },
                checker.BuildMessage("The {0} raised an exception of the forbidden type.").For(LabelForCode)
                    .On(checker.Value.RaisedException).Label("Raised Exception").ToString());
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
        public static ILambdaExceptionCheck<Exception> ThrowsAny(this ICodeCheck<RunTrace> check)
        {
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);

            checker.ExecuteCheck(
                () =>
                    {
                        if (checker.Value.RaisedException == null)
                        {
                            var message = checker
                                .BuildShortMessage("The {0} did not raise an exception, whereas it must.")
                                .For(LabelForCode).ToString();
                            throw new FluentCheckException(message);
                        }
                    },
                checker.BuildMessage("The {0} raised an exception, whereas it must not.").For(LabelForCode)
                    .On(checker.Value.RaisedException).Label("Raised Exception").ToString());
            return new LambdaExceptionCheck<Exception>(checker.Value.RaisedException);
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