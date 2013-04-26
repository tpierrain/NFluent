// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunnerHelper.cs" company="">
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using NUnit.Framework;
    
    /// <summary>
    /// Hosts several methods helping to execute unit test in a controlled fashion.
    /// </summary>
    public static class RunnerHelper
    {
        private static bool inProcess;

        public static CheckDescription AnalyzeSignature(MethodBase method)
        {
            var result = new CheckDescription { Check = method };

            if (method.IsStatic)
            {
                // check if this is an extension method
                if (method.GetCustomAttributes(typeof(ExtensionAttribute), false).Length > 0)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    ParameterInfo param = parameters[0];
                    Type paramType = param.ParameterType;
                    if (!paramType.IsGenericType)
                    {
                        // this is not an check implementation
                        return null;
                    }

                    if (paramType.Name != "IFluentAssertion`1" && paramType.GetInterface("IFluentAssertion`1") == null)
                    {
                        // this is not an check implementation
                        return null;
                    }

                    // identify type subjected to test
                    var testedtype = paramType.GetGenericArguments()[0];
                    result.CheckedType = testedtype;

                    // get other parameters
                    result.CheckParameters = new List<Type>(parameters.Length - 1);
                    for (int i = 1; i < parameters.Length; i++)
                    {
                        result.CheckParameters.Add(parameters[i].ParameterType);
                    }
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
                Type scanning = method.DeclaringType.GetInterface("IFluentAssertion`1");
                if (scanning != null && scanning.IsGenericType)
                {
                    // the type implements IFluentAssertion<T>
                    result.CheckedType = scanning.IsGenericType ? scanning.GetGenericArguments()[0] : null;

                    // get other parameters
                    result.CheckParameters = new List<Type>();
                    foreach (ParameterInfo t in method.GetParameters())
                    {
                        result.CheckParameters.Add(t.ParameterType);
                    }
                }
                else
                {
                    // type does not implement IFluentAssertion<T>, we try to find a 'Value' property
                    var prop = method.DeclaringType.GetProperty("Value");
                    if (prop != null)
                    {
                        result.CheckedType = prop.PropertyType;

                        // get other parameters
                        result.CheckParameters = new List<Type>();
                        foreach (ParameterInfo t in method.GetParameters())
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

        public static void RunThoseTests(IEnumerable<MethodInfo> tests, Type type, FullRunDescription report)
        {
            var specificTests = tests as IList<MethodInfo> ?? tests.ToList();
            if (specificTests.Count > 0)
            {
                Log(string.Format("TestFixture :{0}", type.FullName));

                // creates an instance
                var test = type.InvokeMember(
                    string.Empty,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance,
                    null,
                    null,
                    new object[] { });

                // run TestFixtureSetup
                RunAllSpecificMethods(type, typeof(TestFixtureSetUpAttribute), test);

                try
                {
                    // run all tests
                    foreach (var specificTest in specificTests.Where(
                        specificTest => specificTest.GetCustomAttributes(typeof(ExplicitAttribute), false).Length == 0 
                        && specificTest.GetCustomAttributes(typeof(IgnoreAttribute), false).Length == 0))
                    {
                        RunAllSpecificMethods(type, typeof(SetUpAttribute), test);

                        try
                        {
                            // we have to caputre eceptions
                            RunMethod(specificTest, test, report);
                        }
                        finally
                        {
                            RunAllSpecificMethods(type, typeof(TearDownAttribute), test);
                        }
                    }
                }
                finally
                {
                    RunAllSpecificMethods(type, typeof(TestFixtureTearDownAttribute), test);
                }
            }
        }

        public static void RunMethod(MethodInfo specificTest, object test, FullRunDescription report)
        {
            try
            {
                specificTest.Invoke(test, new object[] { });
            }
            catch (Exception e)
            {
                if (specificTest.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false).Length == 0)
                {
                    throw;
                }

                Type expectedType =
                    ((ExpectedExceptionAttribute)
                     specificTest.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false)[0]).ExpectedException;
                if (e.InnerException != null)
                {
                    if (e.InnerException is FluentAssertionException)
                    {
                        var fluExc = e.InnerException as FluentAssertionException;
                        Type testedtype;
                        CheckDescription desc = GetCheckAndType(fluExc);
                        MethodBase method = desc.Check;
                        testedtype = desc.CheckedType;
                        desc.ErrorSampleMessage = fluExc.Message;

                        // are we building a report
                        if (report != null)
                        {
                            Log(
                                string.Format(
                                    "Check.That({1} sut).{0} failure message\n****\n{2}\n****",
                                    method.Name,
                                    testedtype.Name,
                                    fluExc.Message));
                            report.AddEntry(desc);
                        }

                        return;
                    }

                    if (report != null)
                    {
                        Log(
                            string.Format(
                                "{0} did not generate a FLUENT ASSERTION:\n{1}",
                                specificTest.Name,
                                e.InnerException.Message));
                    }

                    if (e.InnerException.GetType() != expectedType && expectedType != null)
                    {
                        throw;
                    }
                }
                else
                {
                    if (report != null)
                    {
                        Log(string.Format("{0} failed to run:\n{1}", specificTest.Name, e));
                    }

                    throw;
                }
            }
        }

        public static void Log(string message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        /// Runs all tests found in current assembly.
        /// </summary>
        public static void RunAllTests()
        {
            // prevent recursion
            if (inProcess)
            {
                return;
            }

            try
            {
                inProcess = true;

                // get all test fixtures
                foreach (var type in
                    Assembly.GetExecutingAssembly().GetTypes())
                {
                    // enumerate testmethods with expectedexception attribute with an FluentException type
                    IEnumerable<MethodInfo> tests =
                        type.GetMethods().Where(method => method.GetCustomAttributes(typeof(TestAttribute), false).Length > 0);
                    RunThoseTests(tests, type, null);
                }
            }
            finally
            {
                inProcess = false;
            }
        }

        private static CheckDescription GetCheckAndType(FluentAssertionException fluExc)
        {
            // identify failing test
            var trace = new StackTrace(fluExc);

            // get fluententrypoint stackframe
            int frameIndex = trace.FrameCount - 2;
            if (frameIndex < 0)
            {
                frameIndex = 0;
            }

            StackFrame frame = trace.GetFrame(frameIndex);

            // get method
            var method = frame.GetMethod();

            return AnalyzeSignature(method);
        }

        private static void RunAllSpecificMethods(Type type, Type attributeTypeToScan, object test)
        {
            IEnumerable<MethodInfo> startup =
                type.GetMethods().Where(method => method.GetCustomAttributes(attributeTypeToScan, false).Length > 0);
            foreach (var methodInfo in startup)
            {
                try
                {
                    methodInfo.Invoke(test, new object[] { });
                }
                catch (Exception e)
                {
                    Log(string.Format("Error: {0} failed, {1}", methodInfo.Name, e.Message));
                }
            }
        }
    }
}