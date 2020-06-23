// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NullableChecksShould.cs" company="NFluent">
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
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class NullableChecksShould
    {
        [Test]
        public void SupportHasValue()
        {
            int? sut = 2;
            Check.That(sut).HasAValue();
        }        
        
        [Test]
        public void SupportHasNoValue()
        {
            int? sut = null;
            Check.That(sut).HasNoValue();
        }    
        
        [Test]
        public void SupportHasValueWithProperMessageWhenFailing()
        {
            int? sut = null;
            Check.ThatCode(() =>
                    Check.That(sut).HasAValue()).
                IsAFailingCheckWithMessage(
                    "", 
                    "The checked nullable has no value, which is unexpected.");
        }        
        
        [Test]
        public void SupportHasNoValueWithProperMessageWhenFailing()
        {
            int? sut = 2;
            Check.ThatCode(() =>
                    Check.That(sut).HasNoValue()).
                IsAFailingCheckWithMessage("", 
                    "The checked nullable has a value, whereas it must not.", 
                    "The checked nullable:", 
                    "\t[2]");
        }

        [Test]
        public void SupportHasAValueWhich()
        {
            int? sut = 2;
            Check.That(sut).HasAValue().Which.IsEqualTo(2);
        }

        [Test]
        public void RaiseAGoodMessageWithWhich()
        {
            int? sut = 2;
            Check.ThatCode(()=> 
            Check.That(sut).HasAValue().Which.IsEqualTo(3)).IsAFailingCheckWithMessage("", 
                "The checked [value] is different from the expected one.", 
                "The checked [value]:", 
                "\t[2]", 
                "The expected [value]:", 
                "\t[3]");
        }

        [Test]
        public void FailOnWhichWhenNoValue()
        {
            int? sut = null;
            Check.ThatCode(() => 
                Check.That(sut).Not.HasAValue().Which.IsEqualTo(1)
                ).Throws<InvalidOperationException>();
        }
    }
}