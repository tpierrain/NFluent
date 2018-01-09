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
            
            Check.That(sut).Considering(Public.Fields).IsEqualTo(new SutClass(2, 42));
            Check.ThatCode(()=>
            Check.That(sut).Considering(Private.Fields).IsEqualTo(new SutClass(2, 42))).Throws<FluentCheckException>();
        }

        private class SutClass
        {
            private static int autoInc = 0;
            
            public int TheField;

            private int theProperty;

            public int TheProperty
            {
                get { return this.theProperty; }
                set { this.theProperty = value; }
            }

            private int toto;

            public SutClass(int theField, int theProperty)
            {
                TheField = theField;
                TheProperty = theProperty;
                this.toto = autoInc++;
            }
        }
    }

}
