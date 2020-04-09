// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DecimalCheckShould.cs" company="NFluent">
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
    public class DecimalCheckShould
    {
        [Test]
        public void SupportIsCloseTo()
        {
            Check.That(1.01m).IsCloseTo(1m, .01m);
        }

        [Test]
        public void ProvideErrorMessageWithIsCloseTo()
        {
            Check.ThatCode(() =>
                Check.That(1.02m).IsCloseTo(1m, .01m)).IsAFailingCheckWithMessage(
            "", 
            "The checked value is outside the expected value range.", 
            "The checked value:", 
            "\t[1,02]", 
            "The expected value:", 
            "\t[1 (+/- 0,01)]");
        }
    }
}