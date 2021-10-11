// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ContextualizedSingletonShould.cs" company="NFluent">
//   Copyright 2021 Thomas PIERRAIN & Cyrille DUPUYDAUBY
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Tests
{
    using System.Threading;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class ContextualizedSingletonShould
    {
        [Test]
        public void ProvidePlainSingleton()
        {
            var sut = new ContextualizedSingleton<int>
            {
                // declare a value
                DefaultValue = 1
            };

            Check.That(sut.Value).IsEqualTo(1);
        }

        [Test]
        public void SupportNestedChanges()
        {
            var sut = new ContextualizedSingleton<int>
            {
                // declare a value
                DefaultValue = 1
            };
            using (sut.ScopedCustomization(2))
            {
                using (sut.ScopedCustomization(3))
                {
                    Check.That(sut.Value).IsEqualTo(3);
                }
                Check.That(sut.Value).IsEqualTo(2);
            }
        }

        [Test]
        public void ProvideThreadSpecificSingleton()
        {
            var sut = new ContextualizedSingleton<int> { DefaultValue = 1 };
            var lck = new object();
            var started = false;
            ThreadPool.QueueUserWorkItem(sync =>
            {
                lock (sync)
                {
                    if (!started)
                    {
                        // wait for start signal
                        Monitor.Wait(sync);
                    }

                    using (sut.ScopedCustomization(5))
                    {
                        // check we modified our val
                        Check.That(sut.Value).IsEqualTo(5);
                        // signal value changed
                        Monitor.Pulse(sync);
                        // wait for end request
                        Monitor.Wait(sync);
                    }

                    // check val restored
                    Check.That(sut.Value).IsEqualTo(1);
                    // signal end
                    Monitor.Pulse(sync);
                }
            }, lck);

            lock (lck)
            {
                Monitor.Pulse(lck);
                started = true;
                // wait for start
                Monitor.Wait(lck);
                // check that our val is not changed
                Check.That(sut.Value).IsEqualTo(1);
                // signal end requested
                Monitor.Pulse(lck);
                // wait for end confirmation
                Monitor.Wait(lck);
            }

            // check that our val is not changed
            Check.That(sut.Value).IsEqualTo(1);
        }
    }
}