using System.Collections.Generic;
using NFluent.ApiChecks;
#if DOTNET_45
using System;
using System.Threading.Tasks;
#endif
using NUnit.Framework;

namespace NFluent.Tests.FromIssues
{
    using Helpers;

    [TestFixture]
    public class UserReportedIssues2
    {
        #region GH #219
        // GH #219
        public class Parent
        {
            public virtual string AutoProperty { get; set; }
        }

        public class Child : Parent
        {
            public override string AutoProperty { get; set; }
        }
        [Test]
        public void TestMethod()
        {
            // Arrange
            var autoPropertyValue = "I am a test.";
            var childOne = new Child { AutoProperty = autoPropertyValue };

            // Act
            var childTwo = new Child { AutoProperty = autoPropertyValue };

            // Assert
            Check.That(childOne).HasFieldsWithSameValues(childTwo);
        }

        #endregion
        #region issue #215
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

        [Test]
        public void Test()
        {
            var first = new MyClass();
            var second = new MyClass();

            Check.That(first).HasFieldsWithSameValues(second);
        }
        #endregion 

        [Test]
        public void should_recognize_autoproperty_readonly_values()
        {
            var someClass = new SomeClass("Hello"){Other ="world", Values = new Dictionary<string, string>{["key1"] = "value1"}};
            Check.That(someClass).HasFieldsWithSameValues(new {Other = "world", Values = new Dictionary<string, string> { ["key1"] = "value1" } });
        }

        double decimalValue => 0.95000000000000006d;

        // GH #205 
        [Test]
        public void should_generate_correct_error_message_for_double()
        {
            using (var session = new CultureSession("en-US"))
            {
                Check.ThatCode(() => Check.That(this.decimalValue).IsEqualTo(0.95d)).Throws<FluentCheckException>().AndWhichMessage().AsLines()
                .ContainsExactly("",
                "The checked value is different from the expected one, with a difference of 1.1E-16. You may consider using IsCloseTo() for comparison.",
                "The checked value:",
                "\t[0.95]",
                "The expected value:",
                "\t[0.95]");

                Check.ThatCode(() => Check.That(0.9500001f).IsEqualTo(0.95f)).Throws<FluentCheckException>().AndWhichMessage().AsLines()
                .ContainsExactly("",
                "The checked value is different from the expected one, with a difference of 1.2E-07. You may consider using IsCloseTo() for comparison.",
                "The checked value:",
                "\t[0.9500001]",
                "The expected value:",
                "\t[0.9500001]");
                
            }
        }

        [Test]
        public void CollectionTest()
        {
            var a = new List<int> { 1, 2 };
            var b = new List<int> { 3, 4 };

            // List contains references to a and b
            var list1 = new List<List<int>> { a, b };
            Check.That(list1).ContainsExactly(a, b);  // OK
            Check.That(list1).ContainsExactly(new List<List<int>> { a, b });  // OK

            // List contains new instances of lists same as a and b
            var list2 = new List<List<int>>
            {
                new List<int> {1, 2}, // new instance, same as a
                new List<int> {3, 4}  // new instance, same as b
            };
            Check.That(list2).ContainsExactly(a, b);  // Fail
            Check.That(list2).ContainsExactly(new List<List<int>> { a, b });  // Fail
        }

        [Test(Description = "Issue #212")]
        public void CollectionWithNumeric()
        {
            var expected = new int[] { 1, 2 };
            var actual = new uint[] { 1, 2 };
            Check.That(actual).ContainsExactly(expected);
        }

#if DOTNET_45
        [Test]
        public void reproduce_issue_204()
        {
            Check.ThatAsyncCode(() => Execute(5)).Throws<Exception>();
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
        private class BaseClass
        {
            public string Id { get; }

            public BaseClass(string id)
            {
                Id = id;
            }
        }

        private class SomeClass: BaseClass
        {
            public SomeClass(string id): base(id)
            {
            }
            public string Salt => Id;
            public string Other { get; set; }
            public Dictionary<string, string> Values { get; set; }
        }
    }
    
    
}