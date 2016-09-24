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
    using NUnit.Framework;

    [TestFixture]
    public class BooleanRelatedTests
    {
        [Test]
        public void CheckThatWorksOnBoolean()
        {
            const bool NFluentRocks = true;
            const bool TddSucks = false;

            Check.That(NFluentRocks).IsTrue();
            Check.That(TddSucks).IsFalse();
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionOnBoolean()
        {
            const bool NFluentRocks = true;
            const bool TddSucks = false;

            Check.That(NFluentRocks).IsTrue().And.IsEqualTo(true).And.IsNotEqualTo(TddSucks);
            Check.That(TddSucks).IsFalse().And.IsEqualTo(false).And.IsNotEqualTo(NFluentRocks);
        }

        [Test]
        public void NotOperatorWorks()
        {
            const bool NFluentRocks = true;
            const bool TddSucks = false;

            Check.That(NFluentRocks).Not.IsFalse();
            Check.That(TddSucks).Not.IsTrue();
        }

        [Test]
        public void NotIsFalseMayThrowExceptions()
        {
            const bool TddSucks = false;
            
            Check.ThatCode( () => Check.That(TddSucks).Not.IsFalse())
                    .Throws<FluentCheckException>()
                    .WithMessage(Environment.NewLine+ "The checked boolean is false whereas it must be true." + Environment.NewLine + "The checked boolean:" + Environment.NewLine + "\t[False]");
        }

        [Test]
        public void IsTrueThrowsExceptionWhenFalse()
        {
            const bool NFluentRocks = false;

            Check.ThatCode(() => Check.That(NFluentRocks).IsTrue())
                    .Throws<FluentCheckException>()
                    .WithMessage(Environment.NewLine+ "The checked boolean is false whereas it must be true." + Environment.NewLine + "The checked boolean:" + Environment.NewLine + "\t[False]");
        }

        [Test]
        public void NotIsTrueThrowsExceptionWhenFalse()
        {
            const bool NFluentRocks = true;

            Check.ThatCode(() => Check.That(NFluentRocks).Not.IsTrue())
                    .Throws<FluentCheckException>()
                    .WithMessage(Environment.NewLine+ "The checked boolean is true whereas it must be false." + Environment.NewLine + "The checked boolean:" + Environment.NewLine + "\t[True]");
        }

        [Test]
        public void IsEqualThrowsExceptionWhenNotEqual()
        {
            const bool NFluentRocks = true;
            const bool TddSucks = false;

            Check.ThatCode(() => Check.That(NFluentRocks).IsEqualTo(TddSucks))
                    .Throws<FluentCheckException>()
                    .WithMessage(Environment.NewLine+ "The checked boolean is different from the expected one." + Environment.NewLine + "The checked boolean:" + Environment.NewLine + "\t[True]" + Environment.NewLine + "The expected boolean:" + Environment.NewLine + "\t[False]");
        }

        [Test]
        public void IsNotEqualThrowsExceptionWhenEqual()
        {
            const bool NFluentRocks = true;
            const bool WinterNotNFluentRocks = true;

            Check.ThatCode(() => Check.That(NFluentRocks).IsNotEqualTo(WinterNotNFluentRocks))
                    .Throws<FluentCheckException>()
                    .WithMessage(Environment.NewLine+ "The checked boolean is equal to the expected one whereas it must not." + Environment.NewLine + "The expected boolean: different from" + Environment.NewLine + "\t[True] of type: [bool]");
        }

        [Test]
        public void IsFalseThrowsExceptionWhenTrue()
        {
            const bool NFluentRocks = true;

            Check.ThatCode(() => Check.That(NFluentRocks).IsFalse())
                    .Throws<FluentCheckException>()
                    .WithMessage(Environment.NewLine+ "The checked boolean is true whereas it must be false." + Environment.NewLine + "The checked boolean:" + Environment.NewLine + "\t[True]");
        }
    }
}