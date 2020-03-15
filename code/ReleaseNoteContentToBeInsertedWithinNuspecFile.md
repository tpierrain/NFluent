# V 3.0.0
## Major changes
* Dropped support for Net 2.0 and 3.5
* 

## Breaking changes
### Obsolete
Here is the list of methods, classes and other obsolete stuff that have been removed in this version as well
as workaround advices.
* Drop support for Net 2.0 and 3.0: keep using NFluent V2.x versions
* `ILambdaCheck`: the definition was kept to prevent breaking build, but it was no longer used. If this is a
problem for you, open an issue
* **IsPositive** (available for numbers): please use **IsStrictlyPositive** instead.
* **IsNegative** (available for numbers): please use **IsStrictlyNegative** instead.
* **IsLessThan** (available for numbers): please use **IsStrictlyNegative** instead.
* **IsGreaterThan** (available for numbers): please use **IsStrictlyGreaterThan** instead.
* **IsSameReferenceThan**: please use **IsSameReferenceAs** instead.
* **HasFieldsEqualToThose**: please use **HasFieldsWithSameValues** instead.
* **HasFieldsNotEqualToThose**: please use **HasNotFieldsWithSameValues** instead.
* **IsAFaillingCheckWithMessage**: please use **IsAFailingCheckWithMessage** instead.
* **IsAFaillingCheck**: please use **IsAFailingCheck** instead.
* **Properties** (available for enumeration): please use **Extracting** instead.