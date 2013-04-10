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

    /// <summary>
    /// Implements lambda/action specific assertion.
    /// </summary>
    public class LambdaAssertion : IFluentAssertion<Action>
    {
        private Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaAssertion"/> class. 
        /// </summary>
        /// <param name="action">
        /// Action to be assessed.
        /// </param>
        public LambdaAssertion(Action action)
        {
            this.Value = action;
        }

        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent assertion extension method.
        /// </value>
        public Action Value { get; private set; }

        /// <summary>
        /// Gets an IFluentAssertion instance to test the duration of the lambda.
        /// </summary>
        /// <value>
        /// The duration of the lambda.
        /// </value>
        public IFluentAssertion<TimeSpan> Duration
        {
            get
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
                TimeSpan duration = TimeSpan.FromMilliseconds(watch.ElapsedTicks * 1000 / Stopwatch.Frequency);
// ReSharper restore PossibleLossOfFraction
                return new FluentAssertion<TimeSpan>(duration);
            }
        }
    }
}