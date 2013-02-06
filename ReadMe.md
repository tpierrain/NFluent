NFluent overview
==============

NFluent provides some helpers for Easy Software Testing in .NET. NFluent is highly inspired of the awesome FEST Fluent assertion/reflection Java library (http://fest.easytesting.org/)

- - -

Usage sample
------------

With NFluent, you can write some assertions like this (note: CollectionAssert here is part of the NUnit framework).


	var collection = new List<Student> {
                                     new Student { Name = "Thomas", Age = 38 }, 
                                     new Student { Name = "Achille", Age = 10, Nationality = Nationality.French }, 
                                     new Student { Name = "Anton", Age = 7, Nationality = Nationality.French }, 
                                     new Student { Name = "Arjun", Age = 7, Nationality = Nationality.Indian }
                                 };

	CollectionAssert.AreEqual(new[] { "Thomas", "Achille", "Anton", "Arjun" }, collection.Properties("Name"));
	CollectionAssert.AreEqual(new[] { 38, 10, 7, 7 }, collection.Properties("Age"));
	CollectionAssert.AreEqual(new[] { Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian }, collection.Properties("Nationality"));

- - -

Author(s)
--------- 
+ [Thomas PIERRAIN](mailto:thomas@pierrain.net)