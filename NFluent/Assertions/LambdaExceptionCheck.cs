// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="LambdaExceptionCheck.cs" company="">
// //   Copyright 2013 Rui CARVALHO, Thomas PIERRAIN
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

    using NFluent.Helpers;

    /// <summary>
    /// Implements specific exception check after lambda checks.
    /// </summary>
    public class LambdaExceptionCheck : ILambdaExceptionCheck<ILambdaCheck>, IForkableCheck
    {
        /// <summary>
        /// Set with the parent class that fluently called this one.
        /// </summary>
        private readonly ILambdaCheck parent;
        private readonly Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaExceptionCheck"/> class.
        /// This check can only be fluently called after a lambda check.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="exception">The exception.</param>
        public LambdaExceptionCheck(ILambdaCheck parent, Exception exception)
        {
            this.parent = parent;
            this.exception = exception;
        }

        /// <summary>
        /// Checks that the message of the considered exception is correctly written.
        /// </summary>
        /// <param name="exceptionMessage">The expected exception message.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The code did not raised an exception of any type.</exception>
        public ICheckLink<ILambdaExceptionCheck<ILambdaCheck>> WithMessage(string exceptionMessage)
        {
            if (this.exception.Message != exceptionMessage)
            {
                var message = FluentMessage.BuildMessage("The message of the checked exception is not as expected.")
                                            .For("exception message")
                                            .ExpectedValues(this.exception.Message)
                                            .And.WithGivenValue(exceptionMessage)
                                            .ToString();
                        
                throw new FluentCheckException(message);
            }

            return new CheckLink<ILambdaExceptionCheck<ILambdaCheck>>(this);
        }

        /// <summary>
        /// Checks that a specific property of the considered exception has an expected value.
        /// </summary>
        /// <param name="propertyName">The name of the property to check on the considered exception.</param>
        /// <param name="propertyValue">The expected value for the property to check on the considered exception.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The code did not raised an exception of any type.</exception>
        public ICheckLink<ILambdaExceptionCheck<ILambdaCheck>> WithProperty(string propertyName, object propertyValue)
        {
            var type = this.exception.GetType();
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                var message = FluentMessage.BuildMessage(string.Format("There is no property [{0}] on exception type [{1}].", propertyName, type.Name)).ToString();
                throw new FluentCheckException(message);
            }
            
            var value = property.GetValue(this.exception, null);
            if (!value.Equals(propertyValue))
            {
                var message = FluentMessage
                    .BuildMessage(string.Format("The property [{0}] of the {{0}} do not have the expected value.", propertyName))
                    .For("exception")
                    .Expected(propertyValue)
                    .And.WithGivenValue(value).ToString();

                throw new FluentCheckException(message);
            }

            return new CheckLink<ILambdaExceptionCheck<ILambdaCheck>>(this);
        }

        /// <summary>
        /// Creates a new instance of the same fluent check type, injecting the necessary properties
        /// (i.e. the system under test), but with a false Negated property in any case.
        /// </summary>
        /// <returns>
        /// A new instance of the same fluent check type, with the same properties (including the Value one).
        /// </returns>
        /// <remarks>
        /// This method is used during the chaining of multiple checks.
        /// </remarks>
        public object ForkInstance()
        {
            return new LambdaExceptionCheck(this.parent, this.exception);
        }
    }
}