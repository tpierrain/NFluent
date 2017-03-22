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
using System;

namespace NFluent.Tests
{
    using System.Threading;
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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine+ "The checked event has not been set before the given timeout." + Environment.NewLine + "The given timeout (in msec):" + Environment.NewLine + "\t[10]");
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
                    Check.That(myEvent).Not.IsSetWithin(50, TimeUnit.Milliseconds);
                })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine+ "The checked event has been set before the given timeout whereas it must not." + Environment.NewLine + "The given timeout (in msec):" + Environment.NewLine + "\t[50]");
            }
        }

        #endregion

        #region IsNotSetWithin

        [Test]
        public void IsNotSetWithinWorksForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.That(myEvent).IsNotSetWithin(1, TimeUnit.Seconds);
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
                    Check.That(myEvent).IsNotSetWithin(100, TimeUnit.Milliseconds);
                })
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine+ "The checked event has been set before the given timeout." + Environment.NewLine + "The given timeout (in msec):" + Environment.NewLine + "\t[100]");
            }
        }

        [Test]
        public void NotIsNotSetWithinWorksForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                SetTheEventFromAnotherThreadAfterADelay(myEvent, 0);
                Check.That(myEvent).Not.IsNotSetWithin(100, TimeUnit.Milliseconds);
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
                .Throws<FluentCheckException>()
                .WithMessage(Environment.NewLine+ "The checked event has not been set before the given timeout whereas it must." + Environment.NewLine + "The given timeout (in msec):" + Environment.NewLine + "\t[10]");
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
                myEvent.Set();
            });
            otherThread.Start();
            // we wait for the thread to be actualy started to reduce risk of false negative due to CPU contention
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
