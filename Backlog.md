NFluent backlog
===============

Now available on github: https://github.com/tpierrain/NFluent/issues?state=open

- - -

Temporary backlog
-------
1. Review the format of the DateTime checks to apply the same pattern?
1. Replace all the 'The checked value' by 'The checked string' for tests with strings within EqualRelatedTests.cs and StringRelatedTests.cs files.
1. Replace all the 'The actual enumerable' by 'The checked enumerable'

1. Make some FluentMessage able to write "The given value(s)" instead of "the expected enumerable"?
1. Make the NumberCheck not implementing the ICheck anymore.
1. Add some check method to the comparable types (e.g. IsAfter()).
1. Expose IComparable extension methods to 'number' values so that autocompletion works on number values with those comparable checks.

Done item
-------.
1. Reached 100% coverage on NFluent.dll
1. Improved formatting of type's names
1. Improved signature generation and Documentation related tests
1. Added HasFieldsEqualToThose to compare two objects at the field level