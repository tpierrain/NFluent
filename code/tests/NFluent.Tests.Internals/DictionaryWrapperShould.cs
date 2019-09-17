using System.Collections.Generic;

namespace NFluent.Tests.Helpers
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Extensions;
    using NUnit.Framework;
    using NFluent.Helpers;

    [TestFixture]
    public class DictionaryWrapperShould
    {
        [Test]
        public void WrapArbitraryDictionaries()
        {
            var demo = new Dictionary<int, string> {{1, "one"}, {2, "two"}};

            var wrapper = new DictionaryWrapper<object, object, int, string>(demo);

            Check.That(wrapper).ContainsKey((object) 2);
            Check.That(wrapper).ContainsValue((object) "one");
            Check.That(wrapper.Keys).CountIs(demo.Keys.Count).And.IsEqualTo(demo.Keys);
            Check.That(wrapper.Values).CountIs(demo.Values.Count).And.IsEqualTo(demo.Values);

            Check.That(wrapper.Count).IsEqualTo(demo.Count);

            Check.That(wrapper.Cast<object>()).CountIs(2);
        }

        [Test]
        public void CanWrapWithoutTypeSignature()
        {
            var demo = new Dictionary<int, string> {{1, "one"}, {2, "two"}};

            var wrapped = DictionaryExtensions.WrapDictionary<object, object>(demo);

            Check.That(wrapped).IsNotNull();

            Check.That(wrapped).ContainsKey(2);
        }

        [Test]
        public void CanWrapReadonlyDictionary()
        {
            var demo = new Dictionary<int, string> {{1, "one"}, {2, "two"}};
            var rodemo = new RoDico<int, string>(demo);

            var wrapped = DictionaryExtensions.WrapDictionary<object, object>(rodemo);

            Check.That(wrapped).IsNotNull();

            Check.That(wrapped).ContainsKey(2);
        }

        [Test]
        public void CanWrapCustomDictionary()
        {
            var demo = new Dictionary<int, string> {{1, "one"}, {2, "two"}};
            var coDemo = new CustomDico<int, string>(demo);

            var wrapped = DictionaryExtensions.WrapDictionary<object, object>(coDemo);

            Check.That(wrapped).IsNotNull();

            Check.That(wrapped).ContainsKey(2);
        }
    }
}
