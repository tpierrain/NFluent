// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILambdaExceptionCheck.cs" company="">
//   Copyright 2013 Rui CARVALHO, Thomas PIERRAIN
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent
{
    using System;

    /// <summary>
    /// Provides check methods to be executed on the exception raised by a given lambda/action.
    /// </summary>
    /// <typeparam name="TException">The type of the exception.</typeparam>
    public interface ILambdaExceptionCheck<out TException> : IHasParentCheck<TException>,
        IMustImplementIForkableCheckWithoutDisplayingItsMethodsWithinIntelliSense
        where TException : Exception
    {

        /// <summary>
        /// Gets or sets with the parent class that fluently called this one.
        /// </summary>
        /// <actualValue>
        /// The actualValue.
        /// </actualValue>
        TException Value { get; }

        /// <summary>
        /// Checks if the exception was due to an (inner) exception of a specified type.
        /// </summary>
        /// <typeparam name="TE">Expected inner exception type</typeparam>
        /// <returns>A chainable link</returns>
        ILambdaExceptionCheck<TE> DueTo<TE>()
            where TE : Exception;
    }
}