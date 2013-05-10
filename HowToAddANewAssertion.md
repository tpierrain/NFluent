How to create a new assertion method in NFluent
===============================================

Fortunately, __NFluent may be extended very easily for your own needs__.
Whether you want to add a new assertion method on an already covered type, 
or you want to add assertion methods on a type not already covered by the library 
(one of your specific business domain object type for instance): both are easily feasible.

NFluent extensibility model
---------------------------
It's based on .NET extension methods that you may add on `IFluentAssertion<T>` instances (with T as 
the system under test. a.k.a. sut), and a default implementation using a NFluent assertion runner so that
your assertion will be by default compliant with the 'Not' operator.

Now, let's see together how to make it properly:

- - - 

To create a new assertion method, write a test for it (TDD ;-), and then follow the same implementation pattern 
as the one used for the StartsWith() assertion (applying on string) presented below:

```c#
		public static IChainableFluentAssertion<IFluentAssertion<string>> StartsWith(this IFluentAssertion<string> fluentAssertion, string expectedPrefix)
        {
			// Every assertion method starts by some cast operations in order to retrieve the assertion runner
			// and the runnable assertion (both are implemented by the concrete type FluentAssertion<T>)
            var assertionRunner = fluentAssertion as IFluentAssertionRunner<string>;
            var runnableAssertion = fluentAssertion as IRunnableAssertion<string>;

			// Then, we make the runner's ExecuteAssertion() method returning the chainable result
			// This method needs 2 arguments:
			//	 1- a lambda that checks what's necessary, and throws a FluentAssertionException in case of failure
			//   2- a string containing the exception message that should be thrown by the assertion runner
			//	    if the negated version of the assertion is requested (i.e. when the 'Not' operator has 
			//      been set just before) and only if it fails.
            return assertionRunner.ExecuteAssertion(
                () =>
                    {
						// the lambda that do the job
                        if (!runnableAssertion.Value.StartsWith(expectedPrefix))
                        {
                            throw new FluentAssertionException(string.Format("\nThe actual string:\n\t[{0}]\ndoes not start with:\n\t[{1}].", runnableAssertion.Value.ToStringProperlyFormated(), expectedPrefix.ToStringProperlyFormated()));
                        }
                    },
				// The error message for the negatable un-happy path
                string.Format("\nThe actual string:\n\t[{0}]\nstarts with:\n\t[{1}]\nwhich was not expected.", runnableAssertion.Value.ToStringProperlyFormated(), expectedPrefix.ToStringProperlyFormated()));
        }
```


General recommendations
=======================

+ __names__ of the assertion methods __should be chosen carefully__ and smartly embrace the intellisense autocompletion mechanism (i.e. the 'dot' experience).
+ you should __avoid using lambda expressions as assertion methods arguments__ (cause writing a lambda expression within an assertion statement is not really a fluent experience, neither on a reading perspective)
+ every __assertion method should return a chainable assertion, and should throw a FluentAssertionException when failing__ (to make your favorite unit test framwork fail __with a clear status message__.
+ the message of all the FluentAssertionException you throw should be clear as crystal, but also compliant with the ready-to-be-copied-and-paste-for-arrays-or-collections-initialization-purpose objective of NFluent  



