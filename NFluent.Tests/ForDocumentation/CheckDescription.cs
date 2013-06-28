﻿// // --------------------------------------------------------------------------------------------------------------------
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
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;
    using NFluent.Extensions;

    [Serializable]
    public class CheckDescription
    {
        private string entryPoint;
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
                        for (var i = 1; i < checkParameters.Count; i++)
                        {
                            parameters.Append(", ");
                            parameters.Append(checkParameters[i].Name);
                        }
                    }

                    return string.Format("Check.{3}({0} sut).{1}({2})", this.CheckedType.ToStringProperlyFormated(), this.Check.Name, parameters, this.entryPoint);
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

        public static CheckDescription AnalyzeSignature(MethodBase method)
        {
            var result = new CheckDescription { Check = method };

            if (method.IsStatic)
            {
                // check if this is an extension method
                if (method.GetCustomAttributes(typeof(ExtensionAttribute), false).Length > 0)
                {
                    var parameters = method.GetParameters();
                    var param = parameters[0];
                    var paramType = param.ParameterType;
                    if (!paramType.IsGenericType)
                    {
                        // this is not an check implementation
                        return null;
                    }

                    // if it is specific to chains
                    if (paramType.Name == "ICheckLink`1")
                    {
                        paramType = paramType.GetGenericArguments()[0];
                    }

                    if (paramType.Name == "IExtendableCheckLink`1" || paramType.Name == "ICheck`1"
                        || paramType.GetInterface("ICheck`1") != null
                        || paramType.Name == "IStructCheck`1")
                    {
                        if (paramType.Name == "IStructCheck`1")
                        {
                            result.entryPoint = "ThatEnum";
                        }
                        else
                        {
                            result.entryPoint = "That";
                        }

                        var testedtype = paramType.GetGenericArguments()[0];
                        if (testedtype.IsGenericParameter)
                        {
                            testedtype = testedtype.BaseType;
                        }

                        result.CheckedType = testedtype;

                        // get other parameters
                        result.CheckParameters = new List<Type>(parameters.Length - 1);
                        for (var i = 1; i < parameters.Length; i++)
                        {
                            result.CheckParameters.Add(parameters[i].ParameterType);
                        }
                    }
                    else
                    {
                        // this is not an check implementation
                        return null;
                    }

                    // identify type subjected to test
                }
                else
                {
                    // unexpected case: check is a static method which is not an extension method
                    return null;
                }
            }
            else
            {
                if (method.DeclaringType == null)
                {
                    return null;
                }

                // this is an instance method, tested type is part of type defintion
                Type scanning = method.DeclaringType.GetInterface("ICheck`1");
                if (scanning != null && scanning.IsGenericType)
                {
                    // the type implements ICheck<T>
                    result.CheckedType = scanning.IsGenericType ? scanning.GetGenericArguments()[0] : null;

                    if (result != null && result.CheckedType.IsGenericType)
                    {
                        result.CheckedType = result.CheckedType.BaseType;
                    }

                    // get other parameters
                    result.CheckParameters = new List<Type>();
                    foreach (var t in method.GetParameters())
                    {
                        result.CheckParameters.Add(t.ParameterType);
                    }
                }
                else
                {
                    // type does not implement ICheck<T>, we try to find a 'Value' property
                    var prop = method.DeclaringType.GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null)
                    {
                        result.CheckedType = prop.PropertyType;

                        // get other parameters
                        result.CheckParameters = new List<Type>();
                        foreach (var t in method.GetParameters())
                        {
                            result.CheckParameters.Add(t.ParameterType);
                        }
                    }
                    else
                    {
                        // not a check method
                        Debug.WriteLine(string.Format("Type {0} needs to implement a Value property (method {1})", method.DeclaringType.Name, method.Name));
                    }
                }
            }

            return result;
        }
    }
}