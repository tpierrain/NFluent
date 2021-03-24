# V 2.7.2

## Fixes
* HasFieldWithSameValues resulted in false positive when string fields had the same value.
* IsZero failed for very small double (<1E-28) in previous versions.

# GitHub Issues
* #331, #333

# V 2.7.1
# Fixes
* HasFieldsWithSameValues failed to properly compare when the expected value contained duplicate string. 
More generally, instances where only checked once for equality; any subsequent check was assumed to be succesful. 
This could lead to false positive (i.e. checks succeeded when it should have failed). 
This regression was introduced by V 2.2.0 in 02/2018. Sorry about that.
  
# GitHub Issues
* #331


# V 2.7.0
# New checks
* You can use IsCloseTo on DateTime and DateTimeOffset to check if a given date is close to a reference one.
* You can provide your own equality comparer (as an implementation of IEqualityComparer) when using IsEqualTo.

      Check.That(sut).IsEqualTo(expected, new MyEqualityComparer());

# Improvements
* Check.That(IEnumerable).IsEquivalent(...) now uses default logic for equality check.
* Significantly improved error messages for enumeration and dictionary equality comparison.
* Restore typed IsEqualTo check. It should ensure smoother experience with autocompletion logic. Non typed version
(using Object as a parameter) is still available.
* You can use WhichMember to perform checks on any member of an exception.
       
      Check.ThatCode(() => {...}).Throws<ArgumentException>().
        WhichMember( x=> x.ParamName).IsEqualTo(myArg);

# Fixes
* the Not operator no longer erases the custom message set using WithCustomMessage
* Check.That(IDictionary).IsEquivalent now fails as expected when the sut has entries that do not exist in the expected dictionary.
* IsEquivalent now performs deep equivalence. For example, it supports Dictionaries of Dictionaries
* NFluent now mimics Net implicit type conversion for numeric types so that IsEqualTo behaves as expected when implicit conversion required
* Enum properties are properly considered when using Considering.
* Enumeration of KeyValue pairs are no longer treated as dictionaries but as enumeration. This behavior was a hack
to support custom IDictionary<K,V> implementations. Detection logic has been improved so this is no longer necessary.
* Check.That(IEnumerable).IsInDescendingOrder no longer requires items to implement IComparable

# GitHub Issues
* #306, #312, #313, #314, #315, #317, #319, #320, #321

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

