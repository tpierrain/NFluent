## V 2.5.1

### Improvements
* Actual and expected value naming has been redesigned to improve naming accuracy. Impact vary depending on checks and types.

### Fixes
* Fix issue with IEnumerable<pbject> and Contains(Exactly), IsEqualTo, IsEquivalentTo.

### Extensibility
* ICheckLogic.DefineExpectedValues now expects an generic IEnumerable<T> instead of a plain IEnumerable
* you can use ICheckLogic.DefinePossibleTypes if you need to have a list of possible types for the sut (displayed in the error message)
* improved naming: ICheckLogic.DefineExpectedValues has been renamed DefinePossibleValues

### GitHub Issues
* #292

## V 2.5.0
### Main feature
* **CaptureConsole class** mocks the system console. Using it you can inject/simulate
user input (with **Input** and **InputLine** methods) and read/review what the code has put on the
console (with the **Output** property). The class is disposable: normal behavior
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
## V 2.4.0
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
* Truncation default lenght for message is now 20Ko as an experiment. Please bring feedback. You can still adjust 
* default truncation with the Check.StringTruncationLength property
* Multidimensional arrays are properly reported in error messages, respecting index structure.
* Sourcelink (Net Core 2.1+ and Net Standard 2.0): you can debug through NFluent code using Sourlink on Core 2.1 projects
* Multi dimensional array types are reported with the number of dimensions (eg: int[,,])

### Fixes
* As now works with Not (and vice versa).
* Exception when using HasElementThatMatches or ContainsOnlyElementsThatMatch on arrays, and possibly
other enumerable types.
* Exception when using multidimensional arrays (such as  int[2,5]) with Considering/HasFiedsWithSameValueAs.
* false Negative when comparing multimensionnal arrays, e.g.: int[3,5] was equal to int[5,3] and with int[15].
* Exception when reporting strings containing braces.

### GitHub Issues
* #255, #38, #166, #258, #259, #260, #261, #262, #264, #265


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
