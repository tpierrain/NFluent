// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NFluentShould.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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

 namespace NFluent.Tests
{
    using System;
    using Extensibility;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture(Category = "Obsolete")]
    public class CheckerShould
    {
        [Test]
        public void HelpToCreateAnErrorMessage()
        {
            var check = Check.That(2);

            var checker = ExtensibilityHelper.ExtractChecker(check);
            var message = checker.BuildMessage("general error");
            Check.That(message).IsInstanceOf<FluentMessage>();
            Check.That(message.ToString()).AsLines().ContainsExactly("",
                "general error",
                "The checked value:",
                "\t[2]");
        }

        [Test]
        public void HandleProperlyNegationOnFailing()
        {
            var check = Check.That(2);
            var checker = ExtensibilityHelper.ExtractChecker(check.Not);
            
            checker.ExecuteNotChainableCheck(()=> throw ExceptionHelper.BuildException("oups"), "should have failed");
            Check.ThatCode(() =>
                checker.ExecuteNotChainableCheck(() => { }, "should have failed"))
                .IsAFaillingCheck();
        }

        [Test]
        public void HandleProperlyNonFailingChecksOnNonChainable()
        {
            var check = Check.That(2);

            ExtensibilityHelper.ExtractChecker(check).ExecuteNotChainableCheck(() => { }, "should have succeedeed");
            Check.ThatCode(() => ExtensibilityHelper.ExtractChecker(check).ExecuteNotChainableCheck(
                () => throw ExceptionHelper.BuildException("failed"), "should fail")).IsAFaillingCheck();
        }

        [Test]
        public void LetUnknownExceptionGetThrough()
        {
            var check = Check.That(2);
            Check.ThatCode(() =>
                    ExtensibilityHelper.ExtractChecker(check)
                        .ExecuteNotChainableCheck(() => throw new ArgumentException(), "should fails"))
                .Throws<ArgumentException>();
        }

        [Test]
        public void SupportExecuteAndProvideSubItem()
        {
            var check = Check.That(2);

            Check.That(
                ExtensibilityHelper.ExtractChecker(check)
                    .ExecuteCheckAndProvideSubItem(() => Check.That(2), "on negation")).
                InheritsFrom<ICheckLinkWhich<ICheck<int>, ICheck<int>>>();

        }
    }
}
