namespace Spike.Core
{
    public class FluentAssertFactory
    {
        public IFluentAssertion<object> GetInterface(object sut)
        {
            return new ObjectFluentAssertion(sut);
        }
    }
}
