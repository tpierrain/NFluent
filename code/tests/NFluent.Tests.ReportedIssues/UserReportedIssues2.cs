// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="UserReportedIssues2.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
namespace NFluent.Tests.FromIssues
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
#if DOTNET_45
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
#endif
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NFluent.Helpers;
    using NUnit.Framework;
    using NUnit.Framework.Legacy;
    using SutClasses;

    public enum Friends
    {
        Joey = 1,
        Chandler = 2
    }

    [TestFixture]
    public class UserReportedIssues2
    {
        [Test]
        public void Should_not_be_equal_when_order_is_different()
        {
            Check.That(new LinkedList<int>(new [] {3, 1 })).IsEquivalentTo(new LinkedList<int>(new [] {1, 3 }));
            Check.That(new LinkedList<int>(new [] {3, 1 })).IsNotEqualTo(new LinkedList<int>(new [] {1, 3 }));
        }

        [Test]
        public void NonThrowingMethodsReturningNull()
        {
            object ReturnNull() => null;
            ReturnNull(); // does not throw
            Check.ThatCode(() => ReturnNull()).DoesNotThrow();
        }

        [Test]
        public void Check_NaNs()
        {
            //CheckIt(double.NaN); // succeeds 2.7.2, fails 3.0.2 -- 1
            CheckIt((double?)Double.NaN); // 2

            CheckIt2(double.NaN); // 3
            CheckIt2((double?)Double.NaN); // 4

            Check.That(double.NaN.Equals(double.NaN)).IsTrue(); // 5

            //Check.That(double.NaN).IsEqualTo(double.NaN); // fails in both 2.7.2 and 3.0.2 - 6
            Check.That(double.NaN).IsEqualTo<double>(double.NaN); // 7
        
            Check.That((double?)double.NaN).IsEqualTo((double?)double.NaN); // 8
            Check.That((double?)double.NaN).IsEqualTo<double?>(double.NaN); // 9

        }

        private static void CheckIt<T>(T t) => Check.That(t).IsEqualTo(t);

        private static void CheckIt2<T>(T t) => Check.That(t).IsEqualTo<T>(t);

        [Test]
        public void InfiniteShouldBeEqual()
        {
            Check.That(double.PositiveInfinity).IsEqualTo(double.PositiveInfinity);

        }

        [Test]
        public void IsEqualToIssue()
        {
            var json = JsonConvert.SerializeObject(new { data = "testData" });
            
            var obj = JObject.Parse(json);

            Check.That(obj["data"]).Not.IsEqualTo( "Why did it pass?");
            //Check.That(obj["data"]).IsEqualTo(new JObject());
        }
        
        public class MyEntity
        {
    
        }

        public class MyExample
        {
            public object Value { get; set; }
        }
        
        [Test]
        // GH #340
        public void ThrowsOnMacro()
        {


            var value = new MyExample
            {
                Value = new List<MyEntity>()
            };

// Assert
            Check.ThatCode( () =>{
            Check.That(value).IsInstanceOf<MyExample>()
            .Which
                .WhichMember(x => x.Value)
            .Verifies(x =>
            {
                x.IsNull();
            }); }).IsAFailingCheckWithMessage();
        }

        [Test]
        // GH #334
        public void IssueWithDictionaryIsEquivalent()
        {
            var si1 = new Dictionary<string, int> { { "a", 0 }, { "b", 1 }, { "c", 2 } };
            var si2 = new Dictionary<string, int> { { "c", 2 }, { "a", 0 }, { "b", 1 } };
            Check.That(si1).IsEquivalentTo(si2);

            var letterA = "a";
            var letterAnotherWay = new string("a".ToCharArray()); // forces unique instance
            var sinotinterned1 = new Dictionary<string, int> { { letterA, 0 }, { "b", 1 }, { "c", 2 } };
            var sinotinterned2 = new Dictionary<string, int> { { "c", 2 }, { letterAnotherWay, 0 }, { "b", 1 } };
            Check.That(sinotinterned1).IsEquivalentTo(sinotinterned2);

            var is1 = new Dictionary<int, string> { { 1, "va" }, { 2, "vb" }, { 0, "vc" } };
            var is2 = new Dictionary<int, string> { { 0, "vc" }, { 1, "va" }, { 2, "vb" } };
            Check.That(is1).IsEquivalentTo(is2);
        }

        [Test]
        // GH #333
        public void PrecisionIssueWithDouble()
        {
            Check.ThatCode( ()=>
            Check.That(1E-29).IsZero()).IsAFailingCheckWithMessage("", "The checked value is different from zero.", "The checked value:", "	[1E-29]"); // should not succeed! (but does in v2.7.1)
            Check.That(1E-29).IsNotZero(); // should not fail! (but does in v2.7.1)
        }

        [Test]
        // GH #320
        public void IssueWithDoubleAndInt()
        {
            double? number = 2.2;

            Check.ThatCode( () =>
                Check.That(number).Equals(2)).IsAFailingCheck();
            Check.ThatCode( () =>
            Check.That(number).IsEqualTo(2)).IsAFailingCheck(); // OK
        }

        [Test]
        // GH #319
        public void IssueWithTypesAndIsEquivalent()
        {
            Check.That(new byte[] { 0, 1 }).IsEquivalentTo(0, 1); // Fail
            Check.That(new byte[] { 0, 1 }).IsEquivalentTo((byte)0, (byte)1); // OK
            Check.That(new byte[] { 0, 1 }).ContainsExactly(0, 1); // OK
            Check.That(new byte[] { 0, 1 }).ContainsExactly((byte)0, (byte)1); // OK
        }

        [Test]
        // GH #317
        public void ConsideringShouldWorkWithEnumProperties()
        {
            Check.ThatCode( ()=>
            Check.That(new { Friend = Friends.Chandler }).Considering().Public.Properties.IsEqualTo(new { Friend = Friends.Joey }));
        }

        // GH #307
        [Test]
        // TODO: add tests with custom dicos, list of custom dicos...
        public void Test_DictOfDict_IsEquivalentTo()
        {
            var dictOf3_A = new Dictionary<string, string> { { "aa", "AA" }, { "bb", "BB" }, { "cc", "CC" } };
            var dictOf3_B = new Dictionary<string, string> { { "cc", "CC" }, { "aa", "AA" }, { "bb", "BB" } };
            var dictOf2 = new Dictionary<string, string> { { "cc", "CC" }, { "bb", "BB" } };

            var dictOf2Dict_A = new Dictionary<string, Dictionary<string, string>> { { "key1", dictOf2 }, { "key2", dictOf3_A } };
            var dictOf2Dict_B = new Dictionary<string, Dictionary<string, string>> { { "key2", dictOf3_B }, { "key1", dictOf2 } };
            Check.That(dictOf2Dict_A).IsEquivalentTo(dictOf2Dict_B);  
#if !NET35
            var dictOf2Dict_C = new Dictionary<string, IReadOnlyDictionary<string, string>> { { "key1", new RoDico(dictOf2)} , { "key2", new RoDico(dictOf3_B) } };

            Check.That(dictOf2Dict_A).IsEquivalentTo(dictOf2Dict_C); 
            Check.That(dictOf2Dict_C).IsEquivalentTo(dictOf2Dict_B); 

            var customDico = new RoDico(dictOf3_B);
            Check.That(customDico).IsEquivalentTo(dictOf3_B);  
            Check.That(dictOf3_B).IsEquivalentTo(customDico); 
#endif
        }
        
        // GH #313
        [Test]
        public void WithMessageFail()
        {
            Check.ThatCode(() =>
                Check.WithCustomMessage("It's false!").That(false).IsTrue()).IsAFailingCheckWithMessage(
                "It's false!",
                "The checked boolean is false whereas it must be true.", 
                "The checked boolean:", 
                "\t[False]");
        }

        // GH #292
        [Test]
        public void Awkward_behaviour_with_NFluent()
        {
            var toBeChecked = new object[]
            {
                1,
                2,
                3,
                4
            };

            var expected = Enumerable.Range(1, 4);
            
            CollectionAssert.AreEquivalent(expected, toBeChecked);  // OK 
            CollectionAssert.AreEqual(expected, toBeChecked);       // OK
            Check.That(toBeChecked).IsEquivalentTo(expected);      // KO ;-(
        }

        // GH #292
        [Test]
        public void Exception()
        {
            IEnumerable<char> e1 = "test";
            IEnumerable<char> e2 = "test2";

            Check.ThatCode(() =>  Check.That(e1).ContainsExactly(e2)).IsAFailingCheck();
        }

        // GH #290
        [Test]
        public void IsEquivalentToShouldWorkOnDictionaries()
        {
            var dict = new Dictionary<string, object> { ["foo"] = new[] { "bar", "baz" } };
            var expected = new Dictionary<string, object> { ["foo"] = new[] { "bar", "baz" } };
            Check.That(dict).IsEqualTo(expected);
            Check.That(dict).IsEquivalentTo(expected);
#if DOTNET_45
            var value = new ReadOnlyDictionary<string, object>(dict);
            Check.That(value).IsEqualTo(expected);
            Check.That(value).IsEquivalentTo(expected);
#endif
        }

        // GH #286
        [Test]
        public void IsNotZeroFailsForDecimalCloseToZero()
        {
            Check.That(0.12m).IsNotZero();
            Check.That(long.MaxValue).IsNotZero();
        }

        // GH #269 error sor some timespan
        [Test]
        public void IssueWithTimeSpanMinVal()
        {
            Check.That(TimeSpan.MinValue).IsEqualTo(TimeSpan.MinValue); // Ok with NFluent 2.2 but Fails with NFluent 2.3 !
            Check.That(TimeSpan.MinValue).IsLessThan(TimeSpan.MinValue + TimeSpan.FromTicks(1));
            Check.That(TimeSpan.MaxValue).IsGreaterThan(TimeSpan.MaxValue - TimeSpan.FromTicks(1));
        }

        // GH #266 Error using ContainsOnlyElementsThatMatch
        [Test]
        public void IssuesWithEnumerables()
        {
            var integers = new[] { 2, 6 };
// those checks succeed (at least one even entry)
            Check.That(integers).ContainsOnlyElementsThatMatch(x => x % 2 == 0);
        }
        // GH #261 issues with strings with brackets
        [Test]
        public void TestBrackets() {
            const string haystack = "Hello, {LeMonde}";
            const string needle = "{World}";
            Check.ThatCode(()=>
            Check.That(haystack).Contains(needle)).IsAFailingCheck(); // This will intentionally fail
        }

        // GH #258
        [Test]
        public void IsNotNullShouldSupportAs()
        {
            Check.ThatCode(() =>
                Check.That((string) null)
                    .As("foo")
                    .IsNotNull()).IsAFailingCheckWithMessage("", 
                "The checked [foo] must not be null.");
        }

        // GH #257
        [Test]
        public void
            ShouldSupportMultidimensionalArray()
        {
            var array = Array.CreateInstance(typeof(int), new []{2,2}, new []{-1, -1});
            var otherArray = new int[2, 2];
            var val = 0;
            for (var i = 0; i < 2; i++)
            {
                for(var j = 0; j < 2; j++)
                {
                    array.SetValue(val, i-1, j-1);
                    otherArray[i, j] = val;
                    val++;
                }
            }
            var myClass = new  {Property = array};
            Check.That(myClass).HasFieldsWithSameValues(myClass);
            var myOther = new {Property = otherArray};
            myOther.Property[1, 1] = 5;
            Check.ThatCode(() => Check.That(myClass).HasFieldsWithSameValues(myOther)).IsAFailingCheckWithMessage("", 
                "The checked value's field 'Property[1,1]' does not have the expected value.", 
                "The checked value's field 'Property[1,1]':",
                "\t[3]",
                "The expected value's field 'Property[1,1]':",
                "\t[5]");
        }

        class MyType{
            public int[,] Property { get; set; }
        }
        // GH #254
        [Test]
        public void HasElementThatMatchesShouldHandleArrays()
        {
            IEnumerable<string> randomWords = new [] { "yes", "foo", "bar" };
            Check.That(randomWords).HasElementThatMatches((_) => _.StartsWith("ye"));
        }        

        [Test]
        public void ContainsOnlyElementsThatMatchShouldHandleArrays()
        {
            IEnumerable<int> positiveNumbers = new [] { 4, 8, 7 };
            Check.That(positiveNumbers).ContainsOnlyElementsThatMatch((_) => _ >= 0);
        }     

        // GH #143
        [Test]
        public void ThrowShouldCaptureTypeParameters()
        {
            Check.ThatCode(() => throw new MyException<InvalidOperationException>())
                .Throws<MyException<InvalidOperationException>>();
        }

        private class MyException<TU> : Exception where TU : Exception
        {

        }

        // GH #244
        [Test]
        public void Test_Enum_That_Should_Be_In_Error_But_Is_Not()
        {
            var testClass1 = new TestClass
            {
                IntProperty = 10,
                TestEnumProperty = TestEnum.Test2
            };

            var testClass2 = new TestClass
            {
                IntProperty = 10,
                TestEnumProperty = TestEnum.Test3
            };
            Check.ThatCode(() =>
                Check.That(testClass2).HasFieldsWithSameValues(testClass1)).IsAFailingCheck();
        }

        public class TestClass
        {
            public int IntProperty { get; set; }

            public TestEnum TestEnumProperty { get; set; }
        }

        public enum TestEnum
        {
            Test1,
            Test2,
            Test3,
            Test4
        }

        // GH #238
        public class NTest
        {
            private class TestCase
            {
                public List<int> Items { get; set; } = new List<int>();
            }

            [Test]
            public void Test()
            {
                TestCase a = new TestCase();
                TestCase b = new TestCase();

                a.Items.Add(1);
                a.Items.Add(2);
                a.Items.Add(3);

                b.Items.Add(1);
                b.Items.Add(2);
                b.Items.Add(3);

                Check.That(a).Considering().Public.Properties.IsEqualTo(b);
            }
        }


        // GH #231
        [Test]
        public void ShouldNotMixUpCheckedAndExpected()
        {
            double d = 1.0d;
            Check.ThatCode(()=> Check.That(d).IsEqualTo(2.0)).IsAFailingCheckWithMessage(
                "", 
                "The checked value is different from the expected one.", 
                "The checked value:", 
                "\t[1]", 
                "The expected value:", 
                "\t[2]");
        }

        // GH #219
        public class Parent
        {
            public virtual string AutoProperty { get; set; }
        }

        public class Child : Parent
        {
            public override string AutoProperty { get; set; }
        }

        // GH #215
        public class MyClass
        {
            public ISubClass SubClass { get; set; }
        }

        public interface ISubClass
        {
            int MyProperty { get; set; }
        }

        public class SubClass : ISubClass
        {
            public int MyProperty { get; set; }
        }

        private class BaseClass
        {
            public BaseClass(string id)
            {
                this.Id = id;
            }

            public string Id { get; }
        }

        private class SomeClass : BaseClass
        {
            public SomeClass(string id) : base(id)
            {
            }

            public string Salt => Id;
            public string Other { get; set; }
            public Dictionary<string, string> Values { get; set; }
        }

        [Test]
        public void CollectionTest()
        {
            var a = new List<int> {1, 2};
            var b = new List<int> {3, 4};

            // List contains references to a and b
            var list1 = new List<List<int>> {a, b};
            Check.That(list1).ContainsExactly(a, b); // OK
            Check.That(list1).ContainsExactly(new List<List<int>> {a, b}); // OK

            // List contains new instances of lists same as a and b
            var list2 = new List<List<int>>
            {
                new List<int> {1, 2}, // new instance, same as a
                new List<int> {3, 4} // new instance, same as b
            };
            Check.That(list2).ContainsExactly(a, b); // Fail
            Check.That(list2).ContainsExactly(new List<List<int>> {a, b}); // Fail
        }

        // GH #212
        [Test(Description = "Issue #212")]
        public void CollectionWithNumeric()
        {
            var expected = new[] {1, 2};
            var actual = new uint[] {1, 2};
            Check.That(actual).ContainsExactly(expected);
        }

        [Test]
        public void should_not_provide_diff_value_when_at_limit()
        {
            var refValue = 10000;
            var edgeValue = refValue + 0.0001 * refValue;
            Check.ThatCode(() => Check.That((float)edgeValue).IsEqualTo(refValue)).
                IsAFailingCheckWithMessage(	"", 
                    "The checked value is different from the expected one.", 
                    "The checked value:",
                    "\t[10001]",
                    "The expected value:", 
                    "\t[10000]");

            Check.ThatCode(() => Check.That(edgeValue).IsEqualTo(refValue)).
                IsAFailingCheckWithMessage(	"", 
                    "The checked value is different from the expected one.", 
                    "The checked value:",
                    "\t[10001]",
                    "The expected value:", 
                    "\t[10000]");
        }

        [Test]
        public void should_recognize_autoproperty_readonly_values()
        {
            var someClass = new SomeClass("Hello")
            {
                Other = "world",
                Values = new Dictionary<string, string> {["key1"] = "value1"}
            };
            Check.That(someClass).HasFieldsWithSameValues(new
            {
                Other = "world",
                Values = new Dictionary<string, string> {["key1"] = "value1"}
            });
        }

        [Test]
        // GH #226
        public void SupportForWildcards()
        {
            var actualString =
                $"Events with validFrom date in the future aren't supported yet by the platform. Overall Save will be discarded due to event(s){Environment.NewLine}[FondsCurrencyChanged: validFrom=2019-02-13-230000000, createdAt=2018-02-14-102858250]";

            Check.That(actualString).Matches(
                $"Events with validFrom date in the future aren't supported yet by the platform. Overall Save will be discarded due to event\\(s\\){Environment.NewLine}\\[FondsCurrencyChanged: validFrom=.*, createdAt=2018-02-14-.*");
        }

        [Test]
        public void Test()
        {
            var first = new MyClass();
            var second = new MyClass();

            Check.That(first).HasFieldsWithSameValues(second);
        }

        [Test]
        public void TestMethod()
        {
            // Arrange
            var autoPropertyValue = "I am a test.";
            var childOne = new Child {AutoProperty = autoPropertyValue};

            // Act
            var childTwo = new Child {AutoProperty = autoPropertyValue};

            // Assert
            Check.That(childOne).HasFieldsWithSameValues(childTwo);
        }

#if DOTNET_45
        [Test]
        public void reproduce_issue_204()
        {
            Check.ThatAsyncCode(() => this.Execute(5)).Throws<Exception>();
        }

        private Task<int> Execute(int i)
        {
            if (i == 5)
            {
                throw new Exception("bad");
            }

            return Task.FromResult(i);
        }
#endif
    }
}