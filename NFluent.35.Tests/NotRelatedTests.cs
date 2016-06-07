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
namespace NFluent.Tests
{
    using ForDocumentation;

    using NUnit.Framework;

    [TestFixture]
    public class NotRelatedTests
    {
        [Test]
        public void CheckContextWorks()
        {
            Assert.IsTrue(CheckContext.DefaulNegated);
            CheckContext.DefaulNegated = false;
            try
            {
               Assert.IsFalse(CheckContext.DefaulNegated);
            }
            finally
            {
                CheckContext.DefaulNegated = true;
            }
        }

        [Test]
        public void NotIsWorking()
        {
            Check.That("Batman and Robin").Not.Contains("Joker");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string contains unauthorized value(s): \"Batman\"\nThe checked string:\n\t[\"Batman and Robin\"]\nThe unauthorized substring(s):\n\t[\"Batman\"]")]
        public void NotThrowsException()
        {
            Check.That("Batman and Robin").Not.Contains("Batman");
        }

        [Test]
        public void CanCombineNotAndAndOperatorsInTheSameCheckStatement()
        {
            Check.That("Batman and Robin").Not.Contains("Joker").And.StartsWith("Bat").And.Not.Contains("Gandhi");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string contains unauthorized value(s): \"Robin\"\nThe checked string:\n\t[\"Batman and Robin\"]\nThe unauthorized substring(s):\n\t[\"Robin\"]")]
        public void ThrowsProperExceptionWhenCombineNotAndAndOperatorsInTheSameCheckStatement()
        {
            Check.That("Batman and Robin").Not.Contains("Joker").And.StartsWith("Bat").And.Not.Contains("Robin");
        }

        [Test]
        [Explicit("Experimental: to check if negation works")]
        public void ForceNegationOnAllTest()
        {
            if (!CheckContext.DefaulNegated)
            {
                return;
            }

            CheckContext.DefaulNegated = false;
            try
            {
                RunnerHelper.RunAllTests(false);
            }
            finally
            {
                CheckContext.DefaulNegated = true;
            }
        }
    }
}
