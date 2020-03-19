namespace NFluent.Tests.Customization
{
    using Extensibility;
    using NUnit.Framework;

    [TestFixture]
    public class ErrorReportingShould
    {
        [Test]
        public void CustomizeErrorReporting()
        {
            var reporter = new StringReporting();
            var oldReporter = Check.Reporter;
            Check.Reporter = reporter;
            try
            {
                Check.That(12).IsNotEqualTo(12);

            }
            finally
            {
                Check.Reporter = oldReporter;
            }

            Check.That(reporter.Error).AsLines().ContainsExactly("",
                "The checked value is equal to the given one whereas it must not.",
                "The expected value: different from",
                "\t[12] of type: [int]");
        }

        private class StringReporting : IErrorReporter
        {
            public string Error { get; private set; }
            public void ReportError(string message)
            {
                this.Error = message;
            }
        }



    }
}
