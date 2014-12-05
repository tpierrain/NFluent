#region File header
// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ILabelBlock.cs" company="">
// //   Copyright 2014 Cyrille Dupuydauby, Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
#endregion
namespace NFluent.Extensibility
{
    using System;
    using System.Collections;

    /// <summary>
    /// Interface for Label generator.
    /// </summary>
    internal interface ILabelBlock
    {
        /// <summary>
        /// Customs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        string CustomMessage(string message);
    }

    internal class GenericLabelBlock : ILabelBlock
    {

        /// <summary>
        /// Gets or sets the entity logic.
        /// </summary>
        /// <value>
        /// The entity logic.
        /// </value>
        public EntityNameLogic EntityLogic { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", this.Adjective(), this.EntityName());
        }

        /// <summary>
        /// Customs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public string CustomMessage(string message)
        {
            return string.Format(message ?? "The {0} {1}:", this.Adjective(), this.EntityName());
        }

        protected virtual string Adjective()
        {
            return string.Empty;
        }

        private string EntityName()
        {
            return this.EntityLogic == null ? "value" : this.EntityLogic.EntityName;
        }
    }

    internal class EntityNameLogic
    {
        private string forcedEntity;

        public Type EntityType { get; set; }

        public string EntityName
        {
            get
            {
                if (forcedEntity != null)
                {
                    return forcedEntity;
                }

                if (this.EntityType == typeof(bool) || this.EntityType == typeof(Boolean))
                {
                    return "boolean";
                }

                if (this.EntityType == typeof(IEnumerable))
                {
                    return "enumerable";
                }

                if (this.EntityType == typeof(char))
                {
                    return "char";
                }

                return "value";
            }
            set
            {
                this.forcedEntity = value;
            }
        }
    }
}