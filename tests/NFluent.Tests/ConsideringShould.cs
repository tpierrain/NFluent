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
    using Helpers;
    using NFluent.Helpers;
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
                Check.That(sut).Considering().Properties.IsNoInstanceOfType(expected.GetType());
            }).IsAFaillingCheckWithMessage(
                    "", 
                    "The checked value is an instance of [<>f__AnonymousType1<int>] whereas it must not.", 
                    "The checked value:", 
                    "\t[{ TheProperty = 42 }] of type: [NFluent.Helpers.ReflectionWrapper]", 
                    "The expected value: different from", 
                "\tan instance of type: [<>f__AnonymousType1<int>]");
        }

        [Test]
        public void DetectDifferentProperties()
        {
            Check.ThatCode(() =>
                    Check.That(new SutClass(2, 43)).Considering().NonPublic.Properties
                        .IsEqualTo(new {ThePrivateProperty = (object) null}))
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's property 'ThePrivateProperty' is absent from the expected one.", 
                    "The checked value's property 'ThePrivateProperty':", 
                    "\t[null] of type: [object]");
            Check.ThatCode(() =>
                    Check.That(new {ThePrivateProperty = (object) null}).Considering().NonPublic.Properties
                        .IsEqualTo(new SutClass(2, 43)))
                .IsAFaillingCheckWithMessage("", 
                    "The expected value's property 'ThePrivateProperty''s is absent from the checked one.", 
                    "The expected value's property 'ThePrivateProperty':", 
                    "\t[null] of type: [object]");
        }

        [Test]
        public void DetectMissingProperties()
        {
            Check.ThatCode(() =>
                    Check.That(new SutClass(2, 43)).Considering().All.Properties
                        .IsEqualTo(new {ThePrivateProperty = (object) null}))
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's property 'TheProperty' is absent from the expected one.", 
                    "The checked value's property 'TheProperty':", "\t[43] of type: [int]");
            Check.ThatCode(() =>
                    Check.That(new {ThePrivateProperty = (object) null}).Considering().All.Properties
                        .IsEqualTo(new SutClass(2, 43)))
                .IsAFaillingCheckWithMessage("", 
                    "The expected value's property 'TheProperty''s is absent from the checked one.",
                    "The expected value's property 'TheProperty':",
                    "\t[43] of type: [int]");
        }

        [Test]
        public void FailForAllMembers()
        {
            var sut = new SutClass(2, 42, 4, null);
            Check.ThatCode(() =>
            {
                Check.That(sut).Considering().All.Fields.And.All.Properties
                    .IsEqualTo(new SutClass(2, 42, 4, new object()));
            }).IsAFaillingCheckWithMessage("",  
                "The checked value's property 'ThePrivateProperty' does not have the expected value.", 
                "The checked value's property 'ThePrivateProperty':", 
                "\t[null]", 
                "The expected value's property 'ThePrivateProperty':", 
                "\t[System.Object]");
        }

        [Test]
        public void FailForDifferentPublicFields()
        {
            var sut = new SutClass(2, 42);

            Check.ThatCode(() =>
                    Check.That(sut).Considering().Public.Fields.IsEqualTo(new SutClass(3, 42)))
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's field 'TheField' does not have the expected value.",
                    "The checked value's field 'TheField':", 
                    "\t[2]", 
                    "The expected value's field 'TheField':", 
                    "\t[3]");
        }

        [Test]
        public void FailForDifferentPublicProperties()
        {
            var sut = new SutClass(2, 42);

            Check.ThatCode(() =>
                    Check.That(sut).Considering().Public.Properties.IsEqualTo(new SutClass(2, 43)))
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's property 'TheProperty' does not have the expected value.", 
                    "The checked value's property 'TheProperty':",
                    "\t[42]", 
                    "The expected value's property 'TheProperty':", 
                    "\t[43]");

        }
        [Test]
        public void FailForDifferentNonPublicProperties()
        {
            var sut = new SutClass(2, 42, 1, 3);

            Check.ThatCode(() =>
                    Check.That(sut).Considering().NonPublic.Properties.IsEqualTo(new SutClass(2, 43, 1, null)))
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's property 'ThePrivateProperty' does not have the expected value.", 
                    "The checked value's property 'ThePrivateProperty':",
                    "\t[3] of type: [int]", 
                    "The expected value's property 'ThePrivateProperty':", 
                    "\t[null] of type: [object]");
        }

        [Test]
        public void FailOnNullForAllMembers()
        {
            var sut = new SutClass(2, 42, 4, null);

            var expected = new SutClass(2, 42, 4, sut);
            Check.ThatCode(() => Check.That(sut).Considering().All.Fields.And.All.Properties.IsEqualTo(expected))
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's property 'ThePrivateProperty' does not have the expected value.", 
                    "The checked value's property 'ThePrivateProperty':", 
                    "\t[null] of type: [object]", 
                    "The expected value's property 'ThePrivateProperty':", 
                    "\t[NFluent.Tests.ConsideringShould+SutClass] of type: [NFluent.Tests.ConsideringShould+SutClass]");
            Check.ThatCode(() => Check.That(expected).Considering().All.Fields.And.All.Properties.IsEqualTo(sut))
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's property 'ThePrivateProperty' does not have the expected value.", 
                    "The checked value's property 'ThePrivateProperty':", 
                    "\t[NFluent.Tests.ConsideringShould+SutClass] of type: [NFluent.Tests.ConsideringShould+SutClass]",
                    "The expected value's property 'ThePrivateProperty':", 
                    "\t[null] of type: [object]");
        }

        [Test]
        public void FailWhenMissingMember()
        {
            var sut = new SutClass(2, 42);
            var expected = new {TheProperty = 12, Test = 11};
            Check.ThatCode(() =>
            {
                Check.That(sut).Considering().Public.Properties.IsInstanceOfType(expected.GetType());
            }).IsAFaillingCheckWithMessage("", 
                "The expected value's property 'Test' is absent from the checked one.", 
                "The expected value's property 'Test':", 
                "\t[null]");
            Check.ThatCode(() =>
            {
                Check.That(expected).Considering().Public.Properties.IsInstanceOfType(sut.GetType());
            }).IsAFaillingCheckWithMessage("", 
                "The checked value's property 'Test' is absent from the expected one.", 
                "The checked value's property 'Test':", 
                "\t[11]");
        }

        [Test]
        public void FailWhenDifferentType()
        {
            var sut = new SutClass(2, 42);
            var expected = new {TheProperty = "toto"};
            Check.ThatCode(() =>
            {
                Check.That(sut).Considering().Public.Properties.IsInstanceOfType(expected.GetType());
            }).IsAFaillingCheckWithMessage("",  "The checked value's property 'TheProperty' is of a different type than the expected one.",  
                "The checked value's property 'TheProperty':",  
                "\t[42] of type: [int]", 
                "The expected value's property 'TheProperty':", 
                "\tan instance of type: [string]");
        }

        [Test]
        public void WorkForAllMembers()
        {
            var sut = new SutClass(2, 42, 4, null);

            Check.That(sut).Considering().All.Fields.And.All.Properties.IsEqualTo(new SutClass(2, 42, 4, null));
        }

        [Test]
        public void WorkWhenDoubled()
        {
            var sut = new SutClass(2, 42, 4, null);

            Check.That(sut).Considering().All.Fields.And.All.Properties.Considering().All.Fields.And.All.Properties.IsEqualTo(new SutClass(2, 42, 4, null));
        }

        [Test]
        public void WorkForIdenticalPublicFields()
        {
            var sut = new SutClass(2, 42);

            Check.That(sut).Considering().Public.Fields.And.NonPublic.Properties.IsEqualTo(new SutClass(2, 42));
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
            Check.That(sut).Considering().Public.Fields.Equals(null);
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
                {Check.That(sut).Considering().NonPublic.Fields.IsEqualTo(new SutClass(2, 42, 4, sut));})
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's autoproperty 'ThePrivateProperty' (field '<ThePrivateProperty>k__BackingField') does not have the expected value.", 
                    "The checked value's autoproperty 'ThePrivateProperty' (field '<ThePrivateProperty>k__BackingField'):",
                    "\t[null] of type: [object]", 
                    "The expected value's autoproperty 'ThePrivateProperty' (field '<ThePrivateProperty>k__BackingField'):", 
                    "\t[NFluent.Tests.ConsideringShould+SutClass] of type: [NFluent.Tests.ConsideringShould+SutClass]");
            Check.ThatCode(() =>
                {
                    Check.That(new SutClass(2, 42, 4, sut)).Considering().NonPublic.Fields.IsEqualTo(sut);})
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's autoproperty 'ThePrivateProperty' (field '<ThePrivateProperty>k__BackingField') does not have the expected value.", 
                    "The checked value's autoproperty 'ThePrivateProperty' (field '<ThePrivateProperty>k__BackingField'):", 
                    "\t[NFluent.Tests.ConsideringShould+SutClass] of type: [NFluent.Tests.ConsideringShould+SutClass]", 
                    "The expected value's autoproperty 'ThePrivateProperty' (field '<ThePrivateProperty>k__BackingField'):", 
                    "\t[null] of type: [object]");
        }

        [Test]
        public void
            WorkForScopeCombinations()
        {
            var sut = new SutClass(2, 42, 4, null);

            Check.That(sut).Considering().NonPublic.Fields.And.Public.Properties.IsEqualTo(new SutClass(3, 42, 4, sut));
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
                .IsAFaillingCheckWithMessage(
                "",
                "The checked value's field 'arrayOfInts' does not have the expected value.",
                "The checked value's field 'arrayOfInts':",
                "\t{0, 0, 0, 0} (4 items)", 
                "The expected value's field 'arrayOfInts':",
                 "\t{0, 0, 0, 0, 0}");
            Check.ThatCode(() => { Check.That(new {arrayOfInts =  "INTS"}).Considering().NonPublic.Fields.IsEqualTo(expected); })
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's field 'arrayOfInts' does not have the expected value.", 
                    "The checked value's field 'arrayOfInts':", 
                    "\t[\"INTS\"] of type: [string]", 
                    "The expected value's field 'arrayOfInts':", 
                    "\t{0, 0, 0, 0, 0} of type: [int[]]");
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
                }).IsAFaillingCheckWithMessage("", "The checked value is different from the expected one.", "The checked value:", "\t[{ TheProperty = 13 }] of type: [NFluent.Helpers.ReflectionWrapper]", "The expected value: equals to (using operator==)", "\t[NFluent.Tests.ConsideringShould+SutClass] of type: [NFluent.Tests.ConsideringShould+SutClass]");
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
            WorkForIsOneOf()
        {
            var sut = new SutClass(12, 13);
            Check.That(sut).Considering().Public.Properties.IsOneOf(new {TheProperty = 12}, new {TheProperty = 13});
        }

        [Test]
        public void
            FailIfNoneOf()
        {
            var sut = new SutClass(12, 13);
            Check.ThatCode(()=>
            {
               Check.That(sut).Considering().Public.Properties.IsOneOf(new {TheProperty = 12}, new {TheProperty = 14});
            }).IsAFaillingCheckWithMessage("", 
                "The checked value is equal to none of the expected value(s) whereas it should.", 
                "The checked value:", 
                "\t[{ TheProperty = 13 }]", 
                "The expected value(s): one of", 
                "\t{{ TheProperty = 12 }, { TheProperty = 14 }} (2 items)");
            Check.ThatCode(()=>
            {
                Check.That(sut).Considering().Public.Properties.IsOneOf();
            }).IsAFaillingCheckWithMessage("", 
                "The checked value is equal to none of the expected value(s) whereas it should.", 
                "The checked value:", "\t[{ TheProperty = 13 }]", 
                "The expected value(s): one of", 
                "\t{} (0 item)");
        }

        [Test]
        public void
            FailIfNegatedOf()
        {
            var sut = new SutClass(12, 13);
            Check.ThatCode(()=>
            {
                Check.That(sut).Considering().Public.Properties.Not.IsOneOf(new {TheProperty = 12}, new {TheProperty = 13});
            }).IsAFaillingCheckWithMessage("", 
                "The checked value is equal to one of the given value(s) whereas it should not.", 
                "The checked value:", "\t[{ TheProperty = 13 }]", 
                "The expected value(s): none of", 
                "\t{{ TheProperty = 12 }, { TheProperty = 13 }} (2 items)");
        }

        [Test]
        public void
            WorkForIsNull()
        {
            var sut = new SutClass(5, 7);
            Check.That(sut).Considering().NonPublic.Properties.IsNull();
            Check.ThatCode(() => { Check.That(sut).Considering().NonPublic.Properties.Not.IsNull(); })
                .IsAFaillingCheckWithMessage("", "The checked value has only null member, whereas it should not.");
            Check.ThatCode(() => { Check.That(sut).Considering().Public.Properties.IsNull();})
                .IsAFaillingCheckWithMessage("", 
                    "The checked value's property 'TheProperty' is non null, whereas it should be.", 
                    "The checked value's property 'TheProperty':",
                    "\t[7]");
        }

        [Test]
        public void
            WorkForIsNotNull()
        {
            var sut = new SutClass(5, 7);
            Check.That(sut).Considering().Public.Properties.IsNotNull();
            Check.ThatCode(() => { Check.That(sut).Considering().Public.Properties.Not.IsNotNull(); }).
                IsAFaillingCheckWithMessage("", "The checked value has a non null member, whereas it should not.", "The checked value:", "\t[{ TheProperty = 7 }]");
            Check.ThatCode(() => { Check.That(sut).Considering().NonPublic.Properties.IsNotNull();})
                .IsAFaillingCheckWithMessage("",  "The checked value's property 'ThePrivateProperty' is null, whereas it should not.");
        }

        [Test]
        public void
            WorkForIsNotNullOnRecursive()
        {
            Check.That(new Recurse()).Considering().All.Fields.IsNotNull();
            Check.That(new Recurse().X).IsEqualTo(2);
        }

        [Test]
        public void
            SupportsRecursiveStructureInErrorMessages()
        {
            Check.ThatCode(() => { Check.That(new Recurse()).Considering().All.Fields.Not.IsEqualTo(new Recurse()); }
            ).IsAFaillingCheckWithMessage("",
                "The checked value is equal to one of expected one whereas it should not.",
                "The checked value:",
                "	[{ me = ..., x = 2 }]");
        }

        [Test]
        public void
            WorkForIsSameReference()
        {
            var sharedReference = new object();
 
            Check.That(new {Property = sharedReference}).Considering().Public.Properties
                .IsSameReferenceAs(new {Property = sharedReference});
            Check.ThatCode(() =>
            {
                Check.That(new {Property = sharedReference}).Considering().Public.Properties
                    .IsSameReferenceAs(new {Property = (object) null});
            }).IsAFaillingCheckWithMessage("", 
                "The checked value's property 'Property' does not reference the expected one.", 
                "The checked value's property 'Property':", 
                "\t[System.Object]", 
                "The expected value's property 'Property':",
                "\t[null]");

            Check.ThatCode(() =>
            {
                Check.That(new {Properties = (object) null}).Considering().Public.Properties
                    .IsSameReferenceAs(new {Property = sharedReference, Properties = (object) null});
            }).IsAFaillingCheckWithMessage("", 
                "The expected value's property 'Property' is absent from the checked one.", 
                "The expected value's property 'Property':", 
                "\t[System.Object]");

            Check.ThatCode(() =>
            {
                Check.That(new {Property = sharedReference, Properties = 2}).Considering().Public.Properties
                    .IsSameReferenceAs(new {Property = sharedReference});
            }).IsAFaillingCheckWithMessage("", 
                "The checked value's property 'Properties' is absent from the expected one.", 
                "The checked value's property 'Properties':", 
                "\t[2]");

        }

        [Test]
        public void
            WorkForIsDistinct()
        {
            var sharedReference = new object();

            Check.That(new {Property = sharedReference}).Considering().Public.Properties
                .IsDistinctFrom(new {Property = new object()});
            Check.ThatCode(() =>
            {
                Check.That(new {Property = sharedReference}).Considering().Public.Properties
                    .IsDistinctFrom(new {Property = sharedReference});
            }).IsAFaillingCheckWithMessage(
                "", 
                "The checked value's property 'Property' does reference the given one, whereas it should not.", 
                "The expected value's property 'Property': different instance than", 
                "\t[System.Object]");

            Check.ThatCode(() =>
            {
                Check.That(new {Properties = (object) null}).Considering().Public.Properties
                    .IsDistinctFrom(new {Property = sharedReference, Properties = (object) null});
            }).IsAFaillingCheckWithMessage("", 
                "The expected value's property 'Property' is absent from the checked one.",
                "The expected value's property 'Property':",
                "\t[System.Object]");

            Check.ThatCode(() =>
            {
                Check.That(new {Property = sharedReference, Properties = 2}).Considering().Public.Properties
                    .IsDistinctFrom(new {Property = new object()});
            }).IsAFaillingCheckWithMessage("", 
                "The checked value's property 'Properties' is absent from the expected one.", 
                "The checked value's property 'Properties':", 
                "\t[2]");
        }

        [Test]
        public void WorkWithNegatedIsDistinct()
        {
            var sharedReference = new object();

            Check.That(new {Property = sharedReference}).Considering().Public.Properties
                .Not.IsDistinctFrom(new {Property = sharedReference});
            Check.ThatCode(() =>
            {
                Check.That(new {Property = sharedReference}).Considering().Public.Properties
                    .Not.IsDistinctFrom(new {Property = new object()});
            }).IsAFaillingCheckWithMessage(
                "", 
                "The checked value contains the same reference than the expected one, whereas it should not.", 
                "The checked value:", 
                "\t[{ Property = {  } }]");


        }

        private class Recurse
        {
            private Recurse me;
            private readonly int x = 2;

            public Recurse()
            {
                this.me = this;
            }

            public Recurse(int x)
            {
                this.me = this;
                this.x = x;
            }

            public int X => this.x;
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