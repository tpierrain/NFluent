// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="TimeSpanTests.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY
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

    using NFluent.Helpers;

    using NUnit.Framework;

    [TestFixture]
    public class TimeSpanTests
    {
        [Test]
        public void LessThanTest()
        {
            TimeSpan testValue = TimeSpan.FromMilliseconds(500);
            Check.That(testValue).IsLessThan(600, TimeUnit.Milliseconds);
            Check.That(testValue).IsLessThan(TimeSpan.FromMilliseconds(600));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[500 Milliseconds]\nis not less than:\n\t[100 Milliseconds]\nas expected.")]
        public void LessThanTestFails()
        {
            Check.That(TimeSpan.FromMilliseconds(500)).IsLessThan(100, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[500 Milliseconds]\nis not less than:\n\t[100 Milliseconds]\nas expected.")]
        public void LessThanTestFailsWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(500)).IsLessThan(TimeSpan.FromMilliseconds(100));
        }

        [Test]
        public void GreaterThanTest()
        {
            TimeSpan testValue = TimeSpan.FromMilliseconds(500);
            Check.That(testValue).IsGreaterThan(100, TimeUnit.Milliseconds);
            Check.That(testValue).IsGreaterThan(TimeSpan.FromMilliseconds(100));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[50 Milliseconds]\nis not greater than:\n\t[100 Milliseconds]\nas expected.")]
        public void GreaterThanTestFails()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsGreaterThan(100, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[50 Milliseconds]\nis not greater than:\n\t[100 Milliseconds]\nas expected.")]
        public void GreaterThanTestFailsWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsGreaterThan(TimeSpan.FromMilliseconds(100));
        }

        [Test]
        public void EqualTo()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(50, TimeUnit.Milliseconds);
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(TimeSpan.FromMilliseconds(50));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[50 Milliseconds]\nis not equal to:\n\t[40 Milliseconds]\nas expected.")]
        public void EqualTofails()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(40, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[50 Milliseconds]\nis not equal to:\n\t[40 Milliseconds]\nas expected.")]
        public void EqualTofailsWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(TimeSpan.FromMilliseconds(40));
        }
    }
}