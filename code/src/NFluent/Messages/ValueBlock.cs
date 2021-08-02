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
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Extensions;

    /// <summary>
    /// Class describing a value block.
    /// </summary>
    internal class ValueBlock<T> : IValueDescription
    {
        private readonly T value;
        private bool includeHash;
        private bool includeType;
        private readonly Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueBlock{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The tested object.
        /// </param>
        public ValueBlock(T value)
        {
            this.value = value;
            this.type = value.GetTypeWithoutThrowingException();
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

            if (this.includeType)
            {
                builder.AppendFormat(" of type: [{0}]", this.type.ToStringProperlyFormatted());
            }

            if (this.includeHash && this.value != null)
            {
                builder.AppendFormat(" with HashCode: [{0}]", this.value.GetHashCode());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Adds a description of the number of items (only relevant if the object is an enumerable).
        /// </summary>
        /// <param name="itemsCount">
        /// The number of items of the enumerable instance.
        /// </param>
        [ExcludeFromCodeCoverage]
        public void WithEnumerableCount(long itemsCount)
        {
            throw new NotImplementedException();
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
        public void WithType(bool active = true)
        {
            this.includeType = active;
        }

        /// <summary>
        /// The description.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string Description()
        {
            if (this.value is ISelfDescriptiveValue self)
            {
                return self.ValueDescription;
            }
            return $"[{this.value.ToStringProperlyFormatted()}]";
        }
    }
}