namespace NFluent.Tests
{
    using ApiChecks;
    using NFluent.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class AssumeShould
    {
        [Test]
        public void ProvideIgnoreForObjectCheck()
        {
            Assuming.That(12).IsEqualTo(12);
            Check.ThatCode(() => Assuming.That(12).IsEqualTo(13)).IsAFailingAssumption();
        }

        [Test]
        public void ProvideIgnoreWithCustomMessage()
        {
            Check.ThatCode(() => Assuming.WithCustomMessage("it works").That(12).IsEqualTo(13)).
                IsAFailingAssumptionWithMessage("it works", 
                    "The checked value is different from the expected one.", 
                    "The checked value:", 
                    "\t[12]", 
                    "The expected value:", 
                    "\t[13]");
        }

        [Test]
        public void ProvideIgnoreForTypeCheck()
        {
            Assuming.That<int>().IsEqualTo(typeof(int));
            Check.ThatCode(() => Assuming.That<int>().IsInstanceOf<int>()).IsAFailingAssumption();
        }

        [Test]
        public void ProvideIgnoreForStructCheck()
        {
            Assuming.ThatEnum(4).IsEqualTo(4);
            Check.ThatCode(() => Assuming.ThatEnum(4).IsEqualTo(5)).IsAFailingAssumption();
        }

        [Test]
        public void ProvideIgnoreForCode()
        {
            Assuming.ThatCode(() => 2).DoesNotThrow();
            Assuming.ThatCode(() => {}).DoesNotThrow();
            Check.ThatCode(() => Assuming.ThatCode(() =>2).ThrowsAny()).IsAFailingAssumption();
        }

#if !DOTNET_20 && !DOTNET_30 && !DOTNET_40 && !DOTNET_35
        [Test]
        public void ProvideIgnoreForDynamic()
        {
            dynamic test = 2;
            Assuming.ThatDynamic(test).IsNotNull();
            test = null;
            Check.ThatCode(() => Assuming.ThatDynamic(test).IsNotNull()).IsAFailingAssumption();
        }
#endif
    }
}
