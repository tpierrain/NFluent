// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="BooleanRelatedTests.cs" company="">
// //   Copyright 2013 Marc-Antoine LATOUR, Thomas PIERRAIN
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
using System;

namespace NFluent.Tests
{
    using Helpers;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class StructRelatedTests
    {
        [Test]
        public void HasAValueWorks()
        {
            bool? hasAValue = true;

            Check.That(hasAValue).HasAValue();
        }

        [Test]
        public void HasAValueSupportsToBeChainedWithTheWhichOperator()
        {
            bool? hasAValue = true;

            Check.That(hasAValue).HasAValue().Which.IsTrue();
        }

        [Test]
        public void HasNoValueWorks()
        {
            bool? isNull = null;

            Check.That(isNull).HasNoValue();
        }
    }
}