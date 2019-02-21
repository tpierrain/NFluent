// // --------------------------------------------------------------------------------------------------------------------
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
    using System.Diagnostics;
    using Helpers;
    using NUnit.Framework;
    using ApiChecks;
    using NFluent.Helpers;


    [TestFixture]
    public class ExceptionTests
    {
        [Test]
        public void NoExceptionRaised()
        {
            Check.ThatCode(() => new object()).DoesNotThrow();
        }

        [Test]
        public void UnexpectedExceptionRaised()
        {
            Check.ThatCode(() =>
            {
                Check.ThatCode(() => throw new Exception()).DoesNotThrow();
            })
            .ThrowsAny()
            .AndWhichMessage().StartsWith(Environment.NewLine+ "The checked code raised an exception, whereas it must not."); // TODO: reproduce startWith
        }

        [Test]
        public void ExpectedExceptionRaised()
        {
            Check.ThatCode(() => { throw new InvalidOperationException(); }).Throws<InvalidOperationException>();
            Check.ThatCode(() => { throw new Exception(); }).ThrowsAny();
        }

        [Test]
        public void DidNotRaiseExpected()
        {
            Check.ThatCode(() =>
                {
                    Check.ThatCode(() => { throw new Exception(); }).Throws<InvalidOperationException>();
                })
                .IsAFailingCheckWithMessage("",
                    "The checked code's raised exception is of a different type than expected.",
                    "The checked code's raised exception:",
                    "*",
                    "The expected code's raised exception:",
                    "\tan instance of type: [System.InvalidOperationException]");
        }

        [Test]
        public void DidNotRaiseAny()
        {
            Check.ThatCode(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                Check.ThatCode(() => { new object(); }).ThrowsAny();
            })
            .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked code did not raise an exception, whereas it must.");
        }

        [Test]
        public void DidNotRaiseAnyTypedCheck()
        {
            Check.ThatCode(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                Check.ThatCode(() => { new object(); }).Throws<Exception>();
            })
            .IsAFailingCheckWithMessage("",
                    "The checked code did not raise an exception, whereas it must.",
                    "The expected code's raised exception:",
                    "\tan instance of type: [System.Exception]");
        }
    }
}