using NFluent;
using NUnit.Framework;

namespace NuggetVersion
{
    public class UserReported
    {
        [Test]
        // issue #176
        public void NegatedIsZero()
        {
            Check.That(1).Not.IsZero();
            Check.That(1).IsNotZero();
            Check.That(0).Not.IsNotZero();
            Check.That(0).IsZero();
        }
    }
}
