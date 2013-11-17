// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ExtensibilityHelperTests.cs" company="">
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
namespace NFluent.Tests
{
    using NFluent.Extensibility;

    using NUnit.Framework;

    [TestFixture]
    public class ExtensibilityHelperTests
    {
        [Test]
        public void ExtractRunnableCheckWorks()
        {
            var runnableCheck = ExtensibilityHelper<string>.ExtractRunnableCheck(new FluentCheck<string>("kamoulox"));
            Check.That(runnableCheck).IsNotNull();
            Check.That(runnableCheck.Negated).IsFalse();
            Check.That(runnableCheck.Value).IsEqualTo("kamoulox");
        }

        [Test]
        public void ExtractRunnableStructCheckWorks()
        {
            var runnableStructCheck = ExtensibilityHelper<Nationality>.ExtractRunnableStructCheck(new FluentStructCheck<Nationality>(Nationality.Chinese));
            Check.ThatEnum(runnableStructCheck.Value).IsEqualTo(Nationality.Chinese);
            Check.That(runnableStructCheck.Negated).IsFalse();
        }
    }
}
