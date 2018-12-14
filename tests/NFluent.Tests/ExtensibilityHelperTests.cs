// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ExtensibilityHelperTests.cs" company="">
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
    using System;
    using Extensibility;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class ExtensibilityHelperTests
    {
        [Test]
        public void ExtractcheckerWorks()
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
            Check.ThatCode(() => { block.EndCheck(); }).Throws<InvalidOperationException>();
        }


        [Test]
        public void IsAFaillingCheckReportsProperError()
        {
            Check.ThatCode(() =>
                // check with an incomplete error message
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"))
                    .IsAFaillingCheckWithMessage("oups", "and more")
            ).IsAFaillingCheck();            
            
            Check.ThatCode(() =>
                // check with an incorrect error message
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"))
                    .IsAFaillingCheckWithMessage("oupsla")
            ).IsAFaillingCheck();

            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"+Environment.NewLine+"and more"))
                    .IsAFaillingCheckWithMessage("oupsla")
            ).IsAFaillingCheck();
            // can use regular expression.
            Check.ThatCode(() => throw ExceptionHelper.BuildException("oups")).
                IsAFaillingCheckWithMessage("#[pous]+");
            Check.ThatCode(() =>
                // check with a error message that is too long
                Check.ThatCode(() => throw ExceptionHelper.BuildException("oups"+Environment.NewLine+"and more"))
                    .IsAFaillingCheckWithMessage("#[pous]+")
            ).IsAFaillingCheck();
        }
    }
}
