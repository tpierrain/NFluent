// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="GenerateErrorMessages.cs" company="">
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
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using NUnit.Framework;

    [TestFixture]
    public class GenerateErrorMessages
    {
        // run this test to debug a specific test that the code is unable to properly identify
        [Test]
        [Ignore("Use to debug dectetion when failing.")]
        public void SpecificTest()
        {
            var test = new LambdaRelatedTests();

            // test.SetUp();
            MethodInfo method = test.GetType()
                                    .GetMethod(
                                        "FailDurationTest",
                                        BindingFlags.Instance | BindingFlags.Public);
            if (method != null)
            {
                this.RunMethod(method, test);
            }

            // test.TearDown();
        }

        // Run this test to get all error messages
        /* Algo is:
         * - Get all Types from this assembly and with a TestFixture Attribute
         * - For each of those types:
         *   - find all methods with Test and ExpectedExceptionAttribute with a FluentException type (if precised)
         *   - if any is found, run all methods with TestFixtureSetUp and attributes
         *   - each test is run, an error is logged is no fluentexception is raised
         *     - Setup and TearDown methods are run (if any)
         *   - exception is analyzed to identify the related test
         *   - all TestFixtureTeardown are run
         */
        [Test]
        [Explicit("Scan all assemblies, execute tests and generate a report for error messages.")]
        public void ScanUnitTestsAndGenerateReport()
        {
            var report = new FullRunDescription();
     
            // get all test fixtures
            foreach (
                var type in
                    Assembly.GetExecutingAssembly()
                            .GetTypes())
            {
                try
                {
                    // enumerate testmethods with expectedexception attribute with an FluentException type
                    IEnumerable<MethodInfo> tests =
                        type.GetMethods()
                            .Where(method => method.GetCustomAttributes(typeof(TestAttribute), false).Length > 0)
                            .Where(
                                method =>
                                    {
                                        object[] attributes =
                                            method.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false);
                                        if (attributes.Length == 1)
                                        {
                                            var attrib =
                                                attributes[0] as ExpectedExceptionAttribute;
                                            return attrib == null
                                                   || attrib.ExpectedException == typeof(FluentAssertionException);
                                        }

                                        return false;
                                    });
                    var specificTests = tests as IList<MethodInfo> ?? tests.ToList();
                    if (specificTests.Count > 0)
                    {
                        this.Log(string.Format("TestFixture :{0}", type.FullName));

                        // creates an instance
                        var test = type.InvokeMember(
                            string.Empty,
                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance,
                            null,
                            null,
                            new object[] { });

                        // run TestFixtureSetup
                        this.RunAllSpecificMethods(type, typeof(TestFixtureSetUpAttribute), test);

                        // run all tests
                        foreach (var specificTest in specificTests)
                        {
                            if (specificTest.GetCustomAttributes(typeof(ExplicitAttribute), false).Length > 0
                                || specificTest.GetCustomAttributes(typeof(IgnoreAttribute), false).Length > 0)
                            {
                                // test must be skipped
                            }

                            this.RunAllSpecificMethods(type, typeof(SetUpAttribute), test);

                            var result = this.RunMethod(specificTest, test);
                            if (result != null)
                            {
                                report.AddEntry(result);
                            }

                            this.RunAllSpecificMethods(type, typeof(TearDownAttribute), test);
                        }

                        this.RunAllSpecificMethods(type, typeof(TestFixtureTearDownAttribute), test);
                    }
                }
                catch (Exception e)
                {
                    this.Log(string.Format("Exception while working on type:{0}\n{1}", type.FullName, e));
                }
            }

            const string Name = "FluentReport.xml";
            report.Save(Name);
            Debug.Write(string.Format("Report generated in {0}", Path.GetFullPath(Name)));
        }

        [Test]
        [Explicit("Scan all assemblies and generate a report listing existing cheks.")]
        public void ScanAssembliesForCheckAndGenerateReport()
        {
            var report = new FullRunDescription();

            // scan all assemblies
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // get all test fixtures
                foreach (var type in
                    assembly.GetTypes()
                            .Where(
                                type =>
                                ((type.GetInterface("IFluentAssertionBase") != null
                                  && (type.Attributes & TypeAttributes.Abstract) == 0)
                                 || type.GetCustomAttributes(typeof(ExtensionAttribute), false).Length > 0)
                                && ((type.Attributes & TypeAttributes.Public) == TypeAttributes.Public)))
                {
                    try
                    {
                        // enumerate public methods
                        IEnumerable<MethodInfo> tests =
                            type.GetMethods(
                                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
                                | BindingFlags.DeclaredOnly)
                                .Where(
                                    method =>
                                    method.MemberType == MemberTypes.Method
                                    && ((method.Attributes & MethodAttributes.SpecialName) == 0));
                        var publicMethods = tests as IList<MethodInfo> ?? tests.ToList();
                        if (publicMethods.Count > 0)
                        {
                            // run all tests
                            foreach (var checkMethod in publicMethods)
                            {
                                CheckDescription desc = AnalyzeSignature(checkMethod);

                                if (desc != null)
                                {
                                    this.Log(string.Format("Method :{0}.{1}({2})", type.Name, checkMethod.Name, desc.CheckedType.Name));
                                    report.AddEntry(desc);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        this.Log(string.Format("Exception while working on type:{0}\n{1}", type.FullName, e));
                    }
                }
            }

            const string Name = "FluentChecks.xml";
            report.Save(Name);
            Debug.Write(string.Format("Report generated in {0}", Path.GetFullPath(Name)));
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

        private static CheckDescription AnalyzeSignature(MethodBase method)
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
                // this is an instance method, tested type is part of type defintion
                Debug.Assert(method.DeclaringType != null, "method.DeclaringType != null");
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

        private CheckDescription RunMethod(MethodInfo specificTest, object test)
        {
            try
            {
                specificTest.Invoke(test, new object[] { });
            }
            catch (Exception e)
            {
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
                        
                        this.Log(string.Format("Check.That({1} sut).{0} failure message\n****\n{2}\n****", method.Name, testedtype.Name, fluExc.Message));
                        return desc;
                    }

                        this.Log(string.Format("{0} did not generate a FLUENT ASSERTION:\n{1}", specificTest.Name, e.InnerException.Message));
                    return null;
                }

                this.Log(string.Format("{0} failed to run:\n{1}", specificTest.Name, e));
                return null;
            }

            return null;
        }

        private void RunAllSpecificMethods(Type type, Type attributeTypeToScan, object test)
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
                    this.Log(string.Format("Error: {0} failed, {1}", methodInfo.Name, e.Message));
                }
            }
        }

        private void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}