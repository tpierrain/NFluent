NFluent Definition of Done (DoD) for Development
==================================

These are the rules that should be followed in order to contribute to this project:

1. Apache 2.0 License header is set on every source code file
2. No StyleCop warning
	+ this includes to have a proper documentation for the NFluent project (but not for the unit test one)
3. 100% of test coverage for the NFluent project
	+ Test names should be clear enough to know what is in stakes here (this is why I disable the "ElementsMustBeDocumented" StyleCop rule for tests)
4. With (of course) all unit tests passed ;-)
5. The entire build (i.e. including all the unit tests execution) takes less than a minute


Original intentions
-------------------

Before modifying this library, it is very important to keep in mind that __this library is designed to produce fluent assertions at the end!__

Thus, it means that:
+ __names__ of the assertion methods __should be chosen carefully__ and smartly embrace the intellisense autocompletion mechanism (i.e. the 'dot' experience).
+ you should __avoid using lambda expressions within the assertion methods__ (cause writing a lambda expression within an assertion statement is not really a fluent experience, neither on a reading perspective)
+ every __assertion method should return a chainable assertion, and should throw a FluentAssertionException when failing__ (to make your favorite unit test framwork fail __with a clear status message__.
+ the message of all the FluentAssertionException you throw should be clear as crystal, but also compliant with the ready-to-be-copied-and-paste-for-arrays-or-collections-initialization-purpose objective of NFluent  

Also, and to stay coherent with the equivalent **FEST fluent assert** Java library (interesting for people which are coding in those 2 platforms):
+ Thus, before introducing a new method, check the existence of a method name for the same thing within the java library (http://fest.easytesting.org/).
