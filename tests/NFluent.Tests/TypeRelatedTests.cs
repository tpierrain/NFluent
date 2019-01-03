namespace NFluent.Tests
{
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    [Category("Type")]
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

        [Test]
        public void ShouldFailWhenAttributesOnParent()
        {
            Check.ThatCode(() => { Check.That<Child>().HasAttribute<TestFixtureAttribute>(); })
                .IsAFaillingCheckWithMessage(
                    string.Empty,
                    "The checked value does not have an attribute of the expected type.",
                    "The checked value:",
                    "\t[NFluent.Tests.TypeRelatedTests+Child]");

        }

        private class Child: TypeRelatedTests
        {}
    }
}
