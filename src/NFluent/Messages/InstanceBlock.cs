 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="InstanceBlock.cs" company="">
 //   Copyright 2014 Cyrille Dupuydauby,Thomas PIERRAIN
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

namespace NFluent.Messages
{
    using System;
    using System.Text;
    using Extensions;

    /// <summary>
    /// Class describing block for any instance of a given type.
    /// </summary>
    internal class InstanceBlock : IValueDescription
    {
        #region Fields

        /// <summary>
        /// The instance type.
        /// </summary>
        private readonly Type type;

        /// <summary>
        /// The full type name.
        /// </summary>
        private bool fullTypeName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceBlock"/> class.
        /// </summary>
        /// <param name="type">
        /// The tested type.
        /// </param>
        public InstanceBlock(Type type)
        {
            this.type = type;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the message as a string.
        /// </summary>
        /// <returns>
        /// A string with the properly formatted message.
        /// </returns>
        public string GetMessage()
        {
            var builder = new StringBuilder();
            builder.Append("an instance");

            var temp = this.fullTypeName ? this.type.AssemblyQualifiedName : this.type.ToStringProperlyFormatted();
            builder.AppendFormat(" of type: [{0}]", temp);

            return builder.ToString();
        }

        /// <summary>
        /// Adds a description of the number of items (only relevant if the object is an enumerable).
        /// </summary>
        /// <param name="itemsCount">
        /// The number of items of the enumerable instance.
        /// </param>
        public void WithEnumerableCount(long itemsCount)
        {
            throw new NotSupportedException("Cannot use enumeration for generic instance description!");
        }

        /// <summary>
        /// Requests that the Hash value is included in the description block.
        /// </summary>
        /// <param name="active">
        /// True to include the type. This is the default value.
        /// </param>
        public void WithHashCode(bool active = true)
        {
            throw new NotSupportedException("Cannot use hash code for generic instance description!");
        }

        /// <summary>
        /// Requests that the type is included in the description block.
        /// </summary>
        /// <param name="active">
        /// True to include the type. This is the default value.
        /// </param>
        /// <param name="full">
        /// True to display the full type name (with assembly).
        /// </param>
        public void WithType(bool active = true, bool full = false)
        {
            this.fullTypeName = full;
        }

        /// <summary>
        /// Requests that a specific type is included in the description block.
        /// </summary>
        /// <param name="forcedType">
        /// Type to include in the description.
        /// </param>
        /// <remarks>
        /// Default type is the type of the object instance given in constructor.
        /// </remarks>
        public void WithType(Type forcedType)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}   