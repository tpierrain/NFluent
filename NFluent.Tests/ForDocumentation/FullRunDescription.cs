namespace NFluent.Tests.ForDocumentation
// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="CheckDescription.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
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

{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

        #region Public Properties

        [XmlIgnore]
        public string CheckName
        {
            get
            {
                return this.Check.Name;
            }
        }
        public List<Type> CheckParameters { get; set; }

        public string Signature
        {
            get
            {
                var checkParameters = this.CheckParameters;
                if (checkParameters != null)
                {
                    var parameters = new StringBuilder(checkParameters.Count * 20);

                    // build parameter list
                    if (checkParameters.Count > 0)
                    {
                        parameters.Append(checkParameters[0].Name);
                        for (int i = 1; i < checkParameters.Count; i++)
                        {
                            parameters.Append(", ");
                            parameters.Append(checkParameters[i].Name);
                        }
                    }

                    return string.Format("Check.That({0} sut).{1}({2})", this.CheckedType.Name, this.Check.Name, parameters);
                }

                return string.Empty;
            }

            set
            {
            }
        }

        [XmlIgnore]
        public Type CheckedType { get; set; }

        #endregion
    }

    // information regarding various signatures for one check
    public class CheckList
    {
        #region Fields

        private List<CheckDescription> checks = new List<CheckDescription>();

        #endregion

        #region Public Properties

        [XmlAttribute]
        public string CheckName { get; set; }

        public List<CheckDescription> Checks
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

    // information regarding all checks for one type
    public class TypeChecks
    {
        #region Fields

        private readonly Type checkedType;

        private List<CheckList> checks = new List<CheckList>();

        #endregion

        #region Constructors and Destructors

        public TypeChecks(Type checkedType)
        {
            this.checkedType = checkedType;
        }

        public TypeChecks()
        {
        }

        #endregion

        #region Public Properties
        [XmlAttribute]
        public string Typename
        {
            get
            {
                return this.CheckedType.Name;
            }

            set
            {
            }
        }

        public Type CheckedType
        {
            get
            {
                return this.checkedType;
            }
        }

        [XmlArray]
        public List<CheckList> Checks
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
            foreach (CheckList checkList in this.checks)
            {
                if (checkList.CheckName == description.CheckName)
                {
                    checkList.AddCheck(description);
                    return;
                }
            }

            var toAdd = new CheckList();
            toAdd.CheckName = description.CheckName;
            toAdd.AddCheck(description);
            this.checks.Add(toAdd);
        }

        #endregion
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
            foreach (TypeChecks typeCheckse in this.runDescription)
            {
                if (typeCheckse.CheckedType == desc.CheckedType)

                {
                    typeCheckse.AddCheck(desc);
                    return;
                }
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