// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionHelper.cs" company="">
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
#if !(PORTABLE)
namespace NFluent.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    // ncrunch: no coverage start

    /// <summary>
    /// Offer factory services to get adequate exception type depending on testing framework.
    /// </summary>
    public static class ExceptionHelper
    {
        private static ExceptionConstructor constructors;

        private static ExceptionConstructor Constructors
        {
            get
            {
                if (constructors == null)
                {
                    // we need to identiy required exception types
                    var defaultConstructor = typeof(FluentCheckException).GetConstructor(new[] { typeof(string) });
                    var result = new ExceptionConstructor();
                    result.FailedException = defaultConstructor;
                    result.IgnoreException = defaultConstructor;
                    result.InconclusiveException = defaultConstructor;
    
                    // assert we have a default constructor
                    Debug.Assert(defaultConstructor != null, "NFluent exception must provide a constructor accepting a single string as parameter!");

                    // look for NUnit
                    var resultScan = ExceptionScanner("nunit", "NUnit.", "AssertionException", "IgnoreException", "InconclusiveException");
                    
                    if (resultScan == null)
                    {
                        // look for MSTest
                        resultScan = ExceptionScanner("visualstudio", "Microsoft.VisualStudio.TestTools", "AssertFailedException", null, "AssertInconclusiveException");
                    }

                    if (resultScan != null)
                    {
                        result = resultScan;
                    }

                    constructors = result;
                }

                return constructors;
            }
        }

        private static ExceptionConstructor ExceptionScanner(string assemblyMarker, string nameSpace, string assertionExceptionName, string ignoreExceptionName, string inconclusiveExceptionName)
        {
            int foundExceptions = 0;
            var result = new ExceptionConstructor();
            var defaultSignature = new[] { typeof(string) };
            foreach (
                var assembly in 
                    AppDomain.CurrentDomain.GetAssemblies()
                             .Where(ass => ass.FullName.ToLowerInvariant().Contains(assemblyMarker)))
            {
                foreach (var type in assembly.GetExportedTypes())
                {
                    if (type.Namespace.StartsWith(nameSpace))
                    {
                        if (type.Name == assertionExceptionName)
                        {
                            var info = type.GetConstructor(defaultSignature);
                            if (info != null)
                            {
                                result.FailedException = info;
                            }

                            foundExceptions++;
                        }
                        else if (type.Name == ignoreExceptionName)
                        {
                            var info = type.GetConstructor(defaultSignature);
                            if (info != null)
                            {
                                result.IgnoreException = info;
                            }

                            foundExceptions++;
                        }
                        else if (type.Name == inconclusiveExceptionName)
                        {
                            var info = type.GetConstructor(defaultSignature);
                            if (info != null)
                            {
                                // if we do not expect a ignore exception, we remap inconclusive
                                result.InconclusiveException = info;
                                foundExceptions++;
                                if (string.IsNullOrEmpty(ignoreExceptionName))
                                {
                                    result.IgnoreException = info;
                                    foundExceptions++;
                                }
                            }
                        }
                    }

                    // stop search if we found everything
                    if (foundExceptions == 3)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Builds an exception with the given message. Automatically detect the exception type to use depending on the used check framework.
        /// </summary>
        /// <param name="theMessage">The message to build the exception with.</param>
        /// <returns>An exception instance of the appropriate type with the given message.</returns>
        public static Exception BuildException(string theMessage)
        {
            return Constructors.FailedException.Invoke(new object[] { theMessage }) as Exception;
        }

        /// <summary>
        /// Stores adequate constructors.
        /// </summary>
        private class ExceptionConstructor
        {
            public ConstructorInfo FailedException { get; set; }

            public ConstructorInfo InconclusiveException { get; set; }

            public ConstructorInfo IgnoreException { get; set; }
        }
    }

    // ncrunch: no coverage end
}
#endif