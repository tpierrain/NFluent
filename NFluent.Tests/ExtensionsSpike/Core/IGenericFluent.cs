namespace Spike.Core
{
    public interface IGenericFluent<out T>
    {
        T Sut { get; }
    }
}