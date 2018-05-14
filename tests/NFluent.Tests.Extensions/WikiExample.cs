// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ICheckLogic.cs" company="NFluent">
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
    using NUnit.Framework;

    internal static class MyCheck
    {
        public static ICheckLink<ICheck<string>> IsThisNumber(this ICheck<string> check, int expected)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailIfNull()
                .FailWhen( sut => sut != expected.ToString(), "The {0} does not contain the {1}.")
                .DefineExpected(expected)
                .Negates("The {0} contains the {1} whereas it should not.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }
    }
    class WikiExampleShould
    {
        [Test]
        public void FailOnNull()
        {
            string nullString = null;
            Check.ThatCode(() =>
                    Check.That(nullString).IsThisNumber(12))
                .IsAFaillingCheck();
        }

        [Test]
        public void FailWhenNotExpectedValue()
        {
            string texte = "12";
            Check.ThatCode(() =>
                    Check.That(texte).IsThisNumber(14))
                .IsAFaillingCheck();
        }

        [Test]
        public void DoesNotFailWhenNegated()
        {
            string texte = "12";
            Check.ThatCode(() =>
                    Check.That(texte).Not.IsThisNumber(14))
                .DoesNotThrow();
        }

        [Test]
        public void NotFailOnExpectedValue()
        {
            string texte = "14";
            Check.ThatCode(() =>
                    Check.That(texte).IsThisNumber(14))
                .DoesNotThrow();
        }

        [Test]
        public void FailOnNegatedCheck()
        {
            string texte = "14";
            Check.ThatCode(() =>
                    Check.That(texte).Not.IsThisNumber(14))
                .IsAFaillingCheck();
        }
    }
}
