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
            Check.ThatEnum(FrenchNationality).IsEqualTo(Nationality.French);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is different from the expected one.\nThe checked value:\n\t[French]\nThe expected value:\n\t[American]")]
        public void IsEqualToThrowsExceptionWhenFailingWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).IsEqualTo(Nationality.American);
        }

        [Test]
        public void IsNotEqualToWorksWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).IsNotEqualTo(Nationality.Korean);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[French] of type: [NFluent.Tests.Nationality]")]
        public void IsNotEqualToThrowsExceptionWhenFailingWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).IsNotEqualTo(Nationality.French);
        }

        [Test]
        public void NotOperatorWorksOnIsEqualToForEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).Not.IsEqualTo(Nationality.American);
        }

        [Test]
        [ExpectedException(typeof(FluentAssertionException), ExpectedMessage = "The checked value is equal to the expected one whereas it must not.\nThe expected value: different from\n\t[French] of type: [NFluent.Tests.Nationality]")]
        public void NotIsEqualToThrowsExceptionWhenFailingWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).Not.IsEqualTo(Nationality.French);
        }

        [Test]
        public void NotOperatorWorksOnIsNotEqualToForEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnum(FrenchNationality).Not.IsNotEqualTo(Nationality.French);
        }

        // TODO: write tests related to error message of IsNotEqualTo (cause the error message seems not so good)
        [Test]
        public void IsEqualToWorksWithStruct()
        {
            var inLove = new Mood { Description = "In love", IsPositive = true };
            Check.ThatEnum(inLove).IsEqualTo(inLove);
        }
    }
}
