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

namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;

    using NFluent.Extensibility;
    using NFluent.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class FluentMessageTests
    {
        [Test]
        public void BasicTest()
        {
            var message = FluentMessage.BuildMessage("The {0} is ok.").ToString();

            Assert.AreEqual("\nThe checked value is ok.", message);

            // override entity
            message = FluentMessage.BuildMessage("The {0} is ok.").For("string").ToString();
            Assert.AreEqual("\nThe checked string is ok.", message);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BlockFailTest()
        {
            var block = new FluentMessage.MessageBlock(null, null, string.Empty);
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
            var message = new FluentMessage("test");
            var x = 4;
            var block = new FluentMessage.MessageBlock(message, x, string.Empty);

            Assert.AreEqual("The  value:\n\t[4]", block.GetMessage());

            block.WithHashCode().WithType();

            Assert.AreEqual("The  value:\n\t[4] of type: [int] with HashCode: [4]", block.GetMessage());
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

            Assert.AreEqual("\nThe checked date time is before the given date time whereas it must not.\nThe checked date time:\n\t[\"portna\"]\nThe given date time:\n\t[\"ouaq\"]", message);
        }

        [Test]
        public void HowExpectedValuesWorks()
        {
            var heroes = new[] { "Luke", "Yoda", "Chewie" };
            var givenValues = new[] { "Luke", "Yoda", "Chewie", "Vader" };
            
            var message = FluentMessage.BuildMessage("The {0} does not contain exactly the {1}.")
                                            .For("enumerable")
                                            .On(heroes)
                                            .WithEnumerableCount(heroes.Count())
                                            .And.ExpectedValues(givenValues)
                                            .WithEnumerableCount(givenValues.Count())
                                            .ToString();

            Assert.AreEqual("\nThe checked enumerable does not contain exactly the expected value(s).\nThe checked enumerable:\n\t[\"Luke\", \"Yoda\", \"Chewie\"] (3 items)\nThe expected value(s):\n\t[\"Luke\", \"Yoda\", \"Chewie\", \"Vader\"] (4 items)", message);
        }

        [Test]
        public void WeCanConfigureTheExpectedLabel()
        {
            var possibleElements = new string[] { "Paco de Lucia", "Jimi Hendrix", "Baden Powell" };
            var checkedValue = "The Black Keys";

            var errorMessage = FluentMessage.BuildMessage("The {0} is not one of the possible elements.")
                                            .On(checkedValue)
                                            .And.Expected(possibleElements).Label("The possible elements:")
                                            .ToString();

            Assert.AreEqual("\nThe checked value is not one of the possible elements.\nThe checked value:\n\t[\"The Black Keys\"]\nThe possible elements:\n\t[\"Paco de Lucia\", \"Jimi Hendrix\", \"Baden Powell\"]", errorMessage);
        }

        [Test]
        public void WorksWithChar()
        {
            const char LowerCasedA = 'a';

            var message = FluentMessage.BuildMessage("The {0} is properly displayed.")
                                            .For("char")
                                            .On(LowerCasedA)
                                            .ToString();

            Assert.AreEqual("\nThe checked char is properly displayed.\nThe checked char:\n\t['a']", message);
        }

        [Test]
        public void WorksWithPunctuationChar()
        {
            const char SlashChar = '/';

            var message = FluentMessage.BuildMessage("The {0} is properly displayed.")
                                            .For("char")
                                            .On(SlashChar)
                                            .ToString();

            Assert.AreEqual("\nThe checked char is properly displayed.\nThe checked char:\n\t['/']", message);
        }

        [Test]
        public void DoubleCurlyBracesWorks()
        {
            var parameter = "string{45}";

            Assert.AreEqual("string{{45}}", parameter.DoubleCurlyBraces());
        }
    }
}
