// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ErrorReportingShould.cs" company="NFluent">
//   Copyright 2021 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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

namespace NFluent.Tests.Customization
{
    using Extensibility;
    using NUnit.Framework;

    [TestFixture]
    public class ErrorReportingShould
    {
        [Test]
        public void CustomizeErrorReporting()
        {
            var reporter = new StringReporting();
            var oldReporter = Check.Reporter;
            using (Check.ChangeReporterForScope(reporter))
            {
                try
                {
                    Check.That(12).IsNotEqualTo(12);
                }
                finally
                {
                    Check.Reporter = oldReporter;
                }
            }

            Check.That(reporter.Error).AsLines().ContainsExactly("",
                "The checked value is equal to the given one whereas it must not.",
                "The expected value: different from",
                "\t[12] of type: [int]");

            Assert.That(Check.Reporter, Is.EqualTo(oldReporter));
        }

        private class StringReporting : IErrorReporter
        {
            public string Error { get; private set; }

            public void ReportError(string message)
            {
                this.Error = message;
            }
        }
    }
}