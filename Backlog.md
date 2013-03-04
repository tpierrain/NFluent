NFluent backlog
===============

+ Continue the library breakthrough (from an extension method model to a IFluentStuff one)
	+ migrate all methods from bool to void
	+ Make every FluentExceptionMessage a two liners
	+ Generate IEnumerable unit tests via T4 templates? (to test with all concrete collection type)
+ Fix the methods documentation (after the breakthrough)
+ Fix the StyleCop warnings
+ Decide which entry point to provie for the lib
	+ may be interesting to introduce t4 template to generate the various versions of this entry-point
+ git merge with main branch, and push
+ Publish v1.0 into nuget
+ Spread the word about the library to have feedback and first users