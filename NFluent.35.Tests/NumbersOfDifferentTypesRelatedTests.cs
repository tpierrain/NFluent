﻿// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="NumbersOfDifferentTypesRelatedTests.cs" company="">
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
    using System.Globalization;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    public class NumbersOfDifferentTypesRelatedTests
    {
        private CultureInfo savedCulture;

        [SetUp]
        public void SetUp()
        {
            // Important so that ToString() versions of decimal works whatever the current culture.
            this.savedCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
        }

        [TearDown]
        public void TearDown()
        {
            // Boy scout rule ;-)
            Thread.CurrentThread.CurrentCulture = this.savedCulture;
        }
        
        #region IsEqualTo / IsNotEqualTo

        [Test]
        public void IntValueIsEqualToTheSameByteValue()
        {
            byte byteValue = 2;
            Check.That(byteValue).IsEqualTo(2);
        }

        [Test]
        public void ByteValueIsEqualToTheSameIntValue()
        {
            byte byteValue = 2;
            Check.That(2).IsEqualTo(byteValue);
        }

        [Test]
        public void IntValueIsEqualToTheSameLongValue()
        {
            Check.That(2L).IsEqualTo(2);
        }

        [Test]
        public void IntValueIsEqualToTheSameDoubleValue()
        {
            Check.That(2.0D).IsEqualTo(2);
        }

        [Test]
        public void LongValueIsEqualToTheSameDoubleValue()
        {
            Check.That(2.0D).IsEqualTo(2L);
        }

        [Test]
        public void IntValueIsEqualToTheSameFloatValue()
        {
            Check.That(2.0F).IsEqualTo(2);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[2] of type: [long]")]
        public void LongValueIsNotEqualToTheSameIntValueAndThrows()
        {
            Check.That(2).IsNotEqualTo(2L);
        }

        [Test]
        public void LongValueIsEqualToTheSameIntValue()
        {
            Check.That(42).IsEqualTo(42L);
        }

        [Test]
        public void NotIsEqualToWorksWithDifferentTypes()
        {
            const int intValue = 42;
            const long longValue = 21L;

            Check.That(intValue).Not.IsEqualTo(longValue);
        }

        [Test]
        public void IsAfterWorks()
        {
            const long value = 42;
            Check.That(value).IsNotZero().And.IsAfter(40);
        }

        [Test]
        public void IsBeforeWorks()
        {
            const long value = 42;
            Check.That(value).IsNotZero().And.IsBefore(100);
        }

        #endregion
    }
}
