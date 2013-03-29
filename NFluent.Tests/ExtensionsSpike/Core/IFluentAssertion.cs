namespace Spike.Core
{
    public interface IFluentAssertion<out T>
    {
        T Sut { get; }
    }
}