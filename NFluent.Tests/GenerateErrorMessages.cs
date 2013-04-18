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
namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using NUnit.Framework;

    [TestFixture]
    public class GenerateErrorMessages
    {

        private void Log(string message)
        {
            Debug.WriteLine(message);
        }
        [Test]
        [Explicit]
        public void ScanAndGenerate()
        {
            // get all test fixtures
            foreach (
                var type in
                    Assembly.GetExecutingAssembly()
                            .GetTypes()
                            .Where((type) => type.GetCustomAttributes(typeof(TestFixtureAttribute), false).Length > 0))
            {
                try
                {
                    // enumerate testmethods with expected exception flags
                    IEnumerable<MethodInfo> tests =
                        type.GetMethods()
                            .Where((method) => method.GetCustomAttributes(typeof(TestAttribute), false).Length > 0)
                            .Where(
                                (method) =>
                                    { object [] attributes=
                                            method.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false);
                                        if (attributes.Length == 1)
                                        {
                                            var attrib =
                                                attributes[0] as ExpectedExceptionAttribute;
                                            return attrib.ExpectedException == typeof(FluentAssertionException);
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    });
                    if (tests.Any())
                    {
                        Log(string.Format("TestFixture :{0}", type.FullName));
                        // creates an instance
                        object Test = type.InvokeMember(
                            "TestClass",
                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance,
                            null, null, new object[] { });

                        // run TestFixtureSetup
                        this.RunAllSpecificMethods(type, typeof(TestFixtureSetUpAttribute), Test);
                        // run all tests
                        foreach (var specificTest in tests)
                        {
                            this.RunAllSpecificMethods(type, typeof(SetUpAttribute), Test);
 
                            try
                            {
                                specificTest.Invoke(Test, new object[] { });
                            }
                            catch (Exception e)
                            {
                                if (e.InnerException != null)
                                {
                                    if (e.InnerException is FluentAssertionException)
                                    {
                                        this.Log(string.Format("{0} generated:\n{1}", specificTest.Name, e.InnerException.Message));

                                    }
                                    else
                                    {
                                        this.Log(string.Format("{0} did not generate a FLUENT ASSERTION:\n{1}", specificTest.Name, e.InnerException.Message));

                                    }
                                }
                                else
                                {
                                    this.Log(string.Format("{0} failed to run:\n{1}", specificTest.Name, e));
                                }
                            }
                            this.RunAllSpecificMethods(type, typeof(TearDownAttribute), Test);
                            
                        }
                        this.RunAllSpecificMethods(type, typeof(TestFixtureTearDownAttribute), Test);
                    }

                }
                catch (Exception e)
                {
                    Log(string.Format("Exception while working on type:{0}\n{1}", type.FullName, e));
                }
            }
        }

        private void RunAllSpecificMethods(Type type, Type attributeTypeToScan, object Test)
        {
            IEnumerable<MethodInfo> startup =
                type.GetMethods().Where((method) => method.GetCustomAttributes(attributeTypeToScan, false).Length > 0);
            foreach (var methodInfo in startup)
            {
                try
                {
                    methodInfo.Invoke(Test, new object[] { });
                }
                catch (Exception e)
                {
                    this.Log(string.Format("Error: {0} failed, {1}", methodInfo.Name, e.Message));
                }
            }
        }
    }
}