 // --------------------------------------------------------------------------------------------------------------------
 // <copyright file="ExtensionSpikeTests.cs" company="">
 //   Copyright 2013 Thomas PIERRAIN
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

namespace NFluent.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using Extensibility;
    using Kernel;
    using NUnit.Framework;

    [TestFixture]
    public class ExtensionSpikeTests
    {
        [Test]
        public void CanUseVariousExtensions()
        {
            const int mythicNumber = 42;
            const string yodaStatement = "Has the force";

            Check.That(mythicNumber as IComparable).IsBefore(100).And.IsBefore(200);
            Check.That(mythicNumber).IsNotZero().And.IsInstanceOf<int>();
            Check.That(yodaStatement).Contains("force");
        }

        [Test]
        public void WorksWhenSutIsADerivedTypeOfTheInterfaceReferencedAsTheTypeParameterOfTheFluentAssertion()
        {
            // here: 
            // the interface referenced as the type parameter of the fluent check: IComparable
            // and the sut type: System.Version
            var v1 = new Version(1, 0);
            var v2 = new Version(2, 0);
            var v3 = new Version(3, 0);

            Check.That(v1).IsBefore(v2).And.IsBefore(v3);
        }

        [Test]
        public void CheckWorksWithDecimal()
        {
            var one = new decimal(1);
            var pi = new decimal(Math.PI);

            Check.That(pi).IsInstanceOf<decimal>().And.IsNotZero().And.IsStrictlyPositive();

             Check.That(one).IsBefore(pi);
        }

        [Test]
        public void CanLoadExtensionsFromAnotherAssembly()
        {
            var eternalSunshineOfTheSpotlessMind = new Movie("Eternal sunshine of the spotless mind", new Person { Name = "Michel GONDRY" }, new List<Nationality> { Nationality.American });

            Check.That(eternalSunshineOfTheSpotlessMind).IsDirectedBy("Michel GONDRY");
            Check.That(eternalSunshineOfTheSpotlessMind).IsAfGreatMovie();
        }

        [Test]
        public void GeneralExtensibility()
        {
                var sut = new FluentSut<int>(12, null);

                ExtensibilityHelper.BeginCheck(sut)
                    .FailIfNull()
                    .OnNegate("It must be null.")
                    .EndCheck();
        }

        [Test]
        public void CustomizeErrorReporting()
        {
            var reporter = new StringReporting();
            var oldReporter = Check.Reporter;
            Check.Reporter = reporter;
            try
            {
                Check.That(12).IsNotEqualTo(12);

            }
            finally
            {
                Check.Reporter = oldReporter;
            }

            Check.That(reporter.Error).AsLines().ContainsExactly("",
                "The checked value is equal to the expected one whereas it must not.",
                "The expected value: different from",
                "\t[12] of type: [int]");
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
