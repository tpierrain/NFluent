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

namespace NFluent.Tests.xUnit
{
    using Helpers;
    using Xunit;
    using Xunit.Sdk;

    public class NFluentShould
    {
        [Fact]
        public void ExceptionScanTest()
        {
            // inject a type from the fuzzing assembly to check for some degenerative case
            var temp = new NoNameSpaceType();
            Check.That(ExceptionHelper.BuildException("Test")).IsInstanceOf<XunitException>();
        }

        [Fact]
        public void WorkWithXunit()
        {
            Check.That(2).IsEqualTo(2);

            Check.ThatCode(() => Check.That(2).IsEqualTo(0)).IsAFaillingCheck();
        }
    }
}