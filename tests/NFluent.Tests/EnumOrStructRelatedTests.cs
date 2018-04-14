// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EnumOrStructRelatedTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN
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
using System;


namespace NFluent.Tests
{
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class EnumOrStructRelatedTests
    {
        private const Nationality FrenchNationality = Nationality.French;

        [Test]
        public void IsEqualToWorksWithEnum()
        {
            Check.ThatEnum(FrenchNationality).IsEqualTo(Nationality.French);
            Check.That(FrenchNationality).IsEqualTo(Nationality.French);
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenFailingWithEnum()
        {

            Check.ThatCode(() => { Check.ThatEnum(FrenchNationality).IsEqualTo(Nationality.American); })
                .IsAFaillingCheckWithMessage("",
                    "The checked enum is different from the expected one.",
                    "The checked enum:",
                    "\t[French]",
                    "The expected enum:",
                    "\t[American]");
        }

        private struct Basic
        {
            public int x;
            public int y;

            public Basic(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public override int GetHashCode()
            {
                return this.x + this.y;
            }
        }

        [Test]
        public void ShouldFailWithHashesWhenSimilar()
        {
            Check.ThatCode(() => { Check.ThatEnum(new Basic(1,2)).IsEqualTo(new Basic(1, 3)); })
                .IsAFaillingCheckWithMessage("",
                    "The checked struct is different from the expected one.",
                    "The checked struct:",
                    "\t[NFluent.Tests.EnumOrStructRelatedTests+Basic] with HashCode: [3]",
                    "The expected struct:",
                    "\t[NFluent.Tests.EnumOrStructRelatedTests+Basic] with HashCode: [4]");
        }

        [Test]
        public void IsNotEqualToWorksWithEnum()
        {
            Check.ThatEnum(FrenchNationality).IsNotEqualTo(Nationality.Korean);
        }

        [Test]
        public void IsNotEqualToThrowsExceptionWhenFailingWithEnum()
        {
            Check.ThatCode(() =>
            {
                Check.ThatEnum(FrenchNationality).IsNotEqualTo(Nationality.French);
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked enum is equal to the expected one whereas it must not.",
                    "The expected enum: different from",
                    "\t[French] of type: [NFluent.Tests.Nationality]");
        }

        [Test]
        public void NotOperatorWorksOnIsEqualToForEnum()
        {
            Check.ThatEnum(FrenchNationality).Not.IsEqualTo(Nationality.American);
        }

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailingWithEnum()
        {
            Check.ThatCode(() =>
            {
                Check.ThatEnum(FrenchNationality).Not.IsEqualTo(Nationality.French);
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked enum is equal to the expected one whereas it must not.", 
                    "The expected enum: different from",
                    "\t[French] of type: [NFluent.Tests.Nationality]");
        }

        [Test]
        public void NotOperatorWorksOnIsNotEqualToForEnum()
        {
            Check.ThatEnum(FrenchNationality).Not.IsNotEqualTo(Nationality.French);
        }

        // TODO: write tests related to error message of IsNotEqualTo (cause the error message seems not so good)
        [Test]
        public void IsEqualToWorksWithStruct()
        {
            var inLove = new Mood { Description = "In love", IsPositive = true };
            Check.ThatEnum(inLove).IsEqualTo(inLove);
        }

        [Test]
        public void IsInstanceOfWorks()
        {
            Check.ThatEnum(FrenchNationality).IsInstanceOf<Nationality>();
        }

        [Test]
        public void IsNotInstanceOfWorks()
        {
            Check.ThatEnum(FrenchNationality).IsNotInstanceOf<int>();
        }

        [Test]
        public void IsInstanceOfFailsPropery()
        {
            Check.ThatCode(() =>
            {
                Check.ThatEnum(FrenchNationality).IsInstanceOf<int>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is not an instance of the expected type." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[French] of type: [NFluent.Tests.Nationality]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\tan instance of type: [int]");
        }

        [Test]
        public void IsNotInstanceOfFailsProperly()
        {
            Check.ThatCode(() =>
            {
                Check.ThatEnum(FrenchNationality).IsNotInstanceOf<Nationality>();
            })
            .Throws<FluentCheckException>()
            .WithMessage(Environment.NewLine+ "The checked value is an instance of [NFluent.Tests.Nationality] whereas it must not." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\t[French] of type: [NFluent.Tests.Nationality]" + Environment.NewLine + "The expected value: different from" + Environment.NewLine + "\tan instance of type: [NFluent.Tests.Nationality]");
        }

        [Flags]
        private enum Flags
        {
            None = 0,
            First = 1,
            Second = 2,
            Third = 4
        }

        [Test]
        public void HasFlagWorkAsExpected()
        {
            Check.That(Flags.First).HasFlag(Flags.First);
        }

        [Test]
        public void DoesNotHaveFlagWorkAsExpected()
        {
            Check.That(Flags.Second).DoesNotHaveFlag(Flags.First);
        }

        [Test]
        public void HasFlagFailsWhenRelevant()
        {
            Check.ThatCode( ()=>  Check.That(Flags.Second|Flags.Third).HasFlag(Flags.First))
                .IsAFaillingCheckWithMessage("", 
                    "The checked enum does not have the expected flag.", 
                    "The checked enum:", 
                    "\t[Second, Third]", 
                    "The expected enum:", 
                    "\t[First]");
        }

        [Test]
        public void DoesNotHaveFlagFailsWhenRelevant()
        {
            Check.ThatCode( ()=>  Check.That(Flags.First|Flags.Third).DoesNotHaveFlag(Flags.First))
                .IsAFaillingCheckWithMessage("", 
                    "The checked enum does have the expected flag, whereas it should not.", 
                    "The checked enum:", 
                    "\t[First, Third]", 
                    "The expected enum:", 
                    "\t[First]");
        }
        
        [Test]
        public void HasFlagFailsProperlyWhenNegated()
        {
            Check.ThatCode( ()=>  Check.That(Flags.First|Flags.Third).Not.HasFlag(Flags.First))
                .IsAFaillingCheckWithMessage("", 
                    "The checked enum does have the expected flag, whereas it should not.", 
                    "The checked enum:", 
                    "\t[First, Third]", 
                    "The expected enum:", 
                    "\t[First]");
        }

        [Test]
        public void DoesNotHasFlagFailsProperlyWhenNegated()
        {
            Check.ThatCode( ()=>  Check.That(Flags.Second|Flags.Third).Not.DoesNotHaveFlag(Flags.First))
                .IsAFaillingCheckWithMessage("", 
                    "The checked enum does not have the expected flag.", 
                    "The checked enum:", 
                    "\t[Second, Third]", 
                    "The expected enum:", 
                    "\t[First]");
        }

        [Test]
        public void FailsIfNotFlags()
        {
            Check.ThatCode( ()=>  Check.That(Nationality.American).HasFlag(Nationality.French))
                .IsAFaillingCheckWithMessage("", 
                    "The checked enum type is not a set of flags. You must add [Flags] attribute to its declaration.", 
                    "The checked enum:", 
                    "\t[American]", 
                    "The expected enum:", 
                    "\t[French]");
        }
 
        [Test]
        public void FailsIfCheckOn0()
        {
            Check.ThatCode( ()=>  Check.That(Flags.First).HasFlag(Flags.None))
                .IsAFaillingCheckWithMessage("", 
                    "Wrong chek: The expected flag is 0. You must use IsEqualTo, or a non zero flag value.", 
                    "The checked enum:", 
                    "\t[First]", 
                    "The expected enum:", 
                    "\t[None]");
        }
    }
}
