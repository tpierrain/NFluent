// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentMessage.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Extensibility
{
    using System;
    using System.Text;
    using NFluent.Extensions;

    // TODO: probably worth to refactor the implementation of this class

    /// <summary>
    /// Help to build a properly formatted fluent error message.
    /// </summary>
    public class FluentMessage
    {
        #region fields

        private const string DefaultEntity = "value";

        private const string TestedAdjective = "checked";

        private const string ExpectedAdjective = "expected";

        private const string GivenAdjective = "given";

        private readonly string message;

        private MessageBlock expectedBlock = null;

        private MessageBlock checkedBlock = null;

        private MessageBlock givenValueBlock = null;

        private MessageBlock expectedValuesBlock = null;

        private string entity;

        #endregion

        #region constructor

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
            this.EntityDescription = null;
        }

        #endregion

        #region properties

        private string EntityDescription
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
        /// Gets the expected values label.
        /// </summary>
        /// <value>
        /// The expected values label.
        /// </value>
        public string ExpectedValuesLabel
        {
            get
            {
                return "expected value(s)";
            }
        }

        /// <summary>
        /// Gets the given value label.
        /// </summary>
        /// <value>
        /// The given value label.
        /// </value>
        protected string GivenLabel
        {
            get
            {
                return string.Format("{0} {1}", GivenAdjective, this.EntityDescription);
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
                if (this.checkedBlock == null)
                {
                    return string.Format("{0} {1}", TestedAdjective, this.EntityDescription);
                }

                return this.checkedBlock.GetBlockLabel();
            }
        }

        #endregion

        #region methods

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
            var builder = new StringBuilder("\n");
            var givenOrExpectedLabel = this.ExpectedLabel;

            if (this.givenValueBlock != null)
            {
                // we defined a given block which should then replace the classical expected one.
                givenOrExpectedLabel = this.GivenLabel;
            }

            if (this.expectedValuesBlock != null)
            {
                // we defined an expected values block which should then replace the classical expected one.
                givenOrExpectedLabel = this.ExpectedValuesLabel;
            }

            builder.AppendFormat(this.message, this.TestedLabel, givenOrExpectedLabel);

            if (this.checkedBlock != null)
            {
                builder.Append("\n");
                builder.Append(this.checkedBlock.GetMessage());
            }

            if (this.givenValueBlock != null)
            {
                builder.Append("\n");
                builder.Append(this.givenValueBlock.GetMessage());
            }

            if (this.expectedValuesBlock != null)
            {
                builder.Append("\n");
                builder.Append(this.expectedValuesBlock.GetMessage());
            }

            if (this.expectedBlock != null)
            {
                builder.Append("\n");
                builder.Append(this.expectedBlock.GetMessage());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Specifies the attribute to use to describe entities.
        /// </summary>
        /// <param name="newEntityDescription">The new description for the Entity.</param>
        /// <returns>The same fluent message.</returns>
        public FluentMessage For(string newEntityDescription)
        {
            this.EntityDescription = newEntityDescription;
            return this;
        }

        /// <summary>
        /// Adds a block describing the checked objet.
        /// </summary>
        /// <param name="test">The tested object/value.</param>
        /// <returns>A <see cref="FluentMessage"/> to continue build the message.</returns>
        public MessageBlock On(object test)
        {
            this.checkedBlock = new MessageBlock(this, test, TestedAdjective);
            return this.checkedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected result.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>The created MessageBlock.</returns>
        public MessageBlock Expected(object expected)
        {
            this.expectedBlock = new MessageBlock(this, expected, ExpectedAdjective);
            return this.expectedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected values.
        /// </summary>
        /// <param name="expectedValues">The expected values.</param>
        /// <returns>The created MessageBlock.</returns>
        public MessageBlock ExpectedValues(object expectedValues)
        {
            this.expectedValuesBlock = new MessageBlock(this, expectedValues, ExpectedAdjective);
            this.expectedValuesBlock.Label("The expected value(s):");
            return this.expectedValuesBlock;
        }

        /// <summary>
        /// Adds a message block to describe the given value (usually used as an 
        /// alternative to the Expected block).
        /// </summary>
        /// <param name="givenValue">The given value.</param>
        /// <returns>The created MessageBlock.</returns>
        public MessageBlock WithGivenValue(object givenValue)
        {
            this.givenValueBlock = new MessageBlock(this, givenValue, GivenAdjective);
            return this.givenValueBlock;
        }

        /// <summary>
        /// Gets the entity label based on the given type.
        /// </summary>
        /// <param name="value">The value to get the type from.</param>
        /// <returns>The appropriate entity label.</returns>
        private string GetEntityFromType(object value)
        {
            if (this.entity != null)
            {
                return this.entity;
            }

            return DefaultEntity;
        }

        #endregion

       /// <summary>
        /// Class describing a message block.
        /// </summary>
        public class MessageBlock
        {
            #region fields

            private readonly FluentMessage message;

            private readonly object test;

            private readonly string attribute;

            private string customMessage;

            private string comparisonLabel;

            private bool includeHash;

            private bool includeType;

            private Type type;

            private long? enumerableCount;

            #endregion

            #region constructor

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

            #endregion

            /// <summary>
            /// Gets the Message.
            /// </summary>
            /// <value>
            /// The <see cref="FluentMessage"/> holding that block.
            /// </value>
            public FluentMessage And
            {
                get
                {
                    return this.message;
                }
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
                    builder.AppendFormat(this.customMessage ?? "The {0} {1}:", this.attribute, this.message.GetEntityFromType(this.test));
                }
                else
                {
                    builder.AppendFormat(this.customMessage ?? "The {0} {1}: {2}", this.attribute, this.message.GetEntityFromType(this.test), this.comparisonLabel);
                }

                builder.Append("\n");

                if (this.test == null)
                {
                    builder.Append("\t[null]");
                }
                else
                {
                    builder.AppendFormat("\t[{0}]", this.test.ToStringProperlyFormated());
                }

                if (this.enumerableCount.HasValue)
                {
                    var description = "items";
                    if (this.enumerableCount <= 1)
                    {
                        description = "item";
                    }

                    builder.AppendFormat(" ({0} {1})", this.enumerableCount, description);
                }

                if (this.includeType && this.type != null)
                {
                    builder.AppendFormat(" of type: [{0}]", this.type.ToStringProperlyFormated());
                }

                if (this.includeHash && this.test != null)
                {
                    builder.AppendFormat(" with HashCode: [{0}]", this.test.GetHashCode());
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

            /// <summary>
            /// Gets the block label.
            /// </summary>
            /// <returns>The block Label.</returns>
            public string GetBlockLabel()
            {
                return string.Format("{0} {1}", this.attribute, this.message.GetEntityFromType(this.test));
            }

            /// <summary>
            /// Adds a description of the number of items (only relevant if the object is an enumerable).
            /// </summary>
            /// <param name="itemsCount">The number of items of the enumerable instance.</param>
            /// <returns>The description of the number of items (only relevant if the object is an enumerable).</returns>
            public MessageBlock WithEnumerableCount(long itemsCount)
            {
                this.enumerableCount = itemsCount;
                return this;
            }
        }
    }
}