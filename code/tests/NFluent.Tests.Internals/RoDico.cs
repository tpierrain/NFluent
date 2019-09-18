// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="RoDico.cs" company="NFluent">
//   Copyright 2018 Cyrille DUPUYDAUBY
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

namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    internal class RoDico<K,V> : IReadOnlyDictionary<K, V>
    {
        private readonly IDictionary<K, V> myDico;

        public RoDico(IDictionary<K, V> dico)
        {
            this.myDico = dico;
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return this.myDico.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count => this.myDico.Count;

        public bool ContainsKey(K key)
        {
            return this.myDico.ContainsKey(key);
        }

        public bool TryGetValue(K key, out V value)
        {
            return this.myDico.TryGetValue(key, out value);
        }

        public V this[K key] => this.myDico[key];

        public IEnumerable<K> Keys => this.myDico.Keys;
        public IEnumerable<V> Values => this.myDico.Values;
    }
}