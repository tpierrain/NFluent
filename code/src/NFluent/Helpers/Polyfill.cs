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

namespace NFluent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Extensibility;
    using Extensions;
#if NETSTANDARD1_3
    using System.Reflection;
#endif

#if DOTNET_35
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
#if DOTNET_30 || DOTNET_20

    /// <summary>
    /// Delegate that does not return a value and takes no parameter.
    /// </summary>
    public delegate void Action();

    /// <summary>
    /// Delegates that takes and parameter and does not return anything.
    /// </summary>
    /// <typeparam name="TU">Type of parameter.</typeparam>
    /// <param name="param">value of the parameter</param>
    public delegate void Action<in TU>(TU param);

    /// <summary>
    /// Delegates that takes and parameter and does not return anything.
    /// </summary>
    /// <typeparam name="TU">Type of first parameter.</typeparam>
    /// <typeparam name="TV">Type of second parameter.</typeparam>
    /// <param name="param">value of the parameter</param>
    /// <param name="param2">value of the second parameter</param>
    public delegate void Action<in TU, in TV>(TU param, TV param2);

    /// <summary>
    /// Delegates that has a return value.
    /// </summary>
    /// <typeparam name="T">Type of return value.</typeparam>
    /// <returns>Return value.</returns>
    public delegate T Func<out T>();
    
    /// <summary>
    /// Delegates that has a return value and takes one parameter.
    /// </summary>
    /// <typeparam name="T">Type of return value.</typeparam>
    /// <typeparam name="TU">Type of parameter.</typeparam>
    /// <param name="param">value of the parameter</param>
    /// <returns>Return value.</returns>
    public delegate T Func<in TU, out T>(TU param);

    /// <summary>
    /// Delegates that has a return value and takes two parameters.
    /// </summary>
    /// <typeparam name="T">Type of return value.</typeparam>
    /// <typeparam name="TU">Type of first parameter.</typeparam>
    /// <typeparam name="T1">Type of second parameter.</typeparam>
    /// <param name="param">value of the first parameter</param>
    /// <param name="param2">value of the second parameter</param>
    /// <returns>Return value.</returns>
    public delegate T Func<in TU, in T1, out T>(TU param, T1 param2);

    /// <summary>
    /// Delegates that has a return value and takes three parameter.
    /// </summary>
    /// <typeparam name="T">Type of return value.</typeparam>
    /// <typeparam name="TU">Type of first parameter.</typeparam>
    /// <typeparam name="T1">Type of second parameter.</typeparam>
    /// <typeparam name="T2">Type of second parameter.</typeparam>
    /// <param name="param">value of the first parameter</param>
    /// <param name="param2">value of the second parameter</param>
    /// <param name="param3">value of the second parameter</param>
    /// <returns>Return value.</returns>
    public delegate T Func<in TU, in T1, in T2, out T>(TU param, T1 param2, T2 param3);

    /// <summary>
    /// Delegates that is used as a filter. Returns true or false.
    /// </summary>
    /// <typeparam name="T">Type of parameter.</typeparam>
    /// <param name="item">Parameter value.</param>
    /// <returns>true if <see paramref="item"/> matches the rule.</returns>
    public delegate bool Predicate<in T>(T item);

#endif


    /// <summary>
    /// Contains various extensions method to provide poly fills on various net framework versions
    /// </summary>
    internal static class PolyFill
    {
#if DOTNET_30 || DOTNET_20
        public static IList<T> Cast<T>(this IEnumerable list)
        {
            if (list is IList<T> list1)
            {
                return list1;
            }
            var result = new List<T>();
            foreach (var u in list)
            {
                result.Add((T) u);
            }
            return result;
        }

        public static IList<T> ToList<T>(this IEnumerable<T> list)
        {
            return new List<T>(list);
        }

        public static long LongCount<T>(this IList<T> list)
        {
            return list.Count;
        }

        public static T[] ToArray<T>(this IEnumerable<T> enumeration)
        {
            if (enumeration == null)
            {
                return null;
            }

            var size = enumeration.Count();
            var result = new T[size];
            var index = 0;
            foreach (var item in enumeration)
            {
                result[index++] = item;
            }

            return result;
        }

        public static int FindIndex<T>(this IList<T> list, Predicate<T> predicate)
        {
            for(var i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    return i;

                }
            }
            return -1;
        }
    
        public static bool Any<T>(this IList<T> list)
        {
            foreach (var item in list)
            {
                return true;
            }
            return false;
        }

        public static bool Any<T>(this IEnumerable<T> list, Predicate<T> predicate)
        {
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    return true;
                }
            }
            return false;
        }

        public static IList<T> Where<T>(this IList<T> list, Predicate<T> predicate)
        {
            var result = new List<T>();
            foreach (var item in list)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        
        public static void RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i--);
                }
            }
        }

        public static bool Contains<T>(this IEnumerable<T> list, T lookup, IEqualityComparer<T> comparer)
        {
            foreach (var item in list)
            {
                if (comparer.Equals(lookup, item))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Contains<T>(this IEnumerable<T> list, T lookup)
        {
            foreach (var item in list)
            {
                if (object.Equals(lookup, item))
                {
                    return true;
                }
            }
            return false;
        }
    
        public static IEnumerable<TR> Select<T, TR>(this IEnumerable<T> list, Func<T, TR> selector) {
            var result = new List<TR>();
            foreach (var item in list)
            {
                result.Add(selector.Invoke(item));
            }
            return result;
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            foreach (var t in list)
            {
                if (predicate(t))
                {
                    return t;
                }
            }

            return default(T);
        }
#endif
        public static bool IsNullOrWhiteSpace(string testedText)
        {
#if DOTNET_20 || DOTNET_30 || DOTNET_35
            if (string.IsNullOrEmpty(testedText))
            {
                return true;
            }

            foreach (var character in testedText)
            {
                if (!char.IsWhiteSpace(character))
                {
                    return false;
                }
            }
            return true;
#else
            return string.IsNullOrWhiteSpace(testedText);
#endif
        }

        public static long LongLength<T>(this T[] array)
        {
#if NETSTANDARD1_3
            return array.Length;
#else
            return array.LongLength;
#endif
        }
        public static long LongLength(this System.Array array)
        {
#if NETSTANDARD1_3
            return array.Length;
#else
            return array.LongLength;
#endif
        }

#if NETSTANDARD1_3
        public static long GetLongLength(this System.Array array, int index)
        {
            return array.GetLength(index);
        }

        public static object GetValue(this System.Array array, long[] indices)
        {
            var shorterIndices = new int[indices.Length];
            for (var i = 0; i < indices.Length; i++)
            {
                shorterIndices[i] = (int) indices[i];
            }

            return array.GetValue(shorterIndices);
        }

        public static bool IsInstanceOfType(this System.Type type, object instance)
        {
            return type.GetTypeInfo().IsAssignableFrom(instance.GetType().GetTypeInfo());
        }
#endif
    }

#if DOTNET_20 || DOTNET_30 || DOTNET_35 || DOTNET_40
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
