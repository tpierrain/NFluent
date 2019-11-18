﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DurationTests.cs" company="">
// //   Copyright 2014 Thomas PIERRAIN
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
    public class DurationTests
    {
        [Test]
        public void ConvertToMillisecondsWorksWith2Milliseconds()
        {
            int durationInMsec = Duration.ConvertToMilliseconds(2, TimeUnit.Milliseconds);
            Check.That(durationInMsec).IsEqualTo(2);
        }

        [Test]
        public void ShouldCreateFromTimeSpanWithVariousUnits()
        {
            Check.That(Duration.FromTimeSpan(TimeSpan.FromMilliseconds(1))).IsEqualTo(new Duration(1, TimeUnit.Milliseconds));
            Check.That(Duration.FromTimeSpan(TimeSpan.FromMilliseconds(2))).IsEqualTo(new Duration(2, TimeUnit.Milliseconds));
            Check.That(Duration.FromTimeSpan(TimeSpan.FromHours(3))).IsEqualTo(new Duration(3, TimeUnit.Hours));
            Check.That(Duration.FromTimeSpan(TimeSpan.FromTicks(300))).IsEqualTo(new Duration(30_000, TimeUnit.Nanoseconds));
        }

        [Test]
        public void ShouldSupportVariousUnits()
        {
            Check.That(new Duration(100, TimeUnit.Nanoseconds).RawDuration).IsEqualTo(100);
            Check.That(new Duration(100, TimeUnit.Microseconds).RawDuration).IsEqualTo(100);
            Check.That(new Duration(100, TimeUnit.Hours).RawDuration).IsEqualTo(100);
            Check.That(new Duration(5, TimeUnit.Weeks).RawDuration).IsEqualTo(5);
        }

        [Test]
        public void ShouldThrowOnInvalidUnit()
        {
            Check.ThatCode(() => { new Duration(100, (TimeUnit) (100));}).Throws<ArgumentException>();
            Check.ThatCode(() => { new Duration(100, (TimeUnit) (-1));}).Throws<ArgumentException>();
        }

        [Test]
        public void ConvertToMillisecondsWorksWith2Seconds()
        {
            int durationInMsec = Duration.ConvertToMilliseconds(2, TimeUnit.Seconds);
            Check.That(durationInMsec).IsEqualTo(2000);
        }

        [Test]
        public void ConvertToMillisecondsWorksWith2Days()
        {
            int durationInMsec = Duration.ConvertToMilliseconds(2, TimeUnit.Days);
            Check.That(durationInMsec).IsEqualTo(2 * 24 * 60 * 60 * 1000);
        }

        [Test]
        public void ConvertToMillisecondsThrowsWhenNumberOfMillisecondsIsSuperiorToTheIntMaxValue()
        {
            
            Check.ThatCode(() =>
                                {
                                    double maxValue = int.MaxValue;
                                    maxValue++;
                                    Duration.ConvertToMilliseconds(maxValue, TimeUnit.Milliseconds);
                                })
                                .Throws<OverflowException>();
        }

        [Test]
        public void TestDurationClass()
        {
            var firstDuration = new Duration(200, TimeUnit.Minutes);
            var secondDuration = new Duration(200, TimeUnit.Minutes);
            
            Check.That(firstDuration.RawDuration).IsEqualTo(200);
            Check.That(firstDuration.Unit).IsEqualTo(TimeUnit.Minutes);
            Check.That(firstDuration.ToString()).IsEqualTo("200 Minutes");
            Check.That(firstDuration > new Duration(100, TimeUnit.Seconds)).IsTrue();
            Check.That(firstDuration < new Duration(100, TimeUnit.Hours)).IsTrue();
            Check.That(firstDuration < secondDuration).IsFalse();
            Check.That(firstDuration <= secondDuration).IsTrue();
            Check.That(firstDuration > secondDuration).IsFalse();
            Check.That(firstDuration >= secondDuration).IsTrue();

            var anotherDurationWithSameValue = new Duration(200, TimeUnit.Minutes);

            // test objects override
            Check.That(firstDuration.GetHashCode()).IsEqualTo(anotherDurationWithSameValue.GetHashCode());
            Check.That(firstDuration).IsEqualTo(anotherDurationWithSameValue);
            Check.That(anotherDurationWithSameValue.Equals(firstDuration)).IsTrue();
            Check.That(anotherDurationWithSameValue == firstDuration).IsTrue();
            Check.That(anotherDurationWithSameValue.Equals(null)).IsFalse();
            // ReSharper disable once SuspiciousTypeConversion.Global
            Check.That(anotherDurationWithSameValue.Equals(20)).IsFalse();
            Check.That(anotherDurationWithSameValue.Equals((object)firstDuration)).IsTrue();

            Check.That(firstDuration.GetHashCode()).IsEqualTo(-259084257);
        }

        [Test]
        public void TestDurationConversion()
        {
            var test = new Duration(2, TimeUnit.Milliseconds);
            var converted = test.ConvertTo(TimeUnit.Microseconds);

            Check.That(converted.RawDuration).IsEqualTo(2000);
            Check.That(converted.Unit).IsEqualTo(TimeUnit.Microseconds);
        }
    }
}