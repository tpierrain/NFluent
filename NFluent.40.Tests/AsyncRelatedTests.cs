
namespace NFluent.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using NUnit.Framework;

    [TestFixture]
    public class AsyncRelatedTests
    {
        private const int ASecondInMsec = 1000;

        //[Test]
        //public void LateExceptionFromAsynchronousCodeIsNotCatchedWithTheClassicalThatCodeCheck()
        //{
        //    Check.ThatCode(this.DoSomethingBadAsync).Not.Throws<InvalidOperationException>();
        //}
#if dotNet45

        [Test]
        public void LateExceptionFromAsynchronousCodeIsNotCatchedWithTheThatAsyncCodeCheck()
        {
            Check.ThatAsyncCode(this.DoSomethingBadAsync()).Throws<InvalidOperationException>();
        }

#endif

        private async Task DoSomethingBadAsync()
        {
            await Task.Run(() =>
            {
                // This operation takes a while
                Thread.Sleep(ASecondInMsec);
                //throw new InvalidOperationException("What?!?");
            });
        }
    }
}
