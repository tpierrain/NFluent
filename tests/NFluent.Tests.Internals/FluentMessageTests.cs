 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="FluentMessageTests.cs" company="">
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
namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;

    using Extensibility;

    using Extensions;

    using Messages;

    using NUnit.Framework;

    [TestFixture]
    public class FluentMessageTests
    {
        private static readonly string NewLine = Environment.NewLine;

        [Test]
        public void BasicTest()
        {
            var message = FluentMessage.BuildMessage("The {0} is ok.").ToString();

            Assert.AreEqual(NewLine + "The checked value is ok.", message);

            // override entity
            message = FluentMessage.BuildMessage("The {0} is ok.").For("string").ToString();
            Assert.AreEqual(NewLine + "The checked string is ok.", message);
        }
        
        [Test]
        public void PlaceHolderShouldBeRecognized()
        {
            var message = FluentMessage.BuildMessage("The {checked} is ok.").ToString();

            Assert.AreEqual(NewLine + "The checked value is ok.", message);

            message = FluentMessage.BuildMessage("The {expected} is ok.").ToString();

            Assert.AreEqual(NewLine + "The expected value is ok.", message);

            message = FluentMessage.BuildMessage("The {given} is ok.").ToString();

            Assert.AreEqual(NewLine + "The expected value is ok.", message);
        }

        [Test]
        public void CheckedBlockTest()
        {
            var test = DateTime.Today;
            var message = FluentMessage.BuildMessage("The {0} is below.").On(test).ToString();
            var lines = message.Split('\n');
            Assert.AreEqual(4, lines.Length);
            Assert.IsTrue(lines[1].Contains("checked"));
        }

        [Test]
        public void BlockTest()
        {
            var message = FluentMessage.BuildMessage("test");
            const int x = 4;
            var block = new MessageBlock(message, x, GenericLabelBlock.BuildCheckedBlock(new EntityNamer()));

            Assert.AreEqual("The checked value:" + NewLine + "\t[4]", block.GetMessage());

            block.WithHashCode().WithType();

            Assert.AreEqual("The checked value:" + NewLine + "\t[4] of type: [int] with HashCode: [4]", block.GetMessage());
        }

        [Test]
        public void ToStringProperlyFormattedCoverageTests()
        {
            Assert.AreEqual("char", typeof(char).ToStringProperlyFormatted());
            Assert.AreEqual("void", typeof(void).ToStringProperlyFormatted());
            Assert.AreEqual("System.Collections.Generic.Dictionary<string, string>", typeof(Dictionary<string, string>).ToStringProperlyFormatted());
            Assert.AreEqual("Dictionary<string, string>", typeof(Dictionary<string, string>).TypeToStringProperlyFormatted(true));
            Assert.AreEqual("int?", typeof(int?).TypeToStringProperlyFormatted(true));
        }

        [Test]
        public void HowGivenValueWorks()
        {
            var message = FluentMessage.BuildMessage("The {0} is before the {1} whereas it must not.")
                                            .For("date time")
                                            .On("portna")
                                            .And.WithGivenValue("ouaq").ToString();

            Assert.AreEqual(NewLine + "The checked date time is before the given one whereas it must not." + NewLine + "The checked date time:" + NewLine + "\t[\"portna\"]" + NewLine + "The expected date time:" + NewLine + "\t[\"ouaq\"]", message);
        }

        [Test]
        public void ShouldPermitChangingMainMessage()
        {
            var message = FluentMessage.BuildMessage("The {0} is before the {1} whereas it must not.");

            message.ChangeMessageTo("The {0} is not before the {1}.");

            Assert.AreEqual(NewLine + "The checked value is not before the expected one.", message.ToString());

        }

        [Test]
        public void HowExpectedValuesWorks()
        {
            var heroes = new[] { "Luke", "Yoda", "Chewie" };
            var givenValues = new[] { "Luke", "Yoda", "Chewie", "Vader" };
            
            var message = FluentMessage.BuildMessage("The {0} does not contain exactly the {1}.")
                                            .On(heroes)
                                            .WithEnumerableCount(heroes.Count())
                                            .And.ExpectedValues(givenValues)
                                            .WithEnumerableCount(givenValues.Count())
                                            .ToString();

            Assert.AreEqual(NewLine+ "The checked enumerable does not contain exactly the expected value(s)." + NewLine + "The checked enumerable:" + NewLine + "\t{\"Luke\", \"Yoda\", \"Chewie\"} (3 items)" + NewLine + "The expected value(s):" + NewLine + "\t{\"Luke\", \"Yoda\", \"Chewie\", \"Vader\"} (4 items)", message);
        }

        [Test]
        public void WeCanConfigureTheExpectedLabel()
        {
            var possibleElements = new[] { "Paco de Lucia", "Jimi Hendrix", "Baden Powell" };
            const string checkedValue = "The Black Keys";

            var errorMessage = FluentMessage.BuildMessage("The {0} is not one of the possible elements.")
                                            .On(checkedValue)
                                            .And.ReferenceValues(possibleElements).Label("The possible elements:")
                                            .ToString();

            Assert.AreEqual(NewLine+ "The checked string is not one of the possible elements." + NewLine + "The checked string:" + NewLine + "\t[\"The Black Keys\"]" + NewLine + "The possible elements:" + NewLine + "\t{\"Paco de Lucia\", \"Jimi Hendrix\", \"Baden Powell\"}", errorMessage);
        }

        [Test]
        public void WorksWithChar()
        {
            const char lowerCasedA = 'a';

            var message = FluentMessage.BuildMessage("The {0} is properly displayed.")
                                            .On(lowerCasedA)
                                            .ToString();

            Assert.AreEqual(NewLine+ "The checked char is properly displayed." + NewLine + "The checked char:" + NewLine + "\t['a']", message);
        }

        [Test]
        public void WorksWithPunctuationChar()
        {
            const char slashChar = '/';

            var message = FluentMessage.BuildMessage("The {0} is properly displayed.")
                                            .On(slashChar)
                                            .ToString();

            Assert.AreEqual(NewLine+ "The checked char is properly displayed." + NewLine + "The checked char:" + NewLine + "\t['/']", message);
        }

        [Test]
        public void DoubleCurlyBracesWorks()
        {
            const string parameter = "string{45}";

            Assert.AreEqual("string{{45}}", parameter.DoubleCurlyBraces());
        }

        [Test]
        public void ShouldCreateExpectedLabel()
        {
            var label = GenericLabelBlock.BuildExpectedBlock(new EntityNamer());

            Assert.AreEqual("The expected value:", label.CustomMessage(null));
        }
    
        [Test]
        public void InstanceValuesMustGenerateProperText()
        {
            var errorMessage = FluentMessage.BuildMessage("don't care").ExpectedType(typeof(string)).ToString();
            Assert.AreEqual(NewLine+ "don't care" + NewLine + "The expected value:" + NewLine + "\tan instance of type: [string]", errorMessage);
        }

        [Test]
        public void InstanceValuesMustNotSupportEnumerationFeatures()
        {
            Check.ThatCode(()=>
            {
                FluentMessage.BuildMessage("don't care").ExpectedType(typeof(string)).WithEnumerableCount(0);
            }).Throws<NotSupportedException>();
        }

        [Test]
        public void InstanceValuesMustNotSupportHashCodes()
        {
            Check.ThatCode(() =>
            {
                FluentMessage.BuildMessage("don't care").ExpectedType(typeof(string)).WithHashCode();
            }).Throws<NotSupportedException>();
        }

        [Test]
        public void InstanceValuesMustNotSupportWithType()
        {
            Check.ThatCode(() =>
            {
                FluentMessage.BuildMessage("don't care").ExpectedType(typeof(string)).OfType(typeof(int));
            }).Throws<NotSupportedException>();
        }

        [Test]
        public void ShouldBlockWorksOnLongEnumeration()
        {
            var possibleElements = "We need to test the message block methods with a long enumeration. A string converted to a char array should be enough.";
            const string checkedValue = "The Black Keys";

            var unused = FluentMessage.BuildMessage("The {0} is not one of the possible elements.")
                                            .On(checkedValue.ToCharArray())
                                            .And.ReferenceValues(possibleElements.ToCharArray()).Label("The possible elements:")
                                            .ToString();

            Assert.AreEqual(@"
The checked enumerable is not one of the possible elements.
The checked enumerable:
	{'T', 'h', 'e', ' ', 'B', 'l', 'a', 'c', 'k', ' ', 'K', 'e', 'y', 's'}
The possible elements:
	{'W', 'e', ' ', 'n', 'e', 'e', 'd', ' ', 't', 'o', ' ', 't', 'e', 's', 't', ' ', 't', 'h', 'e', ' ', ...}", 
                unused);
        }

    }
}
