namespace Spike.Tests
{
    using System;

    using NUnit.Framework;

    using Spike.Plugins;

    using Check = Spike.Core.Check;

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

        [Test]
        public void WorksWhenSutIsADerivedTypeOfTheInterfaceReferencedAsTheTypeParameterOfTheFluentAssertion()
        {
            Version v1 = new Version(1,0);
            Version v2 = new Version(2,0);

            Check.That(v1).IsBefore(v2);
        }
    }
}
