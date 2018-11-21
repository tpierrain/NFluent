using NUnit.Framework;


namespace NFluent.Tests
{
    using System;
    using Mocks;

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

        [Test]
        public void SimulateInput()
        {
            using (var session = new CaptureConsole())
            {
                session.InputLine("hello");
                Check.That(Console.ReadLine()).IsEqualTo("hello");
                session.Input("AB");
                Check.That(Console.Read()).IsEqualTo('A');
            }
        }
    }
}
    