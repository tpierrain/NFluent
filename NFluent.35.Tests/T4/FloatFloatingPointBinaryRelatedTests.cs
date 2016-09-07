﻿// // --------------------------------------------------------------------------------------------------------------------
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
namespace NFluent.Tests
{
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
        
        #region IsNaN

        [Test]
        public void IsNaNWorks()
        {
            const float Zero = 0F;
            const float NotANumber = Zero / Zero;

            Check.That(NotANumber).IsNaN();
        }

        [Test]
        public void IsNaNThrowsWhenTheValueIsANumber()
        {
            const float Twenty = 20F;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).IsNaN();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked float value is a number whereas it must not.\nThe checked float value:\n\t[20]");
        }

        [Test]
        public void NotIsNaNWorks()
        {
            const float Twenty = 20F;

            Check.That(Twenty).Not.IsNaN();
        }

        [Test]
        public void NotIsNaNThrowsAnExceptionWhenFailing()
        {
            const float Zero = 0F;
            const float NotANumber = Zero / Zero;

            Check.ThatCode(() =>
            {
                Check.That(NotANumber).Not.IsNaN();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked float value is not a number (NaN) whereas it must.\nThe checked float value:\n\t[NaN]");
        }

        #endregion

        #region IsFinite

        [Test]
        public void IsFiniteWorks()
        {
            const float Twenty = 20F;

            Check.That(Twenty).IsFinite();
        }

        [Test]
        public void IsFiniteThrowsWithInfinity()
        {
            const float Zero = 0F;
            const float Twenty = 20F;
            const float InfiniteNumber = Twenty / Zero;

            Check.ThatCode(() =>
            {
                Check.That(InfiniteNumber).IsFinite();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked float value is an infinite number whereas it must not.\nThe checked float value:\n\t[Infinity]");
        }

        [Test]
        public void NotIsFiniteWorks()
        {
            const float Zero = 0F;
            const float Twenty = 20F;
            const float InfiniteNumber = Twenty / Zero;

            Check.That(InfiniteNumber).Not.IsFinite();
        }

        [Test]
        public void NotIsFiniteThrowsWithFiniteNumber()
        {
            const float Twenty = 20F;

            Check.ThatCode(() =>
            {
                Check.That(Twenty).Not.IsFinite();
            })
            .Throws<FluentCheckException>()
            .WithMessage("\nThe checked float value is a finite number whereas it must not.\nThe checked float value:\n\t[20]");
        }

        [Test]
        public void IsAroundWorks()
        {
            const float Twenty = 20F;
            Check.That(Twenty).IsCloseTo(20.1, 0.2);
        }

        [Test]
        public void IsAroundShouldFailsIfToFar()
        {
            const float Twenty = 20F;
            Check.ThatCode(() => {
                Check.That(Twenty).IsCloseTo(20.1, 0.01); }).Throws<FluentCheckException>().WithMessage("\nThe checked value is outside the expected value range.\nThe checked value:\n\t[20]\nThe expected value:\n\t[20,1 (+/- 0,01)]");
        }

        #endregion
    }
}
