namespace Spike.Core
{
    public class ObjectFluent : IGenericFluent<object>
    {
        public object Sut { get; private set; }
        public ObjectFluent(object sut)
        {
            this.Sut = sut;
        }
    }
}