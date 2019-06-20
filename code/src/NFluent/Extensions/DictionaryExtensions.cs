// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DictionaryExtensions.cs" company="NFluent">
//   Copyright 2019 Cyrille DUPUYDAUBY
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

namespace NFluent.Extensions
{
    using System.Collections.Generic;

    internal static class DictionaryExtensions
    {
        public static bool TryGet<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> dico, TK key, out TV value)
        {
            if (dico is IDictionary<TK, TV> trueDico)
            {
                return trueDico.TryGetValue(key, out value);
            }
#if !DOTNET_20 && !DOTNET_30 && !DOTNET_35 && !DOTNET_40

            if (dico is IReadOnlyDictionary<TK, TV> roDico)
            {
                return roDico.TryGetValue(key, out value);
            }
#endif
            foreach (var pair in dico)
            {
                if (Equals(pair.Key, key))
                {
                    value = pair.Value;
                    return true;
                }
            }

            value = default(TV);
            return false;
        }
    }
}