// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="RunTraceResult.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
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
    /// <summary>
    /// This code stores trace information for a code evaluation and its returned value.
    /// </summary>
    /// <typeparam name="T">Code return type.</typeparam>
    public class RunTraceResult<T> : RunTrace
    {
        /// <summary>
        /// Gets or sets the result of the evaluated code.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public T Result { get; set; }
    }
}