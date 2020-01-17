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
            }).
            ThrowsAny().
            AndWhichMessage().
            StartsWith(Environment.NewLine+ "The checked code raised an exception, whereas it must not.");
        }

#if !DOTNET_30 && !DOTNET_20
        [Test]
        public void SupportWhichMember()
        {
            Check.ThatCode(() => throw new ArgumentException("failed", "thearg")).Throws<ArgumentException>().
                WhichMember(e => e.ParamName).
                IsEqualTo("thearg");
        }

        [Test]
        public void WhichMembersGenerateAdequateMessage()
        {
            Check.ThatCode(()=>
            Check.ThatCode(() => throw new ArgumentException("failed", "thearg")).
                Throws<ArgumentException>().
                WhichMember(e => e.ParamName).
                IsEqualTo("the arg")).IsAFailingCheckWithMessage("", 
                "The checked value's ParamName is different from expected one.", 
                "The checked value's ParamName:", 
                "\t[\"thearg\"]", 
                "The expected value's ParamName:", 
                "\t[\"the arg\"]");
        }

        [Test]
        public  void WhichMemberShouldSupportVariousExtractor()
        {
            Check.ThatCode(()=>
                Check.ThatCode(() => throw new ArgumentException("failed", "thearg")).
                    Throws<ArgumentException>().
                    WhichMember(e => e.ParamName.ToUpper()).
                    IsEqualTo("the arg")).IsAFailingCheckWithMessage("", 
                "The checked value's e.ParamName.ToUpper() is different from expected one.", 
                "The checked value's e.ParamName.ToUpper():", 
                "\t[\"THEARG\"]", 
                "The expected value's e.ParamName.ToUpper():", 
                "\t[\"the arg\"]");
        }
        #endif

        [Test]
        public void ExpectedExceptionRaised()
        {
            Check.ThatCode(() => throw new InvalidOperationException()).Throws<InvalidOperationException>();
            Check.ThatCode(() => throw new Exception()).ThrowsAny();
        }

        [Test]
        public void DidNotRaiseExpected()
        {
            Check.ThatCode(() =>
                {
                    Check.ThatCode(() => throw new Exception()).Throws<InvalidOperationException>();
                }).
                IsAFailingCheckWithMessage("",
                    "The checked code's raised exception is of a different type than expected.",
                    "The checked code's raised exception:",
                    "*",
                    "The expected code's raised exception:",
                    "\tan instance of [System.InvalidOperationException]");
        }

        [Test]
        public void DidNotRaiseAny()
        {
            Check.ThatCode(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                Check.ThatCode(() => { new object(); }).ThrowsAny();
            }).
            IsAFailingCheckWithMessage(Environment.NewLine+ "The checked code did not raise an exception, whereas it must.");
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
                    "\tan instance of [System.Exception]");
        }
    }
}