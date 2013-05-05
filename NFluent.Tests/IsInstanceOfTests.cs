namespace NFluent.Tests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class IsInstanceOfTests
    {
        private const string StringObj = "for unit testing";
        private const byte ByteObj = 1;
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

        private readonly DateTime dateTimeObj = new DateTime();
        private readonly TimeSpan timeSpanObj = new TimeSpan();
        private readonly int[] integerArray = new int[10];
        private readonly Version firstVersion = new Version(1, 0, 0, 0);
        private int[] emptyIntegerArray = new int[10];
        private List<string> stringList = new List<string>();
        private Person person = new Person() { Name = "Charles BAUDELAIRE" };

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

            // POCO
            Check.That(this.person).IsInstanceOf<Person>();

            // IEnumerable
            Check.That(this.stringList).IsInstanceOf<List<string>>();
            Check.That(this.integerArray).IsInstanceOf<int[]>();
            
            // Version
            Check.That(this.firstVersion).IsInstanceOf<Version>();
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

            // POCO
            Check.That(this.person).Not.IsInstanceOf<string>();

            // IEnumerable
            Check.That(this.stringList).Not.IsInstanceOf<string>();
            Check.That(this.integerArray).Not.IsInstanceOf<string>();

            // Version
            Check.That(this.firstVersion).Not.IsInstanceOf<string>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[Telemachus]\nis not an instance of:\n\t[NFluent.Tests.Person]\nbut an instance of:\n\t[NFluent.Tests.Child]\ninstead.")]
        public void IsInstanceOfThrowsExceptionWithDerivedTypeAsCheckedExpression()
        {
            var child = new Child() { Name = "Telemachus" };
            Check.That(child).IsInstanceOf<Person>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[23]\nis not an instance of:\n\t[NFluent.Tests.Person]\nbut an instance of:\n\t[System.Int32]\ninstead.")]
        public void IsInstanceOfThrowsExceptionWithProperFormatWhenFailsWithInt()
        {
            Check.That(IntObj).IsInstanceOf<Person>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[\"for unit testing\"]\nis not an instance of:\n\t[NFluent.Tests.Person]\nbut an instance of:\n\t[System.String]\ninstead.")]
        public void IsInstanceOfThrowsExceptionWithProperFormatWhenFailsWithString()
        {
            Check.That(StringObj).IsInstanceOf<Person>();
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

            // POCO
            Check.That(this.person).IsNotInstanceOf<string>();

            // IEnumerable
            Check.That(this.stringList).IsNotInstanceOf<string>();
            Check.That(this.integerArray).IsNotInstanceOf<string>();

            // Version
            Check.That(this.firstVersion).IsNotInstanceOf<string>();
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
            
            // POCO
            Check.That(this.person).Not.IsNotInstanceOf<Person>();
            
            // IEnumerable
            Check.That(this.stringList).Not.IsNotInstanceOf<List<string>>();
            Check.That(this.integerArray).Not.IsNotInstanceOf<int[]>();
            
            // Version
            Check.That(this.firstVersion).Not.IsNotInstanceOf<Version>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[23]\nis an instance of:\n\t[System.Int32]\nwhich is not expected.")]
        public void IsNotInstanceOfThrowsExceptionWithProperFormatWhenFailsWithInt()
        {
            const int IntObject = 23;
            Check.That(IntObject).IsNotInstanceOf<int>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[\"If you don’t want to slip up tomorrow, speak the truth today (Bruce Lee).\"]\nis an instance of:\n\t[System.String]\nwhich is not expected.")]
        public void IsNotInstanceOfThrowsExceptionWithProperFormatWhenFailsWithString()
        {
            const string Statement = "If you don’t want to slip up tomorrow, speak the truth today (Bruce Lee).";
            Check.That(Statement).IsNotInstanceOf<string>();
        }

        #endregion

        [Test]
        public void InheritsFromWorks()
        {
            var child = new Child() { Name = "Telemachus" };
            Check.That(child).InheritsFrom<Person>();
        }

        [Test]
        public void InheritsFromWorksAlsoWithTheSameType()
        {
            var child = new Child() { Name = "Telemachus" };
            Check.That(child).InheritsFrom<Child>();
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe checked expression is not of part of the inheritance hierarchy, or of the same type than the specified one.\nIndeed, checked expression type:\n\t[NFluent.Tests.Person]\nis not a derived type of\n\t[NFluent.Tests.Child].")]
        public void InheritsFromThrowsExceptionWhenFailing()
        {
            var father = new Person() { Name = "Odysseus" };
            Check.That(father).InheritsFrom<Child>();
        }
    }
}
