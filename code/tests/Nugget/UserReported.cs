namespace Nugget
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NFluent;
    using NUnit.Framework;

    public class UserReported
    {
        // issue #176
        [Test]
        public void NegatedIsZero()
        {
            Check.That(1).Not.IsZero();
            Check.That(1).IsNotZero();
            Check.That(0).Not.IsNotZero();
            Check.That(0).IsZero();
        }
        // issue #292
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

            IEnumerable<int> expected = Enumerable.Range(1, 4);
            
            CollectionAssert.AreEquivalent(expected, toBeChecked);  // OK 
            CollectionAssert.AreEqual(expected, toBeChecked);       // OK

            Check.That(toBeChecked).ContainsExactly(expected);      // KO ;-(
        }

        [Test]
        public void IsEqualToIssue_WithToString()
        {
            var input = new { data = "testData" };
            var json = JsonConvert.SerializeObject(input);

            var obj = JObject.Parse(json);

            Check.That(obj["data"].ToString()).IsEqualTo("testData");
        }

    }
}
