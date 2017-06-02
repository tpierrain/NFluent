// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ILambdaExceptionCheck.cs" company="">
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

    /// <summary>
    /// Provides check methods to be executed on the exception raised by a given lambda/action.
    /// </summary>
    /// <typeparam name="TException">The type of the exception.</typeparam>
    public interface ILambdaExceptionCheck<TException> : IHasParentCheck<TException>,
        IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        where TException : Exception
    {
        /// <summary>
        /// Checks that the message of the considered exception is correctly written.
        /// </summary>
        /// <param name="exceptionMessage">The expected exception message.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The code did not raised an exception of any type.</exception>
        ICheckLink<ILambdaExceptionCheck<TException>> WithMessage(string exceptionMessage);

        /// <summary>
        /// Checks that a specific property of the considered exception has an expected value.
        /// </summary>
        /// <typeparam name="TP"> Expected type of the property.
        /// </typeparam>
        /// <param name="propertyName">The name of the property to check on the considered exception.</param>
        /// <param name="propertyValue">The expected value for the property to check on the considered exception.</param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">The code did not raised an exception of any type.</exception>
        ICheckLink<ILambdaExceptionCheck<TException>> WithProperty<TP>(string propertyName, TP propertyValue);

#if !DOTNET_30 && !DOTNET_20
        /// <summary>
        /// Checks that a specific property of the considered exception has an expected value.
        /// </summary>
        /// <typeparam name="TP"> Expected type of the property.
        /// </typeparam>
        /// <param name="propertyExpression">
        /// The Expression to retrieve the property Name.
        /// </param>
        /// <param name="propertyValue">
        /// The expected value for the property to check on the considered exception.
        /// </param>
        /// <returns>
        /// A check link.
        /// </returns>
        /// <exception cref="FluentCheckException">
        /// The code did not raised an exception of any type.
        /// </exception>
        ICheckLink<ILambdaExceptionCheck<TException>> WithProperty<TP>(Expression<Func<TException, TP>> propertyExpression, TP propertyValue);
#endif
        /// <summary>
        /// Checks that an inner exception is present within the outer exception stack trace.
        /// </summary>
        /// <typeparam name="T">
        /// Exception type.
        /// </typeparam>
        /// <returns>
        /// A check link.
        /// </returns>
        ILambdaExceptionCheck<T> DueTo<T>()
            where T : Exception;
    }
}