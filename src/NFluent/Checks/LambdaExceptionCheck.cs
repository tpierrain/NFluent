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
    using Extensibility;
    using Extensions;
    using Helpers;
    using Kernel;
#if NETSTANDARD1_3
    using System.Reflection;
#endif
    /// <summary>
    /// Implements specific Value check after lambda checks.
    /// </summary>
    /// <typeparam name="T">Code checker type./>.
    /// </typeparam>
    public class LambdaExceptionCheck<T> : ILambdaExceptionCheck<T>, IForkableCheck
    {
        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaExceptionCheck{T}"/> class.
        /// This check can only be fluently called after a lambda check.
        /// </summary>
        /// <param name="value">The Value.</param>
        public LambdaExceptionCheck(Exception value)
        {
            this.Value = value;
        }

        #endregion

        #region fields

        /// <summary>
        /// Gets or sets with the parent class that fluently called this one.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        internal Exception Value { get; set; }

        #endregion

        /// <summary>
        /// Checks that the message of the considered Value is correctly written.
        /// </summary>
        /// <param name="exceptionMessage">The expected Value message.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <Value cref="FluentCheckException">The code did not raised an Value of any type.</Value>
        public ICheckLink<ILambdaExceptionCheck<T>> WithMessage(string exceptionMessage)
        {
            if (this.Value.Message != exceptionMessage)
            {
                var message = FluentMessage.BuildMessage("The message of the checked exception is not as expected.")
                                            .For("exception message")
                                            .Expected(exceptionMessage)
                                            .And.On(this.Value.Message)
                                            .ToString();
                        
                throw new FluentCheckException(message);
            }

            return new CheckLink<ILambdaExceptionCheck<T>>(this);
        }

        /// <summary>
        /// Checks that a specific property of the considered Value has an expected value.
        /// </summary>
        /// <param name="propertyName">The name of the property to check on the considered Value.</param>
        /// <param name="propertyValue">The expected value for the property to check on the considered Value.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <Value cref="FluentCheckException">The code did not raised an Value of any type.</Value>
        public ICheckLink<ILambdaExceptionCheck<T>> WithProperty(string propertyName, object propertyValue)
        {
            var type = this.Value.GetType();
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                var message = FluentMessage.BuildMessage(string.Format("There is no property [{0}] on exception type [{1}].", propertyName, type.Name)).ToString();
                throw new FluentCheckException(message);
            }
            
            var value = property.GetValue(this.Value, null);
            if (!value.Equals(propertyValue))
            {
                var message = FluentMessage
                    .BuildMessage(string.Format("The property [{0}] of the {{0}} does not have the expected value.", propertyName.DoubleCurlyBraces()))
                    .For("exception's property")
                    .On(value)
                    .And.WithGivenValue(propertyValue).ToString();

                throw new FluentCheckException(message);
            }

            return new CheckLink<ILambdaExceptionCheck<T>>(this);
        }

        /// <summary>
        /// Checks that an inner exception is present within the outer exception stack trace.
        /// </summary>
        /// <typeparam name="TE">Exception type.
        /// </typeparam>
        /// <returns>
        /// A check link.
        /// </returns>
        public ICheckLink<ILambdaExceptionCheck<T>> DueTo<TE>() where TE : Exception
        {
            var innerException = this.Value.InnerException;
            var dueToExceptionFound = false;
            while (innerException != null)
            {
                if (innerException.GetType() == typeof(TE))
                {
                    dueToExceptionFound = true;
                    break;
                }
                innerException = innerException.InnerException;
            }

            if (!dueToExceptionFound)
            {
                var message = FluentMessage.BuildMessage("The {0} did not contain an expected inner exception whereas it must.")
                                            .For("exception")
                                            .On(ExceptionHelper.DumpInnerExceptionStackTrace(this.Value))
                                            .Label("The inner exception(s):")
                                            .And
                                            .Expected(typeof(TE)).Label("The expected inner exception:")
                                            .ToString();

                throw new FluentCheckException(message);
            }

            return new CheckLink<ILambdaExceptionCheck<T>>(this);
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
            return new LambdaExceptionCheck<T>(this.Value);
        }
    }
}