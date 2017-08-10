using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NFluent.Tests.FromIssues
{
    [TestFixture]
    public class UserReportedIssues2
    {
        [Test]
//        [Ignore("test is baset on property, not fields")]
        public void should_recognize_autoproperty_readonly_values()
        {
            var someClass = new SomeClass("Hello"){Other ="world", Values = new Dictionary<string, string>{["key1"] = "value1"}};
            Check.That(someClass).HasFieldsWithSameValues(new {Other = "world", Values = new Dictionary<string, string> { ["key1"] = "value1" } });
        }

        [Test]
        public void should_work_with_anonymous_dico()
        {
            
        }
        
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