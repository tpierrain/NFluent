namespace Spike.Core
{
    using System.ComponentModel;

    public interface IFluentAssertion<out T>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        T Sut { get; }
    }
}