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
namespace NFluent.Helpers
{
    using System;
    using System.Diagnostics;
#if !PORTABLE && !NETSTANDARD1_3 && !DOTNET_30 && !DOTNET_20
    using System.Linq;
#endif
    using System.Reflection;
    using System.Text;

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
                if (constructors != null)
                {
                    return constructors;
                }

                // we need to identify required exception types
                var defaultConstructor = typeof(FluentCheckException).GetConstructor(new[] { typeof(string) });
                var result = new ExceptionConstructor
                                 {
                                     FailedException = defaultConstructor,
                                     IgnoreException = defaultConstructor,
                                     InconclusiveException = defaultConstructor
                                 };

                // assert we have a default constructor
                Debug.Assert(defaultConstructor != null, "NFluent exception must provide a constructor accepting a single string as parameter!");

                // look for MSTest
                var resultScan = ExceptionScanner("visualstudio", "Microsoft.VisualStudio.TestTools", "AssertFailedException", null, "AssertInconclusiveException")
                                 ?? ExceptionScanner( "nunit", "NUnit.", "AssertionException", "IgnoreException", "InconclusiveException")
                                 ?? ExceptionScanner("xunit.assert", "Xunit.Sdk", "XunitException", null, null);

                result = resultScan ?? result;
                constructors = result;

                return constructors;
            }
        }

        private static ExceptionConstructor ExceptionScanner(string assemblyMarker, string nameSpace, string assertionExceptionName, string ignoreExceptionName, string inconclusiveExceptionName)
        {
#if !(PORTABLE) && !(NETSTANDARD1_3)
            var missingExceptions = 3;
            var result = new ExceptionConstructor();
            var defaultSignature = new[] { typeof(string) };
            if (string.IsNullOrEmpty(ignoreExceptionName))
            {
                missingExceptions--;
            }

            if (string.IsNullOrEmpty(inconclusiveExceptionName))
            {
                missingExceptions--;
            }

            foreach (var assembly in
                    AppDomain.CurrentDomain.GetAssemblies()
                             .Where(ass => ass.FullName.ToLowerInvariant().Contains(assemblyMarker)))
            {
                var exportedTypes = assembly.GetExportedTypes();
                foreach (var type in exportedTypes)
                {
                    if (type.Namespace == null)
                    {
                        continue;
                    }

                    if (type.Namespace.StartsWith(nameSpace))
                    {
                        if (type.Name == assertionExceptionName)
                        {
                            var info = type.GetConstructor(defaultSignature);
                            result.FailedException = info;
                            missingExceptions--;
                        }
                        else if (type.Name == ignoreExceptionName)
                        {
                            var info = type.GetConstructor(defaultSignature);
                            result.IgnoreException = info;
                            missingExceptions--;
                        }
                        else if (type.Name == inconclusiveExceptionName)
                        {
                            var info = type.GetConstructor(defaultSignature);
                            result.InconclusiveException = info;
                            missingExceptions--;
                            if (string.IsNullOrEmpty(ignoreExceptionName))
                            {
                                // if we do not expect a ignore exception, we remap inconclusive
                                result.IgnoreException = info;
                            }
                       }
                    }

                    // stop search if we found everything
                    if (missingExceptions == 0)
                    {
                        return result;
                    }
                }
            }
#endif

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
        /// Checks if an object is an instance of a failed assertion exception.
        /// </summary>
        /// <param name="exc">Exception to check</param>
        /// <returns>true if the exception is of a correct type</returns>
        public static bool IsFailedException(object exc)
        {
            return exc!= null && (exc is FluentCheckException || (exc.GetType() == Constructors.FailedException.DeclaringType));
        }

        /// <summary>
        /// Stores adequate constructors.
        /// </summary>
        private class ExceptionConstructor
        {
            public ConstructorInfo FailedException { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public ConstructorInfo InconclusiveException { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public ConstructorInfo IgnoreException { get; set; }
        }

        // ncrunch: no coverage end

        /// <summary>
        /// Return a string containing the complete stack trace of the InnerExceptions for the given Exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>A string containing the complete stack trace of the InnerExceptions for the given Exception.</returns>
        public static string DumpInnerExceptionStackTrace(Exception exception)
        {
            var result = new StringBuilder();
            var innerException = exception.InnerException;
            var firstRow = true;
            while (innerException != null)
            {
                if (!firstRow)
                {
                    // TODO: parametrized this line sep part
                    result.Append(Environment.NewLine + "--> ");

                }
                result.AppendFormat("{{ {0} }} \"{1}\"", innerException.GetType(), innerException.Message);
                
                innerException = innerException.InnerException;
                firstRow = false;
            }

            return result.ToString();
        }
    }
}