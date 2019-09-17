using System.Collections.Generic;

namespace NFluent.Helpers
{
    using System.Collections;
    using System.Linq;

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
}
