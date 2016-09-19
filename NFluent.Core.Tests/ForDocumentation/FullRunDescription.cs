// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FullRunDescription.cs" company="">
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

namespace NFluent.Tests.ForDocumentation
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    public class FullRunDescription
    {
        #region Fields

        private List<TypeChecks> runDescription = new List<TypeChecks>();

        #endregion

        #region Public Properties

        public List<TypeChecks> RunDescription
        {
            get
            {
                return this.runDescription;
            }

            set
            {
                this.runDescription = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void AddEntry(CheckDescription desc)
        {
            foreach (var typeCheckse in this.runDescription)
            {
                if (typeCheckse.CheckedType != desc.CheckedType)
                {
                    continue;
                }

                typeCheckse.AddCheck(desc);
                return;
            }

            var addedType = new TypeChecks(desc.CheckedType);
            addedType.AddCheck(desc);
            this.runDescription.Add(addedType);
        }

        public void Save(string name)
        {
            var serialier = new XmlSerializer(typeof(FullRunDescription));
            using (var file = new FileStream(name, FileMode.Create))
            {
                serialier.Serialize(file, this);
            }
        }

        #endregion
    }
}