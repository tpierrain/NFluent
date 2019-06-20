// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EqualityMode.cs" company="">
// //   Copyright 2017 Cyrille Dupuydauby
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
    /// 
    /// </summary>
    public enum EqualityMode
    {
        /// <summary>
        /// Compare objects using Equals methods, except for arrays for which comparison is made per entry.
        /// </summary>
        /// <remarks>Default mode</remarks>
        FluentEquals,
        /// <summary>
        /// Compare objects using Equals methods.
        /// </summary>
        Equals,
        /// <summary>
        /// Compare objects using operator==
        /// </summary>
        OperatorEq,
        /// <summary>
        /// Compare objects using operator!=
        /// </summary>
        OperatorNeq
    }
}