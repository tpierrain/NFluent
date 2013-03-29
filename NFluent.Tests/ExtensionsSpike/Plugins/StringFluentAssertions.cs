namespace Spike.Plugins
{
    using System;

    using Spike.Core;

    public static class StringFluentAssertions
    {
        public static void HasTheForce(this IFluentAssertion<string> fluentAssertionInterface)
        {
            if (!fluentAssertionInterface.Sut.ToLower().Contains("force")) throw new Exception("damn, you're so common!");
        }
    }
}