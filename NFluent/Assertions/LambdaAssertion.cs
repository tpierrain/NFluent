// // --------------------------------------------------------------------------------------------------------------------
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
        public LambdaAssertion(Action action) : this(action, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaAssertion" /> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="alreadyExecuted">A value indicating whether the action has already been executed or not.</param>
        private LambdaAssertion(Action action, bool alreadyExecuted)
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
        /// The value to be tested by any fluent assertion extension method.
        /// </value>
        private Action Value { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates a new instance of the same fluent assertion type, injecting the same Value property
        /// (i.e. the system under test), but with a false Negated property in any case.
        /// </summary>
        /// <returns>
        /// A new instance of the same fluent assertion type, with the same Value property.
        /// </returns>
        /// <remarks>
        /// This method is used during the chaining of multiple assertions.
        /// </remarks>
        public object ForkInstance()
        {
            const bool AlreadyExecuted = true;
            var newInstance = new LambdaAssertion(this.Value, AlreadyExecuted)
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
        /// A chainable assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">
        /// The code raised an exception.
        /// </exception>
        public IChainableFluentAssertion<ILambdaAssertion> DoesNotThrow()
        {
            if (this.exception != null)
            {
                var message1 =
                    FluentMessage.BuildMessage("The {0} raised an exception, whereas it must not.")
                                   .For("code")
                                   .On(this.exception).Label("The raised exception:").ToString();
                throw new FluentAssertionException(message1);
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

                throw new FluentAssertionException(message);
            }

            return new ChainableFluentAssertion<ILambdaAssertion>(this);
        }

        /// <summary>
        /// Checks that the code did throw an exception of a specified type.
        /// </summary>
        /// <typeparam name="T">
        /// And.Expected exception type.
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
                var message =
                    FluentMessage.BuildMessage(
                        "The {0} did not raise an exception, whereas it must.").For("code").Expected(typeof(T)).Label("Expected exception type is:").ToString();
                throw new FluentAssertionException(message);
            }

            if (!(this.exception is T))
            {
                var message =
                    FluentMessage.BuildMessage("The {0} raised an exception of a different type than expected.")
                                   .For("code").On(this.exception).Label("Raised Exception").And.Expected(typeof(T)).Label("Expected exception type is:")
                                   .ToString();

                throw new FluentAssertionException(message);
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
                var message =
                    FluentMessage.BuildMessage("The {0} did not raise an exception, whereas it must.").For("code").ToString();
                throw new FluentAssertionException(message);
            }

            return new ChainableFluentAssertion<ILambdaAssertion>(this);
        }

        #endregion

        #region Methods
 
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