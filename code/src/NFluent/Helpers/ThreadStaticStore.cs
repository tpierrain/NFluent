﻿// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ThreadStaticStore.cs" company="NFluent">
//   Copyright 2021 Cyrille DUPUYDAUBY
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

namespace NFluent.Helpers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     This class provides thread static field instances
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class ThreadStaticStore<T> where T : new()
    {
        [ThreadStatic] private static Dictionary<object, T> stacksMap;

        /// <summary>
        /// Get the thread static value associated with <paramref name="identifier"></paramref>
        /// </summary>
        /// <param name="identifier">the identifier for the instance.</param>
        /// <returns>The thread specific value</returns>
        public static T GetStack(object identifier)
        {

            if (stacksMap == null)
            {
                stacksMap = new Dictionary<object, T>();
            }

            if (!stacksMap.TryGetValue(identifier, out var result))
            {
                result = stacksMap[identifier] = new T();
            }

            return result;
        }


        public static bool Exists(object identifier)
        {
            return stacksMap != null && stacksMap.ContainsKey(identifier);
        }

        public static void ClearStack(object identifier)
        {
            stacksMap.Remove(identifier);
        }
    }
}