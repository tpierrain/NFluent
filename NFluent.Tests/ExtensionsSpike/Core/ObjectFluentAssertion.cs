namespace Spike.Core
{
    public class ObjectFluentAssertion : IFluentAssertion<object>
    {
        public object Sut { get; private set; }

        public ObjectFluentAssertion(object sut)
        {
            this.Sut = sut;
        }
    }
}