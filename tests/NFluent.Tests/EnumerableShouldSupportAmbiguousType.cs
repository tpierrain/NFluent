
namespace NFluent.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;
    using NFluent.Helpers;

    [TestFixture]
    public class EnumerableShouldSupportAmbiguousType
    {
        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailing()
        {
            IEnumerable enumerable = new System.Collections.Generic.List<int> { 45, 43, 54, 666 };
            IEnumerable suspiciousEnumerable = new List<int> { 45, 43, 54, 666 };

            Check.ThatCode(() =>  Check.That(suspiciousEnumerable).Not.IsEqualTo(enumerable)).IsAFailingCheckWithMessage("",
                "The checked enumerable is equal to the given one whereas it must not.",
                    "The expected enumerable: different from",
                    "\t{45, 43, 54, 666} (4 items) of type: [System.Collections.Generic.List<int>]");
        }

        private class List<T>: IEnumerable<T>
        {
            private readonly System.Collections.Generic.List<T> innerList = new System.Collections.Generic.List<T>();

            public IEnumerator<T> GetEnumerator()
            {
                return this.innerList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public void Add(T i)
            {
                this.innerList.Add(i);
            }
        }
    }
}
