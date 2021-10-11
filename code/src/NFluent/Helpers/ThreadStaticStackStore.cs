using System;
using System.Collections.Generic;

namespace NFluent.Helpers
{
    /// <summary>
    /// This class provides thread static field instances
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class ThreadStaticStore<T> where T: new()
    {
        [ThreadStatic] private static Dictionary<object, T> stacksMap;

        /// <summary>
        /// Get the thread 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static T GetStack(object identifier)
        {
            stacksMap ??= new Dictionary<object, T>();

            if (!stacksMap.ContainsKey(identifier))
            {
                stacksMap[identifier] = new T();
            }

            return stacksMap[identifier];
        }

        public static bool Exists(object identifier)
        {
            return stacksMap!= null && stacksMap.ContainsKey(identifier);
        }

        public static void ClearStack(object identifier)
        {
            stacksMap?.Remove(identifier);
        }
    }
}
