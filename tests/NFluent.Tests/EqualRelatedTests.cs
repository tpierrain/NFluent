// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="EqualRelatedTests.cs" company="">
// //   Copyright 2013 Thomas PIERRAIN, Cyrille DUPUYDAUBY
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
    using System;
    using System.Collections.Generic;
    using ApiChecks;
    using Helpers;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class EqualRelatedTests
    {
        #region IsEqualTo()

        [Test]
        public void LegacyMode()
        {
            var test = new List<string>();
            var test2 = new List<string>();

            Check.That(test).IsEqualTo(test2);

            var currentMode = Check.EqualMode;
            Check.EqualMode = EqualityMode.Equals;
            Check.ThatCode(() => Check.That(test).IsEqualTo(test2)).IsAFaillingCheck();
            Check.EqualMode = currentMode;
        }

        [Test]
        public void IsEqualToWorksWithBooleans()
        {
            const bool tddSucks = false;
            Check.That(tddSucks).IsNotEqualTo(true);
        }

        [Test]
        public void CanNegateIsEqualToWithBooleans()
        {
            const bool tddSucks = false;
            Check.That(tddSucks).Not.IsNotEqualTo(false);
        }

        [Test]
        public void IsEqualToWorksWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo("Son of a test");
        }
        [Test]
        public void IsEqualToWorksWithStringAndObject()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo((object)"Son of a test");
        }

        [Test]
        public void IsEqualToWorksWithArray()
        {
            var array = new[] { 45, 43, 54, 666 };
            var otherReference = array;

            Check.That(array).IsEqualTo(array);
            Check.That(array).IsEqualTo(otherReference);
        }

        [Test]
        public void IsEqualToWorksWithObject()
        {
            var heroe = new Person { Name = "Gandhi" };
            var otherReference = heroe;

            Check.That(heroe).IsEqualTo(otherReference);
        }

        [Test]
        public void CanNegateIsEqualToWithObject()
        {
            var heroe = new Person { Name = "Gandhi" };

            Check.That(heroe).Not.IsEqualTo(null);
        }

        [Test]
        public void NotIsEqualToWithObjectThrowsExceptionWhenFailing()
        {
            var heroe = new Person { Name = "Gandhi" };
            var otherReference = heroe;

            Check.ThatCode(() =>
            {
                Check.That(heroe).Not.IsEqualTo(otherReference);
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is equal to the expected one whereas it must not.",
                    "The expected value: different from",
                    "\t[Gandhi] of type: [NFluent.Tests.Person]");
        }

        [Test]
        public void IsEqualWorksWithIntNumbers()
        {
            int firstInt = 23;
            int secondButIdenticalInt = 23;

            Check.That(secondButIdenticalInt).IsEqualTo(firstInt);
        }

        [Test]
        public void IsEqualWorksWithDoubleNumbers()
        {
            double firstDouble = 23.7D;
            double secondButIdenticalDouble = 23.7D;

            Check.That(secondButIdenticalDouble).IsEqualTo(firstDouble);
        }

        [Test]
        public void IsEqualWorksWithFloatNumbers()
        {
            float firstFloat = 23.56F;
            float secondButIdenticalFloat = 23.56F;

            Check.That(secondButIdenticalFloat).IsEqualTo(firstFloat);
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenFailingWithIntArray()
        {
            var array = new[] { 45, 43, 54, 666 };
            var otherSimilarButNotEqualArray = new[] { 45, 43, 54, 667 };

            Check.ThatCode(() =>
            {
                Check.That(array).IsEqualTo(otherSimilarButNotEqualArray);
            })
            .IsAFaillingCheck();
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenFailingWithString()
        {
            var first = "Son of a test";

            Check.ThatCode(() =>
            {
                Check.That(first).IsEqualTo("no way");
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked string is different from expected one." + Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"Son of a test\"]" + Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"no way\"]");
        }

        [Test]
        public void NotIsEqualToThrowsExceptionWhenFailingWithString()
        {
            var first = "Son of a test";

            Check.ThatCode(() =>
            {
                Check.That(first).Not.IsEqualTo(first);
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked string is equal to the expected one whereas it must not." + Environment.NewLine + "The expected string: different from" + Environment.NewLine + "\t[\"Son of a test\"]");
        }

        [Test]
        public void IsEqualToThrowsExceptionWhenFailingWithObject()
        {
            var heroe = new Person { Name = "Gandhi" };
            var bastard = new Person { Name = "PolPot" };

            Check.ThatCode(() =>
            {
                Check.That(heroe).IsEqualTo(bastard);
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is different from the expected one.",
                    "The checked value:",
                    "\t[Gandhi]",
                    "The expected value:",
                    "\t[PolPot]");
        }

        [Test]
        public void IsEqualToThrowsProperExceptionEvenWithNullAsExpected()
        {
            var first = "Son of a test";

            Check.ThatCode(() =>
            {
                Check.That(first).IsEqualTo(null);
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked string is not null whereas it must.",
                    "The checked string:",
                    "\t[\"Son of a test\"]",
                    "The expected value:",
                    "\t[null]");
        }

        [Test]
        public void IsEqualToThrowsProperExceptionEvenWithNullAsValue()
        {

            Check.ThatCode(() =>
            {
                Check.That((string)null).IsEqualTo("Kamoulox !");
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked string is null whereas it must not." + Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[null]" + Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"Kamoulox !\"]");
        }

        [Test]
        public void WeCanSeeTheDifferenceBewteenTwoDifferentObjectsThatHaveTheSameToString()
        {
            Person dad = new Person { Name = "John" };
            Person son = new Child { Name = "John" };

            Check.ThatCode(() =>
            {
                Check.That(son).IsEqualTo(dad);
            })
            .IsAFaillingCheckWithMessage("",
                    "The checked value is different from the expected one.",
                    "The checked value:",
                    "\t[John] of type: [NFluent.Tests.Child]",
                    "The expected value:",
                    "\t[John] of type: [NFluent.Tests.Person]");
        }

        [Test]
        public void WeCanAlsoSeeTheDifferenceBetweenTwoDifferentInstancesOfTheSameTypeWhithIdenticalToString()
        {
            Person dad = new Person { Name = "John" };
            Person uncle = new Person { Name = "John" };

            Check.ThatCode(() =>
            {
                Check.That(uncle).IsEqualTo(dad);
            })
            .ThrowsAny().AndWhichMessage().Matches(Environment.NewLine+ "The checked value is different from the expected one." + Environment.NewLine + "The checked value:" + Environment.NewLine + "\\t\\[John\\] with HashCode: \\[.*\\]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\\t\\[John\\] with HashCode: \\[.*\\]");
        }

        #endregion

        #region IsNotEqualTo()

        [Test]
        public void IsNotEqualToWorksWithBooleans()
        {
            const bool tddSucks = false;
            Check.That(tddSucks).IsEqualTo(false);
        }

        [Test]
        public void CanNegateIsNotEqualToWithBooleans()
        {
            const bool tddSucks = false;
            Check.That(tddSucks).Not.IsEqualTo(true);
        }

        [Test]
        public void IsNotEqualToWorksWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsNotEqualTo("other text");
        }

        [Test]
        public void IsNotEqualToWorksWithStringAndObject()
        {
            var first = "Son of a test";
            Check.That(first).IsNotEqualTo((object)"other text");
        }

        [Test]
        public void IsNotEqualToWorksWithArray()
        {
            var array = new[] { 45, 43, 54, 666 };
            var otherArray = new[] { 666, 74 };

            Check.That(array).IsNotEqualTo(otherArray);
        }

        [Test]
        public void IsNotEqualToWorksObject()
        {
            var heroe = new Person { Name = "Gandhi" };
            var badGuy = new Person { Name = "Pol Pot" };

            Check.That(heroe).IsNotEqualTo(badGuy);
        }

        [Test]
        public void IsNotEqualWorksWithIntNumbers()
        {
            int firstInt = 23;
            int secondButIdenticalInt = 7;

            Check.That(secondButIdenticalInt).IsNotEqualTo(firstInt);
        }

        [Test]
        public void IsNotEqualWorksWithDoubleNumbers()
        {
            double firstDouble = 23.7D;
            double secondButIdenticalDouble = 23.75D;

            Check.That(secondButIdenticalDouble).IsNotEqualTo(firstDouble);
        }

        [Test]
        public void IsNotEqualWorksWithFloatNumbers()
        {
            float firstFloat = 23.56F;
            float secondButIdenticalFloat = 23.99999F;

            Check.That(secondButIdenticalFloat).IsNotEqualTo(firstFloat);
        }

        [Test]
        public void IsNotEqualToThrowsExceptionWithClearStatusWhenFails()
        {
            var first = "Son of a test";
            var otherReferenceToSameObject = first;

            Check.ThatCode(() =>
            {
                Check.That(first).IsNotEqualTo(otherReferenceToSameObject);
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked string is equal to the expected one whereas it must not." + Environment.NewLine + "The expected string: different from" + Environment.NewLine + "\t[\"Son of a test\"]");
        }

        [Test]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            var first = "Son of a test";

            Check.ThatCode(() =>
            {
                Check.That(first).Not.IsNotEqualTo("what?");
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked string is different from expected one." + Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"Son of a test\"]" + Environment.NewLine + "The expected string:" + Environment.NewLine + "\t[\"what?\"]");
        }

        #endregion

        #region Equals should always throw

        [Test]
        public void EqualsWorksAsAnAssertion()
        {
            var obj = new object();

            Check.That(obj).Equals(obj);
        }

        [Test]
        public void NotEqualsWorksToo()
        {
            object obj = new object();
            object other = new object();

            Check.That(obj).Not.Equals(other);
        }

        [Test]
        public void EqualsThrowsExceptionWhenFailing()
        {
            string question = "What is the question?";
            int magicNumber = 42;

            Check.ThatCode(() =>
            {
                Check.That(question).Equals(magicNumber);
            })
            .IsAFaillingCheckWithMessage(Environment.NewLine+ "The checked string is different from the expected value." + Environment.NewLine + "The checked string:" + Environment.NewLine + "\t[\"What is the question?\"] of type: [string]" + Environment.NewLine + "The expected value:" + Environment.NewLine + "\t[42] of type: [int]");
        }

        #endregion

        [Test]
        public void IsSameTypeWorks()
        {
 //           Check.That((IEnumerable<object>)new []{new object()}).IsInstanceOf<IEnumerable<object>>();
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionsForDoubleNumber()
        {
            double doubleNumber = 37.2D;
            
            Check.That(doubleNumber).IsEqualTo(37.2D).And.IsNotEqualTo(40.0D).And.IsNotZero().And.IsStrictlyPositive();
            Check.That(doubleNumber).IsNotEqualTo(40.0D).And.IsEqualTo(37.2D);
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionsForObject()
        {
            var camus = new Person { Name = "Camus" };
            var sartre = new Person { Name = "Sartre" };

            Check.That(camus).IsNotEqualTo(sartre).And.IsInstanceOf<Person>();
            Check.That(sartre).IsEqualTo(sartre).And.IsInstanceOf<Person>();
        }

        [Test]
        public void AndOperatorCanChainMultipleAssertionsForString()
        {
            var camus = "Camus";
            var sartre = "Sartre";

            Check.That(camus).IsNotEqualTo(sartre).And.IsInstanceOf<string>();
            Check.That(sartre).IsEqualTo(sartre).And.IsInstanceOf<string>();
        }
    }
}