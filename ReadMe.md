NFluent overview
==============

NFluent provides some helpers for Easy Software Testing in .NET. NFluent is highly inspired of the awesome FEST Fluent assertion/reflection Java library (http://fest.easytesting.org/)

- - -

Usage sample
------------

With NFluent, you can write some assertions like this (note: CollectionAssert here is part of the NUnit framework).


	var initialCollection = new List<Student>()
                                 {
                                     new Student() { Name = "Thomas", Age = 38 },
                                     new Student() { Name = "Achille", Age = 10 },
                                     new Student() { Name = "Anton", Age = 7 }
                                 };

	CollectionAssert.AreEqual(new[] {"Thomas", "Achille", "Anton"}, initialCollection.Properties("Name"));

- - -

Author(s)
--------- 
+ [Thomas PIERRAIN](mailto:thomas@pierrain.net)