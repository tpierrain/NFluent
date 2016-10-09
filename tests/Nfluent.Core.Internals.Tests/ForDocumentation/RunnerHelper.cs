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
    using NUnit.Framework;
#if CORE
    using Microsoft.Extensions.DependencyModel;
#endif

    /// <summary>
    /// Hosts several methods helping to execute unit tests in a controlled fashion.
    /// </summary>
    public static class RunnerHelper
    {
        private static bool inProcess;

        public static void RunThoseTests(IEnumerable<MethodInfo> tests, Type type, FullRunDescription report, bool log)
        {
            var specificTests = tests as IList<MethodInfo> ?? tests.ToList();
            if (specificTests.Count <= 0)
            {
                return;
            }

            Log(string.Format("TestFixture :{0}", type.FullName));
            var constructor = type.GetTypeInfo().GetConstructor(new Type[0]);
            // creates an instance
            var test = constructor.Invoke(new Type[0]);

            // run TestFixtureSetup
            RunAllMethodsWithASpecificAttribute(type, typeof(OneTimeSetUpAttribute), test);

            try
            {
                // run all tests
                foreach (var specificTest in specificTests.Where(
                    specificTest => !specificTest.GetCustomAttributes(typeof(ExplicitAttribute), false).Any() 
                                    && !specificTest.GetCustomAttributes(typeof(IgnoreAttribute), false).Any()))
                {
                    RunAllMethodsWithASpecificAttribute(type, typeof(SetUpAttribute), test);

                    try
                    {
                        // we have to caputre eceptions
                        RunMethod(specificTest, test, report, log);
                    }
                    finally
                    {
                        RunAllMethodsWithASpecificAttribute(type, typeof(TearDownAttribute), test);
                    }
                }
            }
            finally
            {
                RunAllMethodsWithASpecificAttribute(type, typeof(OneTimeTearDownAttribute), test);
            }
        }

        public static void RunAction(Action action)
        {
            RunMethod(action.GetMethodInfo().GetBaseDefinition(), action.Target, null, false);
        }

        internal static Assembly[] GetLoadedAssemblies()
        {
#if CORE
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (!IsCandidateCompilationLibrary(library)) continue;
                var assembly = Assembly.Load(new AssemblyName(library.Name));
                assemblies.Add(assembly);
            }
            return assemblies.ToArray();
        }

        private static bool IsCandidateCompilationLibrary(Library compilationLibrary)
        {
            return compilationLibrary.Name == ("Specify")
                || compilationLibrary.Dependencies.Any(d => d.Name.StartsWith("Specify"));
        }

#else
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// Polyfills for TypeInfo
        /// </summary>
        /// <param name="type"><see cref="Type"/> of interest.</param>
        /// <returns>Related type infos</returns>
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }

        /// <summary>
        /// Polyfill for GetMthodInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(this Action info)
        {
            return info.Method;
        }
#endif

        private static void RunMethod(MethodBase specificTest, object test, FullRunDescription report, bool log)
        {
            try
            {
                specificTest.Invoke(test, new object[] { });
            }
            catch (Exception e)
            {
                throw;
               // throw new NotImplementedException("we have to find a new way since NUnit's ExceptedException has disapeared.");

                //if (specificTest.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false).Length == 0)
                //{
                //    if (CheckContext.DefaulNegated == false)
                //    {
                //        return;
                //    }

                //    throw;
                //}

                //Type expectedType = ((ExpectedExceptionAttribute) specificTest.GetCustomAttributes(typeof(ExpectedExceptionAttribute), false)[0]).ExpectedException;
                //if (e.InnerException != null)
                //{
                //    if (e.InnerException is FluentCheckException)
                //    {
                //        var fluExc = e.InnerException as FluentCheckException;
                //        var desc = GetCheckAndType(fluExc);
                //        if (desc != null)
                //        {
                //            var method = desc.Check;
                //            var testedtype = desc.CheckedType;
                //            desc.ErrorSampleMessage = fluExc.Message;

                //            // are we building a report
                //            if (log)
                //            {
                //                Log(
                //                    string.Format(
                //                        "Check.That({1} sut).{0} failure message" + Environment.NewLine + "****" + Environment.NewLine + "{2}" + Environment.NewLine + "****",
                //                        method.Name,
                //                        testedtype.Name,
                //                        fluExc.Message));
                //            }

                //            if (report != null)
                //            {
                //                report.AddEntry(desc);
                //            }

                //            if (CheckContext.DefaulNegated == false)
                //            {
                //                Log(string.Format("(Forced) Negated test '{0}' should have succeeded, but it failed (method {1}).", specificTest.Name, desc.Signature));
                //            }
                //        }
                //        else
                //        {
                //            Log(string.Format("Failed to parse the method signature {0}", specificTest.Name));
                //        }

                //        return;
                //    }

                //    if (report != null)
                //    {
                //        Log(
                //            string.Format(
                //                "{0} did not generate a fluent check:" + Environment.NewLine + "{1}",
                //                specificTest.Name,
                //                e.InnerException.Message));
                //    }

                //    if (e.InnerException.GetType() != expectedType && expectedType != null)
                //    {
                //        throw;
                //    }
                //}
                //    else
                //    {
                //        if (report != null)
                //        {
                //            Log(string.Format("{0} failed to run:" + Environment.NewLine + "{1}", specificTest.Name, e));
                //        }

                //        throw;
                //    }
            }
        }

        public static void Log(string message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        /// Runs all tests found in current assembly.
        /// </summary>
        /// <param name="log">if set to <c>true</c> log the activity (to Debug output).</param>
        public static void RunAllTests(bool log)
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
                foreach (var type in typeof(RunnerHelper).GetTypeInfo().Assembly.GetTypes())
                {
                    // enumerate testmethods with expectedexception attribute with an FluentException type
                    var tests =
                        type.GetMethods().Where(method => method.GetCustomAttributes(typeof(TestAttribute), false).Any());
                    RunThoseTests(tests, type, null, log);
                }

            }
            finally
            {
                inProcess = false;
            }
        }

        private static CheckDescription GetCheckAndType(FluentCheckException fluExc)
        {
            // identify failing test
            var trace = new StackTrace(fluExc, true);

            // get fluententrypoint stackframe
            var frameIndex = trace.GetFrames().Count() - 2;
            if (frameIndex < 0)
            {
                frameIndex = 0;
            }

            var frame = trace.GetFrames()[frameIndex];

            // get method
            var method = frame.GetMethod();

            return CheckDescription.AnalyzeSignature(method);
        }

        private static void RunAllMethodsWithASpecificAttribute(Type type, Type attributeTypeToScan, object test)
        {
            IEnumerable<MethodInfo> startup =
                type.GetMethods().Where(method => method.GetCustomAttributes(attributeTypeToScan, false).Any());
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