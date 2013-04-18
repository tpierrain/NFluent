namespace NFluent.Tests
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class ComparableRelatedTests
    {
        [Test]
        public void IsBeforeWorksWithVersion()
        {
            Version v1 = new Version(1, 0);
            Version v2 = new Version(2, 0);
            Version v3 = new Version(3, 0);

            Check.That(v1).IsBefore(v2).And.IsBefore(v3);
        }

        [Test]
        [Ignore("Expose IComparable extension methods to 'number' values so that autocompletion works on number values with those comparable assertions.")]
        public void IsBeforeWorksWithoutBeingForcedToCastItAsIComparable()
        {
            const int First = 1;
            const int Second = 2;

            Check.That(First as IComparable).IsBefore(Second);

            // TODO: make the next line build
            // Check.That(First).IsBefore(Second);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[2.0] of type: [System.Version]\nis not before:\n\t[1.0] of type: [System.Version].")]
        public void IsBeforeThrowsExceptionWhenFailing()
        {
            Version v1 = new Version(1, 0);
            Version v2 = new Version(2, 0);

            Check.That(v2).IsBefore(v1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis not before:\n\t[1.0] of type: [System.Version].")]
        public void IsBeforeDoesNotThrowNullReferenceExceptionWithNullAsInput()
        {
            Version v1 = new Version(1, 0);
            Version v2 = null;

            Check.That(v2).IsBefore(v1);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[2.0] of type: [System.Version]\nis not before:\n\t[null].")]
        public void IsBeforeDoesNotThrowNullReferenceExceptionWithNullAsAfterValue()
        {
            Version v1 = null;
            Version v2 = new Version(2, 0);

            Check.That(v2).IsBefore(v1);
        }
    }
}
