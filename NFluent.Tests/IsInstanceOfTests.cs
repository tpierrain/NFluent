namespace NFluent.Tests
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class IsInstanceOfTests
    {
        [Test]
        public void IsInstanceOfWorks()
        {
            const string StringObj = "for unit testing";
            const int IntObj = 23;
            const long LongObj = long.MaxValue;
            const double DoubleObj = 23d;
            var person = new Person();

            Assert.That(StringObj.IsInstanceOf(typeof(string)));
            Assert.That(IntObj.IsInstanceOf(typeof(int)));
            Assert.That(LongObj.IsInstanceOf(typeof(long)));
            Assert.That(DoubleObj.IsInstanceOf(typeof(double)));
            Assert.That(person.IsInstanceOf(typeof(Person)));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[23] is not an instance of the expectedType [NFluent.Tests.Person] but of [System.Int32] instead.")]
        public void IsInstanceOfThrowsExceptionWithProperFormatWhenFailsWithInt()
        {
            const int IntObject = 23;

            Assert.That(IntObject.IsInstanceOf(typeof(Person)));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""for unit testing""] is not an instance of the expectedType [NFluent.Tests.Person] but of [System.String] instead.")]
        public void IsInstanceOfThrowsExceptionWithProperFormatWhenFailsWithString()
        {
            const string StringObj = "for unit testing";
            
            Assert.That(StringObj.IsInstanceOf(typeof(Person)));
        }

        [Test]
        public void IsNotInstanceOfWorks()
        {
            const double DoubleObj = 23d;

            Assert.That(DoubleObj.IsNotInstanceOf(typeof(string)));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[23] is an instance of the type [System.Int32] which is not expected.")]
        public void IsNotInstanceOfThrowsExceptionWithProperFormatWhenFailsWithInt()
        {
            const int IntObject = 23;

            Assert.That(IntObject.IsNotInstanceOf(typeof(int)));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""Failure is mother of success""] is an instance of the type [System.String] which is not expected.")]
        public void IsNotInstanceOfThrowsExceptionWithProperFormatWhenFailsWithString()
        {
            const string Statement = "Failure is mother of success";

            Assert.That(Statement.IsNotInstanceOf(typeof(string)));
        }
    }
}
