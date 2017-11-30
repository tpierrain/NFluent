using System.Collections.Generic;
using NFluent.Helpers;
using NUnit.Framework;

namespace NFluent.Tests
{
    public class CustomEqualTests
    {
        private readonly List<int> a = new List<int> { 1, 2 };
        private readonly List<int> b = new List<int> { 3, 4 };

        private readonly List<List<int>> list = new List<List<int>>
        {
            new List<int> {1, 2}, // new instance, same as a
            new List<int> {3, 4}  // new instance, same as b
        };

        [Test]
        public void ValueDifferenceReturnsEmptyIfEqual()
        {
            var result = EqualityHelper.ValueDifference(a, "a", a, "a");
            Check.That(result).HasSize(0);
        }

        [Test]
        public void ValueDifferenceGivesDetailsOnFailure()
        {
            // List contains new instances of lists same as a and b
 
            var result = EqualityHelper.ValueDifference(a, "a", b, "b");
            Check.That(result).HasSize(2);
            Check.That(result[0].FirstName).IsEqualTo("a[0]");
            Check.That(result[0].SecondName).IsEqualTo("b[0]");
            Check.That(result[0].FirstValue).IsEqualTo(1);
            Check.That(result[0].SecondValue).IsEqualTo(3);

            result = EqualityHelper.ValueDifference(a, "a", a, "a");
            Check.That(result).HasSize(0);

            result = EqualityHelper.ValueDifference(this.list, "a", new List<List<int>> {a, a}, "b");
            Check.That(result[0].FirstName).IsEqualTo("a[1][0]");
        }

        [Test]
        public void ValueDifferenceGivesDetailsOnFailureWithNull()
        {
            var result = EqualityHelper.ValueDifference(new List<List<int>> { a, null }, "a", this.list, "b");
            Check.That(result[0].FirstName).IsEqualTo("a[1]");
            result = EqualityHelper.ValueDifference(this.list, "a", new List<List<int>> { a, null }, "b");
            Check.That(result[0].FirstName).IsEqualTo("a[1]");
        }

        [Test]
        public void ValueDifferenceGivesDetailsOnFailureWithShorterEnums()
        {
            var result = EqualityHelper.ValueDifference(new List<List<int>> {this.a }, "a", this.list, "b");
            Check.That(result[0].FirstName).IsEqualTo(null);
            result = EqualityHelper.ValueDifference(this.list, "a", new List<List<int>> { a }, "b");
            Check.That(result[0].FirstName).IsEqualTo("a[1]");
        }

        [Test]
        public void HandleRecursion()
        {
            var recursive = new List<object> {this.a};
            recursive.Add(recursive);
            var otherRecursive = new List<object> {this.a};
            var interim = new List<object> {recursive};
            otherRecursive.Add(interim);
            Check.ThatCode(() => Check.That(recursive).IsEqualTo(otherRecursive)).Throws<FluentCheckException>();
            Check.ThatCode(() => Check.That(new List<object>{this.a, this.a}).IsEqualTo(recursive)).Throws<FluentCheckException>();
        }
    }
}
