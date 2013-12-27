NFluent backlog
===============

Now available on github: https://github.com/tpierrain/NFluent/issues?state=open

- - -

Temporary backlog
-------
1. Split the AssemblyVersion.cs attributes in multiple files, so that we can identified the differences between output from NFluent.35 and NFluent.40
1. Find why NFluent.40 coverage is 0%
1. Find a solution to disable NCrunch for double testing, but enabling this double testing on the SF
1. Find a better way to handle the dependencies towards NFluent.Tests.Extensions (another linked project?)


1. Replaces the "The expected value(s)" by "the expected <what is in stake here>" (e.g. LambdaCheckException)
1. Adds few methods to the char: IsUpperCase(), IsLowerCase(), IsWhiteSpace(),IsLetterOrDigit()
1. Refactors the T4 script (extract method) for the NFluent project.
1. Adds more unit tests to the FluentMessage so that it will act as a documentation.
1. Process all the //TODO instruction comments.