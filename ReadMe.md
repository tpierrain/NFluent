![NFluent](https://github.com/tpierrain/nfluent/blob/master/NFluentBanner.png?raw=true)

NFluent overview
==============

NFluent is __an assertion library__ which aims __to fluent your .NET TDD experience__.

__NFluent will make your tests__:
+ __fluent to write__: with a __[super-duper-happy](https://github.com/NancyFx/Nancy/wiki/Introduction) auto-completion 'dot' experience__. Indeed, just type the Check.That( followed by one of your object and a dot, and your IDE will show you all the assertions available for the type of the given object to verify. No more, no less (i.e. no auto completion flooding).
+ __fluent to read__: very close to plain English, making it easier for non-technical people to read test code.
+ __fluent to troubleshoot__: every failing assertion of the NFluent library throws an Exception with a crystal-clear message status to ease your TDD experience (see examples below). Thus, no need to set a breakpoint and to debug in order to be able to figure out what went wrong. 
+ __helpful to reverse engineer legacy code__: indeed, temporarily write an on-purpose failing assert on a legacy method, so you can understand it and leverage on the "ready-to-be-copied-and-paste-for-arrays-or-collections-initialization-purpose" NFluent assert failure messages.
+ __less error-prone__: indeed, no more confusion about the order of the "expected" and "actual" values you can find in the classical .NET unit tests frameworks.

NFluent is __directly inspired by the awesome Java FEST Fluent__ assertion/reflection library (http://fest.easytesting.org/).

NFluent & unit test frameworks
-------------------------------
__NFluent is not coupled to any .NET unit test framework. It is fully designed to work in collaboration with your favorite one.__

Your favorite unit test framework (e.g. NUnit, xUnit, ...) will still handle the test identification, execution & Co. __All you have to do is to replace your usage of its `Assert` or `Assert.That()` statements, by the `Check.That()` NFluent statement form. That's all!__

Indeed, we decided to use the `Check.That()` syntax to avoid collisions and name ambiguity with the traditional `Assert` class you can find in most of your .NET unit test frameworks (therefore, no need to declare an alias in your test fixtures).

In fact, __test runners and assertion libraries are two orthogonal topics and concerns__.


As simple as possible
=====================

With NFluent assertion libraries:

All you've got to remember is: `Check.That`, cause every assertion is then provided via a super-duper-auto-completion-dot-experience ;-)
------------------------------------------------------------------------------------------------------------------------


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

	var camus = new Person() { Name = "Camus" };
    var sartre = new Person() { Name = "Sartre" };
    Check.That(camus).IsNotEqualTo(sartre).And.IsInstanceOf<Person>();

	var heroes = "Batman and Robin";
    Check.That(heroes).StartsWith("Bat").And.Contains("Robin");

	string motivationalSaying = "Failure is mother of success.";
    Check.That(motivationalSaying).IsNotInstanceOf<int>();

```
with NFluent, you can also write assertions like this:
```c#
	 var persons = new List<Person>
                                 {
                                     new Person { Name = "Thomas", Age = 38 },
                                     new Person { Name = "Achille", Age = 10, Nationality = Nationality.French },
                                     new Person { Name = "Anton", Age = 7, Nationality = Nationality.French },
                                     new Person { Name = "Arjun", Age = 7, Nationality = Nationality.Indian }
                                 };

    Check.That(persons.Properties("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
    Check.That(persons.Properties("Age")).ContainsExactly(38, 10, 7, 7);
    Check.That(persons.Properties("Nationality")).ContainsExactly(Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian);

    // more fluent than the following classical NUnit way, isn't it? 
    // CollectionAssert.AreEquivalent(persons.Properties("Age"), new[] { 38, 10, 7, 7 });

    // it's maybe even more fluent than the java versions
	// FEST fluent assert v 2.x:
    // assertThat(extractProperty("name" , String.class).from(inn.getItems())).containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
	// FEST fluent assert v 1.x:
	// assertThat(inn.getItems()).onProperty("name").containsExactly("+5 Dexterity Vest", "Aged Brie", "Elixir of the Mongoose", "Sulfuras, Hand of Ragnaros", "Backstage passes to a TAFKAL80ETC concert", "Conjured Mana Cake");
```        
or like this:
```c#
	// Works also with lambda for exception checking
	Check.That(() => { throw new InvalidOperationException(); }).Throws<InvalidOperationException>();

	// or execution duration checking
	Check.That(() => Thread.Sleep(30)).LastsLessThan(60, TimeUnit.Milliseconds);
	
``` 
Why NFluent, and not another .NET fluent assertion framework?
----------------------------------------------------------------------------
+ Because you think like us that writing a lambda expression within an assertion statement is not really a fluent experience (neither on a reading perspective).
+ Because NFluent is completely driven by the __[super-duper-happy-path](https://github.com/NancyFx/Nancy/wiki/Introduction)__ principle to fluent your TDD experience. For instance, we consider the 'dot' autocompletion experience as crucial. Thus, it should not be polluted by things not related to the current unit testing context (which occurs with extension methods on classical .NET types - intellisense flooding).
+ Because you think that those other assertion libraries have not chosen the proper vocabulary (`<subjectUnderTest>.Should().`... why don't they choose `Must` instead?!?). And thus, you'd rather rely on a stronger semantic for your assertions (i.e. NFluent's `Check.That`).
+ And because you like *killing features* and extra bonus, such as the Properties() extension method for IEnumerable for instance (as showed within the usage sample above). 

- - -

Samples of crystal-clear error messages
---------------------------------------

![ErrorSample1](https://github.com/tpierrain/nfluent/blob/master/Images/ErrorSample1.png?raw=true)

![ErrorSample2](https://github.com/tpierrain/nfluent/blob/master/Images/ErrorSample2.png?raw=true)

![ErrorSample3](https://github.com/tpierrain/nfluent/blob/master/Images/ErrorSample3.png?raw=true)



Wanna try NFluent?
------------------
Can't be more easy: NFluent is [available on nuget.org](http://nuget.org/packages/NFluent/)

![nuget](https://github.com/tpierrain/nfluent/blob/master/Images/nuget.png?raw=true)



Uses cases
----------
__[NFluent use cases are available here](./UseCases.md)__.

Newsgroup
---------
For any comment, remark or question on the library, please use the __[NFluent-Discuss google group](https://groups.google.com/forum/#!forum/nfluent-discuss)__.

BackLog
-------
Nfluent __backlog is now available as github issues__

New feature to be added?
------------------------
+ If you want to join the project and contribute: __[check this out before](./CONTRIBUTING.md)__ before, but be our guest. 
+ If you don't want to contribute on the library, but you need a feature not yet implemented, don't hesitate to request it on the __[NFluent-Discuss google group](https://groups.google.com/forum/#!forum/nfluent-discuss)__.
__In any cases: you are welcome!__

Many thanks
------
+ To the other contributors: __[Marc-Antoine LATOUR](https://github.com/malat)__, __[Rui CARVALHO](http://www.codedistillers.com/)__ & __[Cyrille DUPUYDAUBY](http://dupdob.wordpress.com/)__.

+ To __[Rui CARVALHO](http://www.codedistillers.com/)__, for the nice NFluent logo he has designed.

+ To the mates that gave me ideas and feedbacks to make this lib as fluent as possible: __[Joel COSTIGLIOLA](https://github.com/joel-costigliola)__, __[Rui CARVALHO](http://www.codedistillers.com/)__, __[Cyrille DUPUYDAUBY](http://dupdob.wordpress.com/)__, __Benoit LABAERE__, ... 

+ To __Omer RAVIV__, which supports the NFluent project by offering us some free licenses for the nice __[BugAid](http://www.bugaidsoftware.com/features/)__ Visual Studio extensions.

- - -

[thomas@pierrain.net](mailto:thomas@pierrain.net) / March 2013
