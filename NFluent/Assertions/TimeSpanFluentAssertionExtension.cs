// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="TimeSpanFluentAssertionExtension.cs" company="">
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
    using System;

    /// <summary>
    /// Provides assertion methods to be executed on a TimeSpan.
    /// </summary>
    public static class TimeSpanFluentAssertionExtension
    {
        /// <summary>
        /// Gets a FluentAssert to check the TimeSpan duration in milliseconds.
        /// </summary>
        /// <param name="fluentAssertion">
        /// </param>
        /// <returns>
        /// </returns>
        public static IFluentAssertion<double> InMilliseconds(this IFluentAssertion<TimeSpan> fluentAssertion)
        {
            return new FluentAssertion<double>(fluentAssertion.Value.TotalMilliseconds);
        }
    }
}