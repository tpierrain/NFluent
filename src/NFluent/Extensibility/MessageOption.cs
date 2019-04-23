// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MessageOption.cs" company="NFluent">
//   Copyright 2019 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
namespace NFluent.Extensibility
{
    /// <summary>
    /// Options for message generation.
    /// </summary>
    [System.Flags]
    public enum MessageOption
    {
        /// <summary>
        /// No specific option
        /// </summary>
        None = 0,
        /// <summary>
        /// Removes the description block for the checked value or sut
        /// </summary>
        NoCheckedBlock = 1,
        /// <summary>
        /// Removes the description block for the expected value(s)
        /// </summary>
        NoExpectedBlock = 2,
        /// <summary>
        /// Forces the sut type
        /// </summary>
        ForceType = 4,
        /// <summary>
        /// Add type info
        /// </summary>
        WithType = 8,
        /// <summary>
        /// Add hash for values
        /// </summary>
        WithHash = 16
    }
}