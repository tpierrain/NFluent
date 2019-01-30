// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EventWaitHandleTests.cs" company="">
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
// ReSharper disable AccessToDisposedClosure

namespace NFluent.Tests
{
    using System;
    using System.Threading;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class EventWaitHandleTests
    {
        #region IsSetWithin

        [Test]
        public void IsSetWithinWorksForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                SetTheEventFromAnotherThreadAfterADelay(myEvent, 300);

                Check.That(myEvent).IsSetWithin(1, TimeUnit.Seconds);
            }
        }

        [Test]
        public void IsSetWithinThrowsExceptionWhenFailingForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.ThatCode(() =>
                {
                    Check.That(myEvent).IsSetWithin(10, TimeUnit.Milliseconds);
                })
                .IsAFailingCheckWithMessage("",
                        "The checked event has not been set before the given timeout (10 Milliseconds) whereas it must be.");
            }
        }

        [Test]
        public void NotIsSetWithinWorksForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.That(myEvent).Not.IsSetWithin(20, TimeUnit.Milliseconds);
            }
        }

        [Test]
        public void NotIsSetWithinThrowsExceptionForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.ThatCode(() =>
                    {
                        SetTheEventFromAnotherThreadAfterADelay(myEvent, 0);
                        Check.That(myEvent).Not.IsSetWithin(500, TimeUnit.Milliseconds);
                    })
                    .IsAFailingCheckWithMessage("",
                        "The checked event has been set before the given timeout (500 Milliseconds) whereas it must not.");
            }
        }


        [Test]
        public void IsNotSetWithinWorksForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.That(myEvent).IsNotSetWithin(10, TimeUnit.Milliseconds);
            }
        }

        [Test]
        public void IsNotSetWithinThrowsExceptionWhenFailingForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.ThatCode(() =>
                {
                    SetTheEventFromAnotherThreadAfterADelay(myEvent, 0);
                    Check.That(myEvent).IsNotSetWithin(200, TimeUnit.Milliseconds);
                })
                .IsAFailingCheckWithMessage("",
                        "The checked event has been set before the given timeout (200 Milliseconds) whereas it must not.");
            }
        }

        [Test]
        public void NotIsNotSetWithinWorksForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                SetTheEventFromAnotherThreadAfterADelay(myEvent, 0);
                Check.That(myEvent).Not.IsNotSetWithin(200, TimeUnit.Milliseconds);
            }
        }

        [Test]
        public void NotIsNotSetWithinThrowsExceptionForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.ThatCode(() =>
                {
                    Check.That(myEvent).Not.IsNotSetWithin(10, TimeUnit.Milliseconds);
                })
                .IsAFailingCheckWithMessage("",
                        "The checked event has not been set before the given timeout (10 Milliseconds) whereas it must be.");
            }
        }

        #endregion

        #region helpers

        private static void SetTheEventFromAnotherThreadAfterADelay(AutoResetEvent myEvent, int delayBeforeEventIsSetInMilliseconds)
        {
            var signal = new object();
            var started = false;
            var otherThread = new Thread(() =>
            {
                lock (signal)
                {
                    started = true;
                    Monitor.PulseAll(signal);
                }
                Thread.Sleep(delayBeforeEventIsSetInMilliseconds);
                try
                {
                    myEvent.Set();
                }
                catch (ObjectDisposedException)
                {
                    // test failed somehow
                }
            });
            otherThread.Start();
            // we wait for the thread to be actually started to reduce risk of false negative due to CPU contention
            lock (signal)
            {
                if (!started)
                {
                    Monitor.Wait(signal, 10000);
                }
            }
        }

        #endregion
    }
}
