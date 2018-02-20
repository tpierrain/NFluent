// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NegatedLambdaExceptionCheck.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
#if !DOTNET_20 && !DOTNET_30
    using System.Linq.Expressions;
#endif
 
    /// <inheritdoc />
    internal class NegatedLambdaExceptionCheck<T> : ILambdaExceptionCheck<T> where T : Exception
    {
        /// <inheritdoc />
        public ICheckLink<ILambdaExceptionCheck<T>> WithMessage(string exceptionMessage)
        {
            throw new InvalidOperationException("You cannot use WithMessage on negated check.");
        }

        /// <inheritdoc />
        public ICheckLink<ILambdaExceptionCheck<T>> WithProperty<TP>(string propertyName, TP propertyValue)
        {
            throw new InvalidOperationException("You cannot use WithProperty on negated check.");
        }

#if !DOTNET_20 && !DOTNET_30
        /// <inheritdoc />
        public ICheckLink<ILambdaExceptionCheck<T>> WithProperty<TP>(Expression<Func<T, TP>> propertyExpression,
            TP propertyValue)
        {
            throw new InvalidOperationException("You cannot use WithProperty on negated check.");
        }
#endif

        /// <inheritdoc />
        public ILambdaExceptionCheck<TU> DueTo<TU>() where TU : Exception
        {
            throw new InvalidOperationException("You cannot use DueTo on negated check.");
        }
    }
}