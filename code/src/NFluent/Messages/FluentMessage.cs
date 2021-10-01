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
    using System.Text.RegularExpressions;
    using Extensions;
    using Messages;

    /// <summary>
    /// Help to build a properly formatted fluent error message.
    /// Reminder of the naming requirements/rules regarding sut and expected/given values:
    /// 1) the objective is provide descriptive naming
    /// 2) naming is typically done according to data type
    /// 3) specific: if the expected value is null, it should be referred as 'value' instead of its type.
    /// 3) user can provide a custom name. It will supersedes automatic naming
    /// </summary>
    public class FluentMessage
    {
        internal static readonly string EndOfLine = Environment.NewLine;
        private readonly EntityNamingLogic checkedNamingLogic;
        private readonly EntityNamingLogic expectedNamingLogic;
        private readonly GenericLabelBlock checkedLabel;
        private readonly string message;
        private GenericLabelBlock expectedLabel;
        private MessageBlock expectedBlock;
        private MessageBlock checkedBlock;
        private Type checkedType;
        private string customAddOn;
        private readonly bool dontRepeatExpected;
        private readonly bool dontRepeatChecked;

        private static readonly Regex NormalOrder = new Regex(".*\\{0\\}.*\\{1\\}.*", RegexOptions.Compiled);
        private static readonly Regex ReverseOrder = new Regex(".*\\{1\\}.*\\{0\\}.*", RegexOptions.Compiled);

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
            var format = message;
            format = format.Replace("{checked}", "{0}");
            format = format.Replace("{expected}", "{1}");
            format = format.Replace("{given}", "{1}");
            this.message = format;
            this.dontRepeatExpected = NormalOrder.IsMatch(format);
            this.dontRepeatChecked = ReverseOrder.IsMatch(format);

            this.checkedNamingLogic = new EntityNamingLogic();
            this.expectedNamingLogic = new EntityNamingLogic();
            this.checkedLabel = GenericLabelBlock.BuildCheckedBlock(this.checkedNamingLogic);
            this.expectedLabel = GenericLabelBlock.BuildExpectedBlock(this.expectedNamingLogic);
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

            var same = this.expectedLabel.EntityName() == this.checkedLabel.EntityName();
            var localLabel = (same && this.dontRepeatChecked) ? this.checkedLabel.CustomMessage("{0} one") : this.checkedLabel.ToString();
            var givenOrExpectedLabel = (same && this.dontRepeatExpected) ? this.expectedLabel.CustomMessage("{0} one") : this.expectedLabel.ToString();

            builder.AppendFormat(this.message, localLabel, givenOrExpectedLabel);

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
        public FluentMessage For(EntityNamingLogic newEntityDescription)
        {
            this.checkedNamingLogic.Merge(newEntityDescription);
            this.expectedNamingLogic.Merge(newEntityDescription);
            return this;
        }

        /// <summary>
        /// Specifies the type of entities.
        /// </summary>
        /// <param name="forcedType">The type of the Entity.</param>
        /// <returns>The same fluent message.</returns>
        public FluentMessage For(Type forcedType)
        {
            this.expectedNamingLogic.EntityType = forcedType;
            this.checkedNamingLogic.EntityType = this.checkedType ?? forcedType;
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
        public MessageBlock On<T>(T test, long? index = null)
        {
            var theType = test.GetTypeWithoutThrowingException();
            this.checkedBlock = MessageBlock.Build(this, test, this.checkedLabel, index ?? EnumerableExtensions.NullIndex);
            this.For(theType);
            this.checkedType = theType;
            return this.checkedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected result.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="index"></param>
        /// <returns>
        /// The created MessageBlock.
        /// </returns>
        public MessageBlock Expected<T>(T expected, long? index = null)
        {
            this.expectedBlock = MessageBlock.Build(this, expected, this.expectedLabel, index ?? EnumerableExtensions.NullIndex);
            this.For(expected.GetTypeWithoutThrowingException());
            return this.expectedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the expected result.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// The created MessageBlock.
        /// </returns>
        public MessageBlock ReferenceValues<T>(T expected)
        {
            this.expectedBlock = MessageBlock.Build(this, expected, this.expectedLabel, EnumerableExtensions.NullIndex);
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
        public MessageBlock ExpectedValues<T>(T expectedValues, long? index = null)
        {
            this.expectedNamingLogic.SetPlural();
            this.expectedNamingLogic.EntityType = null;
            this.expectedLabel = GenericLabelBlock.BuildExpectedBlock(this.expectedNamingLogic);
            this.expectedBlock = MessageBlock.Build(this, expectedValues, this.expectedLabel, index ?? EnumerableExtensions.NullIndex, true);
            return this.expectedBlock;
        }

        /// <summary>
        /// Adds a message block to describe the given value (usually used as an 
        /// alternative to the Expected block).
        /// </summary>
        /// <param name="givenValue">The given value.</param>
        /// <param name="index">
        /// The index to highlight.
        /// </param>
        /// <returns>The created MessageBlock.</returns>
        public MessageBlock WithGivenValue<T>(T givenValue, long? index = null)
        {
            this.expectedLabel = GenericLabelBlock.BuildGivenBlock(this.expectedNamingLogic);
            this.expectedBlock = MessageBlock.Build(this, givenValue, this.expectedLabel, index ?? EnumerableExtensions.NullIndex);
            return this.expectedBlock;
        }
        
        /// <summary>
        /// Adds a fully custom add on error message (first line).
        /// </summary>
        /// <param name="customAddOnMessage"></param>
        public FluentMessage AddCustomMessage(string customAddOnMessage)
        {
            this.customAddOn = customAddOnMessage;
            return this;
        }
    }
}