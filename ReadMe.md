![NFluent](https://github.com/tpierrain/nfluent/blob/master/NFluentBanner.png?raw=true)

NFluent overview
==============

NFluent is __an ergonomic assertion library__ which aims __to fluent your .NET TDD experience__.

__NFluent will make your tests__:
+ __fluent to write__: with auto completion \+ an happy 'dot' experience. Indeed, just type the Check.That( followed by one of your object and a dot, and your IDE will show you all the assertions available for the type of the given object to verify. No more, no less (i.e. no auto completion flooding).
+ __fluent to read__: very close to plain English, making it easier for non-technical people to read test code.
+ __fluent to troubleshoot__: every failing assertion of the NFluent library throws an Exception with a crystal-clear message status to ease your TDD experience. Thus, no need to set a breakpoint and to debug in order to be able to figure out what went wrong. 
+ __helpful to reverse engineer legacy code__: indeed, temporarily write an on-purpose failing assert on a legacy method, so you can understand it and leverage on the "ready-to-be-copied-and-paste-for-arrays-or-collections-initialization-purpose" NFluent assert failure messages.
+ __less error-prone__: indeed, no more confusion about the order of the "expected" and "actual" values you can find in the classical .NET unit tests frameworks.

NFluent is __directly inspired by the awesome Java FEST Fluent__ assertion/reflection library (http://fest.easytesting.org/).

NFluent & unit test frameworks
-------------------------------
__NFluent is not coupled to any .NET unit test framework. It is fully designed to work in collaboration with your favorite one.__

Your favorite unit test framework (e.g. NUnit, xUnit, ...) will still handle the test identification, execution & Co. __All you have to do is to replace your usage of its `Assert` statements, by the `Check.That()` NFluent statement form. That's all!__

Indeed, we decided to use the `Check.That()` syntax to avoid collisions and name ambiguity with the traditional `Assert` class you can find in most of your .NET unit test frameworks (therefore, no need to declare an alias in your test fixtures).


Uses cases
----------
__[NFluent use cases are available here](./UseCases.md)__.

Usage sample
------------

With NFluent, you can write simple assertions like this:
```c#	
    var integers = new int[] { 1, 2, 3, 4, 5, 666 };
    Check.That(integers).Contains(3, 5, 666);

	var integers = new int[] { 1, 2, 3 };
    Check.That(integers).ContainsOnly(3, 2, 1);

	var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
    Check.That(guitarHeroes).ContainsExactly("Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell");

	var heroes = "Batman and Robin";
    Check.That(heroes).StartsWith("Bat").And.Contains("Robin");

	string motivationalSaying = "Failure is mother of success.";
    Check.That(motivationalSaying).IsNotInstanceOf<int>();

```
note: the Check.That is here part of the NUnit library.

with NFluent, you can also write assertions like this:
```c#
	 var enumerable = new List<Person>
                                 {
                                     new Person { Name = "Thomas", Age = 38 },
                                     new Person { Name = "Achille", Age = 10, Nationality = Nationality.French },
                                     new Person { Name = "Anton", Age = 7, Nationality = Nationality.French },
                                     new Person { Name = "Arjun", Age = 7, Nationality = Nationality.Indian }
                                 };

            Check.That(enumerable.Properties("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
            Check.That(enumerable.Properties("Age")).ContainsExactly(38, 10, 7, 7);
            Check.That(enumerable.Properties("Nationality")).ContainsExactly(Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian);

            // more fluent than the following classical NUnit way, isn't it? 
            // CollectionAssert.AreEquivalent(enumerable.Properties("Age"), new[] { 38, 10, 7, 7 });

            // it's maybe even more fluent than the java versions
			// FEST fluent assert v 2.x:
            // assertThat(extractProperty("name" , String.class).from(inn.getItems())).containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
			// FEST fluent assert v 1.x:
			// assertThat(inn.getItems()).onProperty("name").containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
```        

Why NFluent, and not another .NET fluent assertion framework?
----------------------------------------------------------------------------
+ Because you don't think that writing a lambda expression within an assertion statement is really a fluent experience (neither on a reading perspective).
+ Because NFluent is completely driven by the __[super-duper-happy-path](https://github.com/NancyFx/Nancy/wiki/Introduction)__ principle to fluent your TDD experience. For instance, we consider the 'dot' autocompletion experience as crucial. Thus, it should not be polluted by things not related to the current unit testing context (which occurs with extension methods on classical .NET types - intellisense flooding).
+ Because you dislike the `<subjectUnderTest>.Should().` syntax like me (which I find not semantically as strong as the `Assert` or the `Check.That` ones).
+ And because you like *killing features* such as the Properties() extension method for IEnumerable (as showed within the usage sample above). 

- - -

Newsgroup
---------
For any comment, remark or question on the library, please use the __[NFluent-Discuss google group](https://groups.google.com/forum/#!forum/nfluent-discuss)__.

BackLog
-------
Nfluent __[backlog is available here](./Backlog.md)__

New feature to be added?
------------------------
+ If you want to join the project and contribute: see the __[NFluent DoD for development](./DevDoD.md)__ before, but be my guest. 
+ If you don't want to contribute on the library, but you need a feature not yet implemented, don't hesitate to request it on the __[NFluent-Discuss google group](https://groups.google.com/forum/#!forum/nfluent-discuss)__.
__In any cases: you are welcome!__
- - -

[thomas@pierrain.net](mailto:thomas@pierrain.net) / February 6th 2013