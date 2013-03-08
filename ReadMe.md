![NFluent](https://github.com/tpierrain/nfluent/blob/master/NFluentBanner.png?raw=true)

NFluent overview
==============

NFluent provides some extension methods for a __fluent .NET TDD experience__ based on simple Assert.That(bool condition) statements.

__NFluent will make your tests__:
+ __fluent to write__: with auto completion support. Indeed, just type the Assert.That( followed by one of your object and a dot, and your IDE will show you all the assertions available for the type of the given object to verify.
+ __fluent to read__: very close to plain English, making it easier for non-technical people to read test code.
+ __fluent to troubleshoot__: every failing extension method of the NFluent library throws an Exception with a crystal-clear message status to ease your TDD experience. Thus, no need to set a breakpoint and to debug in order to be able to figure out what went wrong. 
+ __helpful to reverse engineer legacy code__: indeed, temporarily write an on-purpose failing assert on a legacy method, so you can understand it and leverage on the "ready-to-be-copied-and-paste-for-arrays-or-collections-initialization-purpose" NFluent assert failure messages.
+ __less error-prone__: indeed, no more confusion about the order of the "expected" and "actual" values.

NFluent is __highly inspired by the awesome Java FEST Fluent__ assertion/reflection library (http://fest.easytesting.org/).

NFluent & unit test frameworks
-------------------------------
__NFluent is not coupled to any .NET unit test framework. It is fully designed to work in collaboration with your favorite one.__

Your favorite unit test framework (e.g. NUnit, xUnit, ...) will still handle the test identification, execution & Co, but you will simply now replace your usage of its Assert class by the NFluent assertions statements. That's all!

Uses cases
----------
__[NFluent use cases are available here](./UseCases.md)__.

Usage sample
------------

With NFluent, you can write simple assertions like this:
```c#	
    var integers = new int[] { 1, 2, 3, 4, 5, 666 };
    Assert.That(integers).Contains(3, 5, 666);

	var integers = new int[] { 1, 2, 3 };
    Assert.That(integers).ContainsOnly(3, 2, 1);

	var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
    Assert.That(guitarHeroes).ContainsExactly("Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell");
```
note: the Assert.That is here part of the NUnit library.

with NFluent, you can also write assertions like this:
```c#
	 var enumerable = new List<Person>
                                 {
                                     new Person { Name = "Thomas", Age = 38 },
                                     new Person { Name = "Achille", Age = 10, Nationality = Nationality.French },
                                     new Person { Name = "Anton", Age = 7, Nationality = Nationality.French },
                                     new Person { Name = "Arjun", Age = 7, Nationality = Nationality.Indian }
                                 };

            // TODO: find a way to get rid of the lack of type inference here (<Person, string> is not really fluent...)
            Assert.That(enumerable.Properties("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
            Assert.That(enumerable.Properties("Age")).ContainsExactly(38, 10, 7, 7);
            Assert.That(enumerable.Properties("Nationality")).ContainsExactly(Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian);

            // more fluent than the following classical NUnit way, isn't it? 
            // CollectionAssert.AreEquivalent(enumerable.Properties("Age"), new[] { 38, 10, 7, 7 });

            // maybe even more fluent than the java versions
			// FEST fluent assert v 2.x:
            // assertThat(extractProperty("name" , String.class).from(inn.getItems())).containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
			// FEST fluent assert v 1.x:
			// assertThat(inn.getItems()).onProperty("name").containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
```        

Why NFluent, and not another .NET fluent assertion framework?
----------------------------------------------------------------------------
+ Because you don't think that writing a lambda expression within an assertion statement is really a fluent experience (neither a fluent reading experience too).
+ Because this API is completely driven by the __[super-duper-happy-path](https://github.com/NancyFx/Nancy/wiki/Introduction)__ principle to fluent your .NET TDD-practitioner life. For instance, we consider the 'dot' autocompletion experience as crucial. Thus, it should not be a painful experience (intellisense flooding).
+ Because you dislike the SubjectUnderTest.Should()... syntax like me (which is not semantically as strong as the Assert or Check one).
+ And because you like *killer feature* such as the Properties() extension method for IEnumerable for instance (as showed within the usage sample above). 

- - -

Newsgroup
---------
For any comment, remark or question, please use the __[NFluent-Discuss google group](https://groups.google.com/forum/#!forum/nfluent-discuss)__.

BackLog
-------
Nfluent __[backlog is available here](./Backlog.md)__

Quality chart
-------------
You want to contribute? See the __[NFluent definition of done for development](./DevDoD.md)__ before. But welcome!

- - -

[thomas@pierrain.net](mailto:thomas@pierrain.net) / February 6th 2013