[![GitHub stars](https://img.shields.io/github/stars/tpierrain/nfluent.svg?style=social&label=Star)](https://github.com/tpierrain/NFluent)

![NFluent](https://github.com/tpierrain/nfluent/blob/master/NFluentBanner.png?raw=true)
![Motto](https://github.com/tpierrain/nfluent/blob/master/Images/AssertIsDead.png?raw=true)

**Stable**   [![NuGet](https://img.shields.io/nuget/v/NFluent.svg)](https://www.nuget.org/packages/NFluent/)
 [![NuGet](https://img.shields.io/nuget/dt/NFluent.svg)](https://www.nuget.org/packages/NFluent/)

**PreReleases**    [![NuGet Pre Release](https://img.shields.io/nuget/vpre/NFluent.svg)]()

**Beta**    [![MyGet](https://img.shields.io/myget/dupdobnightly/vpre/NFluent.svg)](https://www.myget.org/feed/dupdobnightly/package/nuget/NFluent) [![MyGet](https://img.shields.io/myget/dupdobnightly/dt/NFluent.svg)](https://www.myget.org/feed/dupdobnightly/package/nuget/NFluent)

**Issues**

[![GitHub issues](https://img.shields.io/github/issues/tpierrain/NFluent.svg)](https://github.com/tpierrain/NFluent/issues)
[![GitHub closed issues](https://img.shields.io/github/issues-closed/tpierrain/NFluent.svg)](https://github.com/tpierrain/NFluent/issues?q=is%3Aissue+is%3Aclosed)

**Build Status**

[![Build status](https://ci.appveyor.com/api/projects/status/ju5m6t3fm2xsl0o9/branch/master?svg=true)](https://ci.appveyor.com/project/tpierrain/nfluent/branch/master)
[![Codecov](https://img.shields.io/codecov/c/github/NFluent/NFluent.svg)](https://codecov.io/gh/NFluent/NFluent)
[![AppVeyor tests](https://img.shields.io/appveyor/tests/tpierrain/nfluent/master.svg)](https://ci.appveyor.com/project/tpierrain/nfluent)

---

NFluent is __an assertion library__ which aims __to fluent your .NET TDD experience__.

__Official site: [http://www.n-fluent.net/](http://www.n-fluent.net/)__

- - -

__NFluent will make your tests__:
+ __fluent to write__: with a __[super-duper-happy](https://github.com/NancyFx/Nancy/wiki/Introduction) auto-completion 'dot' experience__. Indeed, just type the Check.That( followed by one of your objects and a dot, and your IDE will show you all the checks available for the type of the given object to verify. No more, no less (i.e. no auto completion flooding).
+ __fluent to read__: very close to plain English, making it easier for non-technical people to read test code.
+ __fluent to troubleshoot__: every failing check of the NFluent library throws an Exception with a crystal-clear message status to ease your TDD experience (see examples below). Thus, no need to set a breakpoint and to debug in order to be able to figure out what went wrong.
+ __helpful to reverse engineer legacy code__: indeed, temporarily write an on-purpose failing assert on a legacy method, so you can understand it and leverage on the "ready-to-be-copied-and-paste-for-arrays-or-collections-initialization-purpose" NFluent assert failure messages.
+ __less error-prone__: indeed, no more confusion about the order of the "expected" and "actual" values you can find in the classical .NET unit tests frameworks.

NFluent is __directly inspired by the awesome Java FEST Fluent__ check/reflection library __(http://fest.easytesting.org/)__ which had been recently forked (by one of its most active contributor) to create the more prolific __[AssertJ](https://github.com/joel-costigliola/assertj-core)__ library.

NFluent & unit test frameworks
-------------------------------
__NFluent is not coupled to any .NET unit test framework. It is fully designed to work in collaboration with your favorite one.__

Your favorite unit test framework (e.g. NUnit, xUnit, ...) will still handle the test identification, execution & Co. __All you have to do is to replace your usage of its `Assert` or `Assert.That()` statements, by the `Check.That()` NFluent statement form. That's all!__

Indeed, we decided to use the `Check.That()` syntax to avoid collisions and name ambiguity with the traditional `Assert` class you can find in most of your .NET unit test frameworks (therefore, no need to declare an alias in your test fixtures).

In fact, __test runners and check libraries are two orthogonal topics and concerns__.


As simple as possible
=====================

With Nfluent check libraries:

All you've got to remember is: `Check.That`, 'cause every check is then provided via a super-duper-auto-completion-dot-experience ;-)
------------------------------------------------------------------------------------------------------------------------


Usage sample
------------

With NFluent, you can write simple checks like this:
```c#
    var integers = new int[] { 1, 2, 3, 4, 5, 666 };
    Check.That(integers).Contains(3, 5, 666);

    integers = new int[] { 1, 2, 3 };
    Check.That(integers).IsOnlyMadeOf(3, 2, 1);

    var guitarHeroes = new[] { "Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell" };
    Check.That(guitarHeroes).ContainsExactly("Hendrix", "Paco de Lucia", "Django Reinhardt", "Baden Powell");

    var camus = new Person() { Name = "Camus" };
    var sartre = new Person() { Name = "Sartre" };
    Check.That(camus).IsNotEqualTo(sartre).And.IsInstanceOf<Person>();

    var heroes = "Batman and Robin";
    Check.That(heroes).Not.Contains("Joker").And.StartsWith("Bat").And.Contains("Robin");

    int? one = 1;
    Check.That(one).HasAValue().Which.IsStrictlyPositive().And.IsEqualTo(1);

    const Nationality FrenchNationality = Nationality.French;
    Check.ThatEnum(FrenchNationality).IsNotEqualTo(Nationality.Korean);

    string motivationalSaying = "Failure is the mother of success.";
    Check.That(motivationalSaying).IsNotInstanceOf<int>();

```
with NFluent, you can also write checks like this:
```c#
	 var persons = new List<Person>
                                 {
                                     new Person { Name = "Thomas", Age = 38 },
                                     new Person { Name = "Achille", Age = 10, Nationality = Nationality.French },
                                     new Person { Name = "Anton", Age = 7, Nationality = Nationality.French },
                                     new Person { Name = "Arjun", Age = 7, Nationality = Nationality.Indian }
                                 };

    Check.That(persons.Extracting("Name")).ContainsExactly("Thomas", "Achille", "Anton", "Arjun");
    Check.That(persons.Extracting("Age")).ContainsExactly(38, 10, 7, 7);
    Check.That(persons.Extracting("Nationality")).ContainsExactly(Nationality.Unknown, Nationality.French, Nationality.French, Nationality.Indian);

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
	Check.ThatCode(() => { throw new InvalidOperationException(); }).Throws<InvalidOperationException>();

	// or execution duration checking
	Check.ThatCode(() => Thread.Sleep(30)).LastsLessThan(60, TimeUnit.Milliseconds);

```
Why NFluent, and not another .NET fluent check framework?
----------------------------------------------------------------------------
+ Because you think like us that writing a lambda expression within a check statement is not really a fluent experience (for reading as well as writing).
+ Because NFluent is completely driven by the __[super-duper-happy-path](https://github.com/NancyFx/Nancy/wiki/Introduction)__ principle to fluent your TDD experience. For instance, we consider the 'dot' autocompletion experience as crucial. Thus, it should not be polluted by things not related to the current unit testing context (which occurs with extension methods on classical .NET types - intellisense flooding).
+ Because you think that those other check libraries have not chosen the proper vocabulary (`<subjectUnderTest>.Should().`... why don't they choose `Must` instead?!?). And thus, you'd rather rely on a stronger semantic for your checks (i.e. NFluent's `Check.That`).
+ Because you like *killing features* and extra bonus, such as the Properties() extension method for IEnumerable for instance (as showed within the usage sample above).
+ And because it's awesome pal. Try it, you will see!

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



Use cases
----------
__[NFluent use cases are available here](./UseCases.md)__.

Newsgroup
---------
For any comment, remark or question about the library, please use the __[NFluent-Discuss google group](https://groups.google.com/forum/#!forum/nfluent-discuss)__.

BackLog
-------
Nfluent __backlog is now available as github issues__

New feature to be added?
------------------------
+ If you want to join the project and contribute: __[check this out before](./CONTRIBUTING.md)__, but be our guest.
+ If you don't want to contribute to the library, but you need a feature not yet implemented, don't hesitate to request it on the __[NFluent-Discuss google group](https://groups.google.com/forum/#!forum/nfluent-discuss)__.
__In any case: you are welcome!__

Other resources
---------------
+ __[Rui](https://github.com/rhwy)__ has published a great article about the NFluent extensibility model. Available __[here on CodeDistillers](http://www.codedistillers.com/rui/2013/11/26/nfluent-extensions/)__


Many thanks
------
+ To the other amazing contributors: __[Marc-Antoine LATOUR](https://github.com/malat)__, __[Rui CARVALHO](http://www.codedistillers.com/)__ & __[Cyrille DUPUYDAUBY](http://dupdob.wordpress.com/)__.

+ To __[Rui CARVALHO](http://www.codedistillers.com/)__, for the nice NFluent logo he has designed.

+ To the mates that gave me ideas and feedbacks to make this lib as fluent as possible: __[Joel COSTIGLIOLA](https://github.com/joel-costigliola)__ (former active contributor of Java FEST Assert, which now works on his __[AssertJ fork](https://github.com/joel-costigliola/assertj-core)__), __[Rui CARVALHO](http://www.codedistillers.com/)__, __[Cyrille DUPUYDAUBY](http://dupdob.wordpress.com/)__, __Benoit LABAERE__, ...

+ To __Omer RAVIV__, which supports the NFluent project by offering us some free licenses for the nice __[BugAid](http://www.bugaidsoftware.com/features/)__ Visual Studio extensions.

+ To __[AppVeyor CI](https://www.appveyor.com/)__, which now supports NFluent builds.

- - -


[thomas@pierrain.net](mailto:thomas@pierrain.net) / September 2016
