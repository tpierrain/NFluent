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
    using Messages;
    using NUnit.Framework;

    [TestFixture]
    public class ValueBlockTests
    {
        [Test]
        public void ShouldWorkForBasicValue()
        {
            var blk = new ValueBlock<int>(2);
            Assert.That(blk.GetMessage(), Is.EqualTo("[2]"));

            blk.WithType();
            Assert.That(blk.GetMessage(), Is.EqualTo("[2] of type: [int]"));
        }

        [Test]
        public void ShouldWorkForEnumeration()
        {
            var list = new []{ "a", "b", "c" };
            var blk = new ValueBlock<string[]>(list);

            Assert.That(blk.GetMessage(), Is.EqualTo("[{\"a\",\"b\",\"c\"}]"));

            Assert.Throws<InvalidOperationException>(() => blk.WithEnumerableCount(list.GetLength(0)));
        }

        [Test]
        public void ShouldWorkForMatrices()
        {
            var matrix = new[,] {{1, 2, 3}, {4, 5, 6}};
            var blk = new ValueBlock<int[,]>(matrix);
            Assert.That(blk.GetMessage(), Is.EqualTo("[{{1,2,3},{4,5,6}}]"));
        }


        [Test]
        public void ShouldTruncateLongEnumeration()
        {
            var list = "this is a long string to be converted to a byte array".ToCharArray();
            var blk = new EnumerationBlock<char[]>(list, 0);

            blk.WithEnumerableCount(list.GetLength(0));
            Assert.That(blk.GetMessage(), Is.EqualTo("{*\'t\'*,\'h\',\'i\',\'s\',\' \',\'i\',\'s\',\' \',\'a\',\' \',\'l\',\'o\',\'n\',\'g\',\' \',\'s\',\'t\',\'r\',\'i\',\'n\',...} (53 items)"));
        }
       
        [Test]
        public void ShouldWorkForEdgeEnumerations()
        {
            var list = string.Empty.ToCharArray();
            var blk = new EnumerationBlock<char[]>(list, 0);

            blk.WithEnumerableCount(list.GetLength(0));
            Assert.That(blk.GetMessage(), Is.EqualTo("{} (0 item)"));
        }

        [Test]
        public void ShouldFocusOnSomePart()
        {
            var list = "this is a long string to be converted to a byte array".ToCharArray();
            var blk = new EnumerationBlock<char[]>(list, 15);

            blk.WithEnumerableCount(list.GetLength(0));
            Assert.That(blk.GetMessage(), Is.EqualTo("{...,\'i\',\'s\',\' \',\'a\',\' \',\'l\',\'o\',\'n\',\'g\',\' \',*\'s\'*,\'t\',\'r\',\'i\',\'n\',\'g\',\' \',\'t\',\'o\',\' \',...} (53 items)"));
        }
    }
}
