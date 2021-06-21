// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="FloatFloatingPointBinaryRelatedTests.cs" company="">
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
    public class FloatFloatingPointBinaryRelatedTests
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------
        private const float Zero = 0F;
        private const float NotANumber = Zero / Zero;
        private const float Twenty = 20F;
        private const float InfiniteNumber = Twenty / Zero;
        private const float TwentyF = 20F;


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
            .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked float value is a number whereas it must not." + Environment.NewLine + "The checked float value:" + Environment.NewLine + "\t[20]");
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
            .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked float value is not a number (NaN) whereas it must." + Environment.NewLine + "The checked float value:" + Environment.NewLine + "\t[NaN]");
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
            .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked float value is an infinite number whereas it must not." + Environment.NewLine + "The checked float value:" + Environment.NewLine + "\t[Infinity]");
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
            .IsAFailingCheckWithMessage(Environment.NewLine+ "The checked float value is a finite number whereas it must not." + Environment.NewLine + "The checked float value:" + Environment.NewLine + "\t[20]");
        }
        
        #endregion
        
        #region IsCloseTo

        [Test]
        public void IsCloseToWorks()
        {
            Check.That(Twenty).IsCloseTo(20.1, 0.2);
            Check.That(Twenty).IsCloseTo(21, 1);
        }

        [Test]
        public void IsCloseToShouldFailsIfTooFar()
        {
            using (new CultureSession("en-US"))
            {
                Check.ThatCode(() => Check.That(TwentyF).IsCloseTo(20.1, 0.01))
                     .IsAFailingCheckWithMessage("",
                                                 "The checked value is outside the expected value range.",
                                                 "The checked value:",
                                                 "\t[20]",
                                                 "The expected value:",
                                                 "\t[20.1 (+/- 0.01)]");
            }
        }


        [Test]
        public void IsEqualToWorksForZero()
        {
            Check.That(float.Epsilon).IsNotEqualTo(Zero);
        }
        #endregion
    }
}
