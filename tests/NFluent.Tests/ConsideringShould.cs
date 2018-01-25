// --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ConsideringRelatedTests.cs" company="NFluent">
//   Copyright 2018 Thomas PIERRAIN & Cyrille DUPUYDAUBY
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ConsideringShould
    {
        private class SutClass
        {
            private static int autoInc;

            public int TheField;
            private int thePrivateField;

            public SutClass(int theField, int theProperty)
            {
                this.TheField = theField;
                this.TheProperty = theProperty;
                this.thePrivateField = autoInc++;
            }

            public SutClass(int theField, int theProperty, int thePrivateField, object thePrivateProperty)
            {
                this.TheField = theField;
                this.TheProperty = theProperty;
                this.ThePrivateProperty = thePrivateProperty;
                this.thePrivateField = thePrivateField;
            }

            protected internal object ThePrivateProperty { get; }

            public int TheProperty { get; }
        }

        [Test]
        public void NotShouldWorkWhenMissingMember()
        {
            var sut = new SutClass(2, 42);
            var expected = new {TheProperty = 12, Test = 11};

            Check.That(sut).Considering().Public.Properties.IsNoInstanceOfType(expected.GetType());
            Check.That(expected).Considering().Public.Properties.IsNoInstanceOfType(sut.GetType());
        }

        [Test]
        public void NotShouldFailWhenSame()
        {
            var sut = new SutClass(2, 42);
            var expected = new {TheProperty = 12};
            Check.ThatCode(() =>
            {
                Check.That(sut).Considering().Public.Properties.IsNoInstanceOfType(expected.GetType());
            }).Throws<FluentCheckException>();
        }

        [Test]
        public void DetectDifferentProperties()
        {
            Check.ThatCode(() =>
                    Check.That(new SutClass(2, 43)).Considering().NonPublic.Properties
                        .IsEqualTo(new {ThePrivateProperty = (object) null}))
                .Throws<FluentCheckException>();
            Check.ThatCode(() =>
                    Check.That(new {ThePrivateProperty = (object) null}).Considering().NonPublic.Properties
                        .IsEqualTo(new SutClass(2, 43)))
                .Throws<FluentCheckException>();
        }

        [Test]
        public void DetectMissingProperties()
        {
            Check.ThatCode(() =>
                    Check.That(new SutClass(2, 43)).Considering().All.Properties
                        .IsEqualTo(new {ThePrivateProperty = (object) null}))
                .Throws<FluentCheckException>();
            Check.ThatCode(() =>
                    Check.That(new {ThePrivateProperty = (object) null}).Considering().All.Properties
                        .IsEqualTo(new SutClass(2, 43)))
                .Throws<FluentCheckException>();
        }

        [Test]
        public void FailForAllMembers()
        {
            var sut = new SutClass(2, 42, 4, null);
            Check.ThatCode(() =>
            {
                Check.That(sut).Considering().All.Fields.And.All.Properties
                    .IsEqualTo(new SutClass(2, 42, 4, new object()));
            }).Throws<FluentCheckException>();
        }

        [Test]
        public void FailForDifferentPublicFields()
        {
            var sut = new SutClass(2, 42);

            Check.ThatCode(() =>
                    Check.That(sut).Considering().Public.Fields.IsEqualTo(new SutClass(3, 42)))
                .Throws<FluentCheckException>();
        }

        [Test]
        public void FailForDifferentPublicProperties()
        {
            var sut = new SutClass(2, 42);

            Check.ThatCode(() =>
                    Check.That(sut).Considering().Public.Properties.IsEqualTo(new SutClass(2, 43)))
                .Throws<FluentCheckException>();
        }

        [Test]
        public void FailOnNullForAllMembers()
        {
            var sut = new SutClass(2, 42, 4, null);

            var expected = new SutClass(2, 42, 4, sut);
            Check.ThatCode(() => Check.That(sut).Considering().All.Fields.And.All.Properties.IsEqualTo(expected));
            Check.ThatCode(() => Check.That(expected).Considering().All.Fields.And.All.Properties.IsEqualTo(sut));
        }

        [Test]
        public void FailWhenMissingMember()
        {
            var sut = new SutClass(2, 42);
            var expected = new {TheProperty = 12, Test = 11};
            Check.ThatCode(() =>
            {
                Check.That(sut).Considering().Public.Properties.IsInstanceOfType(expected.GetType());
            }).Throws<FluentCheckException>();
            Check.ThatCode(() =>
            {
                Check.That(expected).Considering().Public.Properties.IsInstanceOfType(sut.GetType());
            }).Throws<FluentCheckException>();
        }

        [Test]
        public void WorkForAllMembers()
        {
            var sut = new SutClass(2, 42, 4, null);

            Check.That(sut).Considering().All.Fields.And.All.Properties.IsEqualTo(new SutClass(2, 42, 4, null));
        }

        [Test]
        public void WorkForIdenticalPublicFields()
        {
            var sut = new SutClass(2, 42);

            Check.That(sut).Considering().Public.Fields.IsEqualTo(new SutClass(2, 42));
        }

        [Test]
        public void ShouldWorkForIdenticalPublicFieldsAndDifferentProperties()
        {
            var sut = new SutClass(2, 42);

            Check.That(sut).Considering().Public.Fields.IsEqualTo(new SutClass(2, 43));
        }


        [Test]
        public void WorkForIdenticalPublicProperties()
        {
            var sut = new SutClass(2, 42);

            Check.That(sut).Considering().Public.Properties.IsEqualTo(new SutClass(1, 42));
        }

        [Test]
        public void WorkForIsInstanceOf()
        {
            var sut = new SutClass(2, 42);
            var expected = new {TheProperty = 12};
            Check.That(expected).Considering().Public.Properties.IsInstanceOf<SutClass>();
        }

        [Test]
        public void ShouldWorkForIsInstanceOfType()
        {
            var sut = new SutClass(2, 42);
            var expected = new {TheProperty = 12};
            Check.That(sut).Considering().Public.Properties.IsInstanceOfType(expected.GetType());
            Check.That(expected).Considering().Public.Properties.IsInstanceOfType(sut.GetType());
        }

        [Test]
        public void WorkForNull()
        {
            var sut = new SutClass(2, 42);
            Check.ThatCode(() => { Check.That(sut).Considering().Public.Fields.Equals(null); });
        }

        [Test]
        public void WorkForOtherChecks()
        {
            var sut = new SutClass(2, 42);
            Check.That(sut).Considering().Public.Fields.Equals(new SutClass(2, 42));
            Check.That(sut).Considering().All.Fields.Not.Equals(new SutClass(2, 42));
        }

        [Test]
        public void WorkForPrivateFields()
        {
            var sut = new SutClass(2, 42, 4, null);

            Check.That(sut).Considering().NonPublic.Fields.IsEqualTo(new SutClass(2, 42, 4, null));
            Check.ThatCode(() =>
                {
                    Check.That(sut).Considering().NonPublic.Fields.IsEqualTo(new SutClass(2, 42, 4, sut));})
                .Throws<FluentCheckException>();
            Check.ThatCode(() =>
                {
                    Check.That(new SutClass(2, 42, 4, sut)).Considering().NonPublic.Fields.IsEqualTo(sut);})
                .Throws<FluentCheckException>();
        }

        [Test]
        public void WorkWithDeepExclusions()
        {
            var sut = new SutClass(2, 42, 4, new SutClass(1, 2));

            Check.That(sut).Considering().All.Fields
                .Excluding("ThePrivateProperty.thePrivateField", "ThePrivateProperty.TheField")
                .IsEqualTo(new SutClass(2, 42, 4, new SutClass(2, 2)));
        }

        [Test]
        public void WorkWithExclusions()
        {
            var sut = new SutClass(2, 42, 3, null);

            Check.That(sut).Considering().All.Fields.Excluding("thePrivateField")
                .IsEqualTo(new SutClass(2, 42, 4, null));
        }

        [Test]
        public void WorkOnDifferentArray()
        {
            var sut = new {arrayOfInts = new int[4]};
            var expected = new {arrayOfInts = new int[4]};
             Check.That(sut).Considering().NonPublic.Fields.IsEqualTo(expected);
        }

        [Test]
        public void FailOnDifferentArray()
        {
            var sut = new {arrayOfInts = new int[4]};
            var expected = new {arrayOfInts = new int[5]};
            Check.ThatCode(() => { Check.That(sut).Considering().NonPublic.Fields.IsEqualTo(expected); })
                .Throws<FluentCheckException>();
            Check.ThatCode(() => { Check.That(new {arrayOfInts =  "INTS"}).Considering().NonPublic.Fields.IsEqualTo(expected); })
                .Throws<FluentCheckException>();
        }

        [Test]
        public void
        WorkForHasSameValue()
        {
            var sut = new SutClass(12, 13);
            Check.That(sut).Considering().Public.Properties.HasSameValueAs(new SutClass(11, 13));
            Check.ThatCode(() =>
                {
                    Check.That(sut).Considering().Public.Properties.HasSameValueAs(new SutClass(11, 12));
                }).Throws<FluentCheckException>();
        }

        [Test]
        public void
        WorkForHasNotSameValue()
        {
            var sut = new SutClass(12, 13);
            Check.That(sut).Considering().Public.Properties.HasDifferentValueThan(new SutClass(11, 12));
        }

        [Test]
        public void
            WorkFor()
        {
            var sut = new SutClass(12, 13);
            Check.That(sut).Considering().Public.Properties.IsOneOf(new {TheProperty = 12}, new {TheProperty = 13});
        }

        // GH #219
        public class Parent
        {
            public virtual string AutoProperty { get; set; }
        }

        public class Child : Parent
        {
            public override string AutoProperty { get; set; }
        }

        [Test]
        public void HandleOverringForFields()
        {
            // Arrange
            var autoPropertyValue = "I am a test.";
            var childOne = new Child { AutoProperty = autoPropertyValue };

            // Act
            var childTwo = new Child { AutoProperty = autoPropertyValue };

            // Assert
            Check.That(childOne).HasFieldsWithSameValues(childTwo);
        }

        [Test]
        public void HandleOverringForProperties()
        {
            // Arrange
            var autoPropertyValue = "I am a test.";
            var childOne = new Child { AutoProperty = autoPropertyValue };

            // Act
            var childTwo = new Child { AutoProperty = autoPropertyValue };

            // Assert
            Check.That(childOne).Considering().Public.Properties.IsEqualTo(childTwo);
        }

    }
}