namespace Spike.Tests
{
    using System;
    using System.Collections.Generic;

    using NFluent.Tests;
    using NFluent.Tests.Extensions;

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
            const int MythicNumber = 42;
            const string Yoda = "Has the force";

            Check.That(MythicNumber).IsTheUltimateQuestionOfLifeAnswer();
            Check.That(Yoda).HasTheForce();
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

        [Test]
        public void CanLoadExtensionsFromAnotherAssembly()
        {
            var eternalSunshineOfTheSpotlessMind = new Movie("Eternal sunshine of the spotless mind", new Person() { Name = "Michel GONDRY" }, new List<Nationality>() { Nationality.American });

            Check.That(eternalSunshineOfTheSpotlessMind).IsDirectedBy("Michel GONDRY");
            Check.That(eternalSunshineOfTheSpotlessMind).IsAFGreatMovie();
        }
    }
}
