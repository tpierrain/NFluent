// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentMessage.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
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

        internal const string EndOfLine = "\n";

        private readonly string message;

        private readonly EntityNamer entityNamer;

        private readonly ILabelBlock checkedLabel;

        private ILabelBlock expectedLabel;

        private MessageBlock expectedBlock;

        private MessageBlock checkedBlock;

        private string entity;

        private Type referenceType;

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
        private FluentMessage(string message)
        {
            this.message = message;
            this.EntityDescription = null;
            this.entityNamer = new EntityNamer();
            this.checkedLabel = GenericLabelBlock.BuildCheckedBlock(this.entityNamer);
            this.expectedLabel = GenericLabelBlock.BuildExpectedBlock(this.entityNamer);
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
        /// Gets the expected values label.
        /// </summary>
        /// <value>
        /// The expected values label.
        /// </value>
        private string ExpectedValuesLabel
        {
            get
            {
                return "expected value(s)";
            }
        }

        /// <summary>
        /// Gets the tested value label.
        /// </summary>
        /// <value>
        /// The tested label.
        /// </value>
        private string TestedLabel
        {
            get
            {
                return this.checkedLabel.ToString();
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
            var builder = new StringBuilder(EndOfLine);
            if (this.referenceType != null)
            {
                this.entityNamer.EntityType = this.referenceType;
            }

            if (this.entity != null)
            {
                this.entityNamer.EntityName = this.entity;
            }

            var givenOrExpectedLabel = this.expectedLabel.CustomMessage("{0} one");

            builder.AppendFormat(this.message, this.TestedLabel, givenOrExpectedLabel);

            if (this.checkedBlock != null)
            {
                builder.Append(EndOfLine);
                builder.Append(this.checkedBlock.GetMessage());
            }

            if (this.expectedBlock != null)
            {
                builder.Append(EndOfLine);
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
            this.checkedBlock = new MessageBlock(this, test, this.checkedLabel);
            if (this.referenceType == null)
            {
                this.referenceType = test.GetTypeWithoutThrowingException();
            }

            return this.checkedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected result.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>The created MessageBlock.</returns>
        public MessageBlock Expected(object expected)
        {
            this.expectedBlock = new MessageBlock(this, expected, this.expectedLabel);
            this.referenceType = expected.GetTypeWithoutThrowingException();
            return this.expectedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected type.
        /// </summary>
        /// <param name="expectedType">The expected type.</param>
        /// <returns>The created MessageBlock.</returns>
        public MessageBlock ExpectedType(Type expectedType)
        {
            this.expectedBlock = new MessageBlock(this, expectedType, this.expectedLabel);
            this.referenceType = expectedType;
            return this.expectedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected values.
        /// </summary>
        /// <param name="expectedValues">The expected values.</param>
        /// <returns>The created MessageBlock.</returns>
        public MessageBlock ExpectedValues(object expectedValues)
        {
            var customNamer = new EntityNamer { EntityName = "value(s)" };
            this.expectedLabel = GenericLabelBlock.BuildExpectedBlock(customNamer);
            this.expectedBlock = new MessageBlock(this, expectedValues, this.expectedLabel);
            this.referenceType = expectedValues.GetTypeWithoutThrowingException();
            return this.expectedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the given value (usually used as an 
        /// alternative to the Expected block).
        /// </summary>
        /// <param name="givenValue">The given value.</param>
        /// <returns>The created MessageBlock.</returns>
        public MessageBlock WithGivenValue(object givenValue)
        {
            this.expectedLabel = GenericLabelBlock.BuildGivenBlock(this.entityNamer);
            this.expectedBlock = new MessageBlock(this, givenValue, this.expectedLabel);
            return this.expectedBlock;
        }

        #endregion
    }
}