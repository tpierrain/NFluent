namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class IsInstanceOfTests
    {
        #region Generic IsInstanceOf tests

        [Test]
        public void GenericIsInstanceOfWorks()
        {
            const string StringObj = "for unit testing";
            const int IntObj = 23;
            const long LongObj = long.MaxValue;
            const double DoubleObj = 23d;
            var person = new Person();
            List<string> strings = new List<string>();
            int[] integers = new int[10];

            // numerics
            Check.That(StringObj).IsInstanceOf<string>();
            //Check.That(IntObj).IsInstanceOf<int>();
            //Check.That(LongObj).IsInstanceOf<long>();
            //Check.That(DoubleObj).IsInstanceOf<double>();

            //// objects
            //Check.That(person).IsInstanceOf<Person>();

            //// IEnumerable
            //Check.That(strings).IsInstanceOf<List<string>>();
            //Check.That(integers).IsInstanceOf<int[]>();
        }

        #endregion

        #region IsIntanceOf tests

        [Test]
        public void IsInstanceOfWorks()
        {
            // TODO split in two tests: one for IsInstanceOf(), the other for IsNotInstanceOf
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

            // IEnumerable
            Check.That(strings).IsInstanceOf(typeof(List<string>));
            Check.That(strings).IsInstanceOf(typeof(List<string>));
            Check.That(integers).IsInstanceOf(typeof(int[]));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException),
            ExpectedMessage =
                "[23] is not an instance of the expected type [NFluent.Tests.Person] but of [System.Int32] instead.")]
        public void IsInstanceOfThrowsExceptionWithProperFormatWhenFailsWithInt()
        {
            const int IntObject = 23;
            Check.That(IntObject).IsInstanceOf(typeof(Person));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = @"[""for unit testing""] is not an instance of the expected type [NFluent.Tests.Person] but of [System.String] instead.")]
        public void IsInstanceOfThrowsExceptionWithProperFormatWhenFailsWithString()
        {
            const string StringObj = "for unit testing";
            Check.That(StringObj).IsInstanceOf(typeof(Person));
        }

        #endregion

        #region Generic IsNotInstanceOf tests

        [Test]
        public void GenericIsNotInstanceOfWorks()
        {
            const string StringObj = "for unit testing";
            const int IntObj = 23;
            const long LongObj = long.MaxValue;
            const double DoubleObj = 23d;
            var person = new Person();
            List<string> strings = new List<string>();
            int[] integers = new int[10];

            // numerics
            Check.That(StringObj).IsNotInstanceOf<int>();
            Check.That(IntObj).IsNotInstanceOf<long>();
            Check.That(LongObj).IsNotInstanceOf<string>();
            Check.That(DoubleObj).IsNotInstanceOf<int>();

            // objects
            Check.That(person).IsNotInstanceOf<NumbersRelatedTests>();

            // IEnumerable
            Check.That(strings).IsNotInstanceOf<List<int>>();
            Check.That(integers).IsNotInstanceOf<string[]>();
        }

        #endregion

        #region IsNotInstanceOf tests

        [Test]
        public void IsNotInstanceOfWorks()
        {
            const string StringObj = "for unit testing";
            const int IntObj = 23;
            const long LongObj = long.MaxValue;
            const double DoubleObj = 23d;
            decimal decimalObj = new decimal(23d);
            const float FloatObj = 23f;
 
            var person = new Person();
            List<string> strings = new List<string>();
            int[] integers = new int[10];

            // numerics
            Check.That(IntObj).IsNotInstanceOf(typeof(string[]));
            Check.That(LongObj).IsNotInstanceOf(typeof(decimal));
            Check.That(DoubleObj).IsNotInstanceOf(typeof(string));
            Check.That(decimalObj).IsNotInstanceOf(typeof(int));
            Check.That(FloatObj).IsNotInstanceOf(typeof(int));

            // objects
            Check.That(person).IsNotInstanceOf(typeof(IsInstanceOfTests));

            // string
            Check.That(StringObj).IsNotInstanceOf(typeof(decimal));

            // IEnumerable
            Check.That(strings).IsNotInstanceOf(typeof(IEnumerable<string>));
            Check.That(strings).IsNotInstanceOf(typeof(Person));
            Check.That(integers).IsNotInstanceOf(typeof(IEnumerable));
        }

        [Test]
        public void IsNotInstanceOfWorksWithString()
        {
            const string MotivationalSaying = "Failure is mother of success.";
            Check.That(MotivationalSaying).IsNotInstanceOf(typeof(int));
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException),
            ExpectedMessage = "[23] is an instance of the type [System.Int32] which is not expected.")]
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

        #endregion

        // TODO: add unit test related to theIsNotInstance error messages (for IEnumerable, object, etc)
    }
}
