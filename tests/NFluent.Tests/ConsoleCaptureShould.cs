using NUnit.Framework;

namespace NFluent.Tests
{
    using System;
    using System.IO;

    [TestFixture]
    class ConsoleCaptureShould
    {
        [Test]
        public void CaptureOutput()
        {
            using(var session = new CaptureConsole())
            {
                Console.Write("hello");
                Check.That(session.Output).IsEqualTo("hello");
            }
        }
    }

    internal sealed class CaptureConsole : IDisposable
    {
        private TextWriter OldOut;
        private StringWriter NewOut;

        public CaptureConsole()
        {
            this.NewOut = new StringWriter();
            this.OldOut = Console.Out;
            Console.SetOut(this.NewOut);
        }

        public void Dispose()
        {
            Console.SetOut(this.OldOut);
        }

        public string Output => this.NewOut.ToString();
    }
}
    