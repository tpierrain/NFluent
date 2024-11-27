// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensibilityHelperTests.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
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

    [TestFixture]
    public class ExtensibilityHelperTests
    {
        [Test]
        public void ExtractCheckerWorks()
        {
            var checker = ExtensibilityHelper.ExtractChecker(Check.That("kamoulox"));
            Check.That(checker).IsNotNull();
            Check.That(checker.Negated).IsFalse();
            Check.That(checker.Value).IsEqualTo("kamoulox");
        }

        [Test]
        public void ExtractRunnableStructCheckWorks()
        {
            var runnableStructCheck = ExtensibilityHelper.ExtractChecker(Check.That(Nationality.Chinese));
            Check.That(runnableStructCheck.Value).IsEqualTo(Nationality.Chinese);
            Check.That(runnableStructCheck.Negated).IsFalse();
        }

        [Test]
        public void NewCheckBuilderShouldWhenNegationMessageNotDefined()
        {
            var block = ExtensibilityHelper.BeginCheck(Check.That("kamoulox").Not);
            Check.ThatCode(() => { block.EndCheck(); }).Throws<InvalidOperationException>().WithMessage("Negated error message was not specified. Use 'OnNegate' method to specify one.");
        }

        [Test]
        public void ShouldThrowOnFailure()
        {
            bool thrown;
            try
            {
                Check.ThatCode(() => 0).IsAFailingCheckWithMessage("don't care");
                thrown = false;
            }
            catch (Exception)
            {
                thrown = true;
            }
            Assert.That(thrown, "Should have thrown an exception to mark failure.");
        }

            [Test]
        public void IsAFailingCheckReportsProperError()
        {
            Check.ThatCode(() => Check.ThatCode(() => 0).IsAFailingCheckWithMessage("don't care"))
                .IsAFailingCheckWithMessage("", 
                    "The check succeeded whereas it should have failed.", 
                    "The expected fluent check's raised exception:", 
                    Criteria.FromRegEx("\tan instance of .*"));

            Check.ThatCode(() =>
                // check with an incomplete error message
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"))
                    .IsAFailingCheckWithMessage("oups", "and more")
            ).IsAFailingCheckWithMessage("",
                "Lines are missing in the error message starting at #1",
                "The checked fluent check's raised exception's error message:",
                "\t[\"oups\"]",
                "The expected fluent check's raised exception's error message:",
                "\t[\"oups",
                "and more\"]");

            Check.ThatCode(() =>
                // check with an incorrect error message
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"+Environment.NewLine+"and more"))
                    .IsAFailingCheckWithMessage("oupsla"+Environment.NewLine+"and mere")
            ).IsAFailingCheckWithMessage("", 
                "Line 0 is different from what is expected", 
                "Act:oups", 
                "Exp:oupsla", 
                "The checked fluent check's raised exception's error message:", 
                "\t[\"oups", 
                "and more\"]", 
                "The expected fluent check's raised exception's error message:", 
                "\t[\"oupsla",
                 "and mere\"]");

            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"+Environment.NewLine+"and more"))
                    .IsAFailingCheckWithMessage("oupsla")
            ).IsAFailingCheck();
            // can use regular expression.
            Check.ThatCode(() => throw ExceptionHelper.BuildException("oups")).
                IsAFailingCheckWithMessage(Criteria.FromRegEx("[pous]+"));
            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"+Environment.NewLine+"and more"))
                    .IsAFailingCheckWithMessage(Criteria.FromRegEx("[pous]+"))
            ).IsAFailingCheckWithMessage("", 
                "Too many lines in the error message starting at #1", 
                "The checked fluent check's raised exception's error message:", 
                "\t[\"oups", 
                "and more\"]", 
                "The expected fluent check's raised exception's error message:", 
                "\t[\"matches: [pous]+\"]");
            Check.ThatCode(() =>
                // check with a error message that does not match regex
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oupsla"))
                    .IsAFailingCheckWithMessage(Criteria.FromRegEx("[pous]+$"))
            ).IsAFailingCheckWithMessage("", 
                "Line 0 is different from what is expected", 
                "Act:oupsla", 
                "Exp:matches: [pous]+$", 
                "The checked fluent check's raised exception's error message:", 
                "\t[\"oupsla\"]", 
                "The expected fluent check's raised exception's error message:", 
                "\t[\"matches: [pous]+$\"]");

            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw new Exception("oups"))
                    .IsAFailingCheckWithMessage(Criteria.FromRegEx("[pous]+"))
            ).IsAFailingCheckWithMessage("", 
                "The exception raised is not of the expected type.", 
                "The checked fluent check's raised exception:", 
                "\t[{System.Exception}: 'oups'] of type: [System.Exception]", 
                "The expected fluent check's raised exception:", 
                Criteria.FromRegEx("\tan instance of .*"));
        }

        [Test]
        public void IsAFailingAssumptionReportsProperError()
        {
            Check.ThatCode(() => Assuming.ThatCode(() => 0).IsAFailingCheckWithMessage("don't care"))
                .IsAFailingAssumptionWithMessage("", 
                    "The check succeeded whereas it should have failed.", 
                    "The expected fluent check's raised exception:", 
                    Criteria.FromRegEx("\tan instance of .*"));

            Check.ThatCode(() =>
                // check with an incomplete error message
                Check.ThatCode(() => throw ExceptionHelper.BuildInconclusiveException("oups"))
                    .IsAFailingAssumptionWithMessage("oups", "and more")
            ).IsAFailingCheckWithMessage("", 
                "Lines are missing in the error message starting at #1", 
                "The checked fluent assumption's raised exception's error message:", 
                "\t[\"oups\"]", 
                "The expected fluent assumption's raised exception's error message:", 
                "\t[\"oups",
                "and more\"]");
            
            Check.ThatCode(() =>
                // check with an incorrect error message
                Check.ThatCode(() => throw ExceptionHelper.BuildInconclusiveException("oups"))
                    .IsAFailingAssumptionWithMessage("oupsla")
            ).IsAFailingCheckWithMessage("", 
                "Line 0 is different from what is expected", 
                "Act:oups", 
                "Exp:oupsla", 
                "The checked fluent assumption's raised exception's error message:", 
                "\t[\"oups\"]", 
                "The expected fluent assumption's raised exception's error message:", 
                "\t[\"oupsla\"]");

 
            Check.ThatCode(() =>
                // check with an incorrect error message
                Check.ThatCode(() => throw ExceptionHelper.BuildInconclusiveException("oups"+Environment.NewLine+"and more"))
                    .IsAFailingAssumptionWithMessage("oupsla"+Environment.NewLine+"and mere")
            ).IsAFailingCheckWithMessage("", 
                "Line 0 is different from what is expected", 
                "Act:oups", 
                "Exp:oupsla", 
                "The checked fluent assumption's raised exception's error message:", 
                "\t[\"oups", 
                "and more\"]", 
                "The expected fluent assumption's raised exception's error message:", 
                "\t[\"oupsla",
                "and mere\"]");
            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw ExceptionHelper.BuildInconclusiveException("oups"+Environment.NewLine+"and more"))
                    .IsAFailingAssumptionWithMessage("oupsla")
            ).IsAFailingCheck();
            // can use regular expression.
            Check.ThatCode(() => throw ExceptionHelper.BuildInconclusiveException("oups")).
                IsAFailingAssumptionWithMessage( Criteria.FromRegEx("[pous]+"));
            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw ExceptionHelper.BuildInconclusiveException("oups"+Environment.NewLine+"and more"))
                    .IsAFailingAssumptionWithMessage(Criteria.FromRegEx("[pous]+"))
            ).IsAFailingCheckWithMessage("", 
                "Too many lines in the error message starting at #1", 
                "The checked fluent assumption's raised exception's error message:", 
                "\t[\"oups",
                "and more\"]", 
                "The expected fluent assumption's raised exception's error message:", 
                "\t[\"matches: [pous]+\"]");
            Check.ThatCode(() =>
                // check with a error message that does not match regex
                Check.ThatCode(() => throw ExceptionHelper.BuildInconclusiveException("oupsla"))
                    .IsAFailingAssumptionWithMessage(Criteria.FromRegEx("[pous]+$"))
            ).IsAFailingCheckWithMessage("", 
                "Line 0 is different from what is expected", 
                "Act:oupsla", 
                "Exp:matches: [pous]+$", 
                "The checked fluent assumption's raised exception's error message:", 
                "\t[\"oupsla\"]", 
                "The expected fluent assumption's raised exception's error message:", 
                "\t[\"matches: [pous]+$\"]");

            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw new Exception("oups"))
                    .IsAFailingAssumptionWithMessage(Criteria.FromRegEx("[pous]+"))
            ).IsAFailingCheckWithMessage("", 
                "The exception raised is not of the expected type.", 
                "The checked fluent assumption's raised exception:", 
                "\t[{System.Exception}: 'oups'] of type: [System.Exception]", 
                "The expected fluent assumption's raised exception:", 
                Criteria.FromRegEx("\tan instance of .*")
                );
        }

        [Test]
        public void IsAFailingCheckShouldFailWithProperMessage()
        {
            Check.ThatCode(
                    () => Check.ThatCode(() => { }).IsAFailingCheck()).
                IsAFailingCheckWithMessage("", 
                    "The fluent check did not raise an exception, where as it must.", 
                    "The expected fluent check's raised exception:", 
                    Criteria.FromRegEx("\tan instance of \\[.*Exception\\]"));
            Check.ThatCode(
                    () => Check.ThatCode(() => throw new Exception("yep")).IsAFailingCheck()).
                IsAFailingCheckWithMessage(	"", 
                    "The exception raised is not of the expected type", 
                    "The checked fluent check's raised exception:", 
                    "\t[{System.Exception}: 'yep'] of type: [System.Exception]", 
                    "The expected fluent check's raised exception:", 
                    Criteria.FromRegEx("\tan instance of \\[.*Exception\\]"));
        }
    }
}
