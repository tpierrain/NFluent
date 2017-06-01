// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeCheckExtensions.cs" company="">
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
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Extensions;

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
                return this.CheckedType.TypeToStringProperlyFormatted(true);
            }

// ReSharper disable ValueParameterNotUsed
            set
// ReSharper restore ValueParameterNotUsed
            {
            }
        }

        [XmlIgnore]
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
            foreach (var checkList in this.checks)
            {
                if (checkList.CheckName == description.CheckName)
                {
                    checkList.AddCheck(description);
                    return;
                }
            }

            var toAdd = new CheckList { CheckName = description.CheckName };
            toAdd.AddCheck(description);
            this.checks.Add(toAdd);
        }

        #endregion
    }
}