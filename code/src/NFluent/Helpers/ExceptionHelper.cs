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
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Supported test frameworks
    /// </summary>
    public enum TestFramework
    {
        /// <summary>
        /// MsTest
        /// </summary>
        MsTest,
        /// <summary>
        /// NUnit frameworks
        /// </summary>
        NUnit,
        /// <summary>
        /// xUnit framework
        /// </summary>
        XUnit,
    };

    /// <summary>
    /// Offer factory services to get adequate exception type depending on testing framework.
    /// </summary>
    public static class ExceptionHelper
    {
        private static IExceptionConstructor constructors;
        private static readonly string ExceptionSeparator = Environment.NewLine + "--> ";

        private static IExceptionConstructor Constructors
        {
            get
            {
                var exceptionConstructor = constructors;
                while (exceptionConstructor == null)
                {
                    constructors = LoadSupportedExceptionConstructor();
                    exceptionConstructor = constructors;
                }
                return exceptionConstructor;
            }
        }

        private static IExceptionConstructor LoadSupportedExceptionConstructor()
        {
            // we need to identify required exception types
            var exceptions = LoadExceptionConstructors();
            
            foreach (var id in Enum.GetValues(typeof(TestFramework)))
            {
                var builder = exceptions[(TestFramework) id];
                if (builder.IsSupported())
                {
                    return builder;
                }
            }

            return new DefaultExceptionConstructor();
        }

        private static Dictionary<TestFramework, ExceptionConstructor> LoadExceptionConstructors()
        {
            var exceptions = new Dictionary<TestFramework, ExceptionConstructor>
            {
                [TestFramework.MsTest] = new ExceptionConstructor("microsoft.visualstudio.testplatform.testframework", "Microsoft.VisualStudio.TestTools", "AssertFailedException", "AssertInconclusiveException"),
                [TestFramework.NUnit] = new ExceptionConstructor("nunit.framework", "NUnit", "AssertionException", "InconclusiveException"),
                [TestFramework.XUnit] = new ExceptionConstructor("xunit.assert", "Xunit.Sdk", "XunitException", ""),
            };
            
            ScanAssemblies(exceptions);

            return exceptions;
        }

        private static void ScanAssemblies(Dictionary<TestFramework, ExceptionConstructor> exceptions)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var exceptionConstructor in exceptions.Values)
                {
                    exceptionConstructor.ScanAssembly(assembly);
                }
            }
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
                result.AppendFormat(CultureInfo.InvariantCulture, "{{ {0} }} \"{1}\"", innerException.GetType(), innerException.Message);
                
                innerException = innerException.InnerException;
                firstRow = false;
            }

            return result.ToString();
        }
    }
}