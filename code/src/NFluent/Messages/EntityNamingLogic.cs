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
    /// 2 type dependent name (for known types)
    /// 3 'value'
    /// This provides a consistent algorithm for all messages.
    /// </summary>
    public class EntityNamingLogic
    {
        private const string DefaultEntityName = "value";
        private static readonly Dictionary<Type, string> NamingCache = new Dictionary<Type, string>();

        private Func<string> nameBuilder;
        private bool setPluralForm;

        /// <summary>
        /// Builds an instance
        /// </summary>
        public EntityNamingLogic()
        {
        }

        /// <summary>
        /// Builds an instance of naming logic with a forced entity name.
        /// </summary>
        /// <param name="forcedName"></param>
        public EntityNamingLogic(string forcedName)
        {
            this.SetNameBuilder(() => forcedName);
        }

        /// <summary>
        /// Gets/Sets the entity name
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// Gets the entity name
        /// </summary>
        public string EntityName
        {
            get
            {
                var entityName = this.nameBuilder != null ? this.nameBuilder() : GetDefaultName(this.EntityType);
                if (this.setPluralForm)
                {
                    entityName += "(s)";
                }
                return entityName;
            }
        }

        /// <summary>
        /// Clear the default name cached. Used in NFluent tests.
        /// </summary>
        // Stryker disable once Statement: Mutation does not alter behaviour
        public static void ClearDefaultNameCache()
        {
            NamingCache.Clear();
        }

        private static string GetDefaultName(Type type)
        {
            if (type == null)
            {
                return DefaultEntityName;
            }
            
            if (!NamingCache.ContainsKey(type))
            {
                NamingCache[type] = BuildTypeLabel(type);
            }
            return NamingCache[type];
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

            if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                return "date time";
            }

            if (type == typeof(TimeSpan) || type == typeof(Duration))
            {
                return "duration";
            }

            if (type == typeof(char))
            {
                return "char";
            }

            if (type == typeof(object))
            {
                return "object";
            }

            if (typeof(RunTrace).IsAssignableFrom(type))
            {
                return "code";
            }

            if (type.IsNullable())
            {
                return "nullable";
            }

            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsEnum)
            {
                return "enum";
            }

            if (type.IsNumerical())
            {
                return "value";
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

            return typeInfo.IsValueType ? "struct" : DefaultEntityName;
        }

        /// <summary>
        /// Provide a name builder function
        /// </summary>
        /// <param name="builder">builder function</param>
        public void SetNameBuilder(Func<string> builder)
        {
            this.nameBuilder = builder;
        }

        /// <summary>
        /// Use plural forms for naming
        /// </summary>
        public void SetPlural()
        {
            this.setPluralForm = true;
        }

        internal void Merge(EntityNamingLogic other)
        {
            if (other.nameBuilder != null)
            {
                this.nameBuilder = other.nameBuilder;
            }

            if (this.EntityType == null)
            {
                this.EntityType = other.EntityType;
            }
        }

        internal EntityNamingLogic Clone()
        {
            var result = new EntityNamingLogic {EntityType = this.EntityType, nameBuilder = this.nameBuilder};
            return result;
        }
    }
}