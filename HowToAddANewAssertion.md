How to create a new check method in NFluent
===============================================

Fortunately, __NFluent may be extended very easily for your own needs__.
Whether you want to add a new check method on an already covered type, 
or you want to add check methods on a type not already covered by the library 
(one of your specific business domain object type for instance): both are easily feasible.

NFluent extensibility model
---------------------------
It's based on .NET extension methods that you may add on `ICheck<T>` instances (with T as 
the system under test. a.k.a. sut), and a default implementation using a Nfluent check runner so that
your check will be by default compliant with the 'Not' operator.

Now, let's see together how to make it properly:

- - - 

To create a new check method, write a test for it (TDD ;-), and then follow the same implementation pattern 
as the one used for the StartsWith() check (applying on string) presented below:

```c#
		public static IChainableFluentAssertion<IFluentAssertion<string>> StartsWith(this IFluentAssertion<string> fluentAssertion, string expectedPrefix)
        {
			// Every check method starts by some cast operations in order to retrieve the check runner
			// and the runnable check (both are implemented by the concrete type FluentAssertion<T>)
            var checkRunner = fluentAssertion as IFluentAssertionRunner<string>;
            var runnableCheck = fluentAssertion as IrunnableCheck<string>;

			// Then, we make the runner's ExecuteAssertion() method returning the chainable result
			// This method needs 2 arguments:
			//	 1- a lambda that checks what's necessary, and throws a FluentAssertionException in case of failure
			//   2- a string containing the exception message that should be thrown by the check runner
			//	    if the negated version of the check is requested (i.e. when the 'Not' operator has 
			//      been set just before) and only if it fails.
            return checkRunner.ExecuteAssertion(
                () =>
                    {
						// the lambda that do the job
                        if (!runnableCheck.Value.StartsWith(expectedPrefix))
                        {
                            throw new FluentAssertionException(string.Format("\nThe actual string:\n\t[{0}]\ndoes not start with:\n\t[{1}].", runnableCheck.Value.ToStringProperlyFormated(), expectedPrefix.ToStringProperlyFormated()));
                        }
                    },
				// The error message for the negatable un-happy path
                string.Format("\nThe actual string:\n\t[{0}]\nstarts with:\n\t[{1}]\nwhich was not expected.", runnableCheck.Value.ToStringProperlyFormated(), expectedPrefix.ToStringProperlyFormated()));
        }
```


General recommendations
=======================

+ __names__ of the check methods __should be chosen carefully__ and smartly embrace the intellisense autocompletion mechanism (i.e. the 'dot' experience).
+ you should __avoid using lambda expressions as check methods arguments__ (cause writing a lambda expression within an check statement is not really a fluent experience, neither on a reading perspective)
+ every __assertion method should return a chainable check, and should throw a FluentAssertionException when failing__ (to make your favorite unit test framwork fail __with a clear status message__.
+ the message of all the FluentAssertionException you throw should be clear as crystal, but also compliant with the ready-to-be-copied-and-paste-for-arrays-or-collections-initialization-purpose objective of NFluent  



