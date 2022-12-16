// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionHelperTests.cs" company="">
//   Copyright 2013 Cyrille DUPUYDAUBY, Rui CARVALHO, Thomas PIERRAIN
//   // //   Licensed under the Apache License, Version 2.0 (the "License");
//   // //   you may not use this file except in compliance with the License.
//   // //   You may obtain a copy of the License at
//   // //       http://www.apache.org/licenses/LICENSE-2.0
//   // //   Unless required by applicable law or agreed to in writing, software
//   // //   distributed under the License is distributed on an "AS IS" BASIS,
//   // //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   // //   See the License for the specific language governing permissions and
//   // //   limitations under the License.
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NFluent.Tests
{
    using System;
    using System.Threading;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class ExceptionHelperTests
    {
        [Test]
        public void Should_Dump_InnerException_stack_trace()
        {
            var exception = new ArgumentException("blahblah#1", new ArgumentOutOfRangeException("blahblah#2", new Exception("blahblah#3")));

            Check.That(ExceptionHelper.DumpInnerExceptionStackTrace(exception)).IsEqualTo("{ System.ArgumentOutOfRangeException } \"blahblah#2\"" +Environment.NewLine +"--> { System.Exception } \"blahblah#3\"");
        }

        [Test]
        public void Should_detect_NUnit()
        {
            var ex = ExceptionHelper.BuildException("the message");
            var ex2 = ExceptionHelper.BuildException("the message");
#if NETCOREAPP1_0 || NETCOREAPP1_1
            Check.That(ex.GetType().FullName).IsEqualTo("NFluent.Kernel.FluentCheckException");
#else    
            // test is relaxed due to issue on Xamarin
            Check.That(ex.GetType().FullName).IsEqualTo("NUnit.Framework.AssertionException");
            ////Check.That(ex).IsInstanceOf<AssertionException>();
#endif
        }
        
        [Test]
        public void CanBeInvokedFromMultipleThreads()
        {
            // Explicitly avoid using NFluent Check because ExceptionHelper is the SUT.
            
            var exceptionHelperInvoker1 = new ExceptionHelperInvoker();
            var exceptionHelperInvoker2 = new ExceptionHelperInvoker();
            
            exceptionHelperInvoker1.Start();
            exceptionHelperInvoker2.Start();
            
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            
            exceptionHelperInvoker1.Stop();
            exceptionHelperInvoker2.Stop();

            Assert.That(exceptionHelperInvoker1.Error, Is.Null);
            Assert.That(exceptionHelperInvoker2.Error, Is.Null);
        }

        private class ExceptionHelperInvoker
        {
            private Thread thread;
            private volatile bool running; 
            
            public Exception Error { get; private set; }
            
            public void Start()
            {
                this.running = true;
                this.thread = new Thread(this.Run);
                this.thread.Start();
            }

            private void Run()
            {
                try
                {
                    while (this.running)
                    {
                        ExceptionHelper.ResetCache();
                        
                        var exception = ExceptionHelper.BuildException("Error");
                        Assert.That(exception, Is.Not.Null);
                    }
                }
                catch (Exception e)
                {
                    this.Error = e;
                }
            }

            public void Stop()
            {
                this.running = false;
                
                // Use a large timeout to avoid flaky tests on slow CI servers.
                if (!this.thread.Join(TimeSpan.FromMilliseconds(500)))
                    throw new TimeoutException($"Unable to join {nameof(ExceptionHelperInvoker)} thread");
            }
        }
    }
}
