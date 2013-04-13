// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="LambdaRelatedTests.cs" company="">
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
    using System.Threading;

    using NFluent.Helpers;

    using NUnit.Framework;

    [TestFixture]
    public class LambdaRelatedTests
    {
        [Test]
        public void DurationTest()
        {
            Check.That(() => Thread.Sleep(30)).LastsLessThan(60, TimeUnit.Milliseconds);
        }

        [Test]
        [ExpectedException(exceptionType: typeof(FluentAssertionException))]
        public void FailDurationTest()
        {
            Check.That(() => Thread.Sleep(30)).LastsLessThan(0, TimeUnit.Milliseconds);
        }

        [Test]
        public void NoExceptionRaised()
        {
            Check.That(() => new object()).DoesNotThrow();
        }

        [Test]
        [ExpectedException(exceptionType: typeof(FluentAssertionException))]
        public void UnexpectedExceptionRaised()
        {
            Check.That(() => { throw new ApplicationException(); }).DoesNotThrow();
        }

        [Test]
        public void ExpectedExceptionRaised()
        {
            Check.That(() => { throw new ApplicationException(); }).Throws<ApplicationException>();
            Check.That(() => { throw new ApplicationException(); }).ThrowsAny();
        }

        [Test]
        [ExpectedException(exceptionType: typeof(FluentAssertionException))]
        public void DidNotRaiseExpected()
        {
            Check.That(() => { throw new Exception(); }).Throws<ApplicationException>();
        }

        [Test]
        [ExpectedException(exceptionType: typeof(FluentAssertionException))]
        public void DidNotRaiseAny()
        {
            Check.That(() => { new object(); }).ThrowsAny();
        }
        [Test]
        [ExpectedException(exceptionType: typeof(FluentAssertionException))]
        public void DidNotRaiseAnyTypedCheck()
        {
            Check.That(() => { new object(); }).Throws<Exception>();
        }
    }
}