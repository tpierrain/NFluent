// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckList.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
//   // //   Licensed under the Apache License, Version 2.0 (the "License");
//   // //   you may not use this file except in compliance with the License.
//   // //   You may obtain a copy of the License at
//   // //       http://www.apache.org/licenses/LICENSE-2.0
//   // //   Unless required by applicable law or agreed to in writing, software
//   // //   distributed under the License is distributed on an "AS IS" BASIS,
//   // //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   // //   See the License for the specific language governing permissions and
//   // //   limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace NFluent.Tests.ForDocumentation
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    
    /// <summary>
    /// describes a list of nFluent Checks.
    /// </summary>
    public class CheckList
    {
        #region Fields

        private List<CheckDescription> checks = new List<CheckDescription>();

        #endregion

        #region Public Properties

        [XmlAttribute]
        public string CheckName { get; set; }

        /// <summary>
        /// Gets or sets a list of checks with varying signatures
        /// </summary>
        public List<CheckDescription> CheckSignatures
        {
            get
            {
                return this.checks;
            }

            set
            {
                this.checks = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void AddCheck(CheckDescription description)
        {
            this.checks.Add(description);
        }

        #endregion
    }
}