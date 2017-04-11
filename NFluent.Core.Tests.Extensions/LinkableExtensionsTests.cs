// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="LinkableExtensionsTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
// ReSharper disable once CheckNamespace
namespace NFluent.Tests.Extensions
{
    using NUnit.Framework;

    [TestFixture]
    public class LinkableExtensionsTests
    {
        [Test]
        public void CanChainMyChecks()
        {
            var bazunga = new Person();
            Check.That(bazunga).IsPortna().And.IsNawouak();

            Check.That(bazunga).IsNawouak().And.IsPortna();
        }

        [Test]
        public void CanChainMyStructChecks()
        {
            const Nationality frenchNationality = Nationality.French;

            Check.ThatEnum(frenchNationality).IsEuropean().And.IsOccidental();
        }
    }
}
