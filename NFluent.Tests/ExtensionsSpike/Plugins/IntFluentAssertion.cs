namespace Spike.Plugins
{
    using Spike.Core;

    public class IntFluentAssertion : IFluentAssertion<int>
    {
        public int Sut { get; private set; }
        public IntFluentAssertion(int? sut) { Sut = sut ?? 0; }
    }
}
