// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AsyncCodeChecksShould.cs" company="NFluent">
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

#if !DOTNET_35
namespace NFluent.Tests
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;

    public class AsyncBase
    {
        [AsyncStateMachine(typeof(int))]
        protected virtual Task PseudoAsyncMethod()
        {
            return null;
        }
    }

    [TestFixture]
    public class AsyncCodeChecksShould: AsyncBase
    {

        private bool sideEffectAchieved;

        [Test]
        public void ShouldNotUseCheckThatCodeForAsyncMethods()
        {
            Check.ThatCode(BadAsync).DoesNotThrow();
            // Bad way for async methods since it does not catch the proper exception, but 
            // the TPL AggregateException wrapper instead
            Check.ThatCode(DoSomethingBadAsync().Wait).Throws<AggregateException>();
            // this should work without waiting
            Check.ThatCode(DoSomethingBadAsync).Throws<SecurityException>();
            Check.ThatCode(ReturnTheAnswerAfterAWhileAsync).DoesNotThrow();
        }

        [Test]
        public void EdgeCasesForPFakeAsync()
        {
            // pseudo async method
            Check.ThatCode(PseudoAsyncMethod).DoesNotThrow();
        }

        // this attribute is here as an attempt to fool the async method detection
        [DebuggerStepThrough]
        protected override Task PseudoAsyncMethod()
        {
            return new Task(() => { });
        }

        [Test]
        public void CheckThatAsyncCodeOnAsyncFunctionReturnsTheOriginalExceptionType()
        {
            // proper way for async function
            Check.ThatCode(DoSomethingBadAfterAWhileAndBeforeAnsweringAsync).Throws<SecurityException>();
        }

        [Test]
        public void CheckThatAsyncCodeOnAsyncMethodReturnsTheOriginalExceptionType()
        {
            // proper way for async methods
            Check.ThatCode(DoSomethingBadAsync).Throws<SecurityException>();
        }

        [Test]
        public void CheckThatAsyncCodeReturnsWhenTheAsyncMethodHasCompleted()
        {
            Check.That(this.sideEffectAchieved).IsFalse();

            Check.ThatCode(this.SideEffectAsync).DoesNotThrow();

            Check.That(this.sideEffectAchieved).IsTrue();
        }

        [Test]
        public void CheckThatAsyncCodeWorksAlsoWithAsyncLambda()
        {
            Check.ThatCode(async () =>
            {
               await Task.Run(() => Thread.Sleep(150));
               throw new SecurityException("Freeze motha...");
             }).Throws<SecurityException>();
        }

        [Test]
        public void CheckThatAsyncCodeWorksAlsoWithSyncLambda()
        {
            Check.ThatCode(() =>
            {
                Task.Run(() => Thread.Sleep(10));
                throw new SecurityException("Freeze motha...");
            }).Throws<SecurityException>();
        }

        [Test]
        public void CheckThatAsyncCodeWorksForFunctions()
        {
            // proper way for async function
            Check.ThatCode(ReturnTheAnswerAfterAWhileAsync).DoesNotThrow().And.WhichResult().IsEqualTo(42);
        }
        
        [Test]
        public void CheckThatAsyncCodeWorksWithLestLass()
        {
            Check.ThatCode(async () =>
            {
                var _ = await ReturnTheAnswerAfterAWhileAsync();
            }).LastsLessThan(150, TimeUnit.Milliseconds);
        }

        // useless attribute is used to make sure 'async' related attribute is correctly detected when multiple attributes are present
        [Ignore("this is an arbitrary attribute")]
        private static async void BadAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(10);
            });
        }

        private static async Task<int> DoSomethingBadAfterAWhileAndBeforeAnsweringAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(20);
                throw new SecurityException("Too bad mate: you've been busted before having the answer!");
            });

            return 42;
        }

        private static async Task DoSomethingBadAsync()    
        {
            await Task.Run(() =>
            {
                // This operation takes a while
                Thread.Sleep(20);
                throw new SecurityException("Drop your weapon!!!");
            });
        }

        private static async Task<int> ReturnTheAnswerAfterAWhileAsync()
        {
            await Task.Run(() => Thread.Sleep(20));

            return 42;
        }

        private async Task SideEffectAsync()
        {
            await Task.Run(() => Thread.Sleep(100));

            this.sideEffectAchieved = true;
        }
    }
}
#endif