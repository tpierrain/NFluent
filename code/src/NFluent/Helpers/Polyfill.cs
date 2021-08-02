// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Polyfill.cs" company="">
//   Copyright 2017 Cyrille DUPUYDAUBY
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
//         http://www.apache.org/licenses/LICENSE-2.0
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if DOTNET_35
namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Place holder for 
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}
#endif

namespace NFluent
{
#if DOTNET_35
    using System.Collections.Generic;
    using System.Linq;
    /// <summary>
    /// Delegates that has a return value and takes one parameter.
    /// </summary>
    /// <typeparam name="T">Type of return value.</typeparam>
    /// <typeparam name="TU">Type of parameter.</typeparam>
    /// <typeparam name="TV">Type of parameter.</typeparam>
    /// <param name="param">value of the parameter</param>
    /// <param name="param2">value of the parameter</param>
    /// <returns>Return value.</returns>
    public delegate T Func<in TU, in TV, out T>(TU param, TV param2);

    /// <summary>
    /// Delegates that takes two parameters and does not return anything.
    /// </summary>
    /// <typeparam name="TU">Type of first parameter.</typeparam>
    /// <typeparam name="TV">Type of second parameter.</typeparam>
    /// <param name="param">value of the parameter</param>
    /// <param name="param2">value of the second parameter</param>
    public delegate void Action<in TU, in TV>(TU param, TV param2);

    /// <summary>
    /// Implements a function with on argument returning a value.
    /// </summary>
    /// <typeparam name="T">Type of the argument</typeparam>
    /// <typeparam name="TU">Type of the return value</typeparam>
    /// <param name="value">Parameters value</param>
    /// <returns></returns>
    public delegate TU Func<in T, out TU>(T value);

#endif
    /// <summary>
    /// Contains various extensions method to provide poly fills on various net framework versions
    /// </summary>
    internal static class PolyFill
    {
        public static bool IsNullOrWhiteSpace(string testedText)
        {
#if DOTNET_35
            return string.IsNullOrEmpty(testedText) || testedText.All(char.IsWhiteSpace);

#else
            return string.IsNullOrWhiteSpace(testedText);
#endif
        }

        public static long LongLength<T>(this T[] array)
        {
            return array.LongLength;
        }

        public static long LongLength(this System.Array array)
        {
            return array.LongLength;
        }
    }

#if DOTNET_35
    internal interface IReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        int Count { get; }
        bool ContainsKey(TKey key);
        bool TryGetValue(TKey key, out TValue value);
        TValue this[TKey key] { get; }
        IEnumerable<TKey> Keys {  get; }
        IEnumerable<TValue> Values { get; }
    }
#endif
}
