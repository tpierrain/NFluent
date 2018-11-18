 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="CodeCheckExtensions.cs" company="">
 //   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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

// ReSharper disable once CheckNamespace
namespace NFluent
{
    using System;
#if NETSTANDARD1_3
    using System.Reflection;
#endif
    using Extensibility;

    using Helpers;

    /// <summary>
    /// Static class hosting extension methods in relation with checks for code.
    /// </summary>
    public static class CodeCheckExtensions
    {
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
            var durationThreshold = new Duration(threshold, timeUnit);

            ExtensibilityHelper.BeginCheck(check).FailWhen((sut) =>
                new Duration(sut.ExecutionTime, timeUnit) > durationThreshold, "The checked code took too much time to execute.").
                DefineExpectedValue(durationThreshold, "less than", "more than").
                SetSutName("execution time").
                OnNegate("The checked code took too little time to execute.").
                EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
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
            var durationThreshold = new Duration(threshold, timeUnit);

            ExtensibilityHelper.BeginCheck(check).
                CheckSutAttributes(sut =>  new Duration(sut.TotalProcessorTime, timeUnit), "").
                FailWhen((sut) =>
                    sut > durationThreshold, "The checked code consumed too much CPU time.").
                DefineExpectedValue(durationThreshold, "less than", "more than").
                SetSutName("cpu time").
                OnNegate("The checked code took too little cpu time to execute.").
                EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
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
            ExtensibilityHelper.BeginCheck(check).
                SetSutName("code").
                CheckSutAttributes(sut => sut.RaisedException, "raised exception").
                FailWhen(sut=> sut != null, "The checked code raised an exception, whereas it must not.").
                OnNegate("The checked code did not raise an exception, whereas it must.", MessageOption.NoCheckedBlock).
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
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
            var exc = CheckExceptionType(check, typeof(T));

            return new LambdaExceptionCheck<T>((T) exc);
        }

        private static Exception CheckExceptionType(ICodeCheck<RunTrace> check, Type expecting)
        {
            Exception result = null;
            ExtensibilityHelper.BeginCheck(check).SetSutName("code")
               .CheckSutAttributes(sut =>
                {
                    result = sut.RaisedException;
                    return result;
                }, "raised exception")
                .DefineExpectedType(expecting)
                .FailIfNull("The checked code did not raise an exception, whereas it must.")
                .FailWhen(sut => !expecting.IsInstanceOfType(sut),
                    "The {0} is of a different type than expected.")
                .OnNegate("The {0} raised an exception of the forbidden type.")
                .EndCheck();
            if (!expecting.IsInstanceOfType(result))
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Checks that the code did throw an exception of a specified type.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="exceptionType">Expected exception type.</param>
        /// <returns>A check link.</returns>
        public static ILambdaExceptionCheck<Exception> ThrowsType(this ICodeCheck<RunTrace> check, Type exceptionType)
        {
            var exc = CheckExceptionType(check, exceptionType);

            return new LambdaExceptionCheck<Exception>(exc);
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
            ExtensibilityHelper.BeginCheck(check)
                .OnNegate("The {checked} raised an exception, whereas it must not.")
                .SetSutName("code")
                .CheckSutAttributes((sut) => sut.RaisedException, "raised exception")
                .FailIfNull("The {checked} did not raise an exception, whereas it must.")
                .EndCheck();
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);

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
            return ExtensibilityHelper.BuildCheck(check, sut => sut.Result);
        }

    }
}