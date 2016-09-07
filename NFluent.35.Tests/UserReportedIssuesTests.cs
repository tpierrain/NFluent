// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="UserReportedIssuesTests.cs" company="">
// //   Copyright 2014 Cyrille DUPUYDAUBY
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
    using System.IO;
    using System.Text;

    using NUnit.Framework;

    [TestFixture]
    public class UserReportedIssuesTests
    {

        // Issue #148: object cycle should work with hasfieldswithsamevalue
        [Test]
        public void LoopTest()
        {
            var node = new Node();

            Check.That(node).HasFieldsWithSameValues(new Node());
        }


        // Issue #141: issue with private inheritance and 'hasfieldswithsamevalue'
        class Base
        {
            public string BaseProperty { get; set; }
        }

        class Impl : Base
        {

            public string ImplProperty { get; set; }
        }

        [Test]
        public void Test1()
        {
            Check.ThatCode(() =>
            {
                Impl impl = new Impl { BaseProperty = "Any", ImplProperty = "Any1" };
                Check.That(impl).HasFieldsWithSameValues(new { BaseProperty = "Any", ImplProperty = "Any2" });
            })
            .ThrowsAny();
        }

        [Test]
        public void Test2()
        {
            Check.ThatCode(() =>
            {
                Impl impl = new Impl { BaseProperty = "Any1", ImplProperty = "Any" };
                Check.That(impl).HasFieldsWithSameValues(new { BaseProperty = "Any2", ImplProperty = "Any" });
            })
            .ThrowsAny();
        }

        [Test]
        public void Test3()
        {
            Check.ThatCode(() =>
            {
                Impl impl = new Impl { BaseProperty = "Any1", ImplProperty = "Any" };
                Impl impl2 = new Impl { BaseProperty = "Any2", ImplProperty = "Any" };

                Check.That(impl).HasFieldsWithSameValues(impl2);
            })
            .ThrowsAny();
        }

        // Issue #138: superfluous casting required for mathematical expression
        [Test]
        public void CastingForExpression()
        {
            ushort usValue = 0;
            Check.That(usValue).IsEqualTo(0);
        }


        // Issue #135: superfluous casting required
        [Test]
        public void Casting()
        {
            ushort usValue = 0;
            Check.That(usValue).IsEqualTo(0);
        }


        // Issue #131: Pull Request to add First() and Single() for enumeration
        [Test]
        public void CheckForFIrst()
        {
            var enumerable = new List<int> { 42, 43 };
            Check.That(enumerable).Contains(42, 43).InThatOrder();
        }

        [Test]
        public void NullRefExcOnEnumerables()
        {
            var values = new[] { "Yoda", null };
            Check.That(values).Contains(null);
        }

        [Test]
        public void NullRefonHasFieldsWithSameValueWithInterfaces()
        {
            
            Check.ThatCode(() =>
            {
                var modelA = new ModelA { Name = "Yoda" };
                var modelB = new ModelB { Name = new ModelBName { Title = "Frank" } };

                Check.That(modelA).HasFieldsWithSameValues(modelB);
            })
            .Throws<FluentCheckException>();
        }

        // Issue #111
        [Test]
        public void IllogicSyntaxWithAndForList()
        {
            var test = new List<string>();
            Check.That(test).IsNotNull().And.HasSize(0);
        }

        // 30/05/14 Invalid exception on strings with curly braces
        [Test]
        public void SpuriousExceptionOnError()
        {
            Check.ThatCode(() =>
            {
                var toTest = new System.Collections.ArrayList { "MaChaine{94}" };
                const string Result = "MaChaine{964}";
                Check.That(toTest).Contains(Result);
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked enumerable does not contain the expected value(s):\n\t[\"MaChaine{964}\"]\nThe checked enumerable:\n\t[\"MaChaine{94}\"]\nThe expected value(s):\n\t[\"MaChaine{964}\"]");
        }

        // #issue 115,
        [Test]
        public void FailingTestForDemo()
        {
            Check.ThatCode(() =>
            {
                var args = new OrderExecutedEventArgs(100M, 150, Way.Sell);

                Check.That(args).HasFieldsWithSameValues(new { Price = 100, Quantity = 150, Way = Way.Sell });
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked value's field 'Price' does not have the expected value.\nThe checked value:\n\t[100] of type: [decimal]\nThe expected value:\n\t[100] of type: [int]");
        }

        // issue #127, request for byte array support
        // actual issues unclear as it just works.
        [Test]
        public void CheckForSupportOfByteArrays()
        {
            var coder = new ASCIIEncoding();

            var sut = coder.GetBytes("test");
            var expected = coder.GetBytes("test");

            Check.That(sut).ContainsExactly(expected);
        }

        [Test]
        public void LongStringErrorMessageIsProperlyTruncated()
        {
            var checkString = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\CheckedFile.xml");
            var expectedString = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "\\ExpectedFile.xml");
// TODO: implement support for LONG enumeration
//            Check.That(checkString).IsEqualTo(expectedString);
        }

        // helper classes for issue reproduction
        public interface IModelBName
        {
            string Title { get; set; }
        }

        private class OrderExecutedEventArgs : EventArgs
        {
            public decimal Price { get; private set; }

            public int Quantity { get; private set; }

            public Way Way { get; private set; }

            public OrderExecutedEventArgs(decimal price, int quantity, Way way)
            {
                this.Price = price;
                this.Quantity = quantity;
                this.Way = way;
            }
        }

        public enum Way
        {
            Sell,
            Buy
        }

        private class ModelA
        {
            public string Name { get; set; }
        }

        private class ModelB
        {
            public IModelBName Name { get; set; }
        }

        private class ModelBName : IModelBName
        {
            public string Title { get; set; }
        }

        private class Node
        {
            private Node loop;

            public Node()
            {
                this.loop = this;
            }
        }
    }
}