// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IExceptionConstructor.cs" company="NFluent">
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

    /// <summary>
    /// Provides exception constructors depending on the testing framework.
    /// </summary>
    internal interface IExceptionConstructor
    {
        Type InconclusiveExceptionType();
        Exception BuildFailedException(string message);
        Exception BuildInconclusiveException(string message);
        Type FailedExceptionType();
    }
}