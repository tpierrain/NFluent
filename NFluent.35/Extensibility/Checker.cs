// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="Checker.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
namespace NFluent.Extensibility
{
    using System;

    /// <summary>
    /// Provides a mean to execute some checks on a value, taking care of whether it should be negated or not, etc.
    /// This interface is designed for developers that need to add new check (extension) methods.
    /// Thus, it should not be exposed via Intellisense to developers that are using NFluent to write 
    /// checks statements.
    /// </summary>
    /// <typeparam name="T">Type of the value to assert on.</typeparam>
    /// <typeparam name="TC">Check interface.</typeparam>
    internal class Checker<T, TC> : IChecker<T, TC> where TC : class, IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
    {
        #region fields

        private readonly ICheckForExtensibility<T, TC> fluentCheckForExtensibility;

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Checker{T,TC}" /> class.
        /// </summary>
        /// <param name="fluentCheckForExtensibility">The runnable fluent check.</param>
        public Checker(ICheckForExtensibility<T, TC> fluentCheckForExtensibility)
        {
            this.fluentCheckForExtensibility = fluentCheckForExtensibility;
        }

        #endregion

        #region methods

        /// <summary>
        /// Gets the check link to return for the next check to be executed (linked with the And operator).
        /// </summary>
        /// <value>
        /// The check link to return for next check (linked with the And operator) to be executed.
        /// </value>
        public ICheckLink<TC> ReturnValueForLinkage
        {
            get
            {
                return new CheckLink<TC>(this.fluentCheckForExtensibility);
            }
        }

        /// <summary>
        /// Gets the value to be tested (provided for any extension method to be able to test it).
        /// </summary>
        /// <value>
        /// The value to be tested by any fluent check extension method.
        /// </value>
        public T Value
        {
            get
            {
                return this.fluentCheckForExtensibility.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="FluentCheck{T}" /> should be negated or not.
        /// This property is useful when you implement check methods.
        /// </summary>
        /// <value>
        /// <c>true</c> if all the methods applying to this check instance should be negated; <c>false</c> otherwise.
        /// </value>
        public bool Negated
        {
            get
            {
                return this.fluentCheckForExtensibility.Negated;
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Executes the check provided as an happy-path lambda (vs lambda for negated version).
        /// </summary>
        /// <param name="action">
        ///     The happy-path action (vs. the one for negated version which has not to be specified). 
        ///     This lambda should simply return if everything is ok, or throws a 
        ///     <see cref="FluentCheckException"/> otherwise.</param>
        /// <param name="negatedExceptionMessage">
        ///     The message for the exception to be thrown when the check fails, in the case we were running the negated version.
        /// </param>
        /// <returns>
        ///     A new check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The check fails.</exception>
        public ICheckLink<TC> ExecuteCheck(Action action, string negatedExceptionMessage)
        {
            try
            {
                // execute test
                action();
            }
            catch (FluentCheckException)
            {
                // exception raised, and this was not expected
                if (!this.fluentCheckForExtensibility.Negated)
                {
                    throw;
                }

                // exception was expected
                return new CheckLink<TC>(this.fluentCheckForExtensibility);
            }

            if (this.fluentCheckForExtensibility.Negated)
            {
                // the expected exception did not occur
                throw new FluentCheckException(negatedExceptionMessage);
            }

            return this.ReturnValueForLinkage;
        }

        #endregion
    }
}