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

            Check.That(sut).Considering(ObjectFieldsCheckExtensions.Private.Fields).IsEqualTo(new SutClass(2, 42));
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

}
