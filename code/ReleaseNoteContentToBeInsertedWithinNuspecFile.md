# V 3.0.0
## Major changes
* You can provide anonymous types and tuples when using IsEqualTo against any type. The check will be made against all
_sut_'s propertie.
* Dropped support for Net Framework 2.0, 3.0, and 4.0. NFluent supports Net 3.5 SP1, Net. 4.5.2 +, Net Standard 1.3+ and Net Standard 2.0+.
 If you can't upgrade your framework version to a supported one, please use NFluent 2.7. 
* `Check.ThatEnum`has been removed. You must use `Check.That` instead.
# V 2.7.1
# Fixes
* HasFieldsWithSameValues failed to properly compare when the expected value contained duplicate string. 
More generally, instances where only checked once for equality; any subsequent check was assumed to be succesful. 
This could lead to false positive (i.e. checks succeeded when it should have failed). 
This regression was introduced by V 2.2.0 in 02/2018. Sorry about that.
  
# GitHub Issues
* #331



## Breaking changes
* Equality logic changed for `IDictionary`: dictionaries are considered equals if they have the same keys and
the same values for each key. In NFluent V2, they were considered as **different** but **equivalent**.
* `IsAnInstanceOf<nullableType>(null)` now fails (with an appropriate message). Previously, it did succeed. But,
as captured in issue #68, this behavior was triggered by a bug and kept due to a poor error message when fixed.
* The `IStructCheck<T>` interface has been removed as well as associated extensibility helper. Those were dedicated
to value `types`, you can migrate your existing extensions to the `ICheck<T>` type instead. Please open an issue if
you need help.

## Fixes
* HasFieldWithSameValues resulted in false positive when string fields had the same value.

* Closed issues: #325, #331

### Obsolete
Here is the list of methods, classes and other obsolete stuff that have been removed in this version as well
as workaround advices.
* Drop support for Net 2.0 and 3.0: keep using NFluent V2.x versions
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