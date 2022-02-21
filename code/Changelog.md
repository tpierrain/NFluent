## V 2.7.1
### Fixes
* HasFieldsWithSameValues failed to properly compare when the expected value contained duplicate string. 
More generally, instances where only checked once for equality; any subsequent check was assumed to be succesful. 
This could lead to false positive (i.e. checks succeeded when it should have failed). 
This regression was introduced by V 2.2.0 in 02/2018. Sorry about that.

### GitHub Issues
* #331

## V 2.7.0
### New checks
* You can use *IsCloseTo* on **DateTime** and **DateTimeOffset** to check if a given date is close to a reference one.
* You can provide your own equality comparer (as an implementation of *IEqualityComparer*) when using *IsEqualTo*.

      Check.That(sut).IsEqualTo(expected, new MyEqualityComparer());

### Improvements
* *Check.That(IEnumerable).IsEquivalent(...)* now uses default logic for equality check.
* Significantly improved error messages for enumeration and dictionary equality comparison.
* Restore **typed** *IsEqualTo* check. It should ensure smoother experience with autocompletion logic. Non typed version
(using Object as a parameter) is still available.
* You can use **WhichMember** to perform checks on any member of an exception.
       
      Check.ThatCode(() => {...}).Throws<ArgumentException>().
        WhichMember( x=> x.ParamName).IsEqualTo(myArg);

### Fixes
* the Not operator no longer erases the custom message set using WithCustomMessage
* Check.That(IDictionary).IsEquivalent now fails as expected when the _sut_ has entries that do not exist in the expected dictionary.
* IsEquivalent now performs deep equivalence. For example, it supports Dictionaries of Dictionaries
* Enum properties are properly considered when using *Considering*.
* Enumeration of *KeyValue* pairs are no longer treated as dictionaries but as enumeration. This behavior was a hack
to support custom *IDictionary<K,V>* implementations. Detection logic has been improved so this is no longer necessary.
* *Check.That(IEnumerable).IsInDescendingOrder* no longer requires items to implement IComparable

### GitHub Issues
* #306, #312, #313, #314, #315, #317
__________________
## V 2.6.0

### New feature
* NFluent now supports assumption through Assuming entry point. For example you express it as :Assume.That(sut).IsEqualTo(expected); in a nutshell
you type Assuming instead of Check. All checks are available. Note that actual support depends on the underlying testing framework. As of now
it is supported for NUnit and MsTest
* NFluent now supports DateTimeOffset type with the same checks than for DateTime. These checks fails
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
* Fix issue with `IEnumerable<object>` and Contains(Exactly), IsEqualTo, IsEquivalentTo.
* Several error messages have been improved due to fix on check helpers.
* NotSupportedException when using ContainsExactly on strings.
* Fix issue with single dimension arrays and field based checks where the LAST item of the array
* was not evaluated during the check (issue found thanks to mutation test)
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

## V 2.5.0
### Main feature
* **CaptureConsole class** mocks the system console. Using it you can inject/simulate
user input (with **Input** and **InputLine** methods) and read/review what the code has put on the
console (with the **Output** property). The class is disposable: nnormal behavior
is restored on Dispose.
Example

      using (var console = new CaptureConsole)
      {
          console.InputLine("12+13");
          // the code I need to check (a calculator here)
          Calculator.Process();
          // perform the check
          Check.That(console.Output).IsEqualTo("25");
       }
 
### New checks
* Console related checks (see above)

### Improvements
* Stabilize Assembly Version to reduce friction induced by strong naming (assembly version is still V2.4.0)
* Align to Microsoft guidelines for OSS libraries (https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/)
* HasAvalue() and HasNoValue() are available on all nullable types
* Add support for WithCustomMessage for dynamics.
* Revised signature for enumerable checks to reduce type erasure (loss of type information when chaining checks). Regression tests have been added regarding non generic IEnumerable support, but as the changes
are significant, please revert to us if you face issues.
* Fixes reporting of end of line markers: only carriage return chars were reported.
* Changed error text for missing or extra lines in string to make it clearer.
* IsEqualTo provides more details for IEnumerable (make sure first different item is visible).
* Number of items in expected value was often not reported in error messages.

### Fixes
* Fix false positive with TimeSpan due linked to precision loss. It concerns: IsEqualTo(TimeSpan), IsLessThan(TimeSpan), IsGreaterThan(TimeSpan)
* Fix random FileNotFound exceptions on the first failing assertion while using XUnit in some specific setup.
* Error message for Check.ThatCode().LastsLessThan did not report the actual time.
* Error message for Not.ThrowsAny() was wrong.
* Hashtable not properly reported in error messages.
* Fixed error messages for negated checks on dynamics.
* Fix false positive for IsNotZero (and IsZero) for decimal that are close to 0 (<.5).
* IsEquivalentTo now supports dictionary types.
* IsEqualTo now supports dictionary types. Error message hints to use IsEquivalentTo when relevant.

### GitHub Issues
* #269, #274, #270, #275, #276, #280, #283, #184, #284, #286, #290
---------------
# V 2.4.0
### Main feature: Custom explicit error message
You can now provides explicit error messages for each check, thanks to **WithCustomMessage**. E.g:
Check.WithCustomMessage("Ticket must be valid at this stage").That(ticket.Status).IsEqualTo(Status.Valid);
This feature has often been requested  and we are happy to finaly deliver it, but please keep on
naming your test methods properly.
Custom error messages are not avaible for dynamic types.

### New checks
* IsInAscendingOrder: checks if an IEnumerable is sorted in ascending orders, it accepts an optional comparer instance
* IsInDescendingOrder:  checks if an IEnumerable is sorted in descending orders, it accepts an optional comparer instance
* IsSubSetOf: checks if an IEnumerable is a subset of another collection.
* IsInstanceOf<Type>(): now supports the Which() keyword so you may use checks specific to the asserted type.

### Improvements
* Truncation default length for message is now 20Ko as an experiment. Please bring feedback. You can still adjust 
* default truncation with the Check.StringTruncationLength property
* Multidimensional arrays are properly reported in error messages, respecting index structure.
* Sourcelink (Net Core 2.1+ and Net Standard 2.0): you can debug through NFluent code using Sourcelink on Core 2.1 projects

### Fixes
* As now works with Not (and vice versa).
* Exception when using HasElementThatMatches or ContainsOnlyElementsThatMatch on arrays, and possibly
other enumerable types.
* Exception when using multidimensional arrays (such as  int[2,5]) with Considering/HasFiedsWithSameValueAs.
* false Negative when comparing multidimensionnal arrays, e.g.: int[3,5] was equal to int[5,3] and with int[15].
* Exception when reporting strings containing braces.
### GitHub Issues
* #255, #38, #166, #258, #259, #260, #261, #262, #264, #265
---------------
## V 2.3.1
### Fixes
* NullReferenceException on failed check using xUnit and NetCore
### GitHub Issues
* #251
---------------
## V 2.3.0
### Main feature: redesigned extensibility
One of the fundamental features of NFluent is that you can add your own checks.
Articles explained how to do that, but syntax was still too cumbersome 
for our taste. This version brings major improvements detailed here:

* Simplified support for creating custom checks thanks to new helper methods
and classes (see https://github.com/tpierrain/NFluent/wiki/Extensibility)
* Customisation of error reporting: by default, any check failure is reported
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
* New check: ContainsNoDuplicateItem for enumerable, that fails if it contains a dupe.
* New check: IsEquivalentTo for enumerable, that checks if its contents match an expected content, disregarding order.
* New check: DoesNotContainNull for enumerable, that fails if an entry is null.
* New check: IsAnInstanceOfOneOf that checks if the sut is of one of expected types.
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
---------------
# V 2.2.0
Flexible property and field based comparison is now available. Examples:
* Check.That(sut)**.Considering().Public.Properties**.IsEqualTo(expected);
* Check.That(sut)**.Considering().Public.Fields.And.Public.Properties**.IsEqualTo(expected);
* Check.That(sut)**.Considering().Public.Fields.Excluding("coordX", "child.address")**.IsEqualTo(expected);
Syntax is:
* Check.That(sut)**.Considering().(Public|NonPublic|All).(Fields.Properties)[.And.(Public|NonPublic|All).(Fields.Properties)][Excluding("fielda", "sub.fieldb")]**.IsEqualTo(expected);
* **Considering()** is supported by: _IsEqualTo(), IsNotEqualTo(), AsSameValueAs(), HasDifferentValueThan(), IsInstanceOf\<type\>()_ [checks if fields/properties are present],
_IsNotInstanceOf\<type\>()_,  _HasSameValueAs()_, _IsSameReferenceAs(), _IsDistinctFrom()_, _HasDifferentValueThan()_

### New feature(s)
* **Object**
  * New check **IsInstanceOfType(Type type)** which is equivalent to *IsInstanceOf\<type\>()*, in a non generic form for parameteriSed tests.
  * New check **IsNoInstanceOfType(Type type)** which is equivalent to *IsNotInstanceOf\<type\>()*, in a non generic form for parameteriSed tests.
  * New check **InheritsFromType(Type type)** which is equivalent to *InheritsFrom\<type\>()*, in a non generic form for parameterised tests.
* **Enum**
  - New check **HasFlag(xxx)** that checks if a flag is present in an enum value.
* **Code**
  * New check **ThrowsType(Type type)** which is equivalent to *Throws\<type\>*, in a non generic form for parameterised tests.

### Changes
* Improved error messages for missing fields(and properties) for reflection based checks.

### Fixes
* Fix issue with overloaded member/properties for HasFieldswithSameValues(...) (#219)
 
### GitHub Issues
* #219, #218, #216, #215, #214, #121
---------------
# V 2.1.1
Bug fix(es):
* fix issue #215: null reference exception triggered by HasFields... on null interface fields.
--------------
# V 2.1.0
New feature(s):
* Enumerable:
  * Breaking change: equality for Enumerable is now content based (instead of reference based). See issue #209. You can revert adding: Check.EqualMode = EqualityMode.Equals in your test set up code
  * New check ContainsOnlyElementsThatMatch which checks that all items verify a given predicate.
* New Check: IsOneOf checks if the value belongs to a list of authorised value. Check.That(sut).IsOneOf(a, b, c, ...) fails if _sut_ is different from all valid values
* Improved error messages for IsEqualTo() on floating point types. Difference is displayed when small, suggestion to use IsCloseTo when difference is really small (#205).
* Initial support for dynamics (issue #85):
  * Entry point is: Check.ThatDynamic()
  * Available checks are: IsNotNull(), IsEqualTo(...), IsSameReferenceAs(...)
  * And keyword is available
  * Not keyword is available
* You can specify the maximum size (before truncate) for strings in error messages using **Check.StringTruncationLength** property
Bug Fixe(s):
* Fixed support of array types for HasFielsWithSameValues (issue #200)
* Fixed some stack overflow exceptions (issue #210).
* Fixed confusing examples about array item related checks in the readme
--------------
# V 2.00
New feature(s):
* Support for NetStandard > 1.3
* Support for Net.Core > 1.0
* Compatible for Net > 2.0
* Built for: 2.0, 3.0, 3.5, 4.0, 4.5, NetStandard 1.3
* All: introduce: HasSameValueAs(x) that perform comparison using 'operator==' instead of 'Equals'.
* All: introduce: HasAValueDifferentFrom(x) that perform comparison using 'operator!=' instead of '!Equals'.
* Streams: introduce HasSameSequenceOfBytesAs() check.
* Numbers: introduce IsPositiveOrZero() check.
* Numbers: introduce IsNegativeOrZero() check.
* FloatingNumbers (float, double): introduce IsCloseTo(expected, within) check for estimated value.
* Exception: introduce the DueTo<InnerException> as an extension to Throws<Exception>. It verifies that the checked exception was triggered by a specific exception. This is done by scanning the 'innerException' chain until the expected type is identified. Further checking is done on the inner exception.
* Exception: introduce WithProperties( expression, expectedValue) to check the value of any exception members, thanks to a selector expression.
* IEnumerable: introduce HasElementAt(int index) which checks for an item as a specific index. Furthermore, this item can be checked as well thanks to 'Which'. E.g: Check.That(_collection).HasElementAt(3).Which.IsEqualTo("foo").
* IEnumerable: introduce HasFirstElement() which checks for the first item. Furthermore, this item can be checked as well thanks to 'Which'. E.g: Check.That(_collection).HasFirstElement().Which.IsEqualTo("foo").
* IEnumerable: introduce HasLastElement() which checks for the last item. Furthermore, this item can be checked as well thanks to 'Which'. E.g: Check.That(_collection).HasLastElement().Which.IsEqualTo("foo").
* IEnumerable: introduce HasOneElementOnly() which checks for the first and single item. Furthermore, this item can be checked as well thanks to 'Which'. E.g: Check.That(_collection).HasOneElementOnly().Which.IsEqualTo("foo").
* IEnumerable: introduce HasElementThatMatches( bool predicate()) which checks that an enumerable has at least on item matching the given predicate. You can use 'Which' to check further this item. E.g.: Check.That(_collection).HasItemThatMatches((_) => _.StartWith("foo")).Which.IsEqualTo("foobar").
* IDictionary<K,V>: now explicitly supported.
* IDictionary<K,V>: introduce ContainsEntry<K, V>(K key, V value) which checks that a dictionary contains a specific value for a given key.
* Hashtable: now explicitly supported.

Change(s):
* IEnumerable: improved description within error messages: partial dump around first difference for large sets
* IDictionnary: no longer supported. Hashtable is supported instead
* Numbers: introduce IsStrictlyPositive() as a substitute for IsPositive() which is now obsolete.
* Numbers: introduce IsStrictlyNegative() as a substitute for IsNegative() which is now obsolete.
* Numbers: introduce IsStrictlyGreaterThan() as a substitute for IsGreaterThan() which is now obsolete.
* Numbers: introduce IsStrictlyLessThan() as a substitute for IsLessThan() which is now obsolete.
* Floating Numbers: introduce IsCloseTo() to check if a number is close to another one.
* Check.ThatCode(...).Throws<T>() now requires T to be an exception. This restriction ensures only Exceptions are proposed in autocompletion.
* Improve error messages (consistency and relevance).
* Strings: generate specific error message using IsEqualTo() when the actual string is empty
* Strings: generate specific error message using IsEqualTo() when the expected string is empty
* Strings: provides part of string where first difference occurs even when strings have different lengths
* Objects: IsSameReferenceAs supersede IsSameReferenceThan (now flagged as obsolete)
* Objects: Dump hashcodes only when NFluent cannot highlight difference.
* Check.That(Action) can no longer be used (error, obsolete)
* Check.That(Func<T>) can no longer be used (error, obsolete)
* Simplify the way you can extend NFluent by adding your own checks. Now, you can call: var checker = ExtensibilityHelper.ExtractChecker(check);
  at the beginning of your check extension method, and then rely on its ExecuteCheck() or BuildMessage() helper methods to do the job. Note: the Checker is part of the NFluent.Extensibility namespace.
* Reviewing of public elements: Some classes are no longer public. They should never have been in the first case.
* Signature of ILambdaExceptionCheck has been updated to refer the checked exception type (instead of RunTrace). This is a breaking change for edge cases, that will impact your extension method on exceptions, if any; it can also impact your tests if you use the explicit signature.

Bug Fixe(s):
* Fix the inversion between expected and actual error message for the .WithMessage checks on exception.
* Fix the issue with null value on Check.That(...).IsBefore(...)
* HasFieldsWithSameValues now recurse along the hierarchy class (Fix for #141)
* Fix stack overflow triggered by HasFieldsWithSameValues() on object with reference loop (#148)
* Properly escaped strings when generating error messages.
* GitHub issues: #124, #133, #141, #148, #153, #154, #156, #159, #164, #165, #167, #174, #177, #178, #179, #187
--------------
# V 1.3.1
New feature(s):
* Now, supports .NET Tasks and Async methods/lambdas/functions thanks to a new Check.ThatAsyncCode()  statement.

Change(s):
* Error messages with double and float are now displayed with InvariantCulture.
* Improved error messages for HasFieldWithSameValue to deal when ToString gives the same text for actual and expected.
* Slightly changed error messages for exceptions to improve consistency.

Bug Fixe(s):
* There was an inversion between expected and actual error message for the .WithMessage checks on exception. This is fixed.
--------------
# V 1.2.0
New feature(s):
* Adds IsSetWithin check for EventWaitHandle types (ex. Manual or AutoResetEvent)
* Adds IsOneOfThese() check for string.
* NFluent is now also available as a Portable Class Library (Profile: portable-net45+sl5+netcore45+MonoAndroid1+MonoTouch1)
* Adds IsNaN() check for double and float.
* Adds IsFinite() check for double and float.

Change(s):
* Renames the Properties extension method on IEnumerable to Extracting. Properties is still there, but marked as obsolete.
* Improves error messages for IsInstanceOf checks.
* IsNull, IsNotNull, IsSameReferenceThan, IsDistinctFrom, HasFieldsWithSameValues and HasNotFieldsWithSameValues are now able to conserve the checked typed (instead of loosing it by converting it into a object).
* InheritsFrom does not return a linkable check anymore (but void).

Bug Fixe(s):
* Fixes a null ref exception for Contains on null
* Fixes a null ref exception for HasFieldsWithSameValues
* Fixes Invalid exceptions on strings with curly braces
--------------
# V 1.1.0
New feature(s):
* Now supports IsNull() and IsNotNull() checks for nullable (thanks to Mendel Monteiro Beckerman for that).
* Check.ThatCode(...): New entry point for checks on code (Action and Func<T>). It supersedes the equivalent Check.That signature.

Change(s):
* Improves error messages for string comparisons (e.g. visually indicates the presence of tab char with <<tab>>, or distinguish <<CFLF>> and <<LF>>, properly handles long strings, ...)
* Check.That(Func<T>) and Check.That(Action) are now obsolete.
* The HasFieldsEqualToThose() check is now obsolete and should be replaced by HasFieldsWithSameValues() which now support anonymous classes as expected parameter.
* The HasFieldsNotEqualToThose() check is now obsolete and should be replaced by HasNotFieldsWithSameValues()
* Simplification of extensibility
* We are so proud of this 1.1 version of NFluent; we decided to sign it.
--------------
# V 1.0.0
New feature(s):
* NFluent is now also shipped with its .NET 4.0 version (to support dynamic for extensibility)
* Adds recursion to the HasFieldsEqualToThose() check on object

Change(s):
* You can now easily link your own checks by using the And operator thanks to the IChecker.ReturnValueForLinkage property.

Bug Fixe(s):
* IsAfter() check was throwing an exception when the givenValue type (considered as a IComparable instead of a number)  was not the same as the checked value.
-------------
# V 0.11
New feature(s):
* Introduces a mechanism for anyone to easily extend NFluent with his own checks (see. ExtensibilityHelper)
* Adds recursion to the HasFieldsEqualToThose() check on object
--------------
# V 0.10
New feature(s):
* Adds IsNull() and IsNotNull() checks.

Change(s):
* Now, Check.That(4L).IsEqualTo(4) passes instead of throwing that types are different.
-------------
# V 0.9
New feature(s):
* Add the support of char
* Add the IsAfter() check to the IComparable instances
* Adds comparison checks to all the numeric types

Change(s):
* Hides the ForkInstance() method from the IntelliSense

Bug Fixe(s):
* Fixes a bad behaviour of the HasFieldsEqualThose() check against auto property null backing fields.
# V 0.8
New features:
* Now, the Equals() method of the ICheck&lt;T&gt; instances is a real fluent check method (instead of the default object.Equals() one).
* New extensible syntax for Check.That(IEnumerable xxx).Contains(...) that permits to add suffixes narrowing the conditions (e.g.: Check.That(IEnumerable xxx).Contains(...).InThatOrder()). Possible suffixes are (Only, Once or InThatOrder)
* Adds check methods for IDictionary
* Adds more check methods to the IEnumerable (e.g. IsNullOrEmpty)

Changes:
* New error message structure that clearly states what is wrong.
* Renames the ContainsOnly() method to IsOnlyMadeOf().
* Replaces IFluentAssertion by ICheck; and IChainableFluentAssertion by ICheckLink so that it improves the IntelliSense experience.
