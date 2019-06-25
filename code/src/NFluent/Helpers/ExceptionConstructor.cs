// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ExceptionConstructor.cs" company="NFluent">
//   Copyright 2019 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
// --------------------------------------------------------------------------------------------------------------------
namespace NFluent.Helpers
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Stores adequate constructors.
    /// </summary>
    internal class ExceptionConstructor
    {
        private readonly string assemblyNameBeginning;
        private readonly string @namespace;
        private readonly string assertExceptionName;
        private readonly string ignoreExceptionName;
        private readonly string inconclusiveExceptionName;

        private Func<string, Exception> assertExceptionBuilder;
        private Type assertExceptionType;
        private Func<string, Exception> inconclusiveExceptionBuilder;
        private Type inconclusiveExceptionType;
        private Func<string, Exception> ignoreExceptionBuilder;
        private Type ignoreExceptionType;
        private static readonly Type[] Types = { typeof(string) };

        public ExceptionConstructor(string assemblyNameBeginning, string ns, string assertExceptionName, string ignoreExceptionName, string inconclusiveExceptionName)
        {
            this.assemblyNameBeginning = assemblyNameBeginning;
            this.@namespace = ns;
            this.assertExceptionName = assertExceptionName;
            this.ignoreExceptionName = ignoreExceptionName;
            this.inconclusiveExceptionName = inconclusiveExceptionName;
        }

        public ExceptionConstructor(Type basicType, Func<string, Exception> builder)
        {
            this.assertExceptionType = basicType;
            this.assertExceptionBuilder = builder;
        }

        public bool IsSupported()
        {
            return this.assertExceptionBuilder != null;
        }

        public bool ScanAssembly(Assembly assembly)
        {
            if(this.assemblyNameBeginning == null)
            {
                return false;
            }

            if (!assembly.FullName.ToLowerInvariant().Contains(this.assemblyNameBeginning))
            {
                return false;
            }

            var exportedTypes = assembly.GetExportedTypes();

            foreach (var type in exportedTypes)
            {
                if (type.Namespace.StartsWith(this.@namespace))
                {
                    if (type.Name == this.assertExceptionName)
                    {
                        var info = type.GetConstructor(Types);
                        this.assertExceptionBuilder = (message) => info.Invoke(new object[] {message}) as Exception;
                        this.assertExceptionType = type;
                    }
                    else if (type.Name == this.ignoreExceptionName)
                    {
                        var info = type.GetConstructor(Types);
                        this.ignoreExceptionBuilder = (message) => info.Invoke(new object[] {message}) as Exception;
                        this.ignoreExceptionType = type;

                    }
                    else if (type.Name == this.inconclusiveExceptionName)
                    {
                        var info = type.GetConstructor(Types);
                        this.inconclusiveExceptionBuilder = (message) => info.Invoke(new object[] {message}) as Exception;
                        this.inconclusiveExceptionType = type;
                    }
                }
            }
            return true;
        }

        public Exception BuildFailedException(string message)
        {
            return this.assertExceptionBuilder(message);
        }

        public Exception BuildInconclusiveException(string message)
        {
            return (this.inconclusiveExceptionBuilder ??
                    this.ignoreExceptionBuilder ?? this.assertExceptionBuilder)(message);
        }

        public Type FailedExceptionType()
        {
            return this.assertExceptionType;
        }

        public Type InconclusiveExceptionType()
        {
            return (this.inconclusiveExceptionType ??
                   this.ignoreExceptionType ?? this.assertExceptionType);
        }
    }
}