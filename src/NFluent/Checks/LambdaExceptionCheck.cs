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
#if !DOTNET_30 && !DOTNET_20
    using System.Linq.Expressions;
#endif
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
        where T : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaExceptionCheck{T}"/> class.
        /// This check can only be fluently called after a lambda check.
        /// </summary>
        /// <param name="value">The Value.</param>
        public LambdaExceptionCheck(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets with the parent class that fluently called this one.
        /// </summary>
        /// <actualValue>
        /// The actualValue.
        /// </actualValue>
        internal T Value { get; set; }

        /// <inheritdoc />
        public ICheckLink<ILambdaExceptionCheck<T>> WithMessage(string exceptionMessage)
        {
            if (this.Value.Message == exceptionMessage)
            {
                return new CheckLink<ILambdaExceptionCheck<T>>(this);
            }

            var message = FluentMessage.BuildMessage("The message of the checked exception is not as expected.")
                .For("exception message").Expected(exceptionMessage).And.On(this.Value.Message).ToString();

            throw new FluentCheckException(message);
        }

        /// <inheritdoc />
        public ICheckLink<ILambdaExceptionCheck<T>> WithProperty<TP>(string propertyName, TP propertyValue)
        {
            var type = this.Value.GetType();
            var property = type.GetProperty(propertyName);
            if (property == null)
            {
                var message = FluentMessage.BuildMessage(
                    $"There is no property [{propertyName}] on exception type [{type.Name}].").ToString();
                throw new FluentCheckException(message);
            }

            var value = property.GetValue(this.Value, null);
            return this.CheckProperty(propertyName, propertyValue, value);
        }
 
#if !DOTNET_30 && !DOTNET_20
        /// <inheritdoc />
        public ICheckLink<ILambdaExceptionCheck<T>> WithProperty<TP>(Expression<Func<T, TP>> propertyExpression, TP propertyValue)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            var name = memberExpression?.Member.Name ?? propertyExpression.ToString();

            return this.CheckProperty(name, propertyValue, propertyExpression.Compile().Invoke(this.Value));
        }
#endif

        private ICheckLink<ILambdaExceptionCheck<T>> CheckProperty<TP>(string propertyName, TP expectedValue, TP actualValue)
        {
            if (actualValue.Equals(expectedValue))
            {
                return new CheckLink<ILambdaExceptionCheck<T>>(this);
            }

            var message = FluentMessage.BuildMessage("The {0} does not have the expected value.").
                For($"exception's property [{propertyName.DoubleCurlyBraces()}]").
                On(actualValue).And.WithGivenValue(expectedValue).ToString();

            throw new FluentCheckException(message);
        }

        /// <inheritdoc />
        public ILambdaExceptionCheck<TE> DueTo<TE>()
            where TE : Exception
        {
            var innerException = this.Value.InnerException;
            while (innerException != null)
            {
                if (innerException.GetType() == typeof(TE))
                {
                    break;
                }

                innerException = innerException.InnerException;
            }

            if (innerException != null)
            {
                return new LambdaExceptionCheck<TE>((TE)innerException);
            }

            var message = FluentMessage
                .BuildMessage("The {0} did not contain an expected inner exception whereas it must.").For("exception")
                .On(ExceptionHelper.DumpInnerExceptionStackTrace(this.Value)).Label("The inner exception(s):").And
                .Expected(typeof(TE)).Label("The expected inner exception:").ToString();

            throw new FluentCheckException(message);
        }

        /// <inheritdoc />
        public object ForkInstance()
        {
            return new LambdaExceptionCheck<T>(this.Value);
        }
    }
}