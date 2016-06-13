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

        // issue #119: need to propose various behavior for HasFieldsWithSameValues
        public class TestMe
        {
            public int Id { get; set; }
            public List<int> OtherIds;
        }

          
            [Test]
            public void TestPrivateField()
            {
                TestMe test1 = CreateTestItem();
                TestMe test2 = CreateTestItem();

                var stream = new MemoryStream();
                Serialize(stream, test1);
                stream.Position = 0;

                Deserialize(stream, test1);

                // Id is not checked at all and the List<int>._version field is tested when it should not be
                //this check fails
//                Check.That(test1).HasFieldsWithSameValues(test2);
            }

            private static void Deserialize(MemoryStream stream, TestMe testItem)
            {
                var binaryReader = new BinaryReader(stream);
                testItem.Id = binaryReader.ReadInt32();
                var length = binaryReader.ReadInt32();
                testItem.OtherIds.Clear();
                for (int i = 0; i < length; i++)
                {
                    testItem.OtherIds.Add(binaryReader.ReadInt32());
                }
            }

            private static void Serialize(MemoryStream stream, TestMe testItem)
            {
                var binaryWriter = new BinaryWriter(stream);
                binaryWriter.Write(testItem.Id);
                binaryWriter.Write(testItem.OtherIds.Count);
                foreach (var otherId in testItem.OtherIds)
                {
                    binaryWriter.Write(otherId);
                }
            }

            private static TestMe CreateTestItem()
            {
                var testItem = new TestMe();
                testItem.Id = 42;
                testItem.OtherIds = new List<int> {1, 2, 3, 4};
                return testItem;
            }


        // issue #154: NFluent does not use the overloaded operators
        [Test]
        public void FailsToUseOperator()
        {

            Person mySelf = new Person() { Name = "SilNak" };
            PersonEx myClone = new PersonEx() { Name = "SilNak" };

            //Check.That(myClone).IsEqualTo(mySelf); 
            //Check.That(mySelf).IsEqualTo(myClone); 
        }
        internal class Person
        {
            public String Name { get; set; }
            public String Surname { get; set; }

        }
        internal class PersonEx
        {
            public String Name { get; set; }
            public String Surname { get; set; }

            public static bool operator ==(PersonEx person1, Person person2)
            {
                return person1.Name == person2.Name;
            }
            public static bool operator !=(PersonEx person1, Person person2)
            {
                return person1.Name != person2.Name;
            }
            public static bool operator ==(Person person1, PersonEx person2)
            {
                return person1.Name == person2.Name;
            }
            public static bool operator !=(Person person1, PersonEx person2)
            {
                return person1.Name != person2.Name;
            }
        }
        // issue #124: Improve ContainsExactly error messages
        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain exactly the expected value(s). First difference is at index #4.\nThe checked enumerable:\n\t[\"+5 Dexterity Vest\", \"Aged Brie\", \"Elixir of the Mongoose\", \"Sulfuras, Hand of Ragnaros\", \"Backstagex passes to a TAFKAL80ETC concert\", \"Conjured Mana Cake\"] (6 items)\nThe expected value(s):\n\t[\"+5 Dexterity Vest\", \"Aged Brie\", \"Elixir of the Mongoose\", \"Sulfuras, Hand of Ragnaros\", \"Backstagex passes to a TAFKAL80ETC concer\", \"Conjured Mana Cake\"] (6 items)")]
         public void ContainsExactly()
        {
            var stringArray = new string[]
            {
                "+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros",
                "Backstagex passes to a TAFKAL80ETC concert", "Conjured Mana Cake"
            };

            Check.That(stringArray).ContainsExactly(new string [] { "+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros",
                "Backstagex passes to a TAFKAL80ETC concer", "Conjured Mana Cake"});
        }

        [Test]
        public void ImproveCrossTypeCheckingForNumerals()
        {
            ushort usValue = 2; int iValue = 1;
            Check.That(usValue).IsEqualTo(iValue + 1);
        }

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
        [ExpectedException]
        public void Test1()
        {
            Impl impl = new Impl { BaseProperty = "Any", ImplProperty = "Any1" };

            Check.That(impl).HasFieldsWithSameValues(new { BaseProperty = "Any", ImplProperty = "Any2" });
        }

        [Test]
        [ExpectedException]
        public void Test2()
        {
            Impl impl = new Impl { BaseProperty = "Any1", ImplProperty = "Any" };

            Check.That(impl).HasFieldsWithSameValues(new { BaseProperty = "Any2", ImplProperty = "Any" });
        }

        [Test]
        [ExpectedException]
        public void Test3()
        {
            Impl impl = new Impl { BaseProperty = "Any1", ImplProperty = "Any" };
            Impl impl2 = new Impl { BaseProperty = "Any2", ImplProperty = "Any" };

            Check.That(impl).HasFieldsWithSameValues(impl2);
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
        [ExpectedException(typeof(FluentCheckException))]
        public void NullRefonHasFieldsWithSameValueWithInterfaces()
        {
            var modelA = new ModelA { Name = "Yoda" };
            var modelB = new ModelB { Name = new ModelBName { Title = "Frank" } };

            Check.That(modelA).HasFieldsWithSameValues(modelB);
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked enumerable does not contain the expected value(s):\n\t[\"MaChaine{964}\"]\nThe checked enumerable:\n\t[\"MaChaine{94}\"]\nThe expected value(s):\n\t[\"MaChaine{964}\"]")]
        public void SpuriousExceptionOnError()
        {
            var toTest = new System.Collections.ArrayList { "MaChaine{94}" };
            const string Result = "MaChaine{964}";
            Check.That(toTest).Contains(Result);
        }

        // #issue 115,
        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's field 'Price' does not have the expected value.\nThe checked value:\n\t[100] of type: [decimal]\nThe expected value:\n\t[100] of type: [int]")]
        public void FailingTestForDemo()
        {
            var args = new OrderExecutedEventArgs(100M, 150, Way.Sell);

            Check.That(args).HasFieldsWithSameValues(new { Price = 100, Quantity = 150, Way = Way.Sell });
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
            var checkString = File.ReadAllBytes("CheckedFile.xml");
            var expectedString = File.ReadAllBytes("ExpectedFile.xml");
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