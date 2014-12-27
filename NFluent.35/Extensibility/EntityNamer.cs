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

namespace NFluent.Extensibility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class EntityNamer
    {
        public const string DefaultEntityName = "value";

        #region Fields

        private string forcedEntity;

        #endregion

        #region Public Properties

        public string EntityName
        {
            get
            {
                if (this.forcedEntity != null)
                {
                    return this.forcedEntity;
                }

                if (this.EntityType == null)
                {
                    return DefaultEntityName;
                }
                
                if (this.EntityType == typeof(bool))
                {
                    return "boolean";
                }

                if (this.EntityType == typeof(string))
                {
                    return "string";
                }

                if (this.EntityType == typeof(DateTime))
                {
                    return "date time";
                }

                var interfaces = new List<Type>(this.EntityType.GetInterfaces());
                interfaces.Add(this.EntityType);

                if (interfaces.Contains(typeof(IDictionary)))
                {
                    return "dictionary";
                }

                if (interfaces.Contains(typeof(IEnumerable)))
                {
                    return "enumerable";
                }

                return this.EntityType == typeof(char) ? "char" : DefaultEntityName;
            }

            set
            {
                this.forcedEntity = value;
            }
        }

        public Type EntityType { get; set; }

        #endregion
    }
}