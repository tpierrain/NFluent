// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckContext.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Kernel
{
    /// <summary>
    /// Describes default execution context. Used to control context for tests.
    /// </summary>
    internal static class CheckContext
    {
        /// <summary>
        /// Gets/Sets the default negation status
        /// </summary>
        /// <remarks>This property is used for NFluent checking.</remarks>
        public static bool DefaulNegated { get; set; } = true;
    }
}