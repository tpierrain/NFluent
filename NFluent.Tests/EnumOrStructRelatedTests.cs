namespace NFluent.Tests
{
    using NFluent.Tests.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class EnumOrStructRelatedTests
    {
        [Test]
        public void IsEqualToWorksWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnumOrStruct(FrenchNationality).IsEqualTo(Nationality.French);
        }

        [Test]
        public void IsNotEqualToWorksWithEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnumOrStruct(FrenchNationality).IsNotEqualTo(Nationality.American);
        }

        [Test]
        public void NotOperatorWorksOnIsEqualToForEnum()
        {
            const Nationality FrenchNationality = Nationality.French;
            Check.ThatEnumOrStruct(FrenchNationality).Not.IsEqualTo(Nationality.American);
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
