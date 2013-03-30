namespace NFluent.Tests.ExtensionsSpike.Core
{
    using Spike.Core;

    public class FluentAssertion<T> : IFluentAssertion<T>
    {
        public T Sut { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentAssertion{T}" /> class.
        /// </summary>
        /// <param name="sut">The sut.</param>
        public FluentAssertion(T sut)
        {
            this.Sut = sut;
        }
    }
}
