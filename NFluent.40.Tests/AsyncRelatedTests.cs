
namespace NFluent.Tests
{
    using System.Security;
    using System.Threading;
    using System.Threading.Tasks;

    using NUnit.Framework;

    [TestFixture]
    public class AsyncRelatedTests
    {
        private const int ASecondInMsec = 1000;

        [Test]
        [Ignore("Failing test which forces failure of other tests")]
        public void ExceptionFromAsynchronousCodeIsWellDetected()
        {
            Check.ThatCode(this.DoSomethingAsync).Throws<SecurityException>();
        }

        private async void DoSomethingAsync()
        {
            await this.ThrowSecurityExceptionAfterASecond();
        }

        private Task ThrowSecurityExceptionAfterASecond()
        {
            return Task.Run(() =>
            {
                // This operation takes a while
                Thread.Sleep(ASecondInMsec);
                throw new SecurityException("What?!?");
            });
        }
    }
}
