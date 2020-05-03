// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DictionaryWrapper.cs" company="NFluent">
//   Copyright 2019 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
namespace NFluent.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    internal class DictionaryWrapper<TK, TV, TKSource, TVSource> 
        : IReadOnlyDictionary<TK, TV>
    where TKSource: TK
    where TVSource: TV
    {
        private readonly IDictionary<TKSource, TVSource> source;

        public DictionaryWrapper(IDictionary<TKSource, TVSource> source)
        {
            this.source = source;
        }

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            return this.source.Select( entry => new KeyValuePair<TK, TV>(entry.Key, entry.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count => this.source.Count;

        public bool ContainsKey(TK key)
        {
            return this.source.ContainsKey((TKSource)key);
        }

        public bool TryGetValue(TK key, out TV value)
        {
            var found = this.source.TryGetValue((TKSource) key, out var temp);
            value = temp;
            return found;
        }

        public TV this[TK key] => this.source[(TKSource) key];

        public IEnumerable<TK> Keys => this.source.Keys.Cast<TK>();

        public IEnumerable<TV> Values => this.source.Values.Cast<TV>();
    }

    internal class DictionaryWrapper<TK, TV> 
        : IReadOnlyDictionary<TK, TV>
    {
        private readonly IDictionary source;
        public DictionaryWrapper(IDictionary source)
        {
            this.source = source;
        }

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            return new DictionaryEnumerator(this.source);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count => this.source.Count;

        public bool ContainsKey(TK key)
        {
            return this.source.Contains(key);
        }

        public bool TryGetValue(TK key, out TV value)
        {
            var found = this.source.Contains(key);
            if (found)
            {
                value = (TV) this.source[key];
            }
            else
            {
                value = default;
            }
            return found;
        }

        public TV this[TK key] => (TV) this.source[key];

        public IEnumerable<TK> Keys => this.source.Keys.Cast<TK>();

        public IEnumerable<TV> Values => this.source.Values.Cast<TV>();

        private class DictionaryEnumerator : IEnumerator<KeyValuePair<TK, TV>>
        {
            private readonly IDictionaryEnumerator keyEnumerator;

            public DictionaryEnumerator(IDictionary dictionary)
            {
                this.keyEnumerator = dictionary.GetEnumerator();
            }

            public void Dispose()
            {}

            public bool MoveNext()
            {
                return this.keyEnumerator.MoveNext();
            }

            public void Reset()
            {
                this.keyEnumerator.Reset();
            }

            public KeyValuePair<TK, TV> Current => new KeyValuePair<TK, TV>((TK)this.keyEnumerator.Key, (TV)this.keyEnumerator.Value);

            object IEnumerator.Current => this.Current;
        }
    }

    internal class ReadOnlyDictionaryWrapper<TK, TV, TK2, TV2> 
        : IReadOnlyDictionary<TK, TV>
    where TK2: TK
    where TV2: TV
    {
        private readonly IReadOnlyDictionary<TK2, TV2> source;

        public ReadOnlyDictionaryWrapper(IReadOnlyDictionary<TK2, TV2> source)
        {
            this.source = source;
        }

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            return this.source.Select( entry => new KeyValuePair<TK, TV>(entry.Key, entry.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count => this.source.Count;

        public bool ContainsKey(TK key)
        {
            return this.source.ContainsKey((TK2)key);
        }

        public bool TryGetValue(TK key, out TV value)
        {
            var found = this.source.TryGetValue((TK2) key, out var temp);
            value = temp;
            return found;
        }

        public TV this[TK key] => this.source[(TK2) key];

        public IEnumerable<TK> Keys => this.source.Keys.Cast<TK>();

        public IEnumerable<TV> Values => this.source.Values.Cast<TV>();
    }

    internal class KeyValueEnumerationWrapper<TK, TV, TKSource, TVSource>
        : IReadOnlyDictionary<TK, TV>
        where TKSource : TK
        where TVSource : TV
    {
        private readonly IEnumerable<KeyValuePair<TKSource, TVSource>> source;

        public KeyValueEnumerationWrapper(IEnumerable<KeyValuePair<TKSource, TVSource>> source)
        {
            this.source = source;
        }

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            return this.source.Select(p => new KeyValuePair<TK, TV>(p.Key, p.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count => this.source.Count();

        public bool ContainsKey(TK key)
        {
            return this.source.Any(pair => key.Equals(pair.Key));
        }

        public bool TryGetValue(TK key, out TV value)
        {
            try
            {
                value = this.source.First(pair => key.Equals(pair.Key)).Value;
                return true;
            }
            catch (InvalidOperationException)
            {
                value = default;
                return false;
            }
        }

        public TV this[TK key] => this.source.First(pair => key.Equals(pair.Key)).Value;

        public IEnumerable<TK> Keys => this.source.Select(pair => (TK) pair.Key);
        public IEnumerable<TV> Values => this.source.Select(pair => (TV) pair.Value);
    }
}
