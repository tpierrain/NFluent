// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBlock.cs" company="">
//   Copyright 2014 Cyrille Dupuydauby, Thomas PIERRAIN
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
    using System.Collections;
    using System.Text;
    using Extensions;
    using Messages;

    /// <summary>
    /// Class describing a message block.
    /// </summary>
    public class MessageBlock
    {
        private readonly GenericLabelBlock block;
        private readonly FluentMessage message;
        private IValueDescription value;
        private string comparisonLabel;
        private string customMessage;

 
        internal MessageBlock(FluentMessage message, GenericLabelBlock block)
        {
            this.message = message;
            this.block = block;
        }

        internal static MessageBlock Build<T>(FluentMessage message, T value, GenericLabelBlock block, long index = 0,
            bool forceArray = false)
        {
            var result = new MessageBlock(message, block);
            if (value.IsAnEnumeration(forceArray))
            {
                result.value = new EnumerationBlock<IEnumerable>(value as IEnumerable, index);
            }
            else
            {
                result.value = new ValueBlock<T>(value);
            }
            return result;
        }

        /// <summary>
        /// Gets the Message.
        /// </summary>
        /// <value>
        /// The <see cref="FluentMessage"/> holding that block.
        /// </value>
        public FluentMessage And => this.message;

        /// <summary>
        /// Adds a specific comparison message (e.g 'equal to').
        /// </summary>
        /// <param name="comparison">
        /// The comparison suffix.
        /// </param>
        /// <returns>
        /// The <see cref="MessageBlock"/> for fluent API.
        /// </returns>
        public MessageBlock Comparison(string comparison)
        {
            this.comparisonLabel = comparison;
            return this;
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
            builder.Append(this.FullLabel());
            builder.Append(FluentMessage.EndOfLine);
            builder.Append('\t');
            builder.Append(this.value.GetMessage());
            return builder.ToString();
        }

        /// <summary>
        /// Specifies a specific attribute for the message.
        /// </summary>
        /// <param name="newLabel">
        /// The new attribute.
        /// </param>
        /// <returns>
        /// This <see cref="MessageBlock"/>.
        /// </returns>
        public MessageBlock Label(string newLabel)
        {
            this.customMessage = newLabel;
            return this;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.message.ToString();
        }

        /// <summary>
        /// Adds a description of the number of items (only relevant if the object is an enumerable).
        /// </summary>
        /// <param name="itemsCount">
        /// The number of items of the enumerable instance.
        /// </param>
        /// <returns>
        /// The description of the number of items (only relevant if the object is an enumerable).
        /// </returns>
        public MessageBlock WithEnumerableCount(long itemsCount)
        {
            this.value.WithEnumerableCount(itemsCount);
            return this;
        }

        /// <summary>
        /// Requests that the Hash value is included in the description block.
        /// </summary>
        /// <param name="active">
        /// True to include the type. This is the default value.
        /// </param>
        /// <returns>
        /// Returns this instance for chained calls.
        /// </returns>
        public MessageBlock WithHashCode(bool active = true)
        {
            this.value.WithHashCode(active);
            return this;
        }

        /// <summary>
        /// Requests that the type is included in the description block.
        /// </summary>
        /// <param name="active">
        /// True to include the type. This is the default value.
        /// </param>
        /// <returns>
        /// Returns this instance for chained calls.
        /// </returns>
        public MessageBlock WithType(bool active = true)
        {
            this.value.WithType(active);
            return this;
        }

        /// <summary>
        /// The full label.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string FullLabel()
        {
            string fullLabel;
            if (string.IsNullOrEmpty(this.comparisonLabel))
            {
                fullLabel = this.customMessage == null ? $"The {this.block}:" : this.block.CustomMessage(this.customMessage);
            }
            else
            {
                fullLabel = $"The {this.block}: {this.comparisonLabel}";
            }

            return fullLabel;
        }
    }
}