namespace Spike.Tests
{
    using System;

    using NUnit.Framework;

    using Spike.Plugins;
    using Spike.Core;
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
            // here: 
            // the interface referenced as the type parameter of the fluent assertion: IComparable
            // and the sut type: System.Version
            Version v1 = new Version(1,0);
            Version v2 = new Version(2,0);

            Check.That(v1).IsBefore(v2);
        }

        [Test]
        public void CheckWorksWithDecimal()
        {
            Decimal one = new decimal(1);
            Decimal pi = new decimal(Math.PI);

            Check.That(pi).IsInstanceOf<decimal>().And.IsNotZero().And.IsPositive();

            // TODO make the next line to build
            //Check.That(one).IsBefore(pi);
        }
    }
}
