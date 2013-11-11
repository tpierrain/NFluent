﻿// // --------------------------------------------------------------------------------------------------------------------
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
    using NFluent.Helpers;

    /// <summary>
    /// Implements lambda/action specific check.
    /// </summary>
    public class LambdaCheck : ILambdaCheck, IForkableCheck
    {
        #region Fields
        private double durationInNs;

        private Exception exception;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaCheck"/> class.
        /// </summary>
        /// <param name="action">
        /// Action to be assessed.
        /// </param>
        public LambdaCheck(Delegate action)
            : this(action, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaCheck" /> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="alreadyExecuted">A value indicating whether the action has already been executed or not.</param>
        private LambdaCheck(Delegate action, bool alreadyExecuted)
        {
            this.Value = action;

            if (!alreadyExecuted)
            {
                this.Execute();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent check extension method.
        /// </value>
        private Delegate Value { get; set; }

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
            const bool AlreadyExecuted = true;
            var newInstance = new LambdaCheck(this.Value, AlreadyExecuted)
            {
                durationInNs = this.durationInNs,
                exception = this.exception
            };

            return newInstance;
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
            if (this.exception != null)
            {
                var message1 =
                    FluentMessage.BuildMessage("The {0} raised an exception, whereas it must not.")
                                   .For("code")
                                   .On(this.exception).Label("The raised exception:").ToString();
                throw new FluentCheckException(message1);
            }

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
            var comparand = new Duration(this.durationInNs, TimeUnit.Nanoseconds).ConvertTo(timeUnit);
            var durationThreshold = new Duration(threshold, timeUnit);
            if (comparand > durationThreshold)
            {
                var message =
                    FluentMessage.BuildMessage("The checked code took too much time to execute.")
                                   .For("excecution time")
                                   .On(comparand)
                                   .And.Expected(durationThreshold)
                                   .Comparison("less than")
                                   .ToString();

                throw new FluentCheckException(message);
            }

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
            if (this.exception == null)
            {
                var message =
                    FluentMessage.BuildMessage(
                        "The {0} did not raise an exception, whereas it must.").For("code").Expected(typeof(T)).Label("Expected exception type is:").ToString();
                throw new FluentCheckException(message);
            }

            if (!(this.exception is T))
            {
                var message =
                    FluentMessage.BuildMessage("The {0} raised an exception of a different type than expected.")
                                   .For("code").On(this.exception).Label("Raised Exception").And.Expected(typeof(T)).Label("Expected exception type is:")
                                   .ToString();

                throw new FluentCheckException(message);
            }

            return new LambdaExceptionCheck(this, this.exception);
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
            if (this.exception == null)
            {
                var message =
                    FluentMessage.BuildMessage("The {0} did not raise an exception, whereas it must.").For("code").ToString();
                throw new FluentCheckException(message);
            }

            return new LambdaExceptionCheck(this, this.exception);
        }
        #endregion

        #region Methods

        private void Execute()
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                this.Value.DynamicInvoke();
            }
            catch (Exception e)
            {
                this.exception = e.InnerException;
            }
            finally
            {
                watch.Stop();
            }

            // ReSharper disable PossibleLossOfFraction
            this.durationInNs = watch.ElapsedTicks * 1000000000 / Stopwatch.Frequency;
        }

        #endregion
    }
}