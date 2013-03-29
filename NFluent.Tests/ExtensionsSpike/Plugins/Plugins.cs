namespace Spike.Plugins
{
    using System;
    using Spike.Core;

    public class IntFluentAssertion : IFluentAssertion<int>
    {
        public int Sut { get; private set; }
        public IntFluentAssertion(int? sut) { Sut = sut ?? 0; }
    }

    public static class IntExt
    {
        public static void IsCoolNumber(this IFluentAssertion<int> fluentAssertionInterface)
        {
            if (fluentAssertionInterface.Sut != 42) throw new Exception("Not cool, try 42!");
        }
    }
}
