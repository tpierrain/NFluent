// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="RunTrace.cs" company="">
// //   Copyright 2014 Cyrille DUPUYDAUBY
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
    using System.Diagnostics;

    /// <summary>
    /// This class stores trace information for a code evaluation.
    /// </summary>
    public class RunTrace
    {
        /// <summary>
        /// Gets or sets the execution time of the checked code.
        /// </summary>
        /// <value>
        /// The execution time.
        /// </value>
        public Stopwatch ExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the raised exception.
        /// </summary>
        /// <value>
        /// The raised exception.
        /// </value>
        public Exception RaisedException { get; set; }

        /// <summary>
        /// Gets or sets the total processor time.
        /// </summary>
        /// <value>
        /// The total processor time.
        /// </value>
        public TimeSpan TotalProcessorTime { get; set; }
    }
}