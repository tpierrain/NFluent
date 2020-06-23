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
    using Extensibility;
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

            internal object ThePrivateProperty { get; }
            public int TheProperty { get; }
            
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
        }

        private enum SutEnum
        {
            First,
            Second
        }

        [Test]
        public void TreatEnumAsPrimitiveValues()
        {
            Check.That(new {Prop = SutEnum.First}).Considering().Public.Properties.IsEqualTo(new {Prop = SutEnum.First});
            Check.ThatCode( ()=>
            Check.That(new {Prop = SutEnum.First}).Considering().Public.Properties.IsEqualTo(new {Prop = SutEnum.Second})).IsAFailingCheck();
            // Considering must not see the enum underlying field
            Check.That(new {Prop = SutEnum.First}).Considering().Public.Fields.IsEqualTo(new {Prop = SutEnum.Second});
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
            }).IsAFailingCheckWithMessage(
                    "", 
                    Criteria.FromRegEx("The checked value is an instance of \\[<>f__AnonymousType\\d<int>\\] whereas it must not\\."), 
                    "The checked value:", 
                    "\t[{ TheProperty = 42 }] of type: [NFluent.Helpers.ReflectionWrapper]", 
                    "The expected value: different from", 
                    Criteria.FromRegEx("\tan instance of \\[<>f__AnonymousType\\d<int>\\]"));
        }

        [Test]
        public void DetectDifferentProperties()
        {
            Check.ThatCode(() =>
                    Check.That(new SutClass(2, 43)).Considering().NonPublic.Properties
                        .IsEqualTo(new {ThePrivateProperty = (object) null}))
                .IsAFailingCheckWithMessage("", 
                    "The checked value's property 'ThePrivateProperty' is absent from the expected one.", 
                    "The checked value's property 'ThePrivateProperty':", 
                    "\t[null] of type: [object]");
            Check.ThatCode(() =>
                    Check.That(new {ThePrivateProperty = (object) null}).Considering().NonPublic.Properties
                        .IsEqualTo(new SutClass(2, 43)))
                .IsAFailingCheckWithMessage("", 
                    "The expected value's property 'ThePrivateProperty''s is absent from the checked one.", 
                    "The expected value's property 'ThePrivateProperty':", 
                    "\t[null] of type: [object]");
        }

        [Test]
        public void HandleDifferenceInType()
        {
            Check.ThatCode(() => 
            Check.That(new object()).Considering().All.Properties.IsEqualTo(new {child = new EmptyAncestor()})
            ).IsAFailingCheckWithMessage("", 
                "The expected value's property 'child''s is absent from the checked one.", 
                "The expected value's property 'child':", 
                "\t[] of type: [NFluent.Tests.ConsideringShould+EmptyAncestor]");
            Check.ThatCode(() => 
            Check.That(new {child = new EmptyAncestor()}).Considering().All.Properties.IsEqualTo(new int())
            ).IsAFailingCheckWithMessage("", 
                "The checked value's property 'child' is absent from the expected one.", 
                "The checked value's property 'child':", 
                "\t[] of type: [NFluent.Tests.ConsideringShould+EmptyAncestor]");
            Check.ThatCode(() => 
            Check.That(new {child = new EmptyChild()}).Considering().All.Properties.IsEqualTo(new {child = new EmptyAncestor()})
            ).IsAFailingCheckWithMessage("", 
                "The checked value's property 'child' does not have the expected value.", 
                "The checked value's property 'child':", 
                "\t[] of type: [NFluent.Tests.ConsideringShould+EmptyChild]", 
                "The expected value's property 'child':", 
                "\t[] of type: [NFluent.Tests.ConsideringShould+EmptyAncestor]");
        }

        [Test]
        public void DetectMissingProperties()
        {
            Check.ThatCode(() =>
                    Check.That(new SutClass(2, 43)).Considering().All.Properties
                        .IsEqualTo(new {ThePrivateProperty = (object) null}))
                .IsAFailingCheckWithMessage("", 
                    "The checked value's property 'TheProperty' is absent from the expected one.", 
                    "The checked value's property 'TheProperty':", "\t[43] of type: [int]");
            Check.ThatCode(() =>
                    Check.That(new {ThePrivateProperty = (object) null}).Considering().All.Properties
                        .IsEqualTo(new SutClass(2, 43)))
                .IsAFailingCheckWithMessage("", 
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
            }).IsAFailingCheckWithMessage("",  
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
                    Check.That(sut).Considering().Public.Fields.And.All.Properties.IsEqualTo(new SutClass(3, 42)))
                .IsAFailingCheckWithMessage("", 
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

            Check.That(sut).Considering().Public.Properties.IsEqualTo(new SutClass(3, 42));

            Check.ThatCode(() =>
                    Check.That(sut).Considering().Public.Properties.IsEqualTo(new SutClass(3, 43)))
                .IsAFailingCheckWithMessage("", 
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
                .IsAFailingCheckWithMessage("", 
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
                .IsAFailingCheckWithMessage("", 
                    "The checked value's property 'ThePrivateProperty' does not have the expected value.", 
                    "The checked value's property 'ThePrivateProperty':", 
                    "\t[null] of type: [object]", 
                    "The expected value's property 'ThePrivateProperty':", 
                    "\t[NFluent.Tests.ConsideringShould+SutClass] of type: [NFluent.Tests.ConsideringShould+SutClass]");
            Check.ThatCode(() => Check.That(expected).Considering().All.Fields.And.All.Properties.IsEqualTo(sut))
                .IsAFailingCheckWithMessage("", 
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
            }).IsAFailingCheckWithMessage("", 
                "The expected value's property 'Test' is absent from the checked one.", 
                "The expected value's property 'Test':", 
                "\t[null]");
            Check.ThatCode(() =>
            {
                Check.That(expected).Considering().Public.Properties.IsInstanceOfType(sut.GetType());
            }).IsAFailingCheckWithMessage("", 
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
            }).IsAFailingCheckWithMessage("",  "The checked value's property 'TheProperty' is of a different type than the expected one.",  
                "The checked value's property 'TheProperty':",  
                "\t[42] of type: [int]", 
                "The expected value's property 'TheProperty':", 
                "\tan instance of [string]");
        }

        [Test]
        public void WorkForAllMembers()
        {
            var sut = new SutClass(2, 42, 4, null);

            Check.That(sut).Considering().All.Fields.And.All.Properties.IsEqualTo(new SutClass(2, 42, 4, null));
        }

        [Test]
        public void SupportDuplicateCriterias()
        {
            var sut = new SutClass(2, 42, 4, null);

            Check.That(sut).Considering().All.Fields.And.All.Fields.IsEqualTo(new SutClass(2, 42, 4, null));
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
            Check.That((object)null).Considering().Public.Fields.Equals(null);
        }

        [Test]
        public void WorkForOtherChecks()
        {
            var sut = new SutClass(2, 42);
            Check.That(sut).Considering().Public.Properties.IsEqualTo(new SutClass(3, 42));

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
                .IsAFailingCheckWithMessage("", 
                    "The checked value's autoproperty 'ThePrivateProperty' (field '<ThePrivateProperty>k__BackingField') does not have the expected value.", 
                    "The checked value's autoproperty 'ThePrivateProperty' (field '<ThePrivateProperty>k__BackingField'):",
                    "\t[null] of type: [object]", 
                    "The expected value's autoproperty 'ThePrivateProperty' (field '<ThePrivateProperty>k__BackingField'):", 
                    "\t[NFluent.Tests.ConsideringShould+SutClass] of type: [NFluent.Tests.ConsideringShould+SutClass]");
            Check.ThatCode(() =>
                {
                    Check.That(new SutClass(2, 42, 4, sut)).Considering().NonPublic.Fields.IsEqualTo(sut);})
                .IsAFailingCheckWithMessage("", 
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
            Check.That(sut).Considering().Public.Fields.And.NonPublic.Properties.IsEqualTo(new SutClass(2, 0, 0, null));
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
                .IsAFailingCheckWithMessage(
                "",
                "The checked value's field 'arrayOfInts' does not have the expected value.",
                "The checked value's field 'arrayOfInts':",
                "\t{0, 0, 0, 0} (4 items)", 
                "The expected value's field 'arrayOfInts':",
                 "\t{0, 0, 0, 0, 0} (5 items)");
            
            Check.ThatCode(() => { Check.That(new {arrayOfInts =  "INTS"}).Considering().NonPublic.Fields.IsEqualTo(expected); })
                .IsAFailingCheckWithMessage("", 
                    "The checked value's field 'arrayOfInts' does not have the expected value.", 
                    "The checked value's field 'arrayOfInts':", 
                    "\t[\"INTS\"] of type: [string]", 
                    "The expected value's field 'arrayOfInts':", 
                    "\t{0, 0, 0, 0, 0} (5 items) of type: [int[]]");
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
                }).IsAFailingCheckWithMessage("", 
                "The checked value's property 'TheProperty' does not have the expected value.", 
                "The checked value's property 'TheProperty':", 
                "\t[13]", 
                "The expected value's property 'TheProperty':", 
                "\t[12]");
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
            }).IsAFailingCheckWithMessage("", 
                "The checked value is equal to none of the expected object whereas it should.", 
                "The checked value:", 
                "\t[{ TheProperty = 13 }]", 
                "The expected object: one of these", 
                "\t{{ TheProperty = 12 }, { TheProperty = 14 }} (2 items)");
            Check.ThatCode(()=>
            {
                Check.That(sut).Considering().Public.Properties.IsOneOf();
            }).IsAFailingCheckWithMessage("", 
                "The checked value is equal to none of the expected object whereas it should.", 
                "The checked value:", "\t[{ TheProperty = 13 }]", 
                "The expected object: one of these", 
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
            }).IsAFailingCheckWithMessage("", 
                "The checked value is equal to one of the given value(s) whereas it should not.", 
                "The checked value:", "\t[{ TheProperty = 13 }]", 
                "The expected object: none of these", 
                "\t{{ TheProperty = 12 }, { TheProperty = 13 }} (2 items)");
        }

        [Test]
        public void
            WorkForIsNull()
        {
            var sut = new SutClass(5, 7);
            Check.That(sut).Considering().NonPublic.Properties.IsNull();
            Check.That((SutClass)null).Considering().NonPublic.Properties.IsNull();
            Check.ThatCode(() => { Check.That(sut).Considering().NonPublic.Properties.Not.IsNull(); })
                .IsAFailingCheckWithMessage("", "The checked value has only null member, whereas it should not.");
            Check.ThatCode(() => { Check.That(sut).Considering().Public.Properties.IsNull();})
                .IsAFailingCheckWithMessage("", 
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
                IsAFailingCheckWithMessage("", 
                    "The checked value has a non null member, whereas it should not.", 
                    "The checked value:", 
                    "\t[{ TheProperty = 7 }]");
            Check.ThatCode(() => { Check.That(sut).Considering().NonPublic.Properties.IsNotNull();})
                .IsAFailingCheckWithMessage("",  
                    "The checked value's property 'ThePrivateProperty' is null, whereas it should not.");
            Check.ThatCode(() => { Check.That((SutClass)null).Considering().NonPublic.Properties.IsNotNull();})
                .IsAFailingCheckWithMessage("",  
                    "The checked value's property 'ThePrivateProperty' is null, whereas it should not.");
        }

        [Test]
        public void RaisesOnIsNotNullWithDeepNull()
        {
            Check.ThatCode(() => { Check.That(new {x = new {y = new {z = (object) null}}}).Considering().Public.Properties.IsNotNull();})
                .IsAFailingCheckWithMessage("",  
            "The checked value's property 'x.y.z' is null, whereas it should not.");

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
            ).IsAFailingCheckWithMessage("",
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
            }).IsAFailingCheckWithMessage("", 
                "The checked value's property 'Property' does not reference the expected one.", 
                "The checked value's property 'Property':", 
                "\t[System.Object]", 
                "The expected value's property 'Property':",
                "\t[null]");

            Check.ThatCode(() =>
            {
                Check.That(new {Properties = (object) null}).Considering().Public.Properties
                    .IsSameReferenceAs(new {Property = sharedReference, Properties = (object) null});
            }).IsAFailingCheckWithMessage("", 
                "The expected value's property 'Property' is absent from the checked one.", 
                "The expected value's property 'Property':", 
                "\t[System.Object]");

            Check.ThatCode(() =>
            {
                Check.That(new {Property = sharedReference, Properties = 2}).Considering().Public.Properties
                    .IsSameReferenceAs(new {Property = sharedReference});
            }).IsAFailingCheckWithMessage("", 
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
            }).IsAFailingCheckWithMessage(
                "", 
                "The checked value's property 'Property' does reference the given one, whereas it should not.", 
                "The expected value's property 'Property': different instance than", 
                "\t[System.Object]");

            Check.ThatCode(() =>
            {
                Check.That(new {Properties = (object) null}).Considering().Public.Properties
                    .IsDistinctFrom(new {Property = sharedReference, Properties = (object) null});
            }).IsAFailingCheckWithMessage("", 
                "The expected value's property 'Property' is absent from the checked one.",
                "The expected value's property 'Property':",
                "\t[System.Object]");

            Check.ThatCode(() =>
            {
                Check.That(new {Property = sharedReference, Properties = 2}).Considering().Public.Properties
                    .IsDistinctFrom(new {Property = new object()});
            }).IsAFailingCheckWithMessage("", 
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
            }).IsAFailingCheckWithMessage("", 
                "The checked value does not contain the same reference than the given one, whereas it should.", 
                "The checked value:", 
                "\t[{ Property = {  } }]", 
                "The expected value: same as", 
                "\t[{ Property = System.Object }]");
        }

        private class Recurse
        {
            private Recurse me;
            private readonly int x = 2;

            public Recurse()
            {
                this.me = this;
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
        public void HandleOverridingForFields()
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
        public void HandleOverridingForProperties()
        {
            // Arrange
            var autoPropertyValue = "I am a test.";
            var childOne = new Child { AutoProperty = autoPropertyValue };

            // Act
            var childTwo = new Child { AutoProperty = autoPropertyValue };

            // Assert
            Check.That(childOne).Considering().Public.Properties.IsEqualTo(childTwo);
        }

        [Test]
        public void HandleIsEqualProperly()
        {
            Check.That(new { field = MockEqual.True()}).Considering().All.Fields.IsEqualTo(new { field = MockEqual.True()});
            Check.That(new { field = MockEqual.True()}).Considering().All.Fields.Equals(new { field = MockEqual.True()});
        }
        
        [Test]
        public void HandleEqualSupportForField()
        {
            Check.That(new SutClass(1,2,3,new object())).Considering().All.Fields.Equals(new SutClass(1,2,3, MockEqual.True()));
        }
        
        private class EmptyAncestor
        {
            public override string ToString()
            {
                return string.Empty;
            }
        }

        private class EmptyChild : EmptyAncestor
        {}

        private class MockEqual
        {
            protected bool Equals(MockEqual other)
            {
                return this.isEqual;
            }

            public override bool Equals(object obj)
            {
                return this.isEqual;
            }

            public override int GetHashCode()
            {
                return this.isEqual.GetHashCode();
            }

            private bool isEqual;
            private int count;

            private static int counter;
            private MockEqual(bool isEqual)
            {
                this.isEqual = isEqual;
                this.count = counter++;
            }

            public static MockEqual True()
            {
                return new MockEqual(true);
            }

            public static MockEqual False()
            {
                return new MockEqual(false);
            }
        }
    }
    internal static class Extractor
    {
        public static ICheckLink<ICheck<ReflectionWrapper>> HasHashCode(this ICheck<ReflectionWrapper> wrapper, int val)
        {
            ExtensibilityHelper.BeginCheck(wrapper).
                CheckSutAttributes(x=> x.GetHashCode(), "hashcode").
                FailWhen(x => x != val,
                    "The {0} does not have the expected value.").
                OnNegate("The {0} should not have the given hashcode.").
                DefineExpectedValue(val).
                EndCheck();

            return ExtensibilityHelper.BuildCheckLink(wrapper);
        }
    }

}