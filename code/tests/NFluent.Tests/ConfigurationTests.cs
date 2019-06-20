// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ConfigurationTests.cs" company="">
// //   Copyright 2017 Cyrille Dupuydauby
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
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void CanConfigureTruncationLength()
        {
            var current = Check.StringTruncationLength;
            try
            {
                Check.StringTruncationLength = 20;
                Check.That(Check.StringTruncationLength).IsEqualTo(20);

                Check.ThatCode(() => Check.StringTruncationLength = 5).Throws<ArgumentException>();
            }
            finally
            {
                Check.StringTruncationLength = current;
            }
        }
    }
}