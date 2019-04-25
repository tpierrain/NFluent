namespace NFluent.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class AssumeShould
    {
        [Test]
        public void ProvideIgnoreForObjectCheck()
        {
            Assuming.That(12).IsEqualTo(12);
            Check.ThatCode(() => Assuming.That(12).IsEqualTo(13)).Throws<IgnoreException>();
        }

        [Test]
        public void ProvideIgnoreForTypeCheck()
        {
            Assuming.That<int>().IsEqualTo(typeof(int));
            Check.ThatCode(() => Assuming.That<int>().IsInstanceOf<int>()).Throws<IgnoreException>();
        }

        [Test]
        public void ProvideIgnoreForStructCheck()
        {
            Assuming.ThatEnum(4).IsEqualTo(4);
            Check.ThatCode(() => Assuming.ThatEnum(4).IsEqualTo(5)).Throws<IgnoreException>();
        }
    }
}
