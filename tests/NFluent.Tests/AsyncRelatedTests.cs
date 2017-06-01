// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="AsyncRelatedTests.cs" company="">
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

#if DOTNET_45 || NETCOREAPP1_0 
namespace NFluent.Tests
{
    using System;
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class AsyncRelatedTests
    {
#region Fields

        private bool sideEffectAchieved;

#endregion

#region Public Methods and Operators
        
        [Test]
        public void ShouldNotUseCheckThatCodeForAsyncMethods()
        {
            // Bad way for async methods since it does not catch the proper exception, but 
            // the TPL AggregateException wrapper instead
            Check.ThatCode(this.DoSomethingBadAsync().Wait).Throws<AggregateException>();
        }

        [Test]
        public void CheckThatAsyncCodeOnAsyncFunctionReturnsTheOriginalExceptionType()
        {
            // proper way for async function
            Check.ThatAsyncCode(this.DoSomethingBadAfterAWhileAndBeforeAnsweringAsync).Throws<SecurityException>();
        }

        [Test]
        public void CheckThatAsyncCodeOnAsyncMethodReturnsTheOriginalExceptionType()
        {
            // proper way for async methods
            Check.ThatAsyncCode(this.DoSomethingBadAsync).Throws<SecurityException>();
        }

        [Test]
        public void CheckThatAsyncCodeReturnsWhenTheAsyncMethodHasCompleted()
        {
            Check.That(this.sideEffectAchieved).IsFalse();

            Check.ThatAsyncCode(this.SideEffectAsync).DoesNotThrow();

            Check.That(this.sideEffectAchieved).IsTrue();
        }

        [Test]
        public void CheckThatAsyncCodeWorksAlsoWithAsyncLambda()
        {
            Check.ThatAsyncCode(async () =>
            {
                                                 await Task.Run(() => Thread.Sleep(500));
                                                throw new SecurityException("Freeze motha...");
                                            }).Throws<SecurityException>();
        }

        [Test]
        public void CheckThatAsyncCodeWorksAlsoWithSyncLambda()
        {
            Check.ThatAsyncCode(() =>
            {
                Task.Run(() => Thread.Sleep(10));
                throw new SecurityException("Freeze motha...");
            }).Throws<SecurityException>();
        }
        [Test]
        public void CheckThatAsyncCodeWorksForFunctions()
        {
            // proper way for async function
            Check.ThatAsyncCode(this.ReturnTheAnswerAfterAWhileAsync).DoesNotThrow().And.WhichResult().IsEqualTo(42);
        }

#endregion

#region Methods

        private async Task<int> DoSomethingBadAfterAWhileAndBeforeAnsweringAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(100);
                throw new SecurityException("Too bad mate: you've been busted before having the answer!");
            });

            return 42;
        }

        private async Task DoSomethingBadAsync()
        {
            await Task.Run(() =>
            {
                // This operation takes a while
                Thread.Sleep(100);
                throw new SecurityException("Drop your weapon!!!");
            });
        }

        private async Task<int> ReturnTheAnswerAfterAWhileAsync()
        {
            await Task.Run(() => Thread.Sleep(100));

            return 42;
        }

        private async Task SideEffectAsync()
        {
            await Task.Run(() => Thread.Sleep(500));

            this.sideEffectAchieved = true;
        }

#endregion
    }
}
#endif
