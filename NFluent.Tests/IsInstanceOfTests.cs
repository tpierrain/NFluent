namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class IsInstanceOfTests
    {
        [Test]
        public void IsInstanceOfWorks()
        {
            const string StringObj = "for unit testing";
            double intObj = 23;
            long longObj = long.MaxValue;
            double doubleObj = 23d;
            var person = new Person();

            Assert.That(StringObj.IsInstanceOf(typeof(string)));
            Assert.That(intObj.IsInstanceOf(typeof(double)));
            Assert.That(longObj.IsInstanceOf(typeof(long)));
            Assert.That(doubleObj.IsInstanceOf(typeof(double)));
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
    }
}
