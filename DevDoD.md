NFluent Definition of Done (DoD) for Development
==================================

These are the rules that should be followed in order to contribute to this project:

1. No StyleCop warning 
	+ this includes to have a proper documentation for the NFluent project (but not for the unit test one).
2. 100% of test coverage for the NFluent project
	+ Test names should be clear enough to know what is in stakes here (this is why I disable the "ElementsMustBeDocumented" StyleCop rule for tests). 
3. With (of course) all unit tests passed ;-)
4. The entire build (i.e. including all the unit tests execution) takes less than 2 minutes.


Original intentions
-------------------

Before modifying this library, it is very important to keep in mind that __this library is designed to produce fluent assertions at the end!__

Thus, it means that:
+ __names__ of the extension methods __should be chosen carefully__
+ every __extension method that don't succeed should throw a FluentAssertionException__ to make your favorite unit test framwork Assert.That() statement fails __with a clear status message__.

Also, and to stay coherent with the equivalent **FEST fluent assert** Java library (interesting for people which are coding in those 2 platforms):
+ Thus, before introducing a new extension method, check the existence of a method name for the same thing within the java library (http://fest.easytesting.org/).
