// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MacroCheck1Should.cs" company="NFluent">
//   Copyright 2022 Cyrille DUPUYDAUBY
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
    using NUnit.Framework;

    public class MacroCheck1Should
    {
        [Test]
        public void
            CanDeclareMacrosSingleParameter()
        {
            var sut = Check.DeclareMacro((int x, int y) =>
                Check.That(x).IsStrictlyGreaterThan(y), "The {0} is less than {1}.");

            Check.That(2).VerifiesMacro(sut).With(1);
        }
    }
}