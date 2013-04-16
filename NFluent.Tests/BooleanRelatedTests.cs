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
            const bool IsFunny = true;
            const bool WinterIsFunny = false;

            Check.That(IsFunny).IsTrue();
            Check.That(WinterIsFunny).IsFalse();
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnBoolean()
        {
            const bool IsFunny = true;
            const bool WinterIsFunny = false;

            Check.That(IsFunny).IsTrue().And.IsEqualTo(true).And.IsNotEqualTo(WinterIsFunny);
            Check.That(WinterIsFunny).IsFalse().And.IsEqualTo(false).And.IsNotEqualTo(IsFunny);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[False]\nis not true.")]
        public void IsTrueThrowsExceptionWhenFalse()
        {
            const bool IsFunny = false;

            Check.That(IsFunny).IsTrue();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[True]\nis not equal to the expected one, i.e.:\n\t[False].")]
        public void IsEqualThrowsExceptionWhenNotEqual()
        {
            const bool IsFunny = true;
            const bool WinterIsFunny = false;
            Check.That(IsFunny).IsEqualTo(WinterIsFunny);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[True] of type: [System.Boolean].")]
        public void IsNotEqualThrowsExceptionWhenEqual()
        {
            const bool IsFunny = true;
            const bool WinterNotIsFunny = true;

            Check.That(IsFunny).IsNotEqualTo(WinterNotIsFunny);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[True]\nis not false.")]
        public void IsFalseThrowsExceptionWhenTrue()
        {
            const bool IsFunny = true;

            Check.That(IsFunny).IsFalse();
        }
    }
}