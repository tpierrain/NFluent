using System;
using System.Collections.Generic;
using System.Text;

namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ConsideringRelatedTests
    {
        [Test]
        public void ShouldWorkForFields()
        {
            var sut = new SutClass(2, 42);

//            Check.That(sut).Considering(Private.Fields).IsEqualTo(new SutClass(2, 42));
        }

        private class SutClass
        {
            public int TheField;

            public int TheProperty {get; set;}

            public SutClass(int theField, int theProperty)
            {
                TheField = theField;
                TheProperty = theProperty;
            }
        }
    }

    public static class Temp
    {
        public static  ICheck<T> Considering<T>(this ICheck<T> checker, Criteria criteria)
        {
            return checker;
        }
    }

    public class Criteria
    {}

    public class Private
    {
        public static Criteria Fields { get; private set; }
    }

    public static class Public

    {
        public static Criteria Fields { get; private set; }
    }
}
