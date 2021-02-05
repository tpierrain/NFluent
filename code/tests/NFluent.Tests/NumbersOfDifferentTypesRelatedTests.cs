// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumbersOfDifferentTypesRelatedTests.cs" company="">
//   Copyright 2013 Thomas PIERRAIN
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
    using Helpers;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class NumbersOfDifferentTypesRelatedTests
    {
        private CultureSession session;

        [OneTimeSetUp]
        public void ForceCulture()
        {
            this.session = new CultureSession("en-US");
        }

        [OneTimeTearDown]
        public void RestoreCulture()
        {
            this.session.Dispose();
        }

        private static double DecimalValue => 0.95000000000000006d;

        // GH #205 
        [Test]
        public void IsEqualTo_should_provide_details_and_suggest_isCloseTo()
        {
            using (new CultureSession("en-US"))
            {
                Check.ThatCode(() => Check.That(DecimalValue*(1<<16)).IsEqualTo(0.95d*(1<<16))).IsAFailingCheckWithMessage("",
                    "The checked value is different from the expected one, with a difference of 7.3E-12. You may consider using IsCloseTo() for comparison.",
                    "The checked value:",
                    "\t[62259.2]",
                    "The expected value:",
                    "\t[62259.2]");

                Check.ThatCode(() => Check.That(0.9500001f*(1<<16)).IsEqualTo(0.95f*(1<<16))).IsAFailingCheckWithMessage("",
                    "The checked value is different from the expected one, with a difference of 0.0078. You may consider using IsCloseTo() for comparison.",
                    "The checked value:",
                    "\t[62259.21]",
                    "The expected value:",
                    "\t[62259.2]");
                Check.ThatCode(() => Check.That(100001f).IsEqualTo(100000f)).IsAFailingCheckWithMessage("",
                    "The checked value is different from the expected one, with a difference of 1.",
                    "The checked value:",
                    "\t[100001]",
                    "The expected value:",
                    "\t[100000]");
                
                Check.ThatCode(() => Check.That(100000001d).IsEqualTo(100000000d)).IsAFailingCheckWithMessage("",
                    "The checked value is different from the expected one, with a difference of 1.",
                    "The checked value:",
                    "\t[100000001]",
                    "The expected value:",
                    "\t[100000000]");
                
            }
        }

        [Test]
        public void IsEqualTo_should_suggest_isCloseTo_for_fifth_digit()
        {
            Check.ThatCode(() => Check.That(10241.0).IsEqualTo(10240.0)).IsAFailingCheckWithMessage("", 
                "The checked value is different from the expected one.", 
                "The checked value:", 
                "\t[10241]", 
                "The expected value:", 
                "\t[10240]");
            Check.ThatCode(() => Check.That(10241.0f).IsEqualTo(10240.0f)).IsAFailingCheckWithMessage("", 
                "The checked value is different from the expected one.", 
                "The checked value:", 
                "\t[10241]", 
                "The expected value:", 
                "\t[10240]");
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
/*
        [Test]
        public void LongValueIsNotEqualToTheSameIntValueAndThrows()
        {
            Check.ThatCode(() =>
            {
                Check.That(42).IsEqualTo(42L);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is different from the expected one." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[42] of type: [int]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\t[42] of type: [long]");
        }
        */

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
