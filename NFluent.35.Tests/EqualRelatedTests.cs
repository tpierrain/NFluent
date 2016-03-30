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
    using NUnit.Framework;

    [TestFixture]
    public class EqualRelatedTests
    {
        #region IsEqualTo()

        [Test]
        public void IsEqualToWorksWithBooleans()
        {
            const bool TddSucks = false;
            Check.That(TddSucks).IsNotEqualTo(true);
        }

        [Test]
        public void CanNegateIsEqualToWithBooleans()
        {
            const bool TddSucks = false;
            Check.That(TddSucks).Not.IsNotEqualTo(false);
        }

        [Test]
        public void IsEqualToWorksWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo("Son of a test");
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[42] of type: [int]\nThe expected value:\n\t[42] of type: [long]")]
        public void IsEqualToThrowsWhenSameNumberOfDifferentTypes()
        {
            const int IntValue = 42;
            const long LongValue = 42L;

            Check.That(IntValue).IsEqualTo(LongValue);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[Gandhi] of type: [NFluent.Tests.Person]")]
        public void NotIsEqualToWithObjectThrowsExceptionWhenFailing()
        {
            var heroe = new Person { Name = "Gandhi" };
            var otherReference = heroe;

            Check.That(heroe).Not.IsEqualTo(otherReference);
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
        [ExpectedException(typeof(FluentCheckException))]
        public void IsEqualToThrowsExceptionWhenFailingWithIntArray()
        {
            var array = new[] { 45, 43, 54, 666 };
            var otherSimilarButNotEqualArray = new[] { 45, 43, 54, 666 };

            Check.That(array).IsEqualTo(otherSimilarButNotEqualArray);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from expected one.\nThe checked string:\n\t[\"Son of a test\"]\nThe expected string:\n\t[\"no way\"]")]
        public void IsEqualToThrowsExceptionWhenFailingWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo("no way");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is equal to the expected one whereas it must not.\nThe expected string: different from\n\t[\"Son of a test\"]")]
        public void NotIsEqualToThrowsExceptionWhenFailingWithString()
        {
            var first = "Son of a test";
            Check.That(first).Not.IsEqualTo(first);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[Gandhi]\nThe expected value:\n\t[PolPot]")]
        public void IsEqualToThrowsExceptionWhenFailingWithObject()
        {
            var heroe = new Person { Name = "Gandhi" };
            var bastard = new Person { Name = "PolPot" };

            Check.That(heroe).IsEqualTo(bastard);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from expected one.\nThe checked value:\n\t[\"Son of a test\"]\nThe expected value:\n\t[null]")]
        public void IsEqualToThrowsProperExceptionEvenWithNullAsExpected()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo(null);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from expected one.\nThe checked string:\n\t[null]\nThe expected string:\n\t[\"Kamoulox !\"]")]
        public void IsEqualToThrowsProperExceptionEvenWithNullAsValue()
        {
            string first = null;
            Check.That(first).IsEqualTo("Kamoulox !");
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\t[John] of type: [NFluent.Tests.Child]\nThe expected value:\n\t[John] of type: [NFluent.Tests.Person]")]
        public void WeCanSeeTheDifferenceBewteenTwoDifferentObjectsThatHaveTheSameToString()
        {
            Person dad = new Person { Name = "John" };
            Person son = new Child { Name = "John" };
            Check.That(son).IsEqualTo(dad);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), MatchType = MessageMatch.Regex, ExpectedMessage = "\nThe checked value is different from the expected one.\nThe checked value:\n\\t\\[John\\] with HashCode: \\[.*\\]\nThe expected value:\n\\t\\[John\\] with HashCode: \\[.*\\]")]
        public void WeCanAlsoSeeTheDifferenceBetweenTwoDifferentInstancesOfTheSameTypeWhithIdenticalToString()
        {
            Person dad = new Person { Name = "John" };
            Person uncle = new Person { Name = "John" };
            
            Check.That(uncle).IsEqualTo(dad);
        }

        #endregion

        #region IsNotEqualTo()

        [Test]
        public void IsNotEqualToWorksWithBooleans()
        {
            const bool TddSucks = false;
            Check.That(TddSucks).IsEqualTo(false);
        }

        [Test]
        public void CanNegateIsNotEqualToWithBooleans()
        {
            const bool TddSucks = false;
            Check.That(TddSucks).Not.IsEqualTo(true);
        }

        [Test]
        public void IsNotEqualToWorksWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsNotEqualTo("other text");
        }

        [Test]
        public void IsNotEqualToWorksWithArray()
        {
            var array = new[] { 45, 43, 54, 666 };
            var otherArray = new[] { 666, 74 };
            var similarButNotEqualArray = new[] { 45, 43, 54, 666 };

            Check.That(array).IsNotEqualTo(otherArray);
            Check.That(array).IsNotEqualTo(similarButNotEqualArray);
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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is equal to the expected one whereas it must not.\nThe expected string: different from\n\t[\"Son of a test\"]")]
        public void IsNotEqualToThrowsExceptionWithClearStatusWhenFails()
        {
            var first = "Son of a test";
            var otherReferenceToSameObject = first;
            Check.That(first).IsNotEqualTo(otherReferenceToSameObject);
        }

        [Test]
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from expected one.\nThe checked string:\n\t[\"Son of a test\"]\nThe expected string:\n\t[\"what?\"]")]
        public void NotIsNotEqualToThrowsExceptionWhenFailing()
        {
            var first = "Son of a test";
            Check.That(first).Not.IsNotEqualTo("what?");
        }

        #endregion

        #region Equals should always throw

        [Test]
        public void EqualsWorksAsAnAssertion()
        {
            object obj = new object();

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
        [ExpectedException(typeof(FluentCheckException), ExpectedMessage = "\nThe checked string is different from the expected value.\nThe checked string:\n\t[\"What is the question?\"] of type: [string]\nThe expected value:\n\t[42] of type: [int]")]
        public void EqualsThrowsExceptionWhenFailing()
        {
            string question = "What is the question?";
            int magicNumber = 42;

            Check.That(question).Equals(magicNumber);
        }

        #endregion

        [Test]
        public void AndOperatorCanChainMultipleAssertionsForDoubleNumber()
        {
            double doubleNumber = 37.2D;
            
            Check.That(doubleNumber).IsEqualTo(37.2D).And.IsNotEqualTo(40.0D).And.IsNotZero().And.IsPositive();
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