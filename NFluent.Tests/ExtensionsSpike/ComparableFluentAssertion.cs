namespace Spike.Tests
{
    using System;
    using Spike.Core;

    public class ComparableFluentAssertion : IFluentAssertion<IComparable>
    {
        public IComparable Sut { get; private set; }

        public ComparableFluentAssertion(IComparable sut)
        {
            this.Sut = sut;
        }
    }
}