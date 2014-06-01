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
namespace NFluent.Tests
{
    using System.Threading;
    using NUnit.Framework;

    [TestFixture]
    public class EventWaitHandleTests
    {
        #region IsSetBefore

        [Test]
        public void IsSetBeforeWorksForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                SetTheEventFromAnotherThreadAfterADelay(myEvent, 300);

                Check.That(myEvent).IsSetBefore(1000);
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentCheckException), ExpectedMessage = "\nThe checked event has not been set before the given timeout.\nThe given timeout (in msec):\n\t[10]")]
        public void IsSetBeforeThrowsExceptionWhenFailingForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.That(myEvent).IsSetBefore(10);
            }
        }

        [Test]
        public void NotIsSetBeforeWorksForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.That(myEvent).Not.IsSetBefore(20);
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentCheckException), ExpectedMessage = "\nThe checked event has been set before the given timeout whereas it must not.\nThe given timeout (in msec):\n\t[30]")]
        public void NotIsSetBeforeThrowsExceptionForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                SetTheEventFromAnotherThreadAfterADelay(myEvent, 0);
                Check.That(myEvent).Not.IsSetBefore(30);
            }
        }

        #endregion

        #region IsNotSetBefore

        [Test]
        public void IsNotSetBeforeWorksForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.That(myEvent).IsNotSetBefore(1000);
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentCheckException), ExpectedMessage = "\nThe checked event has been set before the given timeout.\nThe given timeout (in msec):\n\t[100]")]
        public void IsNotSetBeforeThrowsExceptionWhenFailingForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                SetTheEventFromAnotherThreadAfterADelay(myEvent, 0);
                Check.That(myEvent).IsNotSetBefore(100);
            }
        }

        [Test]
        public void NotIsNotSetBeforeWorksForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                SetTheEventFromAnotherThreadAfterADelay(myEvent, 0);
                Check.That(myEvent).Not.IsNotSetBefore(20);
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FluentCheckException), ExpectedMessage = "\nThe checked event has not been set before the given timeout whereas it must.\nThe given timeout (in msec):\n\t[10]")]
        public void NotIsNotSetBeforeThrowsExceptionForAutoResetEvent()
        {
            using (var myEvent = new AutoResetEvent(false))
            {
                Check.That(myEvent).Not.IsNotSetBefore(10);
            }
        }

        #endregion


        #region helpers

        private static void SetTheEventFromAnotherThreadAfterADelay(AutoResetEvent myEvent, int delayBeforeEventIsSetInMilliseconds)
        {
            var otherThread = new Thread(() =>
            {
                Thread.Sleep(delayBeforeEventIsSetInMilliseconds);
                myEvent.Set();
            });
            otherThread.Start();
        }

        #endregion
    }
}
