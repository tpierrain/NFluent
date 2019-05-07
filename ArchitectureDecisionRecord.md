
# Architecture Decision Record
This document logs all important decision with their rationale and context.
Its objective is to provide information to maintainers and contributors so
they can respect those decisions or revise them if the context has changed.

Entry appers in inverse chronological order, this document has been initiated
on May 7th, 2019. Any decisions predating this document will refer to this date.

Entries starting with a question mark are temporary and must enventualy be reverted
when conditions permit

-------------
#### 

#### ? Double checks in test (2019/05/07)
There is a regression in OpenCover V4.7 regarding identification of some runtime
optimizations introduced by the C# compiler. It concerns anonymous lambda caching
and lead to false partial coverage detection. This happens when passing a lambda as
a parameter to a method call without any explicit flow control
(if, while, for, switch...).
This can be worked around by adding an extra test, but first, you need to make sure
this line is only tested once. Otherwise, you may have a real (branch) coverage gap
or this may be a different issue with opencover.
Please comment the extra check you add for future removal.

This decision will be reverted when Opencover is fixed.


#### Multi-targeting project organization (2019/05/07):
Priority is to maximize features consistency accross Net platforms and to simplify
maintenance
1. All source files are listed in a single shared project that is refered by
target-specific projects.
1. As source files are shared accross targets, part of the code will have to be guarded
by **#if** directives.
1. The project only refers to officialy defined symbols, DOTNET_xxx and NETSTANDARD_xx

#### Development and testing practices (2019/05/07)
NFluent is part of the testing strategy of many projects, therefore, NFluent must
achieve the highest possible quality.
1. **TDD** is to be prefered when working on the project
1. Each check must have corresponding unit tests that exert it in all supported
scenario
1. Each check error messages must be tested. You can use **IsAFailingCheckWithMessage()**
to implement these tests

#### Test coverage objectives (2019/05/07)
On top of rationale presented in development and testing practices, 
there have been a significant investment in test coverage that is valuable to maintain.
It has allowed multiple merciless major refactoring without any glitch in the past.
1. **100% line coverage is mandatory** for each release, except for methods marked as obsolete
1. **100% branch coverage is mandatory in the long run**, but temporary drops are ok. 99 % is the strict minimum 
1. **Mutation testing** is strongly suggested, but only when the two precedent objectives are met