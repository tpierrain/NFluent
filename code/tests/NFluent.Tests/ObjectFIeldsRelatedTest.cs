namespace NFluent.Tests
{
    using System.Collections.Generic;
    using System;
    using System.Collections;
    using NFluent.Helpers;
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
            Check.ThatCode(() => { Check.That(new[] { 1, 2 }).HasFieldsWithSameValues(new[] { 1, 3 }); })
                .IsAFailingCheck();
        }

        [Test]
        public void ShouldFailOnDifferentArrayLength()
        {
            Check.ThatCode(() => { Check.That(new[] { 1, 2 }).HasFieldsWithSameValues(new[] { 1, 2, 3 }); })
                .IsAFailingCheck();
            Check.ThatCode(() => { Check.That((IEnumerable)new[] { 1, 2 }).HasFieldsWithSameValues((IEnumerable) new[] { 1, 2, 3 }); })
                .IsAFailingCheckWithMessage("", 
                    "The expected value's field 'this[2]''s is absent from the checked one.", 
                    "The expected value's field 'this[2]':", 
                    "\t[3] of type: [int]");
        }

        [Test]
        public void NotShouldFailOnSameArrays()
        {
            Check.ThatCode(() => { Check.That(new[] { 1, 2 }).Not.HasFieldsWithSameValues(new[] { 1, 2 }); })
                .IsAFailingCheck();
        }

        [Test]
        public void ShouldIdentifyNullArrays()
        {
            Check.ThatCode(() => { Check.That(new[] { 1, 2 }).HasFieldsWithSameValues((int[])null); })
                .IsAFailingCheck();
        }

        [Test]
        public void IsEqualFailsIfFieldsDifferent()
        {
            var x = new DummyClass(2, 2);

            Check.ThatCode(() => { Check.That(x).HasFieldsWithSameValues(new DummyClass(1, 2)); })
                .IsAFailingCheckWithMessage("",
                    "The checked value's field 'x' does not have the expected value.",
                    "The checked value's field 'x':",
                    "\t[2]",
                    "The expected value's field 'x':",
                    "\t[1]");
        }

        [Test]
        public void IsEqualFailsIfFieldsDifferentEvenInBaseClass()
        {
            var x = new DummyHeritance(2, 3);

            Check.ThatCode(() => { Check.That(x).HasFieldsWithSameValues(new DummyHeritance(2, 1)); })
                .IsAFailingCheckWithMessage(
                    "", "The checked value\'s field \'y\' does not have the expected value.",
                    "The checked value's field 'y':",
                    "\t[3]",
                    "The expected value's field 'y':",
                    "\t[1]");
        }

        [Test]
        public void HasNotFieldsWithSameValuesFailsIfSame()
        {
            Check.ThatCode(() => { Check.That(new DummyClass()).HasNotFieldsWithSameValues(new DummyClass(2,2)); })
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked value's field 'x' has the same value than the given one, whereas it should not.",
                    "The expected value's field 'x': different from",
                    "\t[2]");
        }

        [Test]
        public void IsEqualfailsWithMissingFields()
        {
            var x = new DummyClass();

            Check.ThatCode(() => { Check.That(x).HasFieldsWithSameValues(new DummyHeritance()); })
                .IsAFailingCheckWithMessage("", 
                    "The expected value's field 'z''s is absent from the checked one.", 
                    "The expected value's field 'z':", 
                    "\t[2] of type: [int]");
        }

        [Test]
        public void IsEqualToDrillsDeepestMember()
        {
            var x = new DummyClass();

            Check.ThatCode(() =>
                {
                    Check.That(new AltDummyClass()).Considering().All.Fields.IsEqualTo(new DummyClass());
                })
                .IsAFailingCheckWithMessage("", 
                    "The expected value's field 'emptyList._syncRoot''s is absent from the checked one.", 
                    "The expected value's field 'emptyList._syncRoot':", 
                    "\t[null] of type: [object]");

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
            Check.That(expected).HasFieldsWithSameValues(x);
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
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked value's autoproperty 'Application' (field '<Application>k__BackingField') has the same value than the given one, whereas it should not.",
                    "The expected value's autoproperty 'Application' (field '<Application>k__BackingField'): different from",
                    "\t[null]");
        }

        [Test]
        public void HasFieldsFailsProperlyForAutoProperty()
        {
            var x = new DummyWithAutoProperty { Application = "check" };

            Check.ThatCode(() => { Check.That(x).HasFieldsWithSameValues(new DummyWithAutoProperty()); })
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked value's autoproperty 'Application' (field '<Application>k__BackingField') does not have the expected value.",
                    "The checked value's autoproperty 'Application' (field '<Application>k__BackingField'):",
                    "\t[\"check\"] of type: [string]",
                    "The expected value's autoproperty 'Application' (field '<Application>k__BackingField'):",
                    "\t[null] of type: [object]");
        }

        [Test]
        public void HasFieldsFailsProperlyForAutoPropertyForNull()
        {
            var x = new DummyWithAutoProperty();
            var y = new DummyWithAutoProperty();
            x.Application = "check";

            Check.ThatCode(() => { Check.That(y).HasFieldsWithSameValues(x); }).IsAFailingCheckWithMessage(
                    "",
                    "The checked value's autoproperty 'Application' (field '<Application>k__BackingField') does not have the expected value.",
                    "The checked value's autoproperty 'Application' (field '<Application>k__BackingField'):",
                    "\t[null] of type: [object]",
                    "The expected value's autoproperty 'Application' (field '<Application>k__BackingField'):",
                    "\t[\"check\"] of type: [string]");
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

            Check.ThatCode(() => { Check.That(x).HasFieldsWithSameValues(y); }).IsAFailingCheckWithMessage(
                    Environment.NewLine + "The checked value's field 'x.y' does not have the expected value."
                    + Environment.NewLine + "The checked value's field 'x.y':" + Environment.NewLine + "\t[3]" + Environment.NewLine
                    + "The expected value's field 'x.y':" + Environment.NewLine + "\t[4]");
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

        private class AltDummyClass
        {
            // ReSharper disable once NotAccessedField.Local
            private int x = 2;

            // ReSharper disable once NotAccessedField.Local
            private int y = 3;

            // ReSharper disable once NotAccessedField.Local
            private object emptyList = new object();
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