// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="MessageBuilderTests.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
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

    using NFluent.Extensions;
    using NFluent.Helpers;

    using NUnit.Framework;

    [TestFixture]
    public class MessageBuilderTests
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
            Assert.AreEqual("Dictionary<string, string>", typeof(Dictionary<string, string>).ToStringProperlyFormated());
        }
    }
}
