// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BatchOfChecks.cs" company="NFluent">
//   Copyright 2021 Cyrille DUPUYDAUBY
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

namespace NFluent.Kernel
{
    using System;
    using System.Text;
    using Extensibility;

    internal class BatchOfChecks : IDisposable, IErrorReporter
    {
        private readonly string message;
        private readonly StringBuilder errors  = new();

        public BatchOfChecks(string message)
        {
            this.message = message;
            Check.ReporterStore.Push(this);
        }

        public void Dispose()
        {
            Check.ReporterStore.Pop();
            if (this.errors.Length > 0)
            {
                var errorMessage = this.errors;
                if (!string.IsNullOrEmpty(this.message))
                {
                    errorMessage = errorMessage.Insert(0, FluentMessage.BuildMessageWithGiven(this.message).ToString());
                }
                Check.Reporter.ReportError(errorMessage.ToString());
            }
        }

        public void ReportError(string message)
        {
            if (this.errors.Length > 0)
            {
                this.errors.AppendLine();
                this.errors.Append("** And **");
            }
            this.errors.Append(message);
        }
    }    
}
