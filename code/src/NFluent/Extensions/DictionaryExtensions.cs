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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Helpers;
    using Messages;

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
                if (!Equals(pair.Key, key))
                {
                    continue;
                }

                value = pair.Value;
                return true;
            }

            value = default(TV);
            return false;
        }

        public static IReadOnlyDictionary<K, V> WrapDictionary<K, V>(object knownDictionary)
        {
            // if the object implements IDictionary
            if (knownDictionary is IDictionary)
            {
                var simpleWrapper= typeof(DictionaryWrapper<,>).MakeGenericType(new []{typeof(K), typeof(V)});
                var constructorForSimpleWrapper = simpleWrapper.GetConstructor(new[] {typeof(IDictionary)});
                Debug.Assert(constructorForSimpleWrapper!= null, "Internal error. Failed to find DictionaryWrapper builder.");
                return (IReadOnlyDictionary<K,V>) constructorForSimpleWrapper.Invoke(new[] {knownDictionary});
            }

            // if it implements IReadonlyDictionary<,>
            var roDictionaryInterface = knownDictionary.GetType().GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>));
            if (roDictionaryInterface != null)
            {
                var targetROType= typeof(ReadOnlyDictionaryWrapper<,,,>).MakeGenericType(new [] {typeof(K), typeof(V), roDictionaryInterface.GetGenericArguments()[0], roDictionaryInterface.GetGenericArguments()[1]});
                var roConstructor = targetROType.GetConstructor(new[] {roDictionaryInterface});
                Debug.Assert(roConstructor!= null, "Internal error. Failed to find DictionaryWrapper builder.");
                return (IReadOnlyDictionary<K,V>) roConstructor.Invoke(new[] {knownDictionary});
            }
 
            // last attempt, IDictionary<,>
            var dictionaryInterface = knownDictionary.GetType().GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
            if (dictionaryInterface == null)
            {
                return null;
            }

            var types = new Type[4];
            types[0] = typeof(K);
            types[1] = typeof(V);
            types[2] = dictionaryInterface.GetGenericArguments()[0];
            types[3] = dictionaryInterface.GetGenericArguments()[1];
            var targetType= typeof(DictionaryWrapper<,,,>).MakeGenericType(types);
            var constructor = targetType.GetConstructor(new[] {dictionaryInterface});
            Debug.Assert(constructor!= null, "Internal error. Failed to find DictionaryWrapper builder.");
            return (IReadOnlyDictionary<K,V>) constructor.Invoke(new[] {knownDictionary});
        }

    }
}