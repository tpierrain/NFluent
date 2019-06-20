// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="TimeHelperTest.cs" company="">
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
    public class TimeHelperTest
    {
        [Test]
        public void CheckUnits()
        {
            Check.That(TimeHelper.GetInNanoSeconds(1, TimeUnit.Nanoseconds)).IsEqualTo(1.0);
            Check.That(TimeHelper.GetInNanoSeconds(1, TimeUnit.Microseconds)).IsEqualTo(1000.0);
            Check.That(TimeHelper.GetInNanoSeconds(1, TimeUnit.Milliseconds)).IsEqualTo(1000000.0);
            Check.That(TimeHelper.GetInNanoSeconds(1, TimeUnit.Seconds)).IsEqualTo(1000000000.0);
            Check.That(TimeHelper.GetInNanoSeconds(1, TimeUnit.Minutes)).IsEqualTo(1000000000.0 * 60);
            Check.That(TimeHelper.GetInNanoSeconds(1, TimeUnit.Hours)).IsEqualTo(1000000000.0 * 60 * 60);
            Check.That(TimeHelper.GetInNanoSeconds(1, TimeUnit.Days)).IsEqualTo(1000000000.0 * 60 * 60 * 24);
            Check.That(TimeHelper.GetInNanoSeconds(1, TimeUnit.Weeks)).IsEqualTo(1000000000.0 * 60 * 60 * 24 * 7);
        }

        [Test]
        public void ToTimeSpanWorks()
        {
            Check.That(TimeHelper.ToTimeSpan(212, TimeUnit.Milliseconds)).IsEqualTo(TimeSpan.FromMilliseconds(212));
        }

        [Test]
        public void ConvertWorks()
        {
            Check.That(TimeHelper.Convert(TimeSpan.FromMilliseconds(500), TimeUnit.Milliseconds)).IsEqualTo(500.0);
        }
        
        [Test]
        public void GetFromNanoSecondsWorks()
        {
            Check.That(TimeHelper.GetFromNanoSeconds(500000000, TimeUnit.Milliseconds)).IsEqualTo(500);
        }

        [Test]
        public void GetInNanoSecondsThrowsWhenHackingTimeUnitValue()
        {
            Check.ThatCode(() =>
            {
                TimeHelper.GetInNanoSeconds(10, (TimeUnit)100);
            })
            .Throws<InvalidOperationException>().WithMessage("100 is not a supported time unit.");
        }

        [Test]
        public void DiscoverUnitWorks()
        {
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromMilliseconds(2500))).IsEqualTo(TimeUnit.Seconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromTicks(1))).IsEqualTo(TimeUnit.Nanoseconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromTicks(50))).IsEqualTo(TimeUnit.Microseconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromMilliseconds(20))).IsEqualTo(TimeUnit.Milliseconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromSeconds(10))).IsEqualTo(TimeUnit.Seconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromMinutes(10))).IsEqualTo(TimeUnit.Minutes);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromHours(10))).IsEqualTo(TimeUnit.Hours);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromDays(2))).IsEqualTo(TimeUnit.Days);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromDays(30))).IsEqualTo(TimeUnit.Weeks);
        }
        [Test]
        public void DiscoverUnitWorksOnLimitValue()
        {
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromTicks(19))).IsEqualTo(TimeUnit.Nanoseconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromTicks(20))).IsEqualTo(TimeUnit.Microseconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromTicks(19999))).IsEqualTo(TimeUnit.Microseconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromTicks(20000))).IsEqualTo(TimeUnit.Milliseconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromMilliseconds(1999))).IsEqualTo(TimeUnit.Milliseconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromMilliseconds(2000))).IsEqualTo(TimeUnit.Seconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromSeconds(119))).IsEqualTo(TimeUnit.Seconds);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromSeconds(120))).IsEqualTo(TimeUnit.Minutes);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromMinutes(119))).IsEqualTo(TimeUnit.Minutes);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromMinutes(120))).IsEqualTo(TimeUnit.Hours);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromHours(47))).IsEqualTo(TimeUnit.Hours);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromHours(48))).IsEqualTo(TimeUnit.Days);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromDays(13))).IsEqualTo(TimeUnit.Days);
            Check.That(TimeHelper.DiscoverUnit(TimeSpan.FromDays(14))).IsEqualTo(TimeUnit.Weeks);
        }
    }
}