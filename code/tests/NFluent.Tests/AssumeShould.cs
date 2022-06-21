namespace NFluent.Tests
{
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
            Assuming.That(4).IsEqualTo(4);
            Check.ThatCode(() => Assuming.That(4).IsEqualTo(5)).IsAFailingAssumption();
        }

        [Test]
        public void ProvideIgnoreForCode()
        {
            Assuming.ThatCode(() => 2).DoesNotThrow();
            Assuming.ThatCode(() => {}).DoesNotThrow();
            Check.ThatCode(() => Assuming.ThatCode(() =>2).ThrowsAny()).IsAFailingAssumption();
        }
        
#if !NETCOREAPP1_1
// this test is not relevant if the framework does not offer assembly reflection methods
        [Test]
        public void AssumptionCheckShouldFailWithProperErrorMessage()
        {
            Check.ThatCode(() =>
            Check.ThatCode(() => Check.ThatCode(() =>2).ThrowsAny()).IsAFailingAssumption()).
            IsAFailingCheckWithMessage("", 
                "The exception raised is not of the expected type", 
                "The checked fluent assumption's raised exception:", 
                "\t[{NUnit.Framework.AssertionException}: '", 
                "The checked code did not raise an exception, whereas it must.'] of type: [NUnit.Framework.AssertionException]", 
                "The expected fluent assumption's raised exception:", 
                "\tan instance of [NUnit.Framework.InconclusiveException]");
            Check.ThatCode(() =>
                    Check.ThatCode(() => Check.ThatCode(() =>2).DoesNotThrow()).IsAFailingAssumption()).
                IsAFailingCheckWithMessage("", 
                    "The assumption succeeded whereas it should have failed.", 
                    "The expected fluent assumption's raised exception:", 
                    "\tan instance of [NUnit.Framework.InconclusiveException]");
            Check.ThatCode(() =>
                    Check.ThatCode(() => Check.ThatCode(() =>2).DoesNotThrow()).IsAFailingAssumptionWithMessage("don't care")).
                IsAFailingCheckWithMessage("", 
                    "The assumption succeeded whereas it should have failed.", 
                    "The expected fluent assumption's raised exception:", 
                    "\tan instance of [NUnit.Framework.InconclusiveException]");
        }
#endif
#if!DOTNET_35
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
