﻿// --------------------------------------------------------------------------------------------------------------------
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

namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;

    using Extensibility;
    using Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class FluentMessageTests
    {
        [Test]
        public void BasicTest()
        {
            var message = FluentMessage.BuildMessage("The {0} is ok.").ToString();

            Assert.That(message, Is.EqualTo(Environment.NewLine + "The checked value is ok."));

            // override entity
            message = FluentMessage.BuildMessage("The {0} is ok.").For(typeof(string)).ToString();
            Assert.That(message, Is.EqualTo(Environment.NewLine + "The checked string is ok."));
        }
        
 
        [Test]
        public void CheckedBlockTest()
        {
            var test = DateTime.Today;
            var message = FluentMessage.BuildMessage("The {0} is below.").On(test).ToString();
            var lines = message.Split('\n');
            Assert.That(lines.Length, Is.EqualTo(4));
            Assert.That(lines[1].Contains("checked"));
        }

        [Test]
        public void ToStringProperlyFormattedCoverageTests()
        {
            Assert.That(typeof(char).ToStringProperlyFormatted(), Is.EqualTo("char"));
            Assert.That(typeof(void).ToStringProperlyFormatted(), Is.EqualTo("void"));
            Assert.That(typeof(Dictionary<string, string>).ToStringProperlyFormatted(), Is.EqualTo("System.Collections.Generic.Dictionary<string, string>"));
            Assert.That(typeof(Dictionary<string, string>).TypeToStringProperlyFormatted(true), Is.EqualTo("Dictionary<string, string>"));
            Assert.That(typeof(int?).TypeToStringProperlyFormatted(true), Is.EqualTo("int?"));
        }

        [Test]
        public void HowGivenValueWorks()
        {
            var message = FluentMessage.BuildMessage("The {0} is before the {1} whereas it must not.")
                                            .For(new Messages.EntityNamingLogic("date time"))
                                            .On("portna")
                                            .And.WithGivenValue("ouaq").ToString();

            Assert.That(message, Is.EqualTo(Environment.NewLine+
                        "The checked date time is before the given one whereas it must not." + Environment.NewLine + "The checked date time:" + Environment.NewLine + "\t[\"portna\"]" + Environment.NewLine + "The expected date time:" + Environment.NewLine + "\t[\"ouaq\"]"));
        }

        [Test]
        public void HowExpectedValuesWorks()
        {
            var heroes = new[] { "Luke", "Yoda", "Chewie" };
            var givenValues = new[] { "Luke", "Yoda", "Chewie", "Vader" };
            
            var message = FluentMessage.BuildMessage("The {0} does not contain exactly the {1}.")
                                            .On(heroes)
                                            .WithEnumerableCount(heroes.Length)
                                            .And.ExpectedValues(givenValues)
                                            .WithEnumerableCount(givenValues.Length)
                                            .ToString();

            Assert.That(message, Is.EqualTo(Environment.NewLine + "The checked enumerable does not contain exactly the expected value(s)." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + "\t{\"Luke\",\"Yoda\",\"Chewie\"} (3 items)" + Environment.NewLine + "The expected value(s):" + Environment.NewLine + "\t{\"Luke\",\"Yoda\",\"Chewie\",\"Vader\"} (4 items)"));
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

            Assert.That(errorMessage, Is.EqualTo(Environment.NewLine + "The checked string is not one of the possible elements." + Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"The Black Keys\"]" + Environment.NewLine + "The possible elements:" + Environment.NewLine + "\t{\"Paco de Lucia\",\"Jimi Hendrix\",\"Baden Powell\"}"));
        }

        [Test]
        public void WorksWithChar()
        {
            const char lowerCasedA = 'a';

            var message = FluentMessage.BuildMessage("The {0} is properly displayed.")
                                            .On(lowerCasedA)
                                            .ToString();

            Assert.That(message, Is.EqualTo(Environment.NewLine + "The checked char is properly displayed." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['a']"));
        }

        [Test]
        public void WorksWithPunctuationChar()
        {
            const char slashChar = '/';

            var message = FluentMessage.BuildMessage("The {0} is properly displayed.")
                                            .On(slashChar)
                                            .ToString();

            Assert.That(message, Is.EqualTo(Environment.NewLine + "The checked char is properly displayed." + Environment.NewLine + "The checked char:" + Environment.NewLine + "\t['/']"));
        }

        [Test]
        public void DoubleCurlyBracesWorks()
        {
            const string parameter = "string{45}";

            Assert.That(parameter.DoubleCurlyBraces(), Is.EqualTo("string{{45}}"));
        }

        [Test]
        public void ShouldBlockWorksOnLongEnumeration()
        {
            var possibleElements = "We need to test the message block methods with a long enumeration.";
            const string checkedValue = "The Black Keys";

            // ReSharper disable once UnusedVariable
            var errorMessage = FluentMessage.BuildMessage("The {0} is not one of the possible elements.")
                                            .On(checkedValue.ToCharArray())
                                            .And.ReferenceValues(possibleElements.ToCharArray()).Label("The possible elements:")
                                            .ToString();

            Assert.That(errorMessage, Is.EqualTo(Environment.NewLine+ "The checked enumerable is not one of the possible elements." + Environment.NewLine + "The checked enumerable:" + Environment.NewLine + 
                        "\t{'T','h','e',' ','B','l','a','c','k',' ','K','e','y','s'}" + Environment.NewLine + 
                        "The possible elements:" + Environment.NewLine + 
                        "\t{'W','e',' ','n','e','e','d',' ','t','o',' ','t','e','s','t',' ','t','h','e',' ',...}"));
        }
    }
}
