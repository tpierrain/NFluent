 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="ValueBlock.cs" company="">
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
    /// Class describing a value block.
    /// </summary>
    internal class ValueBlock : IValueDescription
    {
        private readonly object test;
        private long? enumerableCount;

        private bool fullTypeName;

        private bool includeHash;
        private bool includeType;
        private Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueBlock"/> class.
        /// </summary>
        /// <param name="test">
        /// The tested object.
        /// </param>
        public ValueBlock(object test)
        {
            this.test = test;
            this.type = test.GetTypeWithoutThrowingException();
        }

        /// <summary>
        /// Gets the message as a string.
        /// </summary>
        /// <returns>
        /// A string with the properly formatted message.
        /// </returns>
        public string GetMessage()
        {
            var builder = new StringBuilder();
            builder.Append(this.Description());

            if (this.includeType && this.type != null)
            {
                var temp = this.fullTypeName ? this.type.AssemblyQualifiedName : this.type.ToStringProperlyFormatted();
                builder.AppendFormat(" of type: [{0}]", temp);
            }

            if (this.includeHash && this.test != null)
            {
                builder.AppendFormat(" with HashCode: [{0}]", this.test.GetHashCode());
            }

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
            this.enumerableCount = itemsCount;
        }

        /// <summary>
        /// Requests that the Hash value is included in the description block.
        /// </summary>
        /// <param name="active">
        /// True to include the type. This is the default value.
        /// </param>
        public void WithHashCode(bool active = true)
        {
            this.includeHash = active;
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
            this.includeType = active;
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
            this.type = forcedType;
        }

        /// <summary>
        /// The description.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string Description()
        {
            var description = new StringBuilder();
            description.AppendFormat("[{0}]", this.test.ToStringProperlyFormatted());

            var count = this.enumerableCount.GetValueOrDefault(-1);
            if (count > 1)
            {
                description.AppendFormat(" ({0} items)", count);
            }
            else if (count >= 0)
            {
                description.AppendFormat(" ({0} item)", count);
            }

            return description.ToString();
        }
    }
}