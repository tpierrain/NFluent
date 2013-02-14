NFluent Definition of Done (DoD) for Development
==================================

These are the technical rules that should be followed in order to contribute to this project:

1. No StyleCop warning
2. 100% of test coverage for the NFluent project
3. With (of course) all unit tests passed 

- - - 

On the other hand, it is very important to keep in mind that __this library is designed to produce fluent assertions at the end!__ 

Thus, it means that:
+ __the name__ of the extension methods __should be chosen carefully__
+ every extension method that don't succeed __should throw a FluentAssertionException__ to make your favorite unit test framwork Assert.That statement fails.


