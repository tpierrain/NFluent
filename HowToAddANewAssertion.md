How to create a new assertion method in NFluent
===============================================

Fortunately, __NFluent may be extended very easily for your own needs__.
Whether you want to add a new assertion method on an already covered type, 
or you want to add assertion methods on a type not already covered by the library 
(one of your specific business domain object type for instance): both are easily feasible.

Now, let's see together how to make it properly:

NFluent extensibility model
---------------------------
It's based on .NET assertion methods that you may add on `IFluentAssertion<T>` instances (with T as the system under test. a.k.a. sut).

1. Create the .NET extension method you need
--------------------------------------------
On the type you wanna test. 

a. Returning the proper IChainableFluentAssertion
-------------------------------------------------

b. Throwing proper FluentAssertionException when failing
--------------------------------------------------------
Must respect the pattern:
"`\n`The actual \<value|string|enumerable|...\>\<specific explanation\>:`\n\t`\<actual value\>`\n`\<specific explanation\>:`\n\t`\<given value\>`.`"

c. Making your method compliant with the Not operator
-----------------------------------------------------
Thus, make your method implementation following the structure:

```c#
	// sample of the IsTrue assertion method (for bool):
	public static IChainableFluentAssertion<IFluentAssertion<bool>> IsTrue(this IFluentAssertion<bool> fluentAssertion)
    {
        if (fluentAssertion.Negated)
        {
            IsTrueNegatedImpl(fluentAssertion);
        }
        else
        {
            IsTrueImpl(fluentAssertion);    
        }

        return new ChainableFluentAssertion<IFluentAssertion<bool>>(fluentAssertion);
    }
```  
To be consistent with the rest of the lib, make all your ...Impl and ...NegatedImpl static private methods throw the exceptions.

Also in order to improve the rightness of the lib, make your ...NegatedImpl the opposite of the ...Impl. 
Concretely speaking, it lead to code such as:

```c#
	private static void IsTrueNegatedImpl(IFluentAssertion<bool> fluentAssertion)
    {
        try
        {
            IsTrueImpl(fluentAssertion);
        }
        catch (FluentAssertionException)
        {
            return;
        }

        throw new FluentAssertionException(string.Format("\nThe actual value:\n\t[{0}]\nis true.", fluentAssertion.Value.ToStringProperlyFormated()));
    }
```

2. Reference and use it from within your tests
----------------------------------------------



General recommendations
=======================

Before modifying this library with your own method, it is very important to keep in mind that
 __this library is designed to produce fluent assertions at the end!__

Thus, it means that:
+ __names__ of the assertion methods __should be chosen carefully__ and smartly embrace the intellisense autocompletion mechanism (i.e. the 'dot' experience).
+ you should __avoid using lambda expressions within the assertion methods__ (cause writing a lambda expression within an assertion statement is not really a fluent experience, neither on a reading perspective)
+ every __assertion method should return a chainable assertion, and should throw a FluentAssertionException when failing__ (to make your favorite unit test framwork fail __with a clear status message__.
+ the message of all the FluentAssertionException you throw should be clear as crystal, but also compliant with the ready-to-be-copied-and-paste-for-arrays-or-collections-initialization-purpose objective of NFluent  

Also, and to stay coherent with the equivalent **FEST fluent assert** Java library (interesting for people which are coding in those 2 platforms):
+ Thus, before introducing a new method, check the existence of a method name for the same thing within the java library (http://fest.easytesting.org/).



