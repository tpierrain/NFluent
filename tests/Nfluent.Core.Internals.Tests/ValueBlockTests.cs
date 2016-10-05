//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ValueBlockTests.cs" company="">
//    Copyright 2014 Cyrille DUPUYDAUBY
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Tests
{
    using System;

    using Extensibility;
    using Messages;
    using NUnit.Framework;

    [TestFixture]
    public class ValueBlockTests
    {
        [Test]
        public void ShouldWorkForBasicValue()
        {
            var blk = new ValueBlock(2);
            Assert.AreEqual("[2]", blk.GetMessage());

            blk.WithType();
            Assert.AreEqual("[2] of type: [int]", blk.GetMessage());
        }

        [Test]
        public void ShouldWorkForInstance()
        {
            var blk = new InstanceBlock(typeof(string));

            Assert.AreEqual("an instance of type: [string]", blk.GetMessage());
        }

        [Test]
        public void ShouldWorkForEnumeration()
        {
            var list = new []{ "a", "b", "c" };
            var blk = new ValueBlock(list);

            Assert.AreEqual("[\"a\", \"b\", \"c\"]", blk.GetMessage());

            blk.WithEnumerableCount(list.GetLength(0));
            Assert.AreEqual("[\"a\", \"b\", \"c\"] (3 items)", blk.GetMessage());
        }

        [Test]
        public void ShouldTruncateLongEnumeration()
        {
            var list = "this is a long string to be converted to a byte array".ToCharArray();
            var blk = new EnumerationBlock(list, 0);

            blk.WithEnumerableCount(list.GetLength(0));
            Assert.AreEqual("[\'t\', \'h\', \'i\', \'s\', \' \', \'i\', \'s\', \' \', \'a\', \' \', \'l\', \'o\', \'n\', \'g\', \' \', \'s\', \'t\', \'r\', \'i\', ...] (53 items)", blk.GetMessage());
        }
       
        [Test]
        public void ShouldForEdgeEnumerations()
        {
            var list = string.Empty.ToCharArray();
            var blk = new EnumerationBlock(list, 0);

            blk.WithEnumerableCount(list.GetLength(0));
            Assert.AreEqual("[] (0 item)", blk.GetMessage());
            blk = new EnumerationBlock(null, 0);

            blk.WithEnumerableCount(list.GetLength(0));
            Assert.AreEqual("[null]", blk.GetMessage());
        }

        [Test]
        public void ShoulSupportWithType()
        {
            var list = string.Empty.ToCharArray();
            var blk = new EnumerationBlock(list, 0);

            blk.WithEnumerableCount(list.GetLength(0));
            blk.WithType();
            Assert.AreEqual("[] (0 item) of type: [char[]]", blk.GetMessage());
            blk.WithType(typeof(string));
            Assert.AreEqual("[] (0 item) of type: [string]", blk.GetMessage());
        }

        [Test]
        public void ShouldFocusOnSomePart()
        {
            var list = "this is a long string to be converted to a byte array".ToCharArray();
            var blk = new EnumerationBlock(list, 15);

            blk.WithEnumerableCount(list.GetLength(0));
            Assert.AreEqual("[..., \'i\', \'s\', \' \', \'a\', \' \', \'l\', \'o\', \'n\', \'g\', \' \', \'s\', \'t\', \'r\', \'i\', \'n\', \'g\', \' \', \'t\', \'o\', ...] (53 items)", blk.GetMessage());
        }

        [Test]
        public void InstanceBlockShouldThrowOnEnumeration()
        {
            Check.ThatCode(() =>
            {
                var blk = new InstanceBlock(typeof(string));

                blk.WithEnumerableCount(0);
            })
            .Throws<NotSupportedException>();
        }

        [Test]
        public void InstanceBlockShouldThrowOnhashcode()
        {
            Check.ThatCode(() =>
            {
                var blk = new InstanceBlock(typeof(string));

                blk.WithHashCode();
            })
            .Throws<NotSupportedException>();
        }

        [Test]
        public void InstanceBlockShouldThrowOnForceType()
        {
            Check.ThatCode(() =>
            {
                var blk = new InstanceBlock(typeof(string));

                blk.WithType(typeof(string));
            })
            .Throws<NotSupportedException>();
        }
    }
}
