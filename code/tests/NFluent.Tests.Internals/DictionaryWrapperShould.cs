namespace NFluent.Tests.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
            foreach (var keyValuePair in wrapper)
            {
                Check.That(keyValuePair.Key).IsInstanceOf<int>();
                Check.That(keyValuePair.Value).IsInstanceOf<string>();
            }
        }

        [Test]
        public void SupportKeyBasedAccess()
        {
            var demo = new Dictionary<int, string> {{1, "one"}, {2, "two"}};
            var wrapper = new DictionaryWrapper<object, object, int, string>(demo);

            CheckBaseMethods(wrapper);
        }

        private static void CheckBaseMethods(IReadOnlyDictionary<object, object> wrapper)
        {
            Check.That(wrapper.ContainsKey(1)).IsTrue();
            Check.That(wrapper[2]).IsEqualTo("two");

            Check.That(wrapper.TryGetValue(1, out var val)).IsTrue();
            Check.That(val).IsEqualTo("one");

            Check.That(wrapper.TryGetValue(3, out val)).IsFalse();
            Check.That(val).IsEqualTo(null);

            foreach(var value in (IEnumerable) wrapper)
            {
                Check.That(value).IsNotNull();
            }

            Check.That(wrapper.Count).IsEqualTo(2);

            foreach (var wrapperValue in wrapper.Values)
            {
                Check.That(wrapperValue).IsInstanceOf<string>();
            }

            var iterator = wrapper.GetEnumerator();
            Check.That(iterator.MoveNext()).IsTrue();
            Check.That(iterator.Current.Value).IsEqualTo("one");
            try
            {
                iterator.Reset();
                Check.That(iterator.MoveNext()).IsTrue();
                Check.That(iterator.Current.Value).IsEqualTo("one");
            }
            catch (NotImplementedException)
            {
            }
            catch (NotSupportedException)
            {
            }
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
            var rodemo = new RoDico<int, string>(new Dictionary<int, string> {{1, "one"}, {2, "two"}});
            var wrapped = DictionaryExtensions.WrapDictionary<object, object>(rodemo);

            Check.That(wrapped).IsNotNull();
            Check.That(wrapped).ContainsKey(2);
        }

        [Test]
        public void CanSupportBasicMethodsOnCustomDico()
        {
            var rodemo = new RoDico<int, string>(new Dictionary<int, string> {{1, "one"}, {2, "two"}});
            var wrapped = DictionaryExtensions.WrapDictionary<object, object>(rodemo);

            CheckBaseMethods(wrapped);
        }

#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40
        [Test]
        public void CanSupportBasicMethodsOnReadOnlyDico()
        {
            var rodemo = new ReadOnlyDictionary<int, string>(new Dictionary<int, string> {{1, "one"}, {2, "two"}});
            var wrapped = DictionaryExtensions.WrapDictionary<object, object>(rodemo);

            CheckBaseMethods(wrapped);
        }
#endif

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
