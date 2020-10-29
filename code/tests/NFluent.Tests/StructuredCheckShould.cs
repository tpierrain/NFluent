// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StructuredCheckShould.cs" company="NFluent">
//   Copyright 2020 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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

namespace NFluent.Tests
{
    using System;
    using Extensibility;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class StructuredCheckShould
    {
        private DummyReporter dummyReporter;

        [Test]
        public void OfferSimpleSyntax()
        {
            var sut = new Dummy {Text = "test", X = 1};

            Check.That(sut).WhichMember(o=> o.X).Verifies( m => m.IsEqualTo(1));
            Check.That(sut).WhichMember(o=> o.Text).Verifies( m => m.IsEqualTo("test"));
        }

        [Test]
        public void SupportCustomReporters()
        {
            var lastReporter = Check.Reporter;
            try
            {
                var sut = new Dummy();
                this.dummyReporter = new DummyReporter();
                Check.Reporter = this.dummyReporter;
                Check.That(sut).WhichMember(o => o.X).Verifies(m => m.IsEqualTo(1));

                Check.That(this.dummyReporter.LastMessage).IsNotEmpty();
            }
            finally
            {
                Check.Reporter = lastReporter;
            }
        }

        public class DummyReporter : IErrorReporter
        {
            public string LastMessage;
            public void ReportError(string message)
            {
                this.LastMessage = message;
            }
        }

        [Test]
        public void OfferErrorMessageWhenFailing()
        {
            var sut = new Dummy();
            Check.ThatCode( ()=>
            Check.That(sut).WhichMember(o => o.X).Verifies( m => m.IsEqualTo(1))).
                IsAFailingCheckWithMessage("", 
                    "The checked value fails the check because:", 
                    "The checked value's X is different from the expected one.", 
                    "The checked value's X:", 
                    "\t[0]", 
                    "The expected value's X:", 
                    "\t[1]");
        }

        [Test]
        //[Ignore("WIP")]
        public void SupportMultipleCriteria()
        {
            var sut = new Dummy();
            Check.ThatCode( ()=>
                    Check.That(sut).WhichMember(o => o.X).Verifies( m => m.IsEqualTo(1).And.IsStrictlyGreaterThan(0))).
                IsAFailingCheckWithMessage("", 
                    "The checked value fails the check because:", 
                    "The checked value's X is different from the expected one.", 
                    "The checked value's X:", 
                    "\t[0]", 
                    "The expected value's X:", 
                    "\t[1]", 
                    "The checked value's X is equal to the given one.", 
                    "The checked value's X:", 
                    "\t[0]", 
                    "The expected value's X: strictly greater than", 
                    "\t[0]");
        }
        
        [Test]
        public void NotSupportNegationWithVerifies()
        {
            var sut = new Dummy();
            Check.ThatCode(() =>
                    Check.That(sut).Not.WhichMember(o => o.X).Verifies(m => m.IsEqualTo(1)))
                .Throws<InvalidOperationException>().WithMessage("Verifies can't be used when negated");
        }
        
        internal class Dummy
        {
            public int X;
            public string Text;
        }
    }
}