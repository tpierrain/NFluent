// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DefaultExceptionConstructor.cs" company="NFluent">
//   Copyright 2023 Cyrille DUPUYDAUBY
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

namespace NFluent.Helpers
{
    using System;
    using Kernel;

    /// <summary>
    /// Provides a default exception constructor that returns a FluentCheckException.
    /// </summary>
    internal class DefaultExceptionConstructor : IExceptionConstructor
    {
        public Exception BuildFailedException(string message)
        {
            return new FluentCheckException(message);
        }

        public Exception BuildInconclusiveException(string message)
        {
            return new FluentCheckException(message);
        }

        public Type FailedExceptionType()
        {
            return typeof(FluentCheckException);
        }

        public Type InconclusiveExceptionType()
        {
            return typeof(FluentCheckException);
        }
    }
}