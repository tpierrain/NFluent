namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;

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
            List<string> strings = new List<string>();
            int[] integers = new int[10];

            // numerics
            Check.That(StringObj).IsInstanceOf(typeof(string));
            Check.That(IntObj).IsInstanceOf(typeof(int));
            Check.That(LongObj).IsInstanceOf(typeof(long));
            Check.That(DoubleObj).IsInstanceOf(typeof(double));
           
            // objects
            Check.That(person).IsInstanceOf(typeof(Person));
            Check.That(person).IsNotInstanceOf(typeof(string));
            
            // IEnumerable
            Check.That(strings).IsInstanceOf(typeof(List<string>));
            Check.That(strings).IsInstanceOf(typeof(List<string>));
            Check.That(strings).IsNotInstanceOf(typeof(IEnumerable<string>));

            Check.That(integers).IsInstanceOf(typeof(int[]));
            Check.That(integers).IsNotInstanceOf(typeof(IEnumerable));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[23] is not an instance of the expectedType [NFluent.Tests.Person] but of [System.Int32] instead.")]
        public void IsInstanceOfThrowsExceptionWithProperFormatWhenFailsWithInt()
        {
            const int IntObject = 23;

            Check.That(IntObject).IsInstanceOf(typeof(Person));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""for unit testing""] is not an instance of the expectedType [NFluent.Tests.Person] but of [System.String] instead.")]
        public void IsInstanceOfThrowsExceptionWithProperFormatWhenFailsWithString()
        {
            const string StringObj = "for unit testing";
            
            Check.That(StringObj).IsInstanceOf(typeof(Person));
        }

        [Test]
        public void IsNotInstanceOfWorks()
        {
            const double DoubleObj = 23d;

            Check.That(DoubleObj).IsNotInstanceOf(typeof(string));
        }

        [Test]
        public void IsNotInstanceOfWorksWithString()
        {
            const string MotivationalSaying = "Failure is mother of success.";
            Check.That(MotivationalSaying).IsNotInstanceOf(typeof(int));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "[23] is an instance of the type [System.Int32] which is not expected.")]
        public void IsNotInstanceOfThrowsExceptionWithProperFormatWhenFailsWithInt()
        {
            const int IntObject = 23;

            Check.That(IntObject).IsNotInstanceOf(typeof(int));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""If you don’t want to slip up tomorrow, speak the truth today (Bruce Lee).""] is an instance of the type [System.String] which is not expected.")]
        public void IsNotInstanceOfThrowsExceptionWithProperFormatWhenFailsWithString()
        {
            const string Statement = "If you don’t want to slip up tomorrow, speak the truth today (Bruce Lee).";

            Check.That(Statement).IsNotInstanceOf(typeof(string));
        }

        // TODO: add unit test related to theIsNotInstance error messages (for IEnumerable, object, etc)
    }
}
