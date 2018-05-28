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
    using System.Collections.Generic;
#if !DOTNET_30 && !DOTNET_20
    using System.Linq.Expressions;
#endif
    using Extensibility;
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
    public class LambdaExceptionCheck<T> : FluentSut<T>, ILambdaExceptionCheck<T>, IForkableCheck
        where T : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaExceptionCheck{T}"/> class.
        /// This check can only be fluently called after a lambda check.
        /// </summary>
        /// <param name="value">The Value.</param>
        public LambdaExceptionCheck(T value) : base(value, Check.Reporter)
        {
        }

        /// <inheritdoc />
        public ILambdaExceptionCheck<TE> DueTo<TE>()
            where TE : Exception
        {
            Exception resultException = null;
            ExtensibilityHelper.BeginCheck(this)
                .InvalidIf(sut => sut == null, "No exception. Can't be used when negated.")
                .CheckSutAttributes(sut => sut.InnerException, "inner exception")
                .FailIfNull("There is no inner exception.")
                .Analyze((sut, test) =>
                {
                    resultException = sut;
                    while (resultException != null)
                    {
                        if (resultException.GetType() == typeof(TE))
                        {
                            break;
                        }

                        resultException = resultException.InnerException;
                    }
                    test.FailWhen(_ => resultException == null,
                        "The {0} is not of the expected type.");
                })
                .DefineExpectedType(typeof(TE), "", "")
                .EndCheck();
            return new LambdaExceptionCheck<TE>((TE)resultException);
        }

        /// <inheritdoc />
        public object ForkInstance()
        {
            return new LambdaExceptionCheck<T>(this.Value);
        }

        /// <inheritdoc />
        public new T Value => base.Value;
    }

    /// <summary>
    /// Hosts Exception related checks
    /// </summary>
    public static class ExceptionChecks
    {
        /// <summary>
        /// Checks if the exception has a specific message.
        /// </summary>
        /// <param name="checker">Syntax helper</param>
        /// <param name="exceptionMessage">Exptected message</param>
        /// <typeparam name="T">Exception type</typeparam>
        /// <returns>A chainable check.</returns>
        /// <exception cref="FluentCheckException"></exception>
        public static ICheckLink<ILambdaExceptionCheck<T>> WithMessage<T>(this ILambdaExceptionCheck<T> checker, string exceptionMessage) where T : Exception
        {
            ExtensibilityHelper.BeginCheck(checker as FluentSut<T>)
                .InvalidIf(sut=>sut == null, "No exception. Can't be used when negated.")
                .SetSutName("exception")
                .CheckSutAttributes(sut =>  sut.Message, "message")
                .FailWhen(sut => sut != exceptionMessage, "The {0} is not as expected.")
                .DefineExpectedValue(exceptionMessage)
                .EndCheck();

            return new CheckLink<ILambdaExceptionCheck<T>>(checker);
        }

        /// <summary>
        /// Checks if the exception has a specific property having a specific value.
        /// </summary>
        /// <typeparam name="T">Exception type</typeparam>
        /// <typeparam name="TP">Property type</typeparam>
        /// <param name="checker">Syntax helper</param>
        /// <param name="propertyName">Name of property</param>
        /// <param name="propertyValue">Expected valued of property</param>
        /// <returns>A chainable check.</returns>
        public static ICheckLink<ILambdaExceptionCheck<T>> WithProperty<T,TP>(this ILambdaExceptionCheck<T> checker, string propertyName, TP propertyValue) where T : Exception
        {
            var found = false;
            ExtensibilityHelper.BeginCheck(checker as FluentSut<T>)
                .InvalidIf(sut => sut == null, "No exception. Can't be used when negated.")
                .SetSutName("exception")
                .CheckSutAttributes(sut =>
                {
                    var type = sut.GetType();
                    var property = type.GetProperty(propertyName);
                    if (property == null)
                    {
                        return null;
                    }

                    found = true;
                    return property.GetValue(sut, null);
                }, $"property [{propertyName}]")
                .FailWhen(_=> !found, $"There is no property [{propertyName}] on exception type [{typeof(T).Name}].", MessageOption.NoCheckedBlock)
                .FailWhen(sut => !EqualityHelper.FluentEquals(sut, propertyValue),
                    "The {0} does not have the expected value.")
                .DefineExpectedValue(propertyValue)
                .EndCheck();
 
            return new CheckLink<ILambdaExceptionCheck<T>>(checker);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="types"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ICheckLink<ILambdaExceptionCheck<T>> DueToAnyFrom<T>(this ILambdaExceptionCheck<T> context,
            params Type[] types) where T: Exception
        {
            Exception resultException = null;
            var listOfTypes = new List<Type>(types);
            ExtensibilityHelper.BeginCheck(context as FluentSut<T>)
                .InvalidIf(sut => sut == null, "No exception. Can't be used when negated.")
                .CheckSutAttributes(sut => sut.InnerException, "inner exception")
                .FailIfNull("There is no inner exception.")
                .Analyze((sut, test) =>
                {
                    resultException = sut;
                    while (resultException != null)
                    {
                        if ( listOfTypes.Contains(resultException.GetType()))
                        {
                            break;
                        }

                        resultException = resultException.InnerException;
                    }
                    test.FailWhen(_ => resultException == null,
                        "The {0} is not of one of the expected types.", MessageOption.WithType);
                })
                .DefineExpectedValues(types, types.Length, "an instance of any", "an instance of anything but")
                .EndCheck();            return new CheckLink<ILambdaExceptionCheck<T>>(context);
        }

#if !DOTNET_30 && !DOTNET_20
        /// <summary>
        /// Checks if the exception has a specific property having a specific value.
        /// </summary>
        /// <typeparam name="T">Exception type</typeparam>
        /// <typeparam name="TP">Property type</typeparam>
        /// <param name="checker">Syntax helper</param>
        /// <param name="propertyExpression">Extracting expression</param>
        /// <param name="propertyValue">Expected valued of property</param>
        /// <returns>A chainable check.</returns>
        public static ICheckLink<ILambdaExceptionCheck<T>> WithProperty<T, TP>(this ILambdaExceptionCheck<T> checker, Expression<Func<T, TP>> propertyExpression, TP propertyValue)
            where T:Exception
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            var propertyName = memberExpression?.Member.Name ?? propertyExpression.ToString();
            ExtensibilityHelper.BeginCheck(checker as FluentSut<T>)
                .InvalidIf(sut => sut == null, "No exception. Can't be used when negated.")
                .SetSutName("exception")
                .CheckSutAttributes(sut => propertyExpression.Compile().Invoke(sut), $"property [{propertyName}]")
                .FailWhen(sut => !EqualityHelper.FluentEquals(sut, propertyValue),
                    "The {0} does not have the expected value.")
                .DefineExpectedValue(propertyValue)
                .EndCheck();
            
            return new CheckLink<ILambdaExceptionCheck<T>>(checker);
        }
#endif
    }
}