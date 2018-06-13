## V 2.3.2
---------------
### Fixes

### GitHub Issues


## V 2.3.1
---------------
### Fixes
* NullReferenceException on failed check using xUnit and NetCore

### GitHub Issues
* #251
------
## V 2.3.0
---------------
### Main feature: redesigned extensibility
One of the fundamental features of NFluent is that you can add your own checks.
Articles explained how to do that, but syntax was still too cumbersome 
for our taste. This version brings major improvements detailed here:

* Simplified support for creating custom checks thanks to new helper methods
and classes (see https://github.com/tpierrain/NFluent/wiki/Extensibility)
* Customization of error reporting: by default, any check failure is reported
by raising an exception. You can now provide your own reporting system. You need to provide an implementation
of IErrorReporter interface, and specify you want to use it by setting the Check.Reporter interface.


### Other New features(s)
* IsNullOrWhiteSpace: checks if a string is null, empty or contains only white space(s).
* IReadOnlyDictionary (_Net 4.5+_)
  * ContainsKey, ContainsValue, ContainsPair are supported.
* async method/delegates
  * Check.ThatCode now supports _async_ methods/delegates transparently.
* Check expression now provides the result as a string. I.e Check.That(true).IsTrue().ToString() returns "Success".
* New check: IsDefaultValue, which fails if the sut is not the default value for its type: null for ref types, 0 for value types.
* New check: ContainsNoDuplicateItem for enumerable, that fails of it contains a dupe.
* New check: IsEquivalentTo for enumerable, that checks if its contents match an expected content, disregarding order.
* New check: DoesNotContainNull for enumerable, that fails if an entry is null.
* New check: IsAnInstanceOfOneOf that checks if the sut is of one of exptected types.
* New check: IsNotAnInstanceOfThese that checks if the sut type is different from a list of forbidden types.
* New check: DueToAnyFrom(...) that checks that an exception has been triggered by another exception from a list of possible types.

### Fixes
 * Check.ThatCode(...).Not.Throws\<T\>() may throw an InvalidCastException when thrown exception is not T.
 * Extension checks to Throw\<\>, ThrowType or ThrowAny raise an exception when used with Not as it does not make sense.
 * Which() raises an exception when used on a negated check (Not).
 * Fix exception when using Considering and indexed properties.
 * Fix loss of type when using Contains and ContainsExactly. This restores fluentness for IEnumerable<T> types.
 Fixed error messages for double (and float) equality check that reported checked value in place of the expected one.
 * Fixed error messages for Check.That(TimeSpan).IsGreaterThan
 * False positive whith Considering() or HasFieldsWithSameValues when haing ints and enum attributes with the same value.

### Changes
* Improved error messages
  * ContainsOnlyElementsThatMatch: now provides the index and value of the first failing value.
  * IsOnlyMadeOf: improved error messages
  * DateTime checks: revamped all messages
  * Enum: error message on enum types now use 'enum' instead of 'value'.
  * IsInstanceOf: be more specific regarding types
  * Considering()...IsNull/IsNotNull: error messages specify member triggering the failure.
* Breaking
  * Added automatic conversion between decimal and other numerical types. Check.That(100M).IsEqualTo(100) no longer fails.
  * Removed Failure method from IChecker interface
 
### GitHub Issues
* #228, #227, #222, #223, #217, #230, #232, 
* #236, #238, #242, #243, #244, #245, #246,
* #231, #247, #161, #249
------
