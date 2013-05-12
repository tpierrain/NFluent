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
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using NFluent.Extensions;

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
                    var defaultSignature = new[] { typeof(string) };
                    var result = new ExceptionConstructor();
                    var defaultConstructor = typeof(FluentAssertionException).GetConstructor(defaultSignature);
                    var testingFrameworkFound = false;

                    // assert we have a default constructor
                    Debug.Assert(defaultConstructor != null, "NFluent exception must provide a constructor accepting a single string as parameter!");

                    // by default, we will expose NFluent own exception
                    result.FailedException = defaultConstructor;
                    result.IgnoreException = defaultConstructor;
                    result.InconclusiveException = defaultConstructor;

                    var foundExceptions = 0;

                    // look for NUnit
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.FullName.ToLowerInvariant().Contains("nunit")))
                    {
                        foreach (var type in assembly.GetExportedTypes())
                        {
                            if (type.Namespace != null && type.Namespace.StartsWith("NUnit."))
                            {
                                switch (type.Name)
                                {
                                    case "AssertionException":
                                        result.FailedException = type.GetConstructor(defaultSignature) ?? result.FailedException;
                                        testingFrameworkFound = true;
                                        foundExceptions++;
                                        break;
                                    case "IgnoreException":
                                        result.IgnoreException = type.GetConstructor(defaultSignature) ?? result.IgnoreException;
                                        testingFrameworkFound = true;
                                        foundExceptions++;
                                        break;
                                    case "InconclusiveException":
                                        result.InconclusiveException = type.GetConstructor(defaultSignature) ?? result.InconclusiveException;
                                        testingFrameworkFound = true;
                                        foundExceptions++;
                                        break;
                                }
                            }

                            // stop search if we found everything
                            if (foundExceptions == 3)
                            {
                                break;
                            }
                        }

                        if (foundExceptions == 3)
                        {
                            break;
                        }
                    }

                    if (!testingFrameworkFound)
                    {
                        // look for MSTest
                        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.FullName.ToLowerInvariant().Contains("visualstudio")))
                        {
                            foreach (var type in assembly.GetExportedTypes())
                            {
                                if (type.Namespace != null && type.Namespace.StartsWith("Microsoft.VisualStudio.TestTools"))
                                {
                                    switch (type.Name)
                                    {
                                        case "AssertFailedException":
                                            result.FailedException = type.GetConstructor(defaultSignature) ?? result.FailedException;
                                            testingFrameworkFound = true;
                                            foundExceptions++;
                                            break;
                                        case "AssertInconclusiveException":
                                            result.InconclusiveException = type.GetConstructor(defaultSignature) ?? result.InconclusiveException;
                                            testingFrameworkFound = true;
                                            foundExceptions++;
                                            break;
                                    }
                                }

                                if (foundExceptions == 2)
                                {
                                    break;
                                }
                            }

                            if (foundExceptions == 2)
                            {
                                break;
                            }
                        }
                    }

                    return constructors = result;
                }

                return constructors;
            }
        }

        /// <summary>
        /// Builds an exception with the given message. Automatically detect the exception type to use depending on the used assertion framework.
        /// </summary>
        /// <param name="theMessage">The message to build the exception with.</param>
        /// <returns>An exception instance of the appropriate type with the given message.</returns>
        public static Exception BuildException(string theMessage)
        {
            return Constructors.FailedException.Invoke(new object[] { theMessage }) as Exception;
        }

        /// <summary>
        /// Builds the standard error message, made of 5 lines of text.
        /// </summary>
        /// <param name="message">
        /// The general message. You can use {0} and {1} as placeholders for the checked and expected item labels.
        /// </param>
        /// <param name="testedEntity">
        /// The tested entity label.
        /// </param>
        /// <param name="checkedObject">
        /// The checked Object.
        /// </param>
        /// <param name="expectedObject">
        /// The expected Object.
        /// </param>
        /// <returns>
        /// The properly structured error message.
        /// </returns>
        public static string BuildStandardMessage(string message, string testedEntity, object checkedObject, object expectedObject)
        {
            var builder = new StringBuilder(200);
            var checkedLabel = "checked " + testedEntity;
            var expectedLabel = "expected " + testedEntity;

            // build the summary message
            builder.AppendFormat(message, checkedLabel, expectedLabel);
            builder.AppendLine();

            // add the checked item description
            builder.AppendFormat("The {0}:", checkedLabel);
            builder.AppendLine();
            builder.AppendFormat("\t[{0}]", checkedObject.ToStringProperlyFormated());
            builder.AppendLine();

            // add the expected item description
            builder.AppendFormat("The {0}:", expectedLabel);
            builder.AppendLine();
            builder.AppendFormat("\t[{0}]", expectedObject.ToStringProperlyFormated());

            return builder.ToString();
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
}