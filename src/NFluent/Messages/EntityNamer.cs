//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EntityNamer.cs" company="">
//    Copyright 2014 Cyrille DUPUYDAUBY
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//        http://www.apache.org/licenses/LICENSE-2.0
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Messages
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using Extensions;
    using Helpers;
#if NETSTANDARD1_3 || DOTNET_45
    using System.Reflection;
#endif

    /// <summary>
    /// Is responsible to provide adequate naming for values in NFluent. Rule is:
    /// 1 specified name
    /// 2 type dependant name (for known types)
    /// 3 'value'
    /// This provides a consistent algorithm for all messages.
    /// </summary>
    internal class EntityNamer
    {
        private const string DefaultEntityName = "value";

        private string forcedEntity;

        private static readonly Dictionary<Type, string> namingCache = new Dictionary<Type, string>();

        private static string GetDefaultName(Type type)
        {
            if (type == null)
            {
                return DefaultEntityName;
            }
            
            if (!namingCache.ContainsKey(type))
            {
                namingCache[type] = BuildTypeLabel(type);
            }
            return namingCache[type];
        }

        private static string BuildTypeLabel(Type type)
        {
            if (type == typeof(bool))
            {
                return "boolean";
            }

            if (type == typeof(string))
            {
                return "string";
            }

            if (type == typeof(DateTime))
            {
                return "date time";
            }

            if (type.GetTypeInfo().IsEnum)
            {
                return "enum";
            }

            if (type == typeof(char))
            {
                return "char";
            }

            if (type.IsNumerical())
            {
                return "value";
            }

            if (type == typeof(Duration))
            {
                return "duration";
            }

            if (typeof(EventWaitHandle).IsAssignableFrom(type))
            {
                return "event";
            }

            var interfaces = new List<Type>(type.GetInterfaces()) { type };

            if (interfaces.Contains(typeof(IDictionary)))
            {
                return "dictionary";
            }

            if (interfaces.Contains(typeof(IEnumerable)))
            {
                return "enumerable";
            }

            if (type.GetTypeInfo().IsValueType)
            {
                return "struct";
            }

            return DefaultEntityName;
        }

        public string EntityName
        {
            get
            {
                if (this.forcedEntity != null)
                {
                    return this.forcedEntity;
                }

                return GetDefaultName(this.EntityType);
            }

            set => this.forcedEntity = value;
        }

        public Type EntityType { get; set; }
    }
}