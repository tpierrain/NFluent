// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MsTestCompatibility.cs" company="NFluent">
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

namespace NFluent.Tests.MsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NFluent.Helpers;

    [TestClass]
    public class MsTestCompatibility
    {
        [TestMethod]
        public void BasicTest()
        {
            Check.That("MsTest").IsNotEmpty();

            Check.ThatCode(() => Check.That("MsTest").IsEqualTo("great")).Throws<AssertFailedException>();
        }

        [TestMethod]
        public void ExceptionScanTest()
        {
            Check.That(ExceptionHelper.BuildException("Test")).IsInstanceOf<AssertFailedException>();
        }

        [TestMethod]
        public void AssumptionTest()
        {
            Check.ThatCode(() => Assuming.That("MsTest").IsEqualTo("great")).Throws<AssertInconclusiveException>();
        }
    }
}