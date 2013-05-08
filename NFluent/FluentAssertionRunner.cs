namespace NFluent
{
    using System;

    /// <summary>
    /// Provides a mean to execute a fluent assertion, taking care of whether it should be negated or not, etc.
    /// This interface is designed for developers that need to add new assertion (extension) methods.
    /// Thus, it should not be exposed via Intellisense to developers that are using NFluent to write 
    /// assertions statements.
    /// </summary>
    /// <typeparam name="T">Type of the value to assert on.</typeparam>
    internal class FluentAssertionRunner<T> : IFluentAssertionRunner<T>
    {
        /// <summary>
        /// Executes the assertion provided as an happy-path lambda (vs lambda for negated version).
        /// </summary>
        /// <param name="fluentAssertion">The fluent assertion.</param>
        /// <param name="action">The action.</param>
        /// <param name="negatedExceptionMessage">The message for the negated exception.</param>
        /// <returns>
        /// A new chainable fluent assertion.
        /// </returns>
        /// <exception cref="FluentAssertionException">The assertion fails.</exception>
        public IChainableFluentAssertion<IFluentAssertion<T>> ExecuteAssertion(INegatedAndForkableAssertion fluentAssertion, Action action, string negatedExceptionMessage)
        {
            if (fluentAssertion.Negated)
            {
                // The exact opposite ;-)
                bool mustThrow = false;
                try
                {
                    action();
                    mustThrow = true;
                }
                catch (FluentAssertionException)
                {
                }

                if (mustThrow)
                {
                    throw new FluentAssertionException(negatedExceptionMessage);
                }
            }
            else
            {
                // May throw FluentAssertionException
                action();
            }

            return new ChainableFluentAssertion<IFluentAssertion<T>>(fluentAssertion);
        }
    }
}