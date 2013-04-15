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
            Assert.AreEqual(1.0, TimeHelper.GetInNanoSeconds(1, TimeUnit.NanoSeconds));
            Assert.AreEqual(1000.0, TimeHelper.GetInNanoSeconds(1, TimeUnit.MicroSeconds));
            Assert.AreEqual(1000000.0, TimeHelper.GetInNanoSeconds(1, TimeUnit.Milliseconds));
            Assert.AreEqual(1000000000.0, TimeHelper.GetInNanoSeconds(1, TimeUnit.Seconds));
            Assert.AreEqual(1000000000.0 * 60, TimeHelper.GetInNanoSeconds(1, TimeUnit.Minutes));
            Assert.AreEqual(1000000000.0 * 60 * 60, TimeHelper.GetInNanoSeconds(1, TimeUnit.Hours));
            Assert.AreEqual(1000000000.0 * 60 * 60 * 24, TimeHelper.GetInNanoSeconds(1, TimeUnit.Days));
            Assert.AreEqual(1000000000.0 * 60 * 60 * 24 * 7, TimeHelper.GetInNanoSeconds(1, TimeUnit.Weeks));
        }

        [Test]
        public void CheckTimeSpan()
        {
            Assert.AreEqual(TimeSpan.FromMilliseconds(212), TimeHelper.ToTimeSpan(212, TimeUnit.Milliseconds));
        }
        [Test]
        public void CheckTimeSpan()
        {
            Assert.AreEqual(TimeSpan.FromMilliseconds(212), TimeHelper.ToTimeSpan(212, TimeUnit.Milliseconds));
        }
        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void CheckInvalidUnits()
        {
            Assert.AreNotEqual(0, TimeHelper.GetInNanoSeconds(10, (TimeUnit)100));
        }
    }
}