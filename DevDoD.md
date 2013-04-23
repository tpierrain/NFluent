NFluent Definition of Done (DoD) for Development
==================================

These are the rules that should be followed in order to contribute to this project:

1. Apache 2.0 License header is set on every source code file (but not mandatory for unit test files).
1. No warning during the build (warn as error)
1. No StyleCop warning
	+ this includes to have a proper documentation for the NFluent project (but not for the unit test one)
1. 100% of test coverage for the NFluent project
	+ Test names should be clear enough to know what is in stakes here (this is why I disable the "ElementsMustBeDocumented" StyleCop rule for tests)
1. With (of course) all unit tests passed ;-)
1. The entire build (i.e. including all the unit tests execution) takes less than a minute


And there are rules and advices too on __[how to extend NFluent with your own methods](./HowToAddANewAssertion.md)__.