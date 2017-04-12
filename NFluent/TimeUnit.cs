// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="TimeUnit.cs" company="">
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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Enumerate the available time unit.
    /// </summary>
    public enum TimeUnit : long
    {
        /// <summary>
        /// The nanoseconds.
        /// </summary>
        Nanoseconds, 

        /// <summary>
        /// The Microseconds.
        /// </summary>
        Microseconds, 

        /// <summary>
        /// The Milliseconds.
        /// </summary>
        Milliseconds, 

        /// <summary>
        /// The seconds.
        /// </summary>
        Seconds, 

        /// <summary>
        /// The minutes.
        /// </summary>
        Minutes, 

        /// <summary>
        /// The hours.
        /// </summary>
        Hours, 

        /// <summary>
        /// The days.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1632:DocumentationTextMustMeetMinimumCharacterLength", Justification = "Reviewed. Suppression is OK here.")]
        Days, 

        /// <summary>
        /// The weeks.
        /// </summary>
        Weeks
    }
}