namespace NFluent.Tests
{
    using System;

    using NFluent.Helpers;

    using NUnit.Framework;

    [TestFixture]
    public class IsInstanceHelperTests
    {
        [Test]
        public void IsSameTypeWorksWithNullables()
        {
            int? one = 1;

            IsInstanceHelper.IsSameType(typeof(Nullable<int>), typeof(Nullable<int>), one);
        }

        [Test]
        public void IsSameTypeWorksWithNullablesEvenWithoutValue()
        {
            int? nullNullable = null;

            IsInstanceHelper.IsSameType(typeof(Nullable<int>), typeof(Nullable<int>), nullNullable);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[1]\nis not an instance of:\n\t[System.Nullable`1[System.Int64]]\nbut an instance of:\n\t[System.Nullable`1[System.Int32]]\ninstead.")]
        public void IsSameTypeThrowsProperExceptionWhenFailingWithANullableWithAValue()
        {
            int? one = 1;

            // should pass null instead ot the nullNullable.Value in that case to prevent from throwing a NullReferenceException
            IsInstanceHelper.IsSameType(typeof(Nullable<int>), typeof(Nullable<long>), one);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis not an instance of:\n\t[System.Nullable`1[System.Int64]]\nbut an instance of:\n\t[System.Nullable`1[System.Int32]]\ninstead.")]
        public void IsSameTypeThrowsProperExceptionWhenFailingWithANullableWithoutValue()
        {
            int? nullNullable = null;

            // should pass null instead ot the nullNullable.Value in that case to prevent from throwing a NullReferenceException
            IsInstanceHelper.IsSameType(typeof(Nullable<int>), typeof(Nullable<long>), nullNullable);
        }

        [Test]
        public void BuildErrorMessageWorks()
        {
            // Time to eat our own dog food ;-)
            Check.That(IsInstanceHelper.BuildErrorMessage(3, typeof(int), true)).IsEqualTo("\nThe actual value:\n\t[3]\nis an instance of:\n\t[System.Int32]\nwhich was not expected.");
        }
    }
}
