NFluent backlog
===============

Now available on github: https://github.com/tpierrain/NFluent/issues?state=open

- - -

Temporary backlog
-------
1. Review the implementation of the Duration class (makes it a monoid)
1. Add IsSameSizeAs() check (IEnumerable->string)
1. Refactor the AssessEqual method (StringCheckExtensions)
1. Refactors/improves the msbuild script
1. Automate the release note format review (to replace every < by &lt; and so forth)
1. Replaces the "The expected value(s)" by "the expected <what is at stake here>" (e.g. LambdaCheckException)
1. Adds few methods to the char: IsUpperCase(), IsLowerCase(), IsWhiteSpace(),IsLetterOrDigit()
1. Refactors the T4 script (extract method) for the NFluent project.
1. Adds more unit tests to the FluentMessage so that it will act as a documentation.
1. Process all the //TODO instruction comments.


- - - 

Notes:
------
About NCrunch integration issues:
 - http://forum.ncrunch.net/yaf_postst935_Issue-when-using-conditional-compilation-symbols-in-linked-file.aspx

About Async/await:
 - http://stackoverflow.com/questions/2796928/making-an-extension-method-asynchronous
