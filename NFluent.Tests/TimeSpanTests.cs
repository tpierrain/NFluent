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
        public void LessThanWorks()
        {
            TimeSpan testValue = TimeSpan.FromMilliseconds(500);
            Check.That(testValue).IsLessThan(600, TimeUnit.Milliseconds);
            Check.That(testValue).IsLessThan(TimeSpan.FromMilliseconds(600));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[500 Milliseconds]\nis not less than:\n\t[100 Milliseconds]\nas expected.")]
        public void LessThanThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(500)).IsLessThan(100, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[100 Milliseconds]\nis less than:\n\t[600 Milliseconds]\nwhich is unexpected.")]
        public void NotLessThanThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(100)).Not.IsLessThan(600, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[500 Milliseconds]\nis not less than:\n\t[100 Milliseconds]\nas expected.")]
        public void LessThanThrowsExceptionWhenFailingWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(500)).IsLessThan(TimeSpan.FromMilliseconds(100));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[100 Milliseconds]\nis less than:\n\t[600 Milliseconds]\nwhich is unexpected.")]
        public void NotLessThanThrowsExceptionWhenFailingWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(100)).Not.IsLessThan(TimeSpan.FromMilliseconds(600));
        }

        [Test]
        public void IsGreaterThanWorks()
        {
            TimeSpan testValue = TimeSpan.FromMilliseconds(500);
            Check.That(testValue).IsGreaterThan(100, TimeUnit.Milliseconds);
            Check.That(testValue).IsGreaterThan(TimeSpan.FromMilliseconds(100));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[50 Milliseconds]\nis not greater than:\n\t[100 Milliseconds]\nas expected.")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsGreaterThan(100, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[100 Milliseconds]\nis greater than:\n\t[50 Milliseconds]\nwhich is unexpected.")]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(100)).Not.IsGreaterThan(50, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[50 Milliseconds]\nis not greater than:\n\t[100 Milliseconds]\nas expected.")]
        public void IsGreaterThanThrowsExceptionWhenFailingWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsGreaterThan(TimeSpan.FromMilliseconds(100));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[100 Milliseconds]\nis greater than:\n\t[50 Milliseconds]\nwhich is unexpected.")]
        public void NotIsGreaterThanThrowsExceptionWhenFailingWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(100)).Not.IsGreaterThan(TimeSpan.FromMilliseconds(50));
        }

        [Test]
        public void IsEqualToWorks()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(50, TimeUnit.Milliseconds);
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(TimeSpan.FromMilliseconds(50));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[50 Milliseconds]\nis not equal to:\n\t[40 Milliseconds]\nas expected.")]
        public void IsEqualToThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(40, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[50 Milliseconds]\nis equal to:\n\t[50 Milliseconds]\nwhich is unexpected.")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).Not.IsEqualTo(50, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[50 Milliseconds]\nis not equal to:\n\t[40 Milliseconds]\nas expected.")]
        public void IsEqualToThrowsExceptionWhenFailingWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(TimeSpan.FromMilliseconds(40));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value of:\n\t[50 Milliseconds]\nis equal to:\n\t[50 Milliseconds]\nwhich is unexpected.")]
        public void NotIsEqualToThrowsExceptionWhenFailingWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).Not.IsEqualTo(TimeSpan.FromMilliseconds(50));
        }
    }
}