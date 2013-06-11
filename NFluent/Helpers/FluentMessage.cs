// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentMessage.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
//   // //   Licensed under the Apache License, Version 2.0 (the "License");
//   // //   you may not use this file except in compliance with the License.
//   // //   You may obtain a copy of the License at
//   // //       http://www.apache.org/licenses/LICENSE-2.0
//   // //   Unless required by applicable law or agreed to in writing, software
//   // //   distributed under the License is distributed on an "AS IS" BASIS,
//   // //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   // //   See the License for the specific language governing permissions and
//   // //   limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using NFluent.Extensions;

    /// <summary>
    /// Help to build a properly formatted fluent error message.
    /// </summary>
    internal class FluentMessage
    {
        private const string DefaultEntity = "value";

        private const string TestedAdjective = "checked";

        private const string ExpectedAdjective = "expected";

        private readonly string message;
        private readonly List<MessageBlock> subBlocks = new List<MessageBlock>();

        private string entity;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentMessage"/> class.
        /// </summary>
        /// <param name="message">
        /// The main message.
        /// </param>
        /// <remarks>
        /// You can use {x} as place holders for standard wordings:
        /// - {0}. 
        /// </remarks>
        public FluentMessage(string message)
        {
            this.message = message;
            this.Entity = null;
        }

        private string Entity
        {
            get
            {
                return this.entity ?? DefaultEntity;
            }

            set
            {
                this.entity = value;
            }
        }

        /// <summary>
        /// Gets the expected value label.
        /// </summary>
        /// <value>
        /// The expected label.
        /// </value>
        public string ExpectedLabel
        {
            get
            {
                return string.Format("{0} {1}", ExpectedAdjective, "one");
            }
        }

        /// <summary>
        /// Gets the tested value label.
        /// </summary>
        /// <value>
        /// The tested label.
        /// </value>
        public string TestedLabel
        {
            get
            {
                return string.Format("{0} {1}", TestedAdjective, this.Entity);
            }
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A fluent message builder.</returns>
        public static FluentMessage BuildMessage(string message)
        {
            return new FluentMessage(message);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendFormat(this.message, this.TestedLabel, this.ExpectedLabel);
            foreach (var subBlock in this.subBlocks)
            {
                builder.Append("\n");
                builder.Append(subBlock.GetMessage());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Specifies the attribute to use to describe entities.
        /// </summary>
        /// <param name="newEntity">The newEntity.</param>
        /// <returns>The same fluent message.</returns>
        public FluentMessage For(string newEntity)
        {
            this.Entity = newEntity;
            return this;
        }

        /// <summary>
        /// Adds a block describing the checked objet.
        /// </summary>
        /// <param name="test">The tested object/value.</param>
        /// <returns>A <see cref="FluentMessage"/> to continue build the message.</returns>
        public MessageBlock On(object test)
        {
            var subBlock = new MessageBlock(this, test, TestedAdjective);
            this.subBlocks.Add(subBlock);
            return subBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected result.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>The created MessageBlock.</returns>
        public MessageBlock Expected(object expected)
        {
            var subBlock = new MessageBlock(this, expected, ExpectedAdjective);
            this.subBlocks.Add(subBlock);
            return subBlock;
        }

        /// <summary>
        /// Class describing a message block.
        /// </summary>
        public class MessageBlock
        {
            private readonly FluentMessage message;

            private readonly object test;

            private readonly string attribute;

            private string customMessage;

            private string comparisonLabel;

            private bool includeHash;

            private bool includeType;

            private Type type;

            /// <summary>
            /// Initializes a new instance of the <see cref="MessageBlock"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="test">The tested object.</param>
            /// <param name="attribute">The block attribute.</param>
            public MessageBlock(FluentMessage message, object test, string attribute)
            {
                if (message == null)
                {
                    throw new ArgumentNullException("message");
                }

                this.message = message;
                this.test = test;
                this.type = test.GetTypeWithoutThrowingException();
                this.attribute = attribute;
            }

            /// <summary>
            /// Gets the message as a string.
            /// </summary>
            /// <returns>A string with the properly formatted message.</returns>
            public string GetMessage()
            {
                var builder = new StringBuilder();
                if (string.IsNullOrEmpty(this.comparisonLabel))
                {
                    builder.AppendFormat(this.customMessage ?? "The {0} {1}:", this.attribute, this.message.Entity);
                }
                else
                {
                    builder.AppendFormat(this.customMessage ?? "The {0} {1}: {2}", this.attribute, this.message.Entity, this.comparisonLabel);
                }

                builder.Append("\n");
                if (this.test == null)
                {
                    builder.AppendFormat("\t[null]");
                }
                else
                {
                    builder.AppendFormat(
                        "\t[{0}]",
                        this.test.ToStringProperlyFormated());
                }

                if (this.includeType && this.type != null)
                {
                    builder.AppendFormat(
                        " of type: [{0}]", this.type);
                }

                if (this.includeHash && this.test != null)
                {
                    builder.AppendFormat(
                        " with HashCode: [{0}]", this.test.GetHashCode());
                }
                
                return builder.ToString();
            }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return this.message.ToString();                    
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
                this.includeHash = active;
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
                this.includeType = active;
                return this;
            }

            /// <summary>
            /// Requests that a specific type is included in the description block.
            /// </summary>
            /// <param name="forcedType">Type to include in the description.</param>
            /// <remarks>Default type is the type of the object instance given in constructor.</remarks>
            /// <returns>
            /// Returns this instance for chained calls.
            /// </returns>
            public MessageBlock WithType(Type forcedType)
            {
                this.type = forcedType;
                this.includeType = true;
                return this;
            }

            /// <summary>
            /// Adds a message block to describe the expected result.
            /// </summary>
            /// <param name="expected">The expected value.</param>
            /// <returns>The created MessageBlock.</returns>
            public MessageBlock Expected(object expected)
            {
                return this.message.Expected(expected);
            }

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
            /// Specifies a specific attribute for the message.
            /// </summary>
            /// <param name="newLabel">The new attribute.</param>
            /// <returns>This <see cref="MessageBlock"/>.</returns>
            public MessageBlock Label(string newLabel)
            {
                this.customMessage = newLabel;
                return this;
            }
        }
    }
}