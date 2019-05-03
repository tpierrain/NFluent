//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnumerationBlock.cs" company="">
//    Copyright 2014 Cyrille DUPUYDAUBY
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Messages
{
    using System;
    using System.Collections;
    using System.Text;
    using Extensions;

    /// <summary>
    /// Class describing a value block.
    /// </summary>
    internal class EnumerationBlock : IValueDescription
    {
        private const int NumberOfItemsToList = 20;

        /// <summary>
        /// The tested object.
        /// </summary>
        private readonly IEnumerable test;

        private readonly long referenceIndex;

        /// <summary>
        /// The enumerable count.
        /// </summary>
        private long? enumerableCount;

        /// <summary>
        /// The include type.
        /// </summary>
        private bool includeType;

        /// <summary>
        /// The instance type.
        /// </summary>
        private Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerationBlock" /> class.
        /// </summary>
        /// <param name="test">The tested object.</param>
        /// <param name="referenceIndex">Index of the reference value.</param>
        public EnumerationBlock(IEnumerable test, long referenceIndex)
        {
            this.test = test;
            this.referenceIndex = referenceIndex;
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
                builder.AppendFormat(" of type: [{0}]", this.type.ToStringProperlyFormatted());
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
            this.includeType = true;
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
            description.Append(this.test.ToEnumeratedString(this.referenceIndex, NumberOfItemsToList));

            if (this.enumerableCount.HasValue && this.test != null)
            {
                description.AppendFormat(
                    " ({0} {1})",
                    this.enumerableCount,
                    this.enumerableCount <= 1 ? "item" : "items");
            }

            return description.ToString();
        }
    }
}