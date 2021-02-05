// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionHelper.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NFluent.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Kernel;

    /// <summary>
    /// Supported test frameworks
    /// </summary>
    public enum TestFramework
    {
        /// <summary>
        /// NUnit frameworks
        /// </summary>
        NUnit,
        /// <summary>
        /// xUnit framework
        /// </summary>
        XUnit,
        /// <summary>
        /// MsTest
        /// </summary>
        MsTest,
        /// <summary>
        /// No known framework, using built in exceptions
        /// </summary>
        None
    };

    /// <summary>
    /// Offer factory services to get adequate exception type depending on testing framework.
    /// </summary>
    public static class ExceptionHelper
    {
        private static ExceptionConstructor constructors;
        private static readonly string ExceptionSeparator = Environment.NewLine + "--> ";

        private static readonly Dictionary<TestFramework, ExceptionConstructor> Exceptions =
            new Dictionary<TestFramework, ExceptionConstructor>();

        private const string Patterns = 
            "MsTest,microsoft.visualstudio.testplatform.testframework,Microsoft.VisualStudio.TestTools,AssertFailedException,AssertInconclusiveException;"+
            "NUnit,nunit.framework,NUnit.,AssertionException,InconclusiveException;"+
            "XUnit,xunit.assert,Xunit.Sdk,XunitException,";

        private static ExceptionConstructor Constructors
        {
            get
            {
                if (constructors != null)
                {
                    return constructors;
                }
                
                // we need to identify required exception types
                Exceptions[TestFramework.None] = new ExceptionConstructor(typeof(FluentCheckException), (message) => new FluentCheckException(message));

                InitCache(Patterns);

                foreach (var id in Enum.GetValues(typeof(TestFramework)))
                {
                    var builder = Exceptions[(TestFramework) id];
                    if (!builder.IsSupported())
                    {
                        continue;
                    }

                    constructors = builder;
                    break;
                }
                return constructors;
            }
        }

        private static void InitCache(string patterns)
        {
            var lines = patterns.Split(';');
            foreach (var line in lines)
            {
                var parameters = line.Split(',');
                var testFrameworkId = (TestFramework)Enum.Parse(typeof(TestFramework), parameters[0]);
                Exceptions[testFrameworkId] = new ExceptionConstructor(parameters[1], parameters[2], parameters[3], parameters[4]);
            }
            ExceptionScanner();
        }

        private static void ExceptionScanner()
        {
#if !NETSTANDARD1_3
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var exceptionConstructor in Exceptions.Values)
                {
                    exceptionConstructor.ScanAssembly(assembly);
                }
            }
#endif
        }

        /// <summary>
        /// Reset assembly cache
        /// </summary>
        public static void ResetCache()
        {
            constructors = null;
        }

        /// <summary>
        /// Builds an exception with the given message. Automatically detect the exception type to use depending on the used check framework.
        /// </summary>
        /// <param name="theMessage">The message to build the exception with.</param>
        /// <returns>An exception instance of the appropriate type with the given message.</returns>
        public static Exception BuildException(string theMessage)
        {
            return Constructors.BuildFailedException(theMessage);
        }

        /// <summary>
        /// Builds an exception with the given message. Automatically detect the exception type to use depending on the used check framework.
        /// </summary>
        /// <param name="theMessage">The message to build the exception with.</param>
        /// <returns>An exception instance of the appropriate type with the given message.</returns>
        public static Exception BuildInconclusiveException(string theMessage)
        {
            return Constructors.BuildInconclusiveException(theMessage);
        }

        /// <summary>
        /// Gets the failed assertion exception type.
        /// </summary>
        /// <returns>the failed assertion exception type</returns>
        public static Type FailedExceptionType()
        {
            return Constructors.FailedExceptionType();
        }

        /// <summary>
        /// Checks if the failed assumption exception type
        /// </summary>
        /// <returns>the failed assumption exception type</returns>
        public static Type InconclusiveExceptionType()
        {
            return Constructors.InconclusiveExceptionType();
        }

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
                    result.Append(ExceptionSeparator);
                }
                result.AppendFormat("{{ {0} }} \"{1}\"", innerException.GetType(), innerException.Message);
                
                innerException = innerException.InnerException;
                firstRow = false;
            }

            return result.ToString();
        }
    }
}