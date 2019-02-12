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
            var runnableStructCheck = ExtensibilityHelper.ExtractStructChecker(Check.ThatEnum(Nationality.Chinese));
            Check.ThatEnum(runnableStructCheck.Value).IsEqualTo(Nationality.Chinese);
            Check.That(runnableStructCheck.Negated).IsFalse();
        }

        [Test]
        public void NewCheckBuilderShouldWhenNegationMessageNotDefined()
        {
            var block = ExtensibilityHelper.BeginCheck(Check.That("kamoulox").Not);
            Check.ThatCode(() => { block.EndCheck(); }).Throws<InvalidOperationException>().WithMessage("Negated error message was not specified. Use 'OnNegate' method to specify one.");
        }


        [Test]
        public void IsAFailingCheckReportsProperError()
        {
            Check.ThatCode(() => Check.ThatCode(() => 0).IsAFailingCheckWithMessage("don't care"))
                .IsAFailingCheckWithMessage("", 
                    "The check succeeded whereas it should have failed.", 
                    "The expected fluent check's raised exception's error message:", 
                    "\t{\"don't care\"}");

            Check.ThatCode(() =>
                // check with an incomplete error message
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"))
                    .IsAFailingCheckWithMessage("oups", "and more")
            ).IsAFailingCheckWithMessage("", 
                "Lines are missing in the error message starting at #1", 
                "The checked fluent check's raised exception's error message:", 
                "\t{\"oups\"} (1 item)", 
                "The expected fluent check's raised exception's error message:", 
                "\t{\"oups\", \"and more\"}");
            
            Check.ThatCode(() =>
                // check with an incorrect error message
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"))
                    .IsAFailingCheckWithMessage("oupsla")
            ).IsAFailingCheckWithMessage("", 
                "Line 0 is different from what is expected", 
                "Act:oups", 
                "Exp:oupsla", 
                "The checked fluent check's raised exception's error message:", 
                "\t{\"oups\"} (1 item)", 
                "The expected fluent check's raised exception's error message:", 
                "\t{\"oupsla\"}");

            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"+Environment.NewLine+"and more"))
                    .IsAFailingCheckWithMessage("oupsla")
            ).IsAFaillingCheck();
            // can use regular expression.
            Check.ThatCode(() => throw ExceptionHelper.BuildException("oups")).
                IsAFailingCheckWithMessage("#[pous]+");
            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"+Environment.NewLine+"and more"))
                    .IsAFailingCheckWithMessage("#[pous]+")
            ).IsAFailingCheckWithMessage("", 
                "Too many lines in the error message starting at #1", 
                "The checked fluent check's raised exception's error message:", 
                "\t{\"oups\", \"and more\"} (2 items)", 
                "The expected fluent check's raised exception's error message:", 
                "\t{\"#[pous]+\"}");
            Check.ThatCode(() =>
                // check with a error message that does not match regex
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oupsla"))
                    .IsAFailingCheckWithMessage("#[pous]+$")
            ).IsAFailingCheckWithMessage("", 
                "Line 0 is different from what is expected", 
                "Act:oupsla", 
                "Exp (regex):#[pous]+$", 
                "The checked fluent check's raised exception's error message:", 
                "\t{\"oupsla\"} (1 item)", 
                "The expected fluent check's raised exception's error message:", 
                "\t{\"#[pous]+$\"}");

            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw new Exception("oups"))
                    .IsAFailingCheckWithMessage("#[pous]+")
            ).IsAFailingCheckWithMessage("", 
                "The exception raised is not of the expected type.", 
                "The checked fluent check's raised exception's error message:", 
                "\t{\"oups\"} (1 item)", 
                "The expected fluent check's raised exception's error message:", 
                "\t{\"#[pous]+\"}");
        }

        [Test]
        public void IsAFaillingCheckShouldFailWithProperMessage()
        {
            Check.ThatCode(
                    () => Check.ThatCode(() => { }).IsAFaillingCheck()).
                IsAFailingCheckWithMessage("", 
                    "The fluent check did not raise an exception, where as it must.", 
                    "The expected fluent check's raised exception:", 
                    "#\tan instance of type: \\[.*Exception\\]");
            Check.ThatCode(
                    () => Check.ThatCode(() => throw new Exception("yep")).IsAFaillingCheck()).
                IsAFailingCheckWithMessage(	"", 
                    "The exception raised is not of the expected type", 
                    "The checked fluent check's raised exception:", 
                    "\t[{System.Exception}: 'yep'] of type: [System.Exception]", 
                    "The expected fluent check's raised exception:", 
                "#\tan instance of type: \\[.*Exception\\]");
        }
    }
}
