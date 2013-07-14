﻿// // --------------------------------------------------------------------------------------------------------------------
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is more than the limit.\nThe checked value:\n\t[500 Milliseconds]\nThe expected value: less than\n\t[100 Milliseconds]")]
        public void LessThanThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(500)).IsLessThan(100, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not more than the limit.\nThe checked value:\n\t[100 Milliseconds]\nThe expected value: more than or equal to\n\t[600 Milliseconds]")]
        public void NotLessThanThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(100)).Not.IsLessThan(600, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is more than the limit.\nThe checked value:\n\t[500 Milliseconds]\nThe expected value: less than\n\t[100 Milliseconds]")]
        public void LessThanThrowsExceptionWhenFailingWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(500)).IsLessThan(TimeSpan.FromMilliseconds(100));
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not more than the limit.\nThe checked value:\n\t[100 Milliseconds]\nThe expected value: more than or equal to\n\t[600 Milliseconds]")]
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not more than the limit.\nThe checked value:\n\t[50 Milliseconds]\nThe expected value: less than or equal to\n\t[100 Milliseconds]")]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsGreaterThan(100, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is more than the limit.\nThe checked value:\n\t[100 Milliseconds]\nThe expected value: more than\n\t[50 Milliseconds]")]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(100)).Not.IsGreaterThan(50, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is not more than the limit.\nThe checked value:\n\t[50 Milliseconds]\nThe expected value: more than\n\t[100 Milliseconds]")]
        public void IsGreaterThanThrowsExceptionWhenFailingWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsGreaterThan(TimeSpan.FromMilliseconds(100));
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is more than the limit.\nThe checked value:\n\t[100 Milliseconds]\nThe expected value: less than or equal to\n\t[50 Milliseconds]")]
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[50 Milliseconds]\nThe expected value:\n\t[40 Milliseconds]")]
        public void IsEqualToThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(40, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is the same than expected one.\nThe checked value:\n\t[50 Milliseconds]\nThe expected value: different than\n\t[50 Milliseconds]")]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).Not.IsEqualTo(50, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[50 Milliseconds]\nThe expected value:\n\t[40 Milliseconds]")]
        public void IsEqualToThrowsExceptionWhenFailingWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(TimeSpan.FromMilliseconds(40));
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is the same than expected one.\nThe checked value:\n\t[50 Milliseconds]\nThe expected value: different than\n\t[50 Milliseconds]")]
        public void NotIsEqualToThrowsExceptionWhenFailingWithSpan()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).Not.IsEqualTo(TimeSpan.FromMilliseconds(50));
        }
    }
}