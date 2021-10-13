using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFluent.Tests
{
    using NFluent.Helpers;

    [TestFixture]
    public class BatchChecksShould
    {
        [Test]
        public void 
            WorkForSingleCheck()
        {
            var sut = Check.StartBatch();

            Check.That(2).IsEqualTo(3);

            Check.ThatCode(() => sut.Dispose()).IsAFailingCheckWithMessage("",
            "The checked value is different from the expected one.",
            "The checked value:",
            "\t[2]",
            "The expected value:",
            "\t[3]");
        }

        [Test]
        public void 
            WorkForTwoChecks()
        {
            var sut = Check.StartBatch();

            Check.That(2).IsPositiveOrZero();
            Check.That(2).IsEqualTo(3);

            Check.ThatCode(() => sut.Dispose()).IsAFailingCheckWithMessage("",
                "The checked value is different from the expected one.",
                "The checked value:",
                "\t[2]",
                "The expected value:",
                "\t[3]");

        }
        
        [Test]
        public void 
            WorkForTwoFailingChecks()
        {
            var sut = Check.StartBatch();

            Check.That(2).IsNegativeOrZero();
            Check.That(2).IsEqualTo(3);

            Check.ThatCode(() => sut.Dispose()).IsAFailingCheckWithMessage("",
                "The checked value is not negative or equal to zero.",
                "The checked value:",
                "\t[2]",
                "** And **",
                "The checked value is different from the expected one.",
                "The checked value:",
                "\t[2]",
                "The expected value:",
                "\t[3]");

        }        
        
        [Test]
        public void 
            WorkWhenExceptionRaised()
        {
            Check.ThatCode(() =>
            {
                using (Check.StartBatch())
                {
                    Check.That(2).IsNegativeOrZero();
                    throw new ApplicationException("Random exception");
                }
            }).IsAFailingCheckWithMessage("",
                "The checked value is not negative or equal to zero.",
                "The checked value:",
                "\t[2]");

        }
    }
}
