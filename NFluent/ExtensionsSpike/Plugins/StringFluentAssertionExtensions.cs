namespace Spike.Plugins
{
    using System;
    using Spike.Core;

    public static class StringFluentAssertionExtensions
    {
        public static void HasTheForce(this IFluentAssertion<string> fluentAssertion)
        {
            if (!fluentAssertion.Sut.ToLower().Contains("force")) throw new Exception("damn, you're so common!");
        }
    }
}