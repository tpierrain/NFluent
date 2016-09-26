// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="SbyteSignedNumberRelatedTests.cs" company="">
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
using System;

namespace NFluent.Tests
{
    using Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class SbyteSignedNumberRelatedTests
    {
        #pragma warning disable 169

        //// ---------------------- WARNING ----------------------
        //// AUTO-GENERATED FILE WHICH SHOULD NOT BE MODIFIED!
        //// To change this class, change the one that is used
        //// as the golden source/model for this autogeneration
        //// (i.e. the one dedicated to the integer values).
        //// -----------------------------------------------------

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


        #pragma warning restore 169
        #region IsPositive (obsolete)

        [Test]
        public void IsPositiveWorks()
        {
            const sbyte Two = 2;

            Check.That(Two).IsPositive();
        }

        [Test]
        public void IsPositiveThrowsExceptionWhenEqualToZero()
        {
            const sbyte Zero = 0;

            Check.ThatCode(() =>
            {
                Check.That(Zero).IsPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not strictly positive (i.e. greater than zero)." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[0]");
        }

        [Test]
        public void NotIsPositiveThrowsExceptionWhenFailing()
        {
            const sbyte Two = 2;

            Check.ThatCode(() =>
            {
                Check.That(Two).Not.IsPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is strictly positive (i.e. greater than zero), whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[2]");
        }

        [Test]
        public void IsPositiveThrowsExceptionWhenValueIsNegative()
        {
            const sbyte MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).IsPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not strictly positive (i.e. greater than zero)." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[-50]");
        }

        [Test]
        public void NotIsPositiveWorks()
        {
            const sbyte MinusFifty = -50;

            Check.That(MinusFifty).Not.IsPositive();
        }

        #endregion

        #region IsStrictlyPositive

        [Test]
        public void IsStrictlyPositiveWorks()
        {
            const sbyte Two = 2;

            Check.That(Two).IsStrictlyPositive();
        }

        [Test]
        public void IsStrictlyPositiveThrowsExceptionWhenEqualToZero()
        {
            const sbyte Zero = 0;

            Check.ThatCode(() =>
            {
                Check.That(Zero).IsStrictlyPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not strictly positive (i.e. greater than zero)." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[0]");
        }

        [Test]
        public void NotIsStrictlyPositiveThrowsExceptionWhenFailing()
        {
            const sbyte Two = 2;

            Check.ThatCode(() =>
            {
                Check.That(Two).Not.IsStrictlyPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is strictly positive (i.e. greater than zero), whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[2]");
        }

        [Test]
        public void IsStrictlyPositiveThrowsExceptionWhenValueIsNegative()
        {
            const sbyte MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).IsStrictlyPositive();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not strictly positive (i.e. greater than zero)." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[-50]");
        }

        [Test]
        public void NotIsStrictlyPositiveWorks()
        {
            const sbyte MinusFifty = -50;

            Check.That(MinusFifty).Not.IsStrictlyPositive();
        }

        #endregion

        #region IsPositiveOrZero

        [Test]
        public void IsPositiveOrZeroWorks()
        {
            const sbyte Zero = 0;
            const sbyte Two = 2;

            Check.That(Zero).IsPositiveOrZero();
            Check.That(Two).IsPositiveOrZero();
        }

        [Test]
        public void NotIsPositiveOrZeroThrowsExceptionWhenFailing()
        {
            const sbyte Zero = 0;

            Check.ThatCode(() =>
            {
                Check.That(Zero).Not.IsPositiveOrZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is positive or equal to zero, whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[0]");
        }

        [Test]
        public void IsPositiveOrZeroThrowsExceptionWhenValueIsNegative()
        {
            const sbyte MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).IsPositiveOrZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not positive or equal to zero." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[-50]");
        }

        [Test]
        public void NotIsPositiveOrZeroWorks()
        {
            const sbyte MinusFifty = -50;

            Check.That(MinusFifty).Not.IsPositiveOrZero();
        }

        #endregion

        #region IsNegative (obsolete)

        [Test]
        public void IsNegativeWorks()
        {
            const sbyte MinusFifty = -50;

            Check.That(MinusFifty).IsNegative();
        }

        [Test]
        public void IsNegativeThrowsExceptionWhenEqualToZero()
        {
            const sbyte Zero = 0;

            Check.ThatCode(() =>
            {
                Check.That(Zero).IsNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not strictly negative." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[0]");
        }

        [Test]
        public void NotIsNegativeThrowsExceptionWhenFailing()
        {
            const sbyte MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).Not.IsNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is strictly negative, whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[-50]");
        }

        [Test]
        public void IsNegativeThrowsExceptionWhenValueIsPositive()
        {
            const sbyte Two = 2;

            Check.ThatCode(() =>
            {
                Check.That(Two).IsNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not strictly negative." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[2]");
        }

        [Test]
        public void NotIsNegativeWorks()
        {
            const sbyte Two = 2;

            Check.That(Two).Not.IsNegative();
        }

        #endregion

        #region IsStrictlyNegative

        [Test]
        public void IsStrictyNegativeWorks()
        {
            const sbyte MinusFifty = -50;

            Check.That(MinusFifty).IsStrictlyNegative();
        }

        [Test]
        public void IsStrictyNegativeThrowsExceptionWhenEqualToZero()
        {
            const sbyte Zero = 0;
            
            Check.ThatCode(() =>
            {
                Check.That(Zero).IsStrictlyNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not strictly negative." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[0]");
        }

        [Test]
        public void NotIsStrictyNegativeThrowsExceptionWhenFailing()
        {
            const sbyte MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).Not.IsStrictlyNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is strictly negative, whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[-50]");
        }

        [Test]
        public void IsStrictyNegativeThrowsExceptionWhenValueIsPositive()
        {
            const sbyte Two = 2;

            Check.ThatCode(() =>
            {
                Check.That(Two).IsStrictlyNegative();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not strictly negative." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[2]");
        }

        [Test]
        public void NotIsStrictyNegativeWorks()
        {
            const sbyte Two = 2;

            Check.That(Two).Not.IsStrictlyNegative();
        }

        #endregion
        
        #region IsNegativeOrZero

        [Test]
        public void IsNegativeOrZeroWorks()
        {
            const sbyte MinusFifty = -50;
            const sbyte Zero = 0;

            Check.That(Zero).IsNegativeOrZero();
            Check.That(MinusFifty).IsNegativeOrZero();
        }

        [Test]
        public void NotIsNegativeOrZeroThrowsExceptionWhenFailing()
        {
            const sbyte MinusFifty = -50;

            Check.ThatCode(() =>
            {
                Check.That(MinusFifty).Not.IsNegativeOrZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is negative or equal to zero, whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[-50]");
        }

        [Test]
        public void IsIsNegativeOrZeroThrowsExceptionWhenValueIsPositive()
        {
            const sbyte Two = 2;

            Check.ThatCode(() =>
            {
                Check.That(Two).IsNegativeOrZero();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not negative or equal to zero." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[2]");
        }

        [Test]
        public void NotIsNegativeOrZeroWorks()
        {
            const sbyte Two = 2;

            Check.That(Two).Not.IsNegativeOrZero();
        }

        #endregion
    }
}
