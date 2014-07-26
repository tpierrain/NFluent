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
        private bool sideEffectAchieved;

        #region async methods

        [Test]
        public void CheckThatCodeOnFinishedAsyncMethodReturnsAggregateExceptionInsteadOfTheOriginalExceptionType()
        {
            // bad way for async methods
            Check.ThatCode(this.DoSomethingBadAsync().Wait).Throws<AggregateException>();
        }

        [Test]
        public void CheckThatAsyncCodeOnAsyncMethodReturnsTheOriginalExceptionType()
        {
            // proper way for async methods
            Check.ThatAsyncCode(this.DoSomethingBadAsync).Throws<SecurityException>();
        }

        [Test]
        public void CheckThatAsyncCodeReturnsAtTheEndOfTheAsyncMethod()
        {
            Check.That(this.sideEffectAchieved).IsFalse();

            Check.ThatAsyncCode(this.SideEffectAsync).DoesNotThrow();

            Check.That(this.sideEffectAchieved).IsTrue();
        }

        #endregion

        #region async functions

        [Test]
        public void CheckThatAsyncCodeOnAsyncFunctionReturnsTheOriginalExceptionType()
        {
            // proper way for async function
            Check.ThatAsyncCode((AwaitableFunction<int>)this.DoSomethingBadBeforeTheAnswerAsync).Throws<SecurityException>();
        }

        [Test]
        public void CheckThatAsyncCodeWorksForFunctions()
        {
            // proper way for async function
            Check.ThatAsyncCode((AwaitableFunction<int>)this.ThinkAndReturnTheAnswerAsync).DoesNotThrow().And.WhichResult().IsEqualTo(42);
        }

        #endregion

        private async Task DoSomethingBadAsync()
        {
            await Task.Run(() =>
            {
                // This operation takes a while
                Thread.Sleep(100);
                throw new SecurityException("Drop your weapon!!!");
            });
        }

        private async Task SideEffectAsync()
        {
            await Task.Run(() =>
            {
                // This operation takes a while
                Thread.Sleep(100);
                this.sideEffectAchieved = true;
            });
        }

        private async Task<int> ThinkAndReturnTheAnswerAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(100);
            });

            return 42;
        }

        private async Task<int> DoSomethingBadBeforeTheAnswerAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(100);
                throw new SecurityException("Too bad mate: you've been busted!");
            });

            return 42;
        }
    }
}
