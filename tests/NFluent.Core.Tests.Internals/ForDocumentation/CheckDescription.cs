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
// ReSharper disable once CheckNamespace
namespace NFluent.Tests.ForDocumentation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Serialization;
    using Extensions;

//    [Serializable]
    public class CheckDescription
    {
        #region field

        private string entryPoint;

        #endregion

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
                string methodName;
                if (this.Check.IsGenericMethod)
                {
                    var generic = ((MethodInfo)this.Check).GetGenericMethodDefinition();
                    StringBuilder builder = new StringBuilder(generic.Name);
                    builder.Append('<');
                    bool firstDone = false;
                    foreach (var genericArgument in generic.GetGenericArguments())
                    {
                        if (firstDone)
                        {
                            builder.Append(", ");
                        }
                        else
                        {
                            firstDone = true;
                        }

                        builder.Append(genericArgument.Name);
                    }

                    builder.Append('>');
                    methodName = builder.ToString();
                }
                else
                {
                    methodName = this.Check.Name;
                }

                var checkParameters = this.CheckParameters;
                if (checkParameters != null)
                {
                    var parameters = new StringBuilder(checkParameters.Count * 20);

                    // build parameter list
                    if (checkParameters.Count > 0)
                    {
                        parameters.Append(checkParameters[0].TypeToStringProperlyFormatted(true));
                        for (var i = 1; i < checkParameters.Count; i++)
                        {
                            parameters.Append(", ");
                            parameters.Append(checkParameters[i].TypeToStringProperlyFormatted(true));
                        }
                    }

                    return string.Format("Check.{3}({0} sut).{1}({2})", this.CheckedType.TypeToStringProperlyFormatted(true), methodName, parameters, this.entryPoint);
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

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string ErrorSampleMessage { get; set; }
        #endregion

        public static CheckDescription AnalyzeSignature(MethodBase method)
        {
            var result = new CheckDescription { Check = method };

            if (method.IsStatic)
            {
                // check if this is an extension method
                var customAttributes = method.GetCustomAttributes(typeof(ExtensionAttribute), false);
                int count = 0;
                foreach (var unused in customAttributes)
                {
                    count++;
                }
                if (count > 0)
                {
                    var parameters = method.GetParameters();
                    var param = parameters[0];
                    var paramType = param.ParameterType;
                    if (!paramType.GetTypeInfo().IsGenericType)
                    {
                        // this is not an check implementation
                        return null;
                    }

                    // if it is specific to chains
                    if (paramType.Name == "ICheckLink`1")
                    {
                        paramType = paramType.GetGenericArguments()[0];
                    }

                    if (paramType.Name == "IExtendableCheckLink`1"
                        || paramType.Name == "IExtendableCheckLink`2"
                        || paramType.Name == "ICheck`1"
                        || paramType.GetTypeInfo().GetInterface("ICheck`1") != null
                        || paramType.Name == "IStructCheck`1"
                        || paramType.Name == "ICodeCheck`1")
                    {
                        result.entryPoint = paramType.Name == "IStructCheck`1" ? "ThatEnum" : "That";

                        var testedtype = paramType.GetGenericArguments()[0];
                        if (testedtype.IsGenericParameter)
                        {
                            testedtype = testedtype.GetTypeInfo().BaseType;
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
                result.entryPoint = "That";
                if (method.DeclaringType == null)
                {
                    return null;
                }

                // this is an instance method, tested type is part of type defintion
                Type scanning = method.DeclaringType.GetTypeInfo().GetInterface("ICheck`1");
                if (scanning != null && scanning.GetTypeInfo().IsGenericType)
                {
                    // the type implements ICheck<T>
                    result.CheckedType = scanning.GetTypeInfo().IsGenericType ? scanning.GetGenericArguments()[0] : null;

                    if (result.CheckedType.GetTypeInfo().IsGenericType)
                    {
                        result.CheckedType = result.CheckedType.GetTypeInfo().BaseType;
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
                        // ReSharper disable once RedundantStringFormatCall
                        Debug.WriteLine(string.Format("Type {0} needs to implement a Value property (method {1})", method.DeclaringType.Name, method.Name));
                    }
                }
            }

            return result;
        }
    }
}