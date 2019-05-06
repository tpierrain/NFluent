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

namespace NFluent.Tests.SutClasses
{
#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40
    using System.Collections;
    using System.Collections.Generic;

    internal class RoDico : IReadOnlyDictionary<string, string>
    {
        private readonly IDictionary<string, string> myDico;

        public RoDico(IDictionary<string, string> dico)
        {
            this.myDico = dico;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.myDico.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count => this.myDico.Count;

        public bool ContainsKey(string key)
        {
            return this.myDico.ContainsKey(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return this.myDico.TryGetValue(key, out value);
        }

        public string this[string key] => this.myDico[key];

        public IEnumerable<string> Keys => this.myDico.Keys;
        public IEnumerable<string> Values => this.myDico.Values;
    }
#endif
}