namespace Spike.Core
{
    public class FluentAssertFactory
    {
        public IGenericFluent<object> GetInterface(object sut)
        {
            return new ObjectFluent(sut);
        }
    }
}
