// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Polyfill.cs" company="">
//   Copyright 2017 Cyrille DUPUYDAUBY
//   //   //     Licensed under the Apache License, Version 2.0 (the "License");
//   //   //     you may not use this file except in compliance with the License.
//   //   //     You may obtain a copy of the License at
//   //   //         http://www.apache.org/licenses/LICENSE-2.0
//   //   //     Unless required by applicable law or agreed to in writing, software
//   //   //     distributed under the License is distributed on an "AS IS" BASIS,
//   //   //     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   //   //     See the License for the specific language governing permissions and
//   //   //     limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent
{
#if DOTNET_30 || DOTNET_20
    using System.Collections;
    using System.Collections.Generic;

    public delegate void Action();
    public delegate T Func<out T>();
    public delegate T Func<in TU, out T>(TU param);

    internal delegate bool Predicate<T>(T item);
#endif

    /// <summary>
    /// Contains various extensions method to provide poly fills on various net framework versions
    /// </summary>
    internal static class PolyFill
    {
#if DOTNET_30 || DOTNET_20
        public static IList<T> Cast<T>(this IEnumerable list)
            where T: class
        {
            List<T> result = new List<T>();
            foreach (var u in list)
            {
                result.Add(u as T);
            }
            return result;
        }

        public static IList<T> ToList<T>(this IEnumerable<T> list)
        {
            var result = new List<T>();
            foreach (var u in list)
            {
                result.Add(u);
            }
            return result;
        }

        public static long LongCount<T>(this IList<T> list)
        {
            return list.ToList().Count;
        }

        public static bool Any<T>(this IList<T> list)
        {
            return list.ToList().Count>0;
        }

        public static bool Any<T>(this IList<T> list, Predicate<T> predicate)
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
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i--);
                }
            }
        }
#endif
    }
}
