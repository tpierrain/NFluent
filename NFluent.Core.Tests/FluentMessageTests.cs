// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FluentMessageTests.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System.CodeDom;

namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;

    using Extensibility;
    using Messages;
    using NFluent.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class FluentMessageTests
    {
        [Test]
        public void BasicTest()
        {
            var message = FluentMessage.BuildMessage("The {0} is ok.").ToString();

            Assert.AreEqual(Environment.NewLine+ "The checked value is ok.", message);

            // override entity
            message = FluentMessage.BuildMessage("The {0} is ok.").For("string").ToString();
            Assert.AreEqual(Environment.NewLine+ "The checked string is ok.", message);
        }
        
        [Test]
        public void BlockFailTest()
        {
            Check.ThatCode(() =>
            {
                var block = new MessageBlock(null, null, null);
            })
            .Throws<ArgumentNullException>();
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
            const int X = 4;
            var block = new MessageBlock(message, X, new GenericLabelBlock());

            Assert.AreEqual("The  value:" + Environment.NewLine + "\t[4]", block.GetMessage());

            block.WithHashCode().WithType();

            Assert.AreEqual("The  value:" + Environment.NewLine + "\t[4] of type: [int] with HashCode: [4]", block.GetMessage());
        }

        [Test]
        public void ToStringProperlyFormatedCoverageTests()
        {
            Assert.AreEqual("char", typeof(char).ToStringProperlyFormated());
            Assert.AreEqual("void", typeof(void).ToStringProperlyFormated());
            Assert.AreEqual("System.Collections.Generic.Dictionary<string, string>", typeof(Dictionary<string, string>).ToStringProperlyFormated());
            Assert.AreEqual("Dictionary<string, string>", typeof(Dictionary<string, string>).TypeToStringProperlyFormated(true));
            Assert.AreEqual("int?", typeof(int?).TypeToStringProperlyFormated(true));
        }

        [Test]
        public void HowGivenValueWorks()
        {
            var message = FluentMessage.BuildMessage("The {0} is before the {1} whereas it must not.")
                                            .For("date time")
                                            .On("portna")
                                            .And.WithGivenValue("ouaq").ToString();

            Assert.AreEqual(Environment.NewLine+ "The checked date time is before the given one whereas it must not." + Environment.NewLine + "The checked date time:" + Environment.NewLine + "\t[\"portna\"]" + Environment.NewLine + "The given date time:" + Environment.NewLine + "\t[\"ouaq\"]", message);
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

            Assert.AreEqual(Environment.NewLine+ "The checked enumerable does not contain exactly the expected value(s)." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t[\"Luke\", \"Yoda\", \"Chewie\"] (3 items)" + Environment.NewLine + "The expected value(s):" + Environment.NewLine + "\t[\"Luke\", \"Yoda\", \"Chewie\", \"Vader\"] (4 items)", message);
        }

        [Test]
        public void WeCanConfigureTheExpectedLabel()
        {
            var possibleElements = new[] { "Paco de Lucia", "Jimi Hendrix", "Baden Powell" };
            const string CheckedValue = "The Black Keys";

            var errorMessage = FluentMessage.BuildMessage("The {0} is not one of the possible elements.")
                                            .On(CheckedValue)
                                            .And.ReferenceValues(possibleElements).Label("The possible elements:")
                                            .ToString();

            Assert.AreEqual(Environment.NewLine+ "The checked string is not one of the possible elements." + Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"The Black Keys\"]" + Environment.NewLine + "The possible elements:" + Environment.NewLine + "\t[\"Paco de Lucia\", \"Jimi Hendrix\", \"Baden Powell\"]", errorMessage);
        }

        [Test]
        public void WorksWithChar()
        {
            const char LowerCasedA = 'a';

            var message = FluentMessage.BuildMessage("The {0} is properly displayed.")
                                            .On(LowerCasedA)
                                            .ToString();

            Assert.AreEqual(Environment.NewLine+ "The checked char is properly displayed." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['a']", message);
        }

        [Test]
        public void WorksWithPunctuationChar()
        {
            const char SlashChar = '/';

            var message = FluentMessage.BuildMessage("The {0} is properly displayed.")
                                            .On(SlashChar)
                                            .ToString();

            Assert.AreEqual(Environment.NewLine+ "The checked char is properly displayed." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['/']", message);
        }

        [Test]
        public void DoubleCurlyBracesWorks()
        {
            const string Parameter = "string{45}";

            Assert.AreEqual("string{{45}}", Parameter.DoubleCurlyBraces());
        }

        [Test]
        public void ShouldCreateExpectedLabel()
        {
            var label = GenericLabelBlock.BuildExpectedBlock(null);

            Assert.AreEqual("The expected value:", label.CustomMessage(null));
        }
    
        [Test]
        public void ShouldCreateActualLabel()
        {
            var label = GenericLabelBlock.BuildActualBlock(null);

            Assert.AreEqual("The actual value:", label.CustomMessage(null));
        }


        [Test]
        public void InstanceValuesMustGenerateProperText()
        {
            var errorMessage = FluentMessage.BuildMessage("don't care").ExpectedType(typeof(string)).ToString();
            Assert.AreEqual(Environment.NewLine+ "don't care" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\tan instance of type: [string]", errorMessage);
        }

        [Test]
        public void InstanceValuesMustNotSupportEnumerationFeatures()
        {
            Check.ThatCode(()=>
            {
                var errorMessage =
                    FluentMessage.BuildMessage("don't care").ExpectedType(typeof(string)).WithEnumerableCount(0);
            }).Throws<NotSupportedException>();
        }

        [Test]
        public void InstanceValuesMustNotSupportHashCodes()
        {
            Check.ThatCode(() =>
            {
                var errorMessage = FluentMessage.BuildMessage("don't care").ExpectedType(typeof(string)).WithHashCode();
            }).Throws<NotSupportedException>();
        }

        [Test]
        public void InstanceValuesMustNotSupportWithType()
        {
            Check.ThatCode(() =>
            {
                var errorMessage = FluentMessage.BuildMessage("don't care").ExpectedType(typeof(string)).OfType(typeof(int));
            }).Throws<NotSupportedException>();
        }

        [Test]
        public void ShouldBlockWorksOnLongEnumeration()
        {
            var possibleElements = "We need to test the message block methods with a long enumeration. A string convterted to a char array should be enough.";
            const string CheckedValue = "The Black Keys";

            var errorMessage = FluentMessage.BuildMessage("The {0} is not one of the possible elements.")
                                            .On(CheckedValue.ToCharArray())
                                            .And.ReferenceValues(possibleElements.ToCharArray()).Label("The possible elements:")
                                            .ToString();

//            Assert.AreEqual(Environment.NewLine+ "The checked enumerable is not one of the possible elements." + Environment.NewLine + "The checked enumrable:" + Environment.NewLine + "\t[\"The Black Keys\"]" + Environment.NewLine + "The possible elements:" + Environment.NewLine + "\t[\"Paco de Lucia\", \"Jimi Hendrix\", \"Baden Powell\"]", errorMessage);
        }
    }
}
