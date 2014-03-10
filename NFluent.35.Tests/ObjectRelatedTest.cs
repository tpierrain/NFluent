// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ObjectRelatedTest.cs" company="">
// //   Copyright 2013 Cyrille DUPUYDAUBY, Thomas PIERRAIN
// //   Licensed under the Apache License, Version 2.0 (the "License");
// //   you may not use this file except in compliance with the License.
// //   You may obtain a copy of the License at
// //       http://www.apache.org/licenses/LICENSE-2.0
// //   Unless required by applicable law or agreed to in writing, software
// //   distributed under the License is distributed on an "AS IS" BASIS,
// //   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// //   See the License for the specific language governing permissions and
// //   limitations under the License.
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Tests
{
    using NFluent.Tests.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class ObjectRelatedTest
    {
        [Test]
        public void IsSameReferenceThanWorks()
        {
            var test = new object();
            Check.That(test).IsSameReferenceThan(test);
        }

        [Test]
        public void IsSameReferenceThanDoesNotLoseOriginalTypeForOtherCheck()
        {
            var numbers = new int[] { 1, 4, 42 };
            Check.That(numbers).IsSameReferenceThan(numbers).And.Contains(42);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked object must be the same instance than expected one.\nThe checked object:\n\t[System.Object]\nThe expected object: same instance than\n\t[System.Object]")]
        public void IsSameReferenceFailsProperly()
        {
            Check.That(new object()).IsSameReferenceThan(new object());
        }

        [Test]
        public void IsDistinctWorks()
        {
            Check.That(new object()).IsDistinctFrom(new object());
        }
        
        [Test]
        public void IsDistinctFromDoesNotLoseOriginalTypeForOtherCheck()
        {
            var numbers = new int[] { 1, 4, 42 };
            var otherNumbers = new int[] { 7, 8, 9 };
            Check.That(numbers).IsDistinctFrom(otherNumbers).And.Contains(42);
        }

        [Test]
        public void IsNullWorks()
        {
            Check.That((object)null).IsNull();
        }

        [Test]
        public void IsNullWorksWithNullable()
        {
            Check.That((int?)null).IsNull();
        }

        [Test]
        public void IsNullDoesNotLoseOriginalTypeForOtherCheck()
        {
            var values = new[] { 0, 1, 2 };
            Check.That(values).Not.IsNull().And.HasSize(3);
        }

        [Test]
        public void IsNotNullWorksWithNullable()
        {
            Mood? goodMood = new Mood();
            Check.That(goodMood).IsNotNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value is null whereas it must not.")]
        public void IsNotNullThrowsExceptionWithNullNullable()
        {
            Check.That((Mood?)null).IsNotNull();
        }

        [Test]
        public void IsNotNullDoesNotLoseOriginalTypeForOtherCheck()
        {
            var values = new[] { 0, 1, 2 };
            Check.That(values).IsNotNull().And.HasSize(3);
        }

        [Test]
        public void NotIsNullWorksWithNullable()
        {
            Mood? goodMood = new Mood();
            Check.That(goodMood).Not.IsNull();
        }

        [Test]
        public void NotIsNotNullWorksWithNullable()
        {
            Check.That((Mood?)null).Not.IsNotNull();
        }
        
        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value must be null.\nThe checked value:\n\t[NFluent.Tests.Extensions.Mood]")]
        public void NotIsNotNullThrowsWithNonNullNullable()
        {
            Mood? goodMood = new Mood();
            Check.That(goodMood).Not.IsNotNull();
        }
        
        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked nullable value is null whereas it must not.")]
        public void NotIsNullThrowsExceptionWithNullNullable()
        {
            Check.That((Mood?)null).Not.IsNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked object must be null.\nThe checked object:\n\t[System.Object]")]
        public void IsNullFailsProperly()
        {
            Check.That(new object()).IsNull();
        }

        [Test]
        public void IsNotNullWork()
        {
            Check.That(new object()).IsNotNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked object must not be null.\nThe checked object:\n\t[null]")]
        public void IsNotNullFailsProperly()
        {
            Check.That((object)null).IsNotNull();
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked object must have be an instance distinct from expected one.\nThe checked object:\n\t[System.Object]\nThe expected object: distinct from\n\t[System.Object]")]
        public void IsDistinctFailsProperly()
        {
            var test = new object();
            Check.That(test).IsDistinctFrom(test);
        }

        [Test]
        public void NegatedSameWorks()
        {
            var test = new object();
            Check.That(test).Not.IsDistinctFrom(test);
            Check.That(new object()).Not.IsSameReferenceThan(new object());           
        }

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
            Check.That(new DummyHeritance()).HasNotFieldsWithSameValues(new DummyClass());
        }

        [Test]
        public void HasNotFieldsWithSameValuesDoesNotLoseOriginalTypeForOtherCheck()
        {
            var numbers = new int[] { 1, 2, 3 };
            Check.That(numbers).HasNotFieldsWithSameValues(new { Length = 99 }).And.Contains(2);
        }

        [Test]
        public void HasFieldsNotEqualToThoseIsObsoleteButWorks()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsNotEqualToThose(new DummyClass(1, 2));

            // check with missing fields
            Check.That(new DummyHeritance()).HasFieldsNotEqualToThose(new DummyClass());
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's field 'x' does not have the expected value.\nThe checked value:\n\t[2]\nThe expected value:\n\t[1]")]
        public void IsEqualFailsIfFieldsDifferent()
        {
            var x = new DummyClass();
            Check.That(x).HasFieldsWithSameValues(new DummyClass(1, 2));
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's field 'x' has the same value in the comparand, whereas it must not.\nThe checked value:\n\t[2]\nThe expected value: different from\n\t[2]")]
        public void HasNotFieldsWithSameValuesFailsIfSame()
        {
            Check.That(new DummyClass()).HasNotFieldsWithSameValues(new DummyClass());
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's field 'z' is absent from the expected one.\nThe checked value:\n\t[NFluent.Tests.ObjectRelatedTest+DummyHeritance]\nThe expected value:\n\t[NFluent.Tests.ObjectRelatedTest+DummyClass]")]
        public void IsEqualfailsWithMissingFields()
        {
            var x = new DummyHeritance();
            Check.That(x).HasFieldsWithSameValues(new DummyClass());
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
        public void HasFieldsWithSameValuesDoesNotLoseOriginalTypeForOtherCheck()
        {
            var numbers = new int[] { 1, 2, 3 };
            Check.That(numbers).HasFieldsWithSameValues(new { Length = 3 }).And.Contains(2);
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's autoproperty 'Application' (field '<Application>k__BackingField') has the same value in the comparand, whereas it must not.\nThe checked value:\n\t[null]\nThe expected value: different from\n\t[null]")]
        public void HasFieldsEqualFailsForAutoPropertyWhenNEgated()
        {
            var x = new DummyWithAutoProperty();
            Check.That(x).HasNotFieldsWithSameValues(new DummyWithAutoProperty());
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's autoproperty 'Application' (field '<Application>k__BackingField') does not have the expected value.\nThe checked value:\n\t[\"check\"]\nThe expected value:\n\t[null]")]
        public void HasFieldsFailsProperlyForAutoProperty()
        {
            var x = new DummyWithAutoProperty();
            x.Application = "check";
            Check.That(x).HasFieldsWithSameValues(new DummyWithAutoProperty());
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value's autoproperty 'Application' (field '<Application>k__BackingField') does not have the expected value.\nThe checked value:\n\t[null]\nThe expected value:\n\t[\"check\"]")]
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
            private int x = 2;
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
            private int z = 2;

            public DummyHeritance()
            {
            }

            public DummyHeritance(int x, int y, int z)
                : base(x, y)
            {
                this.z = z;
            }
        }

        private class DummyWithAutoProperty
        {
            public string Application { get; set; }
        }
    }
}
