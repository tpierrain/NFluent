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
        [Ignore("Use to debug detection when failing.")]
        public void SpecificTest()
        {
            RunnerHelper.RunAction(new ContainsChainedSyntaxTests().ContainsInThatOrderFails);
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
                    RunnerHelper.RunThoseTests(tests, type, report);
                }
                catch (Exception e)
                {
                    RunnerHelper.Log(string.Format("Exception while working on type:{0}\n{1}", type.FullName, e));
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
                                ((type.GetInterface("IForkableFluentAssertion") != null
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
                                CheckDescription desc = RunnerHelper.AnalyzeSignature(checkMethod);

                                if (desc != null)
                                {
                                    RunnerHelper.Log(string.Format("Method :{0}.{1}({2})", type.Name, checkMethod.Name, desc.CheckedType.Name));
                                    report.AddEntry(desc);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        RunnerHelper.Log(string.Format("Exception while working on type:{0}\n{1}", type.FullName, e));
                    }
                }
            }

            const string Name = "FluentChecks.xml";
            report.Save(Name);
            Debug.Write(string.Format("Report generated in {0}", Path.GetFullPath(Name)));
        }

        // run a set of test
    }
}