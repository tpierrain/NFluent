#region File header

// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IValueDescription.cs" company="">
// //   Copyright 2014 Cyrille Dupuydauby, Thomas PIERRAIN
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
#endregion

namespace NFluent.Extensibility
{
    using System;

    /// <summary>
    /// Interface for a value description class.
    /// </summary>
    internal interface IValueDescription
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the message as a string.
        /// </summary>
        /// <returns>
        /// A string with the properly formatted message.
        /// </returns>
        string GetMessage();

        /// <summary>
        /// Adds a description of the number of items (only relevant if the object is an enumerable).
        /// </summary>
        /// <param name="itemsCount">
        /// The number of items of the enumerable instance.
        /// </param>
        void WithEnumerableCount(long itemsCount);

        /// <summary>
        /// Requests that the Hash value is included in the description block.
        /// </summary>
        /// <param name="active">
        /// True to include the type. This is the default value.
        /// </param>
        void WithHashCode(bool active = true);

        /// <summary>
        /// Requests that the type is included in the description block.
        /// </summary>
        /// <param name="active">
        /// True to include the type. This is the default value.
        /// </param>
        /// <param name="full">
        /// True to display the full type name (with assembly).
        /// </param>
        void WithType(bool active = true, bool full = false);

        /// <summary>
        /// Requests that a specific type is included in the description block.
        /// </summary>
        /// <param name="forcedType">
        /// Type to include in the description.
        /// </param>
        /// <remarks>
        /// Default type is the type of the object instance given in constructor.
        /// </remarks>
        void WithType(Type forcedType);

        #endregion
    }
}