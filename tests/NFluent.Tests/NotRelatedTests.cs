// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="NotRelatedTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
using System;

namespace NFluent.Tests
{
    using Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class NotRelatedTests
    {
        [Test]
        public void NotIsWorking()
        {
            Check.That("Batman and Robin").Not.Contains("Joker");
        }

        [Test]
        public void NotThrowsException()
        {
            Check.ThatCode(() =>
            {
                Check.That("Batman and Robin").Not.Contains("Batman");
            })
            .FailsWithMessage("", 
                    "The checked string contains unauthorized value(s): \"Batman\"", 
                    "The checked string:", 
                    "\t[\"Batman and Robin\"]",  
                    "The unauthorized substring(s):", 
                    "\t[\"Batman\"]");
        }

        [Test]
        public void CanCombineNotAndAndOperatorsInTheSameCheckStatement()
        {
            Check.That("Batman and Robin").Not.Contains("Joker").And.StartsWith("Bat").And.Not.Contains("Gandhi");
        }

        [Test]
        public void ThrowsProperExceptionWhenCombineNotAndAndOperatorsInTheSameCheckStatement()
        {
            Check.ThatCode(() =>
            {
                Check.That("Batman and Robin").Not.Contains("Joker").And.StartsWith("Bat").And.Not.Contains("Robin");
            })
            .FailsWithMessage(Environment.NewLine+ "The checked string contains unauthorized value(s): \"Robin\"" +Environment.NewLine +"The checked string:" + Environment.NewLine + "\t[\"Batman and Robin\"]" + Environment.NewLine + "The unauthorized substring(s):" + Environment.NewLine + "\t[\"Robin\"]");
        }

    }
}
