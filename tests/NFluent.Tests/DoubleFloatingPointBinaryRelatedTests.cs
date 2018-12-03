// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="DoubleFloatingPointBinaryRelatedTests.cs" company="">
// //   Copyright 2014 Thomas PIERRAIN
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
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class DoubleFloatingPointBinaryRelatedTests
    {
        // DoNotChangeOrRemoveThisLine
        private const double Zero = 0D;
        private const double NotANumber = Zero / Zero;
        private const double Twenty = 20D;
        private const double InfiniteNumber = Twenty / Zero;
        private const double TwentyF = 20F;


        #region IsNaN

        [Test]
        public void IsNaNWorks()
        {

            Check.That(NotANumber).IsNaN();
        }

        [Test]
        public void IsNaNThrowsWhenTheValueIsANumber()
        {
            Check.ThatCode(() =>
            {
                Check.That(Twenty).IsNaN();
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked double value is a number whereas it must not." + Environment.NewLine + "The checked double value:" + Environment.NewLine + "\t[20]");
        }

        [Test]
        public void NotIsNaNWorks()
        {
            Check.That(Twenty).Not.IsNaN();
        }

        [Test]
        public void NotIsNaNThrowsAnExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(NotANumber).Not.IsNaN();
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked double value is not a number (NaN) whereas it must." + Environment.NewLine + "The checked double value:" + Environment.NewLine + "\t[NaN]");
        }

        #endregion

        #region IsFinite

        [Test]
        public void IsFiniteWorks()
        {
            Check.That(Twenty).IsFinite();
        }

        [Test]
        public void IsFiniteThrowsWithInfinity()
        {
            Check.ThatCode(() =>
            {
                Check.That(InfiniteNumber).IsFinite();
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked double value is an infinite number whereas it must not." + Environment.NewLine + "The checked double value:" + Environment.NewLine + "\t[Infinity]");
        }

        [Test]
        public void NotIsFiniteWorks()
        {
            Check.That(InfiniteNumber).Not.IsFinite();
        }

        [Test]
        public void NotIsFiniteThrowsWithFiniteNumber()
        {
            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsFinite();
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked double value is a finite number whereas it must not." + Environment.NewLine + "The checked double value:" + Environment.NewLine + "\t[20]");
        }

        [Test]
        public void IsAroundWorks()
        {
            Check.That(Twenty).IsCloseTo(20.1, 0.2);
            Check.That(Twenty).IsCloseTo(21, 1);
        }

        [Test]
        public void IsAroundShouldFailsIfToFar()
        {
            using (new CultureSession("en-US"))
            {
                Check.ThatCode(() => {
                    Check.That(TwentyF).IsCloseTo(20.1, 0.01);
                }).IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked value is outside the expected value range." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[20]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\t[20.1 (+/- 0.01)]");
            }
        }

        #endregion
    }
}
