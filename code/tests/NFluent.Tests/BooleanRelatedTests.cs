// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="BooleanRelatedTests.cs" company="">
// //   Copyright 2013 Marc-Antoine LATOUR, Thomas PIERRAIN
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
    using Messages;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class BooleanRelatedTests
    {
        [Test]
        public void CheckThatWorksOnBoolean()
        {
            const bool nFluentRocks = true;
            const bool tddSucks = false;

            Check.That(nFluentRocks).IsTrue();
            Check.That(tddSucks).IsFalse();
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnBoolean()
        {
            const bool nFluentRocks = true;
            const bool tddSucks = false;

            Check.That(nFluentRocks).IsTrue().And.IsEqualTo(true).And.IsNotEqualTo(tddSucks);
            Check.That(tddSucks).IsFalse().And.IsEqualTo(false).And.IsNotEqualTo(nFluentRocks);
        }

        [Test]
        public void NotOperatorWorks()
        {
            const bool nFluentRocks = true;
            const bool tddSucks = false;

            Check.That(nFluentRocks).Not.IsFalse();
            Check.That(tddSucks).Not.IsTrue();
        }

        [Test]
        public void NotIsFalseMayThrowExceptions()
        {
            const bool tddSucks = false;
            EntityNamingLogic.ClearDefaultNameCache();
            Check.ThatCode( () => Check.That(tddSucks).Not.IsFalse())
                    .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked boolean is false whereas it must be true." + Environment.NewLine + "The checked boolean:" + Environment.NewLine + "\t[False]");
        }

        [Test]
        public void IsTrueThrowsExceptionWhenFalse()
        {
            const bool nFluentRocks = false;

            Check.ThatCode(() => Check.That(nFluentRocks).IsTrue())
                    .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked boolean is false whereas it must be true." + Environment.NewLine + "The checked boolean:" + Environment.NewLine + "\t[False]");
        }

        [Test]
        public void NotIsTrueThrowsExceptionWhenFalse()
        {
            const bool nFluentRocks = true;

            Check.ThatCode(() => Check.That(nFluentRocks).Not.IsTrue())
                    .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked boolean is true whereas it must be false." + Environment.NewLine + "The checked boolean:" + Environment.NewLine + "\t[True]");
        }

        [Test]
        public void IsEqualThrowsExceptionWhenNotEqual()
        {
            const bool nFluentRocks = true;
            const bool tddSucks = false;

            Check.ThatCode(() => Check.That(nFluentRocks).IsEqualTo(tddSucks))
                    .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked boolean is different from the expected one." + Environment.NewLine + "The checked boolean:" + Environment.NewLine + "\t[True]" + Environment.NewLine + "The expected boolean:" + Environment.NewLine + "\t[False]");
        }

        [Test]
        public void IsNotEqualThrowsExceptionWhenEqual()
        {
            const bool nFluentRocks = true;
            const bool winterNotNFluentRocks = true;

            Check.ThatCode(() => Check.That(nFluentRocks).IsNotEqualTo(winterNotNFluentRocks))
                    .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked boolean is equal to the expected one whereas it must not." + Environment.NewLine + "The expected boolean: different from" + Environment.NewLine + "\t[True] of type: [bool]");
        }

        [Test]
        public void IsFalseThrowsExceptionWhenTrue()
        {
            const bool nFluentRocks = true;

            Check.ThatCode(() => Check.That(nFluentRocks).IsFalse())
                    .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked boolean is true whereas it must be false." + Environment.NewLine + "The checked boolean:" + Environment.NewLine + "\t[True]");
        }
    }
}