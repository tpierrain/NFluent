using System;

namespace Spike.Core
{
    using NFluent;

    public static class DecimalFluentAssertionExtensions
    {
        public static IChainableFluentAssertion<INumberFluentAssertion> IsEqualTo(this IFluentAssertion<decimal> fluentAssertion, object expected)
        {
            // TODO transform NumberFluentAssertion<T> into a static class with functions only
            var numberAssertionStrategy = new NumberFluentAssertion<decimal>(fluentAssertion.Sut);
            return numberAssertionStrategy.IsEqualTo(expected);
        }

        public static IChainableFluentAssertion<INumberFluentAssertion> IsNotEqualTo(this IFluentAssertion<decimal> fluentAssertion, object expected)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<decimal>(fluentAssertion.Sut);
            return numberAssertionStrategy.IsNotEqualTo(expected);
        }

        public static IChainableFluentAssertion<INumberFluentAssertion> IsInstanceOf<T>(this IFluentAssertion<decimal> fluentAssertion)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<decimal>(fluentAssertion.Sut);
            return numberAssertionStrategy.IsInstanceOf<T>();
        }

        public static IChainableFluentAssertion<INumberFluentAssertion> IsNotInstanceOf<T>(this IFluentAssertion<decimal> fluentAssertion)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<decimal>(fluentAssertion.Sut);
            return numberAssertionStrategy.IsNotInstanceOf<T>();
        }

        public static IChainableFluentAssertion<INumberFluentAssertion> IsZero(this IFluentAssertion<decimal> fluentAssertion)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<decimal>(fluentAssertion.Sut);
            return numberAssertionStrategy.IsZero();
        }

        public static IChainableFluentAssertion<INumberFluentAssertion> IsNotZero(this IFluentAssertion<decimal> fluentAssertion)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<decimal>(fluentAssertion.Sut);
            return numberAssertionStrategy.IsNotZero();
        }

        public static IChainableFluentAssertion<INumberFluentAssertion> IsPositive(this IFluentAssertion<decimal> fluentAssertion)
        {
            var numberAssertionStrategy = new NumberFluentAssertion<decimal>(fluentAssertion.Sut);
            return numberAssertionStrategy.IsPositive();
        }
    }
}
