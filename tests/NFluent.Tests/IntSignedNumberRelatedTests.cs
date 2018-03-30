// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IntSignedNumberRelatedTests.cs" company="">
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
    using Helpers;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class IntSignedNumberRelatedTests
    {
        private const int Zero = 0;
        private const int Two = 2;
        private const int MinusFifty = -50;
        // DoNotChangeOrRemoveThisLine

        private CultureSession session;

        [OneTimeSetUp]
        public void ForceCulture()
        {
            this.session = new CultureSession("fr-FR");
        }

        [OneTimeTearDown]
        public void RestoreCulture()
        {
            this.session.Dispose();
        }


        // Since this class is the model/template for the generation of the tests on all the other numbers types, don't forget to re-generate all the other classes every time you change this one. To do that, just save the .\T4" + Environment.NewLine + "umberTestsGenerator.tt file within Visual Studio 2012. This will trigger the T4 code generation process.
        #region IsPositive (obsolete)
#pragma warning disable 618
        [Test]
        public void IsPositiveWorks()
        {
            Check.That(Two).IsPositive();
        }

        [Test]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            Check.ThatCode(() =>
            {
                Check.That(Zero).IsPositive();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not strictly positive (i.e. greater than zero).",  
                     "The checked value:",
                    "\t[0]");
        }

        [Test]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(Two).Not.IsPositive();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is strictly positive (i.e. greater than zero), whereas it must not.",
                    "The checked value:",
                    "\t[2]");
        }

        [Test]
        public void IsPositiveThrowsExceptionWhenValueIsNegative()
        {
            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).IsPositive();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not strictly positive (i.e. greater than zero).",
                    "The checked value:",
                    "\t[-50]");
        }

        [Test]
        public void NotIsPositiveWorks()
        {
            Check.That(MinusFifty).Not.IsPositive();
        }

        #endregion

        #region IsNegative (obsolete)

        [Test]
        public void IsNegativeWorks()
        {
            Check.That(MinusFifty).IsNegative();
        }

        [Test]
        public void IsNegativeThrowsExceptionWhenEqualToZero()
        {
            Check.ThatCode(() =>
            {
                Check.That(Zero).IsNegative();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not strictly negative.",
                    "The checked value:", 
                    "\t[0]");
        }

        [Test]
        public void NotIsNegativeThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).Not.IsNegative();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is strictly negative, whereas it must not.",
                    "The checked value:",
                    "\t[-50]");
        }

        [Test]
        public void IsNegativeThrowsExceptionWhenValueIsPositive()
        {
            Check.ThatCode(() =>
            {
                Check.That(Two).IsNegative();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not strictly negative.",
                    "The checked value:",
                    "\t[2]");
        }

        [Test]
        public void NotIsNegativeWorks()
        {
            Check.That(Two).Not.IsNegative();
        }

        #endregion
#pragma warning restore 618

        #region IsStrictlyPositive

        [Test]
        public void IsStrictlyPositiveWorks()
        {
            Check.That(Two).IsStrictlyPositive();
        }

        [Test]
        public void IsStrictlyPositiveThrowsExceptionWhenEqualToZero()
        {
            Check.ThatCode(() =>
            {
                Check.That(Zero).IsStrictlyPositive();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not strictly positive (i.e. greater than zero).",
                    "The checked value:",
                    "\t[0]");
        }

        [Test]
        public void NotIsStrictlyPositiveThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(Two).Not.IsStrictlyPositive();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is strictly positive (i.e. greater than zero), whereas it must not.",
                    "The checked value:",
                    "\t[2]");
        }

        [Test]
        public void IsStrictlyPositiveThrowsExceptionWhenValueIsNegative()
        {
            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).IsStrictlyPositive();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not strictly positive (i.e. greater than zero).",
                    "The checked value:",
                    "\t[-50]");
        }

        [Test]
        public void NotIsStrictlyPositiveWorks()
        {
            Check.That(MinusFifty).Not.IsStrictlyPositive();
        }

        #endregion

        #region IsPositiveOrZero

        [Test]
        public void IsPositiveOrZeroWorks()
        {
            Check.That(Zero).IsPositiveOrZero();
            Check.That(Two).IsPositiveOrZero();
        }

        [Test]
        public void NotIsPositiveOrZeroThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(Zero).Not.IsPositiveOrZero();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is positive or equal to zero, whereas it must not.",
                    "The checked value:",
                    "\t[0]");
        }

        [Test]
        public void IsPositiveOrZeroThrowsExceptionWhenValueIsNegative()
        {
            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).IsPositiveOrZero();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not positive or equal to zero.",
                    "The checked value:",
                    "\t[-50]");
        }

        [Test]
        public void NotIsPositiveOrZeroWorks()
        {
            Check.That(MinusFifty).Not.IsPositiveOrZero();
        }

        #endregion

        #region IsStrictlyNegative

        [Test]
        public void IsStrictyNegativeWorks()
        {
            Check.That(MinusFifty).IsStrictlyNegative();
        }

        [Test]
        public void IsStrictyNegativeThrowsExceptionWhenEqualToZero()
        {
            Check.ThatCode(() =>
            {
                Check.That(Zero).IsStrictlyNegative();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not strictly negative.",
                    "The checked value:",
                    "\t[0]");
        }

        [Test]
        public void NotIsStrictyNegativeThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).Not.IsStrictlyNegative();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is strictly negative, whereas it must not.",
                    "The checked value:",
                    "\t[-50]");
        }

        [Test]
        public void IsStrictyNegativeThrowsExceptionWhenValueIsPositive()
        {
            Check.ThatCode(() =>
            {
                Check.That(Two).IsStrictlyNegative();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not strictly negative.",
                    "The checked value:",
                    "\t[2]");
        }

        [Test]
        public void NotIsStrictyNegativeWorks()
        {
            Check.That(Two).Not.IsStrictlyNegative();
        }

        #endregion
        
        #region IsNegativeOrZero

        [Test]
        public void IsNegativeOrZeroWorks()
        {

            Check.That(Zero).IsNegativeOrZero();
            Check.That(MinusFifty).IsNegativeOrZero();
        }

        [Test]
        public void NotIsNegativeOrZeroThrowsExceptionWhenFailing()
        {
            Check.ThatCode(() => { Check.That(MinusFifty).Not.IsNegativeOrZero(); })
                .IsAFaillingCheckWithMessage("",
                    "The checked value is negative or equal to zero, whereas it must not.",
                    "The checked value:",
                    "\t[-50]");
        }

        [Test]
        public void IsIsNegativeOrZeroThrowsExceptionWhenValueIsPositive()
        {
            Check.ThatCode(() =>
            {
                Check.That(Two).IsNegativeOrZero();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not negative or equal to zero.",
                    "The checked value:",
                    "\t[2]");
        }

        [Test]
        public void NotIsNegativeOrZeroWorks()
        {
            Check.That(Two).Not.IsNegativeOrZero();
        }

        #endregion
    }
}
