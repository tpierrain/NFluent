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
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class StructuredCheckShould
    {
        [Test]
        public void OfferSimpleSyntax()
        {
            var sut = new Dummy();

            Check.That(sut).WhichMember(o => o.x).Verifies( m => m.IsEqualTo(0));
        }

        [Test]
        [Ignore("WIP")]
        public void OfferErrorMessageWhenFailing()
        {
            var sut = new Dummy();
            Check.ThatCode( ()=>
            Check.That(sut).WhichMember(o => o.x).Verifies( m => m.IsEqualTo(1))).
                IsAFailingCheckWithMessage(
                    "", 
                    "The checked object member [x] does not match expectation.", 
                    Criteria.FromRegEx(".*"));
        }
        
        internal class Dummy
        {
            public int x;
            public string text;
        }
    }
}