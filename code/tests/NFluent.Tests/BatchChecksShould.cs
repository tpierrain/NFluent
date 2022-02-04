// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BatchChecksShould.cs" company="NFluent">
//   Copyright 2021 Thomas PIERRAIN & Cyrille DUPUYDAUBY
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
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class BatchChecksShould
    {
        [Test]
        public void
            WorkForSingleCheck()
        {
            var sut = Check.StartBatch();

            Check.That(2).IsEqualTo(3);

            Check.ThatCode(() => sut.Dispose()).IsAFailingCheckWithMessage("",
                "The checked value is different from the expected one.",
                "The checked value:",
                "\t[2]",
                "The expected value:",
                "\t[3]");
        }

        [Test]
        public void
            WorkForTwoChecks()
        {
            var sut = Check.StartBatch();

            Check.That(2).IsPositiveOrZero();
            Check.That(2).IsEqualTo(3);

            Check.ThatCode(() => sut.Dispose()).IsAFailingCheckWithMessage("",
                "The checked value is different from the expected one.",
                "The checked value:",
                "\t[2]",
                "The expected value:",
                "\t[3]");
        }

        [Test]
        public void
            WorkForTwoFailingChecks()
        {
            var sut = Check.StartBatch();

            Check.That(2).IsNegativeOrZero();
            Check.That(2).IsEqualTo(3);

            Check.ThatCode(() => sut.Dispose()).IsAFailingCheckWithMessage("",
                "The checked value is not negative or equal to zero.",
                "The checked value:",
                "\t[2]",
                "** And **",
                "The checked value is different from the expected one.",
                "The checked value:",
                "\t[2]",
                "The expected value:",
                "\t[3]");
        }

        [Test]
        public void
            WorkWhenExceptionRaised()
        {
            Check.ThatCode(() =>
            {
                using (Check.StartBatch())
                {
                    Check.That(2).IsNegativeOrZero();
                    throw new ApplicationException("Random exception");
                }
            }).IsAFailingCheckWithMessage("",
                "The checked value is not negative or equal to zero.",
                "The checked value:",
                "\t[2]");
        }

        [Test]
        public void
            CanDeclareMacros2Parameters()
        {
            var sut = Check.DeclareMacro<int, int, int>((x, y, z) =>
                Check.That(x).IsStrictlyGreaterThan(y).And.IsStrictlyLessThan(z), "The {0} is not in the {1}-{2} range.");

            Check.That(2).VerifiesMacro(sut).With(1, 3);
        }

        [Test]
        public void
            CanDeclareMacrosSingleParameter()
        {
            var sut = Check.DeclareMacro<int, int>((x, y) =>
                Check.That(x).IsStrictlyGreaterThan(y), "The {0} is less than {1}.");

            Check.That(2).VerifiesMacro(sut).With(1);
        }

        [Test]
        public void
            RaiseProperErrorMessage()
        {
            var sut = Check.DeclareMacro<int, int>((x, y) =>
                Check.That(x).IsStrictlyGreaterThan(y), "The {0} is less than {1}.");

            Check.ThatCode( () =>

            Check.That(2).VerifiesMacro(sut).With(5)).IsAFailingCheckWithMessage("",
"The checked value is less than given one.",  "The checked value is less than the given one.", "The checked value:", "\t[2]", "The expected value: strictly greater than", "\t[5]");

        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       