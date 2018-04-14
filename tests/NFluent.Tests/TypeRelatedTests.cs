namespace NFluent.Tests
{
    using NFluent.ApiChecks;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    class TypeRelatedTests
    {

        [Test]
        public void ShouldCheckForAttributes()
        {
            Check.That<TypeRelatedTests>().HasAttribute<TestFixtureAttribute>();
        }

        [Test]
        public void ShouldFailWhenAttributeNotFound()
        {
            Check.ThatCode(() => { Check.That<TypeRelatedTests>().HasAttribute<OneTimeSetUpAttribute>(); })
                .IsAFaillingCheckWithMessage(
                string.Empty,
                "The checked value does not have an attribute of the expected type.",
                "The checked value:",
                "\t[NFluent.Tests.TypeRelatedTests]");
        }
    }
}
