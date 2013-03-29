namespace Spike.Plugins
{
    using System;

    using Spike.Core;

    public static class IntFluentAssertionExtensions
    {
        public static void IsCoolNumber(this IFluentAssertion<int> fluentAssertionInterface)
        {
            if (fluentAssertionInterface.Sut != 42) throw new Exception("Not cool, try 42!");
        }
    }
}