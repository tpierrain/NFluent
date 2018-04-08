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

            ExtensibilityHelper.BeginCheck(check).FailsIf((sut) =>
                new Duration(sut.ExecutionTime, timeUnit) > durationThreshold, "The checked code took too much time to execute.").
                Expecting(durationThreshold, "less than", "more than").
                SutNameIs("execution time").
                Negates("The checked code took too little time to execute.").
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
                GetSutProperty(sut =>  new Duration(sut.TotalProcessorTime, timeUnit), "").
                FailsIf((sut) =>
                    sut > durationThreshold, "The checked code consumed too much CPU time.").
                Expecting(durationThreshold, "less than", "more than").
                SutNameIs("cpu time").
                Negates("The checked code took too little cpu time to execute.").
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
                GetSutProperty((sut) => sut.RaisedException, "raised").
                SutNameIs("code").
                FailsIf((sut)=> sut != null, "The {0} raised an exception, whereas it must not.").
                Negates("The {0} did not raise an exception, whereas it must.").
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
            CheckExceptionType(check, typeof(T));

            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
            return checker.Negated
                ? (ILambdaExceptionCheck<T>) new NegatedLambdaExceptionCheck<T>()
                : new LambdaExceptionCheck<T>((T) checker.Value.RaisedException);
        }

        private static void CheckExceptionType(ICodeCheck<RunTrace> check, Type expecting)
        {
            ExtensibilityHelper.BeginCheck(check).SutNameIs("code")
               .GetSutProperty((sut) => sut.RaisedException, "raised exception")
                .ExpectingType(expecting, expectedLabel: "", negatedLabel: "should not be")
                .FailsIfNull("The checked code did not raise an exception, whereas it must.")
                .FailsIf((sut) => !expecting.IsInstanceOfType(sut),
                    "The {0} is of a different type than expected.")
                .Negates("The {0} raised an exception of the forbidden type.")
                .EndCheck();
        }

        /// <summary>
        /// Checks that the code did throw an exception of a specified type.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="exceptionType">Expected exception type.</param>
        /// <returns>A check link.</returns>
        public static ILambdaExceptionCheck<Exception> ThrowsType(this ICodeCheck<RunTrace> check, Type exceptionType)
        {
            CheckExceptionType(check, exceptionType);

            var checker = ExtensibilityHelper.ExtractCodeChecker(check);
            return checker.Negated
                ? (ILambdaExceptionCheck<Exception>) new NegatedLambdaExceptionCheck<Exception>()
                : new LambdaExceptionCheck<Exception>(checker.Value.RaisedException);
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
                .Negates("The checked code raised an exception, whereas it must not.")
                .SutNameIs("code")
                .GetSutProperty((sut) => sut.RaisedException, "raised exception")
                .FailsIfNull("The checked code did not raise an exception, whereas it must.")
                .EndCheck();
            var checker = ExtensibilityHelper.ExtractCodeChecker(check);

            if (checker.Negated)
            {
                return new NegatedLambdaExceptionCheck<Exception>();
            }
            else
            {
                return new LambdaExceptionCheck<Exception>(checker.Value.RaisedException);
            }
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

    }
}