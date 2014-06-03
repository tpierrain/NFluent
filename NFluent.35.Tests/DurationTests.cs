// // --------------------------------------------------------------------------------------------------------------------
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
        [ExpectedException(typeof(OverflowException))]
        public void ConvertToMillisecondsThrowsWhenNumberOfMillisecondsIsSuperiorToTheIntMaxValue()
        {
            double maxValue = int.MaxValue;
            maxValue++;
            Duration.ConvertToMilliseconds(maxValue, TimeUnit.Milliseconds);
        }

        [Test]
        public void TestDurationClass()
        {
            var firstDuration = new Duration(200, TimeUnit.Minutes);
            
            Check.That(firstDuration.RawDuration).IsEqualTo(200);
            Check.That(firstDuration.Unit).IsEqualTo(TimeUnit.Minutes);
            Check.That(firstDuration.ToString()).IsEqualTo("200 Minutes");
            Check.That(firstDuration > new Duration(100, TimeUnit.Seconds)).IsTrue();
            Check.That(firstDuration < new Duration(100, TimeUnit.Hours)).IsTrue();

            var anotherDurationWithSameValue = new Duration(200, TimeUnit.Minutes);

            // test objects override
            Check.That(firstDuration.GetHashCode()).IsEqualTo(anotherDurationWithSameValue.GetHashCode());
            Check.That(firstDuration).IsEqualTo(anotherDurationWithSameValue);
            Check.That(anotherDurationWithSameValue.Equals(firstDuration)).IsTrue();
            Check.That(anotherDurationWithSameValue == firstDuration).IsTrue();
            Check.That(anotherDurationWithSameValue.Equals(null)).IsFalse();
            Check.That(anotherDurationWithSameValue.Equals(20)).IsFalse();
            Check.That(anotherDurationWithSameValue.Equals((object)firstDuration)).IsTrue();
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