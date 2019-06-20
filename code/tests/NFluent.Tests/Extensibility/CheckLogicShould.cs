// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CheckLogicShould.cs" company="NFluent">
//   Copyright 2019 Cyrille DUPUYDAUBY & Thomas PIERRAIN
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
    using NUnit.Framework;

    
    [TestFixture]
    internal class CheckLogicShould
    {
        [Test]
        public void BlockUseOfImpossibleNegations()
        {
            Check.ThatCode(() => Check.That(12).Not.CantBeNegatedCheck()).Throws<InvalidOperationException>()
                .WithMessage("CantBeNegated can't be used when negated");
        }

        [Test]
        public void ThrowsWhenDefiningNegatedLabelWhenNonNegateable()
        {
            Check.ThatCode(() => Check.That(12).CantBeNegatedThrowsOnInvalidConstruction()).Throws<InvalidOperationException>()
                .WithMessage("You must not provide a negated comparison label, as CantBeNegatedThrowsOnInvalidConstruction can't be used when negated");
        }
    }

    internal static class ExtensibilitySamples
    {
        public static ICheckLink<ICheck<int>> CantBeNegatedCheck(this ICheck<int> context)
        {
            ExtensibilityHelper.BeginCheck(context).CantBeNegated("CantBeNegated").FailIfNull().EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }

        public static ICheckLink<ICheck<int>> CantBeNegatedThrowsOnInvalidConstruction(this ICheck<int> context)
        {
            ExtensibilityHelper.BeginCheck(context).
                CantBeNegated("CantBeNegatedThrowsOnInvalidConstruction").
                DefineExpectedValue(12, "Dummy", "forbidden").
                EndCheck();
            return ExtensibilityHelper.BuildCheckLink(context);
        }
    }
}