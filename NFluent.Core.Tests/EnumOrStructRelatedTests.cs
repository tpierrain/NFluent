// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EnumOrStructRelatedTests.cs" company="">
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
    using NUnit.Framework;

    [TestFixture]
    public class EnumOrStructRelatedTests
    {
        private const string Blabla = ".*?";
        private const string LineFeed = "\n";
        private const string NumericalHashCodeWithinBrackets = "(\\[(\\d+)\\])";

        [Test]
        public void IsEqualToWorksWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).IsEqualTo(Nationality.French);
            Check.That(FrenchNationality).IsEqualTo(Nationality.French);
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenFailingWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;

            Check.ThatCode(() =>
            {
                Check.ThatEnum(FrenchNationality).IsEqualTo(Nationality.American);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is different from the expected one." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[French]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\t[American]");
        }

        [Test]
        public void IsNotEqualToWorksWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).IsNotEqualTo(Nationality.Korean);
        }

        [Test]
        public void IsNotEqualToThrowsExceptionWhenFailingWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;

            Check.ThatCode(() =>
            {
                Check.ThatEnum(FrenchNationality).IsNotEqualTo(Nationality.French);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[French] of type: [NFluent.Tests.Nationality]");
        }

        [Test]
        public void NotOperatorWorksOnIsEqualToForEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).Not.IsEqualTo(Nationality.American);
        }

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailingWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;

            Check.ThatCode(() =>
            {
                Check.ThatEnum(FrenchNationality).Not.IsEqualTo(Nationality.French);
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is equal to the expected one whereas it must not." + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[French] of type: [NFluent.Tests.Nationality]");
        }

        [Test]
        public void NotOperatorWorksOnIsNotEqualToForEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).Not.IsNotEqualTo(Nationality.French);
        }

        // TODO: write tests related to error message of IsNotEqualTo (cause the error message seems not so good)
        [Test]
        public void IsEqualToWorksWithStruct()
        {
            var inLove = new Mood { Description = "In love", IsPositive = true };
            Check.ThatEnum(inLove).IsEqualTo(inLove);
        }

        [Test]
        public void IsInstanceOfWorks()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).IsInstanceOf<Nationality>();
        }

        [Test]
        public void IsNotInstanceOfWorks()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).IsNotInstanceOf<int>();
        }

        [Test]
        public void IsInstanceOfFailsPropery()
        {
            const Nationality FrenchNationality = Nationality.French;

            Check.ThatCode(() =>
            {
                Check.ThatEnum(FrenchNationality).IsInstanceOf<int>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not an instance of the expected type." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[French] of type: [NFluent.Tests.Nationality]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\tan instance of type: [int]");
        }

        [Test]
        public void IsNotInstanceOfFailsProperly()
        {
            const Nationality FrenchNationality = Nationality.French;

            Check.ThatCode(() =>
            {
                Check.ThatEnum(FrenchNationality).IsNotInstanceOf<Nationality>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [NFluent.Tests.Nationality] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[French] of type: [NFluent.Tests.Nationality]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [NFluent.Tests.Nationality]");
        }
    }
}
