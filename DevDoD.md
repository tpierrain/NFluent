NFluent Definition of Done (DoD) for Development
==================================

These are the technical rules that should be followed in order to contribute to this project:

1. No StyleCop warning
2. 100% of test coverage for the NFluent project
3. With (of course) all unit tests passed 

On the other hand, it is very important to keep in mind that this library is designed to produce fluent assertions at the end. 

It means that:
+ the name of the extension methods should be chosen carefully.
+ every extension method that don't succeed should throw a FluentAssertionException to make your favorite unit test framwork Assert.That statement fails.


