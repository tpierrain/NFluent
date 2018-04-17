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
        public void LessThanThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(500)).IsLessThan(100, TimeUnit.Milliseconds);
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked value is more than the limit." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[500 Milliseconds]" + Environment.NewLine + "The expected value: less than" + Environment.NewLine + "\t[100 Milliseconds]");
        }

        [Test]
        public void NotLessThanThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(100)).Not.IsLessThan(600, TimeUnit.Milliseconds);
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked value is not more than the limit." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[100 Milliseconds]" + Environment.NewLine + "The expected value: more than or equal to" + Environment.NewLine + "\t[600 Milliseconds]");
        }

        [Test]
        public void LessThanThrowsExceptionWhenFailingWithSpan()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(500)).IsLessThan(TimeSpan.FromMilliseconds(100));
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked value is more than the limit." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[500 Milliseconds]" + Environment.NewLine + "The expected value: less than" + Environment.NewLine + "\t[100 Milliseconds]");
        }

        [Test]
        public void NotLessThanThrowsExceptionWhenFailingWithSpan()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(100)).Not.IsLessThan(TimeSpan.FromMilliseconds(600));
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked value is not more than the limit." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[100 Milliseconds]" + Environment.NewLine + "The expected value: more than or equal to" + Environment.NewLine + "\t[600 Milliseconds]");
        }

        [Test]
        public void IsGreaterThanWorks()
        {
            TimeSpan testValue = TimeSpan.FromMilliseconds(500);
            Check.That(testValue).IsGreaterThan(100, TimeUnit.Milliseconds);
            Check.That(testValue).IsGreaterThan(TimeSpan.FromMilliseconds(100));
        }

        [Test]
        public void IsGreaterThanThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
                {
                    Check.That(TimeSpan.FromMilliseconds(50)).IsGreaterThan(100, TimeUnit.Milliseconds);
                })
                .IsAFaillingCheckWithMessage("",
                    "The checked value is not more than the limit.",
                    "The checked value:",
                    "\t[50 Milliseconds]",
                    "The expected value: more than",
                    "\t[100 Milliseconds]");
        }

        [Test]
        public void NotIsGreaterThanThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(100)).Not.IsGreaterThan(50, TimeUnit.Milliseconds);
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is more than the limit.",
                    "The checked value:",
                    "\t[100 Milliseconds]",
                    "The expected value: less than or equal to",
                    "\t[50 Milliseconds]");
        }

        [Test]
        public void IsGreaterThanThrowsExceptionWhenFailingWithSpan()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(50)).IsGreaterThan(TimeSpan.FromMilliseconds(100));
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked value is not more than the limit." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[50 Milliseconds]" + Environment.NewLine + "The expected value: more than" + Environment.NewLine + "\t[100 Milliseconds]");
        }

        [Test]
        public void NotIsGreaterThanThrowsExceptionWhenFailingWithSpan()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(100)).Not.IsGreaterThan(TimeSpan.FromMilliseconds(50));
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked value is more than the limit." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[100 Milliseconds]" + Environment.NewLine + "The expected value: less than or equal to" + Environment.NewLine + "\t[50 Milliseconds]");
        }

        [Test]
        public void IsEqualToWorks()
        {
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(50, TimeUnit.Milliseconds);
            Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(TimeSpan.FromMilliseconds(50));
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(40, TimeUnit.Milliseconds);
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked value is different from the expected one." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[50 Milliseconds]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\t[40 Milliseconds]");
        }

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(50)).Not.IsEqualTo(50, TimeUnit.Milliseconds);
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is the same than expected one, whereas it must not.",
                    "The checked value:",
                    "\t[50 Milliseconds]",
                    "The expected value: different from",
                    "\t[50 Milliseconds]");
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenFailingWithSpan()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(50)).IsEqualTo(TimeSpan.FromMilliseconds(40));
            })
            .IsAFaillingCheckWithMessage("",
                                         "The checked value is different from the expected one.",
                    "The checked value:",
                    "\t[50 Milliseconds]",
                    "The expected value:",
                    "\t[40 Milliseconds]");
        }

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailingWithSpan()
        {
            Check.ThatCode(() =>
            {
                Check.That(TimeSpan.FromMilliseconds(50)).Not.IsEqualTo(TimeSpan.FromMilliseconds(50));
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is the same than expected one, whereas it must not.",
                    "The checked value:",
                    "\t[50 Milliseconds]",
                    "The expected value: different from",
                    "\t[50 Milliseconds]");
        }
    }
}