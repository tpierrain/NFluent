using System.Collections.Generic;

namespace NFluent.Tests
{
    using System.Collections;

    class CustomDico<K,V> : IDictionary<K, V>
    {
        private readonly IDictionary<K, V> source;

        public CustomDico(IDictionary<K,V> source)
        {
            this.source = source;
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return this.source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(KeyValuePair<K, V> item)
        {
            this.source.Add(item);
        }

        public void Clear()
        {
            this.source.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return this.source.Contains(item);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            this.source.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return this.source.Remove(item);
        }

        public int Count => this.source.Count;
        public bool IsReadOnly => this.source.IsReadOnly;
        public bool ContainsKey(K key)
        {
            return this.source.ContainsKey(key);
        }

        public void Add(K key, V value)
        {
            this.source.Add(key, value);
        }

        public bool Remove(K key)
        {
            return this.source.Remove(key);
        }

        public bool TryGetValue(K key, out V value)
        {
            return this.TryGetValue(key, out value);
        }

        public V this[K key]
        {
            get => this.source[key];
            set => this.source[key] = value;
        }

        public ICollection<K> Keys => this.source.Keys;
        public ICollection<V> Values => this.source.Values;
    }
}
