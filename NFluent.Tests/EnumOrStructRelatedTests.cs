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
        public void IsEqualToWorksWithStruct()
        {
            var inLove = new Mood { Description = "In love", IsPositive = true };
            Check.ThatEnumOrStruct(inLove).IsEqualTo(inLove);
        }
    }
}
