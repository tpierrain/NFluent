﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ExceptionTests.cs" company="">
// //   Copyright 2013 Rui CARVALHO, Thomas PIERRAIN
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

    using NUnit.Framework;

    [TestFixture]
    public class ExceptionTests
    {
        [Test]
        public void NoExceptionRaised()
        {
            Check.ThatCode(() => new object()).DoesNotThrow();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.StartsWith, ExpectedMessage = "\nThe checked code raised an exception, whereas it must not.")]
        public void UnexpectedExceptionRaised()
        {
            Check.ThatCode(() => { throw new ApplicationException(); }).DoesNotThrow();
        }

        [Test]
        public void ExpectedExceptionRaised()
        {
            Check.ThatCode(() => { throw new InvalidOperationException(); }).Throws<InvalidOperationException>();
            Check.ThatCode(() => { throw new ApplicationException(); }).ThrowsAny();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.Contains, ExpectedMessage = "\nThe checked code raised an exception of a different type than expected.")]
        public void DidNotRaiseExpected()
        {
            Check.ThatCode(() => { throw new Exception(); }).Throws<ApplicationException>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked code did not raise an exception, whereas it must.")]
        public void DidNotRaiseAny()
        {
            Check.ThatCode(() => { new object(); }).ThrowsAny();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked code did not raise an exception, whereas it must.\nExpected exception type is:\n\t[System.Exception]")]
        public void DidNotRaiseAnyTypedCheck()
        {
            Check.ThatCode(() => { new object(); }).Throws<Exception>();
        }
    }
}