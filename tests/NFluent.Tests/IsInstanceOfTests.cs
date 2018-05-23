// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IsInstanceOfTests.cs" company="">
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
    using System.Collections.Generic;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class IsInstanceOfTests
    {
        private const string StringObj = "for unit testing";
        private const char CharObj = 'a';
        private const byte ByteObj = 1;
        private const sbyte SByteObj = 2;
        private const int IntObj = 23;
        private const long LongObj = long.MaxValue;
        private const double DoubleObj = 23d;
        private const decimal DecimalObj = 2;
        private const uint UIntObj = 2;
        private const short ShortObj = 2;
        private const ushort UShortObj = 2;
        private const ulong ULongObj = 2;
        private const float FloatObj = 3.14F;
        private const bool BoolObj = true;
        private readonly Person person = new Person { Name = "Charles BAUDELAIRE" };
        private readonly DateTime dateTimeObj = new DateTime();
        private readonly TimeSpan timeSpanObj = new TimeSpan();
        private readonly int[] integerArray = new int[10];
        private readonly Version firstVersion = new Version(1, 0, 0, 0);
        private readonly int[] emptyIntegerArray = new int[10];
        private readonly List<string> stringList = new List<string>();

        #region IsInstanceOf tests

        [Test]
        public void IsInstanceOfWorks()
        {
            // string
            Check.That("Failure is mother of success.").IsInstanceOf<string>();

            // bool
            Check.That(BoolObj).IsInstanceOf<bool>();

            // DateTime & TimeSpan
            Check.That(this.dateTimeObj).IsInstanceOf<DateTime>();
            Check.That(this.timeSpanObj).IsInstanceOf<TimeSpan>();

            // numbers
            Check.That(ByteObj).IsInstanceOf<byte>();
            Check.That(ShortObj).IsInstanceOf<short>();
            Check.That(IntObj).IsInstanceOf<int>();
            Check.That(LongObj).IsInstanceOf<long>();
            Check.That(DecimalObj).IsInstanceOf<decimal>();
            Check.That(DoubleObj).IsInstanceOf<double>();
            Check.That(UShortObj).IsInstanceOf<ushort>();
            Check.That(UIntObj).IsInstanceOf<uint>();
            Check.That(ULongObj).IsInstanceOf<ulong>();
            Check.That(FloatObj).IsInstanceOf<float>();
            Check.That(SByteObj).IsInstanceOf<sbyte>();
            Check.That(CharObj).IsInstanceOf<char>();

            // POCO
            Check.That(this.person).IsInstanceOf<Person>();

            // Arrays
            Check.That(this.emptyIntegerArray).IsInstanceOf<int[]>();

            // IEnumerable
            Check.That(this.stringList).IsInstanceOf<List<string>>();
            Check.That(this.integerArray).IsInstanceOf<int[]>();

            // Version
            Check.That(this.firstVersion).IsInstanceOf<Version>();
        }

        [Test]
        public void IsInstanceOfThrowsIfValueIsNull()
        {
            Check.ThatCode(() =>
            {
                Check.That((object) null).IsInstanceOf<object>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not an instance of [object].",
                    "The checked value:",
                    "\t[null] of type: [object]",
                    "The expected value:",
                    "\tan instance of type: [object]");
        }

        [Test]
        public void IsInstanceOfWorksIfValueIsNullButOfSameNullableType()
        {
            Check.That((int?) null).IsInstanceOf<int?>();
        }

        [Test]
        public void NotOperatorWorksOnIsInstanceOfMethods()
        {
            // string
            Check.That("Failure is mother of success.").Not.IsInstanceOf<int>();

            // bool
            Check.That(BoolObj).Not.IsInstanceOf<string>();

            // DateTime & TimeSpan
            Check.That(this.dateTimeObj).Not.IsInstanceOf<int>();
            Check.That(this.timeSpanObj).Not.IsInstanceOf<int>();

            // numbers
            Check.That(ByteObj).Not.IsInstanceOf<string>();
            Check.That(ShortObj).Not.IsInstanceOf<string>();
            Check.That(IntObj).Not.IsInstanceOf<string>();
            Check.That(LongObj).Not.IsInstanceOf<string>();
            Check.That(DecimalObj).Not.IsInstanceOf<string>();
            Check.That(DoubleObj).Not.IsInstanceOf<string>();
            Check.That(UShortObj).Not.IsInstanceOf<string>();
            Check.That(UIntObj).Not.IsInstanceOf<string>();
            Check.That(ULongObj).Not.IsInstanceOf<string>();
            Check.That(FloatObj).Not.IsInstanceOf<string>();
            Check.That(SByteObj).Not.IsInstanceOf<string>();

            // POCO
            Check.That(this.person).Not.IsInstanceOf<string>();

            // Arrays
            Check.That(this.emptyIntegerArray).Not.IsInstanceOf<string[]>();

            // IEnumerable
            Check.That(this.stringList).Not.IsInstanceOf<string>();
            Check.That(this.integerArray).Not.IsInstanceOf<string>();

            // Version
            Check.That(this.firstVersion).Not.IsInstanceOf<string>();
        }

        [Test]
        public void IsInstanceOfThrowsExceptionWithDerivedTypeAsCheckedExpression()
        {
            var child = new Child { Name = "Telemachus" };

            Check.ThatCode(() =>
            {
                Check.That(child).IsInstanceOf<Person>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not an instance of [NFluent.Tests.Person].",
                    "The checked value:",
                    "\t[Telemachus] of type: [NFluent.Tests.Child]",
                    "The expected value:", 
                    "\tan instance of type: [NFluent.Tests.Person]");
        }

        [Test]
        public void IsInstanceOfThrowsExceptionWhenFailingWithTimeSpan()
        {
            TimeSpan oneHour = TimeSpan.FromMinutes(60);

            Check.ThatCode(() =>
            {
                Check.That(oneHour).IsInstanceOf<string>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not an instance of [string].",
                    "The checked value:",
                    "\t[01:00:00] of type: [System.TimeSpan]",
                    "The expected value:",
                    "\tan instance of type: [string]");
        }

        [Test]
        public void IsInstanceOfThrowsExceptionWithProperFormatWhenFailsWithInt()
        {
            Check.ThatCode(() =>
            {
                Check.That(IntObj).IsInstanceOf<Person>();
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked value is not an instance of [NFluent.Tests.Person]." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[23] of type: [int]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\tan instance of type: [NFluent.Tests.Person]");
        }

        [Test]
        public void IsInstanceOfThrowsExceptionWithProperFormatWhenFailsWithString()
        {
            Check.ThatCode(() =>
            {
                Check.That(StringObj).IsInstanceOf<Person>();
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked value is not an instance of [NFluent.Tests.Person]." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[\"for unit testing\"] of type: [string]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\tan instance of type: [NFluent.Tests.Person]");
        }

        #endregion

        #region IsNotInstanceOf tests

        [Test]
        public void IsNotInstanceOfWorks()
        {
            // string
            Check.That("Failure is mother of success.").IsNotInstanceOf<int>();

            // bool
            Check.That(BoolObj).IsNotInstanceOf<string>();

            // DateTime & TimeSpan
            Check.That(this.dateTimeObj).IsNotInstanceOf<int>();
            Check.That(this.timeSpanObj).IsNotInstanceOf<int>();

            // numbers
            Check.That(ByteObj).IsNotInstanceOf<string>();
            Check.That(ShortObj).IsNotInstanceOf<string>();
            Check.That(IntObj).IsNotInstanceOf<string>();
            Check.That(LongObj).IsNotInstanceOf<string>();
            Check.That(DecimalObj).IsNotInstanceOf<string>();
            Check.That(DoubleObj).IsNotInstanceOf<string>();
            Check.That(UShortObj).IsNotInstanceOf<string>();
            Check.That(UIntObj).IsNotInstanceOf<string>();
            Check.That(ULongObj).IsNotInstanceOf<string>();
            Check.That(FloatObj).IsNotInstanceOf<string>();
            Check.That(SByteObj).IsNotInstanceOf<string>();

            // POCO
            Check.That(this.person).IsNotInstanceOf<string>();

            // Arrays
            Check.That(this.emptyIntegerArray).IsNotInstanceOf<string[]>();

            // IEnumerable
            Check.That(this.stringList).IsNotInstanceOf<string>();
            Check.That(this.integerArray).IsNotInstanceOf<string>();

            // Version
            Check.That(this.firstVersion).IsNotInstanceOf<string>();
        }

        [Test]
        public void IsNotInstanceOfWorksIfValueIsNull()
        {
            Check.That((object) null).IsNotInstanceOf<object>();
        }

        [Test]
        public void IsNotInstanceOfThrowsIfValueIsNullButOfSameNullableType()
        {
            Check.ThatCode(() =>
            {
                Check.That((int?) null).IsNotInstanceOf<int?>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is an instance of [int?] whereas it must not.",
                    "The checked value:",
                    "\t[null] of type: [int?]",
                    "The expected value: different from",
                    "\tan instance of type: [int?]");
        }

        [Test]
        public void NotOperatorWorksOnIsNotInstanceOfMethods()
        {
            // string
            Check.That("Failure is mother of success.").Not.IsNotInstanceOf<string>();

            // bool
            Check.That(BoolObj).Not.IsNotInstanceOf<bool>();

            // DateTime & TimeSpan
            Check.That(this.dateTimeObj).Not.IsNotInstanceOf<DateTime>();
            Check.That(this.timeSpanObj).Not.IsNotInstanceOf<TimeSpan>();

            // numbers
            Check.That(ByteObj).Not.IsNotInstanceOf<byte>();
            Check.That(ShortObj).Not.IsNotInstanceOf<short>();
            Check.That(IntObj).Not.IsNotInstanceOf<int>();
            Check.That(LongObj).Not.IsNotInstanceOf<long>();
            Check.That(DecimalObj).Not.IsNotInstanceOf<decimal>();
            Check.That(DoubleObj).Not.IsNotInstanceOf<double>();
            Check.That(UShortObj).Not.IsNotInstanceOf<ushort>();
            Check.That(UIntObj).Not.IsNotInstanceOf<uint>();
            Check.That(ULongObj).Not.IsNotInstanceOf<ulong>();
            Check.That(FloatObj).Not.IsNotInstanceOf<float>();
            Check.That(SByteObj).Not.IsNotInstanceOf<sbyte>();

            // POCO
            Check.That(this.person).Not.IsNotInstanceOf<Person>();

            // Arrays
            Check.That(this.emptyIntegerArray).Not.IsNotInstanceOf<int[]>();

            // IEnumerable
            Check.That(this.stringList).Not.IsNotInstanceOf<List<string>>();
            Check.That(this.integerArray).Not.IsNotInstanceOf<int[]>();

            // Version
            Check.That(this.firstVersion).Not.IsNotInstanceOf<Version>();
        }

        [Test]
        public void IsNotInstanceOfThrowsExceptionWithProperFormatWhenFailsWithInt()
        {
            const int intObject = 23;

            Check.ThatCode(() =>
            {
                Check.That(intObject).IsNotInstanceOf<int>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is an instance of [int] whereas it must not.",
                    "The checked value:",
                    "\t[23] of type: [int]",
                    "The expected value: different from",
                    "\tan instance of type: [int]");
        }

        [Test]
        public void IsNotInstanceOfThrowsExceptionWithProperFormatWhenFailsWithString()
        {
            const string statement = "If you don’t want to slip up tomorrow, speak the truth today (Bruce Lee).";

            Check.ThatCode(() =>
            {
                Check.That(statement).IsNotInstanceOf<string>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is an instance of [string] whereas it must not.",
                    "The checked value:",
                    "\t[\"If you don’t want to slip up tomorrow, speak the truth today (Bruce Lee).\"] of type: [string]",
                    "The expected value: different from",
                    "\tan instance of type: [string]");
        }

        [Test]
        public void NotIsInstanceOfThrowsExceptionWhenFailingWithIntArray()
        {
            Check.ThatCode(() =>
            {
                Check.That(this.emptyIntegerArray).Not.IsInstanceOf<int[]>();
            })
            .IsAFaillingCheckWithMessage("",
            "The checked value is an instance of [int[]] whereas it must not.",
                    "The checked value:",
                    "\t[0, 0, 0, 0, 0, 0, 0, 0, 0, 0] (10 items) of type: [int[]]",
                    "The expected value: different from",
                    "\tan instance of type: [int[]]");
        }

        [Test]
        public void NotIsNotInstanceOfThrowsExceptionWhenFailingWithIntArray()
        {
            Check.ThatCode(() =>
            {
                Check.That(this.emptyIntegerArray).Not.IsNotInstanceOf<int>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is not an instance of [int].",
                    "The checked value:",
                    "\t[0, 0, 0, 0, 0, 0, 0, 0, 0, 0] (10 items) of type: [int[]]",
                    "The expected value:",
                    "\tan instance of type: [int]");
        }

        [Test]
        public void IsNotInstanceOfThrowsExceptionWhenFailingWithTimeSpan()
        {
            TimeSpan oneHour = TimeSpan.FromMinutes(60);

            Check.ThatCode(() =>
            {
                Check.That(oneHour).IsNotInstanceOf<TimeSpan>();
            })
            .IsAFaillingCheckWithMessage("",
            "The checked value is an instance of [System.TimeSpan] whereas it must not.",
                    "The checked value:",
                    "\t[01:00:00] of type: [System.TimeSpan]",
                    "The expected value: different from",
                    "\tan instance of type: [System.TimeSpan]");
        }

        [Test]
        public void NotIsInstanceOfThrowsExceptionWhenFailingWithTimeSpan()
        {
            TimeSpan oneHour = TimeSpan.FromMinutes(60);

            Check.ThatCode(() =>
            {
                Check.That(oneHour).Not.IsInstanceOf<TimeSpan>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is an instance of [System.TimeSpan] whereas it must not.", 
                    "The checked value:",
                    "\t[01:00:00] of type: [System.TimeSpan]",
                    "The expected value: different from",
                    "\tan instance of type: [System.TimeSpan]");
        }

        #endregion

        [Test]
        public void InheritsFromWorks()
        {
            var child = new Child { Name = "Telemachus" };
            Check.That(child).InheritsFrom<Person>();
        }

        [Test]
        public void InheritsFromWorksAlsoWithTheSameType()
        {
            var child = new Child { Name = "Telemachus" };
            Check.That(child).InheritsFrom<Child>();
        }

        [Test]
        public void InheritsFromThrowsExceptionWhenFailing()
        {
            var father = new Person { Name = "Odysseus" };

            Check.ThatCode(() =>
            {
                Check.That(father).InheritsFrom<Child>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value's type does not have the expected inheritance.",
                    "The checked value's type:",
                    "\t[NFluent.Tests.Person]",
                    "The expected value's type: inherits from",
                    "\t[NFluent.Tests.Child]");
        }

        [Test]
        public void InheritsFromFailsWithForcedSutName()
        {
            var father = new Person { Name = "Odysseus" };

            Check.ThatCode(() =>
            {
                Check.That(father).As("dada").InheritsFrom<Child>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked [dada]'s type does not have the expected inheritance.",
                    "The checked [dada]'s type:",
                    "\t[NFluent.Tests.Person]",
                    "The expected [dada]'s type: inherits from",
                    "\t[NFluent.Tests.Child]");
        }

        [Test]
        public void NotInheritsFromWorks()
        {
            var hero = new Person { Name = "Arjuna" };
            Check.That(hero).Not.InheritsFrom<int>();
        }

        [Test]
        public void NotInheritsFromThrowsExceptionWhenFailing()
        {
            var father = new Person { Name = "Odysseus" };

            Check.ThatCode(() =>
            {
                Check.That(father).Not.InheritsFrom<Person>();
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value's type does inherits from the given one where as it must not",
                    "The checked value's type:",
                    "\t[NFluent.Tests.Person]", 
                    "The expected value's type: does not inherits from",
                    "\t[NFluent.Tests.Person]");
        }

        [Test]
        public void
            IsNoInstanceOfAny()
        {
            var father = new Person {Name = "Odysseus"};

            Check.That(father).IsNotAnInstanceOfThese(typeof(int), typeof(string), typeof(Version));
            int? val = 12;

            Check.That(val).IsNotAnInstanceOfThese(typeof(int), typeof(string), typeof(Version));
        }

        [Test]
        public void IsNotAnInstanceOfTheseFailsWithProperMessage()
        {
            var father = new Person {Name = "Odysseus"};
            Check.ThatCode(()=>
                    Check.That(father).IsNotAnInstanceOfThese(typeof(int), typeof(string), typeof(Person))).
                IsAFaillingCheckWithMessage("",
                    "The checked value's type is [NFluent.Tests.Person] where as it must not.",
                    "The checked value's type:",
                    "\t[NFluent.Tests.Person]",
                    "The expected value's type: anything but",
                    "\t[int, string, NFluent.Tests.Person]");
            int? val = 12;
            Check.ThatCode(() => Check.That(val).IsNotAnInstanceOfThese(typeof(int?), typeof(string), typeof(Person)))
                .IsAFaillingCheckWithMessage("",
                    "The checked value's type is [int?] where as it must not.",
                    "The checked value's type:",
                    "\t[int?]", 
                    "The expected value's type: anything but", 
                    "\t[int?, string, NFluent.Tests.Person]");
        }

        [Test]
        public void
            IsAnInstanceOfAny()
        {
            var father = new Person {Name = "Odysseus"};

            Check.That(father).IsAnInstanceOfOneOf(typeof(int?), typeof(string), typeof(Person));
            int? val = 12;

            Check.That(val).IsAnInstanceOfOneOf(typeof(int?), typeof(string), typeof(Version));
        }

        [Test]
        public void IsAnInstanceOfAnyFailsWithProperMessage()
        {
            var father = new Person {Name = "Odysseus"};
            Check.ThatCode(()=>
                    Check.That(father).IsAnInstanceOfOneOf(typeof(int), typeof(string), typeof(Version))).
                IsAFaillingCheckWithMessage("",
                    "The checked value's type is not one of the expected types.",
                    "The checked value's type:",
                    "\t[NFluent.Tests.Person]",
                    "The expected value's type: one of those",
                    "\t[int, string, System.Version]");
            int? val = 12;
            Check.ThatCode(() => Check.That(val).IsAnInstanceOfOneOf(typeof(int), typeof(string), typeof(Person)))
                .IsAFaillingCheckWithMessage("",
                    "The checked value's type is not one of the expected types.",
                    "The checked value's type:",
                    "\t[int?]", 
                    "The expected value's type: one of those", 
                    "\t[int, string, NFluent.Tests.Person]");

            // check for specific case when only one type
            Check.ThatCode(() => Check.That(val).IsAnInstanceOfOneOf(typeof(int)))
                .IsAFaillingCheckWithMessage("",
                    "The checked value is not an instance of [int].",
                    "The checked value:",
                    "\t[12] of type: [int?]", 
                    "The expected value:", 
                    "\tan instance of type: [int]");
        }

    }
}
