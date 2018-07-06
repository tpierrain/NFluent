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

// ReSharper disable once CheckNamespace
namespace NFluent.Extensibility
{
    using System;
    using System.Text;

    using Extensions;
    using Messages;

    /// <summary>
    /// Help to build a properly formatted fluent error message.
    /// </summary>
    public class FluentMessage
    {

        internal static readonly string EndOfLine = Environment.NewLine;
        private readonly EntityNamer checkedNamer;
        private readonly EntityNamer expectedNamer;
        private readonly GenericLabelBlock checkedLabel;
        private string message;
        private GenericLabelBlock expectedLabel;
        private MessageBlock expectedBlock;
        private MessageBlock checkedBlock;
        private string entity;
        private Type referenceType;
        private Type checkedType;
        private string customAddOn;

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
            this.message = message ?? throw new ArgumentNullException(nameof(message));
            this.entity = null;
            this.checkedNamer = new EntityNamer();
            this.expectedNamer = new EntityNamer();
            this.checkedLabel = GenericLabelBlock.BuildCheckedBlock(this.checkedNamer);
            this.expectedLabel = GenericLabelBlock.BuildExpectedBlock(this.expectedNamer);
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
            var builder = new StringBuilder(this.customAddOn);
            builder.Append(EndOfLine);
            if (this.referenceType != null)
            {
                this.expectedNamer.EntityType = this.referenceType;
                this.checkedNamer.EntityType = this.checkedType ?? this.referenceType;
            }

            if (this.entity != null)
            {
                this.checkedNamer.EntityName = this.entity;
                this.expectedNamer.EntityName = this.entity;
            }

            var format = this.message;
            format = format.Replace("{checked}", "{0}");
            format = format.Replace("{expected}", "{1}");
            format = format.Replace("{given}", "{1}");

            var givenOrExpectedLabel = this.expectedLabel.ToString();
            var checkedlabel = this.checkedLabel.ToString();
            // analyse structure of sentence
            if ((this.expectedLabel.EntityName() == this.checkedLabel.EntityName()))
            {
                var checkedPos = format.IndexOf("{0}", StringComparison.Ordinal);
                var expectedPos = format.IndexOf("{1}", StringComparison.Ordinal);

                if (checkedPos >= 0 && expectedPos > checkedPos)
                {
                    givenOrExpectedLabel = this.expectedLabel.CustomMessage("{0} one");
                }
                else if (expectedPos >= 0 && checkedPos > expectedPos)
                {
                    checkedlabel = this.checkedLabel.CustomMessage("{0} one");
                }
            }

            builder.AppendFormat(format, checkedlabel, givenOrExpectedLabel);

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
        /// Change the main message.
        /// </summary>
        /// <param name="newMessage">New message to use.</param>
        public void ChangeMessageTo(string newMessage)
        {
            this.message = newMessage;
        }

        /// <summary>
        /// Specifies the attribute to use to describe entities.
        /// </summary>
        /// <param name="newEntityDescription">The new description for the Entity.</param>
        /// <returns>The same fluent message.</returns>
        public FluentMessage For(string newEntityDescription)
        {
            this.entity = newEntityDescription;
            return this;
        }

        /// <summary>
        /// Specifies the type of entities.
        /// </summary>
        /// <param name="forcedType">The type of the Entity.</param>
        /// <returns>The same fluent message.</returns>
        public FluentMessage For(Type forcedType)
        {
            this.referenceType = forcedType;
            return this;
        }

        /// <summary>
        /// Adds a block describing the checked object.
        /// </summary>
        /// <param name="test">
        /// The tested object/value.
        /// </param>
        /// <param name="index">
        /// The interesting index (for enumerable types).
        /// </param>
        /// <returns>
        /// A <see cref="FluentMessage"/> to continue build the message.
        /// </returns>
        public MessageBlock On<T>(T test, long index = 0)
        {
            this.checkedBlock = new MessageBlock(this, test, this.checkedLabel, index);
            if (this.referenceType == null)
            {
                this.referenceType = test.GetTypeWithoutThrowingException();
            }

            this.checkedType = test.GetTypeWithoutThrowingException();

            return this.checkedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected result.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// The created MessageBlock.
        /// </returns>
        public MessageBlock Expected(object expected)
        {
            this.expectedBlock = new MessageBlock(this, expected, this.expectedLabel);
            this.referenceType = expected.GetTypeWithoutThrowingException();
            return this.expectedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected result.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// The created MessageBlock.
        /// </returns>
        public MessageBlock ReferenceValues(object expected)
        {
            this.expectedBlock = new MessageBlock(this, expected, this.expectedLabel);
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
            this.referenceType = null;
            return this.expectedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected values.
        /// </summary>
        /// <param name="expectedValues">
        /// The expected values.
        /// </param>
        /// <param name="index">
        /// The index to highlight.
        /// </param>
        /// <returns>
        /// The created MessageBlock.
        /// </returns>
        public MessageBlock ExpectedValues(object expectedValues, long index = 0)
        {
            this.expectedLabel = GenericLabelBlock.BuildExpectedBlock(new EntityNamer { EntityName = "value(s)" });
            this.expectedBlock = new MessageBlock(this, expectedValues, this.expectedLabel, index);
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
            this.expectedLabel = GenericLabelBlock.BuildGivenBlock(this.checkedNamer);
            this.expectedBlock = new MessageBlock(this, givenValue, this.expectedLabel);
            return this.expectedBlock;
        }

        /// <summary>
        /// Adds a fully custom add on error message (first line).
        /// </summary>
        /// <param name="customAddOnMessage"></param>
        public void AddCustomMessage(string customAddOnMessage)
        {
            this.customAddOn = customAddOnMessage;
        }
    }
}