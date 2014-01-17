// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="LambdaCheck.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
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

    using NFluent.Extensibility;
    using NFluent.Helpers;

    /// <summary>
    /// Implements lambda/action specific check.
    /// </summary>
    public class LambdaCheck : ILambdaCheck, IForkableCheck
    {
        #region Fields

        private readonly RunTrace runTrace;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaCheck" /> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public LambdaCheck(Delegate action)
        {
            this.runTrace = CodeCheckExtensions.GetTrace(() => action.DynamicInvoke());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaCheck"/> class.
        /// </summary>
        /// <param name="runTrace">Trace of the initial run.
        /// </param>
        private LambdaCheck(RunTrace runTrace)
        {
            this.runTrace = runTrace;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates a new instance of the same fluent check type, injecting the same Value property
        /// (i.e. the system under test), but with a false Negated property in any case.
        /// </summary>
        /// <returns>
        /// A new instance of the same fluent check type, with the same Value property.
        /// </returns>
        /// <remarks>
        /// This method is used during the chaining of multiple checks.
        /// </remarks>
        public object ForkInstance()
        {
            return new LambdaCheck(this.runTrace);
        }

        /// <summary>
        /// Check that the code does not throw an exception.
        /// </summary>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The code raised an exception.
        /// </exception>
        public ICheckLink<ILambdaCheck> DoesNotThrow()
        {
            var check = new FluentCodeCheck<RunTrace>(this.runTrace);
            check.DoesNotThrow();
            return new CheckLink<ILambdaCheck>(this);
        }

        /// <summary>
        /// Checks that the execution time is below a specified threshold.
        /// </summary>
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
        public ICheckLink<ILambdaCheck> LastsLessThan(double threshold, TimeUnit timeUnit)
        {
            var check = new FluentCodeCheck<RunTrace>(this.runTrace);
            check.LastsLessThan(threshold, timeUnit);
            return new CheckLink<ILambdaCheck>(this);
        }

        /// <summary>
        /// Checks that the code did throw an exception of a specified type.
        /// </summary>
        /// <typeparam name="T">
        /// And.Expected exception type.
        /// </typeparam>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The code did not raised an exception of the specified type, or did not raised an exception at all.
        /// </exception>
        public ILambdaExceptionCheck<ILambdaCheck> Throws<T>()
        {
            var check = new FluentCodeCheck<RunTrace>(this.runTrace);
            check.Throws<T>();
            return new LambdaExceptionCheck<ILambdaCheck>(this.runTrace.RaisedException);
        }

        /// <summary>
        /// Checks that the code did throw an exception of any type.
        /// </summary>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The code did not raised an exception of any type.
        /// </exception>
        public ILambdaExceptionCheck<ILambdaCheck> ThrowsAny()
        {
            var check = new FluentCodeCheck<RunTrace>(this.runTrace);
            check.ThrowsAny();
            return new LambdaExceptionCheck<ILambdaCheck>(this.runTrace.RaisedException);
        }
        #endregion
    }
}