How to create a new check method in NFluent
===============================================

Fortunately, __NFluent may be extended very easily for your own needs__.
Whether you want to add a new check method on an already covered type, 
or you want to add check methods on a type not already covered by the library 
(one of your specific business domain object type for instance): both are easily feasible.

NFluent extensibility model
---------------------------
It's based on .NET extension methods that you may add on `ICheck<T>` instances (with T as 
the system under test. a.k.a. sut), and a default implementation using a NFluent checker so that
your check will be by default compliant with the 'Not' operator.

Now, let's see together how to make it properly:

- - - 

To create a new check method, write a test for it (TDD ;-), and then follow the same implementation pattern 
as the one used for the StartsWith() check (applying on string) presented below:

```c#
		public static ICheckLink<ICheck<char>> IsALetter(this ICheck<char> check)
        {
            // Every check method starts by extracting a checker instance from the check thanks to
            // the ExtensibilityHelper static class.
            var checker = ExtensibilityHelper<char>.ExtractChecker(check);

            // Then, we let the checker's ExecuteCheck() method return the ICheckLink<ICheck<T>> result (with T as string here).
            // This method needs 2 arguments:
            //   1- a lambda that checks what's necessary, and throws a FluentAssertionException in case of failure
            //      The exception message is usually fluently build with the FluentMessage.BuildMessage() static method.
            //
            //   2- a string containing the message for the exception to be thrown by the checker when 
            //      the check fails, in the case we were running the negated version.
            //
            // e.g.:
            return checker.ExecuteCheck(
                () =>
                {
                    if (!IsALetter(checker.Value))
                    {
                        var errorMessage = FluentMessage.BuildMessage("The {0} is not a letter.").For("char").On(checker.Value).ToString();
                        
                        throw new FluentCheckException(errorMessage);
                    }
                },
                FluentMessage.BuildMessage("The {0} is a letter whereas it must not.").For("char").On(checker.Value).ToString());
        }
```


General recommendations
=======================
+ __names__ of the check methods __should be chosen carefully__ and smartly embrace the intellisense autocompletion mechanism (i.e. the 'dot' experience).
+ you should __avoid using lambda expressions as check methods arguments__ (cause writing a lambda expression within an check statement is not really a fluent experience, neither on a reading perspective)
+ every __assertion method should return A check link, and should throw a FluentAssertionException when failing__ (to make your favorite unit test framwork fail __with a clear status message__.
+ the message of all the FluentAssertionException you throw should be clear as crystal, but also compliant with the ready-to-be-copied-and-paste-for-arrays-or-collections-initialization-purpose objective of NFluent  

Other resources
===============
+ Rui has published a great article about the NFluent extensibility model. Available __[here on CodeDistillers](http://www.codedistillers.com/rui/2013/11/26/nfluent-extensions/)__

