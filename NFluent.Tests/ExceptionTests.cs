namespace NFluent.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ExceptionTests
    {
        [Test]
        public void NoExceptionRaised()
        {
            Check.That(() => new object()).DoesNotThrow();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.StartsWith, ExpectedMessage = "\nThe checked code raised an exception, whereas it must not.")]
        public void UnexpectedExceptionRaised()
        {
            Check.That(() => { throw new ApplicationException(); }).DoesNotThrow();
        }

        [Test]
        public void ExpectedExceptionRaised()
        {
            Check.That(() => { throw new InvalidOperationException(); }).Throws<InvalidOperationException>();
            Check.That(() => { throw new ApplicationException(); }).ThrowsAny();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.Contains, ExpectedMessage = "\nThe checked code raised an exception of a different type than expected.")]
        public void DidNotRaiseExpected()
        {
            Check.That(() => { throw new Exception(); }).Throws<ApplicationException>();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked code did not raise an exception, whereas it must.")]
        public void DidNotRaiseAny()
        {
            Check.That(() => { new object(); }).ThrowsAny();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked code did not raise an exception, whereas it must.\nExpected exception type is:\n\t[System.Exception]")]
        public void DidNotRaiseAnyTypedCheck()
        {
            Check.That(() => { new object(); }).Throws<Exception>();
        }
    }
}