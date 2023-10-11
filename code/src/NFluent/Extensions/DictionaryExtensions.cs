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
    using System.Collections;
    using System.Collections.Generic;
    using Helpers;
#if DOTNET_45 || NETSTANDARD1_3
    using System.Reflection;
#endif
    using System.Linq;

    internal static class DictionaryExtensions
    {
        public static IReadOnlyDictionary<TK, TV> WrapDictionary<TK, TV>(object knownDictionary)
        {
            // if the object implements IDictionary
            if (knownDictionary is IDictionary simpleDictionary)
            {
                return new DictionaryWrapper<TK, TV>(simpleDictionary);
            }

            // if it implements IReadonlyDictionary<,>
            var interfaces = knownDictionary.GetType().GetInterfaces().Where( i=>i.IsGenericType()).ToArray();
            var roDictionaryInterface = interfaces
                .FirstOrDefault(i=> i.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>));
            // Stryker disable once Block: Mutation does not alter behaviour
            if (roDictionaryInterface != null)
            {
                var targetRoType= typeof(ReadOnlyDictionaryWrapper<,,,>).MakeGenericType(typeof(TK), typeof(TV), roDictionaryInterface.GetGenericArguments()[0], roDictionaryInterface.GetGenericArguments()[1]);
                var readOnlyDictionaryBuilder = targetRoType.GetConstructor(new[] {roDictionaryInterface});
                return (IReadOnlyDictionary<TK,TV>) readOnlyDictionaryBuilder.Invoke(new[] {knownDictionary});
            }
        
            // attempt IDictionary<,>
            var dictionaryInterface = interfaces
                .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
            if (dictionaryInterface != null)
            {
                var targetType = typeof(DictionaryWrapper<,,,>).MakeGenericType(typeof(TK), typeof(TV),
                    dictionaryInterface.GetGenericArguments()[0], dictionaryInterface.GetGenericArguments()[1]);
                var constructor = targetType.GetConstructor(new[] {dictionaryInterface});
                return (IReadOnlyDictionary<TK, TV>) constructor.Invoke(new[] {knownDictionary});
            }

            var enumerationInterface =
                interfaces.FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            var enumerated = enumerationInterface?.GetGenericArguments()[0];
            if (enumerationInterface != null && enumerated.IsGenericType() && enumerated.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                var targetRoType= typeof(KeyValueEnumerationWrapper<,,,>).MakeGenericType(typeof(TK), 
                    typeof(TV), 
                    enumerated.GetGenericArguments()[0], 
                    enumerated.GetGenericArguments()[1]);
                var readOnlyDictionaryBuilder = targetRoType.GetConstructor(new[] {enumerationInterface});
                return (IReadOnlyDictionary<TK, TV>)readOnlyDictionaryBuilder.Invoke(new[]{knownDictionary});
            }
            return null;
        }
    }
}