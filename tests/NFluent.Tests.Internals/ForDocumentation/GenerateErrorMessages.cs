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

// ReSharper disable once CheckNamespace
namespace NFluent.Tests.ForDocumentation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class GenerateErrorMessages
    {
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
        [Ignore("Scan all assemblies, execute tests and generate a report for error messages.")]
        public void ScanUnitTestsAndGenerateReport()
        {
            var report = RunFailingTests(true);

            const string name = "FluentReport.xml";
            report.Save(name);
            Debug.Write(string.Format("Report generated in {0}", Path.GetFullPath(name)));
        }

        [Test]
        [Ignore("Assess error message quality")]
        public void ScanUnitTestsAndAssessMessages()
        {
            var report = RunFailingTests(false);
            foreach (var typeChecks in report.RunDescription)
            {
                foreach (var check in typeChecks.Checks)
                {
                    foreach (var checkDescription in check.CheckSignatures)
                    {
                        string error;
                        if (!CheckMessage(checkDescription.ErrorSampleMessage, out error))
                        {
                            // failing
                            RunnerHelper.Log(string.Format("Error for {0}: {1}", checkDescription.Signature, error));
                            RunnerHelper.Log(checkDescription.ErrorSampleMessage);
                            break;
                        }
                    }
                }
            }
        }

        internal static bool CheckMessage(string message, out string error)
        {
            var lines = message.Split('\n');
            if (lines.Length == 0 && string.IsNullOrEmpty(lines[0]))
            {
                // failing
                error = "empty message!";
                return false;
            }

            if (!lines[1].ToLowerInvariant().Contains("checked"))
            {
                // failing
                error = "message first line must contain 'checked'.";
                return false;
            }

            error = string.Empty;
            return true;
        }

        [Test]
        [Ignore("Scan all assemblies and generate a report listing existing cheks.")]
        public void ScanAssembliesForCheckAndGenerateReport()
        {
            var report = new FullRunDescription();

            // scan all assemblies
            foreach (var assembly in RunnerHelper.GetLoadedAssemblies())
            {
                if (assembly.GetName().Name == "MonoDevelop.NUnit")
                {
                    // skip MonoDevelop.NUnit assembly because GetTypes fails
                    continue;
                }

                try
                {
                    // get all test fixtures
                    foreach (var type in
                        assembly.GetTypes()
                            .Where(
                                    type =>
                                    ((type.GetTypeInfo().GetInterface("IForkableCheck") != null
                                    && (type.GetTypeInfo().Attributes & TypeAttributes.Abstract) == 0)
                                    || type.GetTypeInfo().GetCustomAttributes(typeof(ExtensionAttribute), false).Any())
                                    && ((type.GetTypeInfo().Attributes & TypeAttributes.Public) == TypeAttributes.Public)))
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
                                // scan all methods
                                foreach (var checkMethod in publicMethods)
                                {
                                    try
                                    {
                                        if (checkMethod.Name == "ForkInstance")
                                        {
                                            // skip forkinstance
                                            continue;
                                        }

                                        var desc = CheckDescription.AnalyzeSignature(checkMethod);

                                        if (desc != null)
                                        {
                                            if (desc.CheckedType == null)
                                            {
                                                RunnerHelper.Log(string.Format("Failed to identify checked type on test {0}", checkMethod.Name));
                                            }
                                            else
                                            {
                                                RunnerHelper.Log(string.Format("Method :{0}.{1}({2})", type.Name, checkMethod.Name, desc.CheckedType.Name));
                                                report.AddEntry(desc);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        RunnerHelper.Log(string.Format("Exception while assessing test {2} on type:{0}" + Environment.NewLine + "{1}", type.FullName, e, checkMethod.Name));
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            RunnerHelper.Log(string.Format("Exception while working on type:{0}" + Environment.NewLine + "{1}", type.FullName, e));
                        }
                    }
                }
                catch (Exception e)
                {
                    RunnerHelper.Log(string.Format("Exception while working on assembly:{0}" + Environment.NewLine + "{1}", assembly.FullName, e));
                    throw new Exception(string.Format("Exception while working on assembly:{0}" + Environment.NewLine + "{1}", assembly.FullName, e));
                }
            }

            // xml save
            // ReSharper disable once InconsistentNaming
            const string Name = "FluentChecks.xml";
            report.Save(Name);

            const string name2 = "FluentChecks.csv";

            using (var exportFile = File.Create(name2))
            {
                // csv file
                using (var writer = new StreamWriter(exportFile))
                {
                    foreach (var typeChecks in report.RunDescription)
                    {
                        foreach (var checkList in typeChecks.Checks)
                        {
                            foreach (var signature in checkList.CheckSignatures)
                            {
                                var message = string.Format(
                                    "{0};{1};{2}", typeChecks.CheckedType.TypeToStringProperlyFormatted(true),
                                    checkList.CheckName, signature.Signature);
                                writer.WriteLine(message);
                            }
                        }
                    }
                }
            }

            Debug.Write(string.Format("Report generated in {0}", Path.GetFullPath(Name)));
                Debug.Write(string.Format("Report generated in {0}", Path.GetFullPath(name2)));

        }

        // run a set of test
        private static FullRunDescription RunFailingTests(bool log)
        {
            var report = new FullRunDescription();

            // get all test fixtures
            foreach (var type in typeof(GenerateErrorMessages).GetTypeInfo().Assembly.GetExportedTypes())
            {
                try
                {
                    // enumerate testmethods with expectedexception attribute with an FluentException type
                    var tests =
                        type.GetMethods()
                            .Where(method => method.GetCustomAttributes(typeof(TestAttribute), false).Any())
                            .Where(
                                method =>
                                {
                                    //var attributes = method.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false);
                                    //if (attributes.Length == 1)
                                    //{
                                    //    var attrib = attributes[0] as ExpectedExceptionAttribute;
                                    //    return attrib == null || attrib.ExpectedException == typeof(FluentCheckException);
                                    //}
                                    throw new NotImplementedException("we have to find a new way since NUnit's ExceptedException has disapeared.");
                                });
                    RunnerHelper.RunThoseTests(tests, type, report, log);
                }
                catch (Exception e)
                {
                    RunnerHelper.Log(string.Format("Exception while working on type:{0}" + Environment.NewLine + "{1}", type.FullName, e));
                }
            }

            return report;
        }
    }
}