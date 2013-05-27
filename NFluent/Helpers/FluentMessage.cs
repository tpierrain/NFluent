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
    using System.Collections.Generic;
    using System.Text;

    using NFluent.Extensions;

    /// <summary>
    /// Help to build a properly formatted fluent error message.
    /// </summary>
    internal class FluentMessage
    {
        private readonly string message;
        private readonly List<SubBlock> subBlocks = new List<SubBlock>();
        private string entity;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentMessage"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public FluentMessage(string message)
        {
            this.message = message;
            this.entity = "value";
        }

        private string Entity
        {
            get
            {
                return this.entity;
            }
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
            builder.AppendFormat(this.message, string.Format("checked {0}", this.entity), string.Format("expected {0}", this.entity));
            foreach (var subBlock in this.subBlocks)
            {
                builder.AppendLine();
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
            this.entity = newEntity;
            return this;
        }

        /// <summary>
        /// Adds a block describing the checked objet.
        /// </summary>
        /// <param name="test">The tested object/value.</param>
        /// <returns>A <see cref="FluentMessage"/> to continue build the message.</returns>
        public SubBlock On(object test)
        {
            var subBlock = new SubBlock(this, test, "checked");
            this.subBlocks.Add(subBlock);
            return subBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected result.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>The created SubBlock.</returns>
        public SubBlock Expected(object expected)
        {
            var subBlock = new SubBlock(this, expected, "expected");
            this.subBlocks.Add(subBlock);
            return subBlock;
        }

        /// <summary>
        /// Class describing a message block.
        /// </summary>
        internal class SubBlock
        {
            private readonly FluentMessage message;

            private readonly object test;

            private readonly string attribute;

            private string customMessage;

            private string comparisonLabel;

            /// <summary>
            /// Initializes a new instance of the <see cref="SubBlock"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="test">The tested object.</param>
            /// <param name="attribute">The block attribute.</param>
            public SubBlock(FluentMessage message, object test, string attribute)
            {
                this.message = message;
                this.test = test;
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

                builder.AppendLine();
                builder.AppendFormat(
                    "\t[{0}]",
                    this.test.ToStringProperlyFormated());
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
                if (this.message != null)
                {
                    return this.message.ToString();                    
                }

                return this.GetMessage();
            }

            /// <summary>
            /// Adds a message block to describe the expected result.
            /// </summary>
            /// <param name="expected">The expected value.</param>
            /// <returns>The created SubBlock.</returns>
            public SubBlock Expected(object expected)
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
            /// The <see cref="SubBlock"/> for fluent API.
            /// </returns>
            public SubBlock Comparison(string comparison)
            {
                this.comparisonLabel = comparison;
                return this;
            }

            /// <summary>
            /// Specifies a specific attribute for the message.
            /// </summary>
            /// <param name="newLabel">The new attribute.</param>
            /// <returns>This <see cref="SubBlock"/>.</returns>
            public SubBlock Label(string newLabel)
            {
                this.customMessage = newLabel;
                return this;
            }
        }
    }
}