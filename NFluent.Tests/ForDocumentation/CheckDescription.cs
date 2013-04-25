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
namespace NFluent.Tests.ForDocumentation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    public class CheckDescription
    {
        #region Public Properties

        [XmlIgnore]
        public string CheckName
        {
            get
            {
                return this.Check.Name;
            }
        }

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

            // ReSharper disable ValueParameterNotUsed
            set
            // ReSharper restore ValueParameterNotUsed
            {
            }
        }

        [XmlIgnore]
        public Type CheckedType { get; set; }

        [XmlIgnore]
        public MethodBase Check { get; set; }

        [XmlIgnore]
        public IList<Type> CheckParameters { get; set; }

        public string ErrorSampleMessage { get; set; }
        #endregion
    }
}