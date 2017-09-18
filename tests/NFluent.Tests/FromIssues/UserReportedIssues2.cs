using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NFluent.Tests.FromIssues
{
    [TestFixture]
    public class UserReportedIssues2
    {
        [Test]
        public void should_recognize_autoproperty_readonly_values()
        {
            var someClass = new SomeClass("Hello"){Other ="world", Values = new Dictionary<string, string>{["key1"] = "value1"}};
            Check.That(someClass).HasFieldsWithSameValues(new {Other = "world", Values = new Dictionary<string, string> { ["key1"] = "value1" } });
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