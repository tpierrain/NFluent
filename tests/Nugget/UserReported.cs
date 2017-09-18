using System;
using System.Threading.Tasks;
using NUnit.Framework.Internal;

namespace Nugget
{
    using NFluent;
    using NUnit.Framework;

    public class UserReported
    {
        // issue #176
        [Test]
        public void NegatedIsZero()
        {
            Check.That(1).Not.IsZero();
            Check.That(1).IsNotZero();
            Check.That(0).Not.IsNotZero();
            Check.That(0).IsZero();
        }
    }
}
