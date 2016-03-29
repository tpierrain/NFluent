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
        [ExpectedException(ExpectedException = typeof(FluentCheckException), ExpectedMessage = "\nThe checked boolean is false whereas it must be true.\nThe checked boolean:\n\t[False]")]
        public void NotIsFalseMayThrowExceptions()
        {
            const bool TddSucks = false;
            
            Check.That(TddSucks).Not.IsFalse();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentCheckException), ExpectedMessage = "\nThe checked boolean is false whereas it must be true.\nThe checked boolean:\n\t[False]")]
        public void IsTrueThrowsExceptionWhenFalse()
        {
            const bool NFluentRocks = false;

            Check.That(NFluentRocks).IsTrue();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentCheckException), ExpectedMessage = "\nThe checked boolean is true whereas it must be false.\nThe checked boolean:\n\t[True]")]
        public void NotIsTrueThrowsExceptionWhenFalse()
        {
            const bool NFluentRocks = true;

            Check.That(NFluentRocks).Not.IsTrue();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[True]\nThe expected value:\n\t[False]")]
        public void IsEqualThrowsExceptionWhenNotEqual()
        {
            const bool NFluentRocks = true;
            const bool TddSucks = false;
            Check.That(NFluentRocks).IsEqualTo(TddSucks);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[True] of type: [bool]")]
        public void IsNotEqualThrowsExceptionWhenEqual()
        {
            const bool NFluentRocks = true;
            const bool WinterNotNFluentRocks = true;
            Check.That(NFluentRocks).IsNotEqualTo(WinterNotNFluentRocks);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentCheckException), ExpectedMessage = "\nThe checked boolean is true whereas it must be false.\nThe checked boolean:\n\t[True]")]
        public void IsFalseThrowsExceptionWhenTrue()
        {
            const bool NFluentRocks = true;

            Check.That(NFluentRocks).IsFalse();
        }
    }
}