namespace Spike.Plugins
{
    using Spike.Core;

    public class StringFluentAssertion : IFluentAssertion<string>
    {
        public string Sut { get; private set; }
        public StringFluentAssertion(string sut) { this.Sut = sut; }
    }
}