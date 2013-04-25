﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="LambdaAssertion.cs" company="">
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
    /// Implements lambda/action specific assertion.
    /// </summary>
    public class LambdaAssertion : ILambdaAssertion
    {
        #region Fields
        private const string SutName = "The checked code";

        private double durationInNs;

        private Exception exception;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaAssertion"/> class.
        /// </summary>
        /// <param name="action">
        /// Action to be assessed.
        /// </param>
        public LambdaAssertion(Action action)
        {
            this.Value = action;
            this.Execute();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent assertion extension method.
        /// </value>
        public Action Value { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Check that the code does not throw an exception.
        /// </summary>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The code raised an exception.
        /// </exception>
        public IChainableFluentAssertion<ILambdaAssertion> DoesNotThrow()
        {
            if (this.exception != null)
            {
                throw new FluentAssertionException(
                    string.Format("{0} raised an exception, whereas it must not.\n{0} raised the exception:\n----\n{1}\n----", SutName, ExceptionReport(this.exception)));
            }

            return new ChainableFluentAssertion<ILambdaAssertion>(this);
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
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// Execution was strictly above limit.
        /// </exception>
        public IChainableFluentAssertion<ILambdaAssertion> LastsLessThan(double threshold, TimeUnit timeUnit)
        {
            double comparand = TimeHelper.GetInNanoSeconds(threshold, timeUnit);
            if (this.durationInNs > comparand)
            {
                throw new FluentAssertionException(
                    string.Format(
                        "{0} took too much time to execute.\n{0} execution time:\n\t{1} {3}\nExpected execution time:\n\t{2} {3}",
                        SutName,
                        TimeHelper.GetFromNanoSeconds(this.durationInNs, timeUnit),
                        threshold,
                        timeUnit));
            }

            return new ChainableFluentAssertion<ILambdaAssertion>(this);
        }

        /// <summary>
        /// Checks that the code did throw an exception of a specified type.
        /// </summary>
        /// <typeparam name="T">
        /// Expected exception type.
        /// </typeparam>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The code did not raised an exception of the specified type, or did not raised an exception at all.
        /// </exception>
        public IChainableFluentAssertion<ILambdaAssertion> Throws<T>()
        {
            if (this.exception == null)
            {
                throw new FluentAssertionException(
                    string.Format(
                        "{0} did not raise an exception, whereas it must.\nExpected exception type is:\n\t[{1}]",
                        SutName,
                        typeof(T).Name));
            }

            if (!(this.exception is T))
            {
                throw new FluentAssertionException(
                    string.Format(
                        "{0} raised an exception of a different type than expected.\n{0} raised:\n\t{1}\nExpected exception type:\n\t[{2}].\n", 
                        SutName,
                        ExceptionReport(this.exception),
                        typeof(T).Name));
            }

            return new ChainableFluentAssertion<ILambdaAssertion>(this);
        }

        /// <summary>
        /// Checks that the code did throw an exception of any type.
        /// </summary>
        /// <returns>
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The code did not raised an exception of any type.
        /// </exception>
        public IChainableFluentAssertion<ILambdaAssertion> ThrowsAny()
        {
            if (this.exception == null)
            {
                throw new FluentAssertionException(string.Format("{0} did not raise an exception, whereas it must.", SutName));
            }

            return new ChainableFluentAssertion<ILambdaAssertion>(this);
        }

        #endregion

        #region Methods

        // build an exception description string
        private static string ExceptionReport(Exception e)
        {
            return string.Format("[{0}]: {1}", e.GetType().FullName, e.Message);
        }

        private void Execute()
        {
            var watch = new Stopwatch();
            try
            {
                watch.Start();
                this.Value();
            }
            catch (Exception e)
            {
                this.exception = e;
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