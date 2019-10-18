## V 2.6.1
### Improvements
* Check.That(IEnumerable).IsEquivalent(...) now uses default logic for equality check.
* Significantly improved error messages for enumeration and dictionary equality comparison.

### Fixes
* Check.That(IDictionary).IsEquivalent now fails as expected when the _sut_ has entries that do not exist in the expected dictionary.
* IsEquivalent now performs deep equivalence. For example, it supports Dictionaries of Dictionaries.

### GitHub Issues
* #306

## V 2.6.0

### New feature
* NFluent now supports assumption through Assuming entry point. For example you express it as :Assume.That(sut).IsEqualTo(expected); in a nutshell
you type Assuming instead of Check. All checks are available. Note that actual support depends on the underlying testing framework. As of now
it is supported for NUnit and MsTest
* NFluent now supports DateTimeOffset type with the same gchecks than for DateTime. These checks fails
if the offsets are different. The IsSameUtcInstant cheks perform a comparison integrating the offset.

### New checks
* You can use WhoseSize() to check the size of an enumeration. It is used as an extension keyword, as in:
Check.That(enum).WhoseSize().IsEqualTo(3)

### Improvements
* When using the Equals method, NFluent now uses expected.Equals(actual) instead of actual.Equals(expected).
This should have limited impact.
* Actual and expected value naming has been redesigned to improve naming accuracy. Impact vary depending on checks and types.
* Comparison of enumeration now provides details regarding the differences. You can control
how many differences are reported using the property **ExtensionsCommonHelpers.CountOfLineOfDetails**.
* Cleaned up the reporting of array fields when using Considering. The superfluous dot (as in _field.[index]_)
has been removed.
* Improved implementation for Equals when using Considering. You should use IsEqualTo when checking for
* equality, but we also provide an implementation of Equals as a failsafe.

### Fixes
* Fix issue with IEnumerable<object> and Contains(Exactly), IsEqualTo, IsEquivalentTo.
* Several error messages have been improved due to fix on check helpers.
* NotSupportedException when using ContainsExactly on strings.
* Fix issue with single dimension arrays and field based checks where the LAST item of the array was not evaluated during the check (issue found thanks to mutation test)
* Comparing Array with considering was no different than when using IsEqualTo. This has been fixed.
Therefore error messages are now in line with what was expected

### Extensibility
Foreword: several breaking changes have been introduced that may trigger build error in your custom extensions if you have made any.
Methods and types have been renamed, so your code will have to refer the new names. IF YOU ENCOUNTER ISSUES AND NEED ASSISTANCE, please open an issue, we will assist you ASAP.
* All lambda/code specific interfaces (ICodeCheck<T>...) and classes have been removed. NFluent now uses the standard interfaces and types (i.e. Check<T>)
* ICheckLogic.DefineExpectedValues now expects an generic IEnumerable<T> instead of a plain IEnumerable
* you can use ICheckLogic.DefinePossibleTypes if you need to have a list of possible types for the sut (displayed in the error message)
* improved naming: ICheckLogic.DefineExpectedValues has been renamed DefinePossibleValues
* checks helper (ICheckLogic) now correctly reports the fundamental error instead of a detail error. In previous version, the error messages could focus on details, e.g. report the
exception's message when the issue is the exception's type.
* add a flag (boolean) to BuildCheckLinkWhich method (allows to provide subitem check) that allows to speciyf sub item is available.


### GitHub Issues
* #225, #291, #292, #295, #296, #297, #299, #302

