using System.Collections.Generic;

namespace NFluent.Helpers
{
    using System.Collections;
#if !DOTNET_20 && !DOTNET_30
    using System.Linq;
#endif
    internal class DictionaryWrapper<TK, TV, TK2, TV2> 
        : IReadOnlyDictionary<TK, TV>
    where TK2: TK
    where TV2: TV
    {
        private readonly IDictionary<TK2, TV2> source;

        public DictionaryWrapper(IDictionary<TK2, TV2> source)
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
            if (this.source.TryGetValue((TK2) key, out var temp))
            {
                value = temp;
                return true;
            }

            value = default(TV);
            return false;
        }

        public TV this[TK key] => this.source[(TK2) key];

        public IEnumerable<TK> Keys => this.source.Keys.Cast<TK>();

        public IEnumerable<TV> Values => this.source.Values.Cast<TV>();
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
            if (this.source.TryGetValue((TK2) key, out var temp))
            {
                value = temp;
                return true;
            }

            value = default(TV);
            return false;
        }

        public TV this[TK key] => this.source[(TK2) key];

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
            if (this.source.Contains(key))
            {
                value = (TV) this.source[key];
                return true;
            }

            value = default(TV);
            return false;
        }

        public TV this[TK key] => (TV) this.source[key];

        public IEnumerable<TK> Keys => this.source.Keys.Cast<TK>();

        public IEnumerable<TV> Values => this.source.Values.Cast<TV>();

        private class DictionaryEnumerator : IEnumerator<KeyValuePair<TK, TV>>
        {
            private readonly IDictionaryEnumerator keyEnumerator;
            private readonly IDictionary dictionary;

            public DictionaryEnumerator(IDictionary dictionary)
            {
                this.dictionary = dictionary;
                this.keyEnumerator = dictionary.GetEnumerator();
            }

            public int Count => this.dictionary.Count;
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
}
