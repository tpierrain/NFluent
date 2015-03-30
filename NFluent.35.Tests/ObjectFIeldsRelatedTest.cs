namespace NFluent.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class ObjectFieldsRelatedTest
    {
        [Test]
        public void IsEqualComparingFields()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsWithSameValues(new DummyClass());
        }

        [Test]
        public void ObsoleteAPIChecks()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsEqualToThose(new DummyClass());
        }

        [Test]
        public void HasNotFieldsWithSameValuesWorks()
        {
            var x = new DummyClass();
            Check.That(x).HasNotFieldsWithSameValues(new DummyClass(1, 2)); 

            // check with missing fields
            Check.That(new DummyClass()).HasNotFieldsWithSameValues(new DummyHeritance());
        }

        [Test]
        public void HasNotFieldsWithSameValuesDoesNotLoseOriginalTypeForOtherCheck()
        {
            var numbers = new[] { 1, 2, 3 };
            Check.That(numbers).HasNotFieldsWithSameValues(new { Length = 99 }).And.Contains(2);
        }

        [Test]
        public void HasFieldsNotEqualToThoseIsObsoleteButWorks()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsNotEqualToThose(new DummyClass(1, 2));

            // check with missing fields
            Check.That(new DummyClass()).HasFieldsNotEqualToThose(new DummyHeritance());
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's field 'x' does not have the expected value.\nThe checked value:\n\t[2]\nThe expected value:\n\t[1]")]
        public void IsEqualFailsIfFieldsDifferent()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsWithSameValues(new DummyClass(1, 2));
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's field 'x' has the same value in the comparand, whereas it must not.\nThe expected value: different from\n\t[2] of type: [int]")]
        public void HasNotFieldsWithSameValuesFailsIfSame()
        {
            Check.That(new DummyClass()).HasNotFieldsWithSameValues(new DummyClass());
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's field 'z' is absent from the expected one.\nThe expected value:\n\t[null]")]
        public void IsEqualfailsWithMissingFields()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsWithSameValues(new DummyHeritance());
        }

        [Test]
        public void HasFieldsWithSameValuesWorksForAutoProperty()
        {
            var x = new DummyWithAutoProperty();
            Check.That(x).HasFieldsWithSameValues(new DummyWithAutoProperty());
        }

        [Test]
        public void HasFieldsWithSameValuesWorksAgainstAnonymousClass()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsWithSameValues(new { x = 2, y = 3 });
        }

        [Test]
        public void HasFieldsWithSameValueWorksWithSubSet()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsWithSameValues(new { x = 2});
        }

        [Test]
        public void HasFieldsWithSameValuesDoesNotLoseOriginalTypeForOtherCheck()
        {
            // warning: this test depends on some internal details of List type
            var numbers = new List<int>( new[] { 1, 2, 3 });
            Check.That(numbers).HasFieldsWithSameValues(new { _size = 3 }).And.Contains(2);
        }

        [Test]
        public void HasFieldsEqualToThoseIsObsoleteButWorksAgainstAnonymousClass()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsEqualToThose(new { x = 2, y = 3 });
        }

        [Test]
        public void HasFieldsEqualWorksAgainstAnonymousClassAndAutoProperties()
        {
            var x = new DummyWithAutoProperty { Application = "toto" };

            Check.That(x).HasFieldsWithSameValues(new { Application = "toto" });
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's autoproperty 'Application' (field '<Application>k__BackingField') has the same value in the comparand, whereas it must not.\nThe expected value: different from\n\t[null]")]
        public void HasFieldsEqualFailsForAutoPropertyWhenNegated()
        {
            var x = new DummyWithAutoProperty();
            Check.That(x).HasNotFieldsWithSameValues(new DummyWithAutoProperty());
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's autoproperty 'Application' (field '<Application>k__BackingField') does not have the expected value.\nThe checked value:\n\t[\"check\"]\nThe expected value:\n\t[null]")]
        public void HasFieldsFailsProperlyForAutoProperty()
        {
            var x = new DummyWithAutoProperty { Application = "check" };
            Check.That(x).HasFieldsWithSameValues(new DummyWithAutoProperty());
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's autoproperty 'Application' (field '<Application>k__BackingField') does not have the expected value.\nThe checked value:\n\t[null]\nThe expected value:\n\t[\"check\"] of type: [string]")]
        public void HasFieldsFailsProperlyForAutoPropertyForNull()
        {
            var x = new DummyWithAutoProperty();
            var y = new DummyWithAutoProperty();
            x.Application = "check";
            Check.That(y).HasFieldsWithSameValues(x);
        }

        [Test]
        public void HasFieldsWithSameValuesRecurse()
        {
            var x = new { x = new DummyClass(), text = "toto" };
            var y = new { x = new DummyClass(), text = "toto" };
            Check.That(x).HasFieldsWithSameValues(y);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's field 'x.y' does not have the expected value.\nThe checked value:\n\t[3]\nThe expected value:\n\t[4]")]
        public void HasFieldsWithSameValuesFailsProperlyRecurse()
        {
            var x = new { x = new DummyClass(), text = "toto" };
            var y = new { x = new DummyClass(2, 4), text = "toto" };
            Check.That(x).HasFieldsWithSameValues(y);
        }

        private class DummyClass
        {
            // ReSharper disable once NotAccessedField.Local
            private int x = 2;
            // ReSharper disable once NotAccessedField.Local
            private int y = 3;

            public DummyClass()
            {
            }

            public DummyClass(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        private class DummyHeritance : DummyClass
        {
            // ReSharper disable once NotAccessedField.Local
#pragma warning disable 169
            private int z = 2;
#pragma warning restore 169
        }

        private class DummyWithAutoProperty
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Application { get; set; }
        }
    }
}