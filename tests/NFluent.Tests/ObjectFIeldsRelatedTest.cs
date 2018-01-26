namespace NFluent.Tests
{
    using System.Collections.Generic;
    using System;
    using System.Collections;
    using ApiChecks;
    using Helpers;
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
        public void HasNotFieldsWithSameValuesWorks()
        {
            var x = new DummyClass();
            Check.That(x).HasNotFieldsWithSameValues(new DummyClass(1, 2));

            // check with missing fields
            Check.That(new DummyClass()).HasNotFieldsWithSameValues(new DummyHeritance(1, 2));
        }

        [Test]
        public void ShouldWorkForNullSut()
        {
            Check.That((object)null).HasFieldsWithSameValues((object)null);
        }

        [Test]
        public void HasNotFieldsWithSameValuesDoesNotLoseOriginalTypeForOtherCheck()
        {
            var numbers = new[] { 1, 2, 3 };
            Check.That(numbers).HasNotFieldsWithSameValues(new { Length = 99 }).And.Contains(2);
        }

#pragma warning disable 618
        [Test]
        public void ObsoleteApiChecks()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsEqualToThose(new DummyClass());
        }

        [Test]
        public void HasFieldsNotEqualToThoseIsObsoleteButWorks()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsNotEqualToThose(new DummyClass(1, 2));

            // check with missing fields
            Check.That(new DummyClass()).HasFieldsNotEqualToThose(new DummyHeritance(1, 2));
        }

#pragma warning restore 618

        [Test]
        public void ShouldWorkOnPlainArrays()
        {
            var expected = new ArrayList();
            expected.Add(1);
            expected.Add(2);
            Check.That(new[] { 1, 2 }).IsEqualTo(expected);
        }

        [Test]
        public void ShouldFailOnDifferentArrays()
        {
            Check.That(new[] { 1, 2 }).HasFieldsWithSameValues(new[] { 1, 2 });
            Check.ThatCode(() => { Check.That(new[] { 1, 2 }).HasFieldsWithSameValues(new[] { 2, 3 }); })
                .Throws<FluentCheckException>();
        }

        [Test]
        public void ShouldFailOnDifferentArrayLength()
        {
            Check.ThatCode(() => { Check.That(new[] { 1, 2 }).HasFieldsWithSameValues(new[] { 1, 2, 3 }); })
                .Throws<FluentCheckException>();
        }

        [Test]
        public void NotShouldFailOnSameArrays()
        {
            Check.ThatCode(() => { Check.That(new[] { 1, 2 }).Not.HasFieldsWithSameValues(new[] { 1, 2 }); })
                .Throws<FluentCheckException>();
        }

        [Test]
        public void ShouldIdentifyNullArrays()
        {
            Check.ThatCode(() => { Check.That(new[] { 1, 2 }).HasFieldsWithSameValues((int[])null); })
                .Throws<FluentCheckException>();
        }

        [Test]
        public void IsEqualFailsIfFieldsDifferent()
        {
            var x = new DummyClass(2, 2);

            Check.ThatCode(() => { Check.That(x).HasFieldsWithSameValues(new DummyClass(1, 2)); })
                .Throws<FluentCheckException>().WithMessage(
                    Environment.NewLine + "The checked value's field 'x' does not have the expected value."
                    + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[2]" + Environment.NewLine
                    + "The expected value:" + Environment.NewLine + "\t[1]");
        }

        [Test]
        public void IsEqualFailsIfFieldsDifferentEvenInBaseClass()
        {
            var x = new DummyHeritance(2, 3);

            Check.ThatCode(() => { Check.That(x).HasFieldsWithSameValues(new DummyHeritance(2, 1)); })
                .Throws<FluentCheckException>().WithMessage(
                    Environment.NewLine + "The checked value\'s field \'y\' does not have the expected value."
                    + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[3]" + Environment.NewLine
                    + "The expected value:" + Environment.NewLine + "\t[1]");
        }

        [Test]
        public void HasNotFieldsWithSameValuesFailsIfSame()
        {
            Check.ThatCode(() => { Check.That(new DummyClass()).HasNotFieldsWithSameValues(new DummyClass(2,2)); })
                .Throws<FluentCheckException>().WithMessage(
                    Environment.NewLine
                    + "The checked value's field 'x' has the same value in the comparand, whereas it must not."
                    + Environment.NewLine + "The expected value: different from" + Environment.NewLine
                    + "\t[2] of type: [int]");
        }

        [Test]
        public void IsEqualfailsWithMissingFields()
        {
            var x = new DummyClass();

            Check.ThatCode(() => { Check.That(x).HasFieldsWithSameValues(new DummyHeritance()); })
                .FailsWithMessage("", "The expected one's field 'z' is absent from the checked value.", "The expected field 'z':", "\t[2]");
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
        public void HasFieldsWithSameValuesWorksProperlyWithArrays()
        {
            var x = new DummyClass(2, 4);
            var expected = new { x = 2, y = 4, emptyList = (int[])null };
            Check.That(x).HasFieldsWithSameValues(expected);
            Check.That(expected).HasFieldsWithSameValues(x);;
        }

        [Test]
        public void HasFieldsWithSameValueWorksWithSubSet()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsWithSameValues(new { x = 2 });
        }

        [Test]
        public void HasFieldsWithSameValuesDoesNotLoseOriginalTypeForOtherCheck()
        {
            // warning: this test depends on some internal details of List type
            var numbers = new List<int>(new[] { 1, 2, 3 });
            Check.That(numbers).HasFieldsWithSameValues(new { _size = 3 }).And.Contains(2);
        }

#pragma warning disable 618
        [Test]
        public void HasFieldsEqualToThoseIsObsoleteButWorksAgainstAnonymousClass()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsEqualToThose(new { x = 2, y = 3});
        }
#pragma warning restore 618

        [Test]
        public void HasFieldsEqualWorksAgainstAnonymousClassAndAutoProperties()
        {
            var x = new DummyWithAutoProperty { Application = "thePrivateField" };

            Check.That(x).HasFieldsWithSameValues(new { Application = "thePrivateField" });
        }

        [Test]
        public void HasFieldsEqualFailsForAutoPropertyWhenNegated()
        {
            var x = new DummyWithAutoProperty();

            Check.ThatCode(() => { Check.That(x).HasNotFieldsWithSameValues(new DummyWithAutoProperty()); })
                .Throws<FluentCheckException>().WithMessage(
                    Environment.NewLine
                    + "The checked value's autoproperty 'Application' (field '<Application>k__BackingField') has the same value in the comparand, whereas it must not."
                    + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\t[null]");
        }

        [Test]
        public void HasFieldsFailsProperlyForAutoProperty()
        {
            var x = new DummyWithAutoProperty { Application = "check" };

            Check.ThatCode(() => { Check.That(x).HasFieldsWithSameValues(new DummyWithAutoProperty()); })
                .Throws<FluentCheckException>().WithMessage(
                    Environment.NewLine
                    + "The checked value's autoproperty 'Application' (field '<Application>k__BackingField') does not have the expected value."
                    + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[\"check\"]"
                    + Environment.NewLine + "The expected value:" + Environment.NewLine + "\t[null]");
        }

        [Test]
        public void HasFieldsFailsProperlyForAutoPropertyForNull()
        {
            var x = new DummyWithAutoProperty();
            var y = new DummyWithAutoProperty();
            x.Application = "check";

            Check.ThatCode(() => { Check.That(y).HasFieldsWithSameValues(x); }).Throws<FluentCheckException>()
                .WithMessage(
                    Environment.NewLine
                    + "The checked value's autoproperty 'Application' (field '<Application>k__BackingField') does not have the expected value."
                    + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[null]"
                    + Environment.NewLine + "The expected value:" + Environment.NewLine
                    + "\t[\"check\"] of type: [string]");
        }

        [Test]
        public void HasFieldsWithSameValuesRecurse()
        {
            var x = new { x = new DummyClass(), text = "thePrivateField" };
            var y = new { x = new DummyClass(), text = "thePrivateField" };
            Check.That(x).HasFieldsWithSameValues(y);
        }

        [Test]
        public void HasFieldsWithSameValuesFailsProperlyRecurse()
        {
            var x = new { x = new DummyClass(2, 3), text = "thePrivateField" };
            var y = new { x = new DummyClass(2, 4), text = "thePrivateField" };

            Check.ThatCode(() => { Check.That(x).HasFieldsWithSameValues(y); }).Throws<FluentCheckException>()
                .WithMessage(
                    Environment.NewLine + "The checked value's field 'x.y' does not have the expected value."
                    + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[3]" + Environment.NewLine
                    + "The expected value:" + Environment.NewLine + "\t[4]");
        }

        private class DummyClass
        {
            // ReSharper disable once NotAccessedField.Local
            private int x = 2;

            // ReSharper disable once NotAccessedField.Local
            private int y = 3;

            // ReSharper disable once NotAccessedField.Local
            private List<string> emptyList;

            public DummyClass()
            {
                this.emptyList = new List<string>();
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
            private int z = 2;

            // ReSharper disable once UnusedMember.Local
            public int X
            {
                get
                {
                    return this.z;
                }
            }

            public DummyHeritance()
            {
            }

            public DummyHeritance(int x, int y)
                : base(x, y)
            {
            }
        }

        private class DummyWithAutoProperty
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Application { get; set; }
        }
    }
}