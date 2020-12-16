namespace NFluent.Tests.ReportedIssues
{
    using NUnit.Framework;
    using NFluent.Helpers;

    [TestFixture]
    public class CheckObjectShould
    {
        [TestFixture]
public class Test
{

    [Test]
    public void Succeeds()
    {
        var expected = new SomeClass("AAA", "AAA");
        var actual = new SomeClass("AAA", "AAA");

        Check.That(actual).HasFieldsWithSameValues(expected);
    }

    [Test]
    public void Should_Fail()
    {
        var expected = new SomeClass("AAA", "AAA");
        var actual = new SomeClass("AAA", "BBB");
        Check.ThatCode( ()=>
         Check.That(actual).HasFieldsWithSameValues(expected)
         ).IsAFailingCheck();
    }

    [Test]
    public void Succeeds_2()
    {
        var expected = new TreeClass("AA");
        var actual = new TreeClass("AB");

        Check.ThatCode( ()=>
        Check.That(actual).HasFieldsWithSameValues(expected)
        ).IsAFailingCheck();
    }

    [Test]
    public void Should_Fail_2()
    {
        var expected = new TreeClass("AA");
        var actual = new TreeClass("AB");

        Check.ThatCode( ()=>
        Check.That(actual).HasFieldsWithSameValues(expected)
        ).IsAFailingCheck();
    }

    private class TreeClass
    {
        public string Name { get; }
        public TreeClass Child { get; }

        public TreeClass(string s)
        {
            Name = s[..1];
            if (s.Length > 1)
            {
                Child = new TreeClass(s[1..]);
            }
        }
    }

    private class SomeClass
    {
        public string A { get; }
        public string B { get; }

        public SomeClass(string a, string b)
        {
            A = a;
            B = b;
        }
    }
}

    }
}
