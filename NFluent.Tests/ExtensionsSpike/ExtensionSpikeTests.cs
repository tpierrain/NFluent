namespace Spike.Tests
{
    using NUnit.Framework;

    using Spike.Core;
    using Spike.Plugins;

    [TestFixture]
    public class ExtensionSpikeTests
    {
        [Test]
        public void CanUseVariousExtensions()
        {
            const int x = 42;
            const string yoda = "Has the force";

            Check.That(x).IsCoolNumber();
            Check.That(yoda).HasTheForce();
        }
    }
}
