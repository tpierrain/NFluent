namespace NFluent.Tests
{
    using NFluent.Tests.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class EnumOrStructRelatedTests
    {
        private const string Blabla = ".*?";
        private const string LineFeed = "\\n";
        private const string NumericalHashCodeWithinBrackets = "(\\[(\\d+)\\])";

        [Test]
        public void IsEqualToWorksWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnumOrStruct(FrenchNationality).IsEqualTo(Nationality.French);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value:\n\t[French]\nis not equal to the expected one:\n\t[American].")]
        public void IsEqualToThrowsExceptionWhenFailingWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnumOrStruct(FrenchNationality).IsEqualTo(Nationality.American);
        }

        [Test]
        public void IsNotEqualToWorksWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnumOrStruct(FrenchNationality).IsNotEqualTo(Nationality.American);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "\nThe actual value is unexpectedly equal to the given one, i.e.:\n\t[French] of type: [NFluent.Tests.Nationality].")]
        public void IsNotEqualToThrowsExceptionWhenFailingWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnumOrStruct(FrenchNationality).IsNotEqualTo(Nationality.French);
        }

        [Test]
        public void NotOperatorWorksOnIsEqualToForEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnumOrStruct(FrenchNationality).Not.IsEqualTo(Nationality.American);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), MatchType = MessageMatch.Regex, ExpectedMessage = Blabla + "(\\[French\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + LineFeed + Blabla + LineFeed + Blabla + "(\\[French\\])" + Blabla + "(with)" + Blabla + "(HashCode)" + Blabla + NumericalHashCodeWithinBrackets + Blabla + LineFeed + "(which)" + Blabla)]
        public void NotIsEqualToThrowsExceptionWhenFailingWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnumOrStruct(FrenchNationality).Not.IsEqualTo(Nationality.French);
        }

        [Test]
        public void NotOperatorWorksOnIsNotEqualToForEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnumOrStruct(FrenchNationality).Not.IsNotEqualTo(Nationality.French);
        }

        // TODO: write tests related to error message of IsNotEqualTo (cause the error message seems not so good)
        [Test]
        public void IsEqualToWorksWithStruct()
        {
            var inLove = new Mood { Description = "In love", IsPositive = true };
            Check.ThatEnumOrStruct(inLove).IsEqualTo(inLove);
        }
    }
}
