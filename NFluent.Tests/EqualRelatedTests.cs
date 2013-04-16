namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class EqualRelatedTests
    {
        private const string Blabla = ".*?";
        private const string LineFeed = "\\n";
        private const string NumericalHashCodeWithinBrackets = "(\\[(\\d+)\\])";

        #region IsEqualTo()

        [Test]
        public void IsEqualToWorksWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo("Son of a test");
        }

        [Test]
        public void IsEqualToWorksWithArray()
        {
            var array = new int[] { 45, 43, 54, 666 };
            var otherReference = array;

            Check.That(array).IsEqualTo(array);
            Check.That(array).IsEqualTo(otherReference);
        }

        [Test]
        public void IsEqualToWorksWithObject()
        {
            var heroe = new Person() { Name = "Gandhi" };
            var otherReference = heroe;

            Check.That(heroe).IsEqualTo(otherReference);
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
        [ExpectedException(typeof(FluentAssertionException))]
        public void IsEqualToThrowsExceptionWhenFails()
        {
            var array = new int[] { 45, 43, 54, 666 };
            var otherSimilarButNotEqualArray = new int[] { 45, 43, 54, 666 };

            Check.That(array).IsEqualTo(otherSimilarButNotEqualArray);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[\"Son of a test\"]\nis not equal to the expected one, i.e.:\n\t[\"no way\"].")]
        public void IsEqualToThrowsExceptionWithClearStatusWhenFails()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo("no way");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[\"Son of a test\"] of type: [System.String]\nis not equal to the expected one, i.e.:\n\t[null].")]
        public void IsEqualToThrowsProperExceptionEvenWithNullAsExpected()
        {
            var first = "Son of a test";
            Check.That(first).IsEqualTo(null);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[null]\nis not equal to the expected one, i.e.:\n\t[\"Kamoulox !\"] of type: [System.String].")]
        public void IsEqualToThrowsProperExceptionEvenWithNullAsValue()
        {
            string first = null;
            Check.That(first).IsEqualTo("Kamoulox !");
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[John] of type: [NFluent.Tests.Children]\nis not equal to the expected one, i.e.:\n\t[John] of type: [NFluent.Tests.Person].")]
        public void WeCanSeeTheDifferenceBewteenTwoDifferentObjectsThatHaveTheSameToString()
        {
            Person dad = new Person() { Name = "John" };
            Person son = new Children() { Name = "John" };
            Check.That(son).IsEqualTo(dad);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), MatchType = MessageMatch.Regex, ExpectedMessage = Blabla + "(\\[NFluent\\.Tests\\.Person\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + LineFeed + Blabla + "(\\[NFluent\\.Tests\\.Person\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + ".")]
        public void WeCanAlsoSeeTheDifferenceBetweenTwoDifferentInstancesOfTheSameTypeWhichHaveSameToString()
        {
            // e.g.: "\nExpecting:\n\t[John] of type: [NFluent.Tests.Person] with HashCode: [45523402]\n but was\n\t[John] of type: [NFluent.Tests.Person] with HashCode: [35287174]."
            Person dad = new Person() { Name = "John" };
            Person uncle = new Person() { Name = "John" };
            Check.That(uncle).IsEqualTo(dad);
        }

        #endregion

        #region IsNotEqualTo()

        [Test]
        public void IsNotEqualToWorksWithString()
        {
            var first = "Son of a test";
            Check.That(first).IsNotEqualTo("other text");
        }

        [Test]
        public void IsNotEqualToWorksWithArray()
        {
            var array = new int[] { 45, 43, 54, 666 };
            var otherArray = new int[] { 666, 74 };
            var similarButNotEqualArray = new int[] { 45, 43, 54, 666 };

            Check.That(array).IsNotEqualTo(otherArray);
            Check.That(array).IsNotEqualTo(similarButNotEqualArray);
        }

        [Test]
        public void IsNotEqualToWorksObject()
        {
            var heroe = new Person() { Name = "Gandhi" };
            var badGuy = new Person() { Name = "Pol Pot" };

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
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[\"Son of a test\"] of type: [System.String].")]
        public void IsNotEqualToThrowsExceptionWithClearStatusWhenFails()
        {
            var first = "Son of a test";
            var otherReferenceToSameObject = first;
            Check.That(first).IsNotEqualTo(otherReferenceToSameObject);
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
            var camus = new Person() { Name = "Camus" };
            var sartre = new Person() { Name = "Sartre" };

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