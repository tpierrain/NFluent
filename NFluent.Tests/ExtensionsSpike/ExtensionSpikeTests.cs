namespace Spike.Tests
{
    using System;

    using NFluent;

    using NUnit.Framework;

    using Spike.Core;
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
        public void DoNotThrowExceptionWhenSutIsASubTypeOfTheTypeReferencedInTheExtensionMethod()
        {
            Version v1 = new Version(1,0);
            Version v2 = new Version(2,0);

            Check.That(v1).IsBefore(v2);
        }
    }

    public class ComparableFluentAssertionAssertion : IFluentAssertion<IComparable>
    {
        public IComparable Sut { get; private set; }

        public ComparableFluentAssertionAssertion(IComparable sut)
        {
            this.Sut = sut;
        }
    }

    public static class ComparableExtensions
    {
        public static IFluentAssertion<IComparable> IsBefore(this IFluentAssertion<IComparable> fluentAssertionInterface, IComparable other)
        {
            if (fluentAssertionInterface.Sut.CompareTo(other) >= 0)
            {
                throw new FluentAssertionException("is not before.");
            }
            
            return fluentAssertionInterface;
        }
    }
}
