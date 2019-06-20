// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NFluentShould.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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

    public class NFluentShould
    {
        [Test]
        public void
            ProvideMeaningfulStringRepresentation()
        {
            Check.That(Check.That(1).IsEqualTo(1).ToString()).IsEqualTo("Success");
        }

        [Test]
        public void
            CheckIfAValueIsDefaulted()
        {
            Check.That(0).IsDefaultValue();

            Check.That((object) null).IsDefaultValue();
        }

        [Test]
        public void
            CheckIfAValueIsDefaultedAndFailsIfNot()
        {
            Check.ThatCode(() =>
                Check.That(1).IsDefaultValue()).IsAFailingCheckWithMessage("",
                "The checked value is not the default value for its type.",
                "The checked value:",
                "\t[1]",
                "The expected value:", 
                "\t[0]");

            Check.ThatCode(() =>
                Check.That("test").IsDefaultValue()).IsAFailingCheckWithMessage("",
                "The checked string is not the default value for its type.",
                "The checked string:",
                "\t[\"test\"]",
                "The expected string:", 
                "\t[null]");
        }
    }
}