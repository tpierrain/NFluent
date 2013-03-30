namespace Spike.Core
{
    using NFluent.Tests.ExtensionsSpike.Core;

    public static class Check
    {
        public static IFluentAssertion<T> That<T>(T sut)
        {
            return GetSutWrapper<T>(sut);
        }

        private static IFluentAssertion<T> GetSutWrapper<T>(T sut)
        {
            return new FluentAssertion<T>(sut);
        }
    }
}