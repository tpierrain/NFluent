// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="BooleanRelatedTests.cs" company="">
// //   Copyright 2013 Marc-Antoine LATOUR
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
    using NUnit.Framework;

    [TestFixture]
    public class BooleanRelatedTests
    {
        [Test]
        public void CheckThatWorksOnBoolean()
        {
            const bool NFluentRocks = true;
            const bool TddSucks = false;

            Check.That(NFluentRocks).IsTrue();
            Check.That(TddSucks).IsFalse();
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnBoolean()
        {
            const bool NFluentRocks = true;
            const bool TddSucks = false;

            Check.That(NFluentRocks).IsTrue().And.IsEqualTo(true).And.IsNotEqualTo(TddSucks);
            Check.That(TddSucks).IsFalse().And.IsEqualTo(false).And.IsNotEqualTo(NFluentRocks);
        }

        [Test]
        public void NotOperatorWorks()
        {
            const bool NFluentRocks = true;
            const bool TddSucks = false;

            Check.That(NFluentRocks).Not.IsFalse();
            Check.That(TddSucks).Not.IsTrue();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[False]\nis not true.")]
        public void IsTrueThrowsExceptionWhenFalse()
        {
            const bool NFluentRocks = false;

            Check.That(NFluentRocks).IsTrue();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[True]\nis not equal to the expected one:\n\t[False].")]
        public void IsEqualThrowsExceptionWhenNotEqual()
        {
            const bool NFluentRocks = true;
            const bool TddSucks = false;
            Check.That(NFluentRocks).IsEqualTo(TddSucks);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[True] of type: [System.Boolean].")]
        public void IsNotEqualThrowsExceptionWhenEqual()
        {
            const bool NFluentRocks = true;
            const bool WinterNotNFluentRocks = true;

            Check.That(NFluentRocks).IsNotEqualTo(WinterNotNFluentRocks);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[True]\nis not false.")]
        public void IsFalseThrowsExceptionWhenTrue()
        {
            const bool NFluentRocks = true;

            Check.That(NFluentRocks).IsFalse();
        }
    }
}