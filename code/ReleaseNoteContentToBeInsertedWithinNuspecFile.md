#V 3.0.0.1
## Fix
* Fix `Check.ThatCode` not awaiting `Task` returning lambdas in V3.0.0. Note that `Task<T>` returning lambdas do work in V3.0.0
* an InvalidOperation is thrown when using `Check.ThatCode` on an async void method/lambda (as those cannot be awaited)


# V 3.0.0
## Major changes
* You can execute multiple check as a single batch and get every failures, instead of the first one. This can be achieved using:
	* `Check.StartBatch`: stores the result of each subsequent check(s) and notifies all the errors when the returned object is disposed. Such as
		`using(Check.StartBatch())
		{
			Check.That(sut).....
			Check.That(sut)....
		}`
		Note that any actual exception thrown during the check phase will prevent any subsequent check from behind executed (1) and may not be reported as it may be replaced by an assertion failure exception.
		This comes from C# exeption handling logic.
* You can provide anonymous types and tuples when using IsEqualTo against any type. The check will be made against all
_sut_'s propertie.
* NFluent supports Net 3.5 SP1, Net. 4.5.2 +, Net Standard 2.0+. Dropped support for Net Framework 2.0, 3.0, and 4.0, as well Net Standard<2.0. 
 If you can't upgrade your framework version to a supported one, please use NFluent 2.7.1.


## New Checks
* `Is`: Checks if _sut == expected_. This is a strongly typed equivalent to `IsEqualTo`.
* `IsGreaterOrEqualThan`: Checks if _sut_ >= _expected_. 
* `IsLessOrEqualThan`: Checks if _sut_ <= _expected_. 

## New feautres
* You can provide custom comparer for any type, using `Check.RegisterComparer` like this `Check.Register<MyType>(MyCustomComparer)`. 
You can also use `RegisterLocalComparer` to limit its usage to a declaration scope.

## Breaking changes
* Equality logic changed for `IDictionary`: dictionaries are considered equals if they have the same keys and
the same values for each key. In NFluent V2, entries needed to be declared in the some order or else they were considered as **different** but **equivalent**.
* You need to specify
* `IsAnInstanceOf<nullableType>(null)` now fails (with an appropriate message). Previously, it did succeed. But,
as captured in issue #68, this behavior was triggered by a bug and kept due to a poor error message when fixed.
* The `IStructCheck<T>` interface has been removed as well as associated extensibility helper. Those were dedicated
to value `types`, you can migrate your existing extensions to the `ICheck<T>` type instead. Please open an issue if
you need help.

## Fixes
* HasFieldWithSameValues resulted in false positive when string fields had the same value.
* IsNotEqualTo now properly preserves expected type
* Improved rerporting of differences for enumerations and dictionaries to make them more consistent and fixed some inaccuracies.

## GitHub Issues
* #325, #327, #330, #332

### Obsolete
#### Marked as obsolet
* `ThatAsyncCode`: you can now use `ThatCode` even for async methods.

Here is the list of methods, classes and other obsolete stuff that have been removed in this version as well
as workaround advices.
* Drop support for Net 2.0 and 3.0: keep using NFluent V2.x versions if you support for these.
* `Check.ThatEnum`has been removed. You must use `Check.That` instead.
* `ILambdaCheck`: the definition was kept to prevent breaking build, but it was no longer used. If this is a
problem for you, open an issue
* `IsPositive` (available for numbers): please use `IsStrictlyPositive` instead.
* `IsNegative` (available for numbers): please use `IsStrictlyNegative` instead.
* `IsLessThan` (available for numbers): please use `IsStrictlyNegative` instead.
* `IsGreaterThan` (available for numbers): please use `IsStrictlyGreaterThan` instead.
* `IsSameReferenceThan`: please use `IsSameReferenceAs` instead.
* `HasFieldsEqualToThose`: please use `HasFieldsWithSameValues` instead.
* `HasFieldsNotEqualToThose`: please use `HasNotFieldsWithSameValues` instead.
* `IsAFaillingCheckWithMessage`: please use `IsAFailingCheckWithMessage` instead.
* `IsAFaillingCheck`: please use `IsAFailingCheck` instead.
* `Properties` (available for enumeration): please use `Extracting` instead.
* `Checker.BuildLinkWhich` (used for custom extension): please use `ExtensibilityHelper.BuildCheckLinkWhich` instead.
* `Checker.ExecuteCheckAndProvideSubItem` (used for custom extension): please 'ExtensibilityHelper' static class methods instead.

