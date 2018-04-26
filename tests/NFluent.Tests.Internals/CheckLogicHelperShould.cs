// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CheckLogicHelperShould.cs" company="NFluent">
//   Copyright 2018 Cyrille DUPUYDAUBY
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
    using Extensibility;
    using Helpers;
    using Kernel;
    using NUnit.Framework;

    internal class CheckLogicHelperShould
    {
        [Test]
        public void
            BeEasyForBasicChecks()
        {
            var fluentChecker = new FluentCheck<string>("test");
            var checker = ExtensibilityHelper.ExtractChecker(fluentChecker.Not);

            checker.BeginCheck()
                .Expecting("other")
                .FailsIf((x) => x.Equals("test"), "The {0} should be equal to the {1}.")
                .EndCheck();
        }

        class Point
        {
            public int x = 4;
        }
        [Test]
        public void
            CascadeErrorOnSubProperty()
        {
            var fluentChecker = new FluentCheck<Point>(null);
            var checker = fluentChecker.Checker;

            Check.ThatCode(() =>
                checker.BeginCheck().FailsIfNull().GetSutProperty(point => point.x, "x coordinate")
                    .FailsIf(i => i > 0, "Should be positive").EndCheck()).IsAFaillingCheckWithMessage("", "The checked value's x coordinate is null.");
        }
    }
}